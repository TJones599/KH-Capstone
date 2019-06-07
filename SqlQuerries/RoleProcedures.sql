create proc ROLES_CREATE
(
@Name nvarchar(50),
@Desc nvarchar(250)
)
as
begin

insert into Roles(RoleName, RoleDesc)
values	(		@Name, @Desc)

end

create proc ROLES_VIEW_ALL

as
begin

select RoleName,
		RoleDesc
from Roles

end


create proc ROLES_VIEW_SINGLE
(
@Id int
)
as
begin

select RoleName,
		RoleDesc
from Roles
where RoleID = @Id

end

create proc	ROLES_UPDATE
(
@id int,
@name nvarchar(50),
@desc nvarchar(250)
)
as
begin

update Roles
set RoleName = @name,
	RoleDesc = @desc
where RoleID = @id

end

create proc ROLES_DELETE
(
@id int
)
as
begin

delete from Roles
where RoleID = @id
end

