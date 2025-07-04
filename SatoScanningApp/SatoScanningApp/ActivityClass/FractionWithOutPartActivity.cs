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
    public class FractionWithOutPartActivity : AppCompatActivity
    {
        clsGlobal clsGLB;
        clsNetwork oNetwork;
        MediaPlayer mediaPlayerSound;
        Vibrator vibrator;
        int _FractionBinScanQty = 0;

        EditText editBinBarcode, editFractionBinBarcode, editPickQty;

        Button btnStandardBinQty, btnCurrentBinQty, btnSave, btnReset;

        public FractionWithOutPartActivity()
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
                SetContentView(Resource.Layout.activity_fraction_without_part);

                ImageView imgBack = FindViewById<ImageView>(Resource.Id.imgBack);
                imgBack.Click += (e, a) =>
                {
                    this.Finish();
                };

                TextView txtHeader = FindViewById<TextView>(Resource.Id.txtHeader);
                txtHeader.Text = "FRACTION WITHOUT PART";

                editBinBarcode = FindViewById<EditText>(Resource.Id.editBinBarcode);
                editBinBarcode.KeyPress += editBinBarcode_KeyPress;

                editFractionBinBarcode = FindViewById<EditText>(Resource.Id.editFractionBinBarcode);
                editFractionBinBarcode.KeyPress += editFractionBinBarcode_KeyPress;

                editPickQty = FindViewById<EditText>(Resource.Id.editPickQty);

                btnStandardBinQty = FindViewById<Button>(Resource.Id.btnStandardBinQty);
                btnStandardBinQty.Text = "0";

                btnCurrentBinQty = FindViewById<Button>(Resource.Id.btnCurrentBinQty);
                btnCurrentBinQty.Text = "0";

                btnSave = FindViewById<Button>(Resource.Id.btnSave);
                btnSave.Click += btnSave_Click;

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

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValid())
                {
                    if (string.IsNullOrEmpty(editPickQty.Text) || editPickQty.Text == "0" || Convert.ToInt32(editPickQty.Text) <= 0)
                    {
                        StartPlayingSound();
                        ShowMessageBox("Input pick qty !!", this);
                        editPickQty.RequestFocus();
                        return;
                    }
                    if (Convert.ToInt32(editPickQty.Text) > _FractionBinScanQty)
                    {
                        StartPlayingSound();
                        ShowMessageBox("Pick Qty can not be greater than available qty " + _FractionBinScanQty.ToString(), this);
                        editPickQty.Text = "";
                        editPickQty.RequestFocus();
                        return;
                    }
                    if (Convert.ToInt32(editPickQty.Text) + Convert.ToInt32(btnCurrentBinQty.Text) > Convert.ToInt32(btnStandardBinQty.Text))
                    {
                        StartPlayingSound();
                        ShowMessageBox("Bin qty can not be greater than standard qty ", this);
                        editPickQty.Text = "";
                        editPickQty.RequestFocus();
                        return;
                    }

                    //Popup Added by Akhilesh on 08-01-2021
                    Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(this);
                    Android.App.AlertDialog alert = dialog.Create();
                    alert.SetTitle("Fraction");
                    alert.SetMessage("Do you really want to fraction ");
                    alert.SetButton("Yes", (c, ev) =>
                    {
                        SaveFractionBinDataAsync();
                    });

                    alert.SetButton2("NO", (c, ev) => { });
                    alert.Show();
                    return;
                }
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
                            // GetBinDataAsync();
                            CheckBinBarcodeAsync();
                        }
                        else
                        {
                            StartPlayingSound();
                            ShowMessageBox("Invalid Bin barcode length " + editBinBarcode.Text.Trim(), this);
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

        private void editFractionBinBarcode_KeyPress(object sender, View.KeyEventArgs e)
        {
            try
            {
                if (e.Event.Action == KeyEventActions.Down)
                {
                    if (e.KeyCode == Keycode.Enter)
                    {
                        if (IsValid())
                            GetFractionBinDataAsync();
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
                        btnStandardBinQty.Text = _RESPONSE[1];
                        btnCurrentBinQty.Text = _RESPONSE[2];
                        editFractionBinBarcode.RequestFocus();
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
                string _MESSAGE = "GET_FRACTION_NEW_BIN_DATA~" + binbarcode + "}";
                string[] _RESPONSE = oNetwork.fnSendReceiveData(_MESSAGE).Split('~');
                return _RESPONSE;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        async Task<string> GetFractionBinDataAsync()
        {
            var progressDialog = ProgressDialog.Show(this, "", "Please wait...", true);
            try
            {
                _FractionBinScanQty = 0;
                editPickQty.Text = "";
                string[] _RESPONSE = await Task.Run(() => GetFractionBinDataFromServer(editFractionBinBarcode.Text.Trim()));

                progressDialog.Hide();

                switch (_RESPONSE[0])
                {
                    case "VALID":
                        _FractionBinScanQty = Convert.ToInt32(_RESPONSE[1]);
                        editPickQty.RequestFocus();
                        break;

                    case "INVALID":
                        editFractionBinBarcode.Text = "";
                        editFractionBinBarcode.RequestFocus();
                        StartPlayingSound();
                        ShowMessageBox(_RESPONSE[1], this);
                        break;

                    case "ERROR":
                        editFractionBinBarcode.Text = "";
                        editFractionBinBarcode.RequestFocus();
                        StartPlayingSound();
                        ShowMessageBox(_RESPONSE[1], this);
                        break;

                    case "NO_CONNECTION":
                        editFractionBinBarcode.Text = "";
                        editFractionBinBarcode.RequestFocus();
                        StartPlayingSound();
                        ShowMessageBox("Communication server not connected", this);
                        break;

                    default:
                        editFractionBinBarcode.Text = "";
                        editFractionBinBarcode.RequestFocus();
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

        private string[] GetFractionBinDataFromServer(string binbarcode)
        {
            try
            {
                string _MESSAGE = "GET_FRACTION_BIN_DATA~" + binbarcode + "}";
                string[] _RESPONSE = oNetwork.fnSendReceiveData(_MESSAGE).Split('~');
                return _RESPONSE;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        async void SaveFractionBinDataAsync()
        {
            var progressDialog = ProgressDialog.Show(this, "", "Please wait...", true);
            try
            {
                string[] _RESPONSE = await Task.Run(() => SendFractionBinDataToServer());

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

        private string[] SendFractionBinDataToServer()
        {
            try
            {
                string _MESSAGE = "SAVE_FRACTION_BIN_DATA~" + editBinBarcode.Text.Trim() + "~" + editFractionBinBarcode.Text.Trim() + "~" + editPickQty.Text.Trim() + "~" + clsGlobal.UserId + "}";
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
                editFractionBinBarcode.Text = "";
                editPickQty.Text = "";
                btnStandardBinQty.Text = "0";
                btnCurrentBinQty.Text = "0";
                _FractionBinScanQty = 0;
            }
            catch (Exception ex)
            {
                clsGLB.ShowMessage(ex.Message, this, MessageTitle.ERROR);
            }
        }

        private bool IsValid()
        {
            try
            {
                if (string.IsNullOrEmpty(editBinBarcode.Text) || editBinBarcode.Text.Trim() == "")
                {
                    StartPlayingSound();
                    ShowMessageBox("Please scan bin barcode !!", this);
                    editBinBarcode.RequestFocus();
                    return false;
                }
                if (string.IsNullOrEmpty(btnStandardBinQty.Text) || btnStandardBinQty.Text == "0" || Convert.ToInt32(btnStandardBinQty.Text) == 0)
                {
                    StartPlayingSound();
                    ShowMessageBox("Bin Qty Not Found !!", this);
                    return false;
                }
                if (string.IsNullOrEmpty(editFractionBinBarcode.Text) || editFractionBinBarcode.Text.Trim() == "")
                {
                    StartPlayingSound();
                    ShowMessageBox("Please scan fraction bin barcode !!", this);
                    editFractionBinBarcode.RequestFocus();
                    return false;
                }
                if (editFractionBinBarcode.Text.Trim().Length != 18)
                {
                    StartPlayingSound();
                    ShowMessageBox("Invalid fraction bin barcode length " + editFractionBinBarcode.Text.Trim(), this);
                    editFractionBinBarcode.Text = "";
                    editFractionBinBarcode.RequestFocus();
                    return false;

                }
                if (editFractionBinBarcode.Text.Trim()==editBinBarcode.Text.Trim())
                {
                    StartPlayingSound();
                    ShowMessageBox("New bin and fraction bin can not be same", this);
                    editFractionBinBarcode.Text = "";
                    editFractionBinBarcode.RequestFocus();
                    return false;

                }
                //Check oth bin belong to same back no
                string NewBinBackNo = editBinBarcode.Text.Trim().Substring(editBinBarcode.Text.Trim().Length - 4);
                string FractionBinBackNo = editFractionBinBarcode.Text.Trim().Substring(editFractionBinBarcode.Text.Trim().Length - 4);
                if (NewBinBackNo != FractionBinBackNo)
                {
                    StartPlayingSound();
                    ShowMessageBox("Fraction bin back no " + FractionBinBackNo + " is different from new bin back no " + NewBinBackNo, this);
                    editFractionBinBarcode.Text = "";
                    editFractionBinBarcode.RequestFocus();
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Added By Akhilesh on 20-01-2021 (This fuction will check bin barcode is print by  PC or AEP )
        async Task<string> CheckBinBarcodeAsync()
        {
            var progressDialog = ProgressDialog.Show(this, "", "Please wait...", true);
            try
            {
                Clear();
                string[] _RESPONSE = await Task.Run(() => CheckBinBarcodeFromServer(editBinBarcode.Text.Trim()));

                progressDialog.Hide();

                switch (_RESPONSE[0])
                {
                    case "VALID":
                        GetBinDataAsync();
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

        private string[] CheckBinBarcodeFromServer(string binbarcode)
        {
            try
            {
                string _MESSAGE = "CHECK_BIN_BARCODE~" + binbarcode + "}";
                string[] _RESPONSE = oNetwork.fnSendReceiveData(_MESSAGE).Split('~');
                return _RESPONSE;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}