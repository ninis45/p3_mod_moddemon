using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorWin.Libraries
{
    class LibConfig
    {
        public bool Local { get; set; }
        public string Host { get; set; }
        private Configuration ConfigFile;
        string UrlBd, UserBd, PassBd,CatalogBd;
        public LibConfig()
        {
            Local = Convert.ToBoolean(ConfigurationManager.AppSettings["local"]);
            Host = ConfigurationManager.AppSettings["host"];
        }
        public void Refresh(bool Local, string Host)
        {
            this.Local = Local;
            this.Host = Host;
        }
        public bool Save()
        {
            try
            {
                ConfigFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                if (Local && IsValidConnection() == false)
                {
                    throw new Exception("Para la conexion local, las credencias a la base de datos es incorrecto.");
                }


                var settings = ConfigFile.AppSettings.Settings;

                settings["local"].Value = Local.ToString();
                settings["host"].Value = Host;

                ConfigFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(ConfigFile.AppSettings.SectionInformation.Name);
              

                return true;
            }
            catch (ConfigurationErrorsException ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool IsValidConnection()
        {
            try
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

                //DbConnectionStringBuilder csb = new DbConnectionStringBuilder();
                //csb.ConnectionString = ConnectionString;

                using (SqlConnection conn = new SqlConnection("Data Source=" + UrlBd + ";Initial Catalog="+CatalogBd+";User ID=" + UserBd + ";Password=" + PassBd))
                {
                    conn.Open(); // throws if invalid
                    conn.Close();

                }
                return true;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string ConnectionString
        {
            get
            {

                return Utf8Encode("metadata=res://*/ModeloCI.csdl|res://*/ModeloCI.ssdl|res://*/ModeloCI.msl;provider=System.Data.SqlClient;provider connection string=\"data source=" + UrlBd + ";initial catalog=CI;persist security info=True;user id=" + UserBd + ";password=" + PassBd + ";MultipleActiveResultSets=True;App=EntityFramework\"");
            }
        }
        public static string Utf8Encode(string str)
        {
            byte[] bytes = Encoding.Default.GetBytes(str);
            string utf8_String = Encoding.UTF8.GetString(bytes);

            return utf8_String;

        }
    }
}
