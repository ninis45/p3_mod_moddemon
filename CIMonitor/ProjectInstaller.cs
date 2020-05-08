using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace CIMonitor
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
            //ServiceProcessInstaller processInstaller = new ServiceProcessInstaller();
            //processInstaller.Account = System.ServiceProcess.ServiceAccount.User;
            //processInstaller.Username = null;
            //processInstaller.Password = null;

            //Installers.Add(processInstaller);
        }

        private void serviceInstaller1_AfterInstall(object sender, InstallEventArgs e)
        {
            
             new ServiceController(serviceInstaller1.ServiceName).Start();
        }
    }
}
