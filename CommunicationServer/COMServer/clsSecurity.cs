using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TPR_App;
using System.IO;
//using UmsDLL;
using System.Windows.Forms;
namespace COMServer.Classes
{
    class clsSecurity
    {
        StringBuilder _SbQry = null;
        clsMsgRule oRule = new clsMsgRule();

        #region App New Version Update

        internal string GetAppVersion()
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("Exec [Prc_GetAppVersion] '" + EnumAppType.ANDROIDAPP + "'");
                oDb.Connect();
                DataTable dt = oDb.GetDataTable(_SbQry.ToString());
                if (dt.Rows.Count > 0)
                {
                    oRule.sResponse = clsMsgRule.sValid + "~" + dt.Rows[0]["Version"].ToString();
                }
                else
                {
                    oRule.sResponse = clsMsgRule.sInValid + "~App new version information not found,please check";
                }
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
            return oRule.sResponse;
        }
        internal string GetNewExeDevice()
        {
            try
            {
                if (!Directory.Exists(Application.StartupPath + "\\NewApp\\AndroidApp"))
                {
                    throw new Exception("Location not defined for new app");
                }
                string[] AllFiles = Directory.GetFiles(Application.StartupPath + "\\NewApp\\AndroidApp");
                string FileName = Path.GetFileName(AllFiles[0]);
                byte[] FileNewExe = File.ReadAllBytes(Application.StartupPath + "\\NewApp\\AndroidApp\\" + FileName);

                string exestring = Convert.ToBase64String(FileNewExe);
                oRule.sResponse = clsMsgRule.sValid + "~" + exestring + "~" + FileName;
            }
            catch (Exception ex) { throw ex; }
            return oRule.sResponse + "}";
        }
        internal string GetNewExeDesktop()
        {
            try
            {
                if (!Directory.Exists(Application.StartupPath + "\\NewApp\\DesktopApp"))
                {
                    throw new Exception("Location not defined for new app");
                }
                string[] AllFiles = Directory.GetFiles(Application.StartupPath + "\\NewApp\\DesktopApp");
                string FileName = Path.GetFileName(AllFiles[0]);
                byte[] FileNewExe = File.ReadAllBytes(Application.StartupPath + "\\NewApp\\DesktopApp\\" + FileName);

                string exestring = Convert.ToBase64String(FileNewExe);
                oRule.sResponse = clsMsgRule.sValid + "~" + exestring + "~" + FileName;
            }
            catch (Exception ex) { throw ex; }
            return oRule.sResponse;
        }

        #endregion

