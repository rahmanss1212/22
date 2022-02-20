using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Events.Api.Models.UserManagement;
using Events.Core.Models.General;
using Events.Core.Models.Incidents;
using Events.Core.Models.Reports;
using Events.Data;
using Events.Service.Files;
using Events.Service.Service;
using Events.Service.Service.DataServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagment.services;

namespace Events.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralReportController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly DbServiceImpl<GeneralReport, GeneralReportDataView> _generalReportService;
        private readonly DbServiceImpl<Status, Status> _statusService;
        private readonly IFileHandler fileHandler;
        private readonly IMapper<GeneralReport, GeneralReportDataView> _generalReportMapper;
        private readonly AppDbContext _ctx;
        private readonly IServiceFactory _serviceFactory;

        public GeneralReportController(IServiceFactory service, IUserService userService,
            IMapper<GeneralReport, GeneralReportDataView> mapper, AppDbContext ctx, IFileHandler fileHandler)
        {
            _ctx = ctx;
            _serviceFactory = service;
            _userService = userService;
            this.fileHandler = fileHandler;
            _generalReportService = service.ServicOf<GeneralReport, GeneralReportDataView>();
            _statusService = service.ServicOf<Status, Status>();
            _generalReportMapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int pageSize, int pageNumber)
        {
            try
            {
                try
                {
                    PaggingModel page = new PaggingModel() { PageNumber = pageNumber, PageSize = pageSize };
                    string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                    var user = await _userService.GetByUsername(username);
                    var assignments =
                        (await _serviceFactory.ServicOf<GReportEntityAssignment, GReportEntityAssignment>()
                            .GetItems(x => x.UserId == user.Id && !x.IsHandeled)).Select(x => x.GeneralReportId)
                        .ToList();
                    var list = await _generalReportService.GetItems(x => assignments.Any(v => v == x.Id), page);
                    return Ok(SuccessResponse<GeneralReportDataView>.build(null, 0, list));
                    ;
                }
                catch (Exception e)
                {
                    return Ok(FailedResponse.Build(e.Message));
                }
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpGet("getReportById")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                GeneralReport generalReport = await _generalReportService.Find(id);
                return Ok(SuccessResponse<GeneralReport>.build(generalReport, 0, null));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpPost("AddGeneralReport")]
        public async Task<IActionResult> AddGeneralReport([FromBody] GeneralReport newGeneralReport)
        {
            foreach (var att in newGeneralReport.Attachments)
            {
                string v = fileHandler.UploadFile(att.Attachment);
                att.Attachment.Is64base = false;
                att.Attachment.Url = v;
                att.Attachment.Content = null;
            }

            var username = (string)HttpContext.Items[Constants.UserId.ToString()];
            var status = GENERAL_REPORT.OPEN_REPORT;
            var user = await _userService.GetByUsername(username);
            newGeneralReport.CreatedById = user.Id;
            newGeneralReport.CreatedDate = DateTime.Now.Date;
            newGeneralReport.UrganceyId = newGeneralReport.Urgancey.Id;
            newGeneralReport.SaverityId = newGeneralReport.Saverity.Id;
            newGeneralReport.Urgancey = null;
            newGeneralReport.Saverity = null;
            newGeneralReport.StatusId = status;
            var status2 = NOTIFICATION.ASSIGN_REPORT;
            var usersIds = newGeneralReport.Assignments.Select(x => x.User.Id).ToList();
            List<GReportEntityAssignment> assignments = usersIds.Select(x => new GReportEntityAssignment()
            {
                CreatedDate = DateTime.Now, Request = "تقرير جديد للإطلاع", UserId = x, CreatedById = user.Id,
                StatusId = status2
            }).ToList();
            newGeneralReport.Assignments = assignments;
            var response = await _generalReportService.AddItem(newGeneralReport);
            await _serviceFactory.NotificationHelper().BuildNotificationForUsers(user.Id, usersIds, status2,
                (int)EntityType.Report, response.Entity.Id);
            return Ok(SuccessResponse<GeneralReport>.build(response.Entity));
        }

        [HttpGet("fetchReportCategories")]
        public IList<ReportCategory> GetCategories() => _serviceFactory.GetConstantList<ReportCategory>();

        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory([FromBody] ReportCategory newReportCategory)
        {
            var reportCategory = await _generalReportService.AddConstant(newReportCategory);
            return Ok(SuccessResponse<ReportCategory>.build(null));
        }

        [HttpPost("addUsersToReport")]
        [Authorize]
        public async Task<IActionResult> addUsersToReport([FromBody] AssignModelIncident value)
        {
            try
            {
                String username = (string)HttpContext.Items[Constants.UserId.ToString()];
                EUser eUser = await _userService.GetByUsername(username);
                var report = await _generalReportService.Find(value.Incident);
                var status = NOTIFICATION.ASSIGN_REPORT;
                List<GReportEntityAssignment> assignments = value.Users.Select(x => new GReportEntityAssignment()
                {
                    CreatedDate = DateTime.Now, Request = value.Request, UserId = x, CreatedById = eUser.Id,
                    Status = report.Status
                }).ToList();
                report.Assignments.AddRange(assignments);
                await _generalReportService.UpdateEntity(report);
                await _serviceFactory.NotificationHelper().BuildNotificationForUsers(eUser.Id, value.Users, status,
                    (int)EntityType.Report, report.Id);

                return Ok(SuccessResponse<Incident>.build(null, 0));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpPost("ReportResponse")]
        [Authorize]
        public async Task<IActionResult> ReportResponse([FromBody] AssignModelIncident value)
        {
            try
            {
                String username = (string)HttpContext.Items[Constants.UserId.ToString()];
                EUser eUser = await _userService.GetByUsername(username);
                var report = await _generalReportService.Find(value.Incident);
                foreach (GReportEntityAssignment assignment in
                         report.Assignments.Where(x => x.Id == value.AssignmentId))
                {
                    assignment.Response = value.Response;
                    assignment.LastUpdateDate = DateTime.Now;
                    assignment.IsHandeled = true;
                }

                if (value.Users.Count > 0)
                {
                    var status = NOTIFICATION.ASSIGN_REPORT;
                    List<GReportEntityAssignment> assignments = value.Users.Select(x => new GReportEntityAssignment()
                    {
                        CreatedDate = DateTime.Now, Request = value.Request, UserId = x, CreatedById = eUser.Id,
                        Status = report.Status
                    }).ToList();
                    report.Assignments.AddRange(assignments);
                }

                await _generalReportService.UpdateEntity(report);
                return Ok(SuccessResponse<Incident>.build(null, 0));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpGet("UserReportRequests")]
        [Authorize]
        public async Task<IActionResult> UserReportRequests()
        {
            try
            {
                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user = await _userService.GetByUsername(username);
                var assignments =
                    (await _serviceFactory.ServicOf<GReportEntityAssignment, GReportEntityAssignment>()
                        .GetItems(x => x.User.Id == user.Id && !x.IsHandeled)).Select(x => x.GeneralReportId).ToList();
                var list = await _generalReportService.GetItems(x => assignments.Any(v => v == x.Id));
                return Ok(SuccessResponse<GeneralReportDataView>.build(null, 0, list));
                ;
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }
    }
}