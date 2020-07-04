using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorCore.Libraries
{
    public class LibConfiguration
    {
        public bool Local;
        public string Host;
       
        public string UrlBd, UserBd, PassBd, CatalogBd;
        private Configuration ConfigFile;
        public LibConfiguration()
        {
            ConfigFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            Local = Convert.ToBoolean(ConfigurationManager.AppSettings["local"]);
            Host = ConfigurationManager.AppSettings["host"];

            PartsConnectionString();

        }
        /// <summary>
        /// Actualiza la cadena de conexion y demas componentes del App.config
        /// </summary>
        public void Refresh()
        {
            ConfigFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }
        public void Refresh(bool Local, string Host)
        {
            this.Local = Local;
            this.Host = Host;
           
        }
        public void Refresh(bool local, string userBd,string passBd, string urlBd, string catalogBd)
        {
            this.Local = local;
            this.UserBd = userBd;
           
            this.UrlBd = urlBd;
            this.CatalogBd = catalogBd;
            if(passBd!=null && passBd != "")
            {
                this.PassBd = passBd;
            }
            

        }
        public bool Save()
        {
            try
            {
                
                var settings = ConfigFile.AppSettings.Settings;

                if (Local  && ValidConnection())
                {
                    ConfigFile.ConnectionStrings.ConnectionStrings["Entities_ModeloCI"].ConnectionString = ConnectionString;
                }


                settings["local"].Value = Local.ToString();
                settings["host"].Value = Host;

                ConfigFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("connectionStrings");
                ConfigurationManager.RefreshSection(ConfigFile.AppSettings.SectionInformation.Name);
                

                return true;
            }
            catch (ConfigurationErrorsException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void PartsConnectionString()
        {
            
            string[] parts = ConfigFile.ConnectionStrings.ConnectionStrings["Entities_ModeloCI"].ConnectionString.Split(';');
            int position = 0;


            foreach (var part in parts)
            {
                if (part.ToLower().Contains("data source"))
                {
                    string[] properties = parts[position].Split('=');

                    UrlBd = properties[properties.Count() - 1];

                }
                if (part.ToLower().Contains("user id"))
                {
                    string[] properties = parts[position].Split('=');
                    UserBd = properties[1];

                }
                if (part.ToLower().Contains("password"))
                {
                    string[] properties = parts[position].Split('=');
                    PassBd = properties[1];

                }
                if (part.ToLower().Contains("initial catalog"))
                {
                    string[] properties = parts[position].Split('=');
                    CatalogBd = properties[1];

                }

                position++;
            }
        }
    
        public string ConnectionString
        {
            
            get
            {

                return Utf8Encode("metadata=res://*/ModeloCI.csdl|res://*/ModeloCI.ssdl|res://*/ModeloCI.msl;provider=System.Data.SqlClient;provider connection string=\"data source=" + UrlBd + ";initial catalog="+CatalogBd+";persist security info=True;user id=" + UserBd + ";password=" + PassBd + ";MultipleActiveResultSets=True;App=EntityFramework\"");
            }
        }
        public static string Utf8Encode(string str)
        {
            byte[] bytes = Encoding.Default.GetBytes(str);
            string utf8_String = Encoding.UTF8.GetString(bytes);

            return utf8_String;

        }
        public bool ValidConnection()
        {
            try {
                using (SqlConnection conn = new SqlConnection("Data Source=" + UrlBd + ";Initial Catalog=" + CatalogBd + ";User ID=" + UserBd + ";Password=" + PassBd))
                {
                    conn.Open(); // throws if invalid
                    conn.Close();

                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
