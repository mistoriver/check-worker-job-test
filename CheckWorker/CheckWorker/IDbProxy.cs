using CheckWorker.Helpers;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace CheckWorker
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IDbProxy
    {
        /*
        [OperationContract]
        string GetData(int value);

        [OperationContract]
        Cheque GetDataUsingDataContract(Cheque composite);*/


        // TODO: Add your service operations here


        [OperationContract]
        void TakeChequeAsync(Cheque cheque);

        [OperationContract]
        Task<List<Cheque>> GetChequesAsync(int howMany);

    }
}
