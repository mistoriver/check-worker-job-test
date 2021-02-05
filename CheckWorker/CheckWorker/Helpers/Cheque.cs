using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CheckWorker.Helpers
{
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