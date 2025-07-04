using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Threading;
using System.Diagnostics.Eventing.Reader;
using SatoLib;

namespace AISIN_App
{
    public partial class frmProduction : Form
    {
        #region Variables

        //Hold Reference for auto generated Production Id
        string _ProductionId = "";
        int _ScanQty = 0;
        bool IsBinBarcode = false;
        string Printstatus = string.Empty;
        string SFractionPrintstatus = string.Empty;
        string SServicePrintstatus = string.Empty;
        SatoLib.SatoLogger _obj = new SatoLib.SatoLogger();

        Dal oDal;
        Production oProd;

        #endregion

        #region Form Methods

        public frmProduction()
        {
            try
            {
                InitializeComponent();
                oProd = new Production();
                oDal = new Dal();
            }
            catch (Exception ex)
            {
                lblMessage.Text = "ERROR: " + ex.Message;
            }
        }

        private void frmModelMaster_Load(object sender, EventArgs e)
        {
            try
            {
                ClsGlobal.ClearMessage(lblMessage);
                lblLineNo.Text = "Line No : " + ClsGlobal.LineNo;
                lblBackNo.Text = "";
                txtKanbanBarcode.Focus();
                GetPendingKanBanData();

                _obj.ChangeInterval = SatoLib.SatoLogger.ChangeIntervals.ciHourly;
                _obj.EnableLogFiles = true;
                _obj.LogDays = 15;
                _obj.LogFilesExt = "log";
                _obj.LogFilesPath = Application.StartupPath;
                _obj.LogFilesPrefix = "KanbanBarcode";
                _obj.StartLogging();
            }
            catch (Exception ex)
            {
                lblMessage.Text = "ERROR: " + ex.Message;
            }
        }

        #endregion

        #region Button Event
        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                ClsGlobal.ClearMessage(lblMessage);
                if (Convert.ToInt32(lblScanQty.Text) == 0)
                {
                    txtPartBarcode.Focus();
                    return;
                }
                if (txtReKanbanBarcode.Text.Trim() == "")
                {
                    ClsGlobal.SetInfoMessage("Re Scan Kanban/Bin Barcode!!!", lblMessage);
                    txtReKanbanBarcode.Text = "";
                    txtReKanbanBarcode.Focus();
                    SFractionPrintstatus = "F";
                    return;
                }
                DialogResult dialogResult = MessageBox.Show("Fraction", "Do you want to save?" + txtPartBarcode.Text.Trim().ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.No)
                {
                    return;
                }

                if (txtKanbanBarcode.Text.Trim() != txtReKanbanBarcode.Text.Trim())
                {
                    ClsGlobal.SetInfoMessage("Invalid Kanban Barcode!!!", lblMessage);
                    txtReKanbanBarcode.Text = "";
                    txtReKanbanBarcode.Focus();
                }
                else
                {
                    DataTable dt;
                    oProd.BackNo = lblBackNo.Text.Trim();
                    oProd.CreatedBy = ClsGlobal.UserId;
                    oProd.KanBanBarcode = txtKanbanBarcode.Text.Trim();
                    oProd.LineNo = ClsGlobal.LineNo;
                    oProd.PartBarcode = "";
                    oProd.ProductionId = _ProductionId;
                    oProd.StandardBinQty = int.Parse(lblBinQty.Text.Trim());
                    oProd.DbType = EnumDbType.PRINTFRACTIONPART;
                    dt = oDal.SaveProductionData(oProd);
                    if (dt.Rows[0]["Result"].ToString() == "P")
                    {
                        SFractionPrintstatus = "";
                        ClearAll();
                        ClsGlobal.PrintBinLabel(dt.Rows[0]["Msg"].ToString());
                        ClsGlobal.SetSuccessMessage("(" + dt.Rows[0]["Msg"].ToString() + ") Print Successfully", lblMessage);
                    }
                    else
                    {
                        ClsGlobal.SetInfoMessage(dt.Rows[0]["Msg"].ToString(), lblMessage);
                    }
                    pnlMain.Enabled = false;
                    Thread.Sleep(ClsGlobal.ProductionDisableTimeInSeconds * 1000);

                    pnlMain.Enabled = true;
                    txtPartBarcode.Text = "";
                    txtKanbanBarcode.Focus();
                    txtReKanbanBarcode.Text = "";
                }
            }
            catch (Exception ex)
            { ClsGlobal.SetErrorMessage(ex.Message, lblMessage); pnlMain.Enabled = true; }

            // try
            //{
            //ClsGlobal.ClearMessage(lblMessage);
            //if (ValidateInput())
            //{
            //    SetNgQty();

