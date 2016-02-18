using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOLib;
using System.Threading;
using System.Reflection;
using System.Configuration.Install;
using System.ServiceProcess;
using System.ComponentModel;

namespace AOSrv {
    public class AOService : ServiceBase {

        public AOService() {
            this.ServiceName = "AOSrv";
            this.CanStop = true;
            this.CanPauseAndContinue = false;
            this.AutoLog = false;
        }

        protected override void OnStart(string[] args) {
            Logger.info("OnStart");
            StartSRV();
        }
        protected override void OnStop() {
            Logger.info("OnStop");
            StopSRV();
        }
        private static void DoWork() {
            while (true) {
                Logger.info("Doing work...");
                Thread.Sleep(1000);
            }
        }
        public void StartSRV() {
            Logger.info("Start");
        }
        public void StopSRV() {
            Logger.info("Stop");
        }
        static void Main() {
            System.ServiceProcess.ServiceBase.Run(new AOService());

        }
    }

    [RunInstallerAttribute(true)]
    public class AOInstaller : Installer {
        private ServiceProcessInstaller processInstaller;
        private ServiceInstaller serviceInstaller;

        public AOInstaller() {
            processInstaller = new ServiceProcessInstaller();
            serviceInstaller = new ServiceInstaller();

            processInstaller.Account = ServiceAccount.LocalSystem;
            serviceInstaller.StartType = ServiceStartMode.Manual;
            serviceInstaller.ServiceName = "AOSrv"; //must match CronService.ServiceName

            Installers.Add(serviceInstaller);
            Installers.Add(processInstaller);
        }
    }  

}
