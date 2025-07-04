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
    public class DispatchActivity : AppCompatActivity
    {
        clsGlobal clsGLB;
        clsNetwork oNetwork;
        MediaPlayer mediaPlayerSound;
        Vibrator vibrator;
        List<PickList> _ListPickList = new List<PickList>();

        int _ScanQty = 0, _TotalQty = 0;

        Spinner spinnerPickListNo;
        EditText editBinBarcode;
        TextView txtMsg, txtTotalQty, txtScanQty;

        Button btnReset;

        RecyclerView recycleViewData;
        DispatchAdapter dispatchAdapter;
        RecyclerView.LayoutManager mLayoutManager;

        public DispatchActivity()
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
                SetContentView(Resource.Layout.activity_dispatch);

                ImageView imgBack = FindViewById<ImageView>(Resource.Id.imgBack);
                imgBack.Click += (e, a) =>
                {
                    this.Finish();
                };

                TextView txtHeader = FindViewById<TextView>(Resource.Id.txtHeader);
                txtHeader.Text = "DISPATCH";

                txtMsg = FindViewById<TextView>(Resource.Id.txtMsg);
                txtMsg.Text = "";

                spinnerPickListNo = FindViewById<Spinner>(Resource.Id.spinnerPickListNo);
                spinnerPickListNo.ItemSelected += SpinnerPickListNo_ItemSelected;

                editBinBarcode = FindViewById<EditText>(Resource.Id.editBinBarcode);
                editBinBarcode.KeyPress += editBinBarcode_KeyPress;

                txtTotalQty = FindViewById<TextView>(Resource.Id.txtTotalQty);
                txtTotalQty.Text = "Total Qty : 0";

                txtScanQty = FindViewById<TextView>(Resource.Id.txtScanQty);
                txtScanQty.Text = "Scan Qty : 0";

                btnReset = FindViewById<Button>(Resource.Id.btnReset);
                btnReset.Click += BtnReset_Click;

                recycleViewData = FindViewById<RecyclerView>(Resource.Id.recycleViewData);
                mLayoutManager = new LinearLayoutManager(this);
                recycleViewData.SetLayoutManager(mLayoutManager);

                BindRecycleView();

                GetPickListNoAsync();

                vibrator = this.GetSystemService(VibratorService) as Vibrator;
            }
            catch (Exception ex)
            {
                clsGLB.ShowMessage(ex.Message, this, MessageTitle.ERROR);
            }
        }

        #endregion

        #region Spinner Events
        private void SpinnerPickListNo_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            try
            {
                if (spinnerPickListNo.SelectedItemPosition > 0)
                {
                    Clear();
                    GetPickListDataAsync();
                }
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
                Clear();
                GetPickListNoAsync();
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
                        if (IsValid())
                            SaveDispatchDataAsync();
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

        #region RecycleView

        private void BindRecycleView()
        {
            try
            {
                dispatchAdapter = new DispatchAdapter(_ListPickList, this);
                recycleViewData.SetAdapter(dispatchAdapter);
            }
            catch (Exception ex)
            {
                clsGLB.ShowMessage(ex.Message, this, MessageTitle.ERROR);
            }
        }

        #endregion

        #region Methods
        async Task<string> GetPickListNoAsync()
        {
            var progressDialog = ProgressDialog.Show(this, "", "Please wait...", true);
            try
            {
                string[] _RESPONSE = await Task.Run(() => GetPickListNoFromServer());

                progressDialog.Hide();
                List<string> _ListPickListNo = new List<string>();
                switch (_RESPONSE[0])
                {
                    case "VALID":
                        _ListPickListNo.Add("--Select--");
                        for (int i = 1; i < _RESPONSE.Length; i++)
                        {
                            _ListPickListNo.Add(_RESPONSE[i]);
                        }
                        ArrayAdapter<string> arrayAdapter = new ArrayAdapter<string>(this, Resource.Layout.Spiner, _ListPickListNo);
                        spinnerPickListNo.Adapter = arrayAdapter;
                        spinnerPickListNo.SetSelection(0);
                        editBinBarcode.RequestFocus();
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
            return "";
        }

        private string[] GetPickListNoFromServer()
        {
            try
            {
                string _MESSAGE = "GET_DIS_PICKLIST_NO~" + "}";
                string[] _RESPONSE = oNetwork.fnSendReceiveData(_MESSAGE).Split('~');
                return _RESPONSE;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        async Task<string> GetPickListDataAsync()
        {
            var progressDialog = ProgressDialog.Show(this, "", "Please wait...", true);
            try
            {
                string[] _RESPONSE = await Task.Run(() => GetPickListDataFromServer());

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
                            _TotalQty += int.Parse(ArrCol[1]);
                            _ScanQty += int.Parse(ArrCol[2]);
                            _ListPickList.Add(new PickList
                            {
                                BackNo = ArrCol[0],
                                Qty = int.Parse(ArrCol[1]),
                                ScanQty = int.Parse(ArrCol[2])
                            });
                        }
                        txtTotalQty.Text = "Total Qty : " + _TotalQty.ToString();
                        txtScanQty.Text = "Scan Qty : " + _ScanQty.ToString();
                        editBinBarcode.RequestFocus();
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
                //Refresh the list
                dispatchAdapter.NotifyDataSetChanged();
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

        private string[] GetPickListDataFromServer()
        {
            try
            {
                string _MESSAGE = "GET_DIS_PICKLIST_NO_DATA~" + spinnerPickListNo.SelectedItem.ToString() + "}";
                string[] _RESPONSE = oNetwork.fnSendReceiveData(_MESSAGE).Split('~');
                return _RESPONSE;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        async void SaveDispatchDataAsync()
        {
            var progressDialog = ProgressDialog.Show(this, "", "Please wait...", true);
            try
            {
                txtMsg.Text = "";
                string[] _RESPONSE = await Task.Run(() => SendDispatchDataToServer());

                progressDialog.Hide();

                switch (_RESPONSE[0])
                {
                    case "VALID":
                        txtMsg.Text = _RESPONSE[1].Trim();
                        //Update Pick Qty
                        string BinBackNo = editBinBarcode.Text.Trim().Substring(editBinBarcode.Text.Trim().Length - 4);
                        int Index = _ListPickList.FindIndex(x => x.BackNo == BinBackNo);
                        _ListPickList[Index].ScanQty++;
                        _ScanQty++;
                        //PickList Complete
                        if (_ScanQty >= _TotalQty)
                        {
                            BtnReset_Click(null, null);
                            clsGLB.ShowMessage("Dispatch completed", this, MessageTitle.CONFIRM);
                        }
                        else
                        {
                            txtScanQty.Text = "Scan Qty : " + _ScanQty.ToString();
                            dispatchAdapter.NotifyDataSetChanged();
                            editBinBarcode.Text = "";
                            editBinBarcode.RequestFocus();
                        }
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
        }

        private string[] SendDispatchDataToServer()
        {
            try
            {
                string _MESSAGE = "DIS_SAVE_DATA~" + spinnerPickListNo.SelectedItem.ToString() + "~" + editBinBarcode.Text.Trim() + "~" + clsGlobal.UserId + "}";
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
                editBinBarcode.Text = "";
                txtMsg.Text = "";
                txtTotalQty.Text = "Total Qty : 0";
                txtScanQty.Text = "Scan Qty : 0";
                _ScanQty = 0;
                _TotalQty = 0;
                _ListPickList.Clear();
                dispatchAdapter.NotifyDataSetChanged();
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
                if (spinnerPickListNo.SelectedItemPosition <= 0)
                {
                    StartPlayingSound();
                    ShowMessageBox("Select PickList No", this);
                    spinnerPickListNo.RequestFocus();
                    return false;
                }
                if (_ListPickList.Count == 0)
                {
                    StartPlayingSound();
                    ShowMessageBox("PickList data not found", this);
                    return false;
                }
                if (editBinBarcode.Text.Trim() == "")
                {
                    StartPlayingSound();
                    ShowMessageBox("Scan Bin barcode", this);
                    editBinBarcode.RequestFocus();
                    return false;
                }
                if (editBinBarcode.Text.Trim().Length != 18)
                {
                    StartPlayingSound();
                    ShowMessageBox("Invalid Bin barcode length " + editBinBarcode.Text.Trim(), this);
                    editBinBarcode.Text = "";
                    editBinBarcode.RequestFocus();
                    return false;
                }

                return true;
            }
            catch (Exception ex) { throw ex; }
        }

        #endregion
    }
}