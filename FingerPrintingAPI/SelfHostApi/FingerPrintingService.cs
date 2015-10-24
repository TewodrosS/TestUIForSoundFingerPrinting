using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace SelfHostApi
{
    partial class FingerPrintingService : ServiceBase
    {
        public FingerPrintingService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Uri myUri = new Uri(ConfigurationManager.AppSettings["HostingUrl"]);
            HttpSelfHostConfiguration config = new HttpSelfHostConfiguration(myUri);
            config.MaxReceivedMessageSize = Int32.Parse(ConfigurationManager.AppSettings["MaxReceivedMessageSize"]);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{file}",
                defaults: new { file = RouteParameter.Optional }
            );

            HttpSelfHostServer server = new HttpSelfHostServer(config);

            // Start listening 
            server.OpenAsync().Wait();
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
        }
    }
}
