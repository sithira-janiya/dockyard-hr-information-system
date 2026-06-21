using System;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WebApplication1
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            UnityConfig.RegisterComponents();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            string origin = HttpContext.Current.Request.Headers["Origin"];

            string allowedOriginsSetting =
                ConfigurationManager.AppSettings["DOCKYARD_HR_ALLOWED_ORIGINS"]
                ?? Environment.GetEnvironmentVariable("DOCKYARD_HR_ALLOWED_ORIGINS")
                ?? "http://localhost:3000";

            string[] allowedOrigins = allowedOriginsSetting
                .Split(',')
                .Select(item => item.Trim())
                .Where(item => !string.IsNullOrWhiteSpace(item))
                .ToArray();

            if (!string.IsNullOrWhiteSpace(origin) && allowedOrigins.Contains(origin))
            {
                HttpContext.Current.Response.Headers.Set("Access-Control-Allow-Origin", origin);
                HttpContext.Current.Response.Headers.Set("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
                HttpContext.Current.Response.Headers.Set("Access-Control-Allow-Headers", "Content-Type, Accept, X-Requested-With");
            }

            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                HttpContext.Current.Response.StatusCode = 200;
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
    }
}