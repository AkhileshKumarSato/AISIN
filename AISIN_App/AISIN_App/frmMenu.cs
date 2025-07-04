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
    public partial class frmMenu : Form
    {
        #region Variables

        Dal oDal;
        User oUser;

        #endregion

        #region Form Methods

        public frmMenu()
        {
            try
            {
                InitializeComponent();
                oUser = new User();
                oDal = new Dal();
            }
            catch (Exception ex)
            {
                ClsGlobal.ShowErrorMessageBox(ex.Message);
            }
        }

        private void frmModelMaster_Load(object sender, EventArgs e)
        {
            try
            {
                lblWelcome.Text = "Hi! " + ClsGlobal.UserName;
                if (ClsGlobal.UserId.Trim().ToUpper() != "SATO")
                {
                    DisableMenus();
                    SetMenuRight();
                }
            }
            catch (Exception ex)
            {
                ClsGlobal.ShowErrorMessageBox(ex.Message);
            }
        }

        private void OFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Show();
        }

        #endregion

        #region Button Event

        private void picLogOut_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Menu Click Events
        private void picChangePassword_Click(object sender, EventArgs e)
        {
            frmChangePassword oFrm = new frmChangePassword();
            oFrm.ShowDialog();
        }
        private void picUserMaster_Click(object sender, EventArgs e)
        {
            frmUserMaster frm = new frmUserMaster();
            frm.Show();
            frm.FormClosing += OFrm_FormClosing;
            this.Hide();
        }
        private void picGroupMaster_Click(object sender, EventArgs e)
        {
            frmGroupMaster frm = new frmGroupMaster();
            frm.Show();
            frm.FormClosing += OFrm_FormClosing;
            this.Hide();
        }

        private void picLineMaster_Click(object sender, EventArgs e)
        {
            frmLineMaster frm = new frmLineMaster();
            frm.Show();
            frm.FormClosing += OFrm_FormClosing;
            this.Hide();
        }

        private void picLocationMaster_Click(object sender, EventArgs e)
        {
            frmLocationMaster frm = new frmLocationMaster();
            frm.Show();
            frm.FormClosing += OFrm_FormClosing;
            this.Hide();
        }

        private void picPartMaster_Click(object sender, EventArgs e)
        {
            frmPartMaster frm = new frmPartMaster();
            frm.Show();
            frm.FormClosing += OFrm_FormClosing;
            this.Hide();
        }



        private void picCustomerMaster_Click(object sender, EventArgs e)
        {
            frmCustomerMaster frm = new frmCustomerMaster();
            frm.Show();
            frm.FormClosing += OFrm_FormClosing;
            this.Hide();
        }

        private void picReprint_Click(object sender, EventArgs e)
        {
            frmReprint frm = new frmReprint();
            frm.Show();
            frm.FormClosing += OFrm_FormClosing;
            this.Hide();
        }

        private void picProduction_Click(object sender, EventArgs e)
        {
            frmProduction frm = new frmProduction();
            frm.Show();
            frm.FormClosing += OFrm_FormClosing;
            this.Hide();
        }

        private void picUploadPickList_Click(object sender, EventArgs e)
        {
            frmUploadPickList frm = new frmUploadPickList();
            frm.Show();
            frm.FormClosing += OFrm_FormClosing;
            this.Hide();
        }

        private void picLoadingList_Click(object sender, EventArgs e)
        {
            frmLoadingList frm = new frmLoadingList();
            frm.Show();
            frm.FormClosing += OFrm_FormClosing;
            this.Hide();
        }

        #endregion

        #region Method

        private void SetMenuRight()
        {
            try
            {
                DataTable dt = oDal.GetUserRight(ClsGlobal.UserGroup);
                foreach (DataRow row in dt.Rows)
                {
                    switch (row["ModuleId"].ToString())
                    {
                        case "101":
                            picGroupMaster.Enabled = true;
                            lblGroupMaster.Enabled = true;
                            break;
                        case "102":
                            picUserMaster.Enabled = true;
                            lblUserMaster.Enabled = true;
                            break;
                        case "103":
                            picPartMaster.Enabled = true;
                            lblPartMaster.Enabled = true;
                            break;
                        case "104":
                            picLineMaster.Enabled = true;
                            lblLineMaster.Enabled = true;
                            break;
                        case "105":
                            picLocationMaster.Enabled = true;
                            lblLocationMaster.Enabled = true;
                            break;
                        case "106":
                            picCustomerMaster.Enabled = true;
                            lblCustomerMaster.Enabled = true;
                            break;
                        case "201":
                            picProduction.Enabled = true;
                            lblProduction.Enabled = true;
                            break;
                        case "202":
                            picReprint.Enabled = true;
                            lblReprint.Enabled = true;
                            break;
                        case "203":
                            picUploadPickList.Enabled = true;
                            lblUploadPickList.Enabled = true;
                            break;
                        case "204":
                            picLoadingList.Enabled = true;
                            lblLoadingList.Enabled = true;
                            break;
                        case "205":
                            picProductionNew.Enabled = true;
                            lblProductionNew.Enabled = true;
                            break;
                        case "206":
                            picPartLineMapping.Enabled = true;
                            lblPartLineMapping.Enabled = true;
                            break;
                        //case "301":
                        //    picRptCutting.Enabled = true;
                        //    lblRptCutting.Enabled = true;
                        //    break;
                        //case "302":
                        //    picRptQA.Enabled = true;
                        //    lblRptQA.Enabled = true;
                        //    break;
                        //case "303":
                        //    picReOiling.Enabled = true;
                        //    lblReOiling.Enabled = true;
                        //    lblReOilingCount.Enabled = true;
                        //    break;
                        //case "304":
                        //    picMachining.Enabled = true;
                        //    lblMachining.Enabled = true;
                        //    break;
                        //case "305":
                        //    picFinalPacking.Enabled = true;
                        //    lblFinalPacking.Enabled = true;
                        //    break;
                        //case "306":
                        //    picDispatchReport.Enabled = true;
                        //    lblDispatchReport.Enabled = true;
                        //    break;
                        //case "307":
                        //    picInventoryReport.Enabled = true;
                        //    lblInventoryReport.Enabled = true;
                        //    break;
                        //case "308":
                        //    pictureBoxTrolleyReceivingHistory.Enabled = true;
                        //    lblTrolleyRecevingHistory.Enabled = true;
                        //    break;
                        //case "309":
                        //    pictureBoxPicking.Enabled = true;
                        //    lblPicking.Enabled = true;
                        //    break;

                        //case "401":
                        //    ClsGlobal.IsCuttingManualEnable = true;
                        //    break;

                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ClsGlobal.ShowErrorMessageBox(ex.Message);
            }
        }

        private void DisableMenus()
        {
            try
            {
                //Master Menu
                picGroupMaster.Enabled = false;
                lblGroupMaster.Enabled = false;

                picUserMaster.Enabled = false;
                lblUserMaster.Enabled = false;

                picPartMaster.Enabled = false;
                lblPartMaster.Enabled = false;

                picLineMaster.Enabled = false;
                lblLineMaster.Enabled = false;

                picLocationMaster.Enabled = false;
                lblLocationMaster.Enabled = false;

                picCustomerMaster.Enabled = false;
                lblCustomerMaster.Enabled = false;

                //Process Menu
                picProduction.Enabled = false;
                lblProduction.Enabled = false;

                if (Properties.Settings.Default.New_Page_Used == "Y")
                {
                    picProductionNew.Visible = true;
                    lblProductionNew.Visible = true;
                    picProductionNew.Enabled = true;
                    lblProductionNew.Enabled = true;

                    picPartLineMapping.Visible = true;
                    lblPartLineMapping.Visible = true;
                    picPartLineMapping.Enabled = true;
                    lblPartLineMapping.Enabled = true;

                    


                }
                else
                {
                    picProductionNew.Visible = false;
                    lblProductionNew.Visible = false;

                    picPartLineMapping.Visible = false;
                    lblPartLineMapping.Visible = false;
                }

                picReprint.Enabled = false;
                lblReprint.Enabled = false;

                picUploadPickList.Enabled = false;
                lblUploadPickList.Enabled = false;

                picLoadingList.Enabled = false;
                lblLoadingList.Enabled = false;
            }
            catch (Exception ex) { throw ex; }
        }


        #endregion

        private void picReport_Click(object sender, EventArgs e)
        {
            frmReport frm = new frmReport();
            frm.Show();
            frm.FormClosing += OFrm_FormClosing;
            this.Hide();
        }

        private void picProcessWiseReport_Click(object sender, EventArgs e)
        {
            frmProcessWiseReport frm = new frmProcessWiseReport();
            frm.Show();
            frm.FormClosing += OFrm_FormClosing;
            this.Hide();
        }

        private void picProductionNew_Click(object sender, EventArgs e)
        {
            frmProductionNew frm = new frmProductionNew();
            frm.Show();
            frm.FormClosing += OFrm_FormClosing;
            this.Hide();
        }

        private void picPartLineMapping_Click(object sender, EventArgs e)
        {
            frmLinePartMappingMaster frm = new frmLinePartMappingMaster();
            frm.Show();
            frm.FormClosing += OFrm_FormClosing;
            this.Hide();
        }

        private void picChildPartReport_Click(object sender, EventArgs e)
        {
            frmChildReport frm = new frmChildReport();
            frm.Show();
            frm.FormClosing += OFrm_FormClosing;
            this.Hide();
        }

        private void pickPLC_PartDetails_Click(object sender, EventArgs e)
        {
            frmEndLinePartDetails frm = new frmEndLinePartDetails();
            frm.Show();
            frm.FormClosing += OFrm_FormClosing;
            this.Hide();
        }
    }
}
