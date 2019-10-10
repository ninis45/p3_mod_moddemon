using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinControl
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            UsrControl.Views.Main window = new UsrControl.Views.Main("69A2512F-AA2D-4F0E-8045-0430B4093E05");

            elementHost1.Child = window;

        }
    }
}
