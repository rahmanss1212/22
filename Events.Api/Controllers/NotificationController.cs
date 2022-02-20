
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Events.Api.Models.General;
using Events.Core.Models;
using Events.Core.Models.General;
using Events.Core.Models.Notifications;
using Events.Service.Service;
using Events.Service.Service.DataServices;
using Microsoft.AspNetCore.Mvc;
using UserManagment.services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Events.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {

        private IServiceFactory ServiceFactory;
        private IUserService UsersService;
        private Dictionary<long, String> titles = new Dictionary<long, string>() {
            {NOTIFICATION.CLOSED_INCIDENT,"تم غلق الحادثة: " },
            {NOTIFICATION.OPEN_NOTIFICATION,"تم تسجيل تنبيه : " },
            {NOTIFICATION.EDIT_INCIDENT,"طلب تعديل التنبيه: " },
            {NOTIFICATION.IGNORED_NOTIFICATION,"تم تجاهل التنبيه: " },
            {NOTIFICATION.CLOSED_NOTIFICATION,"تم إغلاق التنبيه: "},
            {NOTIFICATION.INCIDENT,"تم تحويل التنبيه إلى حادثة: " },
            {NOTIFICATION.SEND_TO_VARIFY,"لديك تعريف طلب جديد: " },
            {NOTIFICATION.ADD_COMMENT,"تم التعليق على التنبيه: " },
            {NOTIFICATION.ASSIGN_INCIDENT,"قسيمة إجراء جديدة في التنبيه: " },
            {NOTIFICATION.ASSIGN_TASK," قسيمة إجراء جديدة في المهمة: " },
            {NOTIFICATION.ASSIGN_REPORT,"تم إضافتك في التقرير الاستخباراتي: " },
            {VULNERABILITY.WAITING_HEAD_DIRECTIONS,"إنتظار توجيهات الرئاسة: " },
            {VULNERABILITY.WAITING_VARIFY,"تم تعريف المعرف الإلكتروني بإنتظار تعديل العنوان : " },
            {VULNERABILITY.WAITING_FIX,"إنتظار إصلاح الثغرة: " },
            {VULNERABILITY.WAITING_ORG_INFORMING,"تقرير الفحص بإنتظار إجراء قسم التنسيق: " },
            {VULNERABILITY.WAITING_FIX_AUTHENTICATION,"ثغرة بإنتظار التحقق من الإصلاح: " },
            {NOTIFICATION.REQUEST_RESPONSE,"تم الرد على قسيمة إجراء  " },
            {TASK.ADD_COMMENT,"تم التعليق على المهمة: " },
            {TASK.OPEN,"تم إسناد المهمة: " },
            {TASK.CLOSED,"تم غلق المهمة: " },
            


        };

        private DbServiceImpl<Notification, NotificationView> _dbServiceImpl;

        public NotificationController(IServiceFactory service, IUserService us)
        {
            _dbServiceImpl = service.ServicOf<Notification, NotificationView>();
            UsersService = us;
        }

        // GET: api/<NotificationController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user =await UsersService.GetByUsername(username);
                List<NotificationView> list = await _dbServiceImpl
                    .GetItems(x => x.OwnerId == user.Id && x.NotificationStatus,null);
                
                    list = list.Select(SetProperTitle).Reverse().ToList();
                return Ok(SuccessResponse<NotificationView>.build(null, 0, list));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpPost("updateStatus")]
        public async Task<IActionResult> ChangeStatus([FromBody] Notification value)
        {
            try {
                Notification oldValue = await _dbServiceImpl.Find(value.Id);
                oldValue.NotificationOwners.ForEach(x => {
                    if (x.Id == value.NotificationOwners[0].Id)
                        x.isNew = false;
                });

               await _dbServiceImpl.UpdateEntity(oldValue);
                return Ok(); 

            } catch (Exception e)
            
            {
                return Ok(FailedResponse.Build(e.Message)); 
            }

        }

        private NotificationView SetProperTitle(NotificationView note, int arg2)
        {

            note.Title = titles[note.StatusId] + note.EntityTitle;
            return note;
        }

    }
}
