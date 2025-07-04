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
    public partial class frmGroupMaster : Form
    {
        #region Variables

        Dal oDal;
        Group oGroup;
        bool _IsUpdate = false;

        #endregion

        #region Form Methods

        public frmGroupMaster()
        {
            try
            {
                InitializeComponent();
                oGroup = new Group();
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
                ClsGlobal.ClearMessage(lblMessage);
                btnDelete.Enabled = false;
                txtGroupName.Focus();
                GetGroup();
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
                    //If saving data
                    if (_IsUpdate == false)
                    {
                        oGroup.CreatedBy = ClsGlobal.UserId;
                        oGroup.GroupName = txtGroupName.Text.Trim();
                        oDal.SaveGroup(oGroup, dgvGroupRights);
                        btnReset_Click(sender, e);
                        GetGroup();
                        ClsGlobal.SetSuccessMessage("Saved successfully!!", lblMessage);
                    }
                    else // if updating data
                    {
                        oGroup.CreatedBy = ClsGlobal.UserId;
                        oGroup.GroupName = txtGroupName.Text.Trim();
                        oDal.UpdateGroup(oGroup, dgvGroupRights);
                        btnReset_Click(sender, e);
                        GetGroup();
                        ClsGlobal.SetSuccessMessage("Updated successfully!!", lblMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Violation of PRIMARY KEY"))
                {
                    ClsGlobal.SetErrorMessage("Group Name already exist!!", lblMessage);
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
                txtGroupName.Text = "";
                ClsGlobal.ClearMessage(lblMessage);
                btnDelete.Enabled = false;
                txtGroupName.Enabled = true;
                _IsUpdate = false;
                chkAll.Checked = false;
                chkAll_CheckedChanged(sender, e);
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
                if (string.IsNullOrEmpty(txtGroupName.Text))
                {
                    ClsGlobal.SetInfoMessage("Please select Group", lblMessage);
                    return;
                }
                if (DialogResult.Yes == MessageBox.Show("Äre you sure to delete the record !!", ClsGlobal.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    oGroup.GroupName = txtGroupName.Text.Trim();
                    oGroup.DbType = EnumDbType.DELETE;
                    DataSet ds = oDal.GetGroup(oGroup);
                    if (ds.Tables[0].Rows[0][0].ToString().ToUpper() == "SUCCESS")
                    {
                        btnReset_Click(sender, e);
                        GetGroup();
                        ClsGlobal.SetSuccessMessage("Deleted successfully!!", lblMessage);
                    }
                    else
                        ClsGlobal.SetErrorMessage(ds.Tables[0].Rows[0][0].ToString(), lblMessage);
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

        private void GetGroup()
        {
            try
            {
                oGroup.DbType = EnumDbType.SELECT;
                DataSet ds = oDal.GetGroup(oGroup);
                dgvGroupMaster.DataSource = ds.Tables[0];
                dgvGroupRights.DataSource = ds.Tables[1];
                lblCount.Text = "Rows Count : " + dgvGroupMaster.Rows.Count;
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
                if (txtGroupName.Text.Trim().Length == 0)
                {
                    ClsGlobal.SetInfoMessage("Group Name can't be blank!!", lblMessage);
                    txtGroupName.Focus();
                    txtGroupName.SelectAll();
                    return false;
                }
                //Atleast one Module right
                bool IsChecked = false;
                foreach (DataGridViewRow row in dgvGroupRights.Rows)
                {
                    if (Convert.ToBoolean(row.Cells["HasRight"].Value) == true)
                    {
                        IsChecked = true;
                        break;
                    }
                }
                if (IsChecked == false)
                {
                    ClsGlobal.SetInfoMessage("Select Atleast one module right!!", lblMessage);
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

        #region CheckBox Event
        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in dgvGroupRights.Rows)
                {
                    row.Cells["HasRight"].Value = chkAll.Checked;
                }
            }
            catch (Exception ex)
            {
                ClsGlobal.SetErrorMessage(ex.Message, lblMessage);
            }
        }

        #endregion

        #region DataGridView Events
        private void dgvGroupMaster_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                ClsGlobal.ClearMessage(lblMessage);
                chkAll.Checked = false;
                chkAll_CheckedChanged(sender, e);
                txtGroupName.Text = dgvGroupMaster.Rows[e.RowIndex].Cells["GroupName"].Value.ToString();
                btnDelete.Enabled = true;
                txtGroupName.Enabled = false;
                _IsUpdate = true;
                oGroup.GroupName = txtGroupName.Text.Trim();
                oGroup.DbType = EnumDbType.SELECTBYID;
                DataSet ds = oDal.GetGroup(oGroup);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    foreach (DataGridViewRow rowGrid in dgvGroupRights.Rows)
                    {
                        if (rowGrid.Cells["ModuleId"].Value.ToString() == row["ModuleId"].ToString())
                        {
                            rowGrid.Cells["HasRight"].Value = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ClsGlobal.SetErrorMessage(ex.Message, lblMessage);
            }
        }

        #endregion
    }
}
