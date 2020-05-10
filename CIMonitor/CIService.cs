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

        public System.Timers.Timer MTimer;
        

        public CIMonitor()
        {
            InitializeComponent();



            



        }
        
        protected override void OnStart(string[] args)
        {
            // TODO: agregar código aquí para iniciar el servicio.
             //MonitorCore.Libraries.LibConfiguration LibConfiguration = new MonitorCore.Libraries.LibConfiguration();
             //CancellationTokenSource cancellationToken =new CancellationTokenSource();
            MTimer = new System.Timers.Timer(5000) { AutoReset=true,Enabled=true };
            MonitorCore.Libraries.LibConfiguration LibConfiguration = new MonitorCore.Libraries.LibConfiguration();


            try
            {
                if (args != null)
                {
                    if (args.Count() == 1)
                    {
                        LibConfiguration.Refresh(false, args[0]);
                        LibConfiguration.Save();

                    }
                    if (args.Count() == 4)
                    {
                        LibConfiguration.Refresh(true, args[0], args[1], args[2], args[3]);
                        LibConfiguration.Save();

                    }
                }
                
               
                MMonitor LibMonitor = new MMonitor(LibConfiguration.Local,LibConfiguration.Host);
                LibMonitor.ModeMessage = MMonitor.Modo.service;
                MTimer.Elapsed += LibMonitor.Process;             

                MTimer.Start();

                MMonitor.WriteEventLogEntry(System.Diagnostics.EventLogEntryType.Warning, 0,$"Iniciando monitor con configuracion local: {LibConfiguration.Local.ToString()} y host: {LibConfiguration.Host}", MMonitor.Modo.service);

            }
            catch(Exception ex)
            {
               // throw; 
                MMonitor.WriteEventLogEntry(System.Diagnostics.EventLogEntryType.Error, 0, ex.Message,MMonitor.Modo.service);
            }
            
        }

        protected override void OnStop()
        {
            // TODO: agregar código aquí para realizar cualquier anulación necesaria para detener el servicio.

            try
            {
                MTimer.Stop();
            }
            catch 
            {
                throw;
            }
            
        }
        public void OnDebug()
        {
           // OnStart(new string[] {"http://google.com" });

            OnStart(null);
            OnStop();
        }
        public void ShowConfiguracion()
        {
           
        }
        
        
    }
}
