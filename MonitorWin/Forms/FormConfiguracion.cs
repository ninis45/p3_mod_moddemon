using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using System.Diagnostics;
using System.ServiceProcess;
using System.Configuration;
using MonitorCore;
using System.Threading;
using System.ServiceModel;
using System.Timers;

namespace MonitorWin.Forms
{
    public partial class FormConfiguracion : Form
    {
        private string NameProcess = "CIMonitor";
        private Process[] ListProcess;        
        private MonitorCore.Libraries.LibConfiguration libConfig;

       

       
        private string Prosper;
        private bool ForcedClose;
        private CancellationTokenSource cancellationToken;

        private System.Timers.Timer timerMonitor;

        public FormConfiguracion()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;

            libConfig = new MonitorCore.Libraries.LibConfiguration();

            //TimerMonitor = new System.Timers.Timer(100) { AutoReset = true };
            //TimerMonitor.Elapsed += RequestMonitor;
            RefreshInfo();

           
            
        }

        private void ShowHide(object sender, EventArgs e)
        {
            // this.Hide();//notifyIcon1.Icon = SystemIcons.Application;

            if(this.WindowState == FormWindowState.Minimized)
            {
                notifyIcon1.Icon = this.Icon;
                notifyIcon1.BalloonTipText = "CI Monitor";
                notifyIcon1.ShowBalloonTip(100);
                this.Hide();
            }
            
            //this.WindowState = FormWindowState.Minimized;       
        }