            //    oProd.ProductionPlanId = _ProductionPlanId;
            //    oProd.ModelNo = txtModelNo.Text.Trim();
            //    _DTB = oDal.GetModelDTB(oProd.ModelNo);
            //    oProd.ShiftName = _Shift;
            //    oProd.ChargeNo = Convert.ToInt32(txtChargeNo.Text.Trim());
            //    oProd.PalletNo = Convert.ToInt32(txtPalletNo.Text.Trim());
            //    oProd.LineNo = ClsGlobal.LineNo;
            //    oProd.LotNo = dtpDate.Value.ToString("ddMMyy") + ClsGlobal.LineNo + txtChargeNo.Text.Trim().PadLeft(2, '0') + txtPalletNo.Text.Trim().PadLeft(2, '0');
            //    oProd.LotNoDate = dtpDate.Text;
            //    if (oProd.LotNo.Length != 11)
            //    {
            //        ClsGlobal.SetInfoMessage("Lot No " + oProd.LotNo + " length should be 11, please check", lblMessage);
            //        return;
            //    }
            //    string Operator = "";
            //    for (int i = 0; i < chkListOperator.CheckedItems.Count; i++)
            //    {
            //        Operator += chkListOperator.CheckedItems[i].ToString() + ",";
            //    }
            //    Operator = Operator.TrimEnd(',');
            //    oProd.Operators = Operator;
            //    oProd.Leaders = ClsGlobal.UserId;
            //    oProd.TotalQty = int.Parse(txtTotalQty.Text.Trim());
            //    oProd.OkQty = int.Parse(txtOkQty.Text.Trim());
            //    oProd.NgQty = int.Parse(lblNgQty.Text.Trim());
            //    oProd.TrolleyCard = oProd.ModelNo + "-" + oProd.LotNo;
            //    oProd.Status = Convert.ToInt32(EnumCuttingStatus.Cutting);
            //    oProd.CreatedBy = ClsGlobal.UserId;
            //    oProd.IsMixedTrolley = chkMixedTrolley.Checked;

            //    int NewPendingQty = Convert.ToInt32(lblPendingQty.Text) - Convert.ToInt32(txtOkQty.Text);
            //    //If Ng Qty then open defect screen
            //    if (Convert.ToInt32(lblNgQty.Text) > 0)
            //    {
            //        ClsGlobal.CuttingDefectAutoClose = false;
            //        //frmCuttingDefect oFrm = new frmCuttingDefect(oProd, NewPendingQty, _ListCutting, _DTB);
            //        //oFrm.Show();
            //        //oFrm.FormClosing += OFrm_FormClosing; ;
            //        this.Hide();
            //    }
            //    else
            //    {
            //        //If not mixed trolly then save data
            //        if (chkMixedTrolley.Checked == false)
            //        {
            //            string ReturnMsg = oDal.SaveCuttingData(oProd).Rows[0]["Result"].ToString();
            //            if (ReturnMsg.ToUpper() == "Y")
            //            {
            //                string Msg = PrintLabel(oProd.LotNo, oProd.OkQty.ToString(), oProd.ModelNo, oProd.TrolleyCard);

            //                string ShowMsg = "";
            //                if (Msg == "OK")
            //                {
            //                    if (NewPendingQty == 0)
            //                        ShowMsg = "Print successfully, data saved successfully, Production complete for this model";
            //                    else
            //                        ShowMsg = "Print successfully, data saved successfully";
            //                    ClsGlobal.ShowConfirmMessageBox(ShowMsg);
            //                }
            //                else
            //                {
            //                    if (NewPendingQty == 0)
            //                        ShowMsg = "Data saved successfully, printing error message - " + Msg + " , Production complete for this model";
            //                    else
            //                        ShowMsg = "Data saved successfully, printing error message - " + Msg;
            //                    ClsGlobal.ShowConfirmMessageBox(ShowMsg);
            //                }

