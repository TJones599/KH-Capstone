using KH_Capstone_DAL.Models;
using System.Data.SqlClient;

namespace KH_Capstone_DAL.Mappers
{
    public class Mapper
    {
        //mapper to map a single enemys data from the server to an EnemyDO object
        public static EnemyDO MapSingleEnemy(SqlDataReader from)
        {
            EnemyDO to = new EnemyDO();

            to.EnemyID = (int)from["EnemyID"];
            to.Name = from["Name"] as string;
            to.Location = from["Location"] as string;
            to.Description = from["Description"] as string;
            to.ImagePath = from["ImagePath"] as string;
            to.Validated = (bool)from["Validated"];

            return to;
        }

        //mapper to map a single users data from the server to an UserDO object
        public static UserDO MapSingleUser(SqlDataReader from)
        {
            UserDO user = new UserDO();

            user.UserId = (int)from["UserID"];
            user.UserName = from["UserName"] as string;
            user.Password = from["Password"] as byte[];
            user.RoleName = from["RoleName"] as string;
            user.Role = (int)from["Role"];
            user.FirstName = from["FirstName"] as string;
            user.LastName = from["LastName"] as string;
            user.Banned = (bool)from["Banned"];
            user.Inactive = (bool)from["Inactive"];
            user.Salt = from["Salt"] as string;

            return user;
        }

        //mapper to map a single itemms data from the server to an ItemDO object
        public static ItemDO MapSingleItem(SqlDataReader from)
        {
            ItemDO to = new ItemDO();

            to.ItemID = (int)from["ItemID"];
            to.Name = from["ItemName"] as string;
            to.Description = from["Description"] as string;
            to.ImagePath = from["Image"] as string;
            to.Purchasable = (bool)from["Purchasable"];
            to.Validated = (bool)from["Validate"];

            return to;
        }

        //mapper to map a single EnemyItem link form the server to an EnemyItemDO object
        //this item shows what items an enemy drops. 
        //stores the EnemyID, the ItemID, and the ID of the link in the database
        public static EnemyItemDO MapSingleEnemyLink(SqlDataReader from)
        {
            EnemyItemDO to = new EnemyItemDO();
            to.LinkID = (int)from["ID"];
            to.EnemyID = (int)from["EnemyID"];
            to.ItemID = (int)from["ItemID"];

            return to;
        }
        
        //mapper to map a single roles data from the server to a RoleDO object
        public static RoleDO MapSingleRole(SqlDataReader from)
        {
            RoleDO to = new RoleDO();

            to.RoleID = (int)from["RoleID"];
            to.Name = from["RoleName"] as string;

            return to;
        }
    }
}
