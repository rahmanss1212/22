USE [cdcdb]
GO
/****** Object:  View [dbo].[CountingExploitView]    Script Date: 12/15/2021 10:18:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[CountingExploitView] as select ex.Id , t.ExploitsTitle , t.orgTotal from Exploits  ex
    join
    (
    select  e.title as ExploitsTitle , COUNT(o.Orgname) as orgTotal
    from Vulnerabilities v , DPEs d, Exploits e, Organizations o
    where d.OrganizationId = o.Id and v.ExploitId = e.Id and d.Id =v.DPEId
    group by e.title
) as T
    on ex.title = t.ExploitsTitle;
GO
/****** Object:  View [dbo].[CountingOrgView]    Script Date: 12/15/2021 10:18:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[CountingOrgView] as
SELECT        o.Id, o.Orgname, T.Total, T.D_T, T.N_T, T.Critical, T.D_C, T.Medium, T.D_M, T.Low, T.D_L, T.Unkown, T.D_U
FROM            dbo.Organizations AS o INNER JOIN
                (SELECT        COUNT(CASE WHEN e.severityId = 1 AND v.StatusId = 33 THEN 1 ELSE NULL END) AS Critical, COUNT(CASE WHEN e.severityId = 1 AND v.StatusId <> 33 THEN 1 ELSE NULL END) AS D_C,
                               COUNT(CASE WHEN e.severityId = 2 AND v.StatusId = 33 THEN 1 ELSE NULL END) AS Medium, COUNT(CASE WHEN e.severityId = 2 AND v.StatusId <> 33 THEN 1 ELSE NULL END) AS D_M,
                               COUNT(CASE WHEN e.severityId = 3 AND v.StatusId = 33 THEN 1 ELSE NULL END) AS Low, COUNT(CASE WHEN e.severityId = 3 AND v.StatusId <> 33 THEN 1 ELSE NULL END) AS D_L,
                               COUNT(CASE WHEN e.severityId = 4 AND v.StatusId = 33 THEN 1 ELSE NULL END) AS Unkown, COUNT(CASE WHEN e.severityId = 4 AND v.StatusId <> 33 THEN 1 ELSE NULL END) AS D_U, COUNT(e.Id)
                                                                                                          AS Total, COUNT(CASE WHEN v.StatusId = 33 THEN 1 ELSE NULL END) AS D_T, COUNT(CASE WHEN v.StatusId <> 33 THEN 1 ELSE NULL END) AS N_T, o.Orgname
                 FROM            dbo.Vulnerabilities AS v INNER JOIN
                                 dbo.Exploits AS e ON v.ExploitId = e.Id INNER JOIN
                                 dbo.Organizations AS o INNER JOIN
                                 dbo.DPEs AS d ON o.Id = d.OrganizationId ON v.DPEId = d.Id
                 GROUP BY o.Orgname) AS T ON o.Orgname = T.Orgname
GO
/****** Object:  View [dbo].[DepartmentsRecords]    Script Date: 12/15/2021 10:18:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[DepartmentsRecords] as SELECT        d.Id, d.Name,
                             (SELECT        COUNT(*) AS Expr1
                               FROM            dbo.Sections AS se INNER JOIN
                                                         dbo.Departments AS de ON se.DepartmentId = de.Id INNER JOIN
                                                         dbo.Tasks AS ta INNER JOIN
                                                         dbo.AspNetUsers AS us ON ta.CreatedById = us.Id ON se.Id = us.SectionId
                               WHERE        (de.Id = d.Id) AND (YEAR(ta.CreatedDate) = YEAR(CAST(GETDATE() AS Date))) AND (MONTH(ta.CreatedDate) = MONTH(CAST(GETDATE() AS Date)))) AS Tasks,
                             (SELECT        COUNT(*) AS Expr1
                               FROM            dbo.Sections AS se INNER JOIN
                                                         dbo.Departments AS de ON se.DepartmentId = de.Id INNER JOIN
                                                         dbo.Incidents AS i INNER JOIN
                                                         dbo.AspNetUsers AS us ON i.CreatedById = us.Id ON se.Id = us.SectionId INNER JOIN
                                                         dbo.EntityStatus AS es ON i.StatusId = es.Id
                               WHERE        (de.Id = d.Id) AND (MONTH(i.CreatedDate) = MONTH(CAST(GETDATE() AS Date))) AND (YEAR(i.CreatedDate) = YEAR(CAST(GETDATE() AS Date))) AND (es.StatusId IN (8, 9))) AS Incidents,
                             (SELECT        COUNT(*) AS Expr1
                               FROM            dbo.Sections AS se INNER JOIN
                                                         dbo.Departments AS de ON se.DepartmentId = de.Id INNER JOIN
                                                         dbo.Incidents AS i INNER JOIN
                                                         dbo.AspNetUsers AS us ON i.CreatedById = us.Id ON se.Id = us.SectionId INNER JOIN
                                                         dbo.EntityStatus AS es ON i.StatusId = es.Id
                               WHERE        (de.Id = d.Id) AND (MONTH(i.CreatedDate) = MONTH(CAST(GETDATE() AS Date))) AND (YEAR(i.CreatedDate) = YEAR(CAST(GETDATE() AS Date))) AND (es.StatusId IN (10, 11, 12))) AS Notifications,
                             (SELECT        COUNT(*) AS Expr1
                               FROM            dbo.Sections AS se INNER JOIN
                                                         dbo.Departments AS de ON se.DepartmentId = de.Id INNER JOIN
                                                         dbo.GeneralReports AS gr INNER JOIN
                                                         dbo.AspNetUsers AS us ON gr.CreatedById = us.Id ON se.Id = us.SectionId
                               WHERE        (MONTH(gr.CreatedDate) = MONTH(CAST(GETDATE() AS Date))) AND (YEAR(gr.CreatedDate) = YEAR(CAST(GETDATE() AS Date))) AND (de.Id = d.Id)) AS Reports,
                             (SELECT        COUNT(*) AS Expr1
                               FROM            dbo.Sections AS se INNER JOIN
                                                         dbo.Departments AS de ON se.DepartmentId = de.Id INNER JOIN
                                                         dbo.DPEs AS dpe INNER JOIN
														 dbo.Vulnerabilities  as v on v.dpeid = dpe.id INNER JOIN
                                                         dbo.AspNetUsers AS us ON dpe.CreatedById = us.Id ON se.Id = us.SectionId
                               WHERE        (MONTH(dpe.DateFrom) = MONTH(CAST(GETDATE() AS Date)))AND (YEAR(dpe.DateFrom) = YEAR(CAST(GETDATE() AS Date))) AND (de.Id = d.id)) AS Dpes,
                             (SELECT        COUNT(*) AS Expr1
                               FROM            dbo.Sections AS se INNER JOIN
                                                         dbo.Departments AS de ON se.DepartmentId = de.Id INNER JOIN
                                                         dbo.Apts AS apt INNER JOIN
                                                         dbo.AspNetUsers AS us ON apt.CreatedById = us.Id ON se.Id = us.SectionId
                               WHERE        (YEAR(apt.CreatedDate) = YEAR(CAST(GETDATE() AS Date))) AND (de.Id = d.Id)) AS Apts
FROM            dbo.AspNetUsers AS u INNER JOIN
                         dbo.Sections AS s ON u.SectionId = s.Id INNER JOIN
                         dbo.Departments AS d ON s.DepartmentId = d.Id
GROUP BY d.Id, d.Name
GO
/****** Object:  View [dbo].[DpeView]    Script Date: 12/15/2021 10:18:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Script for SelectTopNRows command from SSMS  ******/
create view [dbo].[DpeView] as SELECT  d.Id
    ,d.LastUpdateDate
    ,d.CreatedDate
    ,d.OrganizationId
    ,o.Orgname
    ,d.StatusId
    ,d.AssessmentMethodologyId
    ,d.title
    ,(select count(id) from Vulnerabilities where DPEId = d.id) as vulnerCount
    ,(select min(e.severityId) from Vulnerabilities v, Exploits e where v.DPEId = d.id and v.ExploitId = e.Id) as severity
    ,(select min(Vulnerabilities.StatusId)  from Vulnerabilities where DPEId = d.id) as VulnerStatus
    FROM dpes d , Organizations o where o.Id = d.OrganizationId

