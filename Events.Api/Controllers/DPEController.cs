using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Events.Api.Authorization;
using Events.Core.Models.DPE;
using Events.Core.Models.General;
using Events.Data;
using Events.Service;
using Events.Service.Service;
using Events.Service.Service.DataServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UserManagment.services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Events.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DPEController : ControllerBase
    {
        private readonly AppDbContext _ctx;
        private IUserService usersService;
        private DbServiceImpl<Exploit, ExploitView> expolitServiceImpl;
        private DbServiceImpl<Vulnerability, VulnerabilityView> vulnerabilityServiceImpl;
        private DbServiceImpl<DPE, DPEview> dpeExploitService;
        private readonly IChangeLogHelper ChangeLogHelper;
        private readonly INotificationHelper notificationHelper;
        private readonly SysConfiguration configuration;
        private readonly string WAITING_COR_PROC = "انتظار إجراء قسم التنسيق";
        private string IGNORE_EXPO = "تم تجاهل الثغرة";
        private const string INFORMING_DONE = "تم إعلام المؤسسة, إنتظار الإصلاح";
        private const string STATUS = "الحالة";
        private const string UNDER_PROC = "قيد الإنشاء";

        public DPEController(AppDbContext ctx, IUserService us, IServiceFactory factory,
            IOptions<SysConfiguration> conf)
        {
            _ctx = ctx;
            expolitServiceImpl = factory.ServicOf<Exploit, ExploitView>();
            ChangeLogHelper = factory.ChangeLogHelper();
            notificationHelper = factory.NotificationHelper();
            configuration = conf.Value;
            dpeExploitService = factory.ServicOf<DPE, DPEview>();
            vulnerabilityServiceImpl = factory.ServicOf<Vulnerability, VulnerabilityView>();
            usersService = us;
        }

        // GET: api/<DPEController>
        [HttpGet("GetDPE")]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            try
            {
                List<long> status = new List<long>()
                {
                    VULNERABILITY.WAITING_FIX,
                    VULNERABILITY.WAITING_ORG_INFORMING,
                    VULNERABILITY.WAITING_HEAD_DIRECTIONS,
                    VULNERABILITY.WAITING_FIX_AUTHENTICATION,
                    VULNERABILITY.WAITING_VARIFY
                };
                var items = await dpeExploitService.GetItems(dpe => dpe.VulnerStatus < VULNERABILITY.FIX_DONE
                                                                    && status.Any(x => x == dpe.StatusId)
                                                                    || dpe.StatusId == VULNERABILITY.WAITING_DOC);
                return Ok(items.Where(x => x.VulnerCount > 0 || x.StatusId == VULNERABILITY.WAITING_DOC).ToList());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpGet("GetDPEArchive")]
        [Authorize]
        public async Task<IActionResult> GetDPEArchive()
        {
            try
            {
                var items = await dpeExploitService.GetItems(dpe => dpe.VulnerStatus == VULNERABILITY.FIX_DONE);
                return Ok(items);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpGet("GetExploit")]
        [Authorize]
        public async Task<IActionResult> GetExploitView(int pageSize, int pageNumber)
        {
            try
            {
                PaggingModel page = new PaggingModel() { PageNumber = pageNumber, PageSize = pageSize };
                var exploitViews = await expolitServiceImpl.GetItems(x => true, page);
                long count = 0;
                if (pageNumber == 1)
                    count = await expolitServiceImpl.GetCount(x => true);
                var resp = SuccessResponse<ExploitView>.build(null, count, exploitViews);
                resp.count = count;
                return Ok(resp);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet("GetAffectedSource")]
        [Authorize]
        public async Task<IActionResult> GetAffectedSource(long id)
        {
            try
            {
                var vulner = await vulnerabilityServiceImpl.Find(x => x.DpeId == id);
                return Ok(vulner);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpGet("GetExploitTitles")]
        //[Authorize]
        public async Task<List<ExploitTitles>> GetExploitTitles()
        {
            try
            {
                var exploitViews = await expolitServiceImpl.GetItems(x => 1 == 1);
                return exploitViews.Select(x => new ExploitTitles() { exploitId = x.ExploitId, title = x.title })
                    .ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        [HttpGet("GetVulnerability")]
        [Authorize]
        public async Task<List<VulnerabilityView>> GetVulnerability()
        {
            try
            {
                return await vulnerabilityServiceImpl.GetItems(x => x.StatusId != VULNERABILITY.FIX_DONE);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        //[HttpGet("GetSingleDpe")]
        //[Authorize]
        //public async Task<List<DPEsingalview>> GetSingleDpe()
        //{
        //    try
        //    {
        //        return await _ctx.DPEsingalviews.ToListAsync();
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        throw;
        //    }
        //}

        [HttpGet("GetCountingOrg")]
        [Authorize]
        public async Task<List<CountingOrgView>> GetCountingOrg()
        {
            try
            {
                return await _ctx.CountingOrgViews.OrderByDescending(x => x.Total).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet("GetCountingExploit")]
        [Authorize]
        public List<CountingExploitView> GetCountingExploit()
        {
            return _ctx.CountingExploitViews.OrderByDescending(x => x.orgTotal).ToList();
        }

        [HttpGet("GetNumberOfDone")]
        [Authorize]
        public List<NumberOfDoneView> GetNumberOfDone()
        {
            return _ctx.NumberOfDoneViews.ToList();
        }

        [HttpGet("GetTypes")]
        [Authorize]
        public List<AssessmentType> GetTypes()
        {
            return _ctx.AssessmentTypes.ToList();
        }

        [HttpGet("GetSeverity")]
        [Authorize]
        public IList<ExploitSeverity> GetSeverity()
        {
            return _ctx.ExploitSeveritys.ToList();
        }

        [HttpGet("GetMethodology")]
        [Authorize]
        public IList<AssessmentMethodology> GetMethodology()
        {
            return _ctx.AssessmentMethodologies.ToList();
        }

        [HttpGet("GetAccessibility")]
        [Authorize]
        public IList<VerificationPossibility> GetAccessibility()
        {
            return _ctx.VerificationPossibilities.ToList();
        }

        [HttpGet("GetIp_type")]
        [Authorize]
        public IList<ExploitIpType> GetIp_type()
        {
            return _ctx.ExploitIpTypes.ToList();
        }

        [HttpGet("GetSources")]
        [Authorize]
        public IList<ExploitAffectedSource> GetSources()
        {
            return _ctx.ExploitAffectedSources.ToList();
        }

        // GET api/<DPEController>/5
        [HttpGet("GetExploitByID")]
        //[Authorize]
        public async Task<Exploit> GetExploitByID(long id)
        {
            return await expolitServiceImpl.Find(id);
        }

        [HttpGet("GetVulnerabilityByID")]
        [Authorize]
        public async Task<Vulnerability> GetVulnerabilityByID(long id)
        {
            try
            {
                return await vulnerabilityServiceImpl.Find(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet("GetVulnerabilityByDpeID")]
        [Authorize]
        public async Task<List<VulnerabilityView>> GetVulnerabilityByDpeID(long id)
        {
            try
            {
                return await vulnerabilityServiceImpl.GetItems(x => x.DPEId == id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet("GetAllVulByExploit")]
        [Authorize]
        public async Task<IList<VulnerabilityView>> GetAllVulByExploit(long id)
        {
            return await vulnerabilityServiceImpl.GetItems(x => x.ExploitId == id);
        }

        [HttpGet("GetAllExploitByOrg")]
        [Authorize]
        public async Task<IList<VulnerabilityView>> GetAllExploitByOrg(long id)
        {
            return await vulnerabilityServiceImpl.GetItems(x => x.OrgId == id);
        }

        [HttpGet("GetSingleDPEByID")]
        [Authorize]
        public async Task<IActionResult> GetSingleDPEByID(long id)
        {
            try
            {
                DPE dpe = await dpeExploitService.Find(id);
                var vulner = await vulnerabilityServiceImpl.GetItems(x => x.DPEId == id);
                var exploitsIds = vulner.Select(x => x.ExploitId).ToList();
                var vulnerIds = vulner.Select(x => x.Id).ToList();
                var nodes = await _ctx.ExploitAffectedSources.Where(x => vulnerIds.Any(a => a == x.VulnerabilityId))
                    .ToListAsync();
                var exploitViews = await expolitServiceImpl.GetItems(x => exploitsIds.Any(l => x.ExploitId == l));
                
                return Ok(dpe);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetDPEByID")]
        [Authorize]
        public async Task<IActionResult> GetDPEByID(long id)
        {
            try
            {
                DPE dpe = await dpeExploitService.Find(id);
                var vulner = await vulnerabilityServiceImpl.GetItems(x => x.DPEId == id);
                var exploitsIds = vulner.Select(x => x.ExploitId).ToList();
                var vulnerIds = vulner.Select(x => x.Id).ToList();
                var nodes = await _ctx.ExploitAffectedSources.Where(x => vulnerIds.Any(a => a == x.VulnerabilityId))
                    .ToListAsync();
                var exploitViews = await expolitServiceImpl.GetItems(x => exploitsIds.Any(l => x.ExploitId == l));
                var vulner1 = vulner.Select(x => new Vulnerability()
                {
                    Id = x.Id,
                    Exploit = getExploit(x.ExploitId, exploitViews),
                    AssessmentTypeId = x.AccessibilityId,
                    AffectedSource = nodes.Where(a => a.VulnerabilityId == x.Id).ToList(),
                    DaysToFix = x.DaysToFix,
                    ExploitId = x.ExploitId,
                    //evidence = x.evidence,
                    StatusId = x.StatusId,
                    DpeId = x.DPEId,
                    hasTested = x.hasTested
                }).ToList();

                dpe.Vulnerability = vulner1;
                return Ok(dpe);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        private Exploit getExploit(long argExploitId, List<ExploitView> exploitViews)
        {
            var exploitView = exploitViews.Find(x => x.ExploitId == argExploitId);
            return new Exploit()
            {
                Id = exploitView.ExploitId,
                CVE = exploitView.cve,
                Title = exploitView.title,
                Descriptin = exploitView.descriptin,
                Solution = exploitView.Solution,
                SeverityId = exploitView.severityId,
                Resources = exploitView.resources
            };
        }

        [HttpPost("VarifyUnkownDpe")]
        [Authorize]
        public async Task<IActionResult> VarifyUnkownDpe([FromBody] DPE dpe)
        {
            try
            {
                var username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user = await usersService.GetByUsername(username);

                var newDPE = await dpeExploitService.Find(dpe.Id);
                newDPE.CreatedById = user.Id;
                newDPE.LastUpdateDate = DateTime.Now;
                newDPE.OrganizationId = dpe.OrganizationId;
                newDPE.StatusId = VULNERABILITY.WAITING_DOC;
                var item = await dpeExploitService.UpdateEntity(newDPE);
                // add notification 
                await notificationHelper.BuildNotificationForClaim(user.Id, (int)EntityType.DPE, dpe.Id,
                    VULNERABILITY.WAITING_VARIFY, TYPES.DPE, VALUES.HEAD_DIRECTIONS);
                return Ok(item);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST api/<DPEController>
        [HttpPost("AddExploit")]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] Exploit newExploit)
        {
            try
            {
                _ctx.Exploits.Add(newExploit);
                await _ctx.SaveChangesAsync();
                return Ok(SuccessResponse<Vulnerability>.build(null, 0, null));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }


        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Exploit newExploit)
        {
            try
            {
                var exploit = await expolitServiceImpl.Find(newExploit.Id);
                exploit.SeverityId = newExploit.Severity.Id;
                exploit.CVE = newExploit.CVE;
                exploit.Descriptin = newExploit.Descriptin;
                exploit.Resources = newExploit.Resources;
                exploit.Title = newExploit.Title;
                exploit.Solution = newExploit.Solution;
                await expolitServiceImpl.UpdateEntity(exploit);

                return Ok(SuccessResponse<Vulnerability>.build(null, 0, null));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpPost("AddDPE")]
        public async Task<IActionResult> Post([FromBody] DPE newDPE)
        {
            try
            {
                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user = await usersService.GetByUsername(username);

                newDPE.CreatedById = user.Id;
                newDPE.LastUpdateDate = DateTime.Now;
                newDPE.StatusId = VULNERABILITY.WAITING_DOC;
                var item = await dpeExploitService.AddItem(newDPE);
                return Ok(item.Entity);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPost("SubmitDPE")]
        public async Task<IActionResult> SubmitDPE([FromBody] DPE newDPE)
        {
            try
            {
                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user = await usersService.GetByUsername(username);

                var count = await vulnerabilityServiceImpl.GetCount(x => x.DPEId == newDPE.Id);
                if (count == 0)
                {
                    return Ok(FailedResponse.Build("لا توجد ثغرات مضافة للتقرير"));
                }

                var dpe = await dpeExploitService.Find(newDPE.Id);
                dpe.StatusId = dpe.Organization.Id == configuration.NonSpacifiedOrgId
                    ? VULNERABILITY.WAITING_VARIFY
                    : VULNERABILITY.WAITING_HEAD_DIRECTIONS;
                dpe.LastUpdateById = user.Id;
                dpe.LastUpdateDate = DateTime.Now;

                var item = await dpeExploitService.UpdateEntity(dpe);

                await notificationHelper.BuildNotificationForClaim(user.Id, (int)EntityType.DPE, dpe.Id,
                    VULNERABILITY.WAITING_VARIFY, TYPES.DPE, VALUES.VARFIY);
                return Ok(item);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPost("ADDVulnerability")]
        public async Task<IActionResult> Post([FromBody] Vulnerability vulner)
        {
            try
            {
                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user = await usersService.GetByUsername(username);


                if (vulner.DpeId != null)
                {
                    vulner.CreatedById = user.Id;
                    vulner.StatusId = VULNERABILITY.WAITING_ORG_INFORMING;
                    var dbResponse = await vulnerabilityServiceImpl.AddItem(vulner);
                    ChangeLogHelper.AddChangeLogToEntity(vulner, user.Id,
                        new[] { new Change() { OldValue = UNDER_PROC, newValue = UNDER_PROC, Field = STATUS } }
                            .ToList());
                    return dbResponse.IsSuccess ? Ok(dbResponse.Entity) : Ok(FailedResponse.Build(dbResponse.Message));
                }

                return BadRequest("لا يوجد رقم معرف للتقييم");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPost("UpdateVulnerability")]
        [Authorize]
        public async Task<IActionResult> UpdateVulnerability([FromBody] Vulnerability value)
        {
            string username = (string)HttpContext.Items[Constants.UserId.ToString()];
            var user = await usersService.GetByUsername(username);

            try
            {
                var newValue = _ctx.Vulnerabilities.Find(value.Id);

                newValue.evidence = value.evidence;
                newValue.ModifiedBy = user;
                newValue.Exploit = _ctx.Exploits.Find(value.Exploit.Id);
                newValue.hasTested = value.hasTested;
                newValue.report = value.report;
                newValue.OrgReport = value.OrgReport;
                newValue.TcReport = value.TcReport;
                newValue.DaysToFix = value.DaysToFix;

                _ctx.Vulnerabilities.Update(newValue);
                _ctx.SaveChanges();

                return Ok(SuccessResponse<Vulnerability>.build(null, 0, null));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpPost("ChangeStatus")]
        [Authorize]
        public async Task<IActionResult> ChangeStatus([FromBody] Vulnerability newValue)
        {
            string username = (string)HttpContext.Items[Constants.UserId.ToString()];
            var user = await usersService.GetByUsername(username);

            try
            {
                var oldValue = await vulnerabilityServiceImpl.Find(newValue.Id);
                string[] status = getStatus(oldValue.StatusId, newValue.StatusId);
                ChangeLogHelper.AddChangeLogToEntity(oldValue, user.Id,
                    new[] { new Change() { OldValue = status[0], newValue = status[1], Field = STATUS } }.ToList());
                oldValue.StatusId = newValue.StatusId;
                oldValue.LastUpdateById = user.Id;
                oldValue.LastUpdateDate = DateTime.Now;
                oldValue.ExtraNote1 = newValue.ExtraNote1;
                await vulnerabilityServiceImpl.UpdateEntity(oldValue);

                // FIX is done, no need to notify any body
                if (newValue.StatusId != VULNERABILITY.FIX_DONE)
                {
                    string notificationClaimValue = newValue.StatusId == VULNERABILITY.WAITING_FIX
                        ? VALUES.ORG_FIX_DONE
                        : VALUES.WAITING_FIX_AUTHENTICATION;
                    await notificationHelper.BuildNotificationForClaim(user.Id, (int)EntityType.Vulnerability,
                        oldValue.Id, newValue.StatusId, TYPES.DPE, notificationClaimValue);
                }

                return Ok(SuccessResponse<Vulnerability>.build(null));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        string[] getStatus(long? oldStatus, long? newStatus)
        {
            return newStatus == VULNERABILITY.WAITING_ORG_INFORMING
                ? new[] { "انتظار التوجيه", "انتظار إعلام المؤسسة" }
                : newStatus == VULNERABILITY.WAITING_FIX_AUTHENTICATION
                    ? new[] { "انتظار الإصلاح", "إنتظار التحقق" }
                    : newStatus == VULNERABILITY.WAITING_FIX
                        ? oldStatus == VULNERABILITY.WAITING_FIX_AUTHENTICATION
                            ? new[] { "انتظار التحقق", "لم يتم الإصلاح , إنتظار الإصلاح" }
                            : new[] { "انتظار الإعلام", "إنتظار الإصلاح" }
                        : new[] { "انتظار التحقق", "انتهت  جمبع الإجراءات" };
        }

        [HttpPost("SetInformingOrgDone")]
        [Authorize]
        public async Task<IActionResult> SetInformingOrgDone([FromBody] DPE value)
        {
            string username = (string)HttpContext.Items[Constants.UserId.ToString()];
            var user = await usersService.GetByUsername(username);


            try
            {
                var oldValue = await dpeExploitService.Find(value.Id);
                ChangeLogHelper.AddChangeLogToEntity(oldValue, user.Id,
                    new[] { new Change() { OldValue = WAITING_COR_PROC, newValue = INFORMING_DONE, Field = STATUS } }
                        .ToList());
                var vulners = await _ctx.Vulnerabilities.Where(x => x.DpeId == value.Id).ToListAsync();
                foreach (var vulnerability in vulners)
                {
                    vulnerability.StatusId = VULNERABILITY.WAITING_FIX;
                    vulnerability.LastUpdateDate = DateTime.Now;
                    ChangeLogHelper.AddChangeLogToEntity(vulnerability, user.Id,
                        new[]
                        {
                            new Change() { OldValue = WAITING_COR_PROC, newValue = INFORMING_DONE, Field = STATUS }
                        }.ToList());
                }

                await vulnerabilityServiceImpl.UpdateRange(vulners);

                oldValue.ContactedPersonId = value.ContactedPersonId;
                oldValue.LastUpdateDate = DateTime.Now;
                oldValue.MethodOfInforming = value.MethodOfInforming;
                oldValue.InformingDate = value.InformingDate;
                oldValue.StatusId = VULNERABILITY.WAITING_FIX;
                oldValue.InformingBy = user.FullName;
                oldValue.InformingNote = value.InformingNote;
                await dpeExploitService.UpdateEntity(oldValue);

                foreach (var vul in vulners)
                {
                    await notificationHelper.BuildNotificationForClaim(user.Id, (int)EntityType.Vulnerability, vul.Id,
                        VULNERABILITY.WAITING_FIX, TYPES.DPE, VALUES.ORG_FIX_DONE);
                }

                return Ok(SuccessResponse<Vulnerability>.build(null, 0, null));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }


        [HttpPost("SetHeadDirections")]
        [Authorize]
        public async Task<IActionResult> SetHeadDirections([FromBody] DPE value)
        {
            string username = (string)HttpContext.Items[Constants.UserId.ToString()];
            var user = await usersService.GetByUsername(username);


            try
            {
                var oldValue = await dpeExploitService.Find(value.Id);
                ChangeLogHelper.AddChangeLogToEntity(oldValue, user.Id,
                    new[]
                    {
                        new Change()
                            { OldValue = "إنتظار توجيهات الرئاسة", newValue = "انتظار التنسيق", Field = STATUS }
                    }.ToList());
                oldValue.HeadDirections = user.FullName + " : " + value.HeadDirections;
                oldValue.LastUpdateDate = DateTime.Now;
                oldValue.LastUpdateById = user.Id;
                oldValue.StatusId = VULNERABILITY.WAITING_ORG_INFORMING;
                await dpeExploitService.UpdateEntity(oldValue);
                await notificationHelper.BuildNotificationForClaim(user.Id, (int)EntityType.DPE, oldValue.Id,
                    VULNERABILITY.WAITING_ORG_INFORMING, TYPES.DPE, VALUES.ORG_INFORM);

                return Ok(SuccessResponse<Vulnerability>.build(null));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpPost("UpdateDPE")]
        [Authorize]
        public async Task<IActionResult> UpdateDPE([FromBody] DPE value)
        {
            string username = (string)HttpContext.Items[Constants.UserId.ToString()];
            var user = await usersService.GetByUsername(username);
            try
            {
                var newValue = await dpeExploitService.Find(value.Id);

                newValue.LastUpdateById = user.Id;
                newValue.DateFrom = value.DateFrom;
                newValue.DateTo = value.DateTo;
                newValue.AssessmentMethodologyId = value.AssessmentMethodologyId;
                newValue.OrganizationId = value.OrganizationId;
                newValue.LastUpdateDate = DateTime.Now;
                newValue.Scope = value.Scope;
                newValue.Recommendation = value.Recommendation;
                newValue.Title = value.Title;


                await dpeExploitService.UpdateEntity(newValue);
                return Ok(SuccessResponse<DPE>.build(newValue, 0, null));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }


        [HttpPost("IgnoreDPE")]
        [Authorize]
        public async Task<IActionResult> IgnoreDPE([FromBody] DPE value)
        {
            string username = (string)HttpContext.Items[Constants.UserId.ToString()];
            var user = await usersService.GetByUsername(username);
            try
            {
                var oldValue = await dpeExploitService.Find(value.Id);
                ChangeLogHelper.AddChangeLogToEntity(oldValue, user.Id,
                    new[]
                    {
                        new Change()
                            { OldValue = "إنتظار إعلام المؤسسة", newValue = "تمت ارشفة التقرير", Field = STATUS }
                    }.ToList());
                oldValue.StatusId = VULNERABILITY.IGNORED_CASE;
                oldValue.ExtraNote1 = value.ExtraNote1;
                oldValue.IsDone = true;
                var vulners = await _ctx.Vulnerabilities.Where(x => x.DpeId == value.Id).ToListAsync();
                foreach (Vulnerability x in vulners)
                {
                    x.StatusId = VULNERABILITY.IGNORED_CASE;
                    x.LastUpdateDate = DateTime.Now;
                    ChangeLogHelper.AddChangeLogToEntity(x, user.Id,
                        new[] { new Change() { OldValue = WAITING_COR_PROC, newValue = IGNORE_EXPO, Field = STATUS } }
                            .ToList());
                }

                ;
                await vulnerabilityServiceImpl.UpdateRange(vulners);
                await dpeExploitService.UpdateEntity(oldValue);

                return Ok(SuccessResponse<DPE>.build(null, 0, null));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        //PUT api/<DPEController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<DPEController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}