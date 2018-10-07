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
    public class AccountController : Controller
    {
        private UserDAO _UserDAO;
        public AccountController()
        {
            string relativePath = ConfigurationManager.AppSettings["relative"];
            string logPath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~"), relativePath);

            string connectionString = ConfigurationManager.ConnectionStrings["dataSource"].ConnectionString;
            _UserDAO = new UserDAO(connectionString, logPath);
        }

        [HttpGet]
        //security filter admin level
        public ActionResult Index()
        {
            ActionResult response;
            try
            {
                List<UserPO> userListPO = Mapper.Mapper.UserDOListToPO(_UserDAO.ViewAllUsers());
                response = View(userListPO);
            }
             catch (SqlException sqlEx)
            {
                response = View();
                throw;
            }
            return response;
        }


        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Login(Login form)
        {
            ActionResult response;
            try
            {
                if(ModelState.IsValid)
                {
                    UserPO user = Mapper.Mapper.UserDOtoPO(_UserDAO.ViewByUserName(form.UserName));
                    if(user.UserID != 0)
                    {
                        if(form.Password.Equals(user.Password))
                        {
                            Session["UserName"] = user.UserName;
                            Session["Role"] = user.Role;

                            response = RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError("Password", "Username or password was incorrect");
                            response = View(form);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Password", "Username or Password was incorrect");
                        response = View(form);
                    }
                }
                else
                {
                    response = View(form);
                }
            }
            catch(SqlException sqlEx)
            {
                response = View(form);
            }
            return response;
        }

        
        [HttpGet]
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserPO form)
        {
            if(ModelState.IsValid)
            {
                UserDO newUser = Mapper.Mapper.UserPOtoDO(form);
                newUser.Role = 1;
                _UserDAO.CreateNewUser(newUser);

                Login user = new Login();
                user.Password = form.Password;
                user.UserName = form.UserName;
            }
            return RedirectToAction("Login", "Account");
        }

    }
}