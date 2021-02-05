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


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class Cheque
    {
        [DataMember]
        public Guid ChequeId { get; set; }

        [DataMember]
        public string ChequeNumber { get; set; }

        [DataMember]
        public decimal Summ { get; set; }

        [DataMember]
        public decimal Discount { get; set; }

        [DataMember]
        public string[] Articles { get; set; }
    }
}
