create proc ITEMS_VIEW_ALL

AS
BEGIN
SELECT ItemId, 
		ItemName,
		ItemDesc,
		Purchasable,
		Validated
FROM Items
END


create proc ITEMS_VIEW_SINGLE
(@Id int)
AS
BEGIN
SELECT ItemId, 
		ItemName,
		ItemDesc,
		Purchasable,
		Validated
FROM Items
where ItemId = @Id
END

create proc ITEMS_UPDATE
(
@ID INT,
@Name NVARCHAR(50),
@Desc NVARCHAR(250),
@image NVARCHAR(250),
@purchasable bit,
@validated bit
)
as
begin

update Items
set ItemName = @Name,
	ItemDesc = @Desc,
	Validated = @validated,
	ImagePath = @image,
	Purchasable = @purchasable
where ItemId = @ID
end



create proc ITEMS_CREATE
(
@Name NVARCHAR(50),
@Desc NVARCHAR(250),
@image NVARCHAR(250),
@purchasable bit,
@validated bit
)
as
begin

insert into Items(ItemName,ItemDesc,Validated,ImagePath,Purchasable)
	values(            @Name,  @Desc,   @validated,@image, @purchasable)

end

create proc ITEMS_DELETE
(
@Id int
)
as
begin

delete from Items
where ItemId = @Id

end