using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Controls;
namespace UsrControl.Libraries
{
    class FlashData : ViewModelBase
    {

        private Types key;
        public Types Key
        {

            get { return key; }
            set
            {
                key = value;
                OnPropertyChanged("Key");

            }
        }
        public string message;
        public string Message {
            get {
                return message;
           } set {
                message = value;
                OnPropertyChanged("Message");
            }

        }


        public enum Types
        {
            error,
            success,
            info,
            warning
        }
        static List<string> items = new List<string>();


        
        public static void SetFlashData(string key, string message)
        {


            //HttpContext context = HttpContext.Current;

            //context.Session.Add(key, message);

        }

        public static string GetFlashData(string key, bool remove = true)
        {

            string message = "";
            //HttpContext context = HttpContext.Current;
            //if (context.Session[key] != null)
            //{


            //    message = (string)context.Session[key];

            //    if (remove == true)
            //        context.Session.Remove(key);
            //}




            return message;
        }
    }
}
