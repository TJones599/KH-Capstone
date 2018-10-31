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
using System;

namespace KH_Capstone.Controllers
{
    public class EnemyController : Controller
    {
        //global strings holding pathing information
        private readonly string connectionString;
        private readonly string logPath;
        private EnemyDAO eDAO;
        private ItemDAO iDAO;
        private EnemyItemDAO linkDAO;

        //controller to gather pathing information and set log path
        public EnemyController()
        {
            connectionString = ConfigurationManager.ConnectionStrings["dataSource"].ConnectionString;
            logPath = Path.Combine(Path.GetDirectoryName(System.Web.HttpContext.Current.Server.MapPath("~")), ConfigurationManager.AppSettings["relative"]);
            Logger.logPath = logPath;
            eDAO = new EnemyDAO(connectionString, logPath);
            iDAO = new ItemDAO(connectionString, logPath);
            linkDAO = new EnemyItemDAO(connectionString, logPath);
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
            //catching any thrown sqlexceptions from the dao
            catch (SqlException sqlEx)
            {
                //checks to see if the exception has been logged
                if (!sqlEx.Data.Contains("Logged") || (bool)sqlEx.Data["Logged"] == false)
                {
                    //if exception has not been logged, log it
                    Logger.LogSqlException(sqlEx);
                }
            }
            //catches any non sql excepions that may be throw
            catch (Exception ex)
            {
                //checks to see if the exception has been logged
                if (!ex.Data.Contains("Logged") || (bool)ex.Data["Logged"] == false)
                {
                    //logs unlogged exceptions
                    Logger.LogException(ex);
                }
            }

            //returns the index View() with the list of enemies pulled from the database
            return View(enemyList);
        }


        /// <summary>
        /// create new enemy, only accessable by registered users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SecurityFilter(1)]
        public ActionResult NewEnemy()
        {
            return View();
        }


        /// <summary>
        /// Create enemy post, takes in a enemyPO form, from the create enemy view
        /// </summary>
        /// <param name="form">EnemyPO</param>
        /// <returns></returns>
        [HttpPost]
        [SecurityFilter(1)]
        public ActionResult NewEnemy(CreateEnemyVM form)
        {
            ActionResult response;
            //checking for valid form entry
            if (ModelState.IsValid)
            {
                //pulling enemy info from server based on name entered in the view form
                EnemyDO enemyExists = eDAO.ViewEnemyByName(form.Enemy.Name);
                //if the enemy info pulled contains an id that is not 0(meaning it does not exist) then it jumps to the else statement
                if (enemyExists.EnemyID == 0)
                {
                    //try catch to capture any sql errors
                    try
                    {
                        //setting default enemy.validated value to false, prevents it showing up on the enemy list until validated
                        form.Enemy.Validated = false;

                        //checks to see if a image was uploaded during enemy creation
                        if (form.File != null && form.File.ContentLength > 0)
                        {
                            //setts enemys image path for the database
                            form.Enemy.ImagePath = "~/Images/Enemies/" + form.Enemy.Name + ".png";
                            //creates a filepath based on the enemies imagepath
                            string path = Server.MapPath(form.Enemy.ImagePath);
                            //saves the uploaded image to the filepath provided
                            form.File.SaveAs(path);
                        }
                        //if no image was uploaded
                        else
                        {
                            //finds the filepath for the default image
                            string oldPath = System.Web.HttpContext.Current.Server.MapPath("~/Images/Enemies/heartless_emblem.png");
                            //sets the filepath for the new enemies image
                            string newPath = System.Web.HttpContext.Current.Server.MapPath("~/Images/Enemies/" + form.Enemy.Name + ".png");

                            //if a filealready exists named the same as the created enemy, delete that image
                            if (System.IO.File.Exists(newPath))
                            {
                                System.IO.File.Delete(newPath);
                            }

                            //copy the default image and resave it under the enemies name
                            System.IO.File.Copy(oldPath, newPath);
                            //set the new enemies image path for the database
                            form.Enemy.ImagePath = "~/Images/Enemies/" + form.Enemy.Name + ".png";
                        }

                        //mapping po object to do and passing it to dao
                        EnemyDO enemy = Mapper.Mapper.EnemyPOtoDO(form.Enemy);
                        eDAO.NewEnemy(enemy);

                        //redirecting to enemy page
                        response = RedirectToAction("Index", "Enemy");
                    }
                    catch (SqlException sqlEx)
                    {
                        //logging any sql exceptions caught
                        if (!sqlEx.Data.Contains("Logged") || (bool)sqlEx.Data["Logged"] == false)
                        {
                            Logger.LogSqlException(sqlEx);
                        }

                        response = View(form);
                        //update response
                    }
                    catch (Exception ex)
                    {
                        if (!ex.Data.Contains("Logged") || (bool)ex.Data["Logged"] == false)
                        {
                            Logger.LogException(ex);
                        }
                        response = View(form);
                    }
                }
                //if the enemy name entered in the view form already exists
                else
                {
                    //set a modelstate error telling the user the enemy already exists
                    ModelState.AddModelError("Name", "Enemy already exists!");
                    //and return to the view with all the form information
                    response = View(form);
                }
            }
            else
            {
                //if form was not valid, returns to form, supplying the information entered previously
                ModelState.AddModelError("Description", "Missing information, please fill in all fields.");
                response = View(form);
            }

            return response;
        }

