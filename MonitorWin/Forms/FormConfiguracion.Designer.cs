namespace MonitorWin.Forms
{
    partial class FormConfiguracion
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConfiguracion));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.configuracionesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.iniciarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.detenerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.salirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.txtHost = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtBd = new System.Windows.Forms.TextBox();
            this.txtUrlBd = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPassBd = new System.Windows.Forms.TextBox();
            this.txtUserBd = new System.Windows.Forms.TextBox();
            this.rdbLocal = new System.Windows.Forms.RadioButton();
            this.rdbRemote = new System.Windows.Forms.RadioButton();
            this.txtArea = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnRestart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.contextMenuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configuracionesToolStripMenuItem,
            this.toolStripSeparator1,
            this.iniciarToolStripMenuItem,
            this.detenerToolStripMenuItem,
            this.salirToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(162, 98);
            // 
            // configuracionesToolStripMenuItem
            // 
            this.configuracionesToolStripMenuItem.Name = "configuracionesToolStripMenuItem";
            this.configuracionesToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.configuracionesToolStripMenuItem.Text = "Configuraciones";
            this.configuracionesToolStripMenuItem.Click += new System.EventHandler(this.configuracionesToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(158, 6);
            // 
            // iniciarToolStripMenuItem
            // 
            this.iniciarToolStripMenuItem.Name = "iniciarToolStripMenuItem";
            this.iniciarToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.iniciarToolStripMenuItem.Text = "Iniciar";
            this.iniciarToolStripMenuItem.Click += new System.EventHandler(this.iniciarToolStripMenuItem_Click);
            // 
            // detenerToolStripMenuItem
            // 
            this.detenerToolStripMenuItem.Name = "detenerToolStripMenuItem";
            this.detenerToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.detenerToolStripMenuItem.Text = "Detener";
            this.detenerToolStripMenuItem.Click += new System.EventHandler(this.detenerToolStripMenuItem_Click);
            // 
            // salirToolStripMenuItem
            // 
            this.salirToolStripMenuItem.Name = "salirToolStripMenuItem";
            this.salirToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.salirToolStripMenuItem.Text = "Salir";
            this.salirToolStripMenuItem.Click += new System.EventHandler(this.salirToolStripMenuItem_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Text = "Monitor CI";
            this.notifyIcon1.Visible = true;
            // 
            // txtHost
            // 
            this.txtHost.Location = new System.Drawing.Point(88, 161);
            this.txtHost.Margin = new System.Windows.Forms.Padding(2);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(231, 20);
            this.txtHost.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(46, 161);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Host";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(664, 241);
            this.btnClose.Margin = new System.Windows.Forms.Padding(2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(56, 24);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "Cerrar";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtBd);
            this.groupBox1.Controls.Add(this.txtUrlBd);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtPassBd);
            this.groupBox1.Controls.Add(this.txtUserBd);
            this.groupBox1.Controls.Add(this.rdbLocal);
            this.groupBox1.Controls.Add(this.rdbRemote);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtHost);
            this.groupBox1.Location = new System.Drawing.Point(9, 10);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(335, 194);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Configuracion";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(28, 108);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Catalogo";
            // 
            // txtBd
            // 
            this.txtBd.Location = new System.Drawing.Point(88, 107);
            this.txtBd.Margin = new System.Windows.Forms.Padding(2);
            this.txtBd.Name = "txtBd";
            this.txtBd.Size = new System.Drawing.Size(231, 20);
            this.txtBd.TabIndex = 12;
            // 
            // txtUrlBd
            // 
            this.txtUrlBd.Location = new System.Drawing.Point(88, 46);
            this.txtUrlBd.Margin = new System.Windows.Forms.Padding(2);
            this.txtUrlBd.Name = "txtUrlBd";
            this.txtUrlBd.Size = new System.Drawing.Size(231, 20);
            this.txtUrlBd.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(32, 50);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Servidor";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(173, 77);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Contraseña";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 77);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Usuario";
            // 
            // txtPassBd
            // 
            this.txtPassBd.Location = new System.Drawing.Point(236, 76);
            this.txtPassBd.Margin = new System.Windows.Forms.Padding(2);
            this.txtPassBd.Name = "txtPassBd";
            this.txtPassBd.Size = new System.Drawing.Size(84, 20);
            this.txtPassBd.TabIndex = 7;
            this.txtPassBd.UseSystemPasswordChar = true;
            // 
            // txtUserBd
            // 
            this.txtUserBd.Location = new System.Drawing.Point(88, 76);
            this.txtUserBd.Margin = new System.Windows.Forms.Padding(2);
            this.txtUserBd.Name = "txtUserBd";
            this.txtUserBd.Size = new System.Drawing.Size(84, 20);
            this.txtUserBd.TabIndex = 6;
            // 
            // rdbLocal
            // 
            this.rdbLocal.AutoSize = true;
            this.rdbLocal.Enabled = false;
            this.rdbLocal.Location = new System.Drawing.Point(11, 24);
            this.rdbLocal.Margin = new System.Windows.Forms.Padding(2);
            this.rdbLocal.Name = "rdbLocal";
            this.rdbLocal.Size = new System.Drawing.Size(51, 17);
            this.rdbLocal.TabIndex = 2;
            this.rdbLocal.Text = "Local";
            this.rdbLocal.UseVisualStyleBackColor = true;
            this.rdbLocal.CheckedChanged += new System.EventHandler(this.IsChecked);
            // 
            // rdbRemote
            // 
            this.rdbRemote.AutoSize = true;
            this.rdbRemote.Checked = true;
            this.rdbRemote.Location = new System.Drawing.Point(11, 135);
            this.rdbRemote.Margin = new System.Windows.Forms.Padding(2);
            this.rdbRemote.Name = "rdbRemote";
            this.rdbRemote.Size = new System.Drawing.Size(62, 17);
            this.rdbRemote.TabIndex = 3;
            this.rdbRemote.TabStop = true;
            this.rdbRemote.Text = "Remoto";
            this.rdbRemote.UseVisualStyleBackColor = true;
            // 
            // txtArea
            // 
            this.txtArea.BackColor = System.Drawing.SystemColors.MenuBar;
            this.txtArea.Location = new System.Drawing.Point(360, 10);
            this.txtArea.Margin = new System.Windows.Forms.Padding(2);
            this.txtArea.Multiline = true;
            this.txtArea.Name = "txtArea";
            this.txtArea.ReadOnly = true;
            this.txtArea.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtArea.Size = new System.Drawing.Size(362, 216);
            this.txtArea.TabIndex = 12;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnRestart);
            this.groupBox2.Controls.Add(this.btnStop);
            this.groupBox2.Controls.Add(this.btnStart);
            this.groupBox2.Location = new System.Drawing.Point(9, 218);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(335, 61);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Servicio CI";
            // 
            // btnRestart
            // 
            this.btnRestart.Location = new System.Drawing.Point(194, 24);
            this.btnRestart.Margin = new System.Windows.Forms.Padding(2);
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Size = new System.Drawing.Size(56, 24);
            this.btnRestart.TabIndex = 2;
            this.btnRestart.Text = "Reiniciar";
            this.btnRestart.UseVisualStyleBackColor = true;
            this.btnRestart.Click += new System.EventHandler(this.btnRestart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(134, 24);
            this.btnStop.Margin = new System.Windows.Forms.Padding(2);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(56, 24);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "Detener";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(73, 24);
            this.btnStart.Margin = new System.Windows.Forms.Padding(2);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(56, 24);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Iniciar";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(603, 241);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(56, 24);
            this.btnClear.TabIndex = 14;
            this.btnClear.Text = "Limpiar";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // FormConfiguracion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(734, 292);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.txtArea);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnClose);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "FormConfiguracion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configuracion :: Monitor Open Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormConfiguracion_FormClosing);
            this.SizeChanged += new System.EventHandler(this.ShowHide);
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem configuracionesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem iniciarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem detenerToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdbLocal;
        private System.Windows.Forms.RadioButton rdbRemote;
        private System.Windows.Forms.TextBox txtArea;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnRestart;
        private System.Windows.Forms.ToolStripMenuItem salirToolStripMenuItem;
        private System.Windows.Forms.TextBox txtUrlBd;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPassBd;
        private System.Windows.Forms.TextBox txtUserBd;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtBd;
        private System.Windows.Forms.Button btnClear;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}