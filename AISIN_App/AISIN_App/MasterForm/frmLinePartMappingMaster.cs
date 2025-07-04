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
    public partial class frmLinePartMappingMaster : Form
    {
        #region Variables

        Dal oDal;
        LinePartMapping oLinePartMapping;
        bool _IsUpdate = false;

        #endregion

        #region Form Methods

        public frmLinePartMappingMaster()
        {
            try
            {
                InitializeComponent();
                oLinePartMapping = new LinePartMapping();
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
                txtLocationCode.Focus();
                GetLineNo();
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
                    oLinePartMapping.PartNo = txtLocationCode.Text.Trim();
                    oLinePartMapping.Description = txtDesc.Text.Trim();
                    List<string> _ListBackNo = new List<string>();

                    foreach (var item in chkListLineNo.CheckedItems)
                        _ListBackNo.Add(item.ToString());

                    oLinePartMapping.ListLineNo = _ListBackNo;
                    oLinePartMapping.CreatedBy = ClsGlobal.UserId;
                    //If saving data
                    if (_IsUpdate == false)
                    {
                        oLinePartMapping.DbType = EnumDbType.INSERT;
                        oDal.SavePartLineMappingData(oLinePartMapping);
                        btnReset_Click(sender, e);
                        ClsGlobal.SetSuccessMessage("Saved successfully!!", lblMessage);
                    }
                    else // if updating data
                    {
                        oLinePartMapping.DbType = EnumDbType.UPDATE;
                        oDal.SavePartLineMappingData(oLinePartMapping);
                        btnReset_Click(sender, e);
                        ClsGlobal.SetSuccessMessage("Updated successfully!!", lblMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Violation of PRIMARY KEY"))
                {
                    ClsGlobal.SetErrorMessage("Location Code already exist!!", lblMessage);
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
                if (string.IsNullOrEmpty(txtLocationCode.Text))
                {
                    ClsGlobal.SetInfoMessage("Please select part", lblMessage);
                    return;
                }
                if (DialogResult.Yes == MessageBox.Show("Äre you sure to delete the record !!", ClsGlobal.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    oLinePartMapping.PartNo = txtLocationCode.Text.Trim();
                    oLinePartMapping.DbType = EnumDbType.DELETE;
                    oDal.ManagePartLineMapping(oLinePartMapping);

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
        private void GetLineNo()
        {
            try
            {
                ClsGlobal.ClearMessage(lblMessage);
                oLinePartMapping.DbType = EnumDbType.LINE;
                DataTable dt = oDal.ManagePartLineMapping(oLinePartMapping);
                for (int i = 0; i < dt.Rows.Count; i++)
                    chkListLineNo.Items.Add(dt.Rows[i]["LineNo"].ToString());
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
                txtLocationCode.Text = "";
                txtDesc.Text = "";
                ClsGlobal.ClearMessage(lblMessage);
                dgvLineNo.DataSource = null;
                chkListLineNo.Items.Clear();
                GetLineNo();
                txtLocationCode.Enabled = true;
                btnDelete.Enabled = false;
                _IsUpdate = false;
                txtLocationCode.Focus();
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
                oLinePartMapping.DbType = EnumDbType.SELECT;
                DataTable dt = oDal.ManagePartLineMapping(oLinePartMapping);
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
                if (txtLocationCode.Text.Trim().Length == 0)
                {
                    ClsGlobal.SetInfoMessage("Part No. can't be blank!!", lblMessage);
                    txtLocationCode.Focus();
                    return false;
                }

                if (txtDesc.Text.Trim().Length == 0)
                {
                    ClsGlobal.SetInfoMessage("Description can't be blank!!", lblMessage);
                    txtDesc.Focus();
                    return false;
                }
               
                //Check part is checked or not
                if (chkListLineNo.CheckedItems.Count == 0)
                {
                    ClsGlobal.SetInfoMessage("Please select Line no!!", lblMessage);
                    chkListLineNo.Focus();
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
                    txtLocationCode.Text = dgv.Rows[e.RowIndex].Cells["PartNo"].Value.ToString();
                    txtDesc.Text = dgv.Rows[e.RowIndex].Cells["Description"].Value.ToString();
                    DataTable dt = oDal.GetLocationPart(txtLocationCode.Text.Trim());
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < chkListLineNo.Items.Count; j++)
                        {
                            if (chkListLineNo.Items[j].ToString() == dt.Rows[i]["LineNo"].ToString())
                            {
                                chkListLineNo.SetItemChecked(j, true);
                                break;
                            }
                            else
                            {
                                chkListLineNo.SetItemChecked(j, false);
                               
                            }
                        }
                    }
                    dgvLineNo.DataSource = dt;
                    btnDelete.Enabled = true;
                    txtLocationCode.Enabled = false;
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
                    ClsGlobal.ClearMessage(lblMessage);
                    oLinePartMapping.DbType = EnumDbType.LINEMAPPED;
                    oLinePartMapping.PartNo = dgv.Rows[e.RowIndex].Cells["PartNo"].Value.ToString();
                    DataTable dt = oDal.ManagePartLineMapping(oLinePartMapping);
                    dgvLineNo.DataSource = dt;
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
                oLinePartMapping.DbType = EnumDbType.SEARCH;
                oLinePartMapping.PartNo = txtSearch.Text.Trim();
                DataTable dt = oDal.ManagePartLineMapping(oLinePartMapping);
                dgv.DataSource = dt;
                lblCount.Text = "Rows Count : " + dgv.Rows.Count;
            }
            catch (Exception ex)
            {
                ClsGlobal.SetErrorMessage(ex.Message, lblMessage);
            }
        }


        #endregion

        private void dgv_CellBorderStyleChanged(object sender, EventArgs e)
        {

        }
    }
}
