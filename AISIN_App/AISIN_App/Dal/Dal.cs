using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AISIN_App
{
    public class Dal
    {
        StringBuilder _SbQry;

        #region Common
        public DataTable GetPart(int Filter = 1)
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("Exec Prc_GetPart " + Filter + "");
                oDb.Connect();
                return oDb.GetDataTable(_SbQry.ToString());
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
        }

        #endregion

        #region AppVersion Update

        public DataTable GetAppVersion()
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("Exec [Prc_GetAppVersion] '" + EnumAppType.DESKTOPAPP + "'");
                oDb.Connect();
                return oDb.GetDataTable(_SbQry.ToString());
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
        }

        #endregion

        #region GroupMaster

        public DataSet GetGroup(Group group)
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("Exec Prc_GroupMaster '" + group.DbType + "','" + group.GroupName + "'");
                oDb.Connect();
                return oDb.GetDataSet(_SbQry.ToString());
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
        }

        public void SaveGroup(Group group, DataGridView dgv)
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("Exec Prc_GroupMaster '" + EnumDbType.INSERT + "','" + group.GroupName + "','','" + group.CreatedBy + "'");
                oDb.Connect();
                oDb.BeginTran();
                //First Insert Group Name 
                oDb.GetDataTable(_SbQry.ToString());
                //Now Insert Group Rights
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (Convert.ToBoolean(row.Cells["HasRight"].Value) == true)
                    {
                        _SbQry.Length = 0;
                        _SbQry.AppendLine("Exec Prc_GroupMaster 'INSERT_GROUP_RIGHT','" + group.GroupName + "','" + row.Cells["ModuleId"].Value.ToString() + "','" + group.CreatedBy + "'");
                        oDb.GetDataTable(_SbQry.ToString());
                    }
                }
                oDb.CommitTran();
            }
            catch (Exception ex) { oDb.RollBackTran(); throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
        }

        public void UpdateGroup(Group group, DataGridView dgv)
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("Exec Prc_GroupMaster 'DELETE_GROUP_RIGHT','" + group.GroupName + "','','" + group.CreatedBy + "'");
                oDb.Connect();
                oDb.BeginTran();
                //First Insert Group Name 
                oDb.GetDataTable(_SbQry.ToString());
                //Now Insert Group Rights
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (Convert.ToBoolean(row.Cells["HasRight"].Value) == true)
                    {
                        _SbQry.Length = 0;
                        _SbQry.AppendLine("Exec Prc_GroupMaster 'INSERT_GROUP_RIGHT','" + group.GroupName + "','" + row.Cells["ModuleId"].Value.ToString() + "','" + group.CreatedBy + "'");
                        oDb.GetDataTable(_SbQry.ToString());
                    }
                }
                oDb.CommitTran();
            }
            catch (Exception ex) { oDb.RollBackTran(); throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
        }

        #endregion

        #region User Master

        public DataTable GetGroupName()
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("SELECT GroupName FROM GROUPMASTER Order By GroupName");
                oDb.Connect();
                return oDb.GetDataTable(_SbQry.ToString());
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
        }

        public DataTable ManageUser(User user)
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("Exec Prc_UserMaster '" + user.DbType + "','" + user.UserId + "','" + user.Name + "'");
                _SbQry.AppendLine(",'" + user.Password + "','" + user.Group + "','" + user.CreatedBy + "','" + user.NewPassword + "'");
                oDb.Connect();
                return oDb.GetDataTable(_SbQry.ToString());
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
        }

        #endregion

        #region Menu

        public DataTable GetUserRight(string UserGroup)
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("SELECT ModuleId FROM GroupRight Where GroupName = '" + UserGroup + "'");
                oDb.Connect();
                return oDb.GetDataTable(_SbQry.ToString());
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
        }

        public DataTable GetTimerTime(string Type = "")
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("Exec [Prc_GetTimerTime] '" + Type + "'");
                oDb.Connect();
                return oDb.GetDataTable(_SbQry.ToString());
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
        }

        #endregion

        #region Line Master

        public DataTable ManageLine(Line line)
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("Exec [Prc_LineMaster] '" + line.DbType + "','" + line.LineNo + "','" + line.Description + "'");
                _SbQry.AppendLine(",'" + line.CreatedBy + "','" + line.BackNo + "'");
                oDb.Connect();
                return oDb.GetDataTable(_SbQry.ToString());
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
        }

        #endregion

        #region Customer Master
        public DataTable GetCustomerPart(string LocationCode)
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder();
                _SbQry.AppendLine("Exec [Prc_GetCustomerPart] '" + LocationCode + "'");
                oDb.Connect();
                return oDb.GetDataTable(_SbQry.ToString());
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
        }
        public DataTable ManageCustomer(Customer customer)
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder();
                _SbQry.AppendLine("Exec [Prc_CustomerMaster] '" + customer.DbType + "','" + customer.CustomerCode + "','" + customer.Name + "'");
                _SbQry.AppendLine(", '" + customer.Address + "', '" + customer.Location + "','" + customer.CreatedBy + "','" + customer.IsCustomerEnable + "'");
                oDb.Connect();
                return oDb.GetDataTable(_SbQry.ToString());
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
        }
        public string SaveCustomerData(Customer customer)
        {
            clsDB oDb = new clsDB();
            try
            {
                string Msg = "Not Saved";
                _SbQry = new StringBuilder();
                oDb.Connect();
                oDb.BeginTran();

                //Always delete mapping data before saving /updating
                oDb.ExecuteNonQuery("Delete from dbo.CustomerMaster_BackNo_Mapping WHERE CustomerCode = '" + customer.CustomerCode + "'");
                //Add/Update data in Customer Table
                _SbQry.AppendLine("Exec [Prc_CustomerMaster] '" + customer.DbType + "','" + customer.CustomerCode + "','" + customer.Name + "'");
                _SbQry.AppendLine(", '" + customer.Address + "', '" + customer.Location + "','" + customer.CreatedBy + "','" + customer.IsCustomerEnable + "'");
                oDb.GetDataTable(_SbQry.ToString());
                _SbQry.Length = 0;
                //Now Insert Mapping data
                foreach (var item in customer.ListBackNo)
                    oDb.ExecuteNonQuery("Insert Into CustomerMaster_BackNo_Mapping(CustomerCode,BackNo) Values('" + customer.CustomerCode + "','" + item + "')");

                Msg = "SUCCESS";
                oDb.CommitTran();
                return Msg;
            }
            catch (Exception ex) { oDb.RollBackTran(); throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
        }

        #endregion

        #region Part Master

        public DataTable ManagePart(Part part)
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("Exec [Prc_PartMaster] '" + part.DbType + "','" + part.BackNo + "','" + part.Description + "'");
                _SbQry.AppendLine(",'" + part.CreatedBy + "'," + part.StandardBinQty + ",'" + part.PartNo + "','" + part.CustomerPartNo + "','" + part.IsBarcodeAvailable + "'");
                oDb.Connect();
                return oDb.GetDataTable(_SbQry.ToString());
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
        }

        #endregion

        #region Location Master
        public DataTable GetLocationPart(string LocationCode)
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder();
                _SbQry.AppendLine("Exec [Prc_GetLocationPart] '" + LocationCode + "'");
                oDb.Connect();
                return oDb.GetDataTable(_SbQry.ToString());
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
        }
        public DataTable ManageLocation(Location location)
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder();
                _SbQry.AppendLine("Exec [Prc_LocationMaster] '" + location.DbType + "','" + location.LocationCode + "','" + location.Description + "'");
                _SbQry.AppendLine(", " + location.Capacity + ",'','" + location.CreatedBy + "'");
                oDb.Connect();
                return oDb.GetDataTable(_SbQry.ToString());
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
        }
        public string SaveLocationData(Location location)
        {
            clsDB oDb = new clsDB();
            try
            {
                string Msg = "Not Saved";
                _SbQry = new StringBuilder();
                oDb.Connect();
                oDb.BeginTran();

                //Always delete mapping data before saving /updating
                oDb.ExecuteNonQuery("Delete from dbo.LocationMaster_BackNo_Mapping WHERE LocationCode = '" + location.LocationCode + "'");
                //Add/Update data in Location Table
                _SbQry.AppendLine("Exec [Prc_LocationMaster] '" + location.DbType + "','" + location.LocationCode + "','" + location.Description + "'");
                _SbQry.AppendLine(", " + location.Capacity + ",'','" + location.CreatedBy + "'");
                oDb.GetDataTable(_SbQry.ToString());
                _SbQry.Length = 0;
                //Now Insert Mapping data
                foreach (var item in location.ListBackNo)
                    oDb.ExecuteNonQuery("Insert Into LocationMaster_BackNo_Mapping(LocationCode,BackNo) Values('" + location.LocationCode + "','" + item + "')");

                Msg = "SUCCESS";
                oDb.CommitTran();
                return Msg;
            }
            catch (Exception ex) { oDb.RollBackTran(); throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
        }

        #endregion

        #region Production
   
        //It will return multiple datatable while loading cutting form
        public DataSet GetPendingKanBanData(Production prod)
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("Exec [Prc_GetProductionData] '" + prod.LineNo + "','" + prod.BinBarcode + "','" + prod.IsBinBarcode + "'");
                oDb.Connect();
                return oDb.GetDataSet(_SbQry.ToString());
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
        }


        public DataSet GetPendingKanBanData_Data(Production prod)
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("Exec [Prc_GetProductionDataData] '" + prod.LineNo + "','" + prod.BinBarcode + "','" + prod.IsBinBarcode + "'");
                oDb.Connect();
                return oDb.GetDataSet(_SbQry.ToString());
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
        }

        public DataSet GetPendingKanBanDataNew(Production prod)
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("Exec [Prc_GetProductionData_New] '" + prod.LineNo + "','" + prod.BinBarcode + "','" + prod.IsBinBarcode + "'");
                oDb.Connect();
                return oDb.GetDataSet(_SbQry.ToString());
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
        }

        public DataTable SaveProductionData(Production prod)
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("Exec [Prc_SaveProductionData] '" + prod.DbType + "','" + prod.ProductionId + "'");
                _SbQry.AppendLine(",'" + prod.LineNo + "','" + prod.BackNo + "','" + prod.KanBanBarcode + "','" + prod.BinBarcode + "','" + prod.PartBarcode + "'");
                _SbQry.AppendLine("," + prod.StandardBinQty + ",'" + prod.CreatedBy + "'");

                oDb.Connect();
                return oDb.GetDataTable(_SbQry.ToString());
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
        }

        public DataTable SaveProductionDataNew(Production prod)
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("Exec [Prc_SaveProductionData_New] '" + prod.DbType + "','" + prod.ProductionId + "'");
                _SbQry.AppendLine(",'" + prod.LineNo + "','" + prod.BackNo + "','" + prod.KanBanBarcode + "','" + prod.BinBarcode + "','" + prod.PartBarcode + "'");
                _SbQry.AppendLine("," + prod.StandardBinQty + ",'" + prod.CreatedBy + "'");

                oDb.Connect();
                return oDb.GetDataTable(_SbQry.ToString());
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
        }
        //Added by Akhilesh on 31-12-2020
        public DataTable SavePartRejectionData(Production prod)
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("Exec [Prc_SaveRejectionData] '" + prod.DbType + "','" + prod.BinBarcode + "'");
                _SbQry.AppendLine(",'" + prod.PartBarcode + "','','" + prod.CreatedBy + "','" + prod.ProductionId + "',0");
              
                oDb.Connect();
                return oDb.GetDataTable(_SbQry.ToString());
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
        }
        //End
        #endregion

        #region RePrint

        public DataTable ManageRePrint(RePrint rePrint)
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("Exec Prc_GetRePrintData '" + rePrint.DbType + "','" + rePrint.PartBarcode + "','" + rePrint.BinBarcode + "'");
                _SbQry.AppendLine(",'" + rePrint.LineNo + "','" + rePrint.FromDate + "','" + rePrint.ToDate + "','" + rePrint.KanBanBarcode + "','" + rePrint.CreatedBy + "'");
                oDb.Connect();
                return oDb.GetDataTable(_SbQry.ToString());
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
        }

        #endregion

        #region PickList

        public DataTable GetPickListNo()
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("Exec Prc_GetPickListNo");
                oDb.Connect();
                return oDb.GetDataTable(_SbQry.ToString());
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
        }

        public string SavePickListData(DataGridView dgv, string PickListNo, int SrNo)
        {
            clsDB oDb = new clsDB();
            try
            {
                string Msg = "Not Saved";
                _SbQry = new StringBuilder();
                oDb.Connect();
                oDb.BeginTran();
                //Check complete picklist should have same customer code
                string CustomerCode = dgv.Rows[0].Cells[0].Value.ToString().Trim();
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    //if all the rows have same customer code 
                    if (CustomerCode == dgv.Rows[i].Cells[0].Value.ToString().Trim())
                    {
                        _SbQry.AppendLine("Exec Prc_SavePickListData '" + PickListNo + "','" + dgv.Rows[i].Cells[0].Value.ToString().Trim() + "'");
                        _SbQry.AppendLine(",'" + dgv.Rows[i].Cells[1].Value.ToString().Trim() + "','" + dgv.Rows[i].Cells[2].Value.ToString().Trim() + "'");
                        _SbQry.AppendLine(",'" + dgv.Rows[i].Cells[3].Value.ToString().Trim() + "','" + dgv.Rows[i].Cells[4].Value.ToString().Trim() + "'");
                        _SbQry.AppendLine(",'" + ClsGlobal.UserId + "'," + SrNo + "");
                        Msg = oDb.GetDataTable(_SbQry.ToString()).Rows[0]["Result"].ToString();
                        _SbQry.Length = 0;
                        if (Msg != "Y")
                        {
                            throw new Exception(Msg);
                        }
                    }
                    else
                        throw new Exception("One picklist can not have multiple customer");
                }
                oDb.CommitTran();
                return Msg;
            }
            catch (Exception ex) { oDb.RollBackTran(); throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
        }

        #endregion

        #region Loading List

        public DataTable ManageLoadingList(EnumDbType dbType, string fromDate = "", string toDate = "", string truckNo = "")
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("Exec Prc_LoadingList '" + dbType + "','" + fromDate + "','" + toDate + "','" + truckNo + "','" + ClsGlobal.UserId + "'");
                oDb.Connect();
                return oDb.GetDataTable(_SbQry.ToString());
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
        }

        #endregion

        #region Report

        public DataTable ManageReport(EnumDbType dbType, string fromDate = "", string toDate = "", string Line = "", string BackNo = "", string TruckNo = "", string sDispatchID = "0")
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("Exec Prc_Report '" + dbType + "','" + fromDate + "','" + toDate + "','" + ClsGlobal.UserId + "','" + Line + "','" + BackNo + "','" + TruckNo + "','" + sDispatchID + "'");
                oDb.Connect();
                return oDb.GetDataTable(_SbQry.ToString());
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
        }


        public DataTable ManageChildPartReport(EnumDbType dbType, string fromDate = "", string toDate = "", string Line = "", string BackNo = "", string TruckNo = "", string sDispatchID = "0")
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("Exec Prc_Report '" + dbType + "','" + fromDate + "','" + toDate + "','" + ClsGlobal.UserId + "','" + Line + "','" + BackNo + "','" + TruckNo + "','" + sDispatchID + "'");
                oDb.Connect();
                return oDb.GetDataTable(_SbQry.ToString());
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
        }

        public DataTable ProcessWiseReport(string dbType, string fromDate = "", string toDate = "", string Line = "", string BackNo = "")
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("Exec Prc_Report '" + dbType + "','" + fromDate + "','" + toDate + "','" + ClsGlobal.UserId + "','" + Line + "','" + BackNo + "'");
                oDb.Connect();
                return oDb.GetDataTable(_SbQry.ToString());
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
        }
        #endregion

        #region Get Back No using Scalar Fucntion
        public DataTable GetBackNoUsingFucntion(string kanbaanBarcode)
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("Exec PRC_GET_BACK_NO '" + kanbaanBarcode + "'");
                oDb.Connect();
                DataTable dt= oDb.GetDataTable(_SbQry.ToString());
                return dt;
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
        }
        #endregion


        #region Get Back No using Scalar Fucntion
        public DataTable GETBinUsingKanban(string kanbaanBarcode)
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("Exec PRC_GET_Bin_Using_Kanban '" + kanbaanBarcode + "'");
                oDb.Connect();
                DataTable dt = oDb.GetDataTable(_SbQry.ToString());
                return dt;
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
        }
        #endregion

        public DataTable Delete_Existing_Part_Using_Kanban(string KANBAAN_BARCODE,string BinBarcode,string DeletedBy)
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder();
                _SbQry.AppendLine("Exec [PRC_Delete_Existing_Part_Using_Kanban] '" + KANBAAN_BARCODE + "','" + BinBarcode + "','" + DeletedBy + "'");
                oDb.Connect();
                return oDb.GetDataTable(_SbQry.ToString());
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
        }

        public DataTable GetLinePart(string PartNo)
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder();
                _SbQry.AppendLine("Exec [Prc_LinePartMapping] '" + PartNo + "'");
                oDb.Connect();
                return oDb.GetDataTable(_SbQry.ToString());
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
        }
        public DataTable ManagePartLineMapping(LinePartMapping location)
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder();
                _SbQry.AppendLine("Exec [Prc_LinePartMapping] '" + location.DbType + "','" + location.PartNo + "','" + location.Description + "'");
                _SbQry.AppendLine(",'" + location.CreatedBy + "'");
                oDb.Connect();
                return oDb.GetDataTable(_SbQry.ToString());
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
        }
        public string SavePartLineMappingData(LinePartMapping location)
        {
            clsDB oDb = new clsDB();
            try
            {
                string Msg = "Not Saved";
                _SbQry = new StringBuilder();
                oDb.Connect();
                oDb.BeginTran();

                //Always delete mapping data before saving /updating
                oDb.ExecuteNonQuery("Delete from dbo.PartMaster_Line_Mapping WHERE PartNo = '" + location.PartNo + "'");
                //Add/Update data in Location Table
                _SbQry.AppendLine("Exec [Prc_LinePartMapping] '" + location.DbType + "','" + location.PartNo + "','" + location.Description + "'");
                _SbQry.AppendLine(",'" + location.CreatedBy + "'");
                oDb.GetDataTable(_SbQry.ToString());
                _SbQry.Length = 0;
                //Now Insert Mapping data
                foreach (var item in location.ListLineNo)
                    oDb.ExecuteNonQuery("Insert Into PartMaster_Line_Mapping(PartNo,[LineNo]) Values('" + location.PartNo + "','" + item + "')");

                Msg = "SUCCESS";
                oDb.CommitTran();
                return Msg;
            }
            catch (Exception ex) { oDb.RollBackTran(); throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
        }

        #region Get PartNo using Scalar Fucntion
        public DataTable GetPartNoFormBackNo(string backno)
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("Exec PRC_PART_NO_USING_BACK_NO '" + backno + "'");
                oDb.Connect();
                DataTable dt = oDb.GetDataTable(_SbQry.ToString());
                return dt;
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
        }
        #endregion

        #region validate PartNo barcode
        public DataTable ValidatePartBarcodeFromPLC(string PartBarcode)
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("Exec PRC_PART_BARCODE_FROM_PLC '" + PartBarcode + "'");
                oDb.Connect();
                DataTable dt = oDb.GetDataTable(_SbQry.ToString());
                return dt;
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
        }
        #endregion


      
    }
}
