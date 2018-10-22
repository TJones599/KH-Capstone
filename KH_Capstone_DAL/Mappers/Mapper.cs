using KH_Capstone_DAL.Models;
using System.Data.SqlClient;

namespace KH_Capstone_DAL.Mappers
{
    public class Mapper
    {
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

        public static UserDO MapSingleUser(SqlDataReader from)
        {
            UserDO user = new UserDO();

            user.UserId = (int)from["UserID"];
            user.UserName = from["UserName"] as string;
            user.Password = from["Password"] as string;
            user.RoleName = from["RoleName"] as string;
            user.Role = (int)from["Role"];
            user.FirstName = from["FirstName"] as string;
            user.LastName = from["LastName"] as string;
            user.Banned = (bool)from["Banned"];
            user.Inactive = (bool)from["Inactive"];

            return user;
        }

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

        public static EnemyItemDetailsDO MapSingleEnemyLink(SqlDataReader from)
        {
            EnemyItemDetailsDO to = new EnemyItemDetailsDO();
            to.LinkID = (int)from["ID"];
            to.EnemyID = (int)from["EnemyID"];
            to.ItemID = (int)from["ItemID"];

            return to;
        }
        
        public static RoleDO MapSingleRole(SqlDataReader from)
        {
            RoleDO to = new RoleDO();

            to.RoleID = (int)from["RoleID"];
            to.Name = from["RoleName"] as string;

            return to;
        }
    }
}
