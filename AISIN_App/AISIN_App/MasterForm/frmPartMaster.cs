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
    public partial class frmPartMaster : Form
    {
        #region Variables

        Dal oDal;
        Part oPart;
        bool _IsUpdate = false;

        #endregion

        #region Form Methods

        public frmPartMaster()
        {
            try
            {
                InitializeComponent();
                oPart = new Part();
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
                txtBackNo.Focus();
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
                    oPart.BackNo = txtBackNo.Text.Trim();
                    oPart.Description = txtDesc.Text.Trim();
                    oPart.StandardBinQty = int.Parse(txtStandardBinQty.Text.Trim());
                    oPart.PartNo = txtPartNo.Text.Trim();
                    oPart.CustomerPartNo = txtCustomerPartNo.Text.Trim();
                    oPart.IsBarcodeAvailable = chkIsBarcodeAvailable.Checked;
                    oPart.CreatedBy = ClsGlobal.UserId;
                    //If saving data
                    if (_IsUpdate == false)
                    {
                        oPart.DbType = EnumDbType.INSERT;
                        oDal.ManagePart(oPart);
                        btnReset_Click(sender, e);
                        ClsGlobal.SetSuccessMessage("Saved successfully!!", lblMessage);
                    }
                    else // if updating data
                    {
                        oPart.DbType = EnumDbType.UPDATE;
                        oDal.ManagePart(oPart);
                        btnReset_Click(sender, e);
                        ClsGlobal.SetSuccessMessage("Updated successfully!!", lblMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Violation of PRIMARY KEY"))
                {
                    ClsGlobal.SetErrorMessage("Back No already exist!!", lblMessage);
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
                if (string.IsNullOrEmpty(txtBackNo.Text))
                {
                    ClsGlobal.SetInfoMessage("Please select part", lblMessage);
                    return;
                }
                if (DialogResult.Yes == MessageBox.Show("Äre you sure to delete the record !!", ClsGlobal.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    oPart.BackNo = txtBackNo.Text.Trim();
                    oPart.DbType = EnumDbType.DELETE;
                    oDal.ManagePart(oPart);

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
                txtBackNo.Text = "";
                txtDesc.Text = "";
                ClsGlobal.ClearMessage(lblMessage);
                txtPartNo.Text = "";
                txtStandardBinQty.Text = "";
                txtPartNo.Text = "";
                txtCustomerPartNo.Text = "";
                chkIsBarcodeAvailable.Checked = false;
                txtBackNo.Enabled = true;
                btnDelete.Enabled = false;
                _IsUpdate = false;
                txtBackNo.Focus();
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
                oPart.DbType = EnumDbType.SELECT;
                DataTable dt = oDal.ManagePart(oPart);
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
                if (txtBackNo.Text.Trim().Length == 0)
                {
                    ClsGlobal.SetInfoMessage("Back No can't be blank!!", lblMessage);
                    txtBackNo.Focus();
                    return false;
                }
                if (txtBackNo.Text.Trim().Length != 4)
                {
                    ClsGlobal.SetInfoMessage("Back No length shoule be 4!!", lblMessage);
                    txtBackNo.Focus();
                    return false;
                }
                if (txtDesc.Text.Trim().Length == 0)
                {
                    ClsGlobal.SetInfoMessage("Description can't be blank!!", lblMessage);
                    txtDesc.Focus();
                    return false;
                }
                if (txtStandardBinQty.Text.Trim() == "" || Convert.ToInt32(txtStandardBinQty.Text.Trim()) == 0)
                {
                    ClsGlobal.SetInfoMessage("Please input standard bin qty!!", lblMessage);
                    txtStandardBinQty.Focus();
                    return false;
                }
                if (txtPartNo.Text.Trim().Length == 0)
                {
                    ClsGlobal.SetInfoMessage("Part No. can't be blank!!", lblMessage);
                    txtPartNo.Focus();
                    return false;
                }
                if (txtCustomerPartNo.Text.Trim().Length == 0)
                {
                    ClsGlobal.SetInfoMessage("Customer Part No. can't be blank!!", lblMessage);
                    txtCustomerPartNo.Focus();
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
                    txtBackNo.Text = dgv.Rows[e.RowIndex].Cells["BackNo"].Value.ToString();
                    txtDesc.Text = dgv.Rows[e.RowIndex].Cells["Description"].Value.ToString();
                    txtStandardBinQty.Text = dgv.Rows[e.RowIndex].Cells["StandardBinQty"].Value.ToString();
                    txtPartNo.Text = dgv.Rows[e.RowIndex].Cells["PartNo"].Value.ToString();
                    txtCustomerPartNo.Text = dgv.Rows[e.RowIndex].Cells["CustomerPartNo"].Value.ToString();
                    chkIsBarcodeAvailable.Checked = Convert.ToBoolean(dgv.Rows[e.RowIndex].Cells["IsBarcode"].Value);

                    btnDelete.Enabled = true;
                    txtBackNo.Enabled = false;
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

        private void txtStandardBinQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                ClsGlobal.ClearMessage(lblMessage);
                if (e.KeyChar != 8 && !char.IsNumber(e.KeyChar))
                    e.Handled = true;
            }
            catch (Exception ex)
            {
                ClsGlobal.SetErrorMessage(ex.Message, lblMessage);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ClsGlobal.ClearMessage(lblMessage);
                oPart.DbType = EnumDbType.SEARCH;
                oPart.BackNo = txtSearch.Text.Trim();
                DataTable dt = oDal.ManagePart(oPart);
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
