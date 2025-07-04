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
    public partial class frmReport : Form
    {
        #region Variables

        Dal oDal;

        #endregion

        #region Form Methods

        public frmReport()
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
                GetLine();
                GetBackNo();
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
                string BackNo = "";
                string LineN = "";
                ClsGlobal.ClearMessage(lblMessage);
                dgv.DataSource = null;
                DataTable dt;

                if (cmbLine.SelectedIndex>=0)
                {
                    LineN = cmbLine.SelectedValue.ToString();
                }

                if (cmbBackNo.SelectedIndex >= 0)
                {
                    BackNo = cmbBackNo.SelectedValue.ToString();
                }
                dt = oDal.ManageReport(EnumDbType.SELECT, dtpFromDate.Text, dtpToDate.Text, LineN, BackNo);
                if (dt.Rows.Count > 0)
                {
                    dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
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
      
        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                ClsGlobal.ClearMessage(lblMessage);
                cmbLine.SelectedIndex = -1;
                cmbBackNo.SelectedIndex = -1;
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

        private void GetLine()
        {
            try
            {
                ClsGlobal.ClearMessage(lblMessage);
                DataTable dt = oDal.ManageReport(EnumDbType.LINE, dtpFromDate.Text, dtpToDate.Text);
                cmbLine.DataSource = dt;
                cmbLine.DisplayMember = "Line_No";
                cmbLine.ValueMember = "Line_No";
                cmbLine.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                ClsGlobal.SetErrorMessage(ex.Message, lblMessage);
            }
        }

        private void GetBackNo()
        {
            try
            {
                ClsGlobal.ClearMessage(lblMessage);
                DataTable dt = oDal.ManageReport(EnumDbType.BACKNO, dtpFromDate.Text, dtpToDate.Text);
                cmbBackNo.DataSource = dt;
                cmbBackNo.DisplayMember = "BackNo";
                cmbBackNo.ValueMember = "BackNo";
                cmbBackNo.SelectedIndex = -1;
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
