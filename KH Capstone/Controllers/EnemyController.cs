using KH_Capstone.LoggerPL;
using KH_Capstone.Models;
using KH_Capstone_DAL;
using KH_Capstone_DAL.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web.Mvc;
using KH_Capstone.Custom;
using System.Web;

namespace KH_Capstone.Controllers
{
    public class EnemyController : Controller
    {
        //global strings holding pathing information
        private readonly string connectionString;
        private readonly string logPath;
        EnemyDAO eDAO;

        //controller to gather pathing information and set log path
        public EnemyController()
        {
            connectionString = ConfigurationManager.ConnectionStrings["dataSource"].ConnectionString;
            logPath = Path.Combine(Path.GetDirectoryName(System.Web.HttpContext.Current.Server.MapPath("~")), ConfigurationManager.AppSettings["relative"]);
            Logger.logPath = logPath;
            eDAO = new EnemyDAO(connectionString, logPath);
        }



        /// <summary>
        /// view all enemies, gathers all enemy information from server and puts it into a view model.
        /// passes view model into view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            //instantiating enemy data access object

            List<EnemyPO> enemyList = new List<EnemyPO>();

            //mapping enemydo list provided by dao to enemypo list
            try
            {
                enemyList = Mapper.Mapper.EnemyDOListToPO(eDAO.ViewAllEnemies());
            }
            catch (SqlException sqlEx)
            {
                Logger.LogSqlException(sqlEx);
                throw;
            }
            return View(enemyList);
        }


        /// <summary>
        /// create new enemy, only accessable by registered users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Security_Filter(1)]
        public ActionResult NewEnemy()
        {
            ActionResult response = new ViewResult();

            CreateEnemyVM enemy = new CreateEnemyVM();
            enemy.Item1 = 0;
            enemy.Item2 = 0;
            enemy.Enemy = new EnemyPO();
            enemy.Enemy.ImagePath = "";
            try
            {
                ItemDAO iDAO = new ItemDAO(connectionString, logPath);
                enemy.ItemList = Mapper.Mapper.ItemDOListToPO(iDAO.ViewAllItems());

                //creating and adding default item "none" to drop down list
                ItemPO @default = new ItemPO
                {
                    Name = "None",
                    ItemID = 0,
                    Description = ""
                };

                enemy.ItemList.Add(@default);

                response = View(enemy);
            }
            catch (SqlException sqlEx)
            {
                if (sqlEx.Data.Contains("Logged"))
                {
                    if ((bool)sqlEx.Data["Logged"] == false)
                    {
                        Logger.LogSqlException(sqlEx);
                    }
                }
                else
                {
                    Logger.LogSqlException(sqlEx);
                }
            }
            return response;
        }


        /// <summary>
        /// Create enemy post, takes in a enemyPO form, from the create enemy view
        /// </summary>
        /// <param name="form">EnemyPO</param>
        /// <returns></returns>
        [HttpPost]
        [Security_Filter(1)]
        public ActionResult NewEnemy(CreateEnemyVM form)
        {
            ActionResult response = new ViewResult();
            //try catch to capture any sql errors
            try
            {
                //instantiating enemydao
                //setting validated field of enemypo to false, keeps it from being displayed until validated by mod/admin
                form.Enemy.Validated = false;

                //sets image
                form.Enemy.ImagePath = "~/Images/Enemies/"+form.Enemy.Name+".png";

                //checking for valid form entry
                if (ModelState.IsValid)
                {
                    if (form.File != null)
                    {
                        if (form.File.ContentLength > 0)
                        {
                            string path = Path.Combine(Server.MapPath("~/Images"), form.Enemy.Name);
                            form.File.SaveAs(path);
                        }
                    }



                    //mapping po object to do and passing it to dao
                    EnemyDO enemy = Mapper.Mapper.EnemyPOtoDO(form.Enemy);
                    eDAO.NewEnemy(enemy);

                    //redirecting to enemy page
                    response = RedirectToAction("Index", "Enemy");
                }
                else
                {
                    //if form was not valid, returns to form, supplying the information entered previously
                    response = View(form);
                }
            }
            catch (SqlException sqlEx)
            {
                //logging any sql exceptions caught
                Logger.LogSqlException(sqlEx);
            }

            return response;
        }



