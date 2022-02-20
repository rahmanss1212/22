using Events.Api.Models.UserManagement;
using Events.Core.Models.General;
using Events.Core.Models.UserManagement;
using Events.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Events.Api;
using Microsoft.Extensions.Options;

namespace UserManagment
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<EUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<ERole> roleManager;
        private readonly AppDbContext _ctx;
        private IHttpContextAccessor _httpContextAccessor;
        private SysConfiguration _sysConfiguration;

        public AuthenticateController(UserManager<EUser> userManager,
            IConfiguration configuration, RoleManager<ERole> rm,IOptions<SysConfiguration> config,
            AppDbContext dbContext,IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            this._userManager = userManager;
            _sysConfiguration = config.Value;
            _configuration = configuration;
            _ctx = dbContext;
            roleManager = rm;
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            try
            {
                var userExists = await _userManager.FindByNameAsync(model.Username);

                if (userExists != null)
                {
                    return Ok(FailedResponse.Build("اسم المستخدم موجود مسبقا"));
                }
                
                var user = new EUser { SectionId = model.sectionId,FullName = model.Name, UserName = model.Username, Email = model.Email };
                
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    ERole eRole = _ctx.Roles.Where(r => r.Name == model.RoleId).FirstOrDefault();
                    EUser eUser = _ctx.Users.Where(u => u.UserName == model.Username).FirstOrDefault();
                    _ctx.UserRoles.Add(new IdentityUserRole<long> { UserId = eUser.Id, RoleId = eRole.Id });
                    _ctx.SaveChanges();
                    return Ok(SuccessResponse<EUser>.build(eUser, 0, null));
                } 
                return Ok(FailedResponse.Build((result.Errors.ToList()[0].Description).ToString()));
            }
            catch (Exception e) {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {

            try
            {
                
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    var userRole = await _userManager.GetRolesAsync(user);

                    if (userRole.Count == 0) { 
                     return Ok(FailedResponse.Build("لا يوجد صلاحيات للمستخدم"));
                    }

                    if (!user.IsEnabled)
                    {
                        return Ok(FailedResponse.Build("تم تعطيل المستخدم"));
                    }

                    string role = userRole.FirstOrDefault();
                    ERole eRole = _ctx.Roles.Where(r => r.Name == role).SingleOrDefault();

                    var menue = _ctx.Employees
                   .Where(u => u.UserName == model.Username).Include(x => x.Section).ThenInclude(x => x.Department).SingleOrDefault();
                    var claims = _ctx.RoleClaims.Where(rc => rc.RoleId == eRole.Id).ToList()
                    .GroupBy(x => x.ClaimType)
                    .ToDictionary(g => g.Key, y => y.Select(c => c.ClaimValue).ToList())
                    .Select(cg => new Claims { type = cg.Key, values = cg.Value }).ToList();
                    

                    var secret = _configuration["JWT:Secret"];
                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

                    var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddYears(10),
                        claims: new[] { new Claim(ClaimTypes.Name, model.Username) , new Claim(ClaimTypes.Role , eRole.Id.ToString()) },
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                        );;
                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo,
                        claims,
                        data = menue,
                        status = 200
                    });
                }
                return Ok(FailedResponse.Build("كلمة المرور/اسم المستخدم خاطئة"));
            }
            catch (Exception e) {
                return Ok(FailedResponse.Build(e.Message));
            }
            
    
        }
        
        
        
        [HttpPost]
        [Route("loginWithWindowasUser")]
        public async Task<IActionResult> loginWithWindowasUser()
        {

            try
            {
                // string username = "l";
                //
                // if (_sysConfiguration.MethodName == SysConfiguration.Environment)
                // {
                //     username = Environment.UserName;
                // }
                //
                // if (_sysConfiguration.MethodName == SysConfiguration.WindowsIdentity)
                // {
                //   username =  WindowsIdentity.GetCurrent().Name;
                // }
                //
                // if (_sysConfiguration.MethodName == SysConfiguration.Http)
                // {
                //     username = _httpContextAccessor.HttpContext.User.Identity.Name;
                // }
                
                /*var username = _sysConfiguration.MethodName == SysConfiguration.Environment ? 
                    Environment.UserName : 
                    _sysConfiguration.MethodName == SysConfiguration.WindowsIdentity ?
                        WindowsIdentity.GetCurrent().Name : 
                        _httpContextAccessor.HttpContext.User.Identity.Name;*/

                string username = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

                var split = username.Split('\\');
                username = split.Length == 2 ? split[1] : split[0];

                //
                username = string.Join("0",username.Split(' '));
                
                var user = await _userManager.FindByNameAsync(username);
                if (user != null)
                {
                    var userRole = await _userManager.GetRolesAsync(user);

                    if (userRole.Count == 0) { 
                     return Ok(FailedResponse.Build("لا يوجد صلاحيات للمستخدم"));
                    }

                    if (!user.IsEnabled)
                    {
                        return Ok(FailedResponse.Build("تم تعطيل المستخدم"));
                    }

                    string role = userRole.FirstOrDefault();
                    ERole eRole = _ctx.Roles.Where(r => r.Name == role).SingleOrDefault();

                    var menue = _ctx.Employees
                   .Where(u => u.UserName == username).Include(x => x.Section).ThenInclude(x => x.Department).SingleOrDefault();
                    var claims = _ctx.RoleClaims.Where(rc => rc.RoleId == eRole.Id).ToList()
                    .GroupBy(x => x.ClaimType)
                    .ToDictionary(g => g.Key, y => y.Select(c => c.ClaimValue).ToList())
                    .Select(cg => new Claims { type = cg.Key, values = cg.Value }).ToList();



                    var secret = _configuration["JWT:Secret"];
                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

                    var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddYears(10),
                        claims: new[] { new Claim(ClaimTypes.Name, username) , new Claim(ClaimTypes.Role , eRole.Id.ToString()) },
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                        );;
                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo,
                        claims,
                        data = menue,
                        status = 200
                    });
                }
                return Ok(FailedResponse.Build("المستخدم" + username + "غير مصرح لدخول النظام"));
            }
            catch (Exception e) {
                return Ok(FailedResponse.Build(e.Message));
            }
            
    
        }

        [HttpPut("updatePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordResource changePasswordResource)
        {
            try
            {
                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user = _ctx.Users.Where(x => x.UserName == username).SingleOrDefault();
                if (user == null)
                {
                    return Ok(FailedResponse.Build("اسم المستخدم غير موجود"));
                }



                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, changePasswordResource.NewPassword);
                _ctx.Entry(user).CurrentValues.SetValues(user);
                var result = await _ctx.SaveChangesAsync();

                if (result == 0)
                {
                    return Ok(FailedResponse.Build("خطأ أثناء تعديل كملة المرور"));
                }
                return Ok(SuccessResponse<EUser>.build(user, 0, null));
            }
            catch (Exception e) {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpPut("updatePasswordByAdmin")]
        public async Task<IActionResult> ChangePasswordByAdmin([FromBody] ChangePasswordResource changePasswordResource)
        {
            try
            {
               
                var user = _ctx.Users.Where(x => x.UserName == changePasswordResource.username).SingleOrDefault();
                if (user == null)
                {
                    return Ok(FailedResponse.Build("اسم المستخدم غير موجود"));
                }

                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, changePasswordResource.NewPassword);
                _ctx.Entry(user).CurrentValues.SetValues(user);
                var result = await _ctx.SaveChangesAsync();

                if (result == 0)
                {
                    return Ok(FailedResponse.Build("خطأ أثناء تعديل كملة المرور"));
                }
                return Ok(SuccessResponse<EUser>.build(user, 0, null));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpPut("disableUser")]
        public async Task<IActionResult> disableUser([FromBody] UserUpdateModel u)
        {
            try
            {

                var user = _ctx.Users.Find(u.id);
                if (user == null)
                {
                    return Ok(FailedResponse.Build("اسم المستخدم غير موجود"));
                }
                
                user.IsEnabled = u.isEnabled;
                
                _ctx.Entry(user).CurrentValues.SetValues(user);
                var result = await _ctx.SaveChangesAsync();

                return Ok(SuccessResponse<EUser>.build(user, 0, null));
                
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpPut("IsHeadOrNot")]
        public async Task<IActionResult> isHeadOrNot ([FromBody] UserUpdateModel u)
        {
            try
            {

                var user = _ctx.Users.Find(u.id);
                if (user == null)
                {
                    return Ok(FailedResponse.Build("اسم المستخدم غير موجود"));
                }
                
                user.IsEnabled = u.isHead;

                _ctx.Entry(user).CurrentValues.SetValues(user);
                var result = await _ctx.SaveChangesAsync();
                return Ok(SuccessResponse<EUser>.build(user, 0, null));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpPut("updateEmployee")]
        public async Task<IActionResult> updateEmployeeDetails([FromBody] UserUpdateModel userUpdate)
        {
            try
            {

                var user = _ctx.Users.Find(userUpdate.id);
                if (user == null)
                {
                    return Ok(FailedResponse.Build("اسم المستخدم غير موجود"));
                }
                user.UserName = userUpdate.Username;
                user.SectionId = userUpdate.sectionId;
                user.IsHead = userUpdate.isHead;
                user.FullName = userUpdate.Fullname;
                user.IsEnabled = userUpdate.isEnabled;
                user.IsHead = userUpdate.isHead;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded && userUpdate.RoleId != "")
                {
                    ERole eRole = _ctx.Roles.Where(r => r.Name == userUpdate.RoleId).FirstOrDefault();
                    
                    IdentityUserRole<long> identityUserRole = _ctx.UserRoles.Where(x => x.UserId == user.Id).SingleOrDefault();
                    if (identityUserRole != null && eRole.Id == identityUserRole.RoleId)
                        return Ok(SuccessResponse<EUser>.build(user, 0, null));
                    if (identityUserRole != null)
                        _ctx.UserRoles.Remove(identityUserRole);
                    _ctx.UserRoles.Add(new IdentityUserRole<long> { UserId = user.Id, RoleId = eRole.Id });
                    _ctx.SaveChanges();
                    return Ok(FailedResponse.Build(result.Errors.ToList()[0].Description));
                }
                return Ok(SuccessResponse<EUser>.build(user, 0, null));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

    }
}
