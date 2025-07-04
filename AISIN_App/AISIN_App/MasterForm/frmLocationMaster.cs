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
    public partial class frmLocationMaster : Form
    {
        #region Variables

        Dal oDal;
        Location oLocation;
        bool _IsUpdate = false;

        #endregion

        #region Form Methods

        public frmLocationMaster()
        {
            try
            {
                InitializeComponent();
                oLocation = new Location();
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
                    oLocation.LocationCode = txtLocationCode.Text.Trim();
                    oLocation.Description = txtDesc.Text.Trim();
                    oLocation.Capacity = int.Parse(txtCapacity.Text.Trim());
                    List<string> _ListBackNo = new List<string>();

                    foreach (var item in chkListBackNo.CheckedItems)
                        _ListBackNo.Add(item.ToString());

                    oLocation.ListBackNo = _ListBackNo;
                    oLocation.CreatedBy = ClsGlobal.UserId;
                    //If saving data
                    if (_IsUpdate == false)
                    {
                        oLocation.DbType = EnumDbType.INSERT;
                        oDal.SaveLocationData(oLocation);
                        btnReset_Click(sender, e);
                        ClsGlobal.SetSuccessMessage("Saved successfully!!", lblMessage);
                    }
                    else // if updating data
                    {
                        oLocation.DbType = EnumDbType.UPDATE;
                        oDal.SaveLocationData(oLocation);
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

        private void bntPrint_Click(object sender, EventArgs e)
        {
            try
            {
                ClsGlobal.ClearMessage(lblMessage);
                List<string> _ListSelectedLocation = new List<string>();
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (!string.IsNullOrEmpty(row.Cells["IsPrint"].Value.ToString()))//Added by Akhilesh on 18-01-2021 for check null value
                    if (Convert.ToBoolean(row.Cells["IsPrint"].Value) == true)
                    {
                        _ListSelectedLocation.Add(row.Cells["LocationCode"].Value.ToString());
                    }
                }

                if (_ListSelectedLocation.Count > 0)
                {
                    bool IsPrintOk = false;
                    foreach (var loccode in _ListSelectedLocation)
                    {
                        string PrintMessage = ClsGlobal.PrintLocationLabel(loccode);
                        if (PrintMessage.ToUpper() == "OK")
                        {
                            IsPrintOk = true;
                        }
                        else
                        {
                            IsPrintOk = false;
                            ClsGlobal.SetInfoMessage(PrintMessage, lblMessage);
                            break;
                        }
                    }
                    if (IsPrintOk)
                    {
                        btnReset_Click(sender, e);
                        ClsGlobal.SetSuccessMessage("Print Sucessfully!!", lblMessage);
                    }
                }
                else
                    ClsGlobal.SetInfoMessage("Select Location", lblMessage);
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
                    oLocation.LocationCode = txtLocationCode.Text.Trim();
                    oLocation.DbType = EnumDbType.DELETE;
                    oDal.ManageLocation(oLocation);

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
                txtLocationCode.Text = "";
                txtDesc.Text = "";
                ClsGlobal.ClearMessage(lblMessage);
                txtCapacity.Text = "";
                chkListBackNo.Items.Clear();
                GetPartNo();
                txtLocationCode.Enabled = true;
                btnDelete.Enabled = false;
                _IsUpdate = false;
                dgvBackNo.DataSource = null;
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
                oLocation.DbType = EnumDbType.SELECT;
                DataTable dt = oDal.ManageLocation(oLocation);
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
                    ClsGlobal.SetInfoMessage("Location Code can't be blank!!", lblMessage);
                    txtLocationCode.Focus();
                    return false;
                }

                if (txtDesc.Text.Trim().Length == 0)
                {
                    ClsGlobal.SetInfoMessage("Description can't be blank!!", lblMessage);
                    txtDesc.Focus();
                    return false;
                }
                if (txtCapacity.Text.Trim() == "" || Convert.ToInt32(txtCapacity.Text.Trim()) == 0)
                {
                    ClsGlobal.SetInfoMessage("Please input capacity!!", lblMessage);
                    txtCapacity.Focus();
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
                    txtLocationCode.Text = dgv.Rows[e.RowIndex].Cells["LocationCode"].Value.ToString();
                    txtDesc.Text = dgv.Rows[e.RowIndex].Cells["Description"].Value.ToString();
                    txtCapacity.Text = dgv.Rows[e.RowIndex].Cells["Capacity"].Value.ToString();
                    DataTable dt = oDal.GetLocationPart(txtLocationCode.Text.Trim());
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < chkListBackNo.Items.Count; j++)
                        {
                            if (chkListBackNo.Items[j].ToString() == dt.Rows[i]["BackNo"].ToString())
                            {
                                chkListBackNo.SetItemChecked(j, true);
                                break;
                            }
                            else
                            {
                                chkListBackNo.SetItemChecked(j, false);
                               
                            }
                        }
                    }
                    dgvBackNo.DataSource = dt;
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
                    DataTable dt = oDal.GetLocationPart(dgv.Rows[e.RowIndex].Cells["LocationCode"].Value.ToString());
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
                oLocation.DbType = EnumDbType.SEARCH;
                oLocation.LocationCode = txtSearch.Text.Trim();
                DataTable dt = oDal.ManageLocation(oLocation);
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