        #region Login & Menu
        internal string ManageUser(User user)
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("Exec Prc_UserMaster '" + EnumDbType.VALIDATEUSER + "','" + user.UserId + "','" + user.Name + "'");
                _SbQry.AppendLine(",'" + user.Password + "','" + user.Group + "','" + user.CreatedBy + "','" + user.NewPassword + "'");
                oDb.Connect();
                DataTable dt = oDb.GetDataTable(_SbQry.ToString());
                if (dt.Rows.Count > 0)
                {
                    oRule.sResponse = clsMsgRule.sValid + "~" + dt.Rows[0]["USERID"].ToString() + "~" + dt.Rows[0]["UserName"].ToString() + "~" + dt.Rows[0]["GroupName"].ToString();
                }
                else
                {
                    oRule.sResponse = clsMsgRule.sInValid + "~Wrong UserId / Password";
                }
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
            return oRule.sResponse;
        }

        internal string GetUserRights(string UserGroup)
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("SELECT ModuleId FROM GroupRight Where GroupName = '" + UserGroup + "'");
                oDb.Connect();
                DataTable dt = oDb.GetDataTable(_SbQry.ToString());
                oRule.sResponse = clsMsgRule.sValid + "~";
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        oRule.sResponse += row["ModuleId"].ToString() + "#";
                    }
                    oRule.sResponse = oRule.sResponse.TrimEnd('#');
                }
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
            return oRule.sResponse;
        }

        #endregion

        #region Rejection
        internal string GetBinData(string Binbarcode)
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("Exec [Prc_GetBinData] '" + Binbarcode + "'");
                oDb.Connect();
                DataTable dt = oDb.GetDataTable(_SbQry.ToString());
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Result"].ToString() == "Y")
                        oRule.sResponse = clsMsgRule.sValid + "~" + dt.Rows[0]["BinQty"].ToString();
                    else
                        oRule.sResponse = clsMsgRule.sInValid + "~" + dt.Rows[0]["Result"].ToString();
                }
                else
                {
                    oRule.sResponse = clsMsgRule.sInValid + "~No value return from database";
                }
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
            return oRule.sResponse;
        }
        internal string SaveBinRejectionData(string Binbarcode, string Reason, string UserId)
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("Exec [Prc_SaveRejectionData] '" + EnumDbType.BIN + "','" + Binbarcode + "',''," + Convert.ToInt32(EnumProductionStatus.REJECT) + ",'" + UserId + "','" + Reason + "'");
                oDb.Connect();
                DataTable dt = oDb.GetDataTable(_SbQry.ToString());
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Result"].ToString() == "Y")
                        oRule.sResponse = clsMsgRule.sValid + "~" + dt.Rows[0]["Msg"].ToString();
                    else
                        oRule.sResponse = clsMsgRule.sInValid + "~" + dt.Rows[0]["Result"].ToString();
                }
                else
                {
                    oRule.sResponse = clsMsgRule.sInValid + "~No value return from database";
                }
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
            return oRule.sResponse;
        }

        internal string SavePartRejectionData(string Partbarcode, string Reason, string UserId)
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("Exec [Prc_SaveRejectionData] '" + EnumDbType.PART + "','','" + Partbarcode + "'," + Convert.ToInt32(EnumProductionStatus.FRACTION) + ",'" + UserId + "','" + Reason + "'");
                oDb.Connect();
                DataTable dt = oDb.GetDataTable(_SbQry.ToString());
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Result"].ToString() == "Y")
                        oRule.sResponse = clsMsgRule.sValid + "~" + dt.Rows[0]["Msg"].ToString();
                    else
                        oRule.sResponse = clsMsgRule.sInValid + "~" + dt.Rows[0]["Result"].ToString();
                }
                else
                {
                    oRule.sResponse = clsMsgRule.sInValid + "~No value return from database";
                }
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
            return oRule.sResponse;
        }

        internal string SavePartRejectionDataNonBarcode(string Binbarcode, int RejectedQty, string Reason, string UserId)
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("Exec [Prc_SaveRejectionData] '" + EnumDbType.REJECT_NONBARCODE + "','" + Binbarcode + "',''," + Convert.ToInt32(EnumProductionStatus.FRACTION) + "");
                _SbQry.AppendLine(",'" + UserId + "','" + Reason + "'," + RejectedQty + "");
                oDb.Connect();
                DataTable dt = oDb.GetDataTable(_SbQry.ToString());
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Result"].ToString() == "Y")
                        oRule.sResponse = clsMsgRule.sValid + "~" + dt.Rows[0]["Msg"].ToString();
                    else
                        oRule.sResponse = clsMsgRule.sInValid + "~" + dt.Rows[0]["Result"].ToString();
                }
                else
                {
                    oRule.sResponse = clsMsgRule.sInValid + "~No value return from database";
                }
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
            return oRule.sResponse;
        }

        #endregion

        #region Fraction
        internal string GetFractionNewBinData(string Binbarcode)
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("Exec [Prc_GetFractionBinData] 'False','" + Binbarcode + "'");
                oDb.Connect();
                DataTable dt = oDb.GetDataTable(_SbQry.ToString());
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Result"].ToString() == "Y")
                        oRule.sResponse = clsMsgRule.sValid + "~" + dt.Rows[0]["StandardBinQty"].ToString() + "~" + dt.Rows[0]["ScanQty"].ToString();
                    else
                        oRule.sResponse = clsMsgRule.sInValid + "~" + dt.Rows[0]["Result"].ToString();
                }
                else
                {
                    oRule.sResponse = clsMsgRule.sInValid + "~No value return from database";
                }
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
            return oRule.sResponse;
        }

        internal string SaveFractionNewBinData(string Binbarcode, string PartBarcode, string UserId)
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("Exec [Prc_SaveFractionData] '" + EnumDbType.BIN + "','" + Binbarcode + "','" + PartBarcode + "'");
                _SbQry.AppendLine(",''," + Convert.ToInt32(EnumProductionStatus.COMPLETE) + ",'" + UserId + "',0");
                oDb.Connect();
                DataTable dt = oDb.GetDataTable(_SbQry.ToString());
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Result"].ToString() == "Y")
                        oRule.sResponse = clsMsgRule.sValid + "~" + dt.Rows[0]["Msg"].ToString();
                    else
                        oRule.sResponse = clsMsgRule.sInValid + "~" + dt.Rows[0]["Result"].ToString();
                }
                else
                {
                    oRule.sResponse = clsMsgRule.sInValid + "~No value return from database";
                }
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
            return oRule.sResponse;
        }

        internal string GetFractionBinData(string Binbarcode)
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("Exec [Prc_GetFractionBinData] 'True','" + Binbarcode + "'");
                oDb.Connect();
                DataTable dt = oDb.GetDataTable(_SbQry.ToString());
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Result"].ToString() == "Y")
                        oRule.sResponse = clsMsgRule.sValid + "~" + dt.Rows[0]["ScanQty"].ToString();
                    else
                        oRule.sResponse = clsMsgRule.sInValid + "~" + dt.Rows[0]["Result"].ToString();
                }
                else
                {
                    oRule.sResponse = clsMsgRule.sInValid + "~No value return from database";
                }
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
            return oRule.sResponse;
        }

        internal string SaveFractionBinData(string Binbarcode, string FractionBinbarcode, int PickQty, string UserId)
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("Exec [Prc_SaveFractionData] '" + EnumDbType.FRACTION + "','" + Binbarcode + "',''");
                _SbQry.AppendLine(",'" + FractionBinbarcode + "'," + Convert.ToInt32(EnumProductionStatus.COMPLETE) + ",'" + UserId + "'," + PickQty + "");
                oDb.Connect();
                DataTable dt = oDb.GetDataTable(_SbQry.ToString());
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Result"].ToString() == "Y")
                        oRule.sResponse = clsMsgRule.sValid + "~" + dt.Rows[0]["Msg"].ToString();
                    else
                        oRule.sResponse = clsMsgRule.sInValid + "~" + dt.Rows[0]["Result"].ToString();
                }
                else
                {
                    oRule.sResponse = clsMsgRule.sInValid + "~No value return from database";
                }
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
            return oRule.sResponse;
        }

        #endregion

        #region FGFeeding

        internal string GetFgFeedingBinData(string Binbarcode)
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("Exec [Prc_GetSaveFGFeeding] '" + EnumDbType.SELECT + "','" + Binbarcode + "'");
                oDb.Connect();
                DataTable dt = oDb.GetDataTable(_SbQry.ToString());
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Result"].ToString() == "Y")
                    {
                        oRule.sResponse = clsMsgRule.sValid + "~";
                        foreach (DataRow row in dt.Rows)
                        {
                            //Column separator $
                            oRule.sResponse += row["LocationCode"].ToString() + "$" + row["Capacity"].ToString() + "$" + row["Available"].ToString() + "#";
                        }
                        //Row Separator #
                        oRule.sResponse = oRule.sResponse.TrimEnd('#');
                    }
                    else
                        oRule.sResponse = clsMsgRule.sInValid + "~" + dt.Rows[0]["Result"].ToString();
                }
                else
                {
                    oRule.sResponse = clsMsgRule.sInValid + "~Bin back no does not map to any location or all locations are full.";
                }
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
            return oRule.sResponse;
        }

        internal string SaveFgFeedingBinData(string Binbarcode, string LocatonCode, string UserId)
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("Exec [Prc_GetSaveFGFeeding] '" + EnumDbType.UPDATE + "','" + Binbarcode + "','" + LocatonCode + "'");
                _SbQry.AppendLine("," + Convert.ToInt32(EnumProductionStatus.FG_FEEDING) + ",'" + UserId + "'");
                oDb.Connect();
                DataTable dt = oDb.GetDataTable(_SbQry.ToString());
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Result"].ToString() == "Y")
                        oRule.sResponse = clsMsgRule.sValid + "~" + dt.Rows[0]["Msg"].ToString();
                    else
                        oRule.sResponse = clsMsgRule.sInValid + "~" + dt.Rows[0]["Result"].ToString();
                }
                else
                {
                    oRule.sResponse = clsMsgRule.sInValid + "~No value return from database";
                }
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
            return oRule.sResponse;
        }

        #endregion

        #region FGPicking

        internal string GetFgPickPickListNo()
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("Exec [Prc_GetFgPicking_PickListNo] '" + EnumDbType.SELECT + "'");
                oDb.Connect();
                DataTable dt = oDb.GetDataTable(_SbQry.ToString());
                if (dt.Rows.Count > 0)
                {
                    oRule.sResponse = clsMsgRule.sValid;
                    foreach (DataRow row in dt.Rows)
                    {
                        //Column separator $
                        oRule.sResponse += "~" + row["PickListNo"].ToString();
                    }
                }
                else
                {
                    oRule.sResponse = clsMsgRule.sInValid + "~There is no pending picklist";
                }
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
            return oRule.sResponse;
        }

        internal string GetFgPickPickListNoData(string PicListNo)
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("Exec [Prc_GetFgPicking_PickListNo] '" + EnumDbType.PICK_DETAILS + "','" + PicListNo + "'");
                oDb.Connect();
                DataTable dt = oDb.GetDataTable(_SbQry.ToString());
                if (dt.Rows.Count > 0)
                {
                    oRule.sResponse = clsMsgRule.sValid + "~";
                    foreach (DataRow row in dt.Rows)
                    {
                        //Column separator $
                        oRule.sResponse += row["BackNo"].ToString() + "$" + row["RequestBinQty"].ToString() + "$" + row["PickQty"].ToString() + "$" + row["IsCustomerKanban"].ToString() + "#";
                    }
                    //Row Separator #
                    oRule.sResponse = oRule.sResponse.TrimEnd('#');
                }
                else
                {
                    oRule.sResponse = clsMsgRule.sInValid + "~No data found for picklist no. " + PicListNo;
                }
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
            return oRule.sResponse;
        }

        internal string GetFgPickPickListNoBinLocationData(string BackNo)
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("Exec [Prc_GetFgPicking_PickListNo] '" + EnumDbType.BIN + "','','" + BackNo + "'");
                oDb.Connect();
                DataTable dt = oDb.GetDataTable(_SbQry.ToString());
                if (dt.Rows.Count > 0)
                {
                    oRule.sResponse = clsMsgRule.sValid + "~";
                    foreach (DataRow row in dt.Rows)
                    {
                        //Column separator $
                        oRule.sResponse += row["LocationCode"].ToString() + "$" + row["BinBarcode"].ToString() + "$" + row["ScanQty"].ToString() + "#";
                    }
                    //Row Separator #
                    oRule.sResponse = oRule.sResponse.TrimEnd('#');
                }
                else
                {
                    oRule.sResponse = clsMsgRule.sInValid + "~No data found for back no. " + BackNo;
                }
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
            return oRule.sResponse;
        }

        internal string FgPickingValidateBin(string PicListNo, string Binbarcode, string Location, string CustomerKanban, string UserId)
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("Exec [Prc_SaveFgPickingData] '" + EnumDbType.SELECT + "','" + PicListNo + "','" + Location + "'");
                _SbQry.AppendLine(",'" + Binbarcode + "','" + CustomerKanban + "'," + Convert.ToInt32(EnumProductionStatus.PICK) + ",'" + UserId + "'");
                oDb.Connect();
                DataTable dt = oDb.GetDataTable(_SbQry.ToString());
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Result"].ToString() == "Y")
                        oRule.sResponse = clsMsgRule.sValid + "~" + dt.Rows[0]["CustomerPartNo"].ToString();
                    else
                        oRule.sResponse = clsMsgRule.sInValid + "~" + dt.Rows[0]["Result"].ToString();
                }
                else
                {
                    oRule.sResponse = clsMsgRule.sInValid + "~No value return from database";
                }
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
            return oRule.sResponse;
        }

        internal string FgPickingSaveBin(string PicListNo, string Binbarcode, string Location, string CustomerKanban, string UserId, bool IsKanban)
        {
            clsDB oDb = new clsDB();
            try
            {
                //If data is saving after customer kanban so bin will not validate again because it has been validated during bin barcode scan
                //if not kanban means bin will be validate and saved 
                EnumDbType DbType = IsKanban ? EnumDbType.KANBAN : EnumDbType.BIN;
                _SbQry = new StringBuilder("Exec [Prc_SaveFgPickingData] '" + DbType + "','" + PicListNo + "','" + Location + "'");
                _SbQry.AppendLine(",'" + Binbarcode + "','" + CustomerKanban + "'," + Convert.ToInt32(EnumProductionStatus.PICK) + ",'" + UserId + "'");
                oDb.Connect();
                DataTable dt = oDb.GetDataTable(_SbQry.ToString());
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Result"].ToString() == "Y")
                        oRule.sResponse = clsMsgRule.sValid + "~" + dt.Rows[0]["Msg"].ToString();
                    else
                        oRule.sResponse = clsMsgRule.sInValid + "~" + dt.Rows[0]["Result"].ToString();
                }
                else
                {
                    oRule.sResponse = clsMsgRule.sInValid + "~No value return from database";
                }
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
            return oRule.sResponse;
        }

        #endregion

        #region Direct Dispatch

        internal string GetDirectDispatchTruckData(string TruckNo, string UserId)
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("Exec [Prc_GetDirectDispatch_TruckData] '" + TruckNo + "','" + UserId + "'");
                oDb.Connect();
                DataTable dt = oDb.GetDataTable(_SbQry.ToString());
                if (dt.Rows.Count > 0)
                {
                    oRule.sResponse = clsMsgRule.sValid + "~" + dt.Rows[0]["DispatchId"].ToString() + "~" + dt.Rows[0]["BinQty"].ToString();
                }
                else
                {
                    oRule.sResponse = clsMsgRule.sInValid + "~No value return from database";
                }
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
            return oRule.sResponse;
        }

        internal string SaveDirectDispatch(string Binbarcode, string DispatchId, string UserId)
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("Exec [Prc_SaveDirectDispatchData] '" + EnumDbType.UPDATE + "','" + DispatchId + "','" + Binbarcode + "'");
                _SbQry.AppendLine("," + Convert.ToInt32(EnumProductionStatus.DISPATCH) + ",'" + UserId + "'");
                oDb.Connect();
                DataTable dt = oDb.GetDataTable(_SbQry.ToString());
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Result"].ToString() == "Y")
                        oRule.sResponse = clsMsgRule.sValid + "~" + dt.Rows[0]["Msg"].ToString();
                    else
                        oRule.sResponse = clsMsgRule.sInValid + "~" + dt.Rows[0]["Result"].ToString();
                }
                else
                {
                    oRule.sResponse = clsMsgRule.sInValid + "~No value return from database";
                }
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
            return oRule.sResponse;
        }


        //Added By Akhilesh on 11-01-2021
        internal string SaveCompleteDispatch(string DispatchId, string UserId)
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("Exec [Prc_SaveDirectDispatchData] '" + EnumDbType.COMPLETE + "','" + DispatchId + "','" + UserId + "'");
                oDb.Connect();
                DataTable dt = oDb.GetDataTable(_SbQry.ToString());
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Result"].ToString() == "Y")
                        oRule.sResponse = clsMsgRule.sValid + "~" + dt.Rows[0]["Msg"].ToString();
                    else
                        oRule.sResponse = clsMsgRule.sInValid + "~" + dt.Rows[0]["Result"].ToString();
                }
                else
                {
                    oRule.sResponse = clsMsgRule.sInValid + "~No value return from database";
                }
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
            return oRule.sResponse;
        }
        //End
        #endregion

        #region Dispatch

        internal string GetDispatchPickListNo()
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("Exec [Prc_GetDispatchData] '" + EnumDbType.SELECT + "'");
                oDb.Connect();
                DataTable dt = oDb.GetDataTable(_SbQry.ToString());
                if (dt.Rows.Count > 0)
                {
                    oRule.sResponse = clsMsgRule.sValid;
                    foreach (DataRow row in dt.Rows)
                    {
                        //Column separator $
                        oRule.sResponse += "~" + row["PickListNo"].ToString();
                    }
                }
                else
                {
                    oRule.sResponse = clsMsgRule.sInValid + "~There is no pending picklist";
                }
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
            return oRule.sResponse;
        }

        internal string GetDispatchPickListNoData(string PicListNo)
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("Exec [Prc_GetDispatchData] '" + EnumDbType.PICK_DETAILS + "','" + PicListNo + "'");
                oDb.Connect();
                DataTable dt = oDb.GetDataTable(_SbQry.ToString());
                if (dt.Rows.Count > 0)
                {
                    oRule.sResponse = clsMsgRule.sValid + "~";
                    foreach (DataRow row in dt.Rows)
                    {
                        //Column separator $
                        oRule.sResponse += row["BackNo"].ToString() + "$" + row["PickQty"].ToString() + "$" + row["DispatchQty"].ToString() + "#";
                    }
                    //Row Separator #
                    oRule.sResponse = oRule.sResponse.TrimEnd('#');
                }
                else
                {
                    oRule.sResponse = clsMsgRule.sInValid + "~No data found for picklist no. " + PicListNo;
                }
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
            return oRule.sResponse;
        }

        internal string DispatchSaveBin(string PicListNo, string Binbarcode, string UserId)
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("Exec [Prc_SaveDispatchData] '" + PicListNo + "','" + Binbarcode + "'");
                _SbQry.AppendLine("," + Convert.ToInt32(EnumProductionStatus.DISPATCH) + ",'" + UserId + "'");
                oDb.Connect();
                DataTable dt = oDb.GetDataTable(_SbQry.ToString());
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Result"].ToString() == "Y")
                        oRule.sResponse = clsMsgRule.sValid + "~" + dt.Rows[0]["Msg"].ToString();
                    else
                        oRule.sResponse = clsMsgRule.sInValid + "~" + dt.Rows[0]["Result"].ToString();
                }
                else
                {
                    oRule.sResponse = clsMsgRule.sInValid + "~No value return from database";
                }
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
            return oRule.sResponse;
        }

        #endregion

        #region CheckBinBarcode
        internal string CheckBinBarcode(string Binbarcode)
        {
            clsDB oDb = new clsDB();
            try
            {
                _SbQry = new StringBuilder("Exec [Prc_CheckBinBarcode] '" + Binbarcode + "'");
                oDb.Connect();
                DataTable dt = oDb.GetDataTable(_SbQry.ToString());
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Result"].ToString() == "Y")
                        oRule.sResponse = clsMsgRule.sValid;
                    else
                        oRule.sResponse = clsMsgRule.sInValid + "~" + dt.Rows[0]["Result"].ToString();
                }
                else
                {
                    oRule.sResponse = clsMsgRule.sInValid + "~No value return from database";
                }
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
            return oRule.sResponse;
        }
        #endregion

    }
}
