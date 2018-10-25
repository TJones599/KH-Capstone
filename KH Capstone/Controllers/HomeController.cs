using KH_Capstone.LoggerPL;
using KH_Capstone.Models;
using KH_Capstone_BLLs;
using KH_Capstone_DAL;
using KH_Capstone_DAL.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Mvc;

namespace KH_Capstone.Controllers
{
    public class HomeController : Controller
    {
        private EnemyItemDAO linkDAO;
        private ItemDAO iDAO;
        private readonly string connectionString;
        private readonly string logPath;

        public HomeController()
        {
            this.connectionString = ConfigurationManager.ConnectionStrings["dataSource"].ConnectionString;
            this.logPath = Path.Combine(Path.GetDirectoryName(System.Web.HttpContext.Current.Server.MapPath("~")),ConfigurationManager.AppSettings["relative"]);
            linkDAO = new EnemyItemDAO(connectionString, logPath);
            iDAO = new ItemDAO(connectionString, logPath);
        }

        public ActionResult Index()
        {
            ActionResult response;
            HomePageVM items = new HomePageVM();
            try
            {
                ItemPO mostCommonItem = new ItemPO();
                ItemPO leastCommonItem = new ItemPO();

                int mostCommonID;
                int leastCommonID;
                List<EnemyItemDetailsDO> fullLinkList = linkDAO.ViewAllLinks();
                List<int> itemIdList = new List<int>();
                itemIdList = Mapper.BLLMapper.LinkListToItemIdList(fullLinkList);

                mostCommonID = AnalyzeItemList.MostCommonID(itemIdList);
                leastCommonID = AnalyzeItemList.LeastCommonID(itemIdList);

                mostCommonItem =Mapper.Mapper.ItemDOtoPO(iDAO.ViewItemSingle(mostCommonID));
                leastCommonItem = Mapper.Mapper.ItemDOtoPO(iDAO.ViewItemSingle(leastCommonID));

                items.MostCommonItem = mostCommonItem;
                items.LeastCommonItem = leastCommonItem;

                response = View(items);
            }
            catch (SqlException sqlEx)
            {
                if (!sqlEx.Data.Contains("Logged") || (bool)sqlEx.Data["Logged"] == false)
                {
                    Logger.LogSqlException(sqlEx);
                }
                response = View(items);
            }
            catch(Exception ex)
            {
                if (!ex.Data.Contains("Logged") || (bool)ex.Data["Logged"] == false)
                {
                    Logger.LogException(ex);
                }
                response = View(items);
            }
            
            return response;
        }

        public ActionResult GraphData()
        {
            //ViewBag.jsonData = GetData();

            return View();
        }
    }
}