        public static async Task RequestMonitor(TimeSpan interval, CancellationToken cancellationToken, MonitorCore.MMonitor mMonitor, TextBox txtArea)
        {
            string Prosper = null;
            int intentos = 0;
            int intentosConds = 0;
            bool last_error = false;
            txtArea.ForeColor = Color.Black;
            while (true)
            {
               

                try
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    intentos++;
                    await Task.Run(()=> {
                        try
                        {
                            mMonitor.Error = false;
                            return mMonitor.Monitor(ref Prosper);
                            
                        }
                        catch (Exception ex) {
                            mMonitor.Error = true;

                            
                            return new List<string>() { ex.Message };
                        }
                       
                    }).ContinueWith((response)=> {
                        var List = response.Result;

                        if (last_error != mMonitor.Error)
                        {
                            Prosper = null;
                            txtArea.Clear();
                            if (mMonitor.Error == false)
                            {
                                txtArea.ForeColor = Color.Black;
                            }
                            else
                            {
                                txtArea.ForeColor = Color.Red;
                            }
                        }
                        if (List.Count > 0)
                        {
                            foreach (var l in List)
                            {
                               


                                txtArea.AppendText(l + " \r\n");


                            }
                        }
                        last_error = mMonitor.Error;

                       
                        
                        

                    }, CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());

                    if (intentos > 5)
                    {
                        intentosConds++;
                        intentos = 0;
                        Prosper = null;
                    }

                    //if(intentosConds > 6)
                    //{
                    //    intentosConds = 0;
                    //    await Task.Run(()=> {

                    //        try
                    //        {
                    //            mMonitor.RequestConds();
                    //        }
                    //        catch(Exception ex)
                    //        {
                    //            txtArea.ForeColor = Color.Red;
                    //            txtArea.AppendText(ex.Message);

                    //            txtArea.ForeColor = Color.Black;
                    //        }
                    //    });
                    //}


                    await Task.Delay(interval, cancellationToken);
                }
                catch (Exception)
                {
                    throw;
                    //last_error = true;
                    //txtArea.ForeColor = Color.Red;
                    //txtArea.Text = "Error: " + ex.Message + "\r\n";
                }
            }
        }

        public static void RequestMonitor(object sender, ElapsedEventArgs e,string Prosper, MonitorCore.MMonitor mMonitor,TextBox txtArea)
        {
            try
            {
                
                var List = mMonitor.Monitor(ref Prosper);

                if (List.Count > 0)
                {
                    foreach (var l in List)
                    {
                        if (mMonitor.Error)
                        {
                            txtArea.ForeColor = Color.Red;
                            txtArea.Clear();
                        }

                        txtArea.AppendText(l + " \r\n");


                    }
                }

                //intentos++;

                //if (intentos > 5)
                //{
                //    intentos = 0;
                //    Prosper = null;
                //}
            }
            catch(Exception ex)
            {
                txtArea.ForeColor = Color.Red;
                txtArea.Text = ex.Message;
                txtArea.Clear();
            }
        }
        

        private void btnStart_Click(object sender, EventArgs e)
        {
            txtArea.ForeColor = Color.Black;

            if (rdbLocal.Checked)
            {
                libConfig.Refresh(rdbLocal.Checked, txtUserBd.Text, txtPassBd.Text, txtUrlBd.Text, txtBd.Text);
            }
            else
            {
                
                libConfig.Refresh(rdbLocal.Checked, txtHost.Text);
            }

           
            
            txtArea.Text = "Iniciando...";
            Task.Run(() => {

                StartService();
            }).ContinueWith((response)=> {
                RefreshInfo();
            }, CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
           
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            txtArea.ForeColor = Color.Black;        
         
            txtArea.Text = "Deteniendo...";
            Task.Run(() => {

                StopService();
            }).ContinueWith((response) => {
                RefreshInfo();
            }, CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
            
        }
        private void RefreshInfo()
        {
            libConfig.Refresh();
            txtBd.Text = libConfig.CatalogBd;
            txtUrlBd.Text = libConfig.UrlBd;
            txtUserBd.Text = libConfig.UserBd;

            txtHost.Text = libConfig.Host;
            rdbLocal.Checked = libConfig.Local;//ConfigurationManager.AppSettings["host"];
            ListProcess = Process.GetProcessesByName(NameProcess);
           

            if (ListProcess.Count() > 0)
            {
                groupBox1.Enabled = false;
                contextMenuStrip1.Items[2].Enabled = false;
                contextMenuStrip1.Items[3].Enabled = true;
                btnStart.Enabled = false;
               btnStop.Enabled = true;
              
                btnRestart.Enabled = true;
                cancellationToken = new CancellationTokenSource();
               txtArea.Text = "Estatus: Iniciado \r\n";


               
                var d = RequestMonitor(new TimeSpan(0, 0, 0, 10,0), cancellationToken.Token, new MMonitor(libConfig.Local, libConfig.Host), txtArea);
            }
            else
            {
                groupBox1.Enabled = true;
                contextMenuStrip1.Items[2].Enabled = true;
                contextMenuStrip1.Items[3].Enabled = false;
                btnStart.Enabled = true;
                btnStop.Enabled = false;
                //btnGo.Enabled = true;
                btnRestart.Enabled = false;
                //txtHost.ReadOnly = false;
                txtArea.Text = "Estatus: Detenido \r\n";
                //timerMonitor.Stop();
                if(cancellationToken != null) cancellationToken.Cancel();
            }
        }

        private void configuracionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void StartService()
        {
            ServiceController service = new ServiceController(NameProcess, Environment.MachineName);

          

            try
            {
                
                if (libConfig.Save()) {

                    MonitorCore.MMonitor mMonitor = new MMonitor(rdbLocal.Checked, txtHost.Text);
                    if (ValidHost(mMonitor))
                    {
                        

                        if (libConfig.Local == false) {
                            service.Start(new string[] { libConfig.Host });
                        }
                        else
                        {
                            service.Start(new string[] { libConfig.UserBd, libConfig.PassBd, libConfig.UrlBd, libConfig.CatalogBd });
                        }
                        service.WaitForStatus(ServiceControllerStatus.Running);





                        MessageBox.Show("Servicio iniciado", "Campos Inteligentes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

            }
            catch (Exception ex)
            {
               
                
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void StopService()
        {
            ServiceController service = new ServiceController(NameProcess, Environment.MachineName);
            try
            {
                
                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped);            

                             
                Task.Delay(2000).Wait();
               
            }
            catch(Exception ex)
            {
                
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
        private void iniciarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            Task.Run(() => {

                StartService();
            }).ContinueWith((response) => {
                RefreshInfo();
            }, CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void detenerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Task.Run(() => {

                StopService();
            }).ContinueWith((response) => {
                RefreshInfo();
            }, CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
             
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            txtArea.ForeColor = Color.Black;

            txtArea.Text = "Reiniciando...";
            Task.Run(() => {

                StopService();
                StartService();
            }).ContinueWith((response) => {
                RefreshInfo();
            }, CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void FormConfiguracion_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (ForcedClose == false) {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
            }
           
           
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ForcedClose = true;
            this.Close();
        }

        private void IsChecked(object sender, EventArgs e)
        {
            RadioButton rdbLocal = sender as RadioButton;

            if (rdbLocal.Checked)
            {
               
                txtHost.Enabled = false;


               

                txtPassBd.Enabled = true;
                txtUrlBd.Enabled = true;
                txtUserBd.Enabled = true;
                txtBd.Enabled = true;
            }
            else
            {
               
                txtHost.Enabled = true;
                txtPassBd.Enabled = false;
                txtUrlBd.Enabled = false;
                txtUserBd.Enabled = false;
                txtBd.Enabled = false;

            }
        }

        private bool ValidHost(MMonitor mMonitor)
        {
            try
            {
               
                string Prosper = null;
                var conn = mMonitor.Monitor(ref Prosper);
                if (conn.Count() > 0)
                {
                    return true;
                }
                


                return false;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }   
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtArea.Clear();
        }
    }
}
