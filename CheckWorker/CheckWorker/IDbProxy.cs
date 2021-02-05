using CheckWorker.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

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
        void TakeCheque(Cheque cheque);

        [OperationContract]
        List<Cheque> GetCheques(int howMany);

    }
}
