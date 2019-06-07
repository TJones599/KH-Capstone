using KH_Capstone.Custom;
using KH_Capstone.LoggerPL;
using KH_Capstone.Models;
using KH_Capstone_BLL;
using KH_Capstone_DAL;
using KH_Capstone_DAL.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web.Mvc;

namespace KH_Capstone.Controllers
{
    public class AccountController : Controller
    {
        private readonly string connectionString;
        private readonly string logPath;
        private UserDAO _UserDAO;

        public AccountController()
        {
            string relativePath = ConfigurationManager.AppSettings["relative"];
            logPath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~"), relativePath);
            Logger.logPath = logPath;
            connectionString = ConfigurationManager.ConnectionStrings["dataSource"].ConnectionString;
            _UserDAO = new UserDAO(connectionString, logPath);
        }

        /// <summary>
        /// Views all user accounts
        /// </summary>
        /// <returns>VIew</returns>
        [HttpGet]
        [SecurityFilter(2)]
        public ActionResult Index()
        {
            ActionResult response;
            //try to connect to the server, and mape a list of users to a List<UserPO>
            try
            {
                List<UserPO> userListPO = Mapper.Mapper.UserDOListToPO(_UserDAO.ViewAllUsers());
                response = View(userListPO);
            }
            //catching any SqlExceptions we may encounter in our db call
            catch (SqlException sqlEx)
            {
                if (!((bool)sqlEx.Data["Logged"] == true) || !sqlEx.Data.Contains("Logged"))
                {
                    Logger.LogSqlException(sqlEx);
                }
                response = RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                if (!ex.Data.Contains("Logged") || (bool)ex.Data["Logged"] == false)
                {
                    Logger.LogException(ex);
                }
                response = RedirectToAction("Index", "Home");
            }
            return response;
        }

        /// <summary>
        /// Gathers the information associated with the logged in user, and sends it to the view
        /// </summary>
        /// <returns>View</returns>
        [HttpGet]
        [SecurityFilter(1)]
        public ActionResult AccountView()
        {
            UserPO userInfo = new UserPO();
            //try to connect to the db, collect the users information (filtered by UserName) and map it to a UserPO
            try
            {
                userInfo = Mapper.Mapper.UserDOtoPO(_UserDAO.ViewByUserName(Session["UserName"].ToString()));
            }
            //catching any sqlExceptions we may encounter in our db call
            catch (SqlException sqlEx)
            {
                if (!((bool)sqlEx.Data["Logged"] == true) || !sqlEx.Data.Contains("Logged"))
                {
                    Logger.LogSqlException(sqlEx);
                }
            }
            catch (Exception ex)
            {
                if (!ex.Data.Contains("Logged") || (bool)ex.Data["Logged"] == false)
                {
                    Logger.LogException(ex);
                }
            }
            return View(userInfo);
        }


        /// <summary>
        /// Update a user, filtered by UserID provided. Returns a View. Only Accessable when logged in
        /// </summary>
        /// <param name="id">UserID</param>
        /// <returns></returns>
        [HttpGet]
        [SecurityFilter(1)]
        public ActionResult UpdateUser(int id)
        {
            ActionResult response;

            //creating a user view model including a UserPO, and a list of Roles
            UserUpdateVM user = new UserUpdateVM();

            //try to connect to the db and aquire the user information filtered by the id provided, and a List of all Roles
            try
            {
                UserDO temp = _UserDAO.ViewSingleUser(id);
                user.User = Mapper.Mapper.UserDOtoPO(temp);
                user.Roles = Mapper.Mapper.RoleDOListToPOList(_UserDAO.GetRoleList());
            }
            //catch any SqlExceptions we encounter during our db calls
            catch (SqlException sqlEx)
            {
                if (!((bool)sqlEx.Data["Logged"] == true) || !sqlEx.Data.Contains("Logged"))
                {
                    Logger.LogSqlException(sqlEx);
                }
            }
            catch (Exception ex)
            {
                if (!ex.Data.Contains("Logged") || (bool)ex.Data["Logged"] == false)
                {
                    Logger.LogException(ex);
                }
            }

            //Prevents users from altering someone elses information, unless they are a higher role than the user
            //being updated
            if (user.User.UserName == Session["UserName"].ToString() || (int)Session["Role"] > user.User.Role)
            {
                response = View(user);
            }
            else
            {
                response = RedirectToAction("Index", "Home");
            }

            return response;
        }


