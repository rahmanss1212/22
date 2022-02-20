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
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Events.Core.Models.APTs;
using Events.Core.Models.DPE;
using Events.Core.Models.StaticIP;

namespace Events.Service.Service.DataServices
{
    public interface IQuery<T, V> where T : Model
    {
        public IOrderedQueryable<V> GetViewQuery();
        public IOrderedQueryable<T> GetQuery();
        public AppDbContext GetContext();
    }

    public abstract class QueryImpl<T, V> : IQuery<T, V> where T : Model
    {
        protected readonly AppDbContext context;

        protected QueryImpl(AppDbContext ctx)
        {
            context = ctx;
        }

        public AppDbContext GetContext()
            => context;

        public abstract IOrderedQueryable<T> GetQuery();
        public abstract IOrderedQueryable<V> GetViewQuery();
    }

    public class EntityAssignmentQuery : QueryImpl<EntityAssignment, EntityAssignment>
    {
        public EntityAssignmentQuery(AppDbContext ctx) : base(ctx)
        {
        }

        public override IOrderedQueryable<EntityAssignment> GetQuery()
            => context.EntityAssignments.Include(x => x.User).OrderBy(x => x.Id);


        public override IOrderedQueryable<EntityAssignment> GetViewQuery()
            => context.EntityAssignments.Include(x => x.Status).ThenInclude(x => x.Status).OrderBy(x => x.Id);
    }

    public class StaticIPsQuery : QueryImpl<StaticIP, StaticIP>
    {
        public StaticIPsQuery(AppDbContext ctx) : base(ctx)
        {
        }

        public override IOrderedQueryable<StaticIP> GetQuery()
            => context.StaticIPs.OrderBy(x => x.Id);


        public override IOrderedQueryable<StaticIP> GetViewQuery()
            => context.StaticIPs.OrderBy(x => x.Id);
    }


    public class LateTopicsQuery : QueryImpl<LateTopics, LateTopics>
    {
        public LateTopicsQuery(AppDbContext ctx) : base(ctx)
        {
        }

        public override IOrderedQueryable<LateTopics> GetQuery()
            => context.LateTopics.OrderBy(x => x.Id);


        public override IOrderedQueryable<LateTopics> GetViewQuery()
            => context.LateTopics.OrderBy(x => x.Id);
    }


    public class SectionRecordsQuery : QueryImpl<SectionRecords, SectionRecords>
    {
        public SectionRecordsQuery(AppDbContext ctx) : base(ctx)
        {
        }

        public override IOrderedQueryable<SectionRecords> GetQuery()
            => context.SectionRecords.OrderBy(x => x.Id);


        public override IOrderedQueryable<SectionRecords> GetViewQuery()
            => context.SectionRecords.OrderBy(x => x.Id);
    }


    public class APTQuery : QueryImpl<APT, AptView>
    {
        public APTQuery(AppDbContext ctx) : base(ctx)
        {
        }

        public override IOrderedQueryable<APT> GetQuery()
            => context.Apts
                .Include(x => x.Targeted).ThenInclude(x => x.Country)
                .Include(x => x.Origin).ThenInclude(x => x.Country)
                .Include(x => x.ThreatSignatures)
                .Include(x => x.AttackStratigies)
                .Include(x => x.AlternativeNames)
                .Include(x => x.CompanyNames)
                .Include(x => x.ToolsNames)
                .Include(x => x.CreatedBy)
                .Include(x => x.TargetSectorNames).ThenInclude(x => x.Sector)
                .Include(x => x.Contents).ThenInclude(x => x.CreatedBy)
                .Include(x => x.Attachments).ThenInclude(x => x.Attachment)
                .OrderBy(x => x.Id);


        public override IOrderedQueryable<AptView> GetViewQuery()
            => context.AptView
                .OrderBy(x => x.Id);
    }

    public class TaskQuery : QueryImpl<Task, Taskview>
    {
        public TaskQuery(AppDbContext ctx) : base(ctx)
        {
        }

