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
    public partial class frmDirectDispatchReport : Form
    {
        #region Variables

        Dal oDal;
        string sTruckNo = "", sFromDate = "", sToDate = "";

        #endregion

        #region Form Methods

        public frmDirectDispatchReport( string TruckNo,string FromDate,string ToDate)
        {
            try
            {
                InitializeComponent();
                oDal = new Dal();
                sTruckNo = TruckNo;
                sFromDate = FromDate;
                sToDate = ToDate;
            }
            catch (Exception ex)
            {
                ClsGlobal.SetErrorMessage(ex.Message, lblMessage);
            }
        }

        private void frmModelMaster_Load(object sender, EventArgs e)
        {
            try
            {
                ShowWait(false);
                ClsGlobal.ClearMessage(lblMessage);
                GetTruckNo();
                GetDispatchID();
                GetLoadingList();
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
                string sDispatchID = "0";
                string TruckNo = "";
                ClsGlobal.ClearMessage(lblMessage);
                dgv.DataSource = null;
                DataTable dt;

                if (cmbTruckNo.SelectedIndex >= 0)
                {
                    TruckNo = cmbTruckNo.SelectedValue.ToString();
                }

                if (cmbDispatchID.SelectedIndex >= 0)
                {
                    sDispatchID = cmbDispatchID.SelectedValue.ToString();
                }

                dt = oDal.ManageReport(EnumDbType.LOADINGSLIP, dtpFromDate.Text, dtpToDate.Text, "", "", TruckNo, sDispatchID);

                if (dt.Rows.Count > 0)
                {
                    dgv.DataSource = dt;
                    dgv.Columns[0].Width = 110;
                    dgv.Columns[1].Width = 110;
                    dgv.Columns[2].Width = 90;
                    dgv.Columns[3].Width = 135;
                    dgv.Columns[4].Width = 120;
                    lblCount.Text = "Rows Count : " + dgv.Rows.Count.ToString();
                }
                else
                    ClsGlobal.SetInfoMessage("Data not found", lblMessage);


                lblCount.Text = "Rows Count : " + dgv.Rows.Count.ToString();
            }
            catch (Exception ex)
            {
                ClsGlobal.SetErrorMessage(ex.Message, lblMessage);
            }
        }
      
        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                ClsGlobal.ClearMessage(lblMessage);
                cmbTruckNo.SelectedIndex = -1;
                cmbDispatchID.SelectedIndex = -1;
                dtpFromDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                dtpToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                dgv.DataSource = null;
                lblCount.Text = "Rows Count : " + dgv.Rows.Count.ToString();
            }
            catch (Exception ex)
            {
                ClsGlobal.SetErrorMessage(ex.Message, lblMessage);
            }
        }
        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                ClsGlobal.ClearMessage(lblMessage);
                if (dgv.Rows.Count > 0)
                {
                    this.Cursor = Cursors.WaitCursor;
                    saveFileDialog1.Filter = "Excel Files|*.xlsx|1997-2003 Excel Files|*.xls|CSV Files|*.csv";
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        ShowWait(true);
                        Application.DoEvents();
                        DataTable dt = dgv.DataSource as DataTable;
                        if (saveFileDialog1.FilterIndex == 1)
                        {
                            ClsGlobal.ExportExcel(dt, saveFileDialog1.FileName);
                        }
                        else if (saveFileDialog1.FilterIndex == 2)
                        {
                            ClsGlobal.ExportExcel(dt, saveFileDialog1.FileName);
                        }
                        else if (saveFileDialog1.FilterIndex == 3)
                            ClsGlobal.ExportCsv(dt, saveFileDialog1.FileName);
                    }
                }
                else
                    ClsGlobal.ShowInfoMessageBox("There is no data to export");
            }
            catch (Exception ex)
            {
                ClsGlobal.SetErrorMessage(ex.Message, lblMessage);
            }
            finally
            { ShowWait(false); this.Cursor = Cursors.Default; }
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

        #region Methods

        private void GetLoadingList()
        {
            try
            {
                ClsGlobal.ClearMessage(lblMessage);
                dgv.DataSource = null;
                DataTable dt;
                dt = oDal.ManageReport(EnumDbType.LOADINGSLIP, sFromDate, sToDate, "", "", sTruckNo);
                if (dt.Rows.Count > 0)
                {
                    dgv.DataSource = dt;
                    dgv.Columns[0].Width = 110;
                    dgv.Columns[1].Width = 110;
                    dgv.Columns[2].Width = 90;
                    dgv.Columns[3].Width = 135;
                    dgv.Columns[4].Width = 120;
                    lblCount.Text = "Rows Count : " + dgv.Rows.Count.ToString();
                }
                else
                    ClsGlobal.SetInfoMessage("Data not found", lblMessage);

                lblCount.Text = "Rows Count : " + dgv.Rows.Count.ToString();
            }
            catch (Exception ex)
            {
                ClsGlobal.SetErrorMessage(ex.Message, lblMessage);
            }
        }

        private void GetTruckNo()
        {
            try
            {
                ClsGlobal.ClearMessage(lblMessage);
                DataTable dt = oDal.ManageReport(EnumDbType.TRUCKNO, dtpFromDate.Text, dtpToDate.Text);
                cmbTruckNo.DataSource = dt;
                cmbTruckNo.DisplayMember = "TruckNo";
                cmbTruckNo.ValueMember = "TruckNo";
                cmbTruckNo.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                ClsGlobal.SetErrorMessage(ex.Message, lblMessage);
            }
        }

        private void GetDispatchID()
        {
            try
            {
                ClsGlobal.ClearMessage(lblMessage);
                DataTable dt = oDal.ManageReport(EnumDbType.DISPATCHID, dtpFromDate.Text, dtpToDate.Text);
                cmbDispatchID.DataSource = dt;
                cmbDispatchID.DisplayMember = "DispatchId";
                cmbDispatchID.ValueMember = "DispatchId";
                cmbDispatchID.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                ClsGlobal.SetErrorMessage(ex.Message, lblMessage);
            }
        }

        private void ShowWait(bool IsShow)
        {
            try
            {
                if (IsShow)
                {
                    pnlMain.Enabled = false;
                }
                else
                {
                    pnlMain.Enabled = true;
                }
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion
    }
}
