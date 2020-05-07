using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Timers;
using System.ServiceModel;
using System.Net.Http;
using System.Net;
using ModeloCI;
using MonitorCore.Models;
using System.Data.Entity;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Threading;

namespace MonitorCore
{
    public class MMonitor
    {
        private Dictionary<string, Task> LTasks = new Dictionary<string, Task>();
        private ChannelFactory<Interfaces.IModelos> channelFactory;
        private Entities_ModeloCI db = new Entities_ModeloCI();
        private EndpointAddress EndPointHost;
        private Interfaces.IModelos Server;
        private bool Local = false;
        public bool Error;

        public MMonitor() {
        }

        public MMonitor(bool Local, string hosts)
        {
            this.Local = Local;
            if(this.Local == false)
            {

                EndPointHost = new EndpointAddress(hosts);
                channelFactory = new ChannelFactory<Interfaces.IModelos>(new BasicHttpBinding() { SendTimeout = TimeSpan.Parse("0:59:59") }, EndPointHost);
                Server = channelFactory.CreateChannel();
            }
            else
            {
                WriteEventLogEntry(EventLogEntryType.Warning, 1, "Cambiando a local");
            }
            
        }
        public async Task Process(TimeSpan interval, CancellationToken cancellationToken)
        {
           

            while (true)
            {
                bool Disposed = false;
                List<VW_MOD_POZO> ListModelos = new List<VW_MOD_POZO>();
               
                try
                {
                    
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    if (Local)
                    {
                        Disposed = ModeloProsper.Modelo.Dispose();
                        ListModelos = ModModel.GetList();
                    }
                    else
                    {
                        List<string> JsonModelos = Server.GetList(1);
                        Disposed = Server.Dispose();
                        if(JsonModelos.Count > 0)
                        {
                            foreach (var vwModPozo in JsonModelos)
                            {
                                ListModelos.Add(JsonConvert.DeserializeObject<VW_MOD_POZO>(vwModPozo));

                            }

                        }
                        

                    }
                    var tmp_tasks = LTasks.Keys.ToArray();

                    foreach (var task in tmp_tasks)
                    {
                        if (LTasks[task].IsCompleted || LTasks[task].IsFaulted)
                        {
                            LTasks.Remove(task);

                        }

                    }

                    foreach (var mod in ListModelos)
                    {
                        if (LTasks.ContainsKey(mod.IDMODPOZO) == false)
                        {
                            if (Local)
                                LTasks.Add(mod.IDMODPOZO, new Task(() => ExecLocal(db, mod)));
                            else
                                LTasks.Add(mod.IDMODPOZO, new Task(() => ExecRemote(db, mod, Server)));


                        }
                        if (mod.ESTATUS == 1)
                        {
                            //cola++;
                        }
                        if (mod.ESTATUS == 2)
                        {
                            // proceso++;
                        }
                    }

                    if (Disposed == true && LTasks.Count > 0)
                    {
                        string[] KTasks = LTasks.Keys.ToArray();
                        string id = KTasks[0];

                        if (LTasks[id].Status != TaskStatus.Running && LTasks[id].Status != TaskStatus.WaitingToRun)
                        {
                            LTasks[id].Start();
                        }


                    }
                   await Task.Delay(interval, cancellationToken);
                }
                catch (Exception ex)
                {
                    WriteEventLogEntry(System.Diagnostics.EventLogEntryType.Error, 1, ex.Message);
                }
            }
        }
        public void Process(object sender, ElapsedEventArgs e)
        {
            bool Disposed = false;
            var r = Server.GetList(1);
            List<VW_MOD_POZO> ListModelos = new List<VW_MOD_POZO>();
            if (Local)
            {
                Disposed = ModeloProsper.Modelo.Dispose();
                ListModelos = ModModel.GetList();
            }
            else
            {

                Disposed = Server.Dispose();
                foreach(var vwModPozo in Server.GetList(1))
                {
                    ListModelos.Add(JsonConvert.DeserializeObject<VW_MOD_POZO>(vwModPozo));

                }
                
            }

            
            try
            {
                var tmp_tasks = LTasks.Keys.ToArray();

                foreach (var task in tmp_tasks)
                {
                    if (LTasks[task].IsCompleted || LTasks[task].IsFaulted)
                    {
                        LTasks.Remove(task);

                    }

                }

                foreach (var mod in ListModelos)
                {
                    if (LTasks.ContainsKey(mod.IDMODPOZO) == false)
                    {
                        if (Local)
                            LTasks.Add(mod.IDMODPOZO, new Task(() => ExecLocal(db, mod)));
                        else
                            LTasks.Add(mod.IDMODPOZO, new Task(() => ExecRemote(db, mod, Server)));


                    }
                    if (mod.ESTATUS == 1)
                    {
                        //cola++;
                    }
                    if (mod.ESTATUS == 2)
                    {
                        // proceso++;
                    }
                }

                if (Disposed == true && LTasks.Count > 0)
                {
                    string[] KTasks = LTasks.Keys.ToArray();
                    string id = KTasks[0];

                    if (LTasks[id].Status != TaskStatus.Running && LTasks[id].Status != TaskStatus.WaitingToRun)
                    {
                        LTasks[id].Start();
                    }


                }
            }
            catch(Exception ex)
            {
                WriteEventLogEntry(System.Diagnostics.EventLogEntryType.Error,0, ex.Message);
            }
            
        }
       
