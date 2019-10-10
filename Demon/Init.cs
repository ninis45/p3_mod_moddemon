using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Threading;
using System.Timers;
using ModeloCI;
using Demon.Libraries;
using System.ServiceModel;
using ModeloProsper;
using System.Threading.Tasks;
using System.Data;
using System.Diagnostics;

namespace Demon
{
    class Init
    {
        
        private static Entities_ModeloCI db = new Entities_ModeloCI();
      
        private static Dictionary<string,Thread> tasks = new Dictionary<string, Thread>();     

        private static Dictionary<string, int> all_tasks = new Dictionary<string, int>();
        //public static ModeloProsper.Modelo modelo = new ModeloProsper.Modelo();
        private static CancellationTokenSource cancellationToken = new CancellationTokenSource();

        private static bool Dispose = false;
        private static Dictionary<string,Task> LTasks = new Dictionary<string,Task>();
      

        public Init()
        {
            
        }
        static void Main(string[] args)
        {
          
            string location = System.Reflection.Assembly.GetExecutingAssembly().Location;

            Console.WriteLine("Version "+ System.Diagnostics.FileVersionInfo.GetVersionInfo(location).FileVersion + " | Build: "+ System.Diagnostics.FileVersionInfo.GetVersionInfo(location).FileBuildPart);
            Console.WriteLine("=================================================================================");
            Console.WriteLine("Environment: "+Libraries.Settings.Get("enviroment")+"\nSoftware version: "+Libraries.Settings.Get("prosper_version")+"\nMessages:"+ (Libraries.Settings.Get("show_messages")=="1"?"Y":"N")+"\nPROSPER: "+(Libraries.Settings.Get("open_prosper") == "1" ? "Y" : "N"));
            Console.WriteLine("=================================================================================");
            //// Create a timer and set a two second interval.
            //aTimer = new System.Timers.Timer();
            //aTimer.Interval = 20000;

            //// Hook up the Elapsed event for the timer. 
            //aTimer.Elapsed += OnTimedEvent;

            //// Have the timer fire repeated events (true is the default)
            //aTimer.AutoReset = true;

            //// Start the timer
            //aTimer.Enabled = true;

            Console.WriteLine("No cierres esta ventana... ");
            //Console.WriteLine("=================================================================================");

            //var proc = Process.GetProcesses();
            //switch(Console.ReadLine())
            //{
            //    case "exit":
            //        break;

            //}

            //ModeloProsper.Modelo modelo = new ModeloProsper.Modelo();

            //modelo.Tester();

            //var proc = Process.GetProcessesByName("pxserver");

            //proc[0].Kill();
            //cancellationToken = new CancellationTokenSource();
            var intervalTimeSpan = new TimeSpan(0, 0, 0, 0, 10000);



            Task Timer = TimerTask(intervalTimeSpan, cancellationToken.Token);
            

            Console.ReadLine();
            cancellationToken.Cancel();

            foreach(var task in LTasks)
            {
                
            }

           // LTasks = new List<Task>();
            //(Execute());

           // var t = Execute();


        }
        public static async Task TimerTask(TimeSpan interval, CancellationToken cancellationToken)
        {
           
            int licencias = Int32.Parse(Libraries.Settings.Get("no_licencias"));
            while (true)
            {


                try
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }


                    

                    int cola = 0, proceso = 0;
                    Dispose = Libraries.Settings.Get("open_server") == "1" ? true : false;
                    

                    //Verificar que pasa caundo se cierra el programa y reinicia con el estatus 2
                    List<VW_MOD_POZO> list = db.VW_MOD_POZO.Where(w => (w.ESTATUS == 1 || w.ESTATUS == 2) && w.FECHA_PROGRAMACION < DateTime.Now).OrderBy(o => o.FECHA_PROGRAMACION).ToList();

                    var tmp_tasks = LTasks.Keys.ToArray();

                    foreach (var task in tmp_tasks)
                    {
                        if (LTasks[task].IsCompleted || LTasks[task].IsFaulted)
                        {
                            LTasks.Remove(task);

                        }

                    }

                    foreach (var mod in list)
                    {                       
                        if(LTasks.ContainsKey(mod.IDMODPOZO)==false)
                        {
                            LTasks.Add(mod.IDMODPOZO,new Task(()=> Execute(mod)));
                        }
                        if (mod.ESTATUS == 1)
                        {
                            cola++;
                        }
                        if (mod.ESTATUS == 2)
                        {
                            proceso++;
                        }
                    }

                    Console.WriteLine("En cola({0}), proceso({1}) {2} | Open Server: {3}", cola.ToString(), proceso.ToString(), DateTime.Now, (Dispose ? "Disponible" : "Ocupado"));
                    Console.WriteLine("*********************************************************************************");

                    
                    if (Modelo.Dispose() == true && LTasks.Count > 0)
                    {
                        string[] KTasks = LTasks.Keys.ToArray();
                        string id = KTasks[0];
                        
                        if(LTasks[id].Status != TaskStatus.Running)
                        {
                            LTasks[id].Start();
                        }
                         

                    }

                    await Task.Delay(interval, cancellationToken);
                }
                catch(Exception ex)
                {
                    WriteLineText(ex.Message, "danger");
                }
            }
        }
        //Proximamente depreciado
        private static void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            
            int cola = 0,proceso=0;
            Dispose = Libraries.Settings.Get("open_server")=="1"?true:false;
            //string from = DateTime.Now.ToShortDateString();
            //string to = from + " 23:59:00";

            //DateTime tfrom = DateTime.Parse(from);
            //DateTime tto = DateTime.Parse(to);

            //Verificar que pasa caundo se cierra el programa y reinicia con el estatus 2

            var list    = db.VW_MOD_POZO.Where(w => (w.ESTATUS == 1 || w.ESTATUS == 2) && w.FECHA_PROGRAMACION < DateTime.Now).OrderBy(o => o.FECHA_PROGRAMACION).ToList(); // db.VW_MOD_POZO.Where(w => w.ESTATUS ==1 &&  w.FECHA_PROGRAMACION >= tfrom && w.FECHA_PROGRAMACION< tto).OrderBy(o=>o.FECHA_PROGRAMACION).ToList();

            var actives = db.VW_MOD_POZO.Where(w => w.ESTATUS == 2).OrderBy(o => o.FECHA_PROGRAMACION).ToList();//(from active in list where active.ESTATUS == 2  select active.ESTATUS).ToList();
            int licencias = Int32.Parse(Libraries.Settings.Get("no_licencias"));


            
            
            var tmp_tasks = tasks.ToArray();           

            foreach (var task in tmp_tasks)
            {
                if (tasks[task.Key].IsAlive == false)
                {
                    tasks.Remove(task.Key);

                }
                
            }
            foreach (var mod in list)
            {
               

                if (tasks.Count < licencias   && tasks.ContainsKey(mod.IDMODPOZO) == false)// (tasks.Count < Int32.Parse(Settings.Get("no_licencias")))
                {
                    tasks[mod.IDMODPOZO] = new Thread(new ThreadStart(() => Execute(mod)));
                    proceso++;
                }
               
                       


                if (mod.ESTATUS == 1)
                {                                                         
                    cola++;
                }

                 


            }
            
            Console.WriteLine("En cola({0}), proceso({1}) {2} | Open Server: {3}", cola.ToString(), tasks.Count.ToString(), e.SignalTime,(Dispose?"Disponible":"Ocupado"));
            Console.WriteLine("*********************************************************************************");  
            
            if(Dispose && tasks.Count>0)
            {

                

                foreach(var task in tasks)
                {
                    if (tasks[task.Key].IsAlive == false)
                    {
                        tasks[task.Key].Start();
                        Console.WriteLine("Inicia tarea: " + task.Key);

                    }
                    

                    
                }
            }





            
        }
     
        private static bool Execute(VW_MOD_POZO mod_pozo)
        {
            bool response = false;
            ModeloProsper.Logger Logger = new ModeloProsper.Logger(mod_pozo.IDMODPOZO);
            ModeloProsper.Modelo modelo = new ModeloProsper.Modelo(mod_pozo.IDMODPOZO);
            WriteLineText("Inicia modelo: " + mod_pozo.POZO);

            try
            {




                Logger.SetEstatus(2);

                if (mod_pozo.ARCHIVOS.GetValueOrDefault() > 0)
                {
                    response = modelo.Sensibilidad_BN();
                }
                else
                {
                    response = modelo.Execute(mod_pozo.IDMODPOZO);
                }





                if (response)
                {



                    //Logger.SetEstatus(3, "Ejecución correcta");
                   


                    if (mod_pozo.ESTABILIDAD > 0)
                    {
                        var Configuracion = (from config in db.CONFIGURACION where config.IDMODPOZO == mod_pozo.IDMODPOZO && config.Fecha == (db.CONFIGURACION.Where(w => w.IDMODPOZO == mod_pozo.IDMODPOZO && config.ESTATUS == 1).Max(m => m.Fecha)) select config).SingleOrDefault();

                        if (Configuracion != null)
                        {
                            Estabilidad(mod_pozo, Configuracion);
                           
                        }
                     
                    }
                    //else
                    //{
                    //    Logger.SetEstatus(3, "Ejecución correcta");
                    //}
                    Logger.SetEstatus(3, "Ejecución correcta");

                    WriteLineText("Termina modelo: " + mod_pozo.POZO, "success");





                }


                return response;
            }
            catch (Exception ex)
            {

                Logger.SetLog(ex.Message);

                if ((Logger.Intentos + 1) > Logger.Configuracion.MAXREINTENTOS) //revisar hay observaciones en este algoritmo
                {
                    Logger.SetEstatus(-1);
                }
                else
                {


                    modelo.Reset(mod_pozo.IDMODPOZO, 0);


                }
                WriteLineText(ex.Message, "danger");




                return false;

            }

        }
        private static bool Estabilidad(VW_MOD_POZO mod_pozo, CONFIGURACION configuracion)
        {

           
            Console.WriteLine("Inicia Estabilidad: " + mod_pozo.POZO);
           
            bool Executed;
            try
            {
                ModeloProsper.Estabilidad ObjEstabilidad = new ModeloProsper.Estabilidad(configuracion);

                
                var deletes = db.RESULTADOS.Where(w => w.IDDATOSENTRADAEST == configuracion.IDDATOSENTRADAEST).ToList();

                Models.ResultadoModel ResultadoModel = new Models.ResultadoModel();

                if (deletes.Count > 0)
                {
                    Task.Run(()=> {
                        deletes.ForEach(e => db.RESULTADOS.Remove(e));
                        db.SaveChanges();
                    });
                   

                }

                if (Executed =  ObjEstabilidad.Execute())
                {
                    var Resultados = ObjEstabilidad.Mapa;
                    //Ejecucion de manera asyncrona y desplegado de errores independientes
                    Task.Run(() => SaveEstabilidad(configuracion, Resultados));
                    
                }





                configuracion.QL = ObjEstabilidad.Ql;
                configuracion.ESTATUS = Executed?2:-1 ;
                configuracion.RECOMENDACIONES = ObjEstabilidad.Recomendacion;
                db.Entry(configuracion).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

               
                WriteLineText("Termina Estabilidad: " + mod_pozo.POZO,"success");
               
                return true;
            }
            catch (Exception ex)
            {
                configuracion.ESTATUS = -1;
                configuracion.RECOMENDACIONES = ex.Message;
                db.Entry(configuracion).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                throw new Exception(ex.Message);               

            }


        }

        private static void SaveEstabilidad(CONFIGURACION Configuracion, List<ModeloProsper.Clases.MapaEstabilidad.Parametros_Estabilidad> Resultados)
        {
            try
            {
                if (Resultados.Count > 0)
                {
                    
                    WriteLineText("Guardando Estabilidad:"+Resultados.Count.ToString()+" resultados (" + Configuracion.IDMODPOZO+").");
                    var ids_estabilidad = db.CAT_ESTABILIDAD_OPC.Where(w => w.Ind_Est != null).ToDictionary(d => d.Ind_Est, d => d.IDCATDESCRIPCION);
                    //Dr Ivan
                    foreach (var item in (from r in Resultados where r.Variable != "Qgi" select r).ToList())
                    {
                        RESULTADOS insert_ivan = new RESULTADOS()
                        {
                            IDRESULTADOS = Guid.NewGuid().ToString().ToUpper(),
                            IDCATDESCRIPCION = ids_estabilidad[item.Ind_Est],
                            IDDATOSENTRADAEST = Configuracion.IDDATOSENTRADAEST,
                            Ind_Est = item.Ind_Est,
                            Ptr = item.Ptr,
                            DiaVal = item.DiaVal,
                            Dp = item.Dp,
                            Dpvalve = item.Dpvalve,
                            Dvalve = item.Dvalve,
                            GOR = item.GOR,
                            QI = item.Ql,
                            Qg = item.Qg,
                            Pwf_IPR = item.Pwf_IPR,
                            Twh = item.Twh,
                            TotalQGas = item.TotalQgas,
                            Pwf_quicklook = item.Pwf_quicklook,
                            Pws = item.Pws,
                            Pti = item.Pti,
                            Ptri = item.Ptri,
                            Tvalv = item.Tvalv,
                            GOR_quicklool = item.GOR_quicklook,
                            GORFREE = item.GORFREE,
                            Ptrcalc = item.Ptrcalc,
                            PI = item.PI,
                            Qgcrit = item.Qgcrit,
                            Qcporcent = item.Qcporcent,
                            HTC = item.HTC,
                            Pwh = item.Pwh,
                            Qgi = item.Qgi,
                            Wc = item.Wc,
                            VARIABLE = item.Variable,
                            metodologia = 0
                        };
                        RESULTADOS insert_pob = new RESULTADOS()
                        {
                            IDRESULTADOS = Guid.NewGuid().ToString().ToUpper(),
                            IDCATDESCRIPCION = ids_estabilidad[item.Ind_Poblano],
                            IDDATOSENTRADAEST = Configuracion.IDDATOSENTRADAEST,
                            Ind_Est = item.Ind_Poblano,
                            Ptr = item.Ptr,
                            DiaVal = item.DiaVal,
                            Dp = item.Dp,
                            Dpvalve = item.Dpvalve,
                            Dvalve = item.Dvalve,
                            GOR = item.GOR,
                            QI = item.Ql,
                            Qg = item.Qg,
                            Pwf_IPR = item.Pwf_IPR,
                            Twh = item.Twh,
                            TotalQGas = item.TotalQgas,
                            Pwf_quicklook = item.Pwf_quicklook,
                            Pws = item.Pws,
                            Pti = item.Pti,
                            Ptri = item.Ptri,
                            Tvalv = item.Tvalv,
                            GOR_quicklool = item.GOR_quicklook,
                            GORFREE = item.GORFREE,
                            Ptrcalc = item.Ptrcalc,
                            PI = item.PI,
                            Qgcrit = item.Qgcrit,
                            Qcporcent = item.Qcporcent,
                            HTC = item.HTC,
                            Pwh = item.Pwh,
                            Qgi = item.Qgi,
                            Wc = item.Wc,
                            VARIABLE = item.Variable,
                            metodologia = 1
                        };

                        db.RESULTADOS.Add(insert_ivan);
                        db.RESULTADOS.Add(insert_pob);





                    }
                    db.SaveChanges();
                    WriteLineText("Estabilidad guardada: " + Configuracion.IDMODPOZO);
                }
            }
            catch(Exception ex)
            {
                WriteLineText(Configuracion.IDMODPOZO + ": " + ex.Message,"danger");
            }
        }
        public static void WriteLineText(string message, string t="")
        {
            switch(t)
            {
                case "warning":
                    break;
                case "danger":
                    Console.ForegroundColor = ConsoleColor.DarkRed;                   
                    
                    break;
                case "success":
                    Console.ForegroundColor = ConsoleColor.DarkGreen;                    
                    
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;

            }
           
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

    }
   
   
}
