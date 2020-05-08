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
        private Libraries.LibConfig libConfig;

       

       
        private string Prosper;
        private bool ForcedClose;
        private CancellationTokenSource cancellationToken;
        public FormConfiguracion()
        {
            InitializeComponent();


            libConfig = new Libraries.LibConfig();

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


       
        public static async Task RequestMonitor(TimeSpan interval, CancellationToken cancellationToken, MonitorCore.MMonitor mMonitor,TextBox txtArea)
        {
            string Prosper = null;
            int intentos = 0;

            while (true)
            {
                txtArea.ForeColor = Color.Black;

                try
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }
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

                    intentos++;

                    if(intentos > 5)
                    {
                        intentos = 0;
                        Prosper = null;
                    }
                    await Task.Delay(interval);
                }
                catch (Exception ex) {
                   
                    txtArea.ForeColor = Color.Red;
                    txtArea.Text = "Error: "+ex.Message + "\r\n";
                }
            }
        }


        private void RequestHost(object sender, EventArgs e)
        {
            MonitorCore.MMonitor mMonitor = new MMonitor(false,txtHost.Text);
            txtArea.ForeColor = Color.Black;
            txtArea.Text = "Conectando...";
            groupBox2.Enabled = false;
            try
            {
                Prosper = null;
                Task.Factory.StartNew(()=> {
                    try
                    {
                        if (mMonitor.Monitor(ref Prosper).Count > 0)
                        {
                            return true;
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Campos inteligentes", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    

                    return false;
                }).ContinueWith((response)=> {
                    try
                    {
                        if (response.Result)
                        {
                            txtArea.Text = "Estatus: Conectado";

                            libConfig.Host = txtHost.Text;
                            groupBox2.Enabled = true;
                        }
                        else
                        {
                            txtArea.ForeColor = Color.Red;
                            txtArea.Text = "Error al intentar conectarse";
                        }
                        
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Campos inteligentes", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtArea.ForeColor = Color.Red;
                        txtArea.Text = ex.Message;
                    }
                    
                }, CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
                
            }
            catch(Exception ex)
            {
                txtArea.Text = ex.Message;
                txtArea.ForeColor = Color.Red;
            }
            
            
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            txtArea.ForeColor = Color.Black;
            
            libConfig.Local = rdbLocal.Checked;            
            libConfig.Save();
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
            txtHost.Text = libConfig.Host;
            rdbLocal.Checked = libConfig.Local;//ConfigurationManager.AppSettings["host"];
            ListProcess = Process.GetProcessesByName(NameProcess);
            var pr = Process.GetProcesses();
            //Local = Convert.ToBoolean(ConfigurationManager.AppSettings["local"]);

            if (ListProcess.Count() > 0)
            {
                contextMenuStrip1.Items[2].Enabled = false;
                contextMenuStrip1.Items[3].Enabled = true;
                btnStart.Enabled = false;
               btnStop.Enabled = true;
               btnGo.Enabled = false;
                btnRestart.Enabled = true;
                cancellationToken = new CancellationTokenSource();
               txtArea.Text = "Estatus: Iniciado \r\n";
                txtHost.ReadOnly = true;
                var d = RequestMonitor(new TimeSpan(0, 0, 0, 0, 9000), cancellationToken.Token, new MMonitor(libConfig.Local, libConfig.Host), txtArea);
            }
            else
            {
                contextMenuStrip1.Items[2].Enabled = true;
                contextMenuStrip1.Items[3].Enabled = false;
                btnStart.Enabled = true;
                btnStop.Enabled = false;
                btnGo.Enabled = true;
                btnRestart.Enabled = false;
                txtHost.ReadOnly = false;
                txtArea.Text = "Estatus: Detenido \r\n";
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
               

               
                service.Start(new string[] { libConfig.Local.ToString(), libConfig.Host, libConfig.ConnectionString });
                service.WaitForStatus(ServiceControllerStatus.Running);
                

                
               

                MessageBox.Show("Servicio iniciado", "Campos Inteligentes", MessageBoxButtons.OK, MessageBoxIcon.Information);


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

                cancellationToken.Cancel();                
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

            if(ForcedClose == false) e.Cancel = true;
            this.WindowState = FormWindowState.Minimized;
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ForcedClose = true;
            this.Close();
        }
    }
}
