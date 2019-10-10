using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ModeloCI;

namespace UsrControl.Helpers
{
    class ToEstatusColor : StyleSelector
    {
        public Style error { get; set; }
        public Style success { get; set; }
        public Style process { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            var mod_pozo = (VW_MOD_POZO)item;

             
            Style estatus = null;




            switch(mod_pozo.ESTATUS)
            {
                case 0:
                    estatus= error;
                    break;               
                                   
                case 3:
                    estatus = success;
                    break;
                default:
                    
                    break;
            }
           
            return estatus;
        }
       
    }
}