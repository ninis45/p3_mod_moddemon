using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
namespace CIMonitor.Libraries
{
    class ConfigXml
    {
        public string Lang;
        public bool Local;
        public string Host;
        private Configuration ConfigFile;
        public ConfigXml(){
            ConfigFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            Lang = ConfigurationManager.AppSettings["lang"];
            Local = Convert.ToBoolean(ConfigurationManager.AppSettings["local"]);
            Host = ConfigurationManager.AppSettings["host"];
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

        public bool Save()
        {
            try
            {

                var settings = ConfigFile.AppSettings.Settings;

                //if (Local && ValidConnection())
                //{
                //    ConfigFile.ConnectionStrings.ConnectionStrings["Entities_ModeloCI"].ConnectionString = ConnectionString;
                //}


                settings["local"].Value = Local.ToString();
                settings["host"].Value = Host;
                settings["lang"].Value = Lang;

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
    }
}
