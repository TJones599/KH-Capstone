using KH_Capstone.Custom;
using KH_Capstone.LoggerPL;
using KH_Capstone.Models;
using KH_Capstone_DAL;
using KH_Capstone_DAL.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web.Mvc;

namespace KH_Capstone.Controllers
{
    public class ItemController : Controller
    {
        /***Items***/
        private readonly string connectionString;
        private readonly string logPath;
        ItemDAO iDAO;

        /// <summary>
        /// gathering information for global variables, instantiating ItemDAO with newly aquired connectionString, and logPath.
        /// Setting Logger FilePath
        /// </summary>
        public ItemController()
        {
            connectionString = ConfigurationManager.ConnectionStrings["dataSource"].ConnectionString;
            logPath = Path.Combine(Path.GetDirectoryName(System.Web.HttpContext.Current.Server.MapPath("~")), ConfigurationManager.AppSettings["relative"]);
            Logger.logPath = logPath;
            iDAO = new ItemDAO(connectionString, logPath);
        }

        //view all items

        /// <summary>
        /// Index Method. Pulls all Item information from db and sends it to the view
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            List<ItemPO> itemList = new List<ItemPO>();

            //try to connect to the server and aquire all item information
            try
            {
                //mapping to ItemPO collection
                itemList = Mapper.Mapper.ItemDOListToPO(iDAO.ViewAllItems());
            }
            //catch and log any sqlExceptions encountered during db call
            catch (SqlException sqlEx)
            {
                //If previously logged, will not log
                if (!((bool)sqlEx.Data["Logged"] == true) || !sqlEx.Data.Contains("Logged"))
                {
                    Logger.LogSqlException(sqlEx);
                }
            }

            return View(itemList);
        }


        //create new item

        /// <summary>
        /// Create Item Method. Directs to Create item view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Security_Filter(1)]
        public ActionResult CreateItem()
        {
            return View();
        }

        /// <summary>
        /// Create Item Post Method. Takes in ItemPO form, and sends that information the the db
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        [Security_Filter(1)]
        public ActionResult CreateItem(ItemPO form)
        {
            ActionResult response = new ViewResult();


            //try to connect to the server and create a new item
            try
            {
                //Makes sure everything required was entered in the view
                if (ModelState.IsValid)
                {
                    form.Validated = false;
                    //looks to see which option was choosen, and sets filepath accordingly
                    switch (form.ImagePath)
                    {
                        case "Item":
                            {
                                form.ImagePath = "~/Images/Items/Item.png";
                                break;
                            }
                        case "Accessory":
                            {
                                form.ImagePath = "~/Images/Items/Accessory.png";
                                break;
                            }
                    }

                    iDAO.CreateNewItem(Mapper.Mapper.ItemPOtoDO(form));
                    response = RedirectToAction("Index", "Item");
                }
                //returns to view if modelstate was false
                else
                {
                    ModelState.AddModelError("Description", "Missing information, please fill in all fields!");
                    response = View(form);
                }
            }
            //catch and log any unloged sqlExceptions encountered during db call
            catch (SqlException sqlEx)
            {
                if (!((bool)sqlEx.Data["Logged"] == true) || !sqlEx.Data.Contains("Logged"))
                {
                    Logger.LogSqlException(sqlEx);
                }
            }

            return response;
        }

        /// <summary>
        /// Update Item Method. Takes in and Items id, pulls information from the db based on that id, and sends that information
        /// to the Update view.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Security_Filter(2)]
        public ActionResult UpdateItem(int id)
        {
            ActionResult response = new ViewResult();
            //try to connect to the db and pull the indicated users information
            try
            {
                ItemPO item = Mapper.Mapper.ItemDOtoPO(iDAO.ViewItemSingle(id));
                response = View(item);
            }
            //catch and log any unlogged sqlExceptions encountered
            catch (SqlException sqlEx)
            {
                if (!((bool)sqlEx.Data["Logged"] == true) || !sqlEx.Data.Contains("Logged"))
                {
                    Logger.LogSqlException(sqlEx);
                }
                response = RedirectToAction("Index", "Item");
            }
            return response;
        }