GO
/****** Object:  View [dbo].[exploitView]    Script Date: 12/15/2021 10:18:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[exploitView]
as SELECT        e.Id AS ExploitId, e.title, e.cve, e.descriptin, e.Solution, es.name AS severity, es.Id AS severityid, e.resources
   FROM            dbo.Exploits AS e  INNER JOIN
                   dbo.ExploitSeveritys AS es ON e.severityId = es.Id;
GO
/****** Object:  View [dbo].[GeneralReportView]    Script Date: 12/15/2021 10:18:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create view [dbo].[GeneralReportView] as
SELECT        r.Id, u.FullName AS createdbyname, r.Title, r.[Content], r.StatusId, s.StatusString AS status, r.CreatedDate, c.Title AS CategoryTitle
FROM            dbo.GeneralReports AS r INNER JOIN
                dbo.AspNetUsers AS u ON r.CreatedById = u.Id INNER JOIN
                dbo.ReportCategories AS c ON r.ReportCategoryId = c.Id INNER JOIN
                dbo.Statuses AS s ON r.StatusId = s.Id
GO
/****** Object:  View [dbo].[inbox]    Script Date: 12/15/2021 10:18:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[inbox] as  
SELECT        i.id, type = CASE WHEN es.StatusId = 10 THEN N'تنبيه جديد' WHEN es.StatusId = 8 THEN N'حادثة أمنية' END, 2 AS typeid, 10 AS subtypeid, es.StatusId AS entitystatusid, i.LastUpdateDate AS date, usr.FullName AS fromUser, 
                         usr.id AS fromuserid, 'NOTIFICATIONS,VIEW' AS claim, 0 AS toUser, i.subject AS title, LEFT(i.Description, 50) AS brife, cast(0 AS bit) AS isNew, cast(0 AS bit) AS isUpdated
FROM            Incidents i, EntityStatus es, AspNetUsers usr
WHERE        i.CreatedById = usr.id AND es.StatusId IN (8, 10) AND es.id = i.StatusId
UNION
SELECT        i.id, N'طلب تعريف' AS type, 7 AS typeid, 0 AS subtypeid, es.StatusId AS entitystatusid, i.LastUpdateDate AS date, usr.FullName AS fromUser, usr.id AS fromuserid, 'IP_IDENTITFICATION,VARFIY' AS claim, 0 AS toUser, 
                         i.subject AS title, LEFT(i.Description, 50) AS brife, cast(0 AS bit) AS isNew, cast(0 AS bit) AS isUpdated
FROM            Incidents i, AspNetUsers usr, EntityStatus es
WHERE        i.CreatedById = usr.id AND es.StatusId = 24 AND es.id = i.StatusId
UNION
SELECT        i.id, N' تنبيه : قسيمة إجراء' AS type, 2 AS typeid, 7 AS subtypeid, estatus.StatusId AS entitystatusid, es.CreatedDate AS date, usr.FullName AS fromUser, usr.id AS fromuserid, '' AS claim, es.UserId AS toUser, i.subject AS title, 
                         LEFT(i.Description, 50) AS brife, cast(0 AS bit) AS isNew, cast(0 AS bit) AS isUpdated
FROM            Incidents i, EntityAssignments es, AspNetUsers usr, EntityStatus estatus
WHERE        es.IncidentId = i.id AND es.CreatedById = usr.id AND es.IsHandeled = 0 AND i.StatusId <> 16 AND i.StatusId = estatus.Id
UNION
SELECT        i.id, N'تنبيه : طلب تعديل' AS type, 2 AS typeid, 16 AS subtypeid, es.StatusId AS entitystatusid, i.LastUpdateDate AS date, usr.FullName AS fromUser, usr.id AS fromuserid, '' AS claim, i.CreatedById AS toUser, i.subject AS title, 
                         LEFT(i.Description, 50) AS brife, cast(0 AS bit) AS isNew, cast(0 AS bit) AS isUpdated
FROM            Incidents i, EntityStatus es, AspNetUsers usr
WHERE        i.LastUpdateById = usr.id AND es.StatusId = 16 AND es.id = i.StatusId
UNION
SELECT        t .id, N'مهمة : قسيمة إجراء' AS type, 1 AS typeid, 7 AS subtypeid, t .StatusId AS entitystatusid, tes.CreatedDate AS date, usr.FullName AS fromUser, usr.id AS fromuserid, '' AS claim, tes.UserId AS toUser, t .Title AS title, 
                         LEFT(t .Description, 50) AS brife, cast(0 AS bit) AS isNew, cast(0 AS bit) AS isUpdated
FROM            Tasks t, TaskEntityAssignments tes, AspNetUsers usr
WHERE        tes.TaskId = t .id AND tes.CreatedById = usr.id AND tes.IsHandled = 0
UNION
SELECT        t .id, type = CASE WHEN t .StatusId = 3 THEN N'مهمة قيد الإجراء' WHEN t .StatusId = 5 THEN N'مهة جديدة' END, 1 AS typeid, 0 AS subtypeid, t .StatusId AS entitystatusid, t.LastUpdateDate AS date, usr.FullName AS fromUser, 
                         usr.id AS fromuserid, '' AS claim, ter.EUserId AS toUser, t .Title AS title, LEFT(t .Description, 50) AS brife, cast(0 AS bit) AS isNew, cast(0 AS bit) AS isUpdated
FROM            Tasks t, TaskEmpsRel ter, AspNetUsers usr
WHERE        ter.TaskId = t .id AND ter.EUserId = usr.id AND t .StatusId IN (3, 5)
UNION
SELECT        r.id, N'تقرير إستخباراتي: قسيمة إجراء' AS type, 5 AS typeid, 0 AS subtypeid, r.StatusId AS entitystatusid, res.CreatedDate AS date, usr.FullName AS fromUser, usr.id AS fromuserid, '' AS claim, res.UserId AS toUser, r.Title AS title, 
                         LEFT(r.Content, 50) AS brife, cast(0 AS bit) AS isNew, cast(0 AS bit) AS isUpdated
FROM            GeneralReports r, GReportEntityAssignment res, AspNetUsers usr
WHERE        res.GeneralReportId = r.id AND res.CreatedById = usr.id AND res.IsHandeled = 0
UNION
SELECT        d .id, N'تقرير الثغرات' AS type, 6 AS typeid, 4 AS subtypeid, d .StatusId AS entitystatusid, d .LastUpdateDate AS date, usr.FullName AS fromUser, usr.id AS fromuserid, 'DPE,HEAD_DIRECTIONS' AS claim, 0 AS toUser, 
                         N'توجيهات الرئاسة' AS title, o.orgname AS brife, cast(0 AS bit) AS isNew, cast(0 AS bit) AS isUpdated
FROM            DPEs d, Organizations o, AspNetUsers usr
WHERE        o.id = d .OrganizationId AND d .CreatedById = usr.id AND d .StatusId = 29
UNION
SELECT        d .id, N'تقرير الثغرات' AS type, 6 AS typeid, 5 AS subtypeid, d .StatusId AS entitystatusid, d .LastUpdateDate AS date, usr.FullName AS fromUser, usr.id AS fromuserid, 'DPE,ORG_INFORM' AS claim, 0 AS toUser, 
                         N'إجراء  قسم التنسيق' AS title, o.orgname AS brife, cast(0 AS bit) AS isNew, cast(0 AS bit) AS isUpdated
FROM            DPEs d, Organizations o, AspNetUsers usr
WHERE        o.id = d .OrganizationId AND d .CreatedById = usr.id AND d .StatusId = 30
UNION
SELECT        v.id, N'ثغرة' AS type, 3 AS typeid, subtypeid = CASE WHEN v.StatusId = 32 THEN 7 WHEN v.StatusId = 31 THEN 6 END, v.StatusId AS entitystatusid, d .LastUpdateDate AS date, usr.FullName AS fromUser, usr.id AS fromuserid, 
                         claim = CASE WHEN v.StatusId = 32 THEN 'DPE,FIX_AUTH' WHEN v.StatusId = 31 THEN 'DPE,ORG_FIX_DONE' END, 0 AS toUser, 
                         title = CASE WHEN v.StatusId = 32 THEN N'إنتظار التحقق من الإصلاح' WHEN v.StatusId = 31 THEN N'إنتظار إصلاح المؤسسة' END, CONCAT(CONCAT(o.orgname, ' : '), e.title) AS brife, cast(0 AS bit) AS isNew, cast(0 AS bit) 
                         AS isUpdated
FROM            DPEs d, Vulnerabilities v, Exploits e, Organizations o, AspNetUsers usr
WHERE        o.id = d .OrganizationId AND e.Id = v.ExploitId AND v.DPEId = d .id AND d .CreatedById = usr.id AND v.StatusId IN (32, 31)
GO
/****** Object:  View [dbo].[incidentView]    Script Date: 12/15/2021 10:18:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[incidentView] as SELECT n.Id, n.CreatedById, n.Subject, n.IsIpsIdentificationRequested AS isSendToIpVarify, n.AptId, org.Orgname, org.Id AS orgid, sec.Name AS sector, sec.Id AS sectorid, usr.FullName,usr.SectionId, n.Date, n.Time, s.Lable AS saverity, st.Id AS statusId, st.StatusString, es.Id AS entityStatus
    FROM   dbo.Incidents AS n INNER JOIN
    dbo.AspNetUsers AS usr ON n.CreatedById = usr.Id INNER JOIN
    dbo.Saverities AS s ON n.SaverityId = s.Id INNER JOIN
    dbo.OrgsIncidentRel AS orgrel ON n.Id = orgrel.IncidentId INNER JOIN
    dbo.Organizations AS org ON orgrel.OrganizationId = org.Id INNER JOIN
    dbo.Sectors AS sec ON org.SectorId = sec.Id INNER JOIN
    dbo.EntityStatus AS es ON n.StatusId = es.Id INNER JOIN
    dbo.Statuses AS st ON es.StatusId = st.Id
GO
/****** Object:  View [dbo].[LateSubjects]    Script Date: 12/15/2021 10:18:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[LateSubjects]  as
SELECT        d.Id, d.Name,
              (SELECT        COUNT(*) AS Expr1
               FROM            dbo.Sections AS se INNER JOIN
                               dbo.Departments AS de ON se.DepartmentId = de.Id INNER JOIN
                               dbo.Tasks AS ta INNER JOIN
                               dbo.TaskEmpsRel tr ON tr.TasKId = ta.id inner join
                               dbo.AspNetUsers AS us ON tr.EUserId = us.Id ON se.Id = us.SectionId
               WHERE        (de.Id = d.Id) and ta.StatusId <> 4 AND ta.DueDate < CAST(GETDATE() AS Date)) AS Tasks,
              (SELECT count(*)  AS Expr1 FROM
                  dbo.Incidents AS i INNER JOIN
                  dbo.EntityStatus AS es ON i.StatusId = es.Id
               WHERE
                       CAST(GETDATE() AS Date) >
                       case
                           when i.UrganceyId = 1 then DATEADD(DAY, 2, i.CreatedDate)
                           when i.UrganceyId = 2 then DATEADD(DAY, 3, i.CreatedDate)
                           when i.UrganceyId = 3 then DATEADD(DAY, 7, i.CreatedDate)
                           end and  es.statusid = 10) AS Incidents,
              (SELECT        COUNT(*) AS Expr1
               FROM            dbo.Sections AS se INNER JOIN
                               dbo.Departments AS de ON se.DepartmentId = de.Id INNER JOIN
                               dbo.GeneralReports AS gr INNER JOIN
                               dbo.AspNetUsers AS us ON gr.CreatedById = us.Id ON se.Id = us.SectionId
               WHERE        (MONTH(gr.CreatedDate) = MONTH(CAST(GETDATE() AS Date))) AND (de.Id = d.Id)) AS Reports,
              (SELECT   count (*) FROM dbo.Vulnerabilities AS v INNER JOIN dbo.DPEs AS d ON v.DPEId = d.Id
               where (SELECT DATEDIFF(DAY, d.InformingDate,(SELECT CAST(GETDATE() AS Date) AS Expr1)) - v.DaysToFix AS Expr1) < 0 )  as vulnerability
FROM            dbo.AspNetUsers AS u INNER JOIN
                dbo.Sections AS s ON u.SectionId = s.Id INNER JOIN
                dbo.Departments AS d ON s.DepartmentId = d.Id
GROUP BY d.Id, d.Name
GO
/****** Object:  View [dbo].[NotificationView]    Script Date: 12/15/2021 10:18:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[NotificationView] as

SELECT        n.Id AS NotificationId, no.isUpdated,no.lastupdated, no.Id AS NotificationOwnerId, '' AS title, '' AS description, n.DateTime, n.StatusId, no.isNew AS notificationstatus, e.FullName,
              CASE WHEN n.EntityType = 2 THEN n.entitytype WHEN n.EntityType = 3 THEN n.entitytype WHEN n.EntityType = 6 THEN n.entitytype WHEN n.EntityType = 1 THEN n.entitytype WHEN n.EntityType = 4 THEN n.parententitytype WHEN
                      n.EntityType = 5 THEN n.entitytype END AS EntityType,
              CASE WHEN n.EntityType = 2 THEN n.entityid WHEN n.EntityType = 3 THEN n.entityid WHEN n.EntityType = 6 THEN n.entityid WHEN n.EntityType = 1 THEN n.entityid WHEN n.EntityType = 4 THEN n.parentEntityId WHEN n.EntityType
                  = 5 THEN n.entityid END AS EntityId, CASE WHEN n.EntityType = 2 THEN
                                                                (SELECT        subject
                                                                 FROM            Incidents
                                                                 WHERE        id = n.entityid) WHEN n.EntityType = 6 THEN
                                                                (SELECT        o.orgname
                                                                 FROM            dpes d, Organizations o
                                                                 WHERE        d .id = n.entityid AND d .organizationid = o.id) WHEN n.EntityType = 3 THEN
                                                                (SELECT        CONCAT(CONCAT(o.orgname, ' : '), e.title)
                                                                 FROM            exploits e, dpes d, Organizations o, Vulnerabilities v
                                                                 WHERE        v.id = n.entityid AND d .organizationid = o.id AND v.exploitid = e.id AND v.DPEId = d .id) WHEN n.EntityType = 1 THEN
                                                                (SELECT        title
                                                                 FROM            Tasks
                                                                 WHERE        id = n.entityid) WHEN n.EntityType = 4 THEN CASE WHEN n.ParentEntityType = 2 THEN
                                                                                                                                   (SELECT        subject
                                                                                                                                    FROM            Incidents
                                                                                                                                    WHERE        id = n.parentEntityId) WHEN n.ParentEntityType = 1 THEN
                                                                                                                                   (SELECT        title
                                                                                                                                    FROM            Tasks
                                                                                                                                    WHERE        id = n.parententityid) END WHEN n.EntityType = 5 THEN
                                                                (SELECT        title
                                                                 FROM            GeneralReports
                                                                 WHERE        id = n.entityid) END AS EntityTitle, no.employeeId AS ownerId
FROM            dbo.Notifications AS n INNER JOIN
                dbo.NotificationOwner AS no ON n.Id = no.NotificationId INNER JOIN
    dbo.AspNetUsers AS e ON n.CreatedById = e.Id
GO
/****** Object:  View [dbo].[NumberOfDoneView]    Script Date: 12/15/2021 10:18:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[NumberOfDoneView] as
SELECT        COUNT(CASE WHEN v.StatusId = 33 THEN 1 ELSE NULL END) AS Done, COUNT(CASE WHEN v.StatusId <> 33 THEN 1 ELSE NULL END) AS NotDone
FROM            dbo.Vulnerabilities AS v
GO
/****** Object:  View [dbo].[SectionRecords]    Script Date: 12/15/2021 10:18:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[SectionRecords] as
SELECT        d.Id, d.Name,
              (SELECT        COUNT(*) AS Expr1
               FROM            dbo.Sections AS se INNER JOIN
                               dbo.Departments AS de ON se.DepartmentId = de.Id INNER JOIN
                               dbo.Tasks AS ta INNER JOIN
                               dbo.AspNetUsers AS us ON ta.CreatedById = us.Id ON se.Id = us.SectionId
               WHERE        (de.Id = d.Id) AND (MONTH(ta.CreatedDate) = MONTH(CAST(GETDATE() AS Date)))) AS Tasks,
              (SELECT        COUNT(*) AS Expr1
               FROM            dbo.Sections AS se INNER JOIN
                               dbo.Departments AS de ON se.DepartmentId = de.Id INNER JOIN
                               dbo.Incidents AS i INNER JOIN
                               dbo.AspNetUsers AS us ON i.CreatedById = us.Id ON se.Id = us.SectionId INNER JOIN
                               dbo.EntityStatus AS es ON i.StatusId = es.Id
               WHERE        (de.Id = d.Id) AND (MONTH(i.CreatedDate) = MONTH(CAST(GETDATE() AS Date))) AND (es.StatusId IN (8, 9))) AS Incidents,
              (SELECT        COUNT(*) AS Expr1
               FROM            dbo.Sections AS se INNER JOIN
                               dbo.Departments AS de ON se.DepartmentId = de.Id INNER JOIN
                               dbo.Incidents AS i INNER JOIN
                               dbo.AspNetUsers AS us ON i.CreatedById = us.Id ON se.Id = us.SectionId INNER JOIN
                               dbo.EntityStatus AS es ON i.StatusId = es.Id
               WHERE        (de.Id = d.Id) AND (MONTH(i.CreatedDate) = MONTH(CAST(GETDATE() AS Date))) AND (es.StatusId IN (10, 11, 12))) AS Notifications,
              (SELECT        COUNT(*) AS Expr1
               FROM            dbo.Sections AS se INNER JOIN
                               dbo.Departments AS de ON se.DepartmentId = de.Id INNER JOIN
                               dbo.GeneralReports AS gr INNER JOIN
                               dbo.AspNetUsers AS us ON gr.CreatedById = us.Id ON se.Id = us.SectionId
               WHERE        (MONTH(gr.CreatedDate) = MONTH(CAST(GETDATE() AS Date))) AND (de.Id = d.Id)) AS Reports,
              (SELECT        COUNT(*) AS Expr1
               FROM            dbo.Sections AS se INNER JOIN
                               dbo.Departments AS de ON se.DepartmentId = de.Id INNER JOIN
                               dbo.DPEs AS dpe INNER JOIN
                               dbo.AspNetUsers AS us ON dpe.CreatedById = us.Id ON se.Id = us.SectionId
               WHERE        (MONTH(dpe.CreatedDate) = MONTH(CAST(GETDATE() AS Date))) AND (de.Id = d.Id)) AS Dpes,
              (SELECT        COUNT(*) AS Expr1
               FROM            dbo.Sections AS se INNER JOIN
                               dbo.Departments AS de ON se.DepartmentId = de.Id INNER JOIN
                               dbo.Apts AS apt INNER JOIN
                               dbo.AspNetUsers AS us ON apt.CreatedById = us.Id ON se.Id = us.SectionId
               WHERE        (YEAR(apt.CreatedDate) = YEAR(CAST(GETDATE() AS Date))) AND (de.Id = d.Id)) AS Apts
FROM            dbo.AspNetUsers AS u INNER JOIN
                dbo.Sections AS s ON u.SectionId = s.Id INNER JOIN
                dbo.Departments AS d ON s.DepartmentId = d.Id
GROUP BY d.Id, d.Name
GO
/****** Object:  View [dbo].[taskView]    Script Date: 12/15/2021 10:18:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


create view [dbo].[taskView] as select
    t.id as TaskId,
    se.name as sectionname ,
    de.name as assignedForDepartment ,
    de.id as assignedForDepartmentId ,
    (select d2.name from departments d2,AspNetUsers u2, Sections s2 where u2.sectionid = s2.id and d2.id = s2.departmentid and u2.id = t.createdbyid) as createdByDepartment,
    (select d2.id from departments d2,AspNetUsers u2, Sections s2 where u2.sectionid = s2.id and d2.id = s2.departmentid and u2.id = t.createdbyid) as createdByDepartmentId,
    t.description,
    t.title,
    t.statusId,
    s.StatusString,
    t.dueDate,
    tt.Name tasktype ,
    t.createdDate,
    t.importance,
    t.urgent,
    ISNULL(u.FullName,'') as assignedForName,
    u.Id as asignedforid,
    (select ISNULL(fullName,'ahmed') from AspNetUsers where t.CreatedById = AspNetUsers.id) as createdBy,
    t.CreatedById,
    t.date,
    ISNULL(t.parentIncidentid,'') as parentIncidentid,
    ISNULL(t.parentTaskid,'') as parentTaskid,
    t.progress,
    ISNULL(t.closingReportid,'') as closingReportid
    from Tasks t,TaskEmpsRel tr,AspNetUsers u,taskType tt,statuses s,sections se,departments de
    where s.Id = t.statusId and t.id = tr.taskid and tr.euserid = u.id AND t.taskTypeId = tt.id and se.id = u.sectionid and de.id = se.departmentid  ;
GO
/****** Object:  View [dbo].[VulnerabilityView]    Script Date: 12/15/2021 10:18:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[VulnerabilityView] as SELECT        v.Id, o.Orgname, e.Id AS ExploitId, e.Title AS ExploitTitle, d.DateFrom as CreatedDate, v.evidence, d.OrganizationId AS orgId, es.name AS Severty, d.Id AS DPEId, ea.name AS Accessibility, ea.Id AS AccessibilityId, v.DaysToFix,
                             (SELECT        DATEDIFF(DAY, d.InformingDate,
                                                             (SELECT        CAST(GETDATE() AS Date) AS Expr1)) - v.DaysToFix AS Expr1) AS days, s.Id AS statusid, s.StatusString AS status, v.hasTested
FROM            dbo.Vulnerabilities AS v INNER JOIN
                         dbo.Exploits AS e ON v.ExploitId = e.Id INNER JOIN
                         dbo.Statuses AS s ON v.StatusId = s.Id INNER JOIN
                         dbo.Organizations AS o INNER JOIN
                         dbo.DPEs AS d ON o.Id = d.OrganizationId ON v.DpeId = d.Id INNER JOIN
                         dbo.AssessmentTypes AS ea ON v.AssessmentTypeId = ea.Id INNER JOIN
                         dbo.ExploitSeveritys AS es ON e.SeverityId = es.Id
GO
