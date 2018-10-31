using System.Web.Mvc;

namespace KH_Capstone.Custom
{
    public class AjaxOnly : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if(!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new RedirectResult("/Home/Index",false);
            }
        }
    }
}