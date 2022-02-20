using Events.Api.Authorization;
using Events.Api.Logging;
using Events.Api.Models.APTs;
using Events.Api.Models.General;
using Events.Api.Models.Incidents;
using Events.Api.Models.Tasks;
using Events.Api.Models.UserManagement;
using Events.Core.Models.General;
using Events.Core.Models.Incidents;
using Events.Service.Service;
using Events.Service.Service.DataServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Resources;
using System.Threading.Tasks;
using Events.Service.Files;
using LinqKit;
using Microsoft.Extensions.Options;
using UserManagment.services;

namespace Events.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentController : ControllerBase
    {
        private IServiceFactory ServiceFactory;
        private DbServiceImpl<Incident, IncidentView> IncidentService;
        private DbServiceImpl<EntityAssignment, EntityAssignment> EntityAssignmentService;
        private DbServiceImpl<Status, Status> StatusService;
        private IUserService usersService;
        private readonly IWebHostEnvironment host;
        private IChangeLogHelper ChangeLogHelper;
        private IFileHandler fileHandler;
        private readonly SysConfiguration configuration;

        public IncidentController(IServiceFactory service, IUserService us, IFileHandler fileHandler,
            IOptions<SysConfiguration> conf)
        {
            ServiceFactory = service;
            IncidentService = ServiceFactory.ServicOf<Incident, IncidentView>();
            StatusService = ServiceFactory.ServicOf<Status, Status>();
            EntityAssignmentService = ServiceFactory.ServicOf<EntityAssignment, EntityAssignment>();
            ChangeLogHelper = ServiceFactory.ChangeLogHelper();
            this.fileHandler = fileHandler;
            usersService = us;
            configuration = conf.Value;
        }

        [HttpGet("fetchReportCategories")]
        public IList<Category> GetIncidentCategories() => ServiceFactory.GetConstantList<Category>();

        [HttpPost("AddIncidentCategory")]
        public async Task<IActionResult> AddIncidentCategory([FromBody] Category newIncidentCategory)
        {
            var result = await IncidentService.AddConstant(newIncidentCategory);
            return Ok(SuccessResponse<IncidentCategory>.build(null));
        }

        [HttpGet("getIncidentById")]
        [ServiceFilter(typeof(LogAction))]
        [ClaimsAuth(Type: TYPES.NOTIFICATIONS, Value: VALUES.VIEW)]
        public async Task<Incident> GetIncidentById(int id)
            => await IncidentService.Find(id);

        [HttpGet("getIncidentByIdForProcesdure")]
        [ServiceFilter(typeof(LogAction))]
        [Authorize]
        public async Task<IActionResult> getIncidentByIdForProcesdure(int id)
        {
            string username = (string)HttpContext.Items[Constants.UserId.ToString()];
            var user = await usersService.GetByUsername(username);

            try
            {
                //TODO check if the user have permission
                var count = await EntityAssignmentService.GetCount(x => x.UserId == user.Id && id == x.IncidentId );
                count += await IncidentService.GetCount(x => x.id == id && x.statusId == NOTIFICATION.EDIT_INCIDENT);
                if (count == 0) return BadRequest("لا توجد صلاحية");
                return Ok(await IncidentService.Find(id));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpGet("IncidentsWithIdRequests")]
        [ServiceFilter(typeof(LogAction))]
        public async Task<List<IncidentView>> IncidentsWithIdRequests(int id)
            => await IncidentService.GetItems(x => x.isSendToIpVarify);

        [HttpGet("fetchIpsOfIncidentById")]
        [ServiceFilter(typeof(LogAction))]
        public async Task<List<IpAddress>> fetchIpsOfIncidentById(int id)
            => (await IncidentService.Find(id)).IpAddresses.FindAll(x => x.IsRequestVarify && !x.IsHandeled);


        // GET: api/Incident
        [HttpGet("incidents")]
        [ServiceFilter(typeof(LogAction))]
        [ClaimsAuth(Type: TYPES.NOTIFICATIONS, Value: VALUES.VIEW)]
        public async Task<IActionResult> GetIncidents(int pageSize, int pageNumber)
        {
            try
            {
                PaggingModel page = new PaggingModel() { PageNumber = pageNumber, PageSize = pageSize };
                long count = 0;
                if (pageNumber == 1)
                    count = await IncidentService.GetCount(x => x.statusId == NOTIFICATION.INCIDENT);
                var incidentViews = await IncidentService.GetItems(x => x.statusId == NOTIFICATION.INCIDENT, page);
                return Ok(SuccessResponse<IncidentView>.build(null, count, incidentViews));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("notifications")]
        [ServiceFilter(typeof(LogAction))]
        [ClaimsAuth(Type: TYPES.NOTIFICATIONS, Value: VALUES.VIEW)]
        public async Task<IActionResult> GetNotification(int pageSize, int pageNumber)
        {
            try
            {
                PaggingModel page = new PaggingModel() { PageNumber = pageNumber, PageSize = pageSize };
                long count = 0;
                if (pageNumber == 1)
                    count = await IncidentService.GetCount(x => x.statusId == NOTIFICATION.OPEN_NOTIFICATION);
                var incidentViews =
                    await IncidentService.GetItems(x => x.statusId == NOTIFICATION.OPEN_NOTIFICATION, page);
                return Ok(SuccessResponse<IncidentView>.build(null, count, incidentViews));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("closed_incidents")]
        [ServiceFilter(typeof(LogAction))]
        [ClaimsAuth(Type: TYPES.NOTIFICATIONS, Value: VALUES.VIEW)]
        public async Task<List<IncidentView>> GetClosedIncidents()
            => await IncidentService.GetItems(x => x.statusId == NOTIFICATION.CLOSED_INCIDENT);


        [HttpGet("UserIncidentsRequests")]
        [ServiceFilter(typeof(LogAction))]
        [Authorize]
        public async Task<IActionResult> UserIncidentsRequests()
        {
            try
            {
                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user = await usersService.GetByUsername(username);

                var assignments = (await ServiceFactory.ServicOf<EntityAssignment, EntityAssignment>()
                    .GetItems(x => x.User.Id == user.Id && !x.IsHandeled)).ToList();
                var ids = assignments.Select(x => x.IncidentId).ToList();

                ExpressionStarter<IncidentView> predicateBuilder = PredicateBuilder.False<IncidentView>();
                predicateBuilder.Or(incidentView =>
                    incidentView.createdById == user.Id && incidentView.statusId == NOTIFICATION.EDIT_INCIDENT);
                predicateBuilder.Or(incidentView =>
                    ids.Any(assignment => assignment == incidentView.id) &&
                    incidentView.statusId != NOTIFICATION.EDIT_INCIDENT);

                var list = await IncidentService.GetItems(predicateBuilder);
                return Ok(SuccessResponse<IncidentView>.build(null, 0, list));
                ;
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }


        [HttpGet("handledNotifications")]
        [ServiceFilter(typeof(LogAction))]
        [ClaimsAuth(Type: TYPES.NOTIFICATIONS, Value: VALUES.VIEW)]
        public async Task<IActionResult> getHandledNotifications()
        {
            try
            {
                var incidentViews = await IncidentService.GetItems(x =>
                    x.statusId == NOTIFICATION.IGNORED_NOTIFICATION || x.statusId == NOTIFICATION.CLOSED_NOTIFICATION);
                return Ok(incidentViews);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        [HttpGet("closed_notification")]
        [ServiceFilter(typeof(LogAction))]
        [ClaimsAuth(Type: TYPES.NOTIFICATIONS, Value: VALUES.VIEW)]
        public async Task<List<IncidentView>> GetClosedNotification()
            => await IncidentService.GetItems(x => x.statusId == NOTIFICATION.CLOSED_NOTIFICATION);


        [HttpGet("incidentData")]
        public object GetData()
        {
            return new
            {
                Saverity = ServiceFactory.GetConstantList<Saverity>().Where(x => x.Id != 1).ToList(),
                Category = ServiceFactory.GetConstantList<Category>(),
                Urgancey = ServiceFactory.GetConstantList<Urgancey>(),
            };
        }

        // GET: api/Incident/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<Incident> Get(int id)
        {
            return await IncidentService.Find(id);
        }

        [HttpGet("GetComments")]
        [Authorize]
        public async Task<List<IncidentComment>> GetComments(int incident)
        {
            try
            {
                Incident inc = await IncidentService.Find(incident);
                if (inc.Comments == null)
                    return new List<IncidentComment>();
                return inc.Comments.ToList();
            }
            catch (Exception e)
            {
                return new List<IncidentComment>();
            }
        }


        [HttpGet("incidentsByTask")]
        [Authorize]
        public async Task<IActionResult> getRelatedIncidents(int id)
        {
            try
            {
                return Ok(SuccessResponse<Incident>.build(await IncidentService.Find(id), 0, null));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }


        // POST: api/Incident
        [HttpPost]
        [ServiceFilter(typeof(LogAction))]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] Incident pIncident)
        {
            try
            {
                foreach (var att in pIncident.Attachments)
                {
                    string v = fileHandler.UploadFile(att.Attachment);
                    att.Attachment.Is64base = false;
                    att.Attachment.Url = v;
                    att.Attachment.Content = null;
                }

                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user = await usersService.GetByUsername(username);

                pIncident.CreatedById = user.Id;
                pIncident.SaverityId = pIncident.Saverity.Id;
                pIncident.CategoryId = pIncident.Category.Id;
                pIncident.AptId = pIncident.Apt.Id;
                pIncident.LastUpdateDate = DateTime.Now;
                pIncident.UrganceyId = pIncident.Urgancey.Id;
                pIncident.Saverity = null;
                pIncident.Category = null;
                pIncident.Apt = null;
                pIncident.Urgancey = null;
                if (pIncident.Orgs.Count > 0)
                {
                    pIncident.Orgs = pIncident.Orgs
                        .Select(org => new OrgsIncidentRel { OrganizationId = org.Organization.Id }).ToList();
                }

                ChangeLogHelper.AddChangeLogToEntity(pIncident, user.Id,
                    new[] { new Change() { OldValue = "قيد  الإنشاء", newValue = "تم الإنشاء", Field = "الحالة" } }
                        .ToList());
                long status;

                if (pIncident.Orgs.Any(x => x.OrganizationId == configuration.NonSpacifiedOrgId))
                {
                    status = NOTIFICATION.SEND_TO_VARIFY;
                    pIncident.IsIpsIdentificationRequested = true;
                    pIncident.IpAddresses.ForEach(ip => ip.IsRequestVarify = true);
                    ChangeLogHelper.AddChangeLogToEntity(pIncident, configuration.SystemUserId,
                        new[]
                        {
                            new Change()
                            {
                                OldValue = "التحقق من الفئات المستهدفة", newValue = "إرسال إلى قسم التعريف",
                                Field = "الحالة"
                            }
                        }.ToList());
                }
                else
                {
                    status = NOTIFICATION.OPEN_NOTIFICATION;
                }


                EntityStatus estatus = new EntityStatus() { StatusId = status };
                pIncident.Status = estatus;
                pIncident.CreatedBy = null;

                var response = await IncidentService.AddItem(pIncident);
                var claimValue = status == NOTIFICATION.OPEN_NOTIFICATION ? VALUES.VIEW : VALUES.VARFIY;
                var claimType = status == NOTIFICATION.OPEN_NOTIFICATION
                    ? TYPES.NOTIFICATIONS
                    : TYPES.IP_IDENTITFICATION;
                await ServiceFactory.NotificationHelper().BuildNotificationForClaim(user.Id, (int)EntityType.Incident,
                    response.Entity.Id, status, claimType, claimValue);
                return Ok(SuccessResponse<Incident>.build(response.Entity, 0));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }


        [HttpPost("Search")]
        [ServiceFilter(typeof(LogAction))]
        [Authorize]
        public async Task<IActionResult> Search([FromBody] SearchModel value)
        {
            try
            {
                long count = 0;
                var exp = isMatch(value);
                if (value.Page.PageNumber == 1)
                    count = (await IncidentService.GetCount(exp));
                var incidentViews =
                    await IncidentService.GetItems( exp,value.Page,null);
                return Ok(SuccessResponse<IncidentView>.build(null, count, incidentViews));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }

        [HttpPost("AttackReport")]
        [ServiceFilter(typeof(LogAction))]
        [Authorize]
        public async Task<IActionResult> AttackReport([FromBody] SearchModel value)
        {
            try
            {
                var statusList = new List<long>()
                {
                    NOTIFICATION.CLOSED_INCIDENT, NOTIFICATION.OPEN_NOTIFICATION, NOTIFICATION.INCIDENT,
                    NOTIFICATION.CLOSED_NOTIFICATION
                };
                List<IncidentView> incidents = await IncidentService
                    .GetItems(v =>
                        v.Date >= value.fromDate && v.Date <= value.toDate && statusList.Any(id => id == v.statusId));
                //incidents = incidents.Where(x => isMatch(value, x)).ToList();
                return Ok(SuccessResponse<IncidentView>.build(null, 0, incidents));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }


        private ExpressionStarter<IncidentView> isMatch(SearchModel value)
        {
            
            ExpressionStarter<IncidentView> predicateBuilder = PredicateBuilder.False<IncidentView>();
            ExpressionStarter<IncidentView> criteria = PredicateBuilder.False<IncidentView>();

            predicateBuilder.Or(v => v.Date >= value.fromDate && v.Date <= value.toDate
                                                              && v.statusId != NOTIFICATION.EDIT_INCIDENT);

            

            List<long> statusList = !String.IsNullOrEmpty(value.status)
                ? value.status.Split(',').Select(x => Int64.Parse(x)).ToList()
                : new List<long>();
            bool status = true;

            if (statusList.Count > 0)
                criteria = criteria.Or(p => statusList.Any(x => x == p.statusId));
                //status = statusList.IndexOf(v.statusId) != -1;

                if (!String.IsNullOrEmpty(value.key))
                {
                    criteria = criteria.Or(p => p.subject.Contains(value.key));
                    criteria = criteria.Or(p => p.OrgName.Contains(value.key));
                }
                
                

            if (!String.IsNullOrEmpty(value.apts))
            {
                var apts = value.apts.Split(',').Select(x => Int64.Parse(x)).ToList();
                criteria = criteria.Or(p => apts.Any(x => x == p.aptId));
            }
            
            if (!String.IsNullOrEmpty(value.sectors))
            {
                var sectors = value.sectors.Split(',');
                Func<IncidentView, bool> func = view => sectors.Intersect(view.sectorId.Split('-')).Any();
                criteria = criteria.Or(p => func.Invoke(p));
            }
            
            if (!String.IsNullOrEmpty(value.orgs))
            {
                
                var orgs = value.orgs.Split(',');
                Func<IncidentView, bool> func = view => orgs.Intersect(view.OrgId.Split('-')).Any();
                criteria = criteria.Or(p => func.Invoke(p));
            }
            predicateBuilder.And(criteria);
            return predicateBuilder;
        }


        [HttpPost("comments")]
        [Authorize]
        public async Task<IActionResult> AddComment([FromBody] Incident value)
        {
            try
            {
                String username = (string)HttpContext.Items[Constants.UserId.ToString()];
                Incident incident = await IncidentService.Find(value.Id);
                var status = NOTIFICATION.ADD_COMMENT;
                IncidentComment incidentComment = value.Comments[0];
                incidentComment.Comment.CreatedDate = DateTime.Now;
                EUser eUser = await usersService.GetByUsername(username);
                incidentComment.Comment.CreatedById = eUser.Id;
                if (incident.Comments == null)
                {
                    incident.Comments = new List<IncidentComment>();
                }

                incident.Comments.Add(incidentComment);
                ChangeLogHelper.AddChangeLogToEntity(incident, eUser.Id,
                    new[] { new Change() { OldValue = "إضافة تعليق", newValue = "إضافة تعليق", Field = "التعليقات" } }
                        .ToList());

                Incident newIncident = await IncidentService.UpdateEntity(incident);
                var lastCommentId = newIncident.Comments[newIncident.Comments.Count - 1].CommentId;
                await ServiceFactory.NotificationHelper().BuildNotification(eUser.Id, status, (int)EntityType.Comment,
                    lastCommentId ?? 0, (int)EntityType.Incident, incident.Id);

                return Ok(SuccessResponse<Incident>.build(null, 0));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }


        [HttpPost("IncidentAssignmentResponse")]
        [Authorize]
        public async Task<IActionResult> IncidentAssignmentResponse([FromBody] AssignModelIncident value)
        {
            try
            {
                var status = NOTIFICATION.REQUEST_RESPONSE;
                String username = (string)HttpContext.Items[Constants.UserId.ToString()];
                EUser eUser = await usersService.GetByUsername(username);
                var Incident = await IncidentService.Find(value.Incident);
                foreach (EntityAssignment assignment in Incident.Assignments)
                {
                    if (assignment.Id == value.AssignmentId)
                    {
                        assignment.Response = value.Response;
                        assignment.LastUpdateDate = DateTime.Now;
                        assignment.IsHandeled = true;
                    }
                }

                ChangeLogHelper.AddChangeLogToEntity(Incident, eUser.Id,
                    new[]
                    {
                        new Change()
                            { OldValue = "إضافة قسيمة إجراء", newValue = "إضافة قسيمة إجراء", Field = "قسيمة إجراء" }
                    }.ToList());
                await IncidentService.UpdateEntity(Incident);
                await ServiceFactory.NotificationHelper()
                    .BuildNotification(eUser.Id, status, (int)EntityType.Incident, Incident.Id);

                return Ok(SuccessResponse<Incident>.build(null, 0));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }


        [HttpPost("addUsersToAssignment")]
        [Authorize]
        public async Task<IActionResult> addUsersToInciednt([FromBody] AssignModelIncident value)
        {
            try
            {
                String username = (string)HttpContext.Items[Constants.UserId.ToString()];
                EUser eUser = await usersService.GetByUsername(username);
                var Incident = await IncidentService.Find(value.Incident);
                List<EUser> Users = value.Users.Select(x => usersService.GetById(x)).ToList();
                var status = NOTIFICATION.ASSIGN_INCIDENT;
                string usersString = string.Join(",", Users.Select(x => x.FullName).ToList());
                List<EntityAssignment> assignments = value.Users.Select(x => new EntityAssignment()
                {
                    CreatedDate = DateTime.Now, Request = value.Request, UserId = x, CreatedById = eUser.Id,
                    Status = Incident.Status
                }).ToList();
                ChangeLogHelper.AddChangeLogToEntity(Incident, eUser.Id,
                    new[]
                    {
                        new Change()
                            { OldValue = "إضافة موظف", newValue = "إضافة موظف", Field = " إضافة" + usersString }
                    }.ToList());
                Incident.Assignments.AddRange(assignments);
                await IncidentService.UpdateEntity(Incident);
                await ServiceFactory.NotificationHelper().BuildNotificationForUsers(eUser.Id, value.Users, status,
                    (int)EntityType.Incident, Incident.Id);

                return Ok(SuccessResponse<Incident>.build(null, 0));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpPost("addReplay")]
        [Authorize]
        public async Task<IActionResult> addReplay([FromBody] Comment value)
        {
            try
            {
                DbServiceImpl<Comment, Comment> CommentService = ServiceFactory.ServicOf<Comment, Comment>();
                String username = (string)HttpContext.Items[Constants.UserId.ToString()];
                EUser eUser = await usersService.GetByUsername(username);
                var status = NOTIFICATION.ADD_COMMENT;
                Comment parentComment = await CommentService.Find(value.Id);
                Comment reply = value.Replaies[value.Replaies.Count - 1];
                reply.CreatedBy = eUser;
                reply.CreatedDate = DateTime.Now;
                parentComment.Replaies.Add(reply);
                await CommentService.UpdateEntity(parentComment);
                var parentId = parentComment.IncidentComments[0].IncidentId;
                await ServiceFactory.NotificationHelper().BuildNotification(eUser.Id, status, (int)EntityType.Comment,
                    parentComment.Id, (int)EntityType.Incident, parentId ?? 0);
                return Ok(SuccessResponse<Incident>.build(null, 0));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }


        [HttpPost("closeIncident")]
        [ServiceFilter(typeof(LogAction))]
        [Authorize]
        public async Task<IActionResult> closeIncident([FromBody] CloseReport report)
        {
            try
            {
                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user = await usersService.GetByUsername(username);
                var entity = await IncidentService.Find(report.reportId);
                var newState = NOTIFICATION.CLOSED_INCIDENT;
                report.CreatedById = user.Id;
                report.CreatedDate = DateTime.Now;
                entity.CloseReport = report;
                entity.Status = new EntityStatus() { StatusId = newState };
                ChangeLogHelper.AddChangeLogToEntity(entity, user.Id,
                    new[]
                    {
                        new Change() { OldValue = "حادثة قيد الإجراء", newValue = "حادثة مغلقة", Field = "الحالة" },
                        new Change() { OldValue = "لا يوجد", newValue = "إضافة تقرير الإغلاق", Field = "تقرير إغلاق" }
                    }.ToList());
                await IncidentService.UpdateEntity(entity);
                await ServiceFactory.NotificationHelper()
                    .BuildNotification(user.Id, newState, (int)EntityType.Incident, entity.Id);
                return Ok(SuccessResponse<Incident>.build(null, 0, null));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpPost("changeIncidentStatus")]
        [ServiceFilter(typeof(LogAction))]
        [Authorize]
        public async Task<IActionResult> changeStatus([FromBody] ChangesModel value)
        {
            try
            {
                var user = await usersService.GetByUsername((string)HttpContext.Items[Constants.UserId.ToString()]);
                var status = value.changeType;
                var incident = await IncidentService.Find(value.id);
                incident.Status = new EntityStatus() { StatusId = status };
                incident.ExtraNote1 = value.reasonString;
                incident.LastUpdateById = user.Id;
                incident.LastUpdateDate = DateTime.Now;
                if (value.changeType ==
                    NOTIFICATION
                        .INCIDENT) // if you change this value , please consider changing it on the client side also
                    ChangeLogHelper.AddChangeLogToEntity(incident, user.Id,
                        new[]
                        {
                            new Change() { OldValue = "تنبيه", newValue = "تحويل إلى حادث", Field = "نوعية التنبيه" }
                        }.ToList());
                else if (value.changeType == NOTIFICATION.IGNORED_NOTIFICATION)
                    ChangeLogHelper.AddChangeLogToEntity(incident, user.Id,
                        new[]
                        {
                            new Change() { OldValue = "لا يوجد", newValue = value.reasonString, Field = "ملاحظات" },
                            new Change() { OldValue = "تنبيه", newValue = "تنبيه متجاهل", Field = "نوعية التنبيه" }
                        }.ToList());
                else if (value.changeType == NOTIFICATION.CLOSED_NOTIFICATION)
                    ChangeLogHelper.AddChangeLogToEntity(incident, user.Id,
                        new[]
                        {
                            new Change() { OldValue = "لا يوجد", newValue = value.reasonString, Field = "ملاحظات" },
                            new Change() { OldValue = "تنبيه", newValue = "تنبيه مغلق", Field = "نوعية التنبيه" }
                        }.ToList());

                await ServiceFactory.NotificationHelper()
                    .BuildNotification(user.Id, status, (int)EntityType.Incident, incident.Id);
                await IncidentService.UpdateEntity(incident);
                return Ok(SuccessResponse<Incident>.build(null, 0, null));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpPost("category")]
        public IActionResult CreateCategory([FromBody] Category category)
        {
            try
            {
                ServiceFactory.AddConstant(category);
                return Ok(SuccessResponse<Category>.build(null, 0, ServiceFactory.GetConstantList<Category>()));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpGet("categories")]
        public IEnumerable<Category> All() => ServiceFactory.GetConstantList<Category>();


        [HttpPost("EditRequest")]
        [ServiceFilter(typeof(LogAction))]
        [Authorize]
        public async Task<IActionResult> EditRequest([FromBody] Incident newValu)
        {
            try
            {
                Incident oldValue = await IncidentService.Find(newValu.Id);

                foreach (var att in newValu.Attachments)
                {
                    string v = fileHandler.UploadFile(att.Attachment);
                    att.Attachment.Is64base = false;
                    att.Attachment.Url = v;
                    att.Attachment.Content = null;
                }

                foreach (var id in newValu.DeletedAttachments)
                {
                    await fileHandler.DeleteAttachment(id);
                }

                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user = await usersService.GetByUsername(username);
                var status = NOTIFICATION.OPEN_NOTIFICATION;


                oldValue.Attachments ??= new List<IncidentAttachment>();
                oldValue.Attachments.AddRange(newValu.Attachments);
                oldValue.Subject = newValu.Subject;
                oldValue.Description = newValu.Description;
                oldValue.Date = newValu.Date;
                oldValue.AptId = newValu.AptId;
                oldValue.Time = newValu.Time;
                oldValue.IsIpsIdentificationRequested = false;
                oldValue.Status = new EntityStatus() { StatusId = status };
                oldValue.Signature = newValu.Signature;
                oldValue.LastUpdateDate = DateTime.Now;
                oldValue.SaverityId = newValu.Saverity.Id;
                oldValue.CategoryId = newValu.Category.Id;
                oldValue.IpAddresses = newValu.IpAddresses;
                oldValue.UrganceyId = newValu.Urgancey.Id;
                ChangeLogHelper.AddChangeLogToEntity(oldValue, user.Id,
                    new[] { new Change() { OldValue = "إنتظار التعديل", newValue = "تم التعديل", Field = "الحالة" } }
                        .ToList());
                if (oldValue.Orgs.Count > 0)
                    oldValue.Orgs = newValu.Orgs
                        .Select(org => new OrgsIncidentRel { OrganizationId = org.Organization.Id }).ToList();
                Incident incident = await IncidentService.UpdateEntity(oldValue);
                await ServiceFactory.NotificationHelper()
                    .BuildNotification(user.Id, status, (int)EntityType.Incident, incident.Id);
                return Ok(SuccessResponse<Incident>.build(incident, 0));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }
    }
}