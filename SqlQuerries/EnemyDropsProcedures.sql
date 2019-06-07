create proc ENEMYDROPS_CREATE
(
@EId int,
@IId int
)
as
begin

insert into EnemyDrops(EnemyId,DropId)
values				(@EId, @IId)

end

create proc ENEMYDROPS_VIEW_ALL

as
begin
select EnemyId,
		DropId
from EnemyDrops
end

create proc ENEMYDROPS_BY_ENEMYID
(
@EId int
)
as
begin

select EnemyId,
		DropId
from EnemyDrops
where EnemyId = @EId

end


create proc ENEMYDROPS_BY_DROPID
(
@DId int
)
as
begin
select EnemyId,
		DropId
from EnemyDrops
where DropId = @DId
end


create proc ENEMYDROPS_UPDATE
(
@EnemyDropId int,
@Eid int,
@DId int
)
as
begin

update EnemyDrops
set EnemyId = @Eid,
	DropId = @DId
where EnemyDropsId = @EnemyDropId

end


create proc ENEMYDROPS_DELETE_BY_ENEMYID
(
@EId int
)
as
begin

delete from EnemyDrops
where EnemyId = @Eid

end




