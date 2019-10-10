using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModeloCI;

namespace Demon.Models
{
    
    
    class ResultadoModel
    {
        //private Entities_ModeloCI db = new Entities_ModeloCI();
        public ResultadoModel()
        {

        }
        public static bool  Save(RESULTADOS item)
        {
            try
            {
                using (Entities_ModeloCI db = new Entities_ModeloCI())
                {

                    db.RESULTADOS.Add(item);
                    //var ids_estabilidad = db.CAT_ESTABILIDAD_OPC.Where(w => w.Ind_Est != null).ToDictionary(d => d.Ind_Est, d => d.IDCATDESCRIPCION);

                    //db.RESULTADOS.Add(new RESULTADOS() { IDRESULTADOS = Guid.NewGuid().ToString().ToUpper(), IDCATDESCRIPCION = ids_estabilidad[item.Ind_Est], IDDATOSENTRADAEST = configuracion.IDDATOSENTRADAEST, Ind_Est = item.Ind_Est, Ptr = item.Ptr, DiaVal = item.DiaVal, Dp = item.Dp, Dpvalve = item.Dpvalve, Dvalve = item.Dvalve, GOR = item.GOR, QI = item.Ql, Qg = item.Qg, Pwf_IPR = item.Pwf_IPR, Twh = item.Twh, TotalQGas = item.TotalQgas, Pwf_quicklook = item.Pwf_quicklook, Pws = item.Pws, Pti = item.Pti, Ptri = item.Ptri, Tvalv = item.Tvalv, GOR_quicklool = item.GOR_quicklook, GORFREE = item.GORFREE, Ptrcalc = item.Ptrcalc, PI = item.PI, Qgcrit = item.Qgcrit, Qcporcent = item.Qcporcent, HTC = item.HTC, Pwh = item.Pwh, Qgi = item.Qgi, Wc = item.Wc });
                    db.SaveChanges();
                }
                   
                return true;
            }
            catch (Exception ex) {
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(item)+" ->  "+ex.Message);

                return false;
            }
          
        }
    }
}
