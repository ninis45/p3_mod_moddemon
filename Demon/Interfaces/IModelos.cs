using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Demon.Interfaces
{
    [ServiceContract(Name = "IModelo")]
    interface IModelo
    {


        [OperationContract]
        List<string> Monitor(ref string OpenServer);

        [OperationContract]
        void Program();

        [OperationContract]
        void Reset(string IdModPozo, int MaxIntentos);

        [OperationContract]
        void Delete(string IdModPozo, string IdUsuario);

        [OperationContract]
        bool Execute(string IdModPozo, string User);

        [OperationContract]
        bool Sensibilidad_BN(string IdModPozo);

        [OperationContract]
        void ShutDown();
    }
}
