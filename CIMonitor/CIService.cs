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
using System.ServiceModel;
using System.Security.Principal;
//using MonitorCore;

namespace CIMonitor
{
    [RunInstaller(true)]
    public partial class CIMonitor : ServiceBase
    {

        public System.Timers.Timer MTimer;
        public System.Timers.Timer CTimer;
        
        public string NameProcess = "MonitorWin";
        private Process[] LProcess;
        private ChannelFactory<Interfaces.IModelos> channelFactory;
        private EndpointAddress EndPointHost;
        private Interfaces.IModelos Server;
        private Libraries.ConfigXml ConfigXml = new Libraries.ConfigXml();

        private int ElapsedCond = 0;
        //private MMonitor MMonitor;
        public CIMonitor()
        {
            InitializeComponent();

            


        }
        
        protected override void OnStart(string[] args)
        {
            // TODO: agregar código aquí para iniciar el servicio.

            

            try
            {
                MTimer = new System.Timers.Timer(60000) { AutoReset = true, Enabled = true };
                CTimer = new System.Timers.Timer(500000) { AutoReset = true, Enabled = true };
                //MonitorCore.Libraries.LibConfiguration LibConfiguration = new MonitorCore.Libraries.LibConfiguration();
                if (args != null)
                {
                    if (args.Count() == 1)
                    {
                        ConfigXml.Refresh(false, args[0]);


                    }
                    //    if (args.Count() == 4)
                    //    {
                    //        LibConfiguration.Refresh(true, args[0], args[1], args[2], args[3]);


                    //    }
                    ConfigXml.Save();
                }

                var httpBinding = new BasicHttpBinding()
                {
                    SendTimeout = TimeSpan.Parse("0:59:00"),
                    MaxBufferSize = Int32.MaxValue,
                    MaxReceivedMessageSize = Int32.MaxValue
                };


                EndPointHost = new EndpointAddress(ConfigXml.Host);
                channelFactory = new ChannelFactory<Interfaces.IModelos>(httpBinding, EndPointHost);
                Server = channelFactory.CreateChannel();
                //MMonitor LibMonitor = new MMonitor();// new MMonitor(LibConfiguration.Local, LibConfiguration.Host);
                //MMonitor = new MMonitor();
                //MMonitor.ModeMessage = MMonitor.Modo.service;

                //var processes = Process.GetProcesses();
                //LProcess = Process.GetProcessesByName(NameProcess);

                //if (LProcess.Count() == 0)
                //{
                //    Process.Start(Path + NameProcess);
                //}
               
                MTimer.Elapsed += Monitor;
               // CTimer.Elapsed += LibMonitor.RequestConds;

                MTimer.Start();
               // CTimer.Start();

               // MMonitor.WriteEventLogEntry(System.Diagnostics.EventLogEntryType.Warning, 0, $"Iniciando monitor con configuracion local: {LibConfiguration.Local.ToString()} y host: {LibConfiguration.Host}", MMonitor.Modo.service);
                
            }
            catch 
            {
                 throw; 
                //MMonitor.WriteEventLogEntry(System.Diagnostics.EventLogEntryType.Error, 0, ex.Message, MMonitor.Modo.service);
            }

        }

