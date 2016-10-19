using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.RestartMe
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if DEBUG
            System.Windows.Forms.Application.Run(new frmDebug());
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new svcMain()
            };
            ServiceBase.Run(ServicesToRun);
#endif
        }
    }
}
