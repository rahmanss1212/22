using Events.Api.Models.APTs;
using Events.Api.Models.General;
using Events.Api.Models.Incidents;
using Events.Api.Models.Tasks;
using Events.Core.Models.APTs;
using Events.Core.Models.Employees;
using Events.Core.Models.General;
using Events.Core.Models.Incidents;
using Events.Core.Models.Tasks;
using Events.Core.Models.UserManagement;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Events.Core.Models.Logging;
using Events.Core.Models;
using Events.Core.Models.Notifications;
using Events.Api.Models.UserManagement;
using Events.Core.Models.DPE;
using System.Collections.Generic;
using System;
using System.Reflection.Metadata;

//using Events.Core.Models.DPE;
using Events.Core.Models.Reports;
using Events.Core.Models.NewsBlog;
using Events.Core.Models.StaticIP;

namespace Events.Data
{
    public class AppDbContext : IdentityDbContext<EUser, ERole, long>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<EUser> Employees { get; set; }

        public DbSet<Department> Departments { set; get; }
        public DbSet<Inbox> Inboxs { set; get; }
        public DbSet<OrganizationContact> OrganizationContacts { get; set; }
        public DbSet<OrganizationDomain> OrganizationDomains { get; set; }
        public DbSet<OrganizationIps> OrganizationIp { get; set; }

        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<APT> Apts { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<OwnerDetails> OwnerDetails { get; set; }

        public DbSet<Incident> Incidents { get; set; }

        public DbSet<EntityAssignment> EntityAssignments { get; set; }
        public DbSet<StaticIP> StaticIPs { get; set; }
        public DbSet<LateTopics> LateTopics { get; set; }
        public DbSet<SectionRecords> SectionRecords { get; set; }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<IncidentComment> IncidentsComments { get; set; }

        public DbSet<ChangeLog> ChangeLogs { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Section> Sections { get; set; }
        public DbSet<New> News { get; set; }
        public DbSet<GeneralReport> GeneralReports { get; set; }
        
        public DbSet<GeneralReportDataView> GeneralReportsViews { get; set; }

        public DbSet<TaskEntityAssignment> TaskEntityAssignments { get; set; }
        
        public DbSet<GReportEntityAssignment> GReportEntityAssignment { get; set; }
        
        
        public DbSet<BlogNews> BlogNews { get; set; }
        public DbSet<ReportCategory> ReportCategories { get; set; }
        public DbSet<IncidentCategory> IncidentCategories { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Saverity> Saverities { get; set; }
        public DbSet<Urgancey> Urganceys { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Sector> Sectors { get; set; }
        public DbSet<AptView> AptView { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<TaskComment> TaskComments { get; set; }
        public DbSet<TaskType> TaskType { get; set; }
        public DbSet<EUser> Users { get; set; }

        public DbSet<NotificationView> VNotifications { get; set; }
        
        public DbSet<TaskEmpsRel> TaskEmpsRel { get; set; }

        public DbSet<UserActivity> UserActivity { get; set; }

        public DbSet<IpAddress> IpAddress { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public DbSet<CloseReport> CloseReports { get; set; }

        public DbSet<Taskview> Taskviews { get; set; }

        public DbSet<IncidentView> IncidentViews { get; set; }
        public DbSet<DPE> DPEs { get; set; }
        public DbSet<DPEview> DpeView { get; set; }
        public DbSet<Exploit> Exploits { get; set; }
        public DbSet<VerificationPossibility> VerificationPossibilities { get; set; }
        public DbSet<AssessmentMethodology> AssessmentMethodologies { get; set; }
        public DbSet<AssessmentType> AssessmentTypes { get; set; }
        public DbSet<ExploitSeverity> ExploitSeveritys { get; set; }
        public DbSet<ExploitIpType> ExploitIpTypes { get; set; }
        public DbSet<ExploitAffectedSource> ExploitAffectedSources { get; set; }
        public DbSet<ExploitView> ExploitViews { get; set; }
        public DbSet<Vulnerability> Vulnerabilities { get; set; }
        
        public DbSet<DepartmentRecords> DepartmentRecords { get; set; }
        public DbSet<VulnerabilityView> VulnerabilityViews { get; set; }
        public DbSet<CountingOrgView> CountingOrgViews { get; set; }
        public DbSet<CountingExploitView> CountingExploitViews { get; set; }
        public DbSet<NumberOfDoneView> NumberOfDoneViews { get; set; }
        public DbSet<NotificationOwner> NotificationOwner { get; set; }


        [System.Obsolete]
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Query<Taskview>().ToView("taskview").HasNoKey();
            modelBuilder.Query<IncidentView>().ToView("IncidentView").HasNoKey() ;
            modelBuilder.Query<NotificationView>().ToView("NotificationView").HasNoKey();
            modelBuilder.Query<Inbox>().ToView("Inbox").HasNoKey();
            modelBuilder.Query<GeneralReportDataView>().ToView("GeneralReportView").HasNoKey();
            modelBuilder.Query<DepartmentRecords>().ToView("DepartmentsRecords").HasNoKey();
            modelBuilder.Query<SectionRecords>().ToView("SectionRecords").HasNoKey();
            modelBuilder.Query<AptView>().ToView("aptview").HasNoKey();
            modelBuilder.Query<LateTopics>().ToView("LateSubjects").HasNoKey();
            modelBuilder.Query<ExploitView>().ToView("ExploitView").HasNoKey();
            modelBuilder.Query<DPEview>().ToView("DpeView").HasNoKey();
            modelBuilder.Query<VulnerabilityView>().ToView("VulnerabilityView").HasNoKey();
            modelBuilder.Query<CountingExploitView>().ToView("CountingExploitView").HasNoKey();
            modelBuilder.Query<CountingOrgView>().ToView("CountingOrgView").HasNoKey();
            modelBuilder.Query<NumberOfDoneView>().ToView("NumberOfDoneView").HasNoKey();

            modelBuilder.Entity<Incident>()
                .HasMany(x => x.Comments)
                .WithOne(x => x.incident)
                .HasForeignKey(x => x.IncidentId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Task>()
                .HasMany(x => x.TaskComments)
                .WithOne(x => x.Task)
                .HasForeignKey(x => x.TaskId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Task>()
                .HasMany(x => x.AssignedEmps)
                .WithOne(x => x.Task)
                .HasForeignKey(x => x.TaskId)
                .OnDelete(DeleteBehavior.Restrict);
            

            

            modelBuilder.Entity<OrgsIncidentRel>().HasKey(o =>
              new { o.IncidentId, o.OrganizationId });
            
            modelBuilder.Entity<DPEWorkTeam>().HasKey(o =>
                new { o.EUserId, o.DpeId });

            modelBuilder.Entity<TaskComment>().HasKey(o =>
  new { o.TaskId, o.CommentId });

            modelBuilder.Entity<CommentAttachment>().HasKey(o =>
new { o.commentId, o.attachmentId });


            modelBuilder.Entity<IncidentComment>().HasKey(o =>
  new { o.IncidentId, o.CommentId });


            modelBuilder.Entity<OriginCountry>()
                .HasKey(bc => new { bc.CountryId, bc.APTId });
            
            modelBuilder.Entity<VulnerabilityVerificationPossibilities>()
                .HasKey(bc => new { bc.VerificationPossibilityId, bc.VulnerabilityId });
            
            modelBuilder.Entity<GeneralReportAttachment>()
                .HasKey(bc => new { bc.GeneralReportId, bc.AttachmentId });

           
            modelBuilder.Entity<TargetedCountry>()
                .HasKey(bc => new { bc.CountryId, bc.APTId });

            modelBuilder.Entity<TargetedSector>()
                .HasKey(bc => new { bc.SectorId, bc.AptId });

            modelBuilder.Entity<TaskEmpsRel>()
                .HasKey(bc => new { bc.EUserId, bc.TaskId });

            modelBuilder.Entity<AptAttachment>()
                .HasKey(bc => new { bc.AttachmentId, bc.APTId });
            modelBuilder.Entity<DPEAttachment>()
                .HasKey(bc => new { bc.AttachmentId, bc.DPEId });

            modelBuilder.Entity<TaskAttachments>()
                .HasKey(bc => new { bc.AttachmentId, bc.TaskId });

            modelBuilder.Entity<IncidentAttachment>()
            .HasKey(bc => new { bc.AttachmentId, bc.IncidentId });

            modelBuilder.Entity<OrgsIncidentRel>()
             .HasKey(bc => new { bc.OrganizationId, bc.IncidentId });

            modelBuilder.Entity<ReportAttachment>()
                .HasKey(bc => new { bc.attachmentId, bc.closeReportId });

            base.OnModelCreating(modelBuilder);


        }

        public IEnumerable<object> Select(Func<object, object> p)
        {
            throw new NotImplementedException();
        }
    }
    
}
