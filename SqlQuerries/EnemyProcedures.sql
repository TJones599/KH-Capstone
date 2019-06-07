
create procedure ENEMIES_VIEW_ALL

as
begin

select EnemyId,
		EnemyName,
		EnemyDesc,
		EnemyLocation,
		Validated
from Enemies

end

create procedure ENEMIES_VIEW_SINGLE
(
@ID INT
)
AS
BEGIN

SELECT *
FROM Enemies
WHERE EnemyId = @ID

END

CREATE PROC ENEMIES_CREATE
(
@Name nvarchar(50),
@Desc nvarchar(250),
@Location nvarchar(50),
@image nvarchar(250),
@Validated bit
)
AS
BEGIN

insert into Enemies(EnemyName, EnemyDesc, EnemyLocation, ImagePath, Validated)
values (				@Name,     @Desc,     @Location,     @image,    @Validated)

END

create proc ENEMIES_UPDATE
(
@ID INT,
@Name nvarchar(50),
@Desc nvarchar(250),
@Location nvarchar(50),
@image nvarchar(250),
@Validated bit
)
as
begin

update Enemies
set EnemyName = @Name,
	EnemyDesc = @Desc,
	EnemyLocation = @Location,
	ImagePath = @image,
	Validated = @Validated
where EnemyId = @ID
end

create proc ENEMIES_DELETE
(
@ID int
)
as
begin
delete from Enemies
where EnemyId = @ID
end





