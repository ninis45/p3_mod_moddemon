using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModeloCI;
namespace MonitorCore.Models
{
    class ModModel
    {
        List<VW_MOD_POZO> Proceso { get; set; }
        List<VW_MOD_POZO> Cola { get; set; }
        Entities_ModeloCI db = new Entities_ModeloCI();

        public static List<VW_MOD_POZO> GetList()
        {
            Entities_ModeloCI db = new Entities_ModeloCI();
            return db.VW_MOD_POZO.Where(w => (w.ESTATUS == 1 || w.ESTATUS == 2) && w.FECHA_PROGRAMACION < DateTime.Now).OrderBy(o => o.FECHA_PROGRAMACION).ToList();
        }
    }
}
