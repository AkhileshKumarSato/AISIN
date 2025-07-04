using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Media;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

using IOCLAndroidApp;
using IOCLAndroidApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SatoScanningApp
{
    [Activity(Label = "Sato ScanningApp", WindowSoftInputMode = SoftInput.StateHidden, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class RejectionWithPartActivity : AppCompatActivity
    {
        clsGlobal clsGLB;
        clsNetwork oNetwork;
        MediaPlayer mediaPlayerSound;
        Vibrator vibrator;

        EditText editBinBarcode, editPartBarcode, editReason;

        Button btnRejectPart, btnRejectBin, btnReset, btnBinQty;

        public RejectionWithPartActivity()
        {
            try
            {
                clsGLB = new clsGlobal();
                oNetwork = new clsNetwork();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
            }
        }

        #region Activity Events
        protected override void OnCreate(Bundle savedInstanceState)
        {

            try
            {
                base.OnCreate(savedInstanceState);
                // Set our view from the "main" layout resource
                SetContentView(Resource.Layout.activity_rejection_with_part);

                ImageView imgBack = FindViewById<ImageView>(Resource.Id.imgBack);
                imgBack.Click += (e, a) =>
                {
                    this.Finish();
                };

                TextView txtHeader = FindViewById<TextView>(Resource.Id.txtHeader);
                txtHeader.Text = "REJECTION WITH PART";

                editBinBarcode = FindViewById<EditText>(Resource.Id.editBinBarcode);
                editBinBarcode.KeyPress += editBinBarcode_KeyPress;

                editPartBarcode = FindViewById<EditText>(Resource.Id.editPartBarcode);
                editReason = FindViewById<EditText>(Resource.Id.editReason);

                btnRejectPart = FindViewById<Button>(Resource.Id.btnRejectPart);
                btnRejectPart.Click += btnRejectPart_Click;

                btnRejectBin = FindViewById<Button>(Resource.Id.btnRejectBin);
                btnRejectBin.Click += BtnRejectBin_Click;

                btnBinQty = FindViewById<Button>(Resource.Id.btnBinQty);
                btnBinQty.Text = "0";


                btnReset = FindViewById<Button>(Resource.Id.btnReset);
                btnReset.Click += BtnReset_Click;

                vibrator = this.GetSystemService(VibratorService) as Vibrator;
            }
            catch (Exception ex)
            {
                clsGLB.ShowMessage(ex.Message, this, MessageTitle.ERROR);
            }
        }

        #endregion

        #region Button Events
        private void btnRejectPart_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(editPartBarcode.Text) || editPartBarcode.Text.Trim() == "")
                {
                    StartPlayingSound();
                    ShowMessageBox("Please scan part barcode !!", this);
                    editPartBarcode.RequestFocus();
                    return;
                }
         
                if (string.IsNullOrEmpty(editReason.Text) || editReason.Text.Trim() == "")
                {
                    StartPlayingSound();
                    ShowMessageBox("Please input reason !!", this);
                    editReason.RequestFocus();
                    return;
                }
                //Popup Added by Akhilesh on 08-01-2021
                Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(this);
                Android.App.AlertDialog alert = dialog.Create();
                alert.SetTitle("Rejection");
                alert.SetMessage("Do you really want to reject ");
                alert.SetButton("Yes", (c, ev) =>
                {

                    SavePartRejectionDataAsync();
                });

                alert.SetButton2("NO", (c, ev) => { });
                alert.Show();
                return;
            }
            catch (Exception ex)
            {
                clsGLB.ShowMessage(ex.Message, this, MessageTitle.ERROR);
            }
        }
        private void BtnRejectBin_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(editBinBarcode.Text) || editBinBarcode.Text.Trim() == "")
                {
                    StartPlayingSound();
                    ShowMessageBox("Please scan bin barcode !!", this);
                    editBinBarcode.RequestFocus();
                    return;
                }
                if (string.IsNullOrEmpty(btnBinQty.Text) || btnBinQty.Text == "0" || Convert.ToInt32(btnBinQty.Text) == 0)
                {
                    StartPlayingSound();
                    ShowMessageBox("Bin Qty Not Found !!", this);
                    return;
                }
                if (string.IsNullOrEmpty(editReason.Text) || editReason.Text.Trim() == "")
                {
                    StartPlayingSound();
                    ShowMessageBox("Please input reason !!", this);
                    editReason.RequestFocus();
                    return;
                }
                //Popup Added by Akhilesh on 08-01-2021
                Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(this);
                Android.App.AlertDialog alert = dialog.Create();
                alert.SetTitle("Rejection");
                alert.SetMessage("Do you really want to reject ");
                alert.SetButton("Yes", (c, ev) =>
                {

                    SaveBinRejectionDataAsync();
                });

                alert.SetButton2("NO", (c, ev) => { });
                alert.Show();
                return;
                
            }
            catch (Exception ex)
            {
                clsGLB.ShowMessage(ex.Message, this, MessageTitle.ERROR);
            }
        }
        private void BtnReset_Click(object sender, EventArgs e)
        {
            try
            {
                editBinBarcode.Text = "";
                Clear();
                editBinBarcode.RequestFocus();
            }
            catch (Exception ex)
            {
                clsGLB.ShowMessage(ex.Message, this, MessageTitle.ERROR);
            }
        }

        #endregion

        #region Methods

 
        async Task<string> GetBinDataAsync()
        {
            var progressDialog = ProgressDialog.Show(this, "", "Please wait...", true);
            try
            {
                Clear();
                string[] _RESPONSE = await Task.Run(() => GetBinDataFromServer(editBinBarcode.Text.Trim()));

                progressDialog.Hide();

                switch (_RESPONSE[0])
                {
                    case "VALID":
                        btnBinQty.Text = _RESPONSE[1];
                        editPartBarcode.RequestFocus();
                        break;

                    case "INVALID":
                        editBinBarcode.Text = "";
                        editBinBarcode.RequestFocus();
                        StartPlayingSound();
                        ShowMessageBox(_RESPONSE[1], this);
                        break;

                    case "ERROR":
                        editBinBarcode.Text = "";
                        editBinBarcode.RequestFocus();
                        StartPlayingSound();
                        ShowMessageBox(_RESPONSE[1], this);
                        break;

                    case "NO_CONNECTION":
                        editBinBarcode.Text = "";
                        editBinBarcode.RequestFocus();
                        StartPlayingSound();
                        ShowMessageBox("Communication server not connected", this);
                        break;

                    default:
                        editBinBarcode.Text = "";
                        editBinBarcode.RequestFocus();
                        StartPlayingSound();
                        ShowMessageBox("No option match from comm server", this);
                        break;
                }
            }
            catch (Exception ex)
            {
                clsGLB.ShowMessage(ex.Message, this, MessageTitle.ERROR);
                progressDialog.Hide();
            }
            finally
            {
                progressDialog.Hide();
            }
            return "";
        }

        private string[] GetBinDataFromServer(string binbarcode)
        {
            try
            {
                string _MESSAGE = "GET_BIN_DATA~" + binbarcode + "}";
                string[] _RESPONSE = oNetwork.fnSendReceiveData(_MESSAGE).Split('~');
                return _RESPONSE;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
   
        async void SaveBinRejectionDataAsync()
        {
            var progressDialog = ProgressDialog.Show(this, "", "Please wait...", true);
            try
            {
                string[] _RESPONSE = await Task.Run(() => SendBinRejectionDataToServer());

                progressDialog.Hide();

                switch (_RESPONSE[0])
                {
                    case "VALID":
                        clsGLB.ShowMessage(_RESPONSE[1], this, MessageTitle.CONFIRM);
                        BtnReset_Click(null, null);
                        break;

                    case "INVALID":
                        StartPlayingSound();
                        ShowMessageBox(_RESPONSE[1], this);
                        break;

                    case "ERROR":
                        StartPlayingSound();
                        ShowMessageBox(_RESPONSE[1], this);
                        break;

                    case "NO_CONNECTION":
                        StartPlayingSound();
                        ShowMessageBox("Communication server not connected", this);
                        break;

                    default:
                        StartPlayingSound();
                        ShowMessageBox("No option match from comm server", this);
                        break;

                }
            }
            catch (Exception ex)
            {
                clsGLB.ShowMessage(ex.Message, this, MessageTitle.ERROR);
                progressDialog.Hide();
            }
            finally
            {
                progressDialog.Hide();
            }
        }

        private string[] SendBinRejectionDataToServer()
        {
            try
            {
                string _MESSAGE = "SAVE_BIN_REJECTION_DATA~" + editBinBarcode.Text.Trim() + "~" + editReason.Text.Trim() + "~"  + clsGlobal.UserId + "}";
                string[] _RESPONSE = oNetwork.fnSendReceiveData(_MESSAGE).Split('~');

                return _RESPONSE;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        async void SavePartRejectionDataAsync()
        {
            var progressDialog = ProgressDialog.Show(this, "", "Please wait...", true);
            try
            {
                string[] _RESPONSE = await Task.Run(() => SendPartRejectionDataToServer());

                progressDialog.Hide();

                switch (_RESPONSE[0])
                {
                    case "VALID":
                        clsGLB.ShowMessage(_RESPONSE[1], this, MessageTitle.CONFIRM);
                        BtnReset_Click(null, null);
                        break;

                    case "INVALID":
                        editPartBarcode.Text = "";
                        editPartBarcode.RequestFocus();
                        StartPlayingSound();
                        ShowMessageBox(_RESPONSE[1], this);
                        break;

                    case "ERROR":
                        editPartBarcode.Text = "";
                        editPartBarcode.RequestFocus();
                        StartPlayingSound();
                        ShowMessageBox(_RESPONSE[1], this);
                        break;

                    case "NO_CONNECTION":
                        editPartBarcode.Text = "";
                        editPartBarcode.RequestFocus();
                        StartPlayingSound();
                        ShowMessageBox("Communication server not connected", this);
                        break;

                    default:
                        editPartBarcode.Text = "";
                        editPartBarcode.RequestFocus();
                        StartPlayingSound();
                        ShowMessageBox("No option match from comm server", this);
                        break;

                }
            }
            catch (Exception ex)
            {
                clsGLB.ShowMessage(ex.Message, this, MessageTitle.ERROR);
                progressDialog.Hide();
            }
            finally
            {
                progressDialog.Hide();
            }
        }

        private string[] SendPartRejectionDataToServer()
        {
            try
            {
                string _MESSAGE = "SAVE_PART_REJECTION_DATA~" + editPartBarcode.Text.Trim() + "~" + editReason.Text.Trim() + "~" + clsGlobal.UserId + "}";
                string[] _RESPONSE = oNetwork.fnSendReceiveData(_MESSAGE).Split('~');

                return _RESPONSE;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ShowMessageBox(string msg, Activity activity)
        {
            Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(activity);
            builder.SetTitle("Message");
            builder.SetMessage(msg);
            builder.SetCancelable(false);
            builder.SetPositiveButton("Ok", handleOkMessage);
            builder.Show();
        }

        void handleOkMessage(object sender, DialogClickEventArgs e)
        {
            try
            {
                vibrator.Cancel();
                StopPlayingSound();
            }
            catch (Exception ex)
            {
                clsGLB.ShowMessage(ex.Message, this, MessageTitle.ERROR);
            }
        }
        private void StartPlayingSound()
        {
            try
            {
                //Start Vibration
                long[] pattern = { 0, 200, 0 }; //0 to start now, 200 to vibrate 200 ms, 0 to sleep for 0 ms.
                vibrator.Vibrate(pattern, 0);//

                StopPlayingSound();
                mediaPlayerSound = MediaPlayer.Create(this, Resource.Raw.Beep);
                mediaPlayerSound.Start();
            }
            catch (Exception ex) { throw ex; }
        }
        private void StopPlayingSound()
        {
            try
            {
                if (mediaPlayerSound != null)
                {
                    mediaPlayerSound.Stop();
                    mediaPlayerSound.Release();
                    mediaPlayerSound = null;
                }
            }
            catch (Exception ex) { throw ex; }
        }

        private void Clear()
        {
            try
            {
                editPartBarcode.Text = "";
                editReason.Text = "";
                btnBinQty.Text = "0";
            }
            catch (Exception ex)
            {
                clsGLB.ShowMessage(ex.Message, this, MessageTitle.ERROR);
            }
        }

        #endregion

        #region EditText Events

        private void editBinBarcode_KeyPress(object sender, View.KeyEventArgs e)
        {
            try
            {
                if (e.Event.Action == KeyEventActions.Down)
                {
                    if (e.KeyCode == Keycode.Enter)
                    {
                        if (editBinBarcode.Text.Trim().Length == 18)
                        {
                            GetBinDataAsync();
                        }
                        else
                        {
                            StartPlayingSound();
                            ShowMessageBox("Invalid Bin barcode " + editBinBarcode.Text.Trim(), this);
                            editBinBarcode.Text = "";
                            editBinBarcode.RequestFocus();
                        }
                    }
                    else
                        e.Handled = false;
                }
            }
            catch (Exception ex)
            {
                StartPlayingSound();
                ShowMessageBox("Error : " + ex.Message, this);
            }
        }
        public override void OnBackPressed()
        {
        }

        #endregion
    }
}