using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISIN_App
{
    #region Group Master
    public class Group : Common
    {
        public string GroupName { get; set; }
    }
    #endregion

    #region Group Rights
    public class GroupRights : Common
    {
        public string GroupName { get; set; }
        public string ModuleId { get; set; }
    }
    #endregion

    #region User Master
    public class User : Common
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string Group { get; set; }
    }
    #endregion

    #region Machine Master
    public class Machine : Common
    {
        public string MachineNo { get; set; }
        public string Description { get; set; }
    }
    #endregion

    #region Trolley Master
    public class Trolley : Common
    {
        public string TrolleyNo { get; set; }
        public int PackSize { get; set; }
        public string Description { get; set; }
        public bool IsReturnAble { get; set; }
    }
    #endregion

    #region Line Master
    public class Line : Common
    {
        public string LineNo { get; set; }
        public string Description { get; set; }
        public string BackNo { get; set; }
    }
    #endregion

    #region Customer Master
    public class Customer : Common
    {
        public string CustomerCode { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Location { get; set; }
        public bool IsCustomerEnable { get; set; }
        public List<string> ListBackNo { get; set; }
    }
    #endregion

    #region Part Master

    public class Part : Common
    {
        public string BackNo { get; set; }
        public string Description { get; set; }
        public int StandardBinQty { get; set; }
        public string PartNo { get; set; }
        public string CustomerPartNo { get; set; }
        public bool IsBarcodeAvailable { get; set; }
    }
    #endregion

    #region Location Master

    public class Location : Common
    {
        public string LocationCode { get; set; }
        public string Description { get; set; }
        public int Capacity { get; set; }
        public List<string> ListBackNo { get; set; }
    }
    #endregion

    #region Line Part Mapping Master

    public class LinePartMapping : Common
    {
        public string PartNo { get; set; }
        public string Description { get; set; }
        public string Line { get; set; }
        public List<string> ListLineNo { get; set; }
    }
    #endregion

    #region Shift Master
    public class Shift : Common
    {
        public string ShiftName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
    #endregion

    #region Production Plan Cutting
    public class ProductionPlan : Common
    {
        public string ProductionPlanId { get; set; }
        public string Month { get; set; }
        public int MonthNo { get; set; }
        public int Year { get; set; }
        public int OrderNo { get; set; }
        public string ModelNo { get; set; }
        public string Remarks { get; set; }
        public int Qty { get; set; }
    }
    #endregion

    #region Production
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
    }

    #endregion

    #region QA

    public class QA : Common
    {
        public string TrolleyCard { get; set; }
        public string Shift { get; set; }
        public int Status { get; set; }
        public int PickedQty { get; set; }
        public int PartialNgQty { get; set; }
        public string PartialNgReason { get; set; }
        public string LotNo { get; set; }
        public bool IsOnHold { get; set; }
    }

    #endregion

    #region Cutting TrolleyUpdate

    public class CuttingTrolleyUpdate:Common
    {
        public string Id { get; set; }
        public string LotNo { get; set; }
        public string LotNoDate { get; set; }
        public bool IsValueChange { get; set; }
    }

    #endregion

    #region Dispatch Order

    public class DispatchOrder : Common
    {
        public string DispatchOrderNo { get; set; }
        public int SrNo { get; set; }
        public string Shift { get; set; }
        public string DispatchDate { get; set; }
        public string ModelNo { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int Qty { get; set; }
        public int OldQty { get; set; }
        public int DispatchQty { get; set; }
    }

    #endregion

    #region Color Master
    public class Colors : Common
    {
        public long  RowId { get; set; }
        public string ColorName { get; set; }
    }
    #endregion

    #region Production
    public class RePrint : Common
    {
        public string LineNo { get; set; }
        public string KanBanBarcode { get; set; }
        public string BinBarcode { get; set; }
        public string PartBarcode { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }

    #endregion
}
