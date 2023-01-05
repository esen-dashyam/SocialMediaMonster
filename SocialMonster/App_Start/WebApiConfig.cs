using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace SocialMonster.App_Start
{
    public class WebApiConfig : System.Web.Http.ApiController
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "Authentication",
                routeTemplate: "api/{controller}/{action}/{username}/{password}",
                defaults: new { controller = "Home", action = "Index", username = UrlParameter.Optional, password = UrlParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "Other",
                routeTemplate: "api/{controller}/{action}/{ID}",
                defaults: new { controller = "Home", action = "Index", ID = UrlParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "NewDefault",
                routeTemplate: "api/{controller}/{ID}",
                defaults: new { controller = "Home", ID = UrlParameter.Optional }
            );
        }
    }
}