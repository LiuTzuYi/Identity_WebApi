using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Identity_WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // 啟用追蹤功能
            config.EnableSystemDiagnosticsTracing();
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
