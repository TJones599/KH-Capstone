using KH_Capstone.Models;
using KH_Capstone_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KH_Capstone.Mapper
{
    public class BLLMapper
    {
        public static List<int> IdListToItemIdList(List<EnemyItemIDLink> from)
        {
            List<int> idList = new List<int>();
            foreach (EnemyItemIDLink item in from)
            {
                idList.Add(item.ItemID);
            }
            return idList;
        }

        public static List<int> ItemListToItemIdList(List<ItemDO> from)
        {
            List<int> itemIdList = new List<int>();
            foreach(ItemDO item in from)
            {
                itemIdList.Add(item.ItemID);
            }
            return itemIdList;
        }

        public static List<int> LinkListToItemIdList(List<EnemyItemDO> from)
        {
            List<int> itemIdList = new List<int>();

            foreach(EnemyItemDO link in from)
            {
                int itemId = link.ItemID;
                itemIdList.Add(itemId);
            }

            return itemIdList;
        }

        public static List<string> PullEnemyLocation(List<EnemyPO> from)
        {
            List<string> to = new List<string>();

            foreach(EnemyPO enemy in from)
            {
                if(enemy.Validated)
                {
                    to.Add(enemy.Location);
                }
            }

            return to;
        }
    }
}