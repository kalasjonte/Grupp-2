using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Grupp_2
{
    public class MvcApplication : System.Web.HttpApplication
    {
        //När applikationen startar kommer denna fil kallas på.
        //I detta fall berättar vi för Runtime vilka Routes vi har i applikationen.
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DbContext>()); //Säger åt Framework hur den ska starta upp och hantera ändringar
        }
    }
}