        public override IOrderedQueryable<Task> GetQuery()
            => context.Tasks.Include(x => x.Status)
                .Include(x => x.AssignedEmps).ThenInclude(x => x.EUser)
                .Include(x => x.Attachments).ThenInclude(x => x.attachment)
                .Include(x => x.ClosingReport).ThenInclude(x => x.attachments).ThenInclude(x => x.attachment)
                .Include(x => x.TaskComments).ThenInclude(x => x.Comment).ThenInclude(x => x.CreatedBy)
                .Include(x => x.TaskComments).ThenInclude(x => x.Comment).ThenInclude(x => x.Attachments)
                .ThenInclude(x => x.attachment)
                .Include(x => x.TaskAssignments).ThenInclude(x => x.User)
                .Include(x => x.Assigned_for)
                .Include(x => x.CreatedBy)
                .Include(x => x.TaskType)
                .Include(x => x.TaskComments)
                .Include(x => x.ParentTask).ThenInclude(x => x.AssignedEmps)
                .Include(x => x.Changes).ThenInclude(x => x.fields)
                .Include(x => x.Changes).ThenInclude(x => x.changedBy)
                .Include(x => x.ParentIncident)
                .OrderByDescending(x => x.CreatedDate);


        public override IOrderedQueryable<Taskview> GetViewQuery()
            => context.Taskviews.OrderByDescending(x => x.createdDate);
    }

    public class StatusQuery : QueryImpl<Status, Status>
    {
        public StatusQuery(AppDbContext ctx) : base(ctx)
        {
        }

        public override IOrderedQueryable<Status> GetQuery()
            => context.Statuses.OrderBy(x => x.Id);

        public override IOrderedQueryable<Status> GetViewQuery()
            => context.Statuses.OrderBy(x => x.Id);
    }

    public class NotificationOwnersQuery : QueryImpl<NotificationOwner, NotificationOwner>
    {
        public NotificationOwnersQuery(AppDbContext ctx) : base(ctx)
        {
        }

        public override IOrderedQueryable<NotificationOwner> GetQuery()
            => context.NotificationOwner.OrderBy(x => x.Id);

        public override IOrderedQueryable<NotificationOwner> GetViewQuery()
            => context.NotificationOwner.OrderBy(x => x.Id);
    }

    public class DepartmentRecordsQuery : QueryImpl<DepartmentRecords, DepartmentRecords>
    {
        public DepartmentRecordsQuery(AppDbContext ctx) : base(ctx)
        {
        }

        public override IOrderedQueryable<DepartmentRecords> GetQuery()
            => context.DepartmentRecords.OrderBy(x => x.Id);

        public override IOrderedQueryable<DepartmentRecords> GetViewQuery()
            => context.DepartmentRecords.OrderBy(x => x.Id);
    }

    public class NotificationQuery : QueryImpl<Notification, NotificationView>
    {
        public NotificationQuery(AppDbContext ctx) : base(ctx)
        {
        }

        public override IOrderedQueryable<Notification> GetQuery()
            => context.Notifications
                .Include(x => x.NotificationOwners)
                .ThenInclude(x => x.employee)
                .OrderByDescending(x => x.Id);


        public override IOrderedQueryable<NotificationView> GetViewQuery()
            => context.VNotifications.OrderBy(x => x.DateTime);
    }

    public class OrganizationQuery : QueryImpl<Organization, Organization>
    {
        public OrganizationQuery(AppDbContext ctx) : base(ctx)
        {
        }

        public override IOrderedQueryable<Organization> GetQuery()
            => context.Organizations.Include(x => x.OrganizationContact).OrderBy(x => x.Id);

        public override IOrderedQueryable<Organization> GetViewQuery()
            => context.Organizations.OrderBy(x => x.Id);
    }

    public class AttachmentQuery : QueryImpl<Attachment, Attachment>
    {
        public AttachmentQuery(AppDbContext ctx) : base(ctx)
        {
        }

        public override IOrderedQueryable<Attachment> GetQuery()
            => context.Attachments.OrderBy(x => x.Id);

        public override IOrderedQueryable<Attachment> GetViewQuery()
            => context.Attachments.OrderBy(x => x.Id);
    }

    public class CommentQuery : QueryImpl<Comment, Comment>
    {
        public CommentQuery(AppDbContext ctx) : base(ctx)
        {
        }