        private static bool ExecRemote(Entities_ModeloCI db, VW_MOD_POZO mod_pozo, Interfaces.IModelos server)
        {
            try
            {
                bool result = false;
                switch (mod_pozo.FUNCION)
                {
                    case 1:
                        result = server.Sensibilidad_BN(mod_pozo.IDMODPOZO);



                        break;
                    case 2:
                        result = server.Condicion(mod_pozo.IDMODPOZO, null);
                        break;
                    default:
                        result = server.Execute(mod_pozo.IDMODPOZO, null);

                        if (result)
                        {
                            server.Estabilidad(mod_pozo.IDMODPOZO);
                        }
                        break;
                }
                return true;
            }
            catch (Exception ex)
            {
                WriteEventLogEntry(System.Diagnostics.EventLogEntryType.Error, 2, ex.Message);

            }
            return false;
        }
        private static bool ExecLocal(Entities_ModeloCI db,VW_MOD_POZO mod_pozo)
        {
            bool response = false;
            ModeloProsper.Logger Logger = new ModeloProsper.Logger(mod_pozo.IDMODPOZO);
            ModeloProsper.Modelo modelo = new ModeloProsper.Modelo(mod_pozo.IDMODPOZO);





            try
            {




                Logger.SetEstatus(2);

                switch (mod_pozo.FUNCION)
                {
                    case 1:

                        response = modelo.Sensibilidad_BN();


                        var conds = db.PA_operacionPozosFecha(mod_pozo.IDPOZO).ToList();
                        PA_operacionPozosFecha_Result cond = conds[0];

                        var cabezera = db.CabeceraPozoGBN.Where(w => w.bajaLogica == null && w.idPozo == mod_pozo.IDPOZO).SingleOrDefault();

                        if (response && cabezera != null && cond.FEC_CONDICION == mod_pozo.FECHAMODELO)
                        {

                            var inyeccion = db.DatosInyeccion.Where(w => w.idCabeceraPozoGBN == cabezera.idCabeceraPozoGBN).ToList();


                            inyeccion.ForEach(e => db.DatosInyeccion.Remove(e));
                            db.SaveChanges();


                            var QGI = db.COMPORTAMIENTO_GAS.Where(w => w.IDMODPOZO == mod_pozo.IDMODPOZO).SingleOrDefault();

                            if (QGI != null)
                            {

                                var QGIDetalles = db.COMPORTAMIENTO_GAS_DETALLES.Where(w => w.IDCOMPORTAMIENTOGAS == QGI.IDCOMPORTAMIENTOGAS).OrderBy(o => o.XAUX).ToList();


                                if (QGIDetalles.Count > 0)
                                {
                                    foreach (var dt in QGIDetalles)
                                    {
                                        db.DatosInyeccion.Add(new DatosInyeccion() { idCabeceraPozoGBN = cabezera.idCabeceraPozoGBN, qLiq = dt.YAUX.GetValueOrDefault(), qGasBN = dt.XAUX.GetValueOrDefault() }); //Multiplicacion x 1000 fue removida de xaux

                                    }
                                    db.SaveChanges();


                                }

                            }

                            cabezera.porc_agua = cond.gastoagua;
                            cabezera.presionCabeza = cond.PRESION_TP.GetValueOrDefault();
                            cabezera.qGasBN = cond.VOLUMEN_BN.GetValueOrDefault();
                            cabezera.fecha = cond.FEC_CONDICION.GetValueOrDefault();


                            db.Entry(cabezera).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        break;
                    case 2:
                        response = modelo.Update();
                        break;
                    default:

                        response = modelo.Create();

                        if (response && mod_pozo.ESTABILIDAD > 0)
                        {
                            var Configuracion = (from config in db.CONFIGURACION where config.IDMODPOZO == mod_pozo.IDMODPOZO && config.Fecha == (db.CONFIGURACION.Where(w => w.IDMODPOZO == mod_pozo.IDMODPOZO && config.ESTATUS == 1).Max(m => m.Fecha)) select config).SingleOrDefault();
                            ModeloProsper.Estabilidad estabilidad = new ModeloProsper.Estabilidad(Configuracion);
                            estabilidad.Execute();
                            estabilidad.Save();

                        }
                        break;

                }


                Logger.SetEstatus(3, "Ejecución correcta");



                return response;
            }
            catch (Exception ex)
            {
                if (Logger.Configuracion.ESTATUS == 2)
                    Logger.SetEstatus(-1, ex.Message);

                if ((Logger.Intentos + 1) < Logger.Configuracion.MAXREINTENTOS)
                    modelo.Reset(mod_pozo.IDMODPOZO, 0);

                WriteEventLogEntry(System.Diagnostics.EventLogEntryType.Error, 1, ex.Message);
                return false;

            }
        }
        public List<string> Monitor(ref string ReturnServ)
        {
           
            try
            {
                Error = false;
                if (Local) {
                    ModeloProsper.Modelo modelo = new ModeloProsper.Modelo();

                    return modelo.Monitor(ref ReturnServ);
                }
                else
                {
                    var httpClient = new HttpClient();

                    var response = httpClient.GetAsync(EndPointHost.Uri.ToString()).Result;
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        throw new Exception($"Unexpected status code: {response.StatusCode}");
                    }
                }
                return Server.Monitor(ref ReturnServ);
            }
            catch(Exception ex)
            {
                Error = true;

                if(ex.InnerException != null) return new List<string>() { ex.InnerException.Message};
                else return new List<string>() { ex.Message };



            }
            
        }
        public static  bool Test(string host)
        {
            try
            {
                // ChannelFactory<Interfaces.IModelos> channelFactoryTest = channelFactory.CreateChannel();
        //        EndpointAddress EndPointHost;
        //private Interfaces.IModelos Server;
        //string ReturnServ = null;
        //        var result = Server.Monitor(ref ReturnServ);

                return true;

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void WriteEventLogEntry(EventLogEntryType entryType, int eventID, string message)
        {
            // Create an instance of EventLog
            System.Diagnostics.EventLog eventLog = new System.Diagnostics.EventLog();

            // Check if the event source exists. If not create it.
            if (System.Diagnostics.EventLog.SourceExists("CIMonitor") == false)
            {
                System.Diagnostics.EventLog.CreateEventSource("CIMonitor", "CIMonitor");
            }

            // Set the source name for writing log entries.
            eventLog.Source = "CIMonitor";




            // Write an entry to the event log.
            eventLog.WriteEntry(message,
                                entryType,
                                eventID);

            // Close the Event Log
            eventLog.Close();

            //switch (entryType)
            //{
            //    case 0:
            //        break;
            //    case 1:
            //        break;
            //    case 2:
            //        break;

            //}

            //using (EventLog eventLog = new EventLog("Application"))
            //{
            //    eventLog.Source = "CIMonitor";
            //    eventLog.WriteEntry(message, entryType, eventID, 1);
            //    eventLog.Close();
            //}
        }
    }
}
