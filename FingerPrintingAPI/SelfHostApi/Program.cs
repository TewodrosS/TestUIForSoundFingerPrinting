using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.SelfHost;
using System.Configuration;
using System.ServiceProcess;

namespace SelfHostApi
{
    public class Program
    {        
        static void Main(string[] args)
        {

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
        {
            new FingerPrintingService()
        };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