        /// <summary>
        /// update enemy information, takes in an enemy id, only accessable to mods/admins
        /// </summary>
        /// <param name="id">EnemyID</param>
        /// <returns></returns>
        [HttpGet]
        [Security_Filter(2)]
        public ActionResult UpdateEnemy(int id)
        {
            EnemyUpdateVM enemy = new EnemyUpdateVM();
            try
            {
                //instantiating an enemyDAO

                //mapping an enemypo to our VM
                enemy.Enemy = Mapper.Mapper.EnemyDOtoPO(eDAO.ViewSingleEnemy(id));

                //instantiating an enemy_itemDAO
                EnemyItemDAO linkDAO = new EnemyItemDAO(connectionString, logPath);

                //collecting item id's linked to the enemy id
                List<EnemyItemDetailsDO> itemDrops = linkDAO.ViewByEnemyID(id);

                //assigning items 1 and 2 based on what was returned from the enemy_itemDAO
                if (itemDrops.Count == 1)
                {
                    enemy.Item1 = itemDrops[0].ItemID;
                }
                else if (itemDrops.Count == 2)
                {
                    enemy.Item1 = itemDrops[0].ItemID;
                    enemy.Item2 = itemDrops[1].ItemID;
                }
                else
                {
                    //setting default values to 1 if no items where linked to enemy

                    //default item "1" does not exist so no links will be made in the database, however modelstate will
                    //not return false.
                    enemy.Item1 = 0;
                    enemy.Item2 = 0;
                }

                //instantiating itemDAO and populating the view models item list for selection drop down list
                ItemDAO iDAO = new ItemDAO(connectionString, logPath);
                enemy.itemList = Mapper.Mapper.ItemDOListToPO(iDAO.ViewAllItems());

                //creating and adding default item "none" to drop down list
                ItemPO @default = new ItemPO
                {
                    Name = "None",
                    ItemID = 0,
                    Description = ""
                };

                enemy.itemList.Add(@default);

            }
            catch (SqlException sqlEx)
            {
                Logger.LogSqlException(sqlEx);
            }
            //passing enemy view model to view
            return View(enemy);
        }


        /// <summary>
        /// Update Enemy Method, taking in and EnemyUpdateVM form, accessable only to mod/admins
        /// </summary>
        /// <param name="form">EnemyUpdateVM</param>
        /// <returns></returns>
        [HttpPost]
        [Security_Filter(2)]
        public ActionResult UpdateEnemy(EnemyUpdateVM form)
        {
            ActionResult response = new ViewResult();

            //if imagepath is null, gives empty string value
            if (form.Enemy.ImagePath is null)
            {
                form.Enemy.ImagePath = "";
            }

            try
            {
                if (ModelState.IsValid)
                {
                    //mapping enemy info from vm to enemypo to send to server
                    EnemyPO Enemy = new EnemyPO();
                    Enemy = Mapper.Mapper.MapEnemyUpdateVMtoEnemyPO(form);

                    EnemyDO enemy = Mapper.Mapper.EnemyPOtoDO(Enemy);

                    //sending enemydo object to enemy dao, passing in the EnemyDo object
                    eDAO.UpdateEnemy(enemy);

                    //instantiate enemy_itemDAO
                    EnemyItemDAO linkDAO = new EnemyItemDAO(connectionString, logPath);

                    //collecting old item drop information and deleting it
                    List<EnemyItemIDLink> dropList = Mapper.Mapper.DetailsDOtoPO(linkDAO.ViewByEnemyID(form.Enemy.EnemyID));
                    foreach (EnemyItemIDLink item in dropList)
                    {
                        linkDAO.DeleteEnemyItems(item.LinkID);
                    }

                    if (form.Item1 == form.Item2)
                    {
                        form.Item2 = 0;
                    }

                    EnemyItemIDLink newLink = new EnemyItemIDLink();

                    newLink.EnemyID = form.Enemy.EnemyID;

                    //creating new links for item's 1 and 2, if item does not exist no link is made
                    //default item does not exist
                    newLink.ItemID = form.Item1;
                    linkDAO.CreateEnemyDetails(Mapper.Mapper.DetailsPOtoDO(newLink));

                    newLink.ItemID = form.Item2;
                    linkDAO.CreateEnemyDetails(Mapper.Mapper.DetailsPOtoDO(newLink));

                    //set response to redirect to enemies homepage
                    response = RedirectToAction("Index", "Enemy");
                }
                else
                {
                    //if form is invalid, return form information

                    //ToDo: include error message
                    response = View(form);
                }

            }
            catch (SqlException sqlEx)
            {
                //log any sql exceptions encountered
                Logger.LogSqlException(sqlEx);
            }

            return response;
        }


