using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Guffaw.Utilities
{
    public static class Utililities
    {

        //public static string IsSelected(this HtmlHelper html, string controllers = "", string actions = "", string cssClass = "active")
        //{
        //    var viewContext = html.ViewContext;
        //    var isChildAction = viewContext.Controller.ControllerContext.IsChildAction;

        //    if (isChildAction)
        //        viewContext = html.ViewContext.ParentActionViewContext;

        //   var routeValues = viewContext.RouteData.Values;
        //   var currentAction = routeValues["action"].ToString();
        //   var currentController = routeValues["controller"].ToString();

        //    if (string.IsNullOrEmpty(actions))
        //        actions = currentAction;

        //    if (string.IsNullOrEmpty(controllers))
        //        controllers = currentController;

        //    var acceptedActions = actions.Trim().Split(',').Distinct().ToArray();
        //    var acceptedControllers = controllers.Trim().Split(',').Distinct().ToArray();

        //    return acceptedActions.Contains(currentAction) && acceptedControllers.Contains(currentController) ?
        //        cssClass : String.Empty;
        //}


        public static string IsSelected(this HtmlHelper html, string controllers = "", string cssClass = "active")
        {
            var viewContext = html.ViewContext;
            var isChildAction = viewContext.Controller.ControllerContext.IsChildAction;

            if (isChildAction)
                viewContext = html.ViewContext.ParentActionViewContext;

            var routeValues = viewContext.RouteData.Values;
            var currentController = routeValues["controller"].ToString();

            if (string.IsNullOrEmpty(controllers))
                controllers = currentController;

            var acceptedControllers = controllers.Trim().Split(',').Distinct().ToArray();

            return acceptedControllers.Contains(currentController) ?
                cssClass : String.Empty;
        }

        public static string TruncateAtWord(this string input, int length)
        {
            if (input == null || input.Length < length)
                return input;
            int iNextSpace = input.LastIndexOf(" ", length, StringComparison.Ordinal);
            return string.Format("{0}...", input.Substring(0, (iNextSpace > 0) ? iNextSpace : length).Trim());
        }

    }
}