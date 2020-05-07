using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorCore.Libraries
{
    public class LibConfiguration
    {
        public bool Local;
        public string Host;
        public string ConnectionString;
        public LibConfiguration()
        {
            Local = Convert.ToBoolean(ConfigurationManager.AppSettings["local"]);
            Host = ConfigurationManager.AppSettings["host"];
            
        }
        public void Refresh(bool Local, string Host,string ConnString)
        {
            this.Local = Local;
            this.Host = Host;
            this.ConnectionString = ConnString;
        }
        public bool Save()
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if(ConnectionString != null && ConnectionString != "")
                {
                    configFile.ConnectionStrings.ConnectionStrings["Entities_ModeloCI"].ConnectionString = ConnectionString;
                }
                

                settings["local"].Value = Local.ToString();
                settings["host"].Value = Host;

                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("connectionStrings");
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
                

                return true;
            }
            catch (ConfigurationErrorsException ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
