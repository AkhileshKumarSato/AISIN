using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Media;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

using IOCLAndroidApp;
using IOCLAndroidApp.Models;
using SatoScanningApp.Adapter;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SatoScanningApp
{
    [Activity(Label = "Sato ScanningApp", WindowSoftInputMode = SoftInput.StateHidden, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class FGFeedingActivity : AppCompatActivity
    {
        clsGlobal clsGLB;
        clsNetwork oNetwork;
        MediaPlayer mediaPlayerSound;
        Vibrator vibrator;
        List<FgFeeding> _ListFgFeeding = new List<FgFeeding>();

        EditText editBinBarcode, editLocation;
        TextView txtMsg;

        Button btnReset;

        RecyclerView recycleViewLocation;
        FGFeedingAdapter fgFeedingAdapter;
        RecyclerView.LayoutManager mLayoutManager;

        public FGFeedingActivity()
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
                SetContentView(Resource.Layout.activity_fgfeeding);

                ImageView imgBack = FindViewById<ImageView>(Resource.Id.imgBack);
                imgBack.Click += (e, a) =>
                {
                    this.Finish();
                };

                TextView txtHeader = FindViewById<TextView>(Resource.Id.txtHeader);
                txtHeader.Text = "FG FEEDING";

                txtMsg = FindViewById<TextView>(Resource.Id.txtMsg);
                txtMsg.Text = "";

                editBinBarcode = FindViewById<EditText>(Resource.Id.editBinBarcode);
                editBinBarcode.KeyPress += editBinBarcode_KeyPress;

                editLocation = FindViewById<EditText>(Resource.Id.editLocation);
                editLocation.KeyPress += editLocation_KeyPress;

                btnReset = FindViewById<Button>(Resource.Id.btnReset);
                btnReset.Click += BtnReset_Click;

                recycleViewLocation = FindViewById<RecyclerView>(Resource.Id.recycleViewLocation);
                mLayoutManager = new LinearLayoutManager(this);
                recycleViewLocation.SetLayoutManager(mLayoutManager);

                BindRecycleView();

                vibrator = this.GetSystemService(VibratorService) as Vibrator;
            }
            catch (Exception ex)
            {
                clsGLB.ShowMessage(ex.Message, this, MessageTitle.ERROR);
            }
        }

        #endregion

        #region Button Events

        private void BtnReset_Click(object sender, EventArgs e)
        {
            try
            {
                editBinBarcode.Text = "";
                Clear();
                fgFeedingAdapter.NotifyDataSetChanged();
                editBinBarcode.RequestFocus();
            }
            catch (Exception ex)
            {
                clsGLB.ShowMessage(ex.Message, this, MessageTitle.ERROR);
            }
        }

        #endregion

        #region Methods
        private void BindRecycleView()
        {
            try
            {
                fgFeedingAdapter = new FGFeedingAdapter(_ListFgFeeding, this);
                recycleViewLocation.SetAdapter(fgFeedingAdapter);
            }
            catch (Exception ex)
            {
                clsGLB.ShowMessage(ex.Message, this, MessageTitle.ERROR);
            }
        }
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
                        //Get the rows
                        string[] ArrRow = _RESPONSE[1].Split("#");
                        for (int i = 0; i < ArrRow.Length; i++)
                        {
                            //get the columns from the row
                            string[] ArrCol = ArrRow[i].Split("$");
                            _ListFgFeeding.Add(new FgFeeding
                            {
                                LocationCode = ArrCol[0],
                                Capacity = int.Parse(ArrCol[1]),
                                AvailableQty = int.Parse(ArrCol[2])
                            });
                        }
                        editLocation.RequestFocus();
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
                //Refresh the list
                fgFeedingAdapter.NotifyDataSetChanged();
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
                string _MESSAGE = "GET_FGFEEDING_BIN_DATA~" + binbarcode + "}";
                string[] _RESPONSE = oNetwork.fnSendReceiveData(_MESSAGE).Split('~');
                return _RESPONSE;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        async void SaveFeedingDataAsync()
        {
            var progressDialog = ProgressDialog.Show(this, "", "Please wait...", true);
            try
            {
                txtMsg.Text = "";
                string[] _RESPONSE = await Task.Run(() => SendFeedingDataToServer());

                progressDialog.Hide();

                switch (_RESPONSE[0])
                {
                    case "VALID":
                        BtnReset_Click(null, null);
                        txtMsg.Text = _RESPONSE[1];
                        break;

                    case "INVALID":
                        editLocation.Text = "";
                        editLocation.RequestFocus();
                        StartPlayingSound();
                        ShowMessageBox(_RESPONSE[1], this);
                        break;

                    case "ERROR":
                        editLocation.Text = "";
                        editLocation.RequestFocus();
                        StartPlayingSound();
                        ShowMessageBox(_RESPONSE[1], this);
                        break;

                    case "NO_CONNECTION":
                        editLocation.Text = "";
                        editLocation.RequestFocus();
                        StartPlayingSound();
                        ShowMessageBox("Communication server not connected", this);
                        break;

                    default:
                        editLocation.Text = "";
                        editLocation.RequestFocus();
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

        private string[] SendFeedingDataToServer()
        {
            try
            {
                string _MESSAGE = "SAVE_FGFEEDING_BIN_DATA~" + editBinBarcode.Text.Trim() + "~" + editLocation.Text.Trim() + "~" + clsGlobal.UserId + "}";
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
                editLocation.Text = "";
                txtMsg.Text = "";
                _ListFgFeeding.Clear();
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

        private void editLocation_KeyPress(object sender, View.KeyEventArgs e)
        {
            try
            {
                if (e.Event.Action == KeyEventActions.Down)
                {
                    if (e.KeyCode == Keycode.Enter)
                    {
                        if (editBinBarcode.Text.Trim() == "")
                        {
                            StartPlayingSound();
                            ShowMessageBox("Scan Bin barcode", this);
                            editLocation.Text = "";
                            editBinBarcode.RequestFocus();
                            return;
                        }
                        if (_ListFgFeeding.Count==0)
                        {
                            StartPlayingSound();
                            ShowMessageBox("Location back no mapping data not found", this);
                            editLocation.Text = "";
                            editBinBarcode.RequestFocus();
                            return;
                        }
                        if (editLocation.Text.Trim() == "")
                        {
                            StartPlayingSound();
                            ShowMessageBox("Scan location barcode", this);
                            editLocation.Text = "";
                            editBinBarcode.RequestFocus();
                            return;
                        }
                        //Check valid location is scanned or not
                        var LocationData = _ListFgFeeding.Find(x => x.LocationCode.ToUpper() == editLocation.Text.Trim().ToUpper());
                        if (LocationData == null)
                        {
                            StartPlayingSound();
                            ShowMessageBox("Location code does not belong to back", this);
                            editLocation.Text = "";
                            editLocation.RequestFocus();
                            return;
                        }
                        if (LocationData.AvailableQty <= 0)
                        {
                            StartPlayingSound();
                            ShowMessageBox("Location is full", this);
                            editLocation.Text = "";
                            editLocation.RequestFocus();
                            return;
                        }
                        SaveFeedingDataAsync();
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