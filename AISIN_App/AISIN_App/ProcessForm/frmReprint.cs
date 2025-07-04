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

namespace AISIN_App
{
    public partial class frmReprint : Form
    {
        #region Variables

        Dal oDal;
        RePrint oRePrint;

        #endregion

        #region Form Methods

        public frmReprint()
        {
            try
            {
                InitializeComponent();
                oRePrint = new RePrint();
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
                txtPartBarcode.Focus();
                GetLineNo();
            }
            catch (Exception ex)
            {
                ClsGlobal.SetErrorMessage(ex.Message, lblMessage);
            }
        }

        #endregion

        #region Button Event
        private void btnGet_Click(object sender, EventArgs e)
        {
            try
            {
                ClsGlobal.ClearMessage(lblMessage);
                if (cmbLineNo.SelectedIndex < 0)
                {
                    ClsGlobal.SetInfoMessage("Select line no.", lblMessage);
                    cmbLineNo.Focus();
                    return;
                }
                if (dtpFromDate.Value > DateTime.Parse(DateTime.Now.ToString("dd-MMM-yyyy")))
                {
                    ClsGlobal.SetInfoMessage("From date can not be from future", lblMessage);
                    dtpFromDate.Focus();
                    return;
                }
                if (dtpToDate.Value > DateTime.Parse(DateTime.Now.ToString("dd-MMM-yyyy")))
                {
                    ClsGlobal.SetInfoMessage("To date can not be from future", lblMessage);
                    dtpToDate.Focus();
                    return;
                }
                if (dtpFromDate.Value > dtpToDate.Value)
                {
                    ClsGlobal.SetInfoMessage("From date can not be greater than to date", lblMessage);
                    dtpFromDate.Focus();
                    return;
                }
                //oRePrint.LineNo = cmbLineNo.SelectedItem.ToString(); Comment by  Akhilesh on 18-01-2021 to get last 2 char from Line No.
                oRePrint.LineNo = cmbLineNo.SelectedItem.ToString().Substring((cmbLineNo.SelectedItem.ToString()).Length - 2);
                oRePrint.FromDate = dtpFromDate.Text;
                oRePrint.ToDate = dtpToDate.Text;
                oRePrint.KanBanBarcode = txtKanBan.Text.Trim();
                oRePrint.DbType = EnumDbType.KANBAN;

                dgv.DataSource = oDal.ManageRePrint(oRePrint);

                if (dgv.Rows.Count == 0)
                    ClsGlobal.SetInfoMessage("Data not found", lblMessage);

            }
            catch (Exception ex)
            {
                ClsGlobal.SetErrorMessage(ex.Message, lblMessage);
            }
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                ClsGlobal.ClearMessage(lblMessage);

                if (txtBinBarcode.Text.Trim() != "")
                {
                    string Msg = ClsGlobal.PrintBinLabel(txtBinBarcode.Text.Trim());
                    if (Msg == "OK")
                    {
                        //Save Reprint History
                        SavePrintHistory(txtBinBarcode.Text.Trim());
                        btnReset_Click(sender, e);
                        ClsGlobal.SetSuccessMessage("Print successfully!!", lblMessage);
                    }
                    else
                        ClsGlobal.SetInfoMessage("Error in printing - " + Msg, lblMessage);
                }
                else
                {
                    ClsGlobal.SetInfoMessage("BinBarcode details not found", lblMessage);
                }
            }
            catch (Exception ex)
            {
                ClsGlobal.SetErrorMessage(ex.Message, lblMessage);
            }
        }
        private void btnPrintWithOutPart_Click(object sender, EventArgs e)
        {
            try
            {
                ClsGlobal.ClearMessage(lblMessage);

                if (dgv.Rows.Count > 0)
                {
                    if (dgv.SelectedRows.Count > 0)
                    {
                        string Binbarcode = dgv.SelectedRows[0].Cells["BinBarcode"].Value.ToString().Trim();
                        string Msg = ClsGlobal.PrintBinLabel(Binbarcode);
                        if (Msg == "OK")
                        {
                            //Save Reprint History
                            SavePrintHistory(Binbarcode);
                            btnReset_Click(sender, e);
                            ClsGlobal.SetSuccessMessage("Print successfully!!", lblMessage);
                        }
                        else
                            ClsGlobal.SetInfoMessage("Error in printing - " + Msg, lblMessage);
                    }
                    else
                        ClsGlobal.SetInfoMessage("Select bin barcode", lblMessage);
                }
                else
                {
                    ClsGlobal.SetInfoMessage("Data not found to reprint", lblMessage);
                }
            }
            catch (Exception ex)
            {
                ClsGlobal.SetErrorMessage(ex.Message, lblMessage);
            }
        }
        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                Clear();
            }
            catch (Exception ex)
            {
                ClsGlobal.SetErrorMessage(ex.Message, lblMessage);
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Label Event
        private void lblMessage_DoubleClick(object sender, EventArgs e)
        {
            ClsGlobal.ShowInfoMessageBox(lblMessage.Text);
        }

        #endregion

        #region TextBox Events

        private void txtPartBarcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 13)
                {
                    ClsGlobal.ClearMessage(lblMessage);
                    oRePrint.DbType = EnumDbType.PART;
                    oRePrint.PartBarcode = txtPartBarcode.Text.Trim();
                    DataTable dt = oDal.ManageRePrint(oRePrint);
                    if (dt.Rows.Count > 0)
                    {
                        txtBinBarcode.Text = dt.Rows[0]["BinBarcode"].ToString();
                    }
                    else
                    {
                        ClsGlobal.SetInfoMessage($"Part {txtPartBarcode.Text.Trim()} details not found", lblMessage);
                        txtPartBarcode.Text = "";
                        txtPartBarcode.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                ClsGlobal.SetErrorMessage(ex.Message, lblMessage);
            }
        }

        #endregion

        #region Tab Events
        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            try
            {
                Clear();
            }
            catch (Exception ex)
            {
                ClsGlobal.SetErrorMessage(ex.Message, lblMessage);
            }
        }



        #endregion

        #region Methods

        private void GetLineNo()
        {
            try
            {
                DataTable dt = oDal.ManageLine(new Line { DbType = EnumDbType.SELECT });
                if (dt.Rows.Count > 0)
                {

                    DataTable dtLine = dt.AsEnumerable().GroupBy(row => row.Field<string>("Line_No")).Select(group => group.First()).CopyToDataTable(); //Added by Akhilesh on 18-01-2021 for filter Line No
                    for (int i = 0; i < dtLine.Rows.Count; i++)
                        cmbLineNo.Items.Add(dtLine.Rows[i]["Line_No"].ToString());
                   
                    cmbLineNo.SelectedIndex = -1;
                }
                else
                    ClsGlobal.SetInfoMessage("Line details not found", lblMessage);
            }
            catch (Exception ex)
            {
                ClsGlobal.ShowErrorMessageBox(ex.Message);
            }
        }

        private void SavePrintHistory(string Binbarcode)
        {
            try
            {
                oRePrint.BinBarcode = Binbarcode;
                oRePrint.CreatedBy = ClsGlobal.UserId;
                oRePrint.DbType = EnumDbType.INSERT;

                oDal.ManageRePrint(oRePrint);
            }
            catch (Exception ex)
            {
                ClsGlobal.ShowErrorMessageBox(ex.Message);
            }
        }

        private void Clear()
        {
            try
            {
                ClsGlobal.ClearMessage(lblMessage);
                cmbLineNo.SelectedIndex = -1;
                txtBinBarcode.Text = "";
                txtKanBan.Text = "";
                txtPartBarcode.Text = "";
                dgv.DataSource = null;
                dtpFromDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                dtpToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                ClsGlobal.ShowErrorMessageBox(ex.Message);
            }
        }

        #endregion
    }
}
