using System;
using System.Web.Mvc;

namespace AjaxDialogs
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
	public class AjaxValidationFilter : FilterAttribute, IActionFilter
	{
		#region IActionFilter Members

		public virtual void OnActionExecuting(ActionExecutingContext filterContext)
		{
		}

		public virtual void OnActionExecuted(ActionExecutedContext filterContext)
		{
			if (filterContext.HttpContext.Request.IsAjaxRequest())
			{
				filterContext.HttpContext.Response.AddHeader("X-Validation-Errors",
				                                             (!filterContext.Controller.ViewData.ModelState.IsValid).ToString().
				                                             	ToLowerInvariant());
			}
		}

		#endregion
	}
}