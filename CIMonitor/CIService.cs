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
        
        
        public CIMonitor()
        {
            InitializeComponent();
            
          

          
            
           

        }
        
        protected override void OnStart(string[] args)
        {
            // TODO: agregar código aquí para iniciar el servicio.
             MonitorCore.Libraries.LibConfiguration LibConfiguration = new MonitorCore.Libraries.LibConfiguration();
             CancellationTokenSource cancellationToken =new CancellationTokenSource();
            // MMonitor LibMonitor;
            //Task.Run(()=> { ShowConfiguracion(); }).Wait();
            try
            {
               // if (args != null && args.Count() > 2)
               // {
               //     LibConfiguration.Refresh(Convert.ToBoolean(args[0]), args[1],args[2]);
               //     LibConfiguration.Save();

               // }
               // cancellationToken = new CancellationTokenSource();
               // var intervalTimeSpan = new TimeSpan(0, 0, 0, 5, 0);
               // LibMonitor = new MMonitor(LibConfiguration.Local, LibConfiguration.Host);

               // //MTimer.Elapsed += LibMonitor.Process;

               //var d = LibMonitor.Process(intervalTimeSpan, cancellationToken.Token);

                

               

            }
            catch(Exception ex)
            {
               // MMonitor.WriteEventLogEntry(System.Diagnostics.EventLogEntryType.Error, 0, ex.Message);
            }
            
        }

        protected override void OnStop()
        {
            // TODO: agregar código aquí para realizar cualquier anulación necesaria para detener el servicio.
            //MMonitor.WriteEventLogEntry(System.Diagnostics.EventLogEntryType.Information,2, "Inicio correcto");
            //cancellationToken.Cancel();
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
