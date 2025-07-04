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
    public partial class frmUserMaster : Form
    {
        #region Variables

        Dal oDal;
        User oUser;
        bool _IsUpdate = false;

        #endregion

        #region Form Methods

        public frmUserMaster()
        {
            try
            {
                InitializeComponent();
                oUser = new User();
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
                btnDelete.Enabled = false;
                ClsGlobal.ClearMessage(lblMessage);
                txtUserId.Focus();
                GetGroup();
                GetUser();
            }
            catch (Exception ex)
            {
                ClsGlobal.SetErrorMessage(ex.Message, lblMessage);
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
                    oUser.UserId = txtUserId.Text.Trim();
                    oUser.Name = txtName.Text.Trim();
                    oUser.Password = txtPassword.Text.Trim();
                    oUser.CreatedBy = ClsGlobal.UserId;
                    oUser.Group = cmbGroup.SelectedItem.ToString();
                    //If saving data
                    if (_IsUpdate == false)
                    {
                        oUser.DbType = EnumDbType.INSERT;
                        oDal.ManageUser(oUser);
                        btnReset_Click(sender, e);
                        ClsGlobal.SetSuccessMessage("Saved successfully!!", lblMessage);
                    }
                    else // if updating data
                    {
                        oUser.DbType = EnumDbType.UPDATE;
                        oDal.ManageUser(oUser);
                        btnReset_Click(sender, e);
                        ClsGlobal.SetSuccessMessage("Updated successfully!!", lblMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Violation of PRIMARY KEY"))
                {
                    ClsGlobal.SetErrorMessage("UserId already exist!!", lblMessage);
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
                txtUserIdSearch.Text = "";
                Clear();
                GetUser();
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
                if (string.IsNullOrEmpty(txtUserId.Text))
                {
                    ClsGlobal.SetInfoMessage("Please select user", lblMessage);
                    return;
                }
                if (DialogResult.Yes == MessageBox.Show("Äre you sure to delete the record !!", ClsGlobal.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    oUser.UserId = txtUserId.Text.Trim();
                    oUser.DbType = EnumDbType.DELETE;
                    oDal.ManageUser(oUser);

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
                txtUserId.Text = "";
                txtName.Text = "";
                txtPassword.Text = "";
                cmbGroup.SelectedIndex = 0;
                ClsGlobal.ClearMessage(lblMessage);
                txtUserId.Enabled = true;
                btnDelete.Enabled = false;
                _IsUpdate = false;
            }
            catch (Exception ex)
            {
                ClsGlobal.SetErrorMessage(ex.Message, lblMessage);
            }
        }

        private void GetGroup()
        {
            try
            {
                ClsGlobal.ClearMessage(lblMessage);
                cmbGroup.Items.Add("--Select--");
                DataTable dt = oDal.GetGroupName();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                        cmbGroup.Items.Add(row["GroupName"].ToString());
                }
                else
                    ClsGlobal.SetInfoMessage("User group data not found", lblMessage);
                cmbGroup.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ClsGlobal.SetErrorMessage(ex.Message, lblMessage);
            }
        }

        private void GetUser()
        {
            try
            {
                ClsGlobal.ClearMessage(lblMessage);
                oUser.DbType = EnumDbType.SELECT;
                DataTable dt = oDal.ManageUser(oUser);
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
                if (txtUserId.Text.Trim().Length == 0)
                {
                    ClsGlobal.SetInfoMessage("User Id can't be blank!!", lblMessage);
                    txtUserId.Focus();
                    txtUserId.SelectAll();
                    return false;
                }
                if (txtName.Text.Trim().Length == 0)
                {
                    ClsGlobal.SetInfoMessage("User name can't be blank!!", lblMessage);
                    txtName.Focus();
                    return false;
                }
                if (txtPassword.Text.Trim().Length == 0)
                {
                    ClsGlobal.SetInfoMessage("Password can't be blank!!", lblMessage);
                    txtPassword.Focus();
                    return false;
                }
                if (cmbGroup.SelectedIndex <= 0)
                {
                    ClsGlobal.SetInfoMessage("Please select Group!!", lblMessage);
                    cmbGroup.Focus();
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
        private void dgvGroupMaster_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Clear();
                txtUserId.Text = dgv.Rows[e.RowIndex].Cells["UserId"].Value.ToString();
                txtName.Text = dgv.Rows[e.RowIndex].Cells["UserName"].Value.ToString();
                txtPassword.Text = dgv.Rows[e.RowIndex].Cells["Password"].Value.ToString();
                cmbGroup.SelectedItem = dgv.Rows[e.RowIndex].Cells["GroupName"].Value.ToString();

                btnDelete.Enabled = true;
                txtUserId.Enabled = false;
                _IsUpdate = true;
            }
            catch (Exception ex)
            {
                ClsGlobal.SetErrorMessage(ex.Message, lblMessage);
            }
        }

        #endregion

        #region TextBox Event

        private void txtUserIdSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ClsGlobal.ClearMessage(lblMessage);
                oUser.DbType = EnumDbType.SEARCH;
                oUser.UserId = txtUserIdSearch.Text.Trim();
                DataTable dt = oDal.ManageUser(oUser);
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
