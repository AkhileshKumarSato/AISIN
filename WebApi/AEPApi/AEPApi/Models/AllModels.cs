using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AEPApi.Models
{
    public enum EnumDbType { SELECT, INSERT, UPDATE, DELETE, SELECTBYID, SEARCH, VALIDATEUSER, UPDATEPASSWORD, KANBAN, PART, REJECT };
    public enum EnumProductionStatus { FRACTION = 1, COMPLETE = 2, SERVICE_PART = 3, REJECT = 4 };

    public class Common
    {
        public string CreatedBy { get; set; }
        public EnumDbType DbType { get; set; }
    }
    public class Production : Common
    {
        public string LineNo { get; set; }
        public string ProductionId { get; set; }
        public string BackNo { get; set; }
        public string KanBanBarcode { get; set; }
        public string BinBarcode { get; set; }
        public string PartBarcode { get; set; }
        public int StandardBinQty { get; set; }
        public int ScanQty { get; set; }
        public int RejectQty { get; set; }
        public bool IsServicePart { get; set; }
        public bool IsBinBarcode { get; set; }
        public int Status { get; set; }


        public string Response { get; set; }
        public string ErrorMessage { get; set; }

    }


    #region Part Master

    public class Part : Common
    {
        public string BackNo { get; set; }
        public string Description { get; set; }
        public int StandardBinQty { get; set; }
        public string PartNo { get; set; }
        public string CustomerPartNo { get; set; }
        public bool IsBarcodeAvailable { get; set; }

        public string Response { get; set; }
        public string ErrorMessage { get; set; }
    }
    #endregion
}