        /// <summary>
        /// UpdateUser Post side. Takes in a UserUpdateVM and sends the user information to the userDAO
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        [SecurityFilter(1)]
        public ActionResult UpdateUser(UserUpdateVM form)
        {
            ActionResult response;

            if (ModelState.IsValid)
            {
                //FirstName and LastName are optional, however cannot be null. This sets them to an empty string if they are null
                if (form.User.FirstName == null)
                {
                    form.User.FirstName = "";
                }
                if (form.User.LastName == null)
                {
                    form.User.LastName = "";
                }

                //try to connect to the server, and update users information
                try
                {
                    UserDO user = Mapper.Mapper.UserPOtoDO(form.User);
                    _UserDAO.UpdateUser(user);
                }
                //catch any sqlExceptions encountered during db call and log them
                catch (SqlException sqlEx)
                {
                    if (!((bool)sqlEx.Data["Logged"] == true) || !sqlEx.Data.Contains("Logged"))
                    {
                        Logger.LogSqlException(sqlEx);
                    }
                }
                catch (Exception ex)
                {
                    if (!ex.Data.Contains("Logged") || (bool)ex.Data["Logged"] == false)
                    {
                        Logger.LogException(ex);
                    }
                }

                //redirects based on if the user is updating their own account, or someone elses.
                //redirects mods and admins to the View all users page if they are updating someone elses account information
                if (Session["UserName"].ToString() != form.User.UserName)
                {
                    response = RedirectToAction("Index", "Account");
                }
                else
                {
                    response = RedirectToAction("AccountView", "Account");
                }
            }
            else
            {
                ModelState.AddModelError("", "Missing information, please enter all fields.");
                response = View(form);
            }

            return response;
        }

        /// <summary>
        /// DeleteUser method. Takes in a UserID, connects to the db and deletes that user from the db
        /// </summary>
        /// <param name="id">UserID</param>
        /// <returns></returns>
        [HttpGet]
        [SecurityFilter(1)]
        public ActionResult DeleteUser(int id)
        {
            ActionResult response = new ViewResult();
            //try to connect to the server and delete the supplied UserID
            try
            {
                UserDO user = _UserDAO.ViewSingleUser(id);
                _UserDAO.DeleteUser(id);

                //if User deleted is currently logged in, abandon session
                if (Session["UserName"].ToString() == user.UserName)
                {
                    Session.Abandon();
                }
            }
            //catch and log any sqlExceptions encountered during the db call
            catch (SqlException sqlEx)
            {
                if (!((bool)sqlEx.Data["Logged"] == true) || !sqlEx.Data.Contains("Logged"))
                {
                    Logger.LogSqlException(sqlEx);
                }
            }
            catch (Exception ex)
            {
                if (!ex.Data.Contains("Logged") || (bool)ex.Data["Logged"] == false)
                {
                    Logger.LogException(ex);
                }
            }

            //Redirects based on Role, Admins are redirected to the view all users page, 
            //otherwise user is redirected to home page
            if (Session["RoleName"].ToString() == "Admin")
            {
                response = RedirectToAction("Index", "Account");
            }
            else
            {
                response = RedirectToAction("Index", "Home");
            }
            return response;
        }