        public override IOrderedQueryable<Comment> GetQuery()
            => context.Comments.Include(x => x.IncidentComments)
                .Include(x => x.TaskComments)
                .Include(x => x.Replaies)
                .OrderBy(x => x.CreatedDate);


        public override IOrderedQueryable<Comment> GetViewQuery()
            => context.Comments.Include(x => x.IncidentComments)
                .Include(x => x.TaskComments)
                .OrderBy(x => x.CreatedDate);
    }

    public class NewsQuery : QueryImpl<New, New>
    {
        public NewsQuery(AppDbContext ctx) : base(ctx)
        {
        }

        public override IOrderedQueryable<New> GetQuery()
            => context.News
                .Include(x => x.Urgancey)
                .Include(x => x.Users)
                .OrderByDescending(x => x.Id);


        public override IOrderedQueryable<New> GetViewQuery()
            => context.News.OrderBy(x => x.Id);
    }

    public class GeneralReportsQuery : QueryImpl<GeneralReport, GeneralReportDataView>
    {
        public GeneralReportsQuery(AppDbContext ctx) : base(ctx)
        {
        }

        public override IOrderedQueryable<GeneralReport> GetQuery()
            => context.GeneralReports.Include(x => x.Status)
                .Include(x => x.Assignments)
                .ThenInclude(x => x.User)
                .Include(x => x.CreatedBy)
                .Include(x => x.Saverity)
                .Include(x => x.Urgancey)
                .Include(x => x.Attachments)
                .ThenInclude(x => x.Attachment)
                .Include(x => x.ReportCategory)
                .OrderByDescending(x => x.Id);

        public override IOrderedQueryable<GeneralReportDataView> GetViewQuery()
            => context.GeneralReportsViews.OrderByDescending(x => x.Id);
    }

    public class TaskAssignmentsQuery : QueryImpl<TaskEntityAssignment, TaskEntityAssignment>
    {
        public TaskAssignmentsQuery(AppDbContext ctx) : base(ctx)
        {
        }

        public override IOrderedQueryable<TaskEntityAssignment> GetQuery()
            => context.TaskEntityAssignments.OrderByDescending(x => x.Id);

        public override IOrderedQueryable<TaskEntityAssignment> GetViewQuery()
            => context.TaskEntityAssignments.OrderByDescending(x => x.Id);
    }

    public class ReportAssignmentsQuery : QueryImpl<GReportEntityAssignment, GReportEntityAssignment>
    {
        public ReportAssignmentsQuery(AppDbContext ctx) : base(ctx)
        {
        }

        public override IOrderedQueryable<GReportEntityAssignment> GetQuery()
            => context.GReportEntityAssignment.OrderByDescending(x => x.Id);

        public override IOrderedQueryable<GReportEntityAssignment> GetViewQuery()
            => context.GReportEntityAssignment.OrderByDescending(x => x.Id);
    }


    public class IncidentCategoryQuery : QueryImpl<IncidentCategory, IncidentCategory>
    {
        public IncidentCategoryQuery(AppDbContext ctx) : base(ctx)
        {
        }

        public override IOrderedQueryable<IncidentCategory> GetQuery()
            => context.IncidentCategories.OrderByDescending(x => x.Id);

        public override IOrderedQueryable<IncidentCategory> GetViewQuery()
            => context.IncidentCategories.OrderByDescending(x => x.Id);
    }

    public class IncidentQuery : QueryImpl<Incident, IncidentView>
    {
        public IncidentQuery(AppDbContext ctx) : base(ctx)
        {
        }

