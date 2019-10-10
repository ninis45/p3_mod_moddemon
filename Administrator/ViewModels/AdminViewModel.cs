using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Administrator.Models;
using ModeloCI;
using Telerik.Windows.Controls;
/*
 AUTH: BERNARDO CAUICH
 ================================================
 LOG DE CAMBIOS
 ================================================
*/
namespace Administrator.ViewModels
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

        }
        public void LoadLists()
        {
            Modelos = new ObservableCollection<VW_MOD_POZO>(ModModel.GetList(null));
            Procesos = new ObservableCollection<VW_MOD_POZO>(ModModel.GetList(1));
            Realizados = new ObservableCollection<VW_MOD_POZO>(ModModel.GetList(3));


            foreach(var m in Modelos)
            {
                if(campos.Contains(m.IDCAMPO)==false)
                {
                    campos.Add(m.IDCAMPO);
                }
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
                   
                    break;

                case 1:
                    Procesos = new ObservableCollection<VW_MOD_POZO>(ModModel.GetList(1));
                   
                    break;
                case 2:
                    Realizados = new ObservableCollection<VW_MOD_POZO>(ModModel.GetList(3));
                    break;
            }
        }
        private void OnDelete(Object obj)
        {
            SVModel.ModeloClient server = new SVModel.ModeloClient();
            try
            {
                if (Modelo == null)
                {
                    MessageBox.Show("Primero selecciona un modelo");

                }
                else
                {
                    server.Delete(Modelo.IDMODPOZO);
                    LoadLists();
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

            if (Modelo == null)
            {
                MessageBox.Show("Primero selecciona un modelo");

            }
            else
            {
                Busy = true;
                server.Reset(Modelo.IDMODPOZO,Modelo.MAXREINTENTOS.GetValueOrDefault());
                LoadLists();
                Busy = false;
            }
        }
        private void OnExecute(Object obj)
        {
            try
            {
                SVModel.ModeloClient server = new SVModel.ModeloClient();

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

            var item = (VW_MOD_POZO)e.NewData;

            CONFIGURACION_ADMINISTRADOR config = db.CONFIGURACION_ADMINISTRADOR.Where(w => w.IDMODPOZO == item.IDMODPOZO).SingleOrDefault();

           
            
           if (config == null)
            {
                config = new CONFIGURACION_ADMINISTRADOR()
                {


                    IDMODPOZO = item.IDMODPOZO,
                    FECHA_PROGRAMACION = item.FECHA_PROGRAMACION.GetValueOrDefault(),
                    IDUSUARIO = IdUsuario,
                    MAXREINTENTOS = item.MAXREINTENTOS.GetValueOrDefault()

                };
                ModModel.Insert(config);
                       
           }
            else
            {
                SVModel.ModeloClient server = new SVModel.ModeloClient();

                server.Reset(config.IDMODPOZO,item.MAXREINTENTOS.GetValueOrDefault());
            }
            MessageBox.Show("Configuración guardada, ahora lo podras ver en Proceso");

            Procesos = new ObservableCollection<VW_MOD_POZO>(ModModel.GetList(1));
            Modelos = new ObservableCollection<VW_MOD_POZO>(ModModel.GetList(null));


        }

    }
}