        //ToDo: include a view to validate deletes

        /// <summary>
        /// delete selected enemy, takes in enemies id, accessable only to admins
        /// </summary>
        /// <param name="id">EnemyID</param>
        /// <returns></returns>
        [HttpGet]
        [Security_Filter(3)]
        public ActionResult DeleteEnemy(int id)
        {
            try
            {
                //instantiate a link to enemy dao
                EnemyItemDAO linkDAO = new EnemyItemDAO(connectionString, logPath);

                //delete selected enemy
                eDAO.DeleteEnemy(id);
            }
            catch (SqlException sqlEx)
            {
                //log any encountered sql exceptions
                Logger.LogSqlException(sqlEx);
            }

            return RedirectToAction("Index", "Enemy");
        }

        /// <summary>
        /// ViewUnvalidatedEnemy method, takes in no arguments, accessable to mods and admins
        /// </summary>
        /// <returns></returns>
        [Security_Filter(2)]
        public ActionResult ViewUnvalidatedEnemy()
        {
            List<EnemyPO> enemyList = new List<EnemyPO>();
            try
            {
                //collect all enemies from database
                enemyList = Mapper.Mapper.EnemyDOListToPO(eDAO.ViewAllEnemies());
            }
            catch (SqlException sqlEx)
            {
                //log sql exceptions encountered
                Logger.LogSqlException(sqlEx);
            }
            //send enemy list to the view
            return View(enemyList);
        }

        /// <summary>
        /// partial view designed to collect item information for items dropped by an enemy and fill out an div.
        /// </summary>
        /// <param name="id">EnemyID</param>
        /// <returns></returns>
        public PartialViewResult ViewEnemyDrops(int id)
        {
            PartialViewResult response = new PartialViewResult();

            try
            {
                //instantiate a enemy_itemDAO and fill a list with item id's based on enemy id
                EnemyItemDAO linkDAO = new EnemyItemDAO(connectionString, logPath);
                List<EnemyItemDetailsDO> doList = linkDAO.ViewByEnemyID(id);


                //collect the selected enemy's information
                EnemyDO enemyDO = eDAO.ViewSingleEnemy(id);
                EnemyPO enemy = Mapper.Mapper.EnemyDOtoPO(enemyDO);

                //instantiate itemdao and populte an item List using itemID's found in doList
                List<ItemDO> dropListDO = new List<ItemDO>();
                ItemDAO iDAO = new ItemDAO(connectionString, logPath);
                foreach (EnemyItemDetailsDO item in doList)
                {
                    dropListDO.Add(iDAO.ViewItemSingle(item.ItemID));
                }
                //mapping DO droplist to PO
                List<ItemPO> dropList = Mapper.Mapper.ItemDOListToPO(dropListDO);

                //create enemy drops vm and assign enemy information and PO dropList to it
                EnemyDropsVM enemyVM = new EnemyDropsVM();
                enemyVM.Items = dropList;
                enemyVM.enemy = enemy;

                //set response PartialView to target the correct partial view, and provide EnemyDropVM 
                response = PartialView("_ViewEnemyDrops", enemyVM);
            }
            catch (SqlException sqlEx)
            {
                //log sql exceptions encountered
                Logger.LogSqlException(sqlEx);
            }

            return response;
        }

    }
}