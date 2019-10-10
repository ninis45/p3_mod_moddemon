using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using UsrControl.Models;
using UsrControl.Libraries;
using ModeloCI;
using Telerik.Windows.Controls;
using System.ServiceModel;
using System.Threading;
/*
AUTH: BERNARDO CAUICH
================================================
LOG DE CAMBIOS
================================================
*/
namespace UsrControl.ViewModels
{
    class AdminViewModel:ViewModelBase
    {
        public Entities_ModeloCI db ;
        public ICommand CommandSave { get; set; }
        public ICommand CommandExecute { get; set; }
        public ICommand CommandDelete { get; set; }
        public ICommand CommandReset { get; set; }
        public ICommand CommandChange { get; set; }
        public List<String> campos = new List<string>();
        private EndpointAddress Address;

        public AdminViewModel()
        {
            db = new Entities_ModeloCI();
            gerencias = db.GERENCIA.Include("CAMPO").ToList();


           
            this.CommandSave = new DelegateCommand(OnSave);
            this.CommandExecute = new DelegateCommand(OnExecute);
            this.CommandDelete = new DelegateCommand(OnDelete);
            this.CommandReset = new DelegateCommand(OnReset);
            this.CommandChange = new DelegateCommand(OnChange);



            IdUsuario = "69A2512F-AA2D-4F0E-8045-0430B4093E05";

            LoadLists();
            Busy = false;

            _ejecucion_procesos = new List<EJECUCION_PROCESOS>();

            foreach (var gerencia in gerencias)
            {
                gerencia.CAMPO = db.CAMPO.Where(w => w.IDGERENCIA == gerencia.IDGERENCIA && campos.Contains(w.IDCAMPO)).ToList();
            }


            Address = new EndpointAddress(Settings.Get("url_resource"));
            //FlashData = new FlashData()
            //{
            //    Key = "success",
            //    Message = "Mensaje desde flash_data = {}"
            //};

            //FlashData.Message = "Mensaje desde Flash.Message";
            //Message = "Mensaje desde Message";
        }
        public void LoadLists()
        {
            Modelos = new ObservableCollection<VW_MOD_POZO>(ModModel.GetList(null));
            Procesos = new ObservableCollection<VW_MOD_POZO>(ModModel.GetList(1));
            Realizados = new ObservableCollection<VW_MOD_POZO>(ModModel.GetList(3));

           
            foreach (var m in Modelos)
            {
                if (campos.Contains(m.IDCAMPO) == false)
                {
                    campos.Add(m.IDCAMPO);
                }
            }
        }
        private string message;
        public string Message
        {
            get { return message; }
            set { message = value; OnPropertyChanged("Message"); }
        }
        private FlashData flash_data;
        public FlashData FlashData
        {
            get
            {
                if (flash_data == null) flash_data = new FlashData();
                return flash_data;
            }
            set
            {
                flash_data = value;
                OnPropertyChanged("FlashData");
            }
        }
        private VW_MOD_POZO modelo;
        public VW_MOD_POZO Modelo
        {
            get {
                if (modelo == null) modelo = new VW_MOD_POZO();

                return modelo;
            }
            set {
                modelo = value;
                OnPropertyChanged("Modelo");
                EjecucionProcesos = db.EJECUCION_PROCESOS.Where(w => w.IDCONFIGURACION == modelo.IDCONFIGURACION).ToList();
            }
        }
        
        private ObservableCollection<VW_MOD_POZO> modelos;
        public ObservableCollection<VW_MOD_POZO> Modelos
        {
            get { return modelos; }
            set { modelos = value; OnPropertyChanged("Modelos"); }
        }
        private ObservableCollection<VW_MOD_POZO> procesos;
        public ObservableCollection<VW_MOD_POZO> Procesos
        {
            get { return procesos; }
            set { procesos = value; OnPropertyChanged("Procesos"); }
        }

        private ObservableCollection<VW_MOD_POZO> realizados;
        public ObservableCollection<VW_MOD_POZO> Realizados
        {
            get { return realizados; }
            set { realizados = value; OnPropertyChanged("Realizados"); }
        }
        private CAMPO campo;
        public CAMPO Campo
        {
            get {
                return campo;
                
            }
            set {
                campo = value;
                OnPropertyChanged("Campo");

                var modelos = ModModel.GetList(null);

                Modelos = new ObservableCollection<VW_MOD_POZO>((from mod in modelos where mod.IDCAMPO == campo.IDCAMPO select mod).ToList());
            }
        }

        private List<EJECUCION_PROCESOS> _ejecucion_procesos;
        public List<EJECUCION_PROCESOS> EjecucionProcesos
        {
            get { return _ejecucion_procesos; }
            set { _ejecucion_procesos = value; OnPropertyChanged("EjecucionProcesos"); }
        }

        
        private List<GERENCIA> gerencias;
        public List<GERENCIA> Gerencias
        {
            get { return gerencias; }
            set { gerencias = value; }
        }
        private bool _busy;
        public bool Busy
        {
            get { return _busy;  }
            set { _busy = value; OnPropertyChanged("Busy"); }
        }

