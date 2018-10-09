using KH_Capstone.Models;
using KH_Capstone_DAL;
using KH_Capstone_DAL.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KH_Capstone.Controllers
{
    public class ItemController : Controller
    {
        /***Items***/
        private readonly string connectionString;
        private readonly string logPath;
        ItemDAO iDAO;

        public ItemController()
        {
            connectionString = ConfigurationManager.ConnectionStrings["dataSource"].ConnectionString;
            logPath = Path.Combine(Path.GetDirectoryName(System.Web.HttpContext.Current.Server.MapPath("~")), ConfigurationManager.AppSettings["relative"]);
            iDAO = new ItemDAO(connectionString, logPath);
        }
        //view all items
        public ActionResult Index()
        {
            List<ItemPO> itemList = Mapper.Mapper.ItemDOListToPO(iDAO.ViewAllItems());
            return View(itemList);
        }

        //view single item
        public ActionResult ViewItem(int id)
        {
            ActionResult response = new ViewResult();
            try
            {
                ItemPO item = Mapper.Mapper.ItemDOtoPO(iDAO.ViewItemSingle(id));
                response = View(item);
            }
            catch(SqlException sqlEx)
            {
                response = RedirectToAction("Index", "Home");
            }
            return response;
        }


        //create new item
        [HttpGet]
        public ActionResult CreateItem()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateItem(ItemPO form)
        {
            ActionResult response = new ViewResult();
            form.Validated = false;
            form.ImagePath = "";
            if (ModelState.IsValid)
            {
                iDAO.CreateNewItem(Mapper.Mapper.ItemPOtoDO(form));
                response = RedirectToAction("Index", "Home");
            }
            else
            {
                response = View(form);
            }
            return response;
        }

        //update item

        [HttpGet]
        public ActionResult UpdateItem(int id)
        {
            ActionResult response = new ViewResult();
            try
            {
                ItemPO item = Mapper.Mapper.ItemDOtoPO(iDAO.ViewItemSingle(id));
                response = View(item);
            }
            catch(SqlException sqlEx)
            {
                response = RedirectToAction("Index", "Home");
            }
            return response;
        }

        [HttpPost]
        public ActionResult UpdateItem(ItemPO form)
        {
            ActionResult response = new ViewResult();
            try
            {
                if(form.ImagePath is null)
                {
                    form.ImagePath = "";
                }
                if (ModelState.IsValid)
                {
                    iDAO.UpdateItem(Mapper.Mapper.ItemPOtoDO(form));
                    response = RedirectToAction("Index", "Item");
                }
                else
                {
                    ModelState.AddModelError("Validated", "Missing information");
                    response = View(form);
                }
            }
            catch (SqlException sqlEx)
            {
                response = View(form);
            }
            return response;
        }
        //delete item

        [HttpGet]
        public ActionResult DeleteItem(int id)
        {
            try
            {
                iDAO.DeleteItem(id);
               
            }
            catch(SqlException sqlEx)
            {
                //ToDo: Logger
            }
            return RedirectToAction("Index", "Home");
        }



        public ActionResult ViewUnvalidatedItems()
        {
            List<ItemPO> itemList = Mapper.Mapper.ItemDOListToPO(iDAO.ViewAllItems());
            return View(itemList);
        }
    }
}