using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace IOCLAndroidApp.Models
{

    #region FG Feeding
    public class FgFeeding
    {
        public string LocationCode { get; set; }
        public int Capacity { get; set; }
        public int AvailableQty { get; set; }
    }

    #endregion

    #region FG Picking
    public class PickList
    {
        public string BackNo { get; set; }
        public int Qty { get; set; }
        public int ScanQty { get; set; }
        public bool IsCustomerKanbanEnable { get; set; }
    }

    #endregion

    #region FG Picking BackNo Location
    public class BinLocation
    {
        public string LocationCode { get; set; }
        public string BinBarcode { get; set; }
        public int ScanQty { get; set; }
    }

    #endregion
}