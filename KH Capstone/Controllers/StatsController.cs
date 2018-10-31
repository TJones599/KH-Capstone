using KH_Capstone.Models;
using KH_Capstone_BLLs;
using KH_Capstone_DAL;
using KH_Capstone_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace KH_Capstone.Controllers
{
    public class StatsController : Controller
    {
        private EnemyItemDAO linkDAO;
        private ItemDAO iDAO;
        private EnemyDAO eDAO;
        private readonly string connectionString;
        private readonly string logPath;

        public StatsController()
        {
            this.connectionString = ConfigurationManager.ConnectionStrings["dataSource"].ConnectionString;
            this.logPath = ConfigurationManager.AppSettings["relative"];
            linkDAO = new EnemyItemDAO(connectionString, logPath);
            iDAO = new ItemDAO(connectionString, logPath);
            eDAO = new EnemyDAO(connectionString, logPath);
        }

        public ActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public JsonResult GetItemData()
        {
            //pullingg linkList from db
            List<EnemyItemDO> list = linkDAO.ViewAllLinks();
            //stripping itemID's off of the list and storing them in a list
            List<int> idList = Mapper.BLLMapper.LinkListToItemIdList(list);

            //sending the idList to a BLL method to find unique id's and count their occurances
            Dictionary<int, int> countDictionary = AnalyzeItemList.ItemDropCount(idList);

            //instantiating a dictionary that will hold item names(based on ids) and how many times they were used
            Dictionary<string, int> itemDropCount = new Dictionary<string, int>();

            //creating a list of ItemPO to hold item information, to pull names from items based on ids in idList
            List<ItemPO> itemList = new List<ItemPO>();

            //foreach unique itemID, add that item's information to the itemList
            foreach (KeyValuePair<int, int> idLink in countDictionary)
            {
                ItemPO item = Mapper.Mapper.ItemDOtoPO(iDAO.ViewItemSingle(idLink.Key));
                itemList.Add(item);
            }

            //foreach item in the itemList, create a key based on item name, and set its base value to 0
            foreach (ItemPO item in itemList)
            {
                itemDropCount.Add(item.Name, 0);
                //for each key value pair in our count dictionary which holds our item id and how many times that item has been used
                //we check if the key value pair's key is the same as the item's id in the outer for each
                //if it matches we update the value of our itemDropCount
                foreach (KeyValuePair<int, int> itemCount in countDictionary)
                {
                    if (itemCount.Key == item.ItemID)
                    {
                        itemDropCount[item.Name] = itemCount.Value;
                    }
                }
            }

            //creating an object that holds 2 lists, one of type string, one of type int
            //holds our item names in one list, and our item occurance in the other
            ChartData<string, int> chartData = new ChartData<string, int>();
            chartData.Labels = new List<string>();
            chartData.Values = new List<int>();

            foreach (KeyValuePair<string, int> itemDrop in itemDropCount)
            {
                chartData.Labels.Add(itemDrop.Key);
                chartData.Values.Add(itemDrop.Value);
            }

            return Json(chartData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetEnemyData()
        {
            List<EnemyDO> doList = eDAO.ViewAllEnemies();
            List<EnemyPO> enemyList = Mapper.Mapper.EnemyDOListToPO(doList);
            List<string> locationList = new List<string>();

            locationList = Mapper.BLLMapper.PullEnemyLocation(enemyList);

            Dictionary<string, int> locationOccurance = new Dictionary<string, int>();
            locationOccurance = AnalyzeItemList.LocationCount(locationList);

            ChartData<string, int> chartData = new ChartData<string, int>();
            chartData.Labels = new List<string>();
            chartData.Values = new List<int>();
            foreach(KeyValuePair<string,int> location in locationOccurance)
            {
                chartData.Labels.Add(location.Key);
                chartData.Values.Add(location.Value);
            }

            return Json(chartData, JsonRequestBehavior.AllowGet);
        }
    }
}