        protected override void OnStop()
        {
            // TODO: agregar código aquí para realizar cualquier anulación necesaria para detener el servicio.

            try
            {
                MTimer.Stop();
                //CTimer.Stop();
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
            OpenMonitorWin();
            OnStop();
        }
       

        private void Monitor(object sender, ElapsedEventArgs e)
        {
            try{
               
                LProcess = Process.GetProcessesByName(NameProcess);
                if (ElapsedCond > 10)
                {
                    Server.Condiciones();
                    
                    ElapsedCond++;
                }
                if (LProcess.Count() == 0)
                {
                    //ProcessStartInfo processStartInfo = new ProcessStartInfo("C:\\Program Files (x86)\\Campos Inteligentes\\Configurador\\MonitorWin.exe");
                    //////processStartInfo.CreateNoWindow = false;
                    //processStartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                    //processStartInfo.LoadUserProfile = true;
                    //processStartInfo.UseShellExecute = false;
                    ////processStartInfo.RedirectStandardError = true;
                    ////processStartInfo.RedirectStandardInput = true;
                    ////processStartInfo.RedirectStandardOutput = true;
                    ////processStartInfo.CreateNoWindow = false;
                    ////processStartInfo.ErrorDialog = false;
                    //processStartInfo.Verb = "runas /user:admin";

                    //Process.Start(processStartInfo);
                    OpenMonitorWin();
                    
                }
                
               
                ElapsedCond++;
            }
            catch(Exception ex){
                WriteEventLogEntry(EventLogEntryType.Error, 30, ex.Message);
            }
           

            //MMonitor.RequestConds();
        }

        public static void WriteEventLogEntry(EventLogEntryType entryType, int eventID, string message)
        {
            try
            {

               
                        // Create an instance of EventLog
                        System.Diagnostics.EventLog eventLog = new System.Diagnostics.EventLog();

                        // Check if the event source exists. If not create it.
                        if (System.Diagnostics.EventLog.SourceExists("CIMonitor") == false)
                        {
                            System.Diagnostics.EventLog.CreateEventSource("CIMonitor", "CIMonitor");
                        }

                        // Set the source name for writing log entries.
                        eventLog.Source = "CIMonitor";




                        // Write an entry to the event log.
                        eventLog.WriteEntry(message,
                                            entryType,
                                            eventID);

                        // Close the Event Log
                        eventLog.Close();
                        
            }
            catch (Exception)
            {
                throw;
            }



        }

        public void OpenMonitorWin(){
            //WindowsIdentity identity = WindowsIdentity.GetCurrent();
            //ProcessStartInfo processStartInfo = new ProcessStartInfo{
            //    Verb = "runas /user:admin",
            //    UseShellExecute =false,
            //    //WorkingDirectory= "C:\\Program Files(x86)\\Campos Inteligents\\Configurador\\",
            //     Arguments = "/C start \"\" \"C:\\Program Files (x86)\\Campos Inteligentes\\Configurador\\MonitorWin.exe\"",
            //     FileName ="cmd.exe" ,
                 
            //};


            //Process.Start(processStartInfo,identity.Name,);
            //int UserTokenHandle = Libraries.ApplicationLa.uncher.
            

            Libraries.ApplicationLauncher.STARTUPINFO LpStartInfo = new Libraries.ApplicationLauncher.STARTUPINFO();
            Libraries.ApplicationLauncher.PROCESS_INFORMATION LpProcessInf = new Libraries.ApplicationLauncher.PROCESS_INFORMATION();
            Libraries.ApplicationLauncher.SECURITY_ATTRIBUTES LpProcessAttr = new Libraries.ApplicationLauncher.SECURITY_ATTRIBUTES();
            Libraries.ApplicationLauncher.SECURITY_ATTRIBUTES LpThreadAttr = new Libraries.ApplicationLauncher.SECURITY_ATTRIBUTES();

            //var r = Libraries.ApplicationLauncher.CreateProcessInConsoleSession("C:\\Program Files (x86)\\Campos Inteligentes\\Configurador\\MonitorWin.exe", false);
            var r = Libraries.ApplicationLauncher.CreateProcessAsUser(IntPtr.Zero, "C:\\Program Files (x86)\\Campos Inteligentes\\Configurador\\MonitorWin.exe", "C:\\Program Files (x86)\\Campos Inteligentes\\Configurador\\MonitorWin.exe",ref LpProcessAttr,ref LpThreadAttr, false,0,IntPtr.Zero,null,ref LpStartInfo,out LpProcessInf);

            if(r == true)
            {
                WriteEventLogEntry(EventLogEntryType.Information, 31, "Aplicacion inicializada: " + NameProcess);
            }
        }
    }
}
