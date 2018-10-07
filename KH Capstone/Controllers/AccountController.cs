using KH_Capstone_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KH_Capstone.Controllers
{
    public class AccountController : Controller
    {
        private UserDAO _UserDAO;
        public AccountController()
        {

        }



        public ActionResult Index()
        {
            return View();
        }
    }
}