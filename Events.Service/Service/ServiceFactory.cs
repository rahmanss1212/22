using Events.Api.Models.APTs;
using Events.Api.Models.General;
using Events.Api.Models.Incidents;
using Events.Api.Models.Tasks;
using Events.Core.Models;
using Events.Core.Models.General;
using Events.Core.Models.Incidents;
using Events.Core.Models.Notifications;
using Events.Core.Models.Reports;
using Events.Core.Models.Tasks;
using Events.Data;
using Events.Service.Service.DataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Events.Api;
using Events.Core.Models.APTs;
using Events.Core.Models.DPE;
using Events.Core.Models.StaticIP;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UserManagment.services;

namespace Events.Service.Service
{
    
    public delegate List<U> ContextForList<U>(AppDbContext context);
    public delegate U ContextForSingle<U>(AppDbContext context);
    
    
    public class ServiceFactory : IServiceFactory
    {

        private readonly Dictionary<Type, object> services;
        private readonly Dictionary<Type, object> Constants;
        private readonly AppDbContext context;
        private readonly IUserService UserService;
        private readonly SysConfiguration _configuration;
        

        public ServiceFactory(IQuery<Incident, IncidentView> svc,AppDbContext ctx,
            IQuery<Task, Taskview> ts, IQuery<Comment, Comment> cs, IQuery<Notification, NotificationView> ns,
             IQuery<NotificationOwner, NotificationOwner> notowner,IQuery<SectionRecords, SectionRecords> secRe,
            IQuery<StaticIP, StaticIP> stip,IQuery<Exploit, ExploitView> exp,IQuery<DPE, DPEview> dpe, IQuery<LateTopics, LateTopics> ltquery,
            IQuery<Status, Status> ss, IUserService us, IQuery<Organization, Organization> os,IQuery<Inbox, Inbox> inbox,
            IQuery<GeneralReport, GeneralReportDataView> grs,IQuery<Attachment, Attachment> att,IQuery<DepartmentRecords, DepartmentRecords> drq,
            IQuery<GReportEntityAssignment, GReportEntityAssignment> gres,IQuery<Vulnerability, VulnerabilityView> vq,
            IQuery<EntityAssignment, EntityAssignment> es, IQuery<IpAddress, IpAddress> ips,IOptions<SysConfiguration> config,
            IQuery<TaskEntityAssignment, TaskEntityAssignment> teas,IQuery<APT, AptView> apt)
        {
            context = ctx;
            UserService = us;
            _configuration = config.Value;
            services = new Dictionary<Type, object>()
            {
                {typeof(Status), ss},
                {typeof(IpAddress), ips},
                {typeof(Notification), ns},
                {typeof(LateTopics), ltquery},
                {typeof(Exploit), exp},
                {typeof(DepartmentRecords), drq},
                {typeof(SectionRecords), secRe},
                {typeof(Inbox), inbox},
                {typeof(Vulnerability), vq},
                {typeof(DPE), dpe},
                {typeof(Incident), svc},
                {typeof(NotificationOwner), notowner},
                {typeof(Comment), cs},
                {typeof(Task), ts},
                {typeof(StaticIP), stip},
                {typeof(GReportEntityAssignment), gres},
                {typeof(Organization), os},
                {typeof(EntityAssignment), es},
                {typeof(APT), apt},
                {typeof(GeneralReport), grs},
                {typeof(TaskEntityAssignment), teas},
                {typeof(Attachment), att},
                
            };
                
                Constants = new Dictionary<Type, object>(){
                { typeof(Saverity),ctx.Saverities },
                { typeof(Category),ctx.Categories },
                { typeof(Urgancey),ctx.Urganceys },
                { typeof(TaskType),ctx.TaskType },
                { typeof(ReportCategory),ctx.ReportCategories },
                { typeof(Section),ctx.Sections },
                { typeof(IncidentCategory),ctx.IncidentCategories },
            };
        }
        

        public U GetConstant<U>(long id) where U : class,IConstant
        => ((DbSet<U>) Constants[typeof(U)]).Find(id);

        public U UseContextForSingleReterive<U>(ContextForSingle<U> ctx)
        => ctx(context);
        
        public List<U> UseContextForListReterive<U>(ContextForList<U> ctx)
        => ctx(context);

        public List<U> GetConstantList<U>() where U : class,IConstant
        => ((DbSet<U>) Constants[typeof(U)]).ToList();
        public async void AddConstant<U>(U entity) 
        {
            context.Add(entity);
            await context.SaveChangesAsync();
        }

        public IChangeLogHelper ChangeLogHelper()
         =>new ChangeLogHelper(this);
        
        public INotificationHelper NotificationHelper()
        => new NotificationHelper(this,UserService,_configuration);

        
        public DbServiceImpl<T, V> ServicOf<T, V>() where T : Model
            => new DbServiceImpl<T, V>((IQuery <T,V>)services[typeof(T)]);
    }
    public interface IServiceFactory
    {
        DbServiceImpl<T,V> ServicOf<T,V>() where T : Model;
        IChangeLogHelper 
            ChangeLogHelper();
        INotificationHelper NotificationHelper();
        List<U> UseContextForListReterive<U>(ContextForList<U> entity);
        U UseContextForSingleReterive<U>(ContextForSingle<U> entity);
        List<U> GetConstantList<U>() where U : class,IConstant;
        U GetConstant<U>(long id) where U : class,IConstant;
        public void AddConstant<U>(U entity);


    }
}
