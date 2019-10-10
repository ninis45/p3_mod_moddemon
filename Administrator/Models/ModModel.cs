using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using ModeloCI;

namespace Administrator.Models
{
    class ModModel
    {
       
        public ModModel()
        {

        }
        public static List<VW_MOD_POZO> GetList(int? estatus)
        {
            Entities_ModeloCI db = new Entities_ModeloCI();
            if (estatus == null)
            {
                return db.VW_MOD_POZO.Where(w=>w.IDCONFIGURACION== null).OrderBy(o=>o.FECHAMODELO).ToList();
            }
            if(estatus == 3)
            {
                return db.VW_MOD_POZO.Where(w => w.ESTATUS == estatus || w.ESTATUS==0).OrderBy(o=>o.FECHA_PROGRAMACION).ToList();
            }
            else
                return db.VW_MOD_POZO.Where(w => w.ESTATUS == estatus).OrderBy(o => o.FECHA_PROGRAMACION).ToList();
        }
        public static bool Insert(CONFIGURACION_ADMINISTRADOR config)
        {
            try
            {
                Entities_ModeloCI db = new Entities_ModeloCI();
                config.ESTATUS = 1;//0=Cancelado,1=Proceso,2=Ejecutandose,3=Ejecutado
                config.FECHA_REGLA = DateTime.Now;
                config.IDCONFIGURACION = Guid.NewGuid().ToString().ToUpper();
                db.CONFIGURACION_ADMINISTRADOR.Add(config);
                db.SaveChanges();

                return true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());   
                return false;
            }
        }
    }
}
