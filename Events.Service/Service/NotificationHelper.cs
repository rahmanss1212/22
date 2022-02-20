using Events.Api.Models.APTs;
using Events.Api.Models.UserManagement;
using Events.Core.Models;
using Events.Core.Models.Notifications;
using Events.Core.Models.Tasks;
using Events.Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Events.Api;
using Events.Api.Authorization;
using Events.Core.Models.General;
using Events.Core.Models.Incidents;
using Events.Service.Service.DataServices;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using UserManagment.services;
using Task = Events.Core.Models.Tasks.Task;

namespace Events.Service
{
    public class NotificationHelper : INotificationHelper
    {

        
        private Dictionary<int, Func<long?,long, Task<List<NotificationOwner>>>> _owners;
        private readonly IUserService _userService;
        private readonly DbServiceImpl<EntityAssignment,EntityAssignment> _incidentEntityAssignment;
        private readonly SysConfiguration _configuration;
        private readonly IServiceFactory factory;
        private DbServiceImpl<Notification, NotificationView> _dbServiceImpl;

        public NotificationHelper(IServiceFactory sf, IUserService us,SysConfiguration config)
        {
            _owners = getDictionary();
            _incidentEntityAssignment = sf.ServicOf<EntityAssignment, EntityAssignment>();
            _dbServiceImpl = sf.ServicOf<Notification, NotificationView>();
            _userService = us;
            factory = sf;
            _configuration = config;
        }




        public async Task<bool> BuildNotification( long pUser, long status, int EntityType, long EntityId,int ParentEntityType =0, long ParentEntityId =0)
        {

            try
            {
                Notification notification = new Notification();
           
                notification.DateTime = DateTime.Now;
                notification.EntityType = EntityType;
                notification.EntityId = EntityId;
                notification.CreatedById = pUser;
                notification.ParentEntityType = ParentEntityType;
                notification.ParentEntityId = ParentEntityId;
                notification.StatusId = status;
                notification.NotificationOwners = await _owners[EntityType].Invoke(EntityId, pUser);
                await _dbServiceImpl.AddItem(notification);
                return true;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public async Task<bool> BuildNotificationForUsers(long pUser, List<long> users, long? status, int EntityType,
            long EntityId, int ParentEntityType = 0, long ParentEntityId = 0)
        {
            try
            {
                Notification notification = new Notification();
                notification.DateTime = DateTime.Now;
                notification.EntityType = EntityType;
                notification.EntityId = EntityId;
                notification.CreatedById = pUser;
                notification.ParentEntityType = ParentEntityType;
                notification.ParentEntityId = ParentEntityId;
                notification.StatusId = status;
                notification.NotificationOwners = users.Select(x => new NotificationOwner() { employeeId = x, isNew = true }).ToList();
                var  response = await _dbServiceImpl.AddItem(notification);
                if (response.IsSuccess)
                {
                    return await updateAllNotficiationRelatedToEntity(EntityType, EntityId);
                }

                return false;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        private async Task<bool> updateAllNotficiationRelatedToEntity(int entityType, long entityId)
        {
            var list = await _dbServiceImpl.GetItems(x => x.EntityId == entityId && x.EntityType == entityType);
            var ids = list.Select(x => x.NotificationId).ToList();
            var dbServiceImpl = factory.ServicOf<NotificationOwner,NotificationOwner>();
            
            ExpressionStarter<NotificationOwner> predicateBuilder = PredicateBuilder.False<NotificationOwner>();
            predicateBuilder.Or(onr =>ids.Any(y => y == onr.NotificationId ));
            
            var owners = await dbServiceImpl.GetItems(predicateBuilder);
            owners.ForEach(x => x.IsUpdated = true);
            return await dbServiceImpl.UpdateRange(owners);
        }

        public async Task<bool> BuildNotificationForClaim(long createdById, int EntityType, long EntityId,
            long? StatusId, string claimType, string claimValue)
        {
            var users = await _userService.GetUsersIdOfClaim(claimType,claimValue);
            return await BuildNotificationForUsers(createdById, users, StatusId, EntityType, EntityId);
        }


        private async Task<List<NotificationOwner>> getUsersRelatedToComment(long? entityId, long User)
        {
            try
            {
                List<NotificationOwner> list = new List<NotificationOwner>();
                Comment comment =await factory.ServicOf<Comment,Comment>().Find(entityId??0);
                if (comment.TaskComments == null)
                    list.AddRange(await getUsersRelatedToTask(comment.TaskComments[0].TaskId, User));
                else
                    list.AddRange(await getUsersRelatedToIncident(comment.IncidentComments[0].IncidentId, User));
                
                return list;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


        }
        private async Task<List<NotificationOwner>> getUsersRelatedToTask(long? entityId, long User)
        {
            try
            {
                var notificationOwners = _userService.GetHead(User)
                    .Select(x => new NotificationOwner() {employeeId = x.Id}).ToList();
                List<NotificationOwner> list = new List<NotificationOwner>();
                list.AddRange(notificationOwners);
                Task task =  await factory.ServicOf<Task, Taskview>().Find(entityId??0);
                list.AddRange(task.AssignedEmps.Select(x => new NotificationOwner() { employeeId = x.EUser.Id, isNew = true }).ToList());
                return getDistinctList(list);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
        private async Task<List<NotificationOwner>> getUsersRelatedToIncident(long? entityId,long User)
        {
            try
            {
                List<NotificationOwner> list = new List<NotificationOwner>();
                list.AddRange((await _userService.GetUsersIdOfClaim(TYPES.NOTIFICATIONS,VALUES.VIEW))
                    .Select(x => new NotificationOwner() { employeeId = x }).ToList());
                var assignments = await _incidentEntityAssignment.GetItems(x => x.IncidentId == entityId && x.IsHandeled);
                list.AddRange(assignments.Select(x => new NotificationOwner() {employeeId = x.UserId??0}).ToList());

                return getDistinctList(list);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


        }

        private static List<NotificationOwner> getDistinctList(List<NotificationOwner> list)
        {
            return list.Select(x => x.employeeId).Distinct().Select(x => new NotificationOwner(){employeeId = x}).ToList();
        }

        private Dictionary<int, Func<long?, long, Task<List<NotificationOwner>>>> getDictionary()
        => new Dictionary<int, Func<long?, long, Task<List<NotificationOwner>>>>()
        {
            {(int)EntityType.Comment, getUsersRelatedToComment},
            {(int)EntityType.Task, getUsersRelatedToTask},
            {(int)EntityType.Incident, getUsersRelatedToIncident}
        };

    }

    public interface INotificationHelper {
        Task<bool> BuildNotification(long pUser, long status, int EntityType, long EntityId, int ParentEntityType = 0, long ParentEntityId = 0);
        Task<bool> BuildNotificationForUsers(long pUser, List<long> users, long? status, int EntityType, long EntityId,
            int ParentEntityType = 0, long ParentEntityId = 0);
        Task<bool> BuildNotificationForClaim(long createdById, int EntityType, long EntityId, long? StatusId,
            string claimType, string claimValue);
    }

}
