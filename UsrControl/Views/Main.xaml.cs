using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ModeloCI;
using UsrControl.ViewModels;

namespace UsrControl.Views
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    
       
    public partial class Main : UserControl
    {
       
        public Main(string IdUsuario="")
        {
            AdminViewModel context = new AdminViewModel();
            InitializeComponent();
            context.IdUsuario = IdUsuario;
            this.DataContext = context;
        }

       
       

       

       

        private void CellValidating(object sender, Telerik.Windows.Controls.GridViewCellValidatingEventArgs e)
        {
            if (e.Cell.Column.UniqueName == "FECHA_PROGRAMACION")
            {
                if(e.NewValue == null)
                {
                    e.IsValid = false;
                    e.ErrorMessage = "La fecha de programación es requerida";
                }

            }
            if (e.Cell.Column.UniqueName == "MAXREINTENTOS")
            {
                if (e.NewValue.ToString() == "")
                {
                    e.IsValid = false;
                    e.ErrorMessage = "El número de intentos debe ser igual o mayor a 1";
                }
                

            }
        }

       
    }
}
