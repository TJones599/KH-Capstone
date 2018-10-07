using KH_Capstone.Models;
using KH_Capstone_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KH_Capstone.Mapper
{
    public class Mapper
    {
        public static EnemyDO EnemyPOtoDO(EnemyPO from)
        {
            EnemyDO to = new EnemyDO();

            to.EnemyID = from.EnemyID;
            to.Name = from.Name;
            to.Location = from.Location;
            to.Description = from.Description;
            to.ImagePath = from.ImagePath;
            to.Validated = from.Validated;

            return to;
        }

        public static EnemyPO EnemyDOtoPO(EnemyDO from)
        {
            EnemyPO to = new EnemyPO();

            to.EnemyID = from.EnemyID;
            to.Name = from.Name;
            to.Location = from.Location;
            to.Description = from.Description;
            to.ImagePath = from.ImagePath;
            to.Validated = from.Validated;

            return to;
        }

        public static ItemDO ItemPOtoDO(ItemPO from)
        {
            ItemDO to = new ItemDO();

            to.ItemID = from.ItemID;
            to.Name = from.Name;
            to.Description = from.Description;
            to.ImagePath = from.ImagePath;
            to.Purchasable = from.Purchasable;
            to.Validated = from.Validated;

            return to;
        }

        public static ItemPO ItemDOtoPO(ItemDO from)
        {
            ItemPO to = new ItemPO();

            to.ItemID = from.ItemID;
            to.Name = from.Name;
            to.Description = from.Description;
            to.ImagePath = from.ImagePath;
            to.Purchasable = from.Purchasable;
            to.Validated = from.Validated;

            return to;
        }

        public static UserDO UserPOtoDO(UserDO from)
        {
            UserDO to = new UserDO();

            to.UserId = from.UserId;
            to.UserName = from.UserName;
            to.Password = from.Password;
            to.Role = from.Role;

            return to;
        }

        public static UserPO UserDOtoPO(UserDO from)
        {
            UserPO to = new UserPO();

            to.UserID = from.UserId;
            to.UserName = from.UserName;
            to.Password = from.Password;
            to.Role = from.Role;

            return to;
        }

    }
}