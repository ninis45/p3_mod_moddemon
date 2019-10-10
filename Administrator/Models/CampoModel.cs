using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModeloCI;

namespace Administrator.Models
{
    class CampoModel
    {
        public string id_campo { get; set; }
        public string nombre { get; set; }
        public List<VW_MOD_POZO> modelos { get; set; }
    }
}