        private string _id_usuario;
        public string IdUsuario
        {
            get {
                return _id_usuario;
            }
            set
            {
                _id_usuario = value;
                OnPropertyChanged("IdUsuario");
            }
        }
        private void OnChange(Object e)
        {

            
            var tab = (Telerik.Windows.Controls.RadSelectionChangedEventArgs)e;
            var source = tab.Source;
            switch (((System.Windows.Controls.Primitives.Selector)source).SelectedIndex)
            {
                case 0:
                    Modelos = new ObservableCollection<VW_MOD_POZO>(ModModel.GetList(null));
                    FlashData.Key = FlashData.Types.error;
                    break;

                case 1:
                    Procesos = new ObservableCollection<VW_MOD_POZO>(ModModel.GetList(1));
                    FlashData.Key = FlashData.Types.success;
                    break;
                case 2:
                    Realizados = new ObservableCollection<VW_MOD_POZO>(ModModel.GetList(3));
                    break;
            }

            //Message = "Mensaje desde Message => LoadList "+ ((System.Windows.Controls.Primitives.Selector)source).SelectedIndex.ToString();

            //FlashData.Message = Message;
            
        }
        private void OnDelete(Object obj)
        {

            SVModel.ModeloClient server = new SVModel.ModeloClient();

          
          

            

            server.Endpoint.Address = Address;
            try
            {
                if (Modelo == null)
                {
                    MessageBox.Show("Primero selecciona un modelo");

                }
                else
                {

                    Task.Factory.StartNew(()=>
                    {
                        Busy = true;
                        server.Delete(Modelo.IDMODPOZO);
                       

                    }, CancellationToken.None,TaskCreationOptions.None, TaskScheduler.Default).ContinueWith((r)=> {
                        LoadLists();
                        Busy = false;
                    });

                   
                   
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }
        private void OnReset(Object obj)
        {
            SVModel.ModeloClient server = new SVModel.ModeloClient();
            server.Endpoint.Address = Address;

            try
            {
                if (Modelo == null)
                {
                    MessageBox.Show("Primero selecciona un modelo");

                }
                else
                {
                    Busy = true;
                   // server.Reset(Modelo.IDMODPOZO, Modelo.MAXREINTENTOS.GetValueOrDefault());
                    

                    Task.Factory.StartNew(() =>
                    {
                        Busy = true;
                        server.Reset(Modelo.IDMODPOZO, Modelo.MAXREINTENTOS.GetValueOrDefault());


                    }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).ContinueWith((r) => {
                        LoadLists();
                        Busy = false;
                    });
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
        private void OnExecute(Object obj)
        {
            try
            {
                SVModel.ModeloClient server = new SVModel.ModeloClient();
                
                //EndpointIdentity spn = System.ServiceModel.EndpointIdentity.CreateSpnIdentity("host/mikev-ws");
                // var address = new EndpointAddress(Settings.Get("url_resource"));

                server.Endpoint.Address =  Address;

                server.Execute(Modelo.IDMODPOZO, IdUsuario);

                LoadLists();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
        private void OnSave(Object obj)
        {
            var e = (GridViewRowEditEndedEventArgs)obj;

            try
            {
                

                if (e.EditAction != Telerik.Windows.Controls.GridView.GridViewEditAction.Cancel)
                {


                    var item = (VW_MOD_POZO)e.NewData;

                    CONFIGURACION_ADMINISTRADOR config = db.CONFIGURACION_ADMINISTRADOR.Where(w => w.IDMODPOZO == item.IDMODPOZO).SingleOrDefault();



                    if (config == null)
                    {
                        string now = item.FECHA_PROGRAMACION.Value.ToShortDateString() + " " + DateTime.Now.TimeOfDay.ToString();
                        config = new CONFIGURACION_ADMINISTRADOR()
                        {
                            IDMODPOZO = item.IDMODPOZO,
                            FECHA_PROGRAMACION = DateTime.Parse(now), //item.FECHA_PROGRAMACION.GetValueOrDefault(),
                            IDUSUARIO = IdUsuario,
                            MAXREINTENTOS = item.MAXREINTENTOS == null ? 1 : item.MAXREINTENTOS.GetValueOrDefault()
                        };
                        ModModel.Insert(config);

                    }
                    else
                    {
                        SVModel.ModeloClient server = new SVModel.ModeloClient();
                        server.Endpoint.Address = Address;
                        server.Reset(config.IDMODPOZO, item.MAXREINTENTOS.GetValueOrDefault());
                    }
                    MessageBox.Show("Configuración guardada, ahora lo podras ver en Proceso");

                    Procesos = new ObservableCollection<VW_MOD_POZO>(ModModel.GetList(1));
                    Modelos = new ObservableCollection<VW_MOD_POZO>(ModModel.GetList(null));

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

    }
}
