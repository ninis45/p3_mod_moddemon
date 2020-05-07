using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace MonitorCore.Interfaces
{
    [ServiceContract(Name = "IModelo")]
    public interface IModelos
    {


        [OperationContract]
        List<string> Monitor(ref string OpenServer);

        [OperationContract]
        List<string> GetList(int Estatus,int? Page=1);

        [OperationContract]
        bool Dispose();

        [OperationContract]
        void Program();

        [OperationContract]
        void Reset(string IdModPozo, int MaxIntentos);

        [OperationContract]
        void Delete(string IdModPozo, string IdUsuario);

        [OperationContract]
        bool Execute(string IdModPozo, string User);

        [OperationContract]
        bool Condicion(string IdModPozo, string User);

        [OperationContract]
        bool Estabilidad(string IdModPozo);

        [OperationContract]
        bool Sensibilidad_BN(string IdModPozo);

        [OperationContract]
        void ShutDown();
    }
}