        public override IOrderedQueryable<Incident> GetQuery()
        {
            return context.Incidents
                .Include(x => x.Category)
                .Include(x => x.Apt)
                .Include(x => x.Status).ThenInclude(x => x.Status)
                .Include(x => x.Saverity)
                .Include(x => x.Urgancey)
                .Include(x => x.CloseReport)
                .Include(x => x.Assignments).ThenInclude(x => x.User)
                .Include(x => x.Orgs).ThenInclude(oi => oi.Organization)
                .Include(x => x.Changes).ThenInclude(x => x.changedBy)
                .Include(x => x.Changes).ThenInclude(x => x.fields)
                .Include(x => x.Comments).ThenInclude(oi => oi.Comment).ThenInclude(x => x.CreatedBy)
                .Include(x => x.Comments).ThenInclude(oi => oi.Comment).ThenInclude(x => x.Replaies)
                .ThenInclude(x => x.CreatedBy)
                .Include(x => x.IpAddresses).ThenInclude(ip => ip.Dest)
                .Include(x => x.Attachments).ThenInclude(at => at.Attachment)
                .Include(x => x.IpAddresses).ThenInclude(ip => ip.Source)
                .Include(x => x.IpAddresses).ThenInclude(ip => ip.OwnerDetail)
                .Include(x => x.CreatedBy)
                .OrderBy(x => x.Id);
        }


        public override IOrderedQueryable<IncidentView> GetViewQuery()
            => context.IncidentViews.OrderBy(x => x.Date);
    }

    public class ExploitQuery : QueryImpl<Exploit, ExploitView>
    {
        public ExploitQuery(AppDbContext ctx) : base(ctx)
        {
        }

        public override IOrderedQueryable<Exploit> GetQuery()
        {
            return context.Exploits.Include(x => x.Severity)
                .OrderBy(x => x.Id);
        }


        public override IOrderedQueryable<ExploitView> GetViewQuery()
            => context.ExploitViews.OrderBy(x => x.ExploitId);
    }

    public class InboxQuery : QueryImpl<Inbox, Inbox>
    {
        public InboxQuery(AppDbContext ctx) : base(ctx)
        {
        }

        public override IOrderedQueryable<Inbox> GetQuery()
            => context.Inboxs.OrderByDescending(x => x.Date);


        public override IOrderedQueryable<Inbox> GetViewQuery()
            => context.Inboxs.OrderByDescending(x => x.Date);
    }


    public class DPEQuery : QueryImpl<DPE, DPEview>
    {
        public DPEQuery(AppDbContext ctx) : base(ctx)
        {
        }

        public override IOrderedQueryable<DPE> GetQuery()
            => context.DPEs
                .Include(x => x.CreatedBy)
                .Include(x => x.LastUpdateBy)
                .Include(x => x.ContactedPerson)
                .Include(x => x.Organization)
                .Include(x => x.AssessmentMethodology)
                .Include(x => x.DpeWorkTeam).ThenInclude(x => x.EUser)
                .OrderByDescending(x => x.Id);


        public override IOrderedQueryable<DPEview> GetViewQuery()
            => context.DpeView.OrderByDescending(x => x.Id);
    }


    public class VulnerabilitiesQuery : QueryImpl<Vulnerability, VulnerabilityView>
    {
        public VulnerabilitiesQuery(AppDbContext ctx) : base(ctx)
        {
        }

        public override IOrderedQueryable<Vulnerability> GetQuery()
            => context.Vulnerabilities
                .Include(x => x.Exploit).ThenInclude(x => x.Severity)
                .Include(x => x.ModifiedBy)
                .Include(x => x.Changes).ThenInclude(x => x.fields)
                .Include(x => x.Changes).ThenInclude(x => x.changedBy)
                .Include(x => x.VulnerabilityVerificationPossibilities).ThenInclude(x => x.VerificationPossibility)
                .Include(x => x.AffectedSource).ThenInclude(x => x.ExploitIpType)
                .OrderBy(x => x.Id);


        public override IOrderedQueryable<VulnerabilityView> GetViewQuery()
            => context.VulnerabilityViews.OrderBy(x => x.Id);
    }

    public class IpAddressQuery : QueryImpl<IpAddress, IpAddress>
    {
        public IpAddressQuery(AppDbContext ctx) : base(ctx)
        {
        }

        public override IOrderedQueryable<IpAddress> GetQuery()
        {
            return context.IpAddress
                .Include(x => x.OwnerDetail)
                .Include(x => x.Dest)
                .Include(x => x.Source)
                .OrderBy(x => x.Id);
        }

        public override IOrderedQueryable<IpAddress> GetViewQuery()
        {
            return context.IpAddress
                .Include(x => x.OwnerDetail)
                .Include(x => x.Dest)
                .Include(x => x.Source)
                .OrderBy(x => x.Id);
        }
    }
}