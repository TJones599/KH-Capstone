
CREATE TABLE Roles
(
RoleID int primary key identity(1,1),
RoleName nvarchar(50) not null,
RoleDesc nvarchar(250) not null
)

drop table Roles

CREATE TABLE Users
(
UserId int primary key identity(1,1),
Username NVARCHAR(50) not null,
UserPass varbinary(100)not null,
userRole int not null,
Foreign key (userRole) References Roles(RoleID),
FirstName NVARCHAR(50),
LastName NVARCHAR(50),
Banned binary not null,
Inactive binary not null,
Salt NVARCHAR(250) not null
)

drop table Users

create table Enemies
(
EnemyId int primary key identity(1,1),
EnemyName nvarchar(50) not null,
EnemyLocation nvarchar(50) not null,
EnemyDesc nvarchar(250) not null,
ImagePath nvarchar(250) not null,
Validated binary not null
)

drop table Enemies

create table Items
(
ItemId int primary key identity(1,1),
ItemName nvarchar(50) not null,
ItemDesc nvarchar(250) not null,
ImagePath nvarchar(250) not null,
Purchasable binary not null,
Validated binary not null
)

drop table Items

create table EnemyDrops
(
EnemyDropsId int primary key identity(1,1),
EnemyId int not null,
foreign key (EnemyId) references Enemies(EnemyId),
DropId int not null,
foreign key (DropId) references Items(ItemId)
)

drop table EnemyDrops