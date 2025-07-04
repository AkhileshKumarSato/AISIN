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
    public partial class frmLineMaster : Form
    {
        #region Variables

        Dal oDal;
        Line oLine;
        bool _IsUpdate = false;

        #endregion

        #region Form Methods

        public frmLineMaster()
        {
            try
            {
                InitializeComponent();
                oLine = new Line();
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
                btnDelete.Enabled = false;
                ClsGlobal.ClearMessage(lblMessage);
                if (Properties.Settings.Default.New_Page_Used == "N")
                {
                    txtLineNo.MaxLength = 2;
                }
                else
                {
                    txtLineNo.MaxLength = 6;
                }
                txtLineNo.Focus();
                GetPartNo();
                BindGrid();
            }
            catch (Exception ex)
            {
                lblMessage.Text = "ERROR: " + ex.Message;
            }
        }

        #endregion

        #region Button Event

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ClsGlobal.ClearMessage(lblMessage);
                if (ValidateInput())
                {
                    if (Properties.Settings.Default.New_Page_Used == "N")
                    {
                        if (txtLineNo.Text.Trim().Length > 1)
                        {
                            oLine.LineNo = "AS00" + txtLineNo.Text.Trim();
                        }
                        if (txtLineNo.Text.Trim().Length == 1)
                        {
                            oLine.LineNo = "AS000" + txtLineNo.Text.Trim();
                        }
                    }
                    else
                    {
                        oLine.LineNo = txtLineNo.Text.Trim();

                    }
                    oLine.BackNo = cmbBackNo.SelectedValue.ToString();
                    oLine.Description = txtDesc.Text.Trim();
                    oLine.CreatedBy = ClsGlobal.UserId;
                    //If saving data
                    if (_IsUpdate == false)
                    {
                        oLine.DbType = EnumDbType.INSERT;
                        oDal.ManageLine(oLine);
                        btnReset_Click(sender, e);
                        ClsGlobal.SetSuccessMessage("Saved successfully!!", lblMessage);
                    }
                    else // if updating data
                    {
                        oLine.DbType = EnumDbType.UPDATE;
                        oDal.ManageLine(oLine);
                        btnReset_Click(sender, e);
                        ClsGlobal.SetSuccessMessage("Updated successfully!!", lblMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Violation of PRIMARY KEY"))
                {
                    ClsGlobal.SetErrorMessage("Line No already exist!!", lblMessage);
                }
                else
                {
                    ClsGlobal.SetErrorMessage(ex.Message, lblMessage);
                }
            }
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                txtSearch.Text = "";
                Clear();
                BindGrid();
            }
            catch (Exception ex)
            {
                ClsGlobal.SetErrorMessage(ex.Message, lblMessage);
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                ClsGlobal.ClearMessage(lblMessage);
                if (string.IsNullOrEmpty(txtLineNo.Text))
                {
                    ClsGlobal.SetInfoMessage("Please select line", lblMessage);
                    return;
                }
                if (DialogResult.Yes == MessageBox.Show("Äre you sure to delete the record !!", ClsGlobal.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    oLine.LineNo = txtLineNo.Text.Trim();
                    oLine.DbType = EnumDbType.DELETE;
                    oDal.ManageLine(oLine);

                    btnReset_Click(sender, e);
                    ClsGlobal.SetSuccessMessage("Deleted successfully!!", lblMessage);
                }
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

        #region Methods

        private void Clear()
        {
            try
            {
                txtLineNo.Text = "";
                txtDesc.Text = "";
                cmbBackNo.SelectedIndex = -1;
                ClsGlobal.ClearMessage(lblMessage);
                txtLineNo.Enabled = true;
                btnDelete.Enabled = false;
                _IsUpdate = false;
                txtLineNo.Focus();
            }
            catch (Exception ex)
            {
                ClsGlobal.SetErrorMessage(ex.Message, lblMessage);
            }
        }
        private void GetPartNo()
        {
            try
            {
                ClsGlobal.ClearMessage(lblMessage);
                DataTable dt = oDal.GetPart(2);
                cmbBackNo.DataSource = dt;
                cmbBackNo.DisplayMember = "BackPartNo";
                cmbBackNo.ValueMember = "BackNo";
                cmbBackNo.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                ClsGlobal.SetErrorMessage(ex.Message, lblMessage);
            }
        }
        private void BindGrid()
        {
            try
            {
                ClsGlobal.ClearMessage(lblMessage);
                oLine.DbType = EnumDbType.SELECT;
                DataTable dt = oDal.ManageLine(oLine);
                dgv.DataSource = dt;
                lblCount.Text = "Rows Count : " + dgv.Rows.Count;
            }
            catch (Exception ex)
            {
                ClsGlobal.SetErrorMessage(ex.Message, lblMessage);
            }
        }

        private bool ValidateInput()
        {
            try
            {
                if (txtLineNo.Text.Trim().Length == 0)
                {
                    ClsGlobal.SetInfoMessage("Line No can't be blank!!", lblMessage);
                    txtLineNo.Focus();
                    return false;
                }
                if (txtDesc.Text.Trim().Length == 0)
                {
                    ClsGlobal.SetInfoMessage("Description can't be blank!!", lblMessage);
                    txtDesc.Focus();
                    return false;
                }
                if (cmbBackNo.SelectedIndex < 0)
                {
                    ClsGlobal.SetInfoMessage("Please select back no!!", lblMessage);
                    cmbBackNo.Focus();
                    return false;
                }
                return true;
            }
            catch (Exception ex) { throw ex; }
        }

        #endregion

        #region Label Event
        private void lblMessage_DoubleClick(object sender, EventArgs e)
        {
            ClsGlobal.ShowInfoMessageBox(lblMessage.Text);
        }

        #endregion

        #region DataGridView Events
        private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    Clear();
                    txtLineNo.Text = dgv.Rows[e.RowIndex].Cells["LineNo"].Value.ToString();
                    txtDesc.Text = dgv.Rows[e.RowIndex].Cells["Description"].Value.ToString();
                    cmbBackNo.SelectedValue = dgv.Rows[e.RowIndex].Cells["BackNo"].Value.ToString();

                    btnDelete.Enabled = true;
                    txtLineNo.Enabled = false;
                    _IsUpdate = true;
                }
            }
            catch (Exception ex)
            {
                ClsGlobal.SetErrorMessage(ex.Message, lblMessage);
            }
        }

        #endregion

        #region TextBox Event

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ClsGlobal.ClearMessage(lblMessage);
                oLine.DbType = EnumDbType.SEARCH;
                oLine.LineNo = txtSearch.Text.Trim();
                DataTable dt = oDal.ManageLine(oLine);
                dgv.DataSource = dt;
                lblCount.Text = "Rows Count : " + dgv.Rows.Count;
            }
            catch (Exception ex)
            {
                ClsGlobal.SetErrorMessage(ex.Message, lblMessage);
            }
        }

        #endregion

    }
}
