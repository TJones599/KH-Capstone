using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KH_Capstone_DAL;
using KH_Capstone_DAL.Models;
using System.Configuration;
using System.IO;
using KH_Capstone.Models;

namespace KH_Capstone.Controllers
{
    public class EnemyController : Controller
    {
        /***ENEMY***/

        private readonly string connectionString;
        private readonly string logPath;
        private EnemyDAO eDAO;
        

        public EnemyController()
        {
            connectionString = ConfigurationManager.ConnectionStrings["dataSource"].ConnectionString;
            logPath = Path.Combine(Path.GetDirectoryName(System.Web.HttpContext.Current.Server.MapPath("~")),ConfigurationManager.AppSettings["relative"]);
            eDAO = new EnemyDAO(connectionString, logPath);
        }

        //view all
        [HttpGet]
        public ActionResult Index()
        {
            List<EnemyPO> enemyList = Mapper.Mapper.EnemyDOListToPO(eDAO.ViewAllEnemies());
            return View(enemyList);
        }

        //view single
        [HttpGet]
        public ActionResult ViewEnemy(int id)
        {
            EnemyPO enemy = Mapper.Mapper.EnemyDOtoPO(eDAO.ViewSingleEnemy(id));
            return View(enemy);
        }

        //create
        [HttpGet]
        public ActionResult NewEnemy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewEnemy(EnemyPO form)
        {
            ActionResult response = new ViewResult();
            form.Validated = false;
            form.ImagePath = "";
            if (ModelState.IsValid)
            {
                EnemyDO enemy = Mapper.Mapper.EnemyPOtoDO(form);
                eDAO.NewEnemy(enemy);
                response = RedirectToAction("Index", "Home");
            }
            else
            {
                response = View(form);
            }

            return response;
        }


        //update
        [HttpGet]
        public ActionResult UpdateEnemy(int id)
        {
            EnemyPO enemy = Mapper.Mapper.EnemyDOtoPO(eDAO.ViewSingleEnemy(id));
            return View(enemy);
        }

        [HttpPost]
        public ActionResult UpdateEnemy(EnemyPO form)
        {
            ActionResult response = new ViewResult();
            if (form.ImagePath is null)
            {
                form.ImagePath = "";
            }

            if (ModelState.IsValid)
            {
                EnemyDO enemy = Mapper.Mapper.EnemyPOtoDO(form);
                eDAO.UpdateEnemy(enemy);
                response = RedirectToAction("Index", "Home");
            }
            else
            {
                response = View(form);
            }
            return response;
        }

        //delete
        //ToDo: may want to include a view for delete
        [HttpGet]
        public ActionResult DeleteEnemy(int id)
        {
            eDAO.DeleteEnemy(id);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ViewUnvalidatedEnemy()
        {
            List<EnemyPO> enemyList = Mapper.Mapper.EnemyDOListToPO(eDAO.ViewAllEnemies());
            return View(enemyList);
        }
        


    }
}