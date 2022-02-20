using Events.Api.Models.UserManagement;
using Events.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

namespace UserManagment.services
{
    public interface IUserService
    {
        EUser Authenticate(string username, string password);
        IEnumerable<EUser> GetAll();
        Task<EUser> GetByUsername(string username);

        EUser GetById(long username);
        Task<bool> DoesHavePermission(long Id,string claimType,string claimValue);

        EUser Create(EUser employee, string password);
        Task<List<EUser>> GetEmpsOfDepartment(long Id);
        Task<List<long>> GetUsersIdOfClaim(string claimType,string claimValue);
        List<EUser> GetHead(long user);

        void Update(EUser employee, string password = null);
        void Delete(int id);
    }

    //=======================================================================================================================
    public class UserService : IUserService
    {
        private readonly AppDbContext _ctx;
        private UserManager<EUser> _userManager;

        public UserService(AppDbContext context, UserManager<EUser> userManager)
        {
            _ctx = context;
            _userManager = userManager;
        }


        //=======================================================================================================================

        public EUser Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = _ctx.Employees.SingleOrDefault(x => x.UserName == username);

            // check if username exists 
            if (user == null)
                return null;


            return user;
        }

        //=======================================================================================================================

        public IEnumerable<EUser> GetAll()
        {
            return _ctx.Employees;
        }


        //=======================================================================================================================

        public async Task<EUser> GetByUsername(string username)
        {
            return await _ctx.Employees.Where(x => x.UserName == username)
                .Include(x => x.Section)
                .ThenInclude(x => x.Department)
                .FirstOrDefaultAsync();

        }



        //=======================================================================================================================

        public async Task<bool> DoesHavePermission(long Id,string claimType,string claimValue)
        {
            var role = await _ctx.UserRoles.SingleOrDefaultAsync(e => e.UserId == Id);
            var list = await _ctx.RoleClaims.Where(rc => rc.RoleId == role.RoleId).ToListAsync();
            return list.Any(c => c.ClaimType == claimType && c.ClaimValue == claimValue);
        }

        public EUser Create(EUser employee, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (_ctx.Employees.Any(x => x.UserName == employee.UserName))
                throw new AppException("Username " + employee.UserName + " is already taken");


            employee.PasswordHash = password;


            _ctx.Employees.Add(employee);
            _ctx.SaveChanges();

            return employee;
        }


        //=======================================================================================================================

        public void Update(EUser empParm, string password = null)
        {
            var employee = _ctx.Employees.Find(empParm.Id);
            if (employee == null)
                throw new AppException("User Not Found");

            // update username if it has changed
            if (!string.IsNullOrWhiteSpace(empParm.UserName) && empParm.UserName != employee.UserName)
            {
                // throw error if the new username is already taken
                if (_ctx.Employees.Any(x => x.UserName == empParm.UserName))
                    throw new AppException("Username " + empParm.UserName + " is already taken");
                employee.UserName = empParm.UserName;
            }
            // update user properties if provided

            if (!string.IsNullOrWhiteSpace(empParm.FullName))
                employee.FullName = empParm.FullName;

            if (!string.IsNullOrWhiteSpace(password))
            {
                employee.PasswordHash = password;
            }

            _ctx.Employees.Update(employee);
            _ctx.SaveChanges();
        }



        //=======================================================================================================================


        public void Delete(int id)
        {
            var employee = _ctx.Employees.Find(id);
            if (employee != null)
            {

                _ctx.Employees.Remove(employee);
                _ctx.SaveChanges();
            }

        }

        public async Task<List<long>> GetUsersIdOfClaim(string claimType, string claimValue)
        {
            var identityRoleClaims =await _ctx.RoleClaims.Where(x => x.ClaimValue == claimValue && x.ClaimType == claimType).ToListAsync();
            var roles = identityRoleClaims.Select(x => x.RoleId).ToList();
            var roleUsers = await _ctx.UserRoles.Where(x => roles.Contains(x.RoleId)).ToListAsync();
            return roleUsers.Select(x => x.UserId).ToList();
        }

        public List<EUser> GetHead(long userId)
        {
            var user = GetById(userId);
            return _ctx.Employees.Where(e => e.Section.Department.Id == user.Section.Department.Id && e.IsHead).ToList();
        }

        
        

        public async Task<List<EUser>> GetEmpsOfDepartment(long Id)
        => await _ctx.Users.Where(e => e.Section.Department.Id == Id).ToListAsync();

        EUser GetById(long username)
        {
            return _ctx.Employees.Where(x => x.Id == username)
            .Include(x => x.Section)
            .ThenInclude(x => x.Department)
            .FirstOrDefault();
        }

        EUser IUserService.GetById(long username)
        {
            return _ctx.Employees.Where(x => x.Id == username)
            .Include(x => x.Section)
            .ThenInclude(x => x.Department)
            .FirstOrDefault();
        }
    }
}

