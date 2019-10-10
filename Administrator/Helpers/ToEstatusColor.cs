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

namespace Administrator.Helpers
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

            //if (item is Club)
            //{
            //    Club club = item as Club;
            //    if (club.StadiumCapacity > 50000)
            //    {
            //        return BigStadiumStyle;
            //    }
            //    else
            //    {
            //        return SmallStadiumStyle;
            //    }
            //}
            return estatus;
        }
        //public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        //{
        //    return "Visibility";
        //}

        //public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
/*
 public class StadiumCapacityStyle : StyleSelector 
{ 
    public override Style SelectStyle(object item, DependencyObject container) 
    { 
        if (item is Club) 
        { 
            Club club = item as Club; 
            if (club.StadiumCapacity > 50000) 
            { 
                return BigStadiumStyle; 
            } 
            else 
            { 
                return SmallStadiumStyle; 
            } 
        } 
        return null; 
    } 
    public Style BigStadiumStyle { get; set; } 
    public Style SmallStadiumStyle { get; set; } 
} 
     */
