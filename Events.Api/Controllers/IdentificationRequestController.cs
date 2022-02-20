
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Events.Api.Models.General;
using Events.Api.Models.Incidents;
using Events.Core.Models.General;
using Events.Core.Models.Incidents;
using Events.Core.Models.IpIdentification;
using Events.Core.Models.Logging;
using Events.Core.Models.Services;
using Events.Data;
using Events.Service.Service;
using Events.Service.Service.DataServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UserManagment.services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Events.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentificationRequestController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IServiceFactory serviceFactory;
        private readonly DbServiceImpl<Organization, Organization> _orgService;
        private readonly DbServiceImpl<Incident, IncidentView> _incidentService;
        private  readonly IUserService _usersService;
        private readonly IChangeLogHelper ChangeLogHelper;
        private readonly DbServiceImpl<Status, Status> _statusService;
        private readonly SysConfiguration configuration;
        

        public IdentificationRequestController(AppDbContext impl, IServiceFactory service,IOptions<SysConfiguration> config,
            IUserService us)
        {
            context = impl;
            serviceFactory = service;
            _orgService = service.ServicOf<Organization, Organization>();
            _incidentService = service.ServicOf<Incident, IncidentView>();
            _statusService = service.ServicOf<Status, Status>();
            ChangeLogHelper = service.ChangeLogHelper();
            configuration = config.Value;
            _usersService = us;
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                Incident inc = context.Incidents
                   .Where(x => x.Id == id)
                   .Include(x => x.IpAddresses).ThenInclude(x => x.Source)
                    .Include(x => x.IpAddresses).ThenInclude(x => x.Dest)
                    .Include(x => x.IpAddresses).ThenInclude(x => x.OwnerDetail)
                   .SingleOrDefault();
                return Ok(SuccessResponse<IpAddress>.build(null, 0, inc.IpAddresses));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }
        
        [HttpGet("{id}")]
        public IActionResult GetTargetedIps(int id)
        {
            try
            {
                Incident inc = context.Incidents
                    .Where(x => x.Id == id)
                    .Include(x => x.IpAddresses).ThenInclude(x => x.Source)
                    .Include(x => x.IpAddresses).ThenInclude(x => x.Dest)
                    .Include(x => x.IpAddresses).ThenInclude(x => x.OwnerDetail)
                    .SingleOrDefault();
                var re  = inc.IpAddresses.Select(x => x.OwnerDetail).ToList();
                //var ownerDetailsEnumerable = DistinctBy(inc.IpAddresses.Select(x => x.OwnerDetail), x => x.ownerSub).ToList();
                return Ok(SuccessResponse<OwnerDetails>.build(null, 0, re));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }
        
       

        [HttpGet("GetAptGroups")]
        public IActionResult GetApts(int id)
        {
            try
            {
                List<string> apts = context.IpAddress.Select(x => x.AptGroup).Distinct().ToList(); ;
                List<StringDTO> aptsList = apts.Select(x => new StringDTO() { Name = x }).ToList();
                return Ok(SuccessResponse<StringDTO>.build(null, 0, aptsList));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpGet("GetOwners")]
        public IActionResult GetOwners(int id)
        {
            try
            {
                List<string> owners = context.OwnerDetails.Select(x => x.ownerSub).Distinct().ToList(); ;
                List<StringDTO> ownersList = owners.Where(x => x != null).Select(x => new StringDTO() { Name = x }).ToList();
                return Ok(SuccessResponse<StringDTO>.build(null, 0, ownersList));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpGet("getOwnerId")]
        public IActionResult Get(string id)
        {
            try
            {
                OwnerDetails details = context.OwnerDetails.Where(x => x.subsId == id).SingleOrDefault();

                return Ok(SuccessResponse<OwnerDetails>.build(details, 0, null));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpGet]
        [Authorize]

        public IActionResult Get()
        {
            try
            {
                List<IpAddress> list = context.IpAddress
                    .Include(x => x.Source)
                    .Include(x => x.Dest)
                    .Include(x => x.OwnerDetail)
                    .ToList();

                list = list.Where(x => isOman(x.SourceCountry) || isOman(x.DestinationCountry)).ToList();

                return Ok(SuccessResponse<IpAddress>.build(null, 0, list));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }

        }
        [HttpGet("getIncidentsWithIdRequests")]
        [Authorize]
        public IActionResult GetIncidentsById()
        {
            try
            {
                List<Incident> list = context.Incidents
                    .Include(x => x.Status).ThenInclude(x => x.Status)
                    .Where(x => x.Status.Status.Id == NOTIFICATION.SEND_TO_VARIFY)
                    .ToList();

                return Ok(SuccessResponse<Incident>.build(null, 0, list));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        private bool isOman(string country)
        {
            var c = country.ToLower();
            return c.Equals("om") || c.Equals("oman");
        }

        // POST api/<RequestIPController>
        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody] IdentificationRequest value)
        {
            try
            {
                var user = context.Users.SingleOrDefault(u => u.UserName ==
                                                              (string)HttpContext.Items[Constants.UserId.ToString()]);
                Incident incident = context.Incidents
                    .Where(x => x.Id == value.id).Include(x => x.Changes).Include(x => x.IpAddresses).SingleOrDefault();
                incident.IpAddresses
                .ForEach(ip =>
                {
                    ip.IsRequestVarify = true;
                    context.IpAddress.Update(ip);
                });
                ChangeLog changeLog = ChangeLog.Build(user.Id);
                changeLog.addChangeField(new ChangeLogField()
                {
                    FieldName = "طلب تعريف العناوين",
                    OldValue = "لا يوجد",
                    NewValue = "إرسال طلب تعريف"
                });

                incident.Changes.Add(changeLog);
                incident.IsIpsIdentificationRequested = true;
                var incidentStatus = new EntityStatus();
                incidentStatus.Status = context.Statuses.Find(NOTIFICATION.SEND_TO_VARIFY);
                incident.LastUpdateDate = DateTime.Now;
                incident.Status = incidentStatus;
                context.SaveChanges();
                return Ok(SuccessResponse<Incident>.build(null, 0, null));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }

        }

        [HttpPost("search")]
        [Authorize]
        public async Task<IActionResult> Search([FromBody] IpSearchViewModel value)
        {
            try
            {

                var incs = await serviceFactory.ServicOf<Incident, IncidentView>().GetItems(v =>
                  v.Date >= value.fromDate && v.Date <= value.toDate);

                var ids = incs.Select(x => x.id).ToList();

                List<IpAddress> ips = await serviceFactory.ServicOf<IpAddress, IpAddress>().GetItems(v =>
                  ids.Any(id => id == v.IncidentId));
                    ips = ips.Where(x => isMatch(value, x)).ToList();
                return Ok(SuccessResponse<IpAddress>.build(null, 0, ips));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }

        }

        private bool isMatch(IpSearchViewModel value, IpAddress v)
        {
            List<String> apts = !value.Apts.Equals(String.Empty) ?
                value.Apts.Split(',').ToList() : new List<String>();

            List<String> owners = !value.Owners.Equals(String.Empty) ?
                value.Owners.Split(',').ToList() : new List<String>();

            bool status = true;
            if (apts.Count > 0)
                status = apts.IndexOf(v.AptGroup) != -1;

            if (!status) return false;

            if (owners.Count > 0)
                status = v.OwnerDetail != null ? v.OwnerDetail.ownerSub != null ? owners.IndexOf(v.OwnerDetail.ownerSub) != -1 :false: false;
            
            if (!status) return false;

            if (value.OwnerType != "" && value.OwnerType != "غير محدد")
               status = v.OwnerDetail != null ? v.OwnerDetail.ownerType != null  ? v.OwnerDetail.ownerType == value.OwnerType : false : false;


            if (!status) return false;

            value.Search = value.Search.Trim();


            long x;
            long.TryParse(value.Search, out x);

            if (!value.Search.Equals(String.Empty))
            {
                status = v.Dest.ip.Contains(value.Search) || v.Dest.port.Contains(value.Search) ||
                v.Source.ip.Contains(value.Search) || v.Source.port.Contains(value.Search) || v.IncidentId == x;
            }

            if (status) return true;

            if (!value.Search.Equals(String.Empty) && v.OwnerDetail != null)
            {
                status = v.OwnerDetail.subsId != null ? v.OwnerDetail.subsId.Contains(value.Search) : false;
                if (status) return true;
                status = v.OwnerDetail.cid != null ?  v.OwnerDetail.cid.Contains(value.Search) : false;
                if (status) return true;
                status = v.OwnerDetail.ownerSub != null ? v.OwnerDetail.ownerSub.Contains(value.Search) : false;
            }


            return status;
        }

        // PUT api/<RequestIPController>/5
        [HttpPut("SetIpUnkown")]
        [Authorize]
        public async Task<IActionResult> SetIpUnkown([FromBody] IdentificationRequest value)
        {
            try
            {
                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user =await _usersService.GetByUsername(username);
                
                IpAddress ip = context.IpAddress.Find(value.id);

                ip.IsHandeled = true;
                ip.IsKnown = false;
                context.IpAddress.Update(ip);
                await context.SaveChangesAsync();
                
                var incident =await _incidentService.Find(value.incidentId);
                

                if (incident.IpAddresses.TrueForAll(ip => ip.IsHandeled))
                {
                    var status = await _statusService.Find(NOTIFICATION.OPEN_NOTIFICATION);
                    incident.Status = new EntityStatus() {Status = status};
                    incident.LastUpdateDate = DateTime.Now;
                    ChangeLogHelper.AddChangeLogToEntity(incident, user.Id,new[] { new Change() { OldValue = "طلب التعريف", newValue = "تم الإنتهاء من طلب التعريف", Field = "الحالة" } }.ToList());
                }

                await _incidentService.UpdateEntity(incident);
                return Ok(SuccessResponse<Incident>.build(null, 0, null));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }
        
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Put([FromBody] IdentificationRequest value)
        {
            try
            {
                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user =await _usersService.GetByUsername(username);
                
                var isGov = value.ownerType.Equals("حكومي");
                IpAddress ip = context.IpAddress.Find(value.id);
                OwnerDetails details = isGov ?
                    await context.OwnerDetails.FirstOrDefaultAsync(x => x.ownerSub == value.orgname) : 
                   await context.OwnerDetails.FirstOrDefaultAsync(x => x.subsId == value.subsId && x.ownerSub == value.ownerSub);
                if (details != null)
                {
                    ip.OwnerDetail = details;
                }
                else
                {
                    ip.OwnerDetail = new OwnerDetails();
                    ip.OwnerDetail.cid = value.cid;
                    ip.OwnerDetail.subsId = value.subsId;
                    ip.OwnerDetail.ownerSub = isGov ?  value.orgname : value.ownerSub;
                    ip.OwnerDetail.ownerType = value.ownerType;
                    ip.OwnerDetail.phoneNum = value.phoneNum;
                    ip.AptGroup = value.aptGroup;
                }

                ip.IsHandeled = true;
                ip.IsKnown = value.isKnown;
                context.IpAddress.Update(ip);
                await context.SaveChangesAsync();
                
                var incident = await _incidentService.Find(value.incidentId);

                if (isGov && !incident.Orgs.Exists(v=> v.Organization.Id == value.orgid))
                {
                    
                    var organization =await _orgService.Find(value.orgid);
                    incident.Orgs.Add(new OrgsIncidentRel(){Organization =  organization});
                    var itemToRemove = incident.Orgs.SingleOrDefault(x => x.OrganizationId == configuration.NonSpacifiedOrgId);
                    if (itemToRemove != null)
                        incident.Orgs.Remove(itemToRemove);
                }

                if (incident.IpAddresses.TrueForAll(ip => ip.IsHandeled))
                {
                    var status =await _statusService.Find(NOTIFICATION.OPEN_NOTIFICATION);
                    incident.Status = new EntityStatus() {Status = status};
                    incident.LastUpdateDate = DateTime.Now;
                    ChangeLogHelper.AddChangeLogToEntity(incident, user.Id,new[] { new Change() { OldValue = "طلب التعريف", newValue = "تم الإنتهاء من طلب التعريف", Field = "الحالة" } }.ToList());
                }

                await _incidentService.UpdateEntity(incident);
                return Ok(SuccessResponse<Incident>.build(null, 0, null));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        // DELETE api/<RequestIPController>/5
        [HttpDelete]
        [Authorize]
        public IActionResult Delete(int id)
        {
            return Ok(SuccessResponse<Incident>.build(null, 0, null));
        }
    }

    public static class IDS
    {
        private static IEnumerable<TSource> DistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}
