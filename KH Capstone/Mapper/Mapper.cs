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

        public static List<EnemyPO> EnemyDOListToPO(List<EnemyDO> from)
        {
            List<EnemyPO> enemyList = new List<EnemyPO>();

            foreach(EnemyDO enemy in from)
            {
                enemyList.Add(EnemyDOtoPO(enemy));
            }
            return enemyList;
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

        public static List<ItemPO> ItemDOListToPO(List<ItemDO> from)
        {
            List<ItemPO> to = new List<ItemPO>();
            foreach(ItemDO item in from)
            {
                to.Add(ItemDOtoPO(item));
            }
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

        public static UserDO UserPOtoDO(UserPO from)
        {
            UserDO to = new UserDO();

            to.UserId = from.UserID;
            to.UserName = from.UserName;
            to.Password = from.Password;
            to.RoleName = from.RoleName;
            to.Role = from.Role;
            to.FirstName = from.FirstName;
            to.LastName = from.LastName;
            to.Banned = from.Banned;
            to.Inactive = from.Inactive;

            return to;
        }

        public static UserPO UserDOtoPO(UserDO from)
        {
            UserPO to = new UserPO();

            to.UserID = from.UserId;
            to.UserName = from.UserName;
            to.Password = from.Password;
            to.RoleName = from.RoleName;
            to.Role = from.Role;
            to.FirstName = from.FirstName;
            to.LastName = from.LastName;
            to.Banned = from.Banned;
            to.Inactive = from.Inactive;

            return to;
        }

        public static List<UserPO> UserDOListToPO(List<UserDO> from)
        {
            List<UserPO> to = new List<UserPO>();

            foreach(UserDO user in from)
            {
                UserPO temp = new UserPO();

                temp.UserID = user.UserId;
                temp.UserName = user.UserName;
                temp.Password = null;
                temp.RoleName = user.RoleName;
                temp.Role = user.Role;
                temp.FirstName = user.FirstName;
                temp.LastName = user.LastName;
                temp.Banned = user.Banned;
                temp.Inactive = user.Inactive;

                to.Add(temp);
            }

            return to;
        }

        public static EnemyItemIDLink DetailsDOtoPO(EnemyItemDetailsDO from)
        {
            EnemyItemIDLink to = new EnemyItemIDLink();

            to.LinkID = from.LinkID;
            to.EnemyID = from.EnemyID;
            to.ItemID = from.ItemID;

            return to;
        }

        public static List<EnemyItemIDLink> DetailsDOtoPO(List<EnemyItemDetailsDO> from)
        {
            List<EnemyItemIDLink> to = new List<EnemyItemIDLink>();
            foreach (EnemyItemDetailsDO enemy in from)
            {
                EnemyItemIDLink temp = new EnemyItemIDLink();
                temp.LinkID = enemy.LinkID;
                temp.EnemyID = enemy.EnemyID;
                temp.ItemID = enemy.ItemID;
                to.Add(temp);
            }

            return to;
        }

        public static EnemyItemDetailsDO DetailsPOtoDO(EnemyItemIDLink from)
        {
            EnemyItemDetailsDO to = new EnemyItemDetailsDO();

            to.LinkID = from.LinkID;
            to.EnemyID = from.EnemyID;
            to.ItemID = from.ItemID;

            return to;
        }

        public static List<EnemyItemDetailsDO> DetailsPOtoDO(List<EnemyItemIDLink> from)
        {
            List<EnemyItemDetailsDO> to = new List<EnemyItemDetailsDO>();
            foreach (EnemyItemIDLink enemy in from)
            {
                EnemyItemDetailsDO temp = new EnemyItemDetailsDO();
                temp.LinkID = enemy.LinkID;
                temp.EnemyID = enemy.EnemyID;
                temp.ItemID = enemy.ItemID;
                to.Add(temp);
            }

            return to;
        }

        public static List<RolePO> RoleDOListToPOList(List<RoleDO> from)
        {
            List<RolePO> to = new List<RolePO>();

            foreach(RoleDO role in from)
            {
                RolePO temp = new RolePO();

                temp.RoleID = role.RoleID;
                temp.RoleName = role.Name;

                to.Add(temp);
            }

            return to;
        }

        public static EnemyPO MapEnemyUpdateVMtoEnemyPO(EnemyUpdateVM form)
        {
            EnemyPO to = new EnemyPO();

            to.EnemyID = form.Enemy.EnemyID;
            to.Name = form.Enemy.Name;
            to.Location = form.Enemy.Location;
            to.Description = form.Enemy.Description;
            to.ImagePath = form.Enemy.ImagePath;
            to.Validated = form.Enemy.Validated;

            return to;
        }
    }
}