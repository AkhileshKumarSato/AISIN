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
    public partial class frmChangePassword : Form
    {
        #region Variables

        Dal oDal;
        User oUser;

        #endregion

        #region Form Methods

        public frmChangePassword()
        {
            try
            {
                InitializeComponent();
                oUser = new User();
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
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Normal;

                ClsGlobal.ClearMessage(lblMessage);
                txtOldPassword.Focus();
            }
            catch (Exception ex)
            {
                ClsGlobal.SetErrorMessage(ex.Message, lblMessage);
            }
        }

        #endregion

        #region Button Event
        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ClsGlobal.ClearMessage(lblMessage);
                if (string.IsNullOrEmpty(txtOldPassword.Text))
                {
                    ClsGlobal.SetInfoMessage("Enter Old Password", lblMessage);
                    txtOldPassword.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtNewPassword.Text))
                {
                    ClsGlobal.SetInfoMessage("Enter New Password", lblMessage);
                    txtNewPassword.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(txtConfirmPassword.Text))
                {
                    ClsGlobal.SetInfoMessage("Enter Confirm Password", lblMessage);
                    txtConfirmPassword.Focus();
                    return;
                }

                if (txtConfirmPassword.Text.Trim() != txtNewPassword.Text.Trim())
                {
                    ClsGlobal.SetInfoMessage("Password does not match", lblMessage);
                    txtNewPassword.Focus();
                    return;
                }

                oUser.UserId = ClsGlobal.UserGroup;
                oUser.Password = txtOldPassword.Text.Trim();
                oUser.NewPassword = txtNewPassword.Text.Trim();
                oUser.DbType = EnumDbType.UPDATEPASSWORD;
                DataTable dt = oDal.ManageUser(oUser);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Result"].ToString().Trim().ToUpper() == "Y")
                    {
                        btnReset_Click(sender, e);
                        ClsGlobal.SetSuccessMessage("Password changed successfully!!", lblMessage);
                    }
                    else
                    {
                        ClsGlobal.SetInfoMessage(dt.Rows[0]["Result"].ToString(), lblMessage);
                    }
                }
                else
                {
                    ClsGlobal.SetInfoMessage("No data return form database", lblMessage);
                }
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
                txtOldPassword.Text = "";
                txtNewPassword.Text = "";
                txtConfirmPassword.Text = "";
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
    }
}