        [HttpGet]
        [SecurityFilter(1)]
        public ActionResult UpdateAccountStatus(int id, bool statusUpdate)
        {
            ActionResult response = new ViewResult();
            //try to connect to the server and delete the supplied UserID
            try
            {
                UserDO user = _UserDAO.ViewSingleUser(id);
                _UserDAO.AccountStatus(id, statusUpdate);

                //if User deleted is currently logged in, abandon session
                if (Session["UserName"].ToString() == user.UserName)
                {
                    Session.Abandon();
                }
            }
            //catch and log any sqlExceptions encountered during the db call
            catch (SqlException sqlEx)
            {
                if (!((bool)sqlEx.Data["Logged"] == true) || !sqlEx.Data.Contains("Logged"))
                {
                    Logger.LogSqlException(sqlEx);
                }
            }
            catch (Exception ex)
            {
                if (!ex.Data.Contains("Logged") || (bool)ex.Data["Logged"] == false)
                {
                    Logger.LogException(ex);
                }
            }

            //Redirects based on Role, Admins are redirected to the view all users page, 
            //otherwise user is redirected to home page
            if (Session["RoleName"].ToString() == "Admin")
            {
                response = RedirectToAction("Index", "Account");
            }
            else
            {
                response = RedirectToAction("Index", "Home");
            }
            return response;
        }

