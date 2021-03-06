USE [cdcdb]
GO
/****** Object:  View [dbo].[IncidentView]    Script Date: 6/7/2021 10:54:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[IncidentView] as Select n.id,n.CreatedById, n.subject,org.orgname,usr.fullname ,n.date,n.time,s.lable as saverity,
st.Id as statusId,st.statusString,es.id as entityStatus
from incidents n,organizations org,AspNetUsers usr,saverities s,OrgsIncidentRel orgrel, statuses st, entitystatus es
where n.createdById = usr.Id and  s.id = n.saverityid and n.id = orgrel.incidentId and orgrel.organizationId = org.id and n.statusId = es.id
and es.statusid =st.Id;
GO
/****** Object:  View [dbo].[NotificationView]    Script Date: 6/7/2021 10:54:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[NotificationView] as select
n.Id as NotificationId,
no.Id as NotificationOwnerId,
'' as title,
'' as description,
n.DateTime,
n.statusid,
no.isnew as notificationstatus,
e.fullname,
EntityType = case
when n.EntityType = 2 then n.entitytype
when n.EntityType = 1 then n.entitytype
when n.EntityType = 4 then n.parententitytype 
end,

EntityId = case
when n.EntityType = 2 then n.entityid
when n.EntityType = 1 then n.entityid
when n.EntityType = 4 then n.parentEntityId 
end,
EntityTitle = case 
when n.EntityType = 2 then (select subject from Incidents where id = n.entityid)
when n.EntityType = 1 then (select title from Tasks where id = n.entityid)
when n.EntityType = 4 then case 
when n.ParentEntityType = 2 then (select subject from Incidents where id = n.parentEntityId)
when n.ParentEntityType = 1 then (select title from Tasks where id = n.parententityid)
end 
end,
no.employeeId as ownerId
from notifications n, notificationowner no , AspNetUsers e
where n.id = no.NotificationId and e.id = n.createdByid;
GO
/****** Object:  View [dbo].[taskView]    Script Date: 6/7/2021 10:54:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  View [dbo].[taskView]    Script Date: 12/29/2020 11:55:48 AM ******/

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
