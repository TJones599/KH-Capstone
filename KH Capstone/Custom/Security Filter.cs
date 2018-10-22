using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KH_Capstone.Custom
{
    public class Security_Filter : ActionFilterAttribute
    {
        private readonly int _Role;
        public Security_Filter(int role)
        {
            _Role = role;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            HttpSessionStateBase session = filterContext.HttpContext.Session;

            if ((session["Role"]) is null || (session["Role"] != null && (int)session["Role"] < _Role))
            {
                filterContext.Result = new RedirectResult("/Account/Login",false);
            }

            base.OnActionExecuted(filterContext);
        }








    }
}