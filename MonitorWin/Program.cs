using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MonitorWin.Forms;
namespace MonitorWin
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            bool createdNew;
            var  m_Mutex = new Mutex(true, "MUTEXMONITORCI", out createdNew);

            if (createdNew == true)
                Application.Run(new FormConfiguracion());
            else {

                MessageBox.Show("Ya existe una aplicacion en ejecución", "Monitor CI", MessageBoxButtons.OK, MessageBoxIcon.Stop); ;
            }
            
        }
    }
}
