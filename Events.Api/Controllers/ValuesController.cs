using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Events.Api.Models.UserManagement;
using Events.Core.Models;
using Events.Core.Models.DPE;
using Events.Core.Models.General;
using Events.Core.Models.Incidents;
using Events.Core.Models.Notifications;
using Events.Core.Models.Tasks;
using Events.Data;
using Events.Service.Service;
using Events.Service.Service.DataServices;
using LinqKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Options;
using UserManagment.services;
using Task = Events.Core.Models.Tasks.Task;


namespace Events.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        private readonly AppDbContext context;
        private readonly DbServiceImpl<Incident, IncidentView> incidentService;
        private readonly DbServiceImpl<Inbox, Inbox> InboxService;
        private readonly DbServiceImpl<Vulnerability, VulnerabilityView> _VulnerServiceImpl;
        private readonly DbServiceImpl<Notification, NotificationView> notificationService;
        private readonly DbServiceImpl<LateTopics, LateTopics> lateTopicsService;
        private readonly DbServiceImpl<DepartmentRecords, DepartmentRecords> departmentRecordsService;
        private readonly DbServiceImpl<SectionRecords, SectionRecords> sectionRecordsService;
        private readonly DbServiceImpl<Task, Taskview> tasksService;
        private readonly IUserService usersService;
        private readonly SysConfiguration configuration;

        public ValuesController(AppDbContext ctx, IOptions<SysConfiguration> conf,
            IUserService us, IServiceFactory service,
            IUserService userService)
        {
            
            context = ctx;
            incidentService = service.ServicOf<Incident, IncidentView>();
            _VulnerServiceImpl = service.ServicOf<Vulnerability, VulnerabilityView>();
            departmentRecordsService = service.ServicOf<DepartmentRecords, DepartmentRecords>();
            sectionRecordsService = service.ServicOf<SectionRecords, SectionRecords>();
            lateTopicsService = service.ServicOf<LateTopics, LateTopics>();
            tasksService = service.ServicOf<Task, Taskview>();
            InboxService = service.ServicOf<Inbox, Inbox>();
            notificationService = service.ServicOf<Notification, NotificationView>();
            usersService = us;
            configuration = conf.Value;

        }

        private bool isOman(string country)
        {
            var c = country.ToLower();
            return c.Equals("om") || c.Equals("oman");
        }

        // GET: api/<ValuesController>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            try
            {
                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
            var user =await usersService.GetByUsername(username);
            Expression<Func<Inbox, bool>> expression = x => x.ToUser == user.Id || x.Claim != "";
            var inboxe =  await InboxService.GetItems(expression,null,getFunction(user));
            


            CountModel countModel = new CountModel();
            countModel.Reports = inboxe.Count(n => n.TypeId == (int)EntityType.Report);

            var YearVulner = await _VulnerServiceImpl.GetItems(x => true, null, x => compareYears(x.CreatedDate));
            var pendingVulners = await _VulnerServiceImpl.GetItems(compareStatus(VULNERABILITY.FIX_DONE));

            // vulnerabilities status
            countModel.WaitingOrgInformVul = pendingVulners.Count(compareStatus2(VULNERABILITY.WAITING_ORG_INFORMING));
            countModel.WaitingFixVul = pendingVulners.Count(view =>view.StatusId == VULNERABILITY.WAITING_FIX);
            countModel.WaitingFixAuthVul = pendingVulners.Count(view =>view.StatusId == VULNERABILITY.WAITING_FIX_AUTHENTICATION);

               var lateTopics = await lateTopicsService.GetItems(x => true);

               foreach(LateTopics late in lateTopics.Where(x => x.Id != configuration.TCDepartmentId))
                {
                    late.Incidents = 0;
                    late.Vulnerability = 0;
                    late.lateInforming = 0;
                }
               
               foreach(LateTopics late in lateTopics.Where(x => x.Id != configuration.InternalSec))
               {
                   late.IpAddress = 0;
               }

               countModel.LateTopics = lateTopics;
            
            //department Statistics
            var departmentStatistics = await departmentRecordsService.GetItems(x => true);
            countModel.DepartmentRecords = departmentStatistics;
            //late Vulnerabilites
            countModel.LateVul = pendingVulners.Count(view => view.RemainingDays < 0);

             var results = await tasksService.GetItems(x => x.asignedforid == user.Id && (x.statusId == TASK.IN_PROGRESS
             || x.statusId == TASK.OPEN));

            var values = results.Select(t => t.progress).ToList();
            countModel.TasksPrgressAverage = values.Count > 0 ? values.Average() : 100;
                     
                


                // vulnerabilities Saverity
                countModel.VulCritical = YearVulner.Count(view => view.Severty.Equals("Critical"));
            countModel.VulMedium = YearVulner.Count(view => view.Severty.Equals("Medium"));
            countModel.VulLow = YearVulner.Count(view => view.Severty.Equals("Low"));

            var statues = new List<long>() {NOTIFICATION.OPEN_NOTIFICATION,NOTIFICATION.INCIDENT};
            var socSerctions = new List<long>() {configuration.ExternalSec, configuration.InternalSec};
            
            
            
            
            var incidents = await incidentService
                .GetItems(x => statues.Any(id => id == x.statusId) );
            var incidentsCount = incidents.Where(x => x.statusId == NOTIFICATION.INCIDENT)
                .Select(x => x.id).Distinct().Count();

                var sectionRecordsList = await sectionRecordsService.GetItems(x => true);


                countModel.IncidentsESec = sectionRecordsList.Find(x => x.Id == configuration.ExternalSec)?.Notifications ?? 0;
                countModel.IncidentsISoc = sectionRecordsList.Find(x => x.Id == configuration.InternalSec)?.Notifications ?? 0;




            countModel.aptESec = sectionRecordsList.FirstOrDefault(x => x.Id == configuration.ExternalSec)?.Apts ?? 0;
            countModel.aptISoc = sectionRecordsList.FirstOrDefault(x => x.Id == configuration.InternalSec)?.Apts ?? 0;
            // Notification
            var notifications =incidents.Where(x => x.statusId == NOTIFICATION.OPEN_NOTIFICATION);
            var notificationsCount =    notifications.Select(x => x.id).Distinct().Count();


            // pending ips requests..
            var ipsList = context.IpAddress
                    .Include(x => x.Source)
                    .Include(x => x.Dest)
                    .Where(x => x.IsRequestVarify && !x.IsHandeled)
                    .ToList();
            long ips = ipsList.Where(x => isOman(x.SourceCountry) || isOman(x.DestinationCountry)).ToList().Count();
            
            

            //incidents
            countModel.Incidents = incidentsCount;
            countModel.Notifications = notificationsCount;

            //notifications

            //ips
            countModel.VarifyRequests = ips;
            
            
            return Ok(SuccessResponse<CountModel>.build(countModel, 0));
            
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
            
        }
        
        [HttpGet("GetInbox")]
        [Authorize]
        public async Task<IActionResult> GetInbox()
        {
            try
            {
                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user =await usersService.GetByUsername(username);
                Expression<Func<Inbox, bool>> expression = x =>x.ToUser == user.Id || x.Claim != "";
                var inboxe =  await InboxService.GetItems(expression,null,getFunction(user));
                var entityIds = inboxe.Select(y => new { id = y.Id , type = y.TypeId}).ToList();

            
                var result = 
                    await notificationService.GetItems(x =>x.OwnerId == user.Id,null, x => entityIds.Any(s => s.id == x.EntityId && x.EntityType == s.type));
            
                foreach (var inbox in inboxe)
                {
                    if (inbox.TypeId == (int) EntityType.Incident)
                    {
                        var notificationView = result.Where(x =>
                            x.EntityId == inbox.Id && x.EntityType == inbox.TypeId && inbox.EntityStatusId == x.StatusId).ToList();
                        inbox.isNew = notificationView.Any(x => x.NotificationStatus);
                        inbox.isUpdated = notificationView.Any(x => x.IsUpdated);
                        inbox.Date = notificationView.Any(x => x.IsUpdated) ? notificationView.Find(x => x.IsUpdated).lastupdated : inbox.Date;
                    }
                    else
                    {

                        var notificationView = result.Find(x => x.EntityId == inbox.Id && x.EntityType == inbox.TypeId);
                        inbox.isNew = notificationView?.NotificationStatus ?? false;
                        inbox.isUpdated = notificationView?.IsUpdated ?? false;
                        inbox.Date = notificationView != null && notificationView.IsUpdated
                            ? notificationView.lastupdated
                            : inbox.Date;
                    }
                }

                return Ok(SuccessResponse<Inbox>.build(null,0,inboxe));
            
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
            
        }
        
        [HttpPost("UpdateInobx")]
        [Authorize]
        public async Task<IActionResult> UpdateInobx([FromBody] Inbox inbox)
        {
            try
            {
                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user =await usersService.GetByUsername(username);

                Expression<Func<Notification, bool>> predicate;
                if (inbox.TypeId == (int) EntityType.Incident)
                    predicate = note => note.EntityType == inbox.TypeId && note.EntityId == inbox.Id && note.StatusId == inbox.EntityStatusId;
                else
                    predicate = note => note.EntityType == inbox.TypeId && note.EntityId == inbox.Id;
                
                var notification = await notificationService.GetFirst(predicate);


                if (notification != null)
                {
                    var found = false;
                    foreach (var owner in notification.NotificationOwners.Where(x => x.employeeId == user.Id))
                    {
                        found = true;
                        owner.isNew = false;
                        owner.IsUpdated = false;
                    }
                    if(found) 
                        await notificationService.UpdateEntity(notification);
                    else
                    {
                        notification.NotificationOwners.Add(new NotificationOwner(){employeeId = user.Id,isNew = false,IsUpdated = false});
                    }

                    return Ok(SuccessResponse<Notification>.build(null));

                }
                
                await addNotification(user.Id, inbox);
                return Ok(SuccessResponse<NotificationView>.build(null));
            
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
            
        }

        private async Task<object> addNotification(long userId, Inbox inbox)
        {
            var notification = new Notification();
            notification.StatusId = inbox.EntityStatusId;
            notification.CreatedById = inbox.FromUserId;
            notification.DateTime = inbox.Date;
            notification.EntityType = inbox.TypeId;
            notification.EntityId = inbox.Id;
            notification.NotificationOwners = new List<NotificationOwner>()
                {new NotificationOwner() {employeeId = userId, isNew = false, IsUpdated = false}};


            var response = await notificationService.AddItem(notification);
            return response;
        }
        

        private static Func<VulnerabilityView, bool> compareStatus2(long status)
        {
            return view => view.StatusId == status;
        }

        private static Expression<Func<VulnerabilityView, bool>> compareStatus(long status)
        {
            return x => x.StatusId != status;
        }

        private static  bool compareYears(DateTime dateTime)
       => dateTime.Year == DateTime.Now.Year;
        



        private Func<Inbox, bool> getFunction(EUser user)
        => x => x.ToUser != 0 || (
        x.Claim != "" && usersService.DoesHavePermission(user.Id, x.Claim.Split(',')[0], x.Claim.Split(',')[1]).Result);
        
        
    }
    
    
}