            //                btnReset_Click(sender, e);
            //            }
            //            else
            //                ClsGlobal.SetInfoMessage("Data could not be saved, " + ReturnMsg, lblMessage);
            //        }
            //        else //If mixed trolley then no need to save just hold in the list until user does not print trolley card
            //            AddMixedTrolleyData(NewPendingQty);
            //    }
            //}
            //}
            //catch (Exception ex)
            //{
            //    if (ex.Message.Contains("Violation of PRIMARY KEY"))
            //    {
            //        ClsGlobal.SetErrorMessage("Lot No already exist,please check!!", lblMessage);
            //    }
            //    else
            //    {
            //        ClsGlobal.SetErrorMessage(ex.Message, lblMessage);
            //    }
            //}
        }



        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                ClearAll();
            }
            catch (Exception ex)
            {
                ClsGlobal.SetErrorMessage(ex.Message, lblMessage);
            }
        }

        #endregion

        #region Label Event
        private void lblMessage_DoubleClick(object sender, EventArgs e)
        {
            ClsGlobal.ShowInfoMessageBox(lblMessage.Text);
        }

        #endregion

        #region TextBox Events
        private void txtKanbanBarcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                ClsGlobal.ClearMessage(lblMessage);
                Clear();
                if (e.KeyChar == 13)
                {
                    _obj.LogMessage(EventNotice.EventTypes.evtInfo, "ScanKanbanBarcode: " + txtKanbanBarcode.Text.TrimStart().TrimEnd().Trim(), "");

                    //Check Barcode is Kanban Or BinBarcode(Length 18 ddMMyyhhMMss01PART)
                    string BarcodeData = txtKanbanBarcode.Text.TrimStart().TrimEnd().Trim();
                    // txtKanbanBarcode.Text = BarcodeData;
                    oProd.IsBinBarcode = BarcodeData.Length == 18 ? true : false;
                    //If bin barcode then fetch data details
                    if (oProd.IsBinBarcode)
                    {
                        GetBinBarcodeData(BarcodeData);
                    }
                    else // not bin barcode means kanban barcode so fetch other details
                    {
                        //Export & Domestic barcode will have same data format
                        /*
                         * AIGSYSGL292   LJ10  LJAS0016   LJ        411340-17411          SE1000   LJ       XXXXXXXXX  0       Y390 0000008000000000003530                                0                              0000008                  PCS
                         */
                        //string[] sArrKanBan = BarcodeData.Split(' '); //Split with Space
                        if (BarcodeData.Length > 0)
                        {
                            var BackNo = "";
                            DataTable dtBackNo = oDal.GetBackNoUsingFucntion(BarcodeData);
                            if (dtBackNo.Rows.Count > 0)
                            {
                                BackNo = dtBackNo.Rows[0][0].ToString();
                                if (BackNo == "ZZZZ")
                                {

                                    ClsGlobal.SetInfoMessage("Invalid Kanban Barcode or Back No Not Found!!!", lblMessage);
                                    txtKanbanBarcode.Text = string.Empty;
                                    txtKanbanBarcode.Clear();
                                    txtKanbanBarcode.Focus();
                                    return;
                                }
                            }

                            else
                            {
                                ClsGlobal.SetInfoMessage("Invalid Kanban Barcode ", lblMessage);
                                txtKanbanBarcode.Text = string.Empty;
                                txtKanbanBarcode.Clear();
                                txtKanbanBarcode.Focus();
                                return;
                            }
                            //Commented by dipak pathak 03-01-2022 made new fucntion to get back no

                            //BackNo = sArrKanBan[45].Trim();
                            //if (BackNo.Length != 4)
                            //{
                            //    BackNo = sArrKanBan[44].Trim();
                            //}
                            //if (BarcodeData.Length == 218) // and added by dipak 14 - 01 - 21 when kanban mismatch
                            //{
                            //    BackNo = sArrKanBan[45].Trim();
                            //}
                            //else
                            //{
                            //    BackNo = sArrKanBan[44].Trim();
                            //}
                            //var BackNo = sArrKanBan[45].Trim(); //Commented by dipak 14-01-21 when kanban mismatch
                            if (BackNo.Length == 4)
                            {
                                DataTable dtPart = oDal.ManagePart(new Part { DbType = EnumDbType.SELECTBYID, BackNo = BackNo });
                                if (dtPart.Rows.Count > 0)
                                {
                                    lblBinQty.Text = dtPart.Rows[0]["StandardBinQty"].ToString();
                                    _ScanQty = 0;
                                    lblScanQty.Text = _ScanQty.ToString();
                                    lblBackNo.Text = BackNo;
                                    txtPartBarcode.Text = "";
                                    txtPartBarcode.Focus();

                                }
                                else
                                {
                                    ClsGlobal.SetInfoMessage("BackNo " + BackNo + "  details not found", lblMessage);
                                    txtKanbanBarcode.Text = string.Empty;
                                    txtKanbanBarcode.Clear();
                                    txtKanbanBarcode.Focus();
                                }
                            }
                            else
                            {
                                ClsGlobal.SetInfoMessage("Invalid backNo in kanban barcode " + BackNo, lblMessage);
                                txtKanbanBarcode.Text = string.Empty;
                                txtKanbanBarcode.Clear();
                                txtKanbanBarcode.Focus();
                            }
                        }
                        else
                        {
                            ClsGlobal.SetInfoMessage("Invalid Kanban Barcode", lblMessage);
                            txtKanbanBarcode.Text = string.Empty;
                            txtKanbanBarcode.Clear();
                            txtKanbanBarcode.Focus();

                        }
                    }
                }
            }
            catch (Exception ex)
            { ClsGlobal.SetErrorMessage(ex.Message, lblMessage); pnlMain.Enabled = true; }
        }
        private void txtPartBarcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                ClsGlobal.ClearMessage(lblMessage);
                if (e.KeyChar == 13)
                {
                    string PartBarcode = txtPartBarcode.Text.Trim();
                    if (PartBarcode.Length >= 4)
                    {
                        //Part barcode belong to same back no
                        // string PartBackNo = PartBarcode.Substring(PartBarcode.Length - 5); Changed by dipak 30-12-20
                        string PartBackNo = PartBarcode.Substring(PartBarcode.Length - 4);
                        DataTable dt;
                        if (lblBackNo.Text.Trim() == PartBackNo)
                        {
                            oProd.BackNo = lblBackNo.Text.Trim();
                            oProd.CreatedBy = ClsGlobal.UserId;
                            oProd.KanBanBarcode = txtKanbanBarcode.Text.Trim();
                            oProd.LineNo = ClsGlobal.LineNo;
                            oProd.PartBarcode = PartBarcode;
                            oProd.ProductionId = _ProductionId;
                            oProd.StandardBinQty = int.Parse(lblBinQty.Text.Trim());
                            //Production id is not generated, means this is the first part for kanban
                            if (_ProductionId == "")
                            {
                                oProd.ProductionId = "0"; ///Added by dipak 30-12-20 
                                oProd.DbType = EnumDbType.KANBAN;
                                dt = oDal.SaveProductionData(oProd);
                                if (dt.Rows[0]["Result"].ToString() == "Y")
                                {
                                    _ProductionId = dt.Rows[0]["Msg"].ToString();
                                    // _ScanQty++;//commented by dipak 30-12-20
                                    GetPendingKanBanData(); //added by dipak 30-12-20
                                }
                                else if (dt.Rows[0]["Result"].ToString() == "A")
                                {
                                    GetPendingKanBanData(); //added by dipak 30-12-20
                                    ClsGlobal.SetInfoMessage(dt.Rows[0]["Msg"].ToString(), lblMessage); //Added by dipak 30-12-20 

                                }
                                else
                                {
                                    ClsGlobal.SetInfoMessage(dt.Rows[0]["Msg"].ToString(), lblMessage); //Added by dipak 30-12-20 
                                }
                            }
                            else ///Else entry added by dipak 30-12-20
                            {
                                oProd.DbType = EnumDbType.PART;
                                dt = oDal.SaveProductionData(oProd);
                                if (dt.Rows[0]["Result"].ToString() == "Y")
                                {
                                    GetPendingKanBanData(); //added by dipak 30-12-20
                                }
                                else if (dt.Rows[0]["Result"].ToString() == "P") // When Barcode generated/read for print //added by dipak 30-12-20
                                {
                                    GetPendingKanBanDataData("P", "");
                                    //GetPendingKanBanData(); //added by dipak 30-12-20
                                    Printstatus = "P";

                                    ClsGlobal.PrintStatus = "P";
                                    ClsGlobal.Barcode = dt.Rows[0]["Msg"].ToString();
                                    txtReKanbanBarcode.Text = "";
                                    txtReKanbanBarcode.Focus();
                                    //ClearAll();
                                    // ClsGlobal.PrintBinLabel(dt.Rows[0]["Msg"].ToString());
                                    //ClsGlobal.SetSuccessMessage("(" + dt.Rows[0]["Msg"].ToString() + ") Print Successfully", lblMessage);
                                }
                                else if (dt.Rows[0]["Result"].ToString() == "E") // When Barcode generated/read for print existing bin //added by Akhilesh on 31-12-2020
                                {

                                    GetPendingKanBanDataData("E", dt.Rows[0]["Msg"].ToString());
                                    ClsGlobal.PrintStatus = "E";
                                    ClsGlobal.Barcode = dt.Rows[0]["Msg"].ToString();
                                    txtReKanbanBarcode.Text = "";
                                    txtReKanbanBarcode.Focus();



                                    //ClsGlobal.PrintStatus = "E";
                                    //ClearAll();
                                    ////ClsGlobal.PrintBinLabel(dt.Rows[0]["Msg"].ToString());
                                    //ClsGlobal.SetSuccessMessage("(" + dt.Rows[0]["Msg"].ToString() + ") Completed Successfully", lblMessage);
                                    //txtKanbanBarcode.Focus();
                                }
                                else if (dt.Rows[0]["Result"].ToString() == "D") //Added by Akhilesh on 31-12-2020 for check part rejection
                                {
                                    DialogResult dialogResult = MessageBox.Show("Rejection", "Do you want to reject Part ?" + txtPartBarcode.Text.Trim().ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                    if (dialogResult == DialogResult.Yes)
                                    {
                                        dt = oDal.SavePartRejectionData(oProd);

                                        if (dt.Rows[0]["Result"].ToString() == "Y")
                                        {
                                            ClsGlobal.SetInfoMessage(dt.Rows[0]["Msg"].ToString(), lblMessage);
                                            GetPendingKanBanData();
                                        }

                                    }
                                    else
                                    {
                                        ClsGlobal.SetInfoMessage(dt.Rows[0]["Msg"].ToString(), lblMessage); //Added by dipak 30-12-20 
                                    }
                                }
                                else
                                {
                                    ClsGlobal.SetInfoMessage(dt.Rows[0]["Msg"].ToString(), lblMessage); //Added by dipak 30-12-20 
                                }

                            }
                            pnlMain.Enabled = false;
                            Thread.Sleep(ClsGlobal.ProductionDisableTimeInSeconds * 1000);

                            pnlMain.Enabled = true;
                            txtPartBarcode.Text = "";
                            //Added by  Akhilesh on 06-01-2021 to focus after printing
                            if (dt.Rows[0]["Result"].ToString() == "P")
                            {
                                txtReKanbanBarcode.Focus();
                            }
                            else if (dt.Rows[0]["Result"].ToString() == "E")
                            {
                                txtReKanbanBarcode.Focus();
                            }

                            else
                            {
                                txtPartBarcode.Focus();
                            }
                        }
                        else
                        {
                            ClsGlobal.SetInfoMessage("Wrong back no " + PartBackNo + " in part barcode " + PartBarcode, lblMessage);
                            txtPartBarcode.Text = "";
                            txtPartBarcode.Focus();
                        }
                    }
                    else
                    {
                        ClsGlobal.SetInfoMessage("Invalid Part Barcode " + PartBarcode, lblMessage);
                        txtPartBarcode.Text = "";
                        txtPartBarcode.Focus();
                    }
                }
            }
            catch (Exception ex)
            { ClsGlobal.SetErrorMessage(ex.Message, lblMessage); pnlMain.Enabled = true; }
        }

        #endregion

        #region Methods
        private void ClearAll()
        {
            try
            {
                txtKanbanBarcode.Text = "";
                Clear();
                txtKanbanBarcode.Focus();
            }
            catch (Exception ex)
            {
                ClsGlobal.SetErrorMessage(ex.Message, lblMessage);
            }
        }
        private void Clear()
        {
            try
            {
                ClsGlobal.ClearMessage(lblMessage);
                _ProductionId = "";
                txtPartBarcode.Text = "";
                lblBackNo.Text = "";
                lbPartBarcode.Items.Clear();
                lblBinQty.Text = "0";
                lblScanQty.Text = "0";
                _ScanQty = 0;
            }
            catch (Exception ex)
            {
                ClsGlobal.SetErrorMessage(ex.Message, lblMessage);
            }
        }
        private void SetNgQty()
        {
            try
            {
                //ClsGlobal.ClearMessage(lblMessage);
                //int TotalQty = 0;
                //if (txtOkQty.Text.Trim() == "")
                //{
                //    TotalQty = string.IsNullOrEmpty(txtTotalQty.Text) ? 0 : Convert.ToInt32(txtTotalQty.Text);
                //    lblNgQty.Text = (TotalQty - 0).ToString();
                //}
                //if (txtOkQty.Text.Length > 0)
                //{
                //    TotalQty = string.IsNullOrEmpty(txtTotalQty.Text) ? 0 : Convert.ToInt32(txtTotalQty.Text);
                //    if (Convert.ToInt32(txtOkQty.Text) <= TotalQty)
                //    {
                //        lblNgQty.Text = (Convert.ToInt32(txtTotalQty.Text) - Convert.ToInt32(txtOkQty.Text)).ToString();
                //    }
                //    else
                //        ClsGlobal.SetInfoMessage("OK Qty can not be greater than total qty", lblMessage);
                //}
            }
            catch (Exception ex) { throw ex; }
        }
        /*
         * This method will check any pending kanban for printing then fetch data and set the data on load
         */
        private void GetPendingKanBanData()
        {
            try
            {
                ClearAll();
                oProd.LineNo = ClsGlobal.LineNo;
                // Added by Akhilesh on 31-12-2020
                if (oProd.KanBanBarcode != null)
                {
                    oProd.IsBinBarcode = oProd.KanBanBarcode.Length == 18 ? true : false; // Added by Akhilesh on 31-12-2020
                } //End
                else
                {
                    oProd.IsBinBarcode = false;
                }
                DataSet ds = oDal.GetPendingKanBanData(oProd);
                //If any pending kanban available
                if (ds.Tables[0].Rows.Count > 0)
                {
                    _ProductionId = ds.Tables[0].Rows[0]["ProductionId"].ToString();
                    txtKanbanBarcode.Text = ds.Tables[0].Rows[0]["KanBanBarcode"].ToString();
                    lblBackNo.Text = ds.Tables[0].Rows[0]["BackNo"].ToString();
                    lblBinQty.Text = ds.Tables[0].Rows[0]["StandardBinQty"].ToString();
                    _ScanQty = int.Parse(ds.Tables[0].Rows[0]["ScanQty"].ToString());
                    lblScanQty.Text = _ScanQty.ToString();
                    //Get Part Barcode details
                    if (oProd.IsBinBarcode == false)
                    {
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                            lbPartBarcode.Items.Add(ds.Tables[1].Rows[i]["PartBarcode"].ToString());
                    }
                    else
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            lbPartBarcode.Items.Add(ds.Tables[0].Rows[i]["PartBarcode"].ToString());
                            txtKanbanBarcode.Text = ds.Tables[0].Rows[i]["BinBarcode"].ToString();
                        }
                    }

                    txtPartBarcode.Focus();
                }
                else { txtKanbanBarcode.Focus(); }
            }
            catch (Exception ex)
            {
                ClsGlobal.SetErrorMessage(ex.Message, lblMessage);
            }
        }


        private void GetPendingKanBanDataData(string sType, string Barcode)
        {
            try
            {
                ClearAll();
                oProd.LineNo = ClsGlobal.LineNo;
                // Added by Akhilesh on 31-12-2020
                if (oProd.KanBanBarcode != null)
                {
                    oProd.IsBinBarcode = oProd.KanBanBarcode.Length == 18 ? false : false; // Added by Akhilesh on 31-12-2020
                } //End
                else
                {
                    oProd.IsBinBarcode = false;
                }
                DataSet ds = oDal.GetPendingKanBanData_Data(oProd);
                //If any pending kanban available
                if (ds.Tables[0].Rows.Count > 0)
                {
                    _ProductionId = ds.Tables[0].Rows[0]["ProductionId"].ToString();
                    if (sType == "P")
                    {
                        txtKanbanBarcode.Text = ds.Tables[0].Rows[0]["KanBanBarcode"].ToString();
                    }
                    else
                    {
                        txtKanbanBarcode.Text = Barcode;
                    }
                    lblBackNo.Text = ds.Tables[0].Rows[0]["BackNo"].ToString();
                    lblBinQty.Text = ds.Tables[0].Rows[0]["StandardBinQty"].ToString();
                    _ScanQty = int.Parse(ds.Tables[0].Rows[0]["ScanQty"].ToString());
                    lblScanQty.Text = _ScanQty.ToString();
                    //Get Part Barcode details
                    if (oProd.IsBinBarcode == false || oProd.IsBinBarcode == true)
                    {
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                            lbPartBarcode.Items.Add(ds.Tables[1].Rows[i]["PartBarcode"].ToString());
                    }
                    else
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            lbPartBarcode.Items.Add(ds.Tables[0].Rows[i]["PartBarcode"].ToString());
                            txtKanbanBarcode.Text = ds.Tables[0].Rows[i]["BinBarcode"].ToString();
                        }
                    }

                    txtReKanbanBarcode.Focus();
                    ClsGlobal.SetInfoMessage("Re Scan Kanban/Bin Barcode!!", lblMessage);
                }
                else { txtKanbanBarcode.Focus(); }
            }
            catch (Exception ex)
            {
                ClsGlobal.SetErrorMessage(ex.Message, lblMessage);
            }
        }

        private void GetBinBarcodeData(string BarcodeData)
        {
            try
            {
                //string LineNo = BarcodeData.Substring(14, 2); //comment by Akhilesh on 31-12-2020 -Line wrong position
                string LineNo = BarcodeData.Substring(12, 2);
                //If BinBarcode line no does not match to app line no.
                if (LineNo.PadLeft(2, '0') != ClsGlobal.LineNo.PadLeft(2, '0'))
                {
                    ClsGlobal.SetInfoMessage("BinBarcode " + BarcodeData + " does not belong to this line", lblMessage);
                    txtKanbanBarcode.Text = "";
                    txtKanbanBarcode.Focus();
                }
                else //BinBarcode belong to this line
                {
                    oProd.LineNo = ClsGlobal.LineNo;
                    oProd.IsBinBarcode = true;
                    oProd.BinBarcode = BarcodeData; //Added by Akhilesh on 31-12-2020
                    DataTable dt = oDal.GetPendingKanBanData(oProd).Tables[0];
                    if (dt.Rows[0]["ProductionId"].ToString() == "N") //Added by Akhilesh on 31-12-2020
                    {
                        ClsGlobal.SetInfoMessage(dt.Rows[0]["PartBarcode"].ToString(), lblMessage);
                        txtKanbanBarcode.Text = "";
                        txtKanbanBarcode.Focus();
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        _ProductionId = dt.Rows[0]["ProductionId"].ToString();
                        lblBackNo.Text = dt.Rows[0]["BackNo"].ToString();
                        lblBinQty.Text = dt.Rows[0]["StandardBinQty"].ToString();
                        _ScanQty = int.Parse(dt.Rows[0]["ScanQty"].ToString());
                        lblScanQty.Text = _ScanQty.ToString();
                        //Get Part Barcode details

                        for (int i = 0; i < dt.Rows.Count; i++)
                            if (dt.Rows[i]["PartBarcode"].ToString() != "") //Added By Akhilesh on 31-12-2020
                                lbPartBarcode.Items.Add(dt.Rows[i]["PartBarcode"].ToString());

                        txtPartBarcode.Focus();
                        if (_ScanQty > 0 && _ScanQty >= int.Parse(lblBinQty.Text))
                        { ClsGlobal.SetInfoMessage("Bin already completed", lblMessage); }
                    }
                    else
                    {
                        ClsGlobal.SetInfoMessage("Wrong BinBarcode " + BarcodeData, lblMessage);
                        txtKanbanBarcode.Text = "";
                        txtKanbanBarcode.Focus();
                    }
                }
            }
            catch (Exception ex)
            { ClsGlobal.SetErrorMessage(ex.Message, lblMessage); }
        }

        private bool ValidateInput()
        {
            //try
            //{
            //    if (txtModelNo.Text.Trim().Length == 0)
            //    {
            //        ClsGlobal.SetInfoMessage("Model no not found!!", lblMessage);
            //        txtModelNo.Focus();
            //        return false;
            //    }
            //    if (Convert.ToDateTime(dtpDate.Text) > DateTime.Parse(DateTime.Now.ToString("dd-MMM-yyyy")))
            //    {
            //        ClsGlobal.SetInfoMessage("Date can not be future date!!", lblMessage);
            //        dtpDate.Focus();
            //        return false;
            //    }
            //    if (txtChargeNo.Text.Trim().Length == 0 || int.Parse(txtChargeNo.Text.Trim()) == 0)
            //    {
            //        ClsGlobal.SetInfoMessage("Please input charge no!!", lblMessage);
            //        txtChargeNo.Focus();
            //        return false;
            //    }
            //    if (txtPalletNo.Text.Trim().Length == 0 || int.Parse(txtPalletNo.Text.Trim()) == 0)
            //    {
            //        ClsGlobal.SetInfoMessage("Please input pallet no!!", lblMessage);
            //        txtPalletNo.Focus();
            //        return false;
            //    }
            //    if (txtTotalQty.Text.Trim().Length == 0 || int.Parse(txtTotalQty.Text.Trim()) == 0)
            //    {
            //        ClsGlobal.SetInfoMessage("Please input total qty!!", lblMessage);
            //        txtTotalQty.Focus();
            //        return false;
            //    }
            //    if (txtOkQty.Text.Trim().Length == 0 || int.Parse(txtOkQty.Text.Trim()) == 0)
            //    {
            //        ClsGlobal.SetInfoMessage("Please input ok qty!!", lblMessage);
            //        txtOkQty.Focus();
            //        return false;
            //    }
            //    if (chkListOperator.CheckedItems.Count == 0)
            //    {
            //        ClsGlobal.SetInfoMessage("Please select operator!!", lblMessage);
            //        txtOkQty.Focus();
            //        return false;
            //    }
            //    if ((int.Parse(txtOkQty.Text.Trim()) + int.Parse(lblNgQty.Text.Trim())) != int.Parse(txtTotalQty.Text.Trim()))
            //    {
            //        ClsGlobal.SetInfoMessage("(Ok+Ng) Qty shoule be equal to total qty ", lblMessage);
            //        return false;
            //    }
            //    if (int.Parse(txtOkQty.Text.Trim()) > int.Parse(lblPendingQty.Text.Trim()))
            //    {
            //        ClsGlobal.SetInfoMessage("Ok Qty can not be greater than pending qty.", lblMessage);
            //        return false;
            //    }
            //    string Shift = oDal.GetShift().Rows[0]["ShiftName"].ToString();
            //    if (Shift.Trim().ToUpper() != _Shift.Trim().ToUpper())
            //    {
            //        ClsGlobal.ShowInfoMessageBox("Hi, Shift has been changed, please logout the application and login again");
            //        return false;
            //    }
            return true;
            //}
            //catch (Exception ex) { throw ex; }
        }

        #endregion

        private void btnServicePart_Click(object sender, EventArgs e)
        {
            try
            {
                ClsGlobal.ClearMessage(lblMessage);
                if (Convert.ToInt32(lblScanQty.Text) == 0)
                {
                    txtPartBarcode.Focus();
                    return;
                }

                if (txtReKanbanBarcode.Text.Trim() == "")
                {
                    ClsGlobal.SetInfoMessage("Re Scan Kanban/Bin Barcode!!!", lblMessage);
                    txtReKanbanBarcode.Text = "";
                    txtReKanbanBarcode.Focus();
                    SServicePrintstatus = "S";
                    return;
                }
                DialogResult dialogResult = MessageBox.Show("ServicePart", "Do you want to save ?" + txtPartBarcode.Text.Trim().ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.No)
                {
                    return;
                }

                if (txtKanbanBarcode.Text.Trim() != txtReKanbanBarcode.Text.Trim())
                {
                    ClsGlobal.SetInfoMessage("Invalid Kanban Barcode!!!", lblMessage);
                    txtReKanbanBarcode.Text = "";
                    txtReKanbanBarcode.Focus();
                }
                else
                {
                    DataTable dt;
                    oProd.BackNo = lblBackNo.Text.Trim();
                    oProd.CreatedBy = ClsGlobal.UserId;
                    oProd.KanBanBarcode = txtKanbanBarcode.Text.Trim();
                    oProd.LineNo = ClsGlobal.LineNo;
                    oProd.PartBarcode = "";
                    oProd.ProductionId = _ProductionId;
                    oProd.StandardBinQty = int.Parse(lblBinQty.Text.Trim());
                    oProd.DbType = EnumDbType.PRINTSERVICEPART;
                    dt = oDal.SaveProductionData(oProd);
                    if (dt.Rows[0]["Result"].ToString() == "P")
                    {
                        SServicePrintstatus = "";
                        ClearAll();
                        ClsGlobal.PrintBinLabel(dt.Rows[0]["Msg"].ToString());
                        ClsGlobal.SetSuccessMessage("(" + dt.Rows[0]["Msg"].ToString() + ") Print Successfully", lblMessage);
                    }
                    else
                    {
                        ClsGlobal.SetInfoMessage(dt.Rows[0]["Msg"].ToString(), lblMessage);
                    }
                    pnlMain.Enabled = false;
                    Thread.Sleep(ClsGlobal.ProductionDisableTimeInSeconds * 1000);
                    pnlMain.Enabled = true;
                    txtPartBarcode.Text = "";
                    txtKanbanBarcode.Focus();
                    txtReKanbanBarcode.Text = "";
                }
            }
            catch (Exception ex)
            { ClsGlobal.SetErrorMessage(ex.Message, lblMessage); pnlMain.Enabled = true; }
        }

        private void txtPartBarcode_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtReKanbanBarcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {


                if (txtReKanbanBarcode.Text.Trim() == "")
                {
                    _obj.LogMessage(EventNotice.EventTypes.evtInfo, "ReKanbanBarcode: " + txtReKanbanBarcode.Text.TrimStart().TrimEnd().Trim(), "");

                    ClsGlobal.SetInfoMessage("Re Scan Kanban/Bin Barcode!!!", lblMessage);
                    txtReKanbanBarcode.Text = "";
                    txtReKanbanBarcode.Focus();
                    return;
                }

                if (txtKanbanBarcode.Text.Trim() != txtReKanbanBarcode.Text.Trim())
                {
                    _obj.LogMessage(EventNotice.EventTypes.evtInfo,
    $"NotMatched : {Environment.NewLine}Scan Kanban : {txtKanbanBarcode.Text.Trim()}{Environment.NewLine}Rescan Kanban : {txtReKanbanBarcode.Text.Trim()}",
    "");
                    ClsGlobal.SetInfoMessage("Scan Kanban and Rescan Kanban not matched!!!", lblMessage);
                    txtReKanbanBarcode.Text = "";
                    txtReKanbanBarcode.Focus();
                    return;
                }
                else
                {

                    if (SFractionPrintstatus == "F")
                    {
                        btnNext_Click(null, null);
                        return;
                    }
                    if (SServicePrintstatus == "S")
                    {
                        btnServicePart_Click(null, null);
                        return;
                    }

                    //   ClsGlobal.ClearMessage(lblMessage);
                }

                if (ClsGlobal.Barcode != "" && ClsGlobal.Barcode != null)
                {
                    if (txtKanbanBarcode.Text.Trim() == "" && txtReKanbanBarcode.Text.Trim() == "")
                    {
                        _obj.LogMessage(EventNotice.EventTypes.evtInfo,
    $"BlankKanban : {Environment.NewLine}Scan Kanban : {txtKanbanBarcode.Text.Trim()}{Environment.NewLine}Rescan Kanban : {txtReKanbanBarcode.Text.Trim()}",
    "");
                        ClsGlobal.SetInfoMessage("Invalid Kanban Barcode!!!", lblMessage);
                        txtReKanbanBarcode.Text = "";
                        txtReKanbanBarcode.Focus();
                    }
                    else if (txtKanbanBarcode.Text.Trim() != txtReKanbanBarcode.Text.Trim())
                    {
                        _obj.LogMessage(EventNotice.EventTypes.evtInfo,
    $"NotMatched : {Environment.NewLine}Scan Kanban : {txtKanbanBarcode.Text.Trim()}{Environment.NewLine}Rescan Kanban : {txtReKanbanBarcode.Text.Trim()}",
    "");
                        ClsGlobal.SetInfoMessage("Scan Kanban and Rescan Kanban not matched!!!", lblMessage);
                        txtReKanbanBarcode.Text = "";
                        txtReKanbanBarcode.Focus();
                    }
                    else
                    {
                        if (ClsGlobal.Barcode != "")
                        {
                            if (ClsGlobal.PrintStatus == "P")
                            {
                                ClearAll();
                                ClsGlobal.PrintBinLabel(ClsGlobal.Barcode);
                                ClsGlobal.SetSuccessMessage("(" + ClsGlobal.Barcode + ") Print Successfully", lblMessage);
                                ClsGlobal.Barcode = "";
                                txtReKanbanBarcode.Text = "";
                            }
                            else if (ClsGlobal.PrintStatus == "E" && Convert.ToInt32(lblScanQty.Text) > 0 && Convert.ToInt32(lblBinQty.Text) == Convert.ToInt32(lblScanQty.Text))
                            {
                                ClearAll();
                                //ClsGlobal.PrintBinLabel(dt.Rows[0]["Msg"].ToString());
                                ClsGlobal.SetSuccessMessage("(" + ClsGlobal.Barcode + ") Completed Successfully", lblMessage);
                                txtReKanbanBarcode.Text = "";
                                txtKanbanBarcode.Focus();

                            }
                        }

                        else
                        {
                            txtReKanbanBarcode.Focus();
                        }
                    }
                }
                else if (txtReKanbanBarcode.Text != "")
                {
                    //ClsGlobal.ClearMessage(lblMessage);
                    return;
                }
                else
                {
                    ClsGlobal.SetInfoMessage("Need to complete part barcode scanning first!!!", lblMessage);
                    txtReKanbanBarcode.Text = "";
                    txtPartBarcode.Focus();
                }
            }
        }
    }
}
