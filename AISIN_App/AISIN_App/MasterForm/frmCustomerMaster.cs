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
    public partial class frmCustomerMaster : Form
    {
        #region Variables

        Dal oDal;
        Customer oCustomer;
        bool _IsUpdate = false;

        #endregion

        #region Form Methods

        public frmCustomerMaster()
        {
            try
            {
                InitializeComponent();
                oCustomer = new Customer();
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
                txtCustomerCode.Focus();
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
                    oCustomer.CustomerCode = txtCustomerCode.Text.Trim();
                    oCustomer.Name = txtCustomerName.Text.Trim();
                    oCustomer.Location = txtLocation.Text.Trim();
                    oCustomer.Address = txtAddress.Text.Trim();
                    List<string> _ListBackNo = new List<string>();

                    foreach (var item in chkListBackNo.CheckedItems)
                        _ListBackNo.Add(item.ToString());

                    oCustomer.ListBackNo = _ListBackNo;
                    oCustomer.IsCustomerEnable = chkIsCustomerKanban.Checked;
                    oCustomer.CreatedBy = ClsGlobal.UserId;
                    //If saving data
                    if (_IsUpdate == false)
                    {
                        oCustomer.DbType = EnumDbType.INSERT;
                        oDal.SaveCustomerData(oCustomer);
                        btnReset_Click(sender, e);
                        ClsGlobal.SetSuccessMessage("Saved successfully!!", lblMessage);
                    }
                    else // if updating data
                    {
                        oCustomer.DbType = EnumDbType.UPDATE;
                        oDal.SaveCustomerData(oCustomer);
                        btnReset_Click(sender, e);
                        ClsGlobal.SetSuccessMessage("Updated successfully!!", lblMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Violation of PRIMARY KEY"))
                {
                    ClsGlobal.SetErrorMessage("Customer Code already exist!!", lblMessage);
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
                if (string.IsNullOrEmpty(txtCustomerCode.Text))
                {
                    ClsGlobal.SetInfoMessage("Please select part", lblMessage);
                    return;
                }
                if (DialogResult.Yes == MessageBox.Show("Äre you sure to delete the record !!", ClsGlobal.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    oCustomer.CustomerCode = txtCustomerCode.Text.Trim();
                    oCustomer.DbType = EnumDbType.DELETE;
                    oDal.ManageCustomer(oCustomer);

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
        private void GetPartNo()
        {
            try
            {
                ClsGlobal.ClearMessage(lblMessage);
                DataTable dt = oDal.GetPart(1);
                for (int i = 0; i < dt.Rows.Count; i++)
                    chkListBackNo.Items.Add(dt.Rows[i]["BackNo"].ToString());
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
                txtCustomerCode.Text = "";
                txtCustomerName.Text = "";
                txtLocation.Text = "";
                txtAddress.Text = "";
                ClsGlobal.ClearMessage(lblMessage);
                chkIsCustomerKanban.Checked = false;
                chkListBackNo.Items.Clear();
                GetPartNo();
                txtCustomerCode.Enabled = true;
                btnDelete.Enabled = false;
                _IsUpdate = false;
                dgvBackNo.DataSource = null;
                txtCustomerCode.Focus();
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
                oCustomer.DbType = EnumDbType.SELECT;
                DataTable dt = oDal.ManageCustomer(oCustomer);
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
                if (txtCustomerCode.Text.Trim().Length == 0)
                {
                    ClsGlobal.SetInfoMessage("Customer Code can't be blank!!", lblMessage);
                    txtCustomerCode.Focus();
                    return false;
                }

                if (txtCustomerName.Text.Trim().Length == 0)
                {
                    ClsGlobal.SetInfoMessage("Customer Name can't be blank!!", lblMessage);
                    txtCustomerName.Focus();
                    return false;
                }
                if (txtLocation.Text.Trim().Length == 0)
                {
                    ClsGlobal.SetInfoMessage("Customer Location can't be blank!!", lblMessage);
                    txtLocation.Focus();
                    return false;
                }
                if (txtAddress.Text.Trim().Length == 0)
                {
                    ClsGlobal.SetInfoMessage("Customer Address can't be blank!!", lblMessage);
                    txtAddress.Focus();
                    return false;
                }
                //Check part is checked or not
                if (chkListBackNo.CheckedItems.Count == 0)
                {
                    ClsGlobal.SetInfoMessage("Please select back no!!", lblMessage);
                    chkListBackNo.Focus();
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
                    txtCustomerCode.Text = dgv.Rows[e.RowIndex].Cells["CustomerCode"].Value.ToString();
                    txtCustomerName.Text = dgv.Rows[e.RowIndex].Cells["CustomerName"].Value.ToString();
                    txtLocation.Text = dgv.Rows[e.RowIndex].Cells["Location"].Value.ToString();
                    txtAddress.Text = dgv.Rows[e.RowIndex].Cells["Address"].Value.ToString();
                    chkIsCustomerKanban.Checked = Convert.ToBoolean(dgv.Rows[e.RowIndex].Cells["IsCustomerKanban"].Value);
                    DataTable dt = oDal.GetCustomerPart(txtCustomerCode.Text.Trim());
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < chkListBackNo.Items.Count; j++)
                        {
                            if (chkListBackNo.Items[j].ToString() == dt.Rows[i]["BackNo"].ToString())
                            {
                                chkListBackNo.SetItemChecked(j, true);
                                break;
                            }
                        }
                    }
                    dgvBackNo.DataSource = dt;
                    btnDelete.Enabled = true;
                    txtCustomerCode.Enabled = false;
                    _IsUpdate = true;
                }
            }
            catch (Exception ex)
            {
                ClsGlobal.SetErrorMessage(ex.Message, lblMessage);
            }
        }
        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataTable dt = oDal.GetCustomerPart(dgv.Rows[e.RowIndex].Cells["CustomerCode"].Value.ToString());
                    dgvBackNo.DataSource = dt;
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
                oCustomer.DbType = EnumDbType.SEARCH;
                oCustomer.CustomerCode = txtSearch.Text.Trim();
                DataTable dt = oDal.ManageCustomer(oCustomer);
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
