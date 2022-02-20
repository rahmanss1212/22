using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Events.Api.Models.APTs;
using Events.Api.Models.General;
using Events.Api.Models.Tasks;
using Events.Api.Models.UserManagement;
using Events.Core.Models.Employees;
using Events.Core.Models.General;
using Events.Core.Models.Incidents;
using Events.Core.Models.Logging;
using Events.Core.Models.Tasks;
using Events.Core.Models.ViewModels;
using Events.Service.Files;
using Events.Service.Service;
using Events.Service.Service.DataServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagment.services;
using Task = Events.Core.Models.Tasks.Task;

namespace EventsManagemtns.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {

        private readonly DbServiceImpl<Task, Taskview> TaskService;
        private readonly DbServiceImpl<Status, Status> StatusService;
        private readonly IUserService UserService;
        private IChangeLogHelper ChangeLogHelper;
        private readonly IServiceFactory ServiceFactory;
        private IFileHandler fileHandler;
        public TasksController(IUserService us, IServiceFactory serFactory, IFileHandler fileHandler)
        {
            UserService = us;
            ServiceFactory = serFactory;
            this.fileHandler = fileHandler;
            TaskService = serFactory.ServicOf<Task, Taskview>();
            ChangeLogHelper = ServiceFactory.ChangeLogHelper();
            StatusService = ServiceFactory.ServicOf<Status, Status>();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                Task task = await TaskService.Find(id);
                return Ok(SuccessResponse<Task>.build(task, 0, null));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }

        }

        // GET api/<TasksController>/5
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {

            try
            {
                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user = await UserService.GetByUsername(username);
                var results = await TaskService.GetItems(x => x.asignedforid == user.Id && (x.statusId == TASK.IN_PROGRESS
                    || x.statusId == TASK.OPEN));


                return Ok(SuccessResponse<Taskview>.build(null, 0, results));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }
        
        [HttpGet("GetLateTasksOfDepartment")]
        [Authorize]
        public async Task<IActionResult> GetLateTasksOfDepartment(long departmentId)
        {

            try
            {
                var results = await TaskService.GetItems(x => x.assignedForDepartmentId == departmentId && (x.statusId == TASK.IN_PROGRESS
                    || x.statusId == TASK.OPEN) && DateTime.Compare(DateTime.Parse(x.dueDate),DateTime.Now) > 0);


                return Ok(SuccessResponse<Taskview>.build(null, 0, results));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpGet("getClosedTasks")]
        [Authorize]
        public async Task<IActionResult> getClosedTasks()
        {

            try
            {
                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user =await UserService.GetByUsername(username);
                var results =await TaskService.GetItems(x => x.asignedforid == user.Id && x.statusId == TASK.CLOSED);
                return Ok(SuccessResponse<Taskview>.build(null, 0, results));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpPost("SaecrhAllTasks")]
        [Authorize]
        public async Task<IActionResult> GetAllTasks([FromBody] SearchModel value)
        {

            try
            {
                var date = DateTime.Now.TimeOfDay;
                value.toDate = value.toDate.Add(date).AddHours(1);
                List<Taskview> tasks = await TaskService
                    .GetItems(v => v.createdDate.Date >= value.fromDate && v.createdDate.Date <= value.toDate);
                    tasks = tasks.Where(x => isMatch(value, x)).ToList();
                return Ok(SuccessResponse<Taskview>.build(null, 0, tasks));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }


        [HttpGet("Taskview")]
        [Authorize]
        public async Task<IActionResult> GetTaskview()
        {
            try
            {
                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user =await UserService.GetByUsername(username);
                var results =await TaskService.GetItems(x => x.createdById == user.Id && x.statusId != TASK.CLOSED);

                return Ok(SuccessResponse<Taskview>.build(null, 0, results));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpGet("fetchAllTasks")]
        [Authorize]
        public async Task<IActionResult> fetchAllTasks()
        {
            try
            {
                var results =await TaskService.GetItems(x => x.statusId != TASK.CLOSED);
                return Ok(SuccessResponse<Taskview>.build(null, 0, results));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpGet("TasksByOwner")]
        [Authorize]
        public async Task<IActionResult> fetchTasksByOwner()
        {
            try
            {
                var results =await TaskService.GetItems(x => x.statusId != TASK.CLOSED);
                return Ok(SuccessResponse<Taskview>.build(null, 0, results));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }



        [HttpGet("GetTasksByDepartment")]
        [Authorize]
        public async Task<IActionResult> GetTasksByDepartment()
        {
            try
            {
                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user =await UserService.GetByUsername(username);

                if (!user.IsHead && !user.IsSubHead && !user.IsAssignedHead)
                    return Ok(FailedResponse.Build("المستخدم ليس رئيس قسم"));

                var results =await TaskService.GetItems(v => v.assignedForDepartmentId == user.Section.Department.Id || v.createdByDepartmentId == user.Section.Department.Id);
                return Ok(SuccessResponse<Taskview>.build(null, 0, results));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }


        [HttpGet("subTasks")]
        [Authorize]
        public async Task<IActionResult> GetSubTask(long parentTask)
        {

            try
            {
                var results =await TaskService.GetItems(x => x.parentTaskid == parentTask);
                return Ok(SuccessResponse<Taskview>.build(null, 0, results));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpGet("relatedTask")]
        [Authorize]
        public async Task<IActionResult> getRelatedTask(long incident)
        {
            try
            {
               
                var results =await TaskService.GetItems(x => x.parentIncidentid == incident);
                return Ok(SuccessResponse<Taskview>.build(null, 0, results));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }

        }

        [HttpGet("taskTypes")]
        [Authorize]
        public IActionResult GetTaskType()
        {
            try
            {
                List<TaskType> lists = ServiceFactory.GetConstantList<TaskType>();
                return Ok(SuccessResponse<TaskType>.build(null, 0, lists));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }

        }
        [HttpGet("TaskEmpsRel")]
        [Authorize]
        public IActionResult TaskEmp()
        {
            try
            {
                List<Department> lists = ServiceFactory.UseContextForListReterive(u => u.Departments
                 .Include(x => x.Sections).ThenInclude(x => x.Tasks).ThenInclude(x => x.Status)
                 .ToList());


                return Ok(SuccessResponse<Department>.build(null, 0, lists));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }

        }



        [HttpPost("SearchAssignedTasks")]
        [Authorize]
        public async Task<IActionResult> SearchAssignedTasks([FromBody] SearchModel value)
        {
            try
            {
                var date = DateTime.Now.TimeOfDay;
                value.toDate = value.toDate.Add(date).AddHours(1);
                
                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user =await UserService.GetByUsername(username);
                List<Taskview> tasks = await TaskService
                    .GetItems(v => v.asignedforid == user.Id && v.createdDate.Date >= value.fromDate && v.createdDate.Date <= value.toDate);
                    tasks = tasks.Where(x => isMatch(value, x)).ToList();
                return Ok(SuccessResponse<Taskview>.build(null, 0, tasks));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }



        [HttpPost("SearchOwnedTasks")]
        [Authorize]
        public async Task<IActionResult> SearchTasks([FromBody] SearchModel value)
        {
            try
            {
                var date = DateTime.Now.TimeOfDay;
                value.toDate = value.toDate.Add(date).AddHours(1);
                
                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user =await UserService.GetByUsername(username);
                List<Taskview> tasks = await TaskService
                    .GetItems(v => v.createdById == user.Id && v.createdDate.Date >= value.fromDate
                     && v.createdDate.Date <= value.toDate);
                   tasks = tasks.Where(x => isMatch(value, x)).ToList();
                return Ok(SuccessResponse<Taskview>.build(null, 0, tasks));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpPost("SearchDepartmentTasks")]
        [Authorize]
        public async Task<IActionResult> SearchDepartmentTasks([FromBody] SearchModel value)
        {
            try
            {
                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user =await UserService.GetByUsername(username);
                
                var date = DateTime.Now.TimeOfDay;
                value.toDate = value.toDate.Add(date).AddHours(1);
                

                List<Taskview> tasks = await TaskService
                    .GetItems(v => v.createdDate >= value.fromDate && v.createdDate <= value.toDate);
                    tasks = tasks.Where(v => (v.assignedForDepartmentId == user.Section.Department.Id || v.createdByDepartmentId == user.Section.Department.Id) && isMatch(value, v)).ToList();
                    return Ok(SuccessResponse<Taskview>.build(null, 0, tasks));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpPost("SearchAllTasks")]
        [Authorize]
        public async Task<IActionResult> SearchAllTasks([FromBody] SearchModel value)
        {
            try
            {
                var date = DateTime.Now.TimeOfDay;
                value.toDate = value.toDate.Add(date).AddHours(1);
                
                List<Taskview> tasks =await TaskService
                    .GetItems(v => v.createdDate >= value.fromDate && v.createdDate <= value.toDate );
                    tasks = tasks.Where(x => isMatch(value, x)).ToList();
                return Ok(SuccessResponse<Taskview>.build(null, 0, tasks));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }
        private bool isMatch(SearchModel value, Taskview v)
        {

            List<long> statusList = !value.status.Equals(String.Empty) ?
                value.status.Split(',').Select(x => Int64.Parse(x)).ToList() : new List<long>();

            List<long> departments = !(value.departments is null) ?  !value.departments.Equals(String.Empty) ? 
                value.departments.Split(',').Select(x => Int64.Parse(x)).ToList() : new List<long>() :new List<long>();

            List<long> employees = value.employees != null && !value.employees.Equals(String.Empty) ?
                           value.employees.Split(',').Select(x => Int64.Parse(x)).ToList() : new List<long>();
            


            bool status = true;
            if (statusList.Count > 0)
                status = statusList.IndexOf(v.statusId) != -1;

            if (!status)
                return status;
            
            if (departments.Count > 0)
                status = departments.IndexOf(v.assignedForDepartmentId) != -1;

            if (!status)
                return status;
            

            if (employees.Count > 0)
                status = employees.IndexOf(v.asignedforid) != -1;

            
            if (!status)
                return status;

            
            if (!value.key.Equals(String.Empty))
                status = v.title.Contains(value.key) || v.description.Contains(value.key);

            return status;
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] Task value)
        {
            try
            {
                Incident incident = new Incident();

                List<TaskAttachments> attachments = value.Attachments;
                foreach (var att in attachments)
                {

                    string v = fileHandler.UploadFile(att.attachment);
                    att.attachment.Is64base = false;
                    att.attachment.Url = v;
                    att.attachment.Content = null;
                }

                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user =await UserService.GetByUsername(username);
                value.CreatedById = user.Id;
                value.CreatedDate = DateTime.Now;
                value.LastUpdateDate = DateTime.Now;
                value.TaskTypeId = value.TaskType.Id;
                value.StatusId =TASK.OPEN;
                if (value.ParentTask.Id != 0)
                {
                    Task task = await TaskService.Find(value.ParentTask.Id);
                    value.ParentTaskId = value.ParentTask.Id;
                    ChangeLog changeLog2 = ChangeLog.Build(user.Id);
                    changeLog2.addChangeField(new ChangeLogField()
                    {
                        FieldName = "المهام الفرعية",
                        OldValue = "",
                        NewValue = "إضافة مهمة فرعية"
                    });


                    if (task.Changes == null)
                        task.Changes = new List<ChangeLog>();
                }
                

                if (value.ParentIncident.Id != 0)
                {
                    value.ParentIncidentId = value.ParentIncident.Id;
                }
                
                value.ParentIncident = null;
                value.ParentTask = null;

                ServiceFactory.ChangeLogHelper().AddChangeLogToEntity(value, user.Id, new[] { new Change() { OldValue = "تحت الإنشاء", newValue = "إنشاء مهمة جديد", Field = "الحالة" } }.ToList());
                value.Assigned_forId = value.Assigned_for.Id;
                value.Assigned_for = null;
                value.Status = null;
                value.TaskType = null;
                value.AssignedEmps = value.AssignedEmps.Select(emp => new TaskEmpsRel() { EUserId = emp.EUser.Id }).ToList();
                var response =  await TaskService.AddItem(value);
                value.AssignedEmps.Select(x => x.EUser);
                await ServiceFactory.NotificationHelper().BuildNotificationForUsers(user.Id, value.AssignedEmps.Select(x => x.EUserId??0).ToList(),
                    value.StatusId, (int)EntityType.Task, response.Entity.Id);

                if (incident.Id != 0)
                    await ServiceFactory.ChangeLogHelper().AddChangeLogToDb<Incident, IncidentView>(incident, user.Id, new[] { new Change() { OldValue = "إضافة مهمة متعلقة", newValue = "إضافة مهمة متعلقة", Field = "إسناد مهمة" } }.ToList());

                return Ok(SuccessResponse<Task>.build(null, 0, null));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }

        }



        [HttpPost("UpdateTask")]
        [Authorize]
        public async System.Threading.Tasks.Task<IActionResult> UpdateTask([FromBody] Task newValue)
        {
            try
            {
                
                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user =await UserService.GetByUsername(username);

         
                
                foreach (var att in newValue.Attachments)
                {

                    string v = fileHandler.UploadFile(att.attachment);
                    att.attachment.Is64base = false;
                    att.attachment.Url = v;
                    att.attachment.Content = null;
                }

                foreach (var id in newValue.DeletedAttachments)
                {
                    await fileHandler.DeleteAttachment(id);
                }
                
                

                var oldValue = await TaskService.Find(newValue.Id);
                oldValue.Attachments ??= new List<TaskAttachments>();
                oldValue.Attachments.AddRange(newValue.Attachments);
                oldValue.LastUpdateDate = DateTime.Now.Date;
                oldValue.Description = newValue.Description;
                oldValue.Title = newValue.Title;
                oldValue.Urgent = newValue.Urgent;
                oldValue.Weight = newValue.Weight;
                oldValue.Importance = newValue.Importance;
                oldValue.TaskTypeId = newValue.TaskType.Id;
                ServiceFactory.ChangeLogHelper().AddChangeLogToEntity(oldValue, user.Id, new[] { new Change() { OldValue = "قيد التعديل", newValue = "تم التعديل", Field = "الحالة" } }.ToList());
                oldValue.AssignedEmps = newValue.AssignedEmps.Select(emp => new TaskEmpsRel() { EUserId = emp.EUser.Id }).ToList();
                Task task = await TaskService.UpdateEntity(oldValue);


                return Ok(SuccessResponse<Task>.build(null, 0, null));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }

        }


        // PUT api/<TasksController>
        [HttpPut("updateStatus")]
        [Authorize]
        public async System.Threading.Tasks.Task<IActionResult> Put(long statusId, long id)
        {
            try
            {
                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user =await UserService.GetByUsername(username);
                var entity = await TaskService.Find(id);
                var st =await StatusService.Find(statusId);
                entity.Status = st;
                if (st.Id == TASK.CLOSED)
                    entity.Progress = 100;
                await ServiceFactory.ServicOf<Task, Taskview>().UpdateEntity(entity);
                return Ok(SuccessResponse<Task>.build(null, 0, null));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpPost("AddTaskComments")]
        [Authorize]
        public async System.Threading.Tasks.Task<IActionResult> AddTaskComments([FromBody] CommentViewModel value)
        {

            try
            {
                List<CommentAttachment> commentAttachments = new List<CommentAttachment>();
                List<Attachment> attachments = value.attachments;
                foreach (var att in attachments)
                {

                    string v = fileHandler.UploadFile(att);
                    att.Is64base = false;
                    att.Url = v;
                    att.Content = null;
                    commentAttachments.Add(new CommentAttachment()
                    {
                        attachment = att
                    });

                }
                String username = (string)HttpContext.Items[Constants.UserId.ToString()];
                EUser eUser = await UserService.GetByUsername(username);
                Task task =await TaskService.Find(value.relid);
                var status =  TASK.ADD_COMMENT;
                TaskComment comment = new TaskComment();
                comment.Comment = new Comment();
                comment.Comment.Attachments = commentAttachments;
                comment.Comment.CommentString = value.commentString;
                comment.Comment.CreatedDate = DateTime.Now;
                comment.Comment.CreatedById = eUser.Id;

                if (task.TaskComments == null)
                    task.TaskComments = new List<TaskComment>();
                task.TaskComments.Add(comment);
                 await TaskService.UpdateEntity(task);

                //Task newTask = await ServiceFactory.ServicOf<Task, Taskview>().UpdateEntity(task);
                //var lastCommentId = newTask.TaskComments[newTask.TaskComments.Count - 1].CommentId;
                //await ServiceFactory.NotificationHelper().BuildNotification(eUser, status, (int)EntityType.Comment, lastCommentId, (int)EntityType.Task, task.Id);

                return Ok(SuccessResponse<Task>.build(null, 0, null));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }


        [HttpPost("addReplay")]
        [Authorize]
        public async System.Threading.Tasks.Task<IActionResult> addReplay([FromBody] Comment value)
        {
            try
            {

                String username = (string)HttpContext.Items[Constants.UserId.ToString()];
                Comment parentComment =await ServiceFactory.ServicOf<Comment, Comment>().Find(value.Id);
                Comment reply = value.Replaies[value.Replaies.Count - 1];
                reply.CreatedBy = await UserService.GetByUsername(username);
                reply.CreatedDate = DateTime.Now;

                parentComment.Replaies.Add(reply);
                await ServiceFactory.ServicOf<Comment, Comment>().UpdateEntity(parentComment);
                return Ok(SuccessResponse<Task>.build(null, 0));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }

        }

        [HttpGet("GetTaskComments")]
        [Authorize]
        public async Task<IActionResult> GetTaskComments(long taskId)
        {
            try
            {
                Task task =await TaskService.Find(taskId);
                return Ok(SuccessResponse<Task>.build(task, taskId, null));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }



        [HttpPut("progress")]
        [Authorize]
        public async System.Threading.Tasks.Task<IActionResult> updateProgress(long id, int prog)
        {
            try
            {
                var entity =await TaskService.Find(id);
                entity.Progress = prog;
                await TaskService.UpdateEntity(entity);
                return Ok(SuccessResponse<Task>.build(null, 0));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }
        [HttpPut("status")]
        [Authorize]
        public async System.Threading.Tasks.Task<IActionResult> updateStatus(long id)
        {
            try
            {
                Status status = await StatusService.Find((Int64)TASK.IN_PROGRESS);
                var entity =await TaskService.Find(id);
                entity.Status = status;
                await TaskService.UpdateEntity(entity);
                return Ok(SuccessResponse<Task>.build(null, 0));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpPost("closeTask")]
        [Authorize]
        public async Task<IActionResult> Put([FromBody] CloseReport report)
        {
            try
            {
                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user =await UserService.GetByUsername(username);
                var entity =await TaskService.Find(report.reportId);
                report.CreatedById = user.Id;
                report.LastUpdateById = user.Id;
                report.CreatedDate = DateTime.Now;
                entity.ClosingReport = report;
                entity.Progress = 100;
                entity.StatusId =TASK.CLOSED;
                await TaskService.UpdateEntity(entity);
                return Ok(SuccessResponse<Task>.build(null, 0, null));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }

        }

        [HttpPost("shareTask")]
        [Authorize]
        public async System.Threading.Tasks.Task<IActionResult> ShareTask([FromBody] Task value)
        {
            try
            {
                var task = await TaskService.Find(value.Id);
                var emps = value.AssignedEmps.Select(x => new TaskEmpsRel()
                {
                    EUserId = x.EUser.Id
                }).ToList();

                task.AssignedEmps.AddRange(emps);
                await TaskService.UpdateEntity(task);
                return Ok(SuccessResponse<Task>.build(null, 0, null));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }

        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {

                Task task =await TaskService.Find(id);
                await TaskService.Delete(task);
                return Ok(SuccessResponse<Task>.build(null, 0, null));

            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }

        }
        
        [HttpGet("fetchTaskCategories")]
        public IList<TaskType> GetTaskCategories() => ServiceFactory.GetConstantList<TaskType>();

        [HttpPost("AddTaskCategory")]
        public async System.Threading.Tasks.Task<IActionResult> AddTaskCategory([FromBody] TaskType newTaskCategory)
        {
            var taskCategory = await TaskService.AddConstant(newTaskCategory);
            return Ok(SuccessResponse<Task>.build(null));
        }


        [HttpPost("TaskAssignmentResponse")]
        [Authorize]
        public async System.Threading.Tasks.Task<IActionResult> TaskAssignmentResponse([FromBody] AssignModelTask value)
        {
            try
            {
                var status =NOTIFICATION.REQUEST_RESPONSE;
                var username = (string) HttpContext.Items[Constants.UserId.ToString()];
                var eUser =await UserService.GetByUsername(username);
                var task = await TaskService.Find(value.Task);

                foreach (var assignment in
                    task.TaskAssignments.Where(x => x.Id == value.AssignmentId))
                {
                    assignment.Response = value.Response;
                    assignment.LastUpdateDate = DateTime.Now;
                    assignment.IsHandled = true;
                }

                var assignments = value.Users.Select(x =>
                        new TaskEntityAssignment()
                        {
                            CreatedDate = DateTime.Now,
                            Request = value.Request,
                            UserId = x,
                            CreatedById = eUser.Id,
                        })
                    .ToList();

                var changes = new[]
                    {
                        new Change{OldValue = "إضافة قسيمة إجراء", newValue = "إضافة قسيمة إجراء", Field = "قسيمة إجراء"}
                    }.ToList();
                ChangeLogHelper.AddChangeLogToEntity(task, eUser.Id,changes);
                task.TaskAssignments.AddRange(assignments);
                await TaskService.UpdateEntity(task);
                await ServiceFactory.NotificationHelper()
                    .BuildNotification(eUser.Id, status, (int) EntityType.Task, task.Id);

                return Ok(SuccessResponse<Task>.build(null));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpGet("UserTasksRequests")]
        [Authorize]
        public async Task<IActionResult> UserTasksRequests()
        {
            try
            {
                var username = (string) HttpContext.Items[Constants.UserId.ToString()];
                var user =await UserService.GetByUsername(username);
                var assignments =await ServiceFactory.ServicOf<TaskEntityAssignment, TaskEntityAssignment>()
                    .GetItems(x => x.User.Id == user.Id && !x.IsHandled);
                    var tasksIds = assignments.Select(x => x.TaskId).ToList();
                var tasks = await TaskService.GetItems(x => tasksIds.Any(v => v == x.TaskId));
                return Ok(SuccessResponse<Taskview>.build(null, 0, tasks));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }


        [HttpPost("AddUsersToTaskAssignment")]
        [Authorize]
        public async Task<IActionResult> addUsersToTaskAssignment([FromBody] AssignModelTask value)
        {
            try
            {
                var username = (string) HttpContext.Items[Constants.UserId.ToString()];
                var eUser =await UserService.GetByUsername(username);
                var task =await TaskService.Find(value.Task);
                var users = value.Users.Select(x => UserService.GetById(x)).ToList();
                var status =NOTIFICATION.ASSIGN_TASK;
                var usersString = string.Join(",", users.Select(x => x.FullName).ToList());


                var assignments = value.Users.Select(x =>
                        new TaskEntityAssignment()
                        {
                            CreatedDate = DateTime.Now,
                            Request = value.Request,
                            UserId = x,
                            CreatedById = eUser.Id,
                        })
                    .ToList();

                ChangeLogHelper.AddChangeLogToEntity(task, eUser.Id,
                    new[]
                        {
                            new Change()
                                {OldValue = "إضافة موظف", newValue = "إضافة موظف", Field = " إضافة" + usersString}
                        }
                        .ToList());

                task.TaskAssignments.AddRange(assignments);
                await TaskService.UpdateEntity(task);
                await ServiceFactory.NotificationHelper()
                    .BuildNotificationForUsers(eUser.Id, value.Users, status, (int) EntityType.Task, task.Id);

                return Ok(SuccessResponse<Task>.build(null, 0));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

    }
}
