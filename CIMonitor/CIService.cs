using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using MonitorCore;

namespace CIMonitor
{
    [RunInstaller(true)]
    public partial class CIMonitor : ServiceBase
    {

        System.Timers.Timer MTimer;
        public CIMonitor()
        {
            InitializeComponent();
            
          

          
            
           

        }
        
        protected override void OnStart(string[] args)
        {
            // TODO: agregar código aquí para iniciar el servicio.
             MonitorCore.Libraries.LibConfiguration LibConfiguration = new MonitorCore.Libraries.LibConfiguration();
             CancellationTokenSource cancellationToken =new CancellationTokenSource();
             MTimer = new System.Timers.Timer(5000) { AutoReset=true,Enabled=true };

             
            
            try
            {
                if (args != null && args.Count() > 2)
                {
                    LibConfiguration.Refresh(Convert.ToBoolean(args[0]), args[1], args[2]);
                    LibConfiguration.Save();

                }

               
                MMonitor LibMonitor = new MMonitor(LibConfiguration.Local,LibConfiguration.Host);
                LibMonitor.ModeMessage = MMonitor.Modo.service;
                MTimer.Elapsed += LibMonitor.Process;
                

                MTimer.Start();



            }
            catch(Exception ex)
            {
                
                MMonitor.WriteEventLogEntry(System.Diagnostics.EventLogEntryType.Error, 0, ex.Message,MMonitor.Modo.service);
            }
            
        }

        protected override void OnStop()
        {
            // TODO: agregar código aquí para realizar cualquier anulación necesaria para detener el servicio.
            
           
            MTimer.Stop();
        }
        public void OnDebug()
        {
            OnStart(null);
            OnStop();
        }
        public void ShowConfiguracion()
        {
           
        }
        
    }
}