        /// <summary>
        /// update enemy information, takes in an enemy id, only accessable to mods/admins
        /// </summary>
        /// <param name="id">EnemyID</param>
        /// <returns></returns>
        [HttpGet]
        [SecurityFilter(2)]
        public ActionResult UpdateEnemy(int id)
        {
            ActionResult response;
            EnemyUpdateVM enemy = new EnemyUpdateVM();
            try
            {
                //mapping an enemypo to our VM
                enemy.Enemy = Mapper.Mapper.EnemyDOtoPO(eDAO.ViewSingleEnemy(id));

                //instantiating an enemy_itemDAO
                EnemyItemDAO linkDAO = new EnemyItemDAO(connectionString, logPath);

                //collecting item id's linked to the enemy id
                List<EnemyItemDO> itemDrops = linkDAO.ViewByEnemyID(id);

                //assigning items 1 and 2 based on what was returned from the enemy_itemDAO
                if (itemDrops.Count >= 1)
                {
                    //sets Item1 to the itemid from the link table in the database
                    enemy.Item1 = itemDrops[0].ItemID;
                    //checks to see if there was a second item linked to the enemy
                    if (itemDrops.Count == 2)
                    {
                        //sets Item2 to the second items id
                        enemy.Item2 = itemDrops[1].ItemID;
                    }
                }
                //setting default values to 0 if no items where linked to enemy
                else
                {
                    enemy.Item1 = 0;
                    enemy.Item2 = 0;
                }


                //creating and adding default item "none" to drop down list
                ItemPO @default = new ItemPO
                {
                    Name = "None",
                    ItemID = 0,
                    Description = ""
                };

                enemy.itemList.Add(@default);

                //adds list to existing list, does not delete old information.
                enemy.itemList.AddRange(Mapper.Mapper.ItemDOListToPO(iDAO.ViewAllItems()));

                response = View(enemy);
            }
            //catches any slqexceptions that occur during our DAO call
            catch (SqlException sqlEx)
            {
                //logs the exception IF it has not already been marked as logged
                if (!sqlEx.Data.Contains("Logged") || (bool)sqlEx.Data["Logged"] == false)
                {
                    Logger.LogSqlException(sqlEx);
                }
                //redirects to the index page of the enemy controller. The page containing the full list of enemies.
                response = RedirectToAction("Index", "Enemy");
            }
            //catches any non-sqlexceptions that may occure during mapping or otherwise
            catch (Exception ex)
            {
                //logs the exception if it has not already been marked as logged
                if (!ex.Data.Contains("Logged") || (bool)ex.Data["Logged"] == false)
                {
                    Logger.LogException(ex);
                }
                //redirects to the index page of the enemy controller. The page containing the full list of enemies.
                response = RedirectToAction("Index", "Enemy");
            }
            //passing enemy view model to view
            return response;
        }