        /// <summary>
        /// /UpdateItem Post Method. Takes ItemPO information provided by the view form.
        /// connects to db and updates item information based on item id
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        [Security_Filter(2)]
        public ActionResult UpdateItem(ItemPO form)
        {
            ActionResult response = new ViewResult();


            //try to connect to the db and update items information
            try
            {
                //first test to see if item form was properly filled out
                if (ModelState.IsValid)
                {
                    //updates ImagePath based on selected option
                    switch (form.Type)
                    {
                        case "Item":
                            {
                                if (System.IO.File.Exists(Server.MapPath("~") + "/Images/Items/Item/" + form.Name + ".png"))
                                {
                                    form.ImagePath = "~/Images/Items/Item/" + form.Name + ".png";
                                }
                                else
                                {
                                    form.ImagePath = "~/Images/Items/Item/Item.png";
                                }
                                break;
                            }
                        case "Accessory":
                            {
                                if (System.IO.File.Exists(Server.MapPath("~") + "/Images/Items/Accessory/" + form.Name + ".png"))
                                {
                                    form.ImagePath = "~/Images/Items/Accessory/" + form.Name + ".png";
                                }
                                else
                                {
                                    form.ImagePath = "~/Images/Items/Accessory/Accessory.png";
                                }
                                break;
                            }
                    }

                    iDAO.UpdateItem(Mapper.Mapper.ItemPOtoDO(form));
                    response = RedirectToAction("Index", "Item");
                }
                //If not filled out properly, return to View, with error messages for imagepath , or a general error of missing information.
                //ImagePath is a Radio button selection
                else
                {
                    if (form.ImagePath == null)
                    {
                        ModelState.AddModelError("ImagePath", "Please select one!");
                    }
                    else
                    {
                        ModelState.AddModelError("Validated", "Missing information");
                    }
                    response = View(form);
                }
            //catch and log any unlogged sqlExceptions encountered
            }
            catch (SqlException sqlEx)
            {
                if (!((bool)sqlEx.Data["Logged"] == true) || !sqlEx.Data.Contains("Logged"))
                {
                    Logger.LogSqlException(sqlEx);
                }
                response = View(form);
            }

            return response;
        }


        //delete item
        /// <summary>
        /// DeleteItem Method. Takes in an ItemID, connects to the db, and deletes item by provided id
        /// </summary>
        /// <param name="id">ItemID</param>
        /// <returns></returns>
        [HttpGet]
        [Security_Filter(3)]
        public ActionResult DeleteItem(int id)
        {
            EnemyItemDAO linkDAO = new EnemyItemDAO(connectionString, logPath);
            //try to connect to server, and delete item by id
            try
            {
                //delete item by id
                iDAO.DeleteItem(id);
            }
            //catch and log any unlogged sqlExceptions
            catch (SqlException sqlEx)
            {
                if (!((bool)sqlEx.Data["Logged"] == true) || !sqlEx.Data.Contains("Logged"))
                {
                    Logger.LogSqlException(sqlEx);
                }
            }
            //return to Items home page
            return RedirectToAction("Index", "Item");
        }

        /// <summary>
        /// ViewUnvalidatedItems() pulls all items from db. View filters by Validated field 
        /// </summary>
        /// <returns></returns>
        [Security_Filter(2)]
        public ActionResult ViewUnvalidatedItems()
        {
            List<ItemPO> itemList = new List<ItemPO>();
            //try to connect to the db and pull item information
            try
            {
                itemList = Mapper.Mapper.ItemDOListToPO(iDAO.ViewAllItems());
            }
            //catch and log any unlogged sqlExceptions
            catch (SqlException sqlEx)
            {
                if (!((bool)sqlEx.Data["Logged"] == true) || !sqlEx.Data.Contains("Logged"))
                {
                    Logger.LogSqlException(sqlEx);
                }
            }
            return View(itemList);
        }

        /// <summary>
        /// PartialView method ViewDropsFrom(ing id) takes in an EnemyID and connects to the server
        /// pulls all enemies that drop the item id provided
        /// </summary>
        /// <param name="id">ItemID</param>
        /// <returns></returns>
        public PartialViewResult ViewDropsFrom(int id)
        {
            PartialViewResult response = new PartialViewResult();
            
            //instantiating EnemyDAO and EnemyItemDAO for later user
            EnemyDAO eDAO = new EnemyDAO(connectionString, logPath);
            EnemyItemDAO linkDAO = new EnemyItemDAO(connectionString, logPath);

            //try to connect to the database via multiple DAO's
            try
            {
                //creating and populating a list, describing Enemy and Item Relationship based on a JunctionTable
                List<EnemyItemDetailsDO> doList = linkDAO.ViewByItemID(id);

                //gathering selected items information for View Model
                ItemDO tempItem = iDAO.ViewItemSingle(id);
                ItemPO viewItem = Mapper.Mapper.ItemDOtoPO(tempItem);

                //gathering enemy information using the EnemyID's found in out Enemy Item Relationship list
                List<EnemyDO> tempEnemyList = new List<EnemyDO>();
                foreach (EnemyItemDetailsDO item in doList)
                {
                    //adding each Enemy to our EnemyDO list
                    tempEnemyList.Add(eDAO.ViewSingleEnemy(item.EnemyID));
                }

                //converting our EnemyDO List to an EnemyPO List
                List<EnemyPO> enemyList = Mapper.Mapper.EnemyDOListToPO(tempEnemyList);

                //instantiatin our View Model
                ItemDropsVM itemVM = new ItemDropsVM();

                //settingg View Model values to our collected information
                //---Item information
                //---EnemyPO List
                itemVM.item = viewItem;
                itemVM.enemies = enemyList;

                //setting our response to our target Partial View, passing in our View Model
                response = PartialView("_ViewDropsFrom", itemVM);
            }
            //catch and log any unlogged sqlExceptions
            catch (SqlException sqlEx)
            {
                if (!((bool)sqlEx.Data["Logged"] == true) || !sqlEx.Data.Contains("Logged"))
                {
                    Logger.LogSqlException(sqlEx);
                }
            }

            return response;
        }
    }
}