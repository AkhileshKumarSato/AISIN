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
    public partial class frmLoadingList : Form
    {
        #region Variables

        Dal oDal;

        #endregion

        #region Form Methods

        public frmLoadingList()
        {
            try
            {
                InitializeComponent();
                oDal = new Dal();
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
                dgv.DataSource = null;
              
                DataTable dt = oDal.ManageLoadingList(EnumDbType.SELECT, dtpFromDate.Text, dtpToDate.Text, txtTruckNo.Text.Trim());
                if (dt.Rows.Count > 0)
                {
                    dgv.DataSource = dt;
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
        private void btnOut_Click(object sender, EventArgs e)
        {
            try
            {
                ClsGlobal.ClearMessage(lblMessage);
                DialogResult dialogResult = new DialogResult();
                dialogResult = MessageBox.Show("Loading Out", "Do you want to out ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.No)
                {
                    return;
                }

                if (txtTruckNo.Text.Trim() == "")
                {
                    ClsGlobal.SetInfoMessage("Please input truck no.", lblMessage);
                    txtTruckNo.Focus();
                    return;
                }

                string Msg= oDal.ManageLoadingList(EnumDbType.UPDATE, dtpFromDate.Text, dtpToDate.Text, txtTruckNo.Text.Trim()).Rows[0]["Result"].ToString();
                if (Msg =="Y")
                {
                    ClsGlobal.SetSuccessMessage("Saved successfully!!", lblMessage);
                    dialogResult = MessageBox.Show("Loading Slip ", "Do you want to generate loading slip ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        frmDirectDispatchReport objDirectDispatchReport = new frmDirectDispatchReport(txtTruckNo.Text.Trim().ToString(), dtpFromDate.Text, dtpToDate.Text);
                        objDirectDispatchReport.ShowDialog();
                        btnReset_Click(sender, e);
                    }
                    else
                    {
                        btnReset_Click(sender, e);
                        //ClsGlobal.SetSuccessMessage("Saved successfully!!", lblMessage);
                    }
                }
                else
                    ClsGlobal.SetInfoMessage(Msg, lblMessage);
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
                dtpFromDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                dtpToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                txtTruckNo.Text = "";
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

        private void ShowWait(bool IsShow)
        {
            try
            {
                if (IsShow)
                {
                    pnlMain.Enabled = false;
                    lblWait.Visible = true;
                }
                else
                {
                    pnlMain.Enabled = true;
                    lblWait.Visible = false;
                }
            }
            catch (Exception ex) { throw ex; }
        }



        #endregion

        private void btnReOpen_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("ReOpen", "Do you want to ReOpen.", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {

                    ClsGlobal.ClearMessage(lblMessage);
                    if (txtTruckNo.Text.Trim() == "")
                    {
                        ClsGlobal.SetInfoMessage("Please input truck no.", lblMessage);
                        txtTruckNo.Focus();
                        return;
                    }
                    string Msg = oDal.ManageLoadingList(EnumDbType.COMPLETE, dtpFromDate.Text, dtpToDate.Text, txtTruckNo.Text.Trim()).Rows[0]["Result"].ToString();
                    if (Msg == "Y")
                    {
                        // btnReset_Click(sender, e);
                        ClsGlobal.SetSuccessMessage("ReOpen successfully!!", lblMessage);
                    }
                    else
                        ClsGlobal.SetInfoMessage(Msg, lblMessage);
                }
            }
            catch (Exception ex)
            {
                ClsGlobal.SetErrorMessage(ex.Message, lblMessage);
            }
        }
    }
}