        /// <summary>
        /// Login, returns View() prompting user to log in
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Login Post, takes in Login form, test if entries provided were valid, if user exists, and if
        /// password provided matches users stored password. Sets all Session information
        /// </summary>
        /// <param name="form">Login Form</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(Login form)
        {
            ActionResult response;
            //checks to see if username and password were properly filled out
            if (ModelState.IsValid)
            {
                //try to connect to the db and aquire user information, based on username entered
                try
                {
                    UserPO user = Mapper.Mapper.UserDOtoPO(_UserDAO.ViewByUserName(form.UserName));
                    //if user marked banned or inactive, returns view, passing back the form and error message
                    //if user id = 0, user does not exist. returns to view, passing back the form and error message
                    if (user.UserID != 0)
                    {
                        byte[] tempHash = Hashing.GenerateSHA256Hash(form.Password, user.Salt);
                        bool passwordsMatch = Hashing.CompareByteArray(user.Password, tempHash);

                        //ToDo: unhash password
                        //tests if users stored password matches the one entered. if false, returns to view passing back the form and error message
                        if (passwordsMatch)
                        {

                            if (!user.Banned && !user.Inactive)
                            {
                                //setting all Session information required.
                                Session["UserName"] = user.UserName;
                                Session["Role"] = user.Role;
                                Session["RoleName"] = user.RoleName;
                                Session["FirstName"] = user.FirstName;
                                Session["LastName"] = user.LastName;
                                Session["Banned"] = user.Banned;

                                //redirecting to home page
                                response = RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                //Provides an error message, informing the user the account they attempted to access is banned
                                string errorMessage = "User: " + user.UserName + " has been banned, or marked inactive!";
                                ModelState.AddModelError("Password", errorMessage);
                                response = View(form);
                            }
                        }
                        else
                        {
                            //providing an error message if username or password is incorrect and returning to view
                            ModelState.AddModelError("Password", "Username or password was incorrect");
                            response = View(form);
                        }
                    }
                    else
                    {
                        //providing an error message if username or password is incorrect and returning to view
                        ModelState.AddModelError("Password", "Username or Password was incorrect");
                        response = View(form);
                    }
                }
                //catch and log sqlExceptions encountered
                catch (SqlException sqlEx)
                {
                    if (!((bool)sqlEx.Data["Logged"] == true) || !sqlEx.Data.Contains("Logged"))
                    {
                        Logger.LogSqlException(sqlEx);
                    }
                    response = View(form);
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
            else
            {
                //returning to view if modelstate invalid
                ModelState.AddModelError("Password", "Username or Password is incorrect!");
                response = View(form);
            }
            return response;
        }

        /// <summary>
        /// Logout Method. Abandon's Session and returns to home page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SecurityFilter(1)]
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Register Method. Directs to Registration Page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Register Post Method. takes in a UserPO and creates a new user in the db.
        /// </summary>
        /// <param name="form">UserPO</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Register(RegisterVM form)
        {
            ActionResult response;
            //tests to see if form was fully filled out
            if (ModelState.IsValid)
            {
                UserDO userDO = _UserDAO.ViewByUserName(form.UserName);
                //try to connect to the server and create a new user
                if (userDO.UserId == 0)
                {
                    try
                    {
                        UserPO user = new UserPO();
                        //if newpassword and passwordConfirmation match, set password = newpassword. otherwise return to view, passing back 
                        //the entered form and an error message.
                        user.Salt = Hashing.CreateSalt(10);
                        user.Password = Hashing.GenerateSHA256Hash(form.NewPassword, user.Salt);

                        user.FirstName = form.FirstName;
                        user.LastName = form.LastName;
                        //FirstName and LastName are optional cannot be null. Sets them to empty string if null
                        if (user.FirstName == null)
                        {
                            user.FirstName = "";
                        }
                        if (user.LastName == null)
                        {
                            user.LastName = "";
                        }

                        //setting default values for Role, Banned and RoleName.
                        user.Role = 1;
                        user.Banned = false;
                        user.RoleName = "User";
                        user.Inactive = false;
                        user.UserName = form.UserName;

                        //mapping to UserDO and creating new user in the db
                        UserDO newUser = Mapper.Mapper.UserPOtoDO(user);
                        _UserDAO.CreateNewUser(newUser);

                        //Automaticly logging newly registered user in, setting all Session information needed
                        Session["UserName"] = newUser.UserName;
                        Session["Role"] = newUser.Role;
                        Session["RoleName"] = newUser.RoleName;
                        Session["FirstName"] = newUser.FirstName;
                        Session["LastName"] = newUser.LastName;
                        Session["Banned"] = newUser.Banned;

                        //setting response to return to home page
                        response = RedirectToAction("Index", "Home");

                    }
                    //Catch and log any sqlExceptions encountered during DB call
                    catch (SqlException sqlEx)
                    {
                        if (!((bool)sqlEx.Data["Logged"] == true) || !sqlEx.Data.Contains("Logged"))
                        {
                            Logger.LogSqlException(sqlEx);
                        }
                        response = RedirectToAction("Index", "Home");
                    }
                    catch (Exception ex)
                    {
                        if (!ex.Data.Contains("Logged") || (bool)ex.Data["Logged"] == false)
                        {
                            Logger.LogException(ex);
                        }
                        response = RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("UserName", "UserName already exists!");
                    response = View(form);
                }

            }
            else
            {
                ModelState.AddModelError("PasswordConfirmation", "Missing information, please fill out all fields.");
                response = View(form);
            }

            return response;
        }

        [HttpGet]
        [SecurityFilter(1)]
        public ActionResult UpdatePassword(int id)
        {
            return View();
        }

        [HttpPost]
        [SecurityFilter(1)]
        public ActionResult UpdatePassword(UpdatePasswordVM form)
        {
            ActionResult response = new ViewResult();
            if (ModelState.IsValid)
            {
                UserPO user = Mapper.Mapper.UserDOtoPO(_UserDAO.ViewByUserName(Session["UserName"].ToString()));

                byte[] oldPassword = Hashing.GenerateSHA256Hash(form.OldPassword, user.Salt);
                bool passwordsMatch = Hashing.CompareByteArray(oldPassword, user.Password);

                if (passwordsMatch)
                {

                    if (form.NewPassword == form.PasswordConfirmation)
                    {
                        user.Salt = Hashing.CreateSalt(10);
                        user.Password = Hashing.GenerateSHA256Hash(form.NewPassword, user.Salt);
                    }

                    try
                    {
                        UserDO userDO = Mapper.Mapper.UserPOtoDO(user);
                        _UserDAO.UpdateUser(userDO);
                        response = RedirectToAction("AccountView", "Account");
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
                        response = View(form);
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
            }
            else
            {
                ModelState.AddModelError("PasswordConfirmation", "Invalid entry. Please check your entries and try again.");
                return View(form);
            }
            return response;
        }

    }
}