        /// <summary>
        /// Update Enemy Method, taking in and EnemyUpdateVM form, accessable only to mod/admins
        /// </summary>
        /// <param name="form">EnemyUpdateVM</param>
        /// <returns></returns>
        [HttpPost]
        [SecurityFilter(2)]
        public ActionResult UpdateEnemy(EnemyUpdateVM form)
        {
            ActionResult response;

            //checks to make sure that all required fields are filled out
            if (ModelState.IsValid)
            {
                //if all fields are filled out, try to connect to the server
                try
                {
                    //if a file was submited, update the image of the enemy to the given file, and rename the file to the 
                    //enemies name
                    if (form.File != null && form.File.ContentLength > 0)
                    {
                        //if the enemy already has an image, delete that image
                        if (System.IO.File.Exists(Server.MapPath(form.Enemy.ImagePath)))
                        {
                            System.IO.File.Delete(Server.MapPath(form.Enemy.ImagePath));
                        }
                        //find the filepath for the enemies image
                        string path = Server.MapPath(form.Enemy.ImagePath);
                        //saves the new image to the enemies image filepath
                        form.File.SaveAs(path);
                    }

                    //map the new enemy information to an EnemyDO
                    EnemyDO enemy = Mapper.Mapper.EnemyPOtoDO(form.Enemy);

                    //sending enemydo object to enemy dao, passing in the EnemyDo object
                    eDAO.UpdateEnemy(enemy);

                    //instantiate enemy_itemDAO

                    //collecting old item drop information and deleting it
                    List<EnemyItemIDLink> dropList = Mapper.Mapper.DetailsDOtoPO(linkDAO.ViewByEnemyID(form.Enemy.EnemyID));

                    //for every item linked to the enemy, delete that link. Destroys old enemy drop information
                    foreach (EnemyItemIDLink item in dropList)
                    {
                        linkDAO.DeleteEnemyItems(item.LinkID);
                    }

                    //if item1 = item2, default item2 to 0. prevents enemy from having 2 links to 1 item.
                    if (form.Item1 == form.Item2)
                    {
                        form.Item2 = 0;
                    }

                    //instantiats a new EnemyItemIDLink (object holds the information about which item drops form the enemy)
                    EnemyItemIDLink newLink = new EnemyItemIDLink();
                    //sets the newLinks enemyid to the id of the id of the enemy being updated
                    newLink.EnemyID = form.Enemy.EnemyID;

                    //creating new links for item 1 if one was selected. if the item does not exist, no link is made
                    if (form.Item1 != 0)
                    {
                        //sets the newLinks item id to the new item id found in Item1
                        newLink.ItemID = form.Item1;
                        //creates a new link between the enemy and the item. Enemy will now display the item as a drop
                        linkDAO.CreateEnemyDetails(Mapper.Mapper.DetailsPOtoDO(newLink));
                    }

                    //creating new links for item 2 if one was selected. if the item does not exist, no link is made
                    if (form.Item2 != 0)
                    {
                        //sets the newLink item id to the id found in Item2
                        newLink.ItemID = form.Item2;
                        //creates a new link between the enemy and item. Enemy will now display the item as a drop
                        linkDAO.CreateEnemyDetails(Mapper.Mapper.DetailsPOtoDO(newLink));
                    }

                    //set response to redirect to enemies homepage
                    response = RedirectToAction("Index", "Enemy");

                }
                //log any sql exceptions encountered
                catch (SqlException sqlEx)
                {
                    //checks to see if the exception has been logged, and logs it if it hasn't been
                    if (!sqlEx.Data.Contains("Logged") || (bool)sqlEx.Data["Logged"] == false)
                    {
                        Logger.LogSqlException(sqlEx);
                    }
                    //redirects to the enemy list page
                    response = RedirectToAction("Index", "Enemy");
                }
                //catches any exceptions that occure due to mapping or otherwise
                catch (Exception ex)
                {
                    //checks to see if the exception has been logged, and logs it if it hasn't been
                    if (!ex.Data.Contains("Logged") || (bool)ex.Data["Logged"] == false)
                    {
                        Logger.LogException(ex);
                    }
                    //redirects to the enemy list page
                    response = RedirectToAction("Index", "Enemy");
                }
            }
            //if modelstate is false
            else
            {
                //refills the item list for the drop down menus
                form.itemList = Mapper.Mapper.ItemDOListToPO(iDAO.ViewAllItems());

                //adds teh default item of "None" to the list in case the enemy doesn't drop an itme
                ItemPO @default = new ItemPO
                {
                    Name = "None",
                    ItemID = 0,
                    Description = ""
                };
                form.itemList.Add(@default);

                //creates a modelstate error stating that the form is missing information
                ModelState.AddModelError("Validated", "Missing information. Please fill in all fields.");
                //sets response to the View() and passes in the old form information including the newly populated item list
                response = View(form);
            }

            return response;
        }

