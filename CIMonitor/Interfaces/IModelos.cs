using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
namespace CIMonitor.Interfaces
{
    [ServiceContract(Name = "IModelo")]
    public interface IModelos
    {
        [OperationContract]
        Dictionary<string, List<string>> Condiciones();
    }
}
