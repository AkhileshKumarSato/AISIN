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
    public partial class frmUploadPickList : Form
    {
        #region Variables

        Dal oDal;
        int _SrNo = 0;

        #endregion

        #region Form Methods

        public frmUploadPickList()
        {
            try
            {
                InitializeComponent();
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
                lblWait.Visible = false;
                GetPickListNo();
            }
            catch (Exception ex)
            {
                lblMessage.Text = "ERROR: " + ex.Message;
            }
        }

        #endregion

        #region Button Event
        private void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                Clear();
                if (txtPickListNo.Text.Trim() == "")
                {
                    ClsGlobal.SetInfoMessage("PickList No not found", lblMessage);
                    return;
                }
                OpenFileDialog ofdImport = new OpenFileDialog();
                ofdImport.FilterIndex = 1;
                ofdImport.RestoreDirectory = true;

                if (ofdImport.ShowDialog() == DialogResult.OK)
                {
                    pnlMain.Enabled = false;
                    lblWait.Visible = true;
                    Application.DoEvents();
                    string FileName = ofdImport.FileName;
                    string FileExt = Path.GetExtension(FileName);
                    if (FileExt == ".xls" || FileExt == ".xlsx")
                    {
                        clsODBC oOdbc = new clsODBC();
                        oOdbc.DataSource = FileName.Trim();
                        if (oOdbc.Connect())
                        {
                            string Query = "SELECT * FROM [Sheet1$]";
                            DataTable dtResultSet = oOdbc.GetDataTable(Query);
                            oOdbc.Disconnect();
                            BindGridForExcel(dtResultSet);
                        }
                    }
                    else if (FileExt == ".csv")
                    {
                        BindGridForCsv(FileName);
                    }
                    else
                    {
                        ClsGlobal.SetErrorMessage("Unknows file extention " + FileExt, lblMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Cannot open database '(unknown)'"))
                    ClsGlobal.SetErrorMessage("File is open or corrupted", lblMessage);
                else
                    ClsGlobal.SetErrorMessage(ex.Message, lblMessage);
            }
            finally
            {
                pnlMain.Enabled = true;
                lblWait.Visible = false;
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ClsGlobal.ClearMessage(lblMessage);
                if (txtPickListNo.Text.Trim() == "")
                {
                    ClsGlobal.SetInfoMessage("PickList No not found", lblMessage);
                    return;
                }
                if (_SrNo == 0)
                {
                    ClsGlobal.SetInfoMessage("PickList serial no details not found", lblMessage);
                    return;
                }
                if (dgv.Rows.Count == 0)
                {
                    ClsGlobal.SetInfoMessage("PickList details not found", lblMessage);
                    return;
                }
                lblWait.Visible = true;
                pnlMain.Enabled = false;
                Application.DoEvents();
                string Msg = oDal.SavePickListData(dgv, txtPickListNo.Text.Trim(), _SrNo);
                if (Msg == "Y")
                {
                    string PickListNo = txtPickListNo.Text.Trim();
                    btnReset_Click(sender, e);
                    ClsGlobal.SetSuccessMessage("Picklist No. " + PickListNo + " Saved successfully", lblMessage);
                }
                else
                    ClsGlobal.SetInfoMessage(Msg, lblMessage);
            }
            catch (Exception ex)
            {
                ClsGlobal.SetErrorMessage(ex.Message, lblMessage);
            }
            finally
            {
                lblWait.Visible = false;
                pnlMain.Enabled = true;
            }
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                Clear();
                txtPickListNo.Text = "";
                _SrNo = 0;
                GetPickListNo();
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

        #region Methods

        private void BindGridForExcel(DataTable dt)
        {
            try
            {
                ClsGlobal.ClearMessage(lblMessage);
                dgv.DataSource = dt;
                lblCount.Text = "Rows Count : " + dgv.Rows.Count;
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
                dgv.DataSource = null;
                dgv.Rows.Clear();
                dgv.Columns.Clear();
                lblCount.Text = "Rows Count : " + dgv.Rows.Count;
            }
            catch (Exception ex)
            {
                ClsGlobal.SetErrorMessage(ex.Message, lblMessage);
            }
        }

        private void GetPickListNo()
        {
            try
            {
                ClsGlobal.ClearMessage(lblMessage);
                DataTable dt = oDal.GetPickListNo();
                txtPickListNo.Text = dt.Rows[0]["PickListNo"].ToString();
                _SrNo = Convert.ToInt32(dt.Rows[0]["SrNo"]);
            }
            catch (Exception ex)
            {
                ClsGlobal.SetErrorMessage(ex.Message, lblMessage);
            }
        }
        private void BindGridForCsv(string FileName)
        {
            StreamReader sr = null;
            try
            {
                ClsGlobal.ClearMessage(lblMessage);
                sr = new StreamReader(FileName);
                //Skip the header
                sr.ReadLine();
                dgv.Columns.Add("Customer Code", "Customer Code");
                dgv.Columns.Add("BackNo", "BackNo");
                dgv.Columns.Add("PartNo", "PartNo");
                dgv.Columns.Add("Customer Part No", "Customer Part No");
                dgv.Columns.Add("Qty", "Qty");
                while (!sr.EndOfStream)
                {
                    string[] ArrCol = sr.ReadLine().Split(',');
                    if (ArrCol[0].Trim() != "")
                    {
                        //Customer Code,BackNo,PartNo,Customer Part No,Qty
                        dgv.Rows.Add(ArrCol[0].Trim(), ArrCol[1].Trim(), ArrCol[2].Trim(), ArrCol[3].Trim(), ArrCol[4].Trim());
                    }
                }
                lblCount.Text = "Rows Count : " + dgv.Rows.Count;
            }
            catch (Exception ex)
            {
                ClsGlobal.SetErrorMessage(ex.Message, lblMessage);
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                    sr.Dispose();
                    sr = null;
                }
            }
        }

        #endregion

    }
}