        /// <summary>
        /// delete selected enemy, takes in enemies id, accessable only to admins
        /// </summary>
        /// <param name="id">EnemyID</param>
        /// <returns></returns>
        [HttpGet]
        [SecurityFilter(3)]
        public ActionResult DeleteEnemy(int id)
        {
            //trys to connect to the server through the EnemyDAO
            try
            {
                //delete selected enemy based on the id passed in
                eDAO.DeleteEnemy(id);
            }
            //Catch any sql exceptions that may occure
            catch (SqlException sqlEx)
            {
                //check to see if the exception has been logged, log it if it hasn't been
                if (!sqlEx.Data.Contains("Logged") || (bool)sqlEx.Data["Logged"] == false)
                {
                    Logger.LogSqlException(sqlEx);
                }
            }
            //always redirect to the enemy list page
            return RedirectToAction("Index", "Enemy");
        }

        /// <summary>
        /// ViewUnvalidatedEnemy method, takes in no arguments, accessable to mods and admins
        /// </summary>
        /// <returns></returns>
        [SecurityFilter(2)]
        public ActionResult ViewUnvalidatedEnemy()
        {
            //instantiating our response and our enemyList
            ActionResult response;
            List<EnemyPO> enemyList = new List<EnemyPO>();

            //try to connect to the server
            try
            {
                //collect all enemies from database
                enemyList = Mapper.Mapper.EnemyDOListToPO(eDAO.ViewAllEnemies());
                //pass that enemyList to the view if no exceptions were encountered
                response = View(enemyList);
            }
            //Catch any sql exceptions encountered
            catch (SqlException sqlEx)
            {
                //check to see if the exception has already been logged, log it if it hasn't been
                if (!sqlEx.Data.Contains("Logged") || (bool)sqlEx.Data["Logged"] == false)
                {
                    Logger.LogSqlException(sqlEx);
                }
                //redirect to the enemy list page
                response = RedirectToAction("Index", "Home");
            }
            //catch any non-sqlexceptions encountered
            catch (Exception ex)
            {
                //check to see if the exception has already been logged, and log it if it hasn't been
                if (!ex.Data.Contains("Logged") || (bool)ex.Data["Logged"] == false)
                {
                    Logger.LogException(ex);
                }
                //redirect to the enemy list page
                response = RedirectToAction("Index", "Home");
            }
            return response;
        }

        /// <summary>
        /// partial view designed to collect item information for items dropped by an enemy and fill out an div.
        /// </summary>
        /// <param name="id">EnemyID</param>
        /// <returns></returns>
        //[AjaxOnly] prevents access to the page unless it has been called via an ajax request
        [AjaxOnly]
        public PartialViewResult ViewEnemyDrops(int id)
        {
            //instantiating our response 
            PartialViewResult response;

            try
            {
                //instantiate a enemy_itemDAO and fill a list with item id's based on enemy id
                EnemyItemDAO linkDAO = new EnemyItemDAO(connectionString, logPath);
                List<EnemyItemDO> enemyDropList = linkDAO.ViewByEnemyID(id);

                //instantiate itemdao and populte an item List using itemID's found in enemyDropList
                List<ItemDO> dropListDO = new List<ItemDO>();
                ItemDAO iDAO = new ItemDAO(connectionString, logPath);
                foreach (EnemyItemDO item in enemyDropList)
                {
                    dropListDO.Add(iDAO.ViewItemSingle(item.ItemID));
                }

                //mapping DO droplist to PO
                List<ItemPO> dropList = Mapper.Mapper.ItemDOListToPO(dropListDO);

                //create enemy drops vm and assign enemy information and PO dropList to it
                EnemyDropsVM enemyVM = new EnemyDropsVM();
                enemyVM.Items = dropList;
                enemyVM.enemyID = id;

                //set response PartialView to target the correct partial view, and provide EnemyDropVM 
                response = PartialView("_ViewEnemyDrops", enemyVM);
            }
            //Catch any sql exceptions encountered
            catch (SqlException sqlEx)
            {
                //check to see if the exception has already been logged. If not, then log it
                if (!sqlEx.Data.Contains("Logged") || (bool)sqlEx.Data["Logged"] == false)
                {
                    Logger.LogSqlException(sqlEx);
                }
                //set response to a blank partialview();
                response = PartialView();
            }
            //catch any non-sqlexception encountered
            catch (Exception ex)
            {
                //check to see if the exception has been logged. If not, then log it
                if (!ex.Data.Contains("Logged)") || (bool)ex.Data["Logged"] == false)
                {
                    Logger.LogException(ex);
                }
                //set response to a partialview();
                response = PartialView();
            }

            return response;
        }
    }
}