using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Grupp_2
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //Här har vi en Rout med namnet "Default". En router ansvarar för att skapa kopplingen mellan rätt controller till inkommande HTTP-request.
            routes.MapRoute(
                name: "Default",
                
                //Om en URL matchar med detta URL-mönstret:
                //Första delen "{controller}" antas vara namnet på controller:n
                //Andra delen "{action}" antas vara namnet på action:en (controllerns method som ska hantera http-requesten)
                //Tredje delen "{id}" är ett ID som vi kan passa till action:en
                url: "{controller}/{action}/{id}",
                
                //Om URL:en INTE innehåller NÅGRA av ovanstående delar så passar vi till defualt: controller = "Home".
                //Om URL:en BARA definierar {controller} men inte {action}, så passar vi till defualt: action = "Index".
                //{id} i detta fall är optianal: "id = UrlParameter.Optional".
                defaults: new { controller = "School", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
