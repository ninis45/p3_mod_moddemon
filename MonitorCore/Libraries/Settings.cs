using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModeloCI;
namespace MonitorCore.Libraries
{
    class Settings
    {
        public static void Set(string slug, string val)
        {

        }
        public static string Get(string slug)
        {



            Entities_ModeloCI db = new Entities_ModeloCI();

            var settings = db.SETTINGS.Where(w => w.SLUG == slug).SingleOrDefault();
            if (settings != null)
            {
                string value = settings.DEFAULT;

                if (settings.VALUE != null)
                {
                    value = settings.VALUE;
                }
                return value;
            }
            return "";


        }
    }
}
