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
    public class FGPickingActivity : AppCompatActivity
    {
        clsGlobal clsGLB;
        clsNetwork oNetwork;
        MediaPlayer mediaPlayerSound;
        Vibrator vibrator;
        List<PickList> _ListPickList = new List<PickList>();

        int _ScanQty = 0, _TotalQty = 0;
        string _CustomerPartNo = "";

        Spinner spinnerPickListNo;
        EditText editBinBarcode, editLocation, editCustomerKanban;
        TextView txtMsg, txtTotalQty, txtScanQty;

        Button btnReset;

        RecyclerView recycleViewPicking;
        FGPickingAdapter pickingAdapter;
        RecyclerView.LayoutManager mLayoutManager;

        public FGPickingActivity()
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
                SetContentView(Resource.Layout.activity_fgPicking);

                ImageView imgBack = FindViewById<ImageView>(Resource.Id.imgBack);
                imgBack.Click += (e, a) =>
                {
                    this.Finish();
                };

                TextView txtHeader = FindViewById<TextView>(Resource.Id.txtHeader);
                txtHeader.Text = "FG PICKING";

                txtMsg = FindViewById<TextView>(Resource.Id.txtMsg);
                txtMsg.Text = "";

                spinnerPickListNo = FindViewById<Spinner>(Resource.Id.spinnerPickListNo);
                spinnerPickListNo.ItemSelected += SpinnerPickListNo_ItemSelected;

                editBinBarcode = FindViewById<EditText>(Resource.Id.editBinBarcode);
                editBinBarcode.KeyPress += editBinBarcode_KeyPress;

                editLocation = FindViewById<EditText>(Resource.Id.editLocation);
                editLocation.KeyPress += editLocation_KeyPress;

                editCustomerKanban = FindViewById<EditText>(Resource.Id.editCustomerKanban);
                editCustomerKanban.KeyPress += editCustomerKanban_KeyPress;

                txtTotalQty = FindViewById<TextView>(Resource.Id.txtTotalQty);
                txtTotalQty.Text = "Total Qty : 0";

                txtScanQty = FindViewById<TextView>(Resource.Id.txtScanQty);
                txtScanQty.Text = "Scan Qty : 0";

                btnReset = FindViewById<Button>(Resource.Id.btnReset);
                btnReset.Click += BtnReset_Click;

                recycleViewPicking = FindViewById<RecyclerView>(Resource.Id.recycleViewPicking);
                mLayoutManager = new LinearLayoutManager(this);
                recycleViewPicking.SetLayoutManager(mLayoutManager);

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
                        if (IsValidForBin())
                        {
                            if (editCustomerKanban.Enabled)
                                ValidateBinAsync();
                            else
                                SaveFgPickingDataAsync();
                        }
                        else
                            editBinBarcode.Text = "";
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

        private void editCustomerKanban_KeyPress(object sender, View.KeyEventArgs e)
        {
            try
            {
                if (e.Event.Action == KeyEventActions.Down)
                {
                    if (e.KeyCode == Keycode.Enter)
                    {
                        if (IsValidForBin())
                        {
                            if (editCustomerKanban.Enabled)
                            {
                                if (editCustomerKanban.Text.Trim() == "")
                                {
                                    StartPlayingSound();
                                    ShowMessageBox("Scan Customer kanban", this);
                                    editCustomerKanban.RequestFocus();
                                    return;
                                }
                            }
                            /*
                             * Check length to identify customer type. Currently only toyota and honda export kanban barcode
                             * will be used, others are not scannable.Honda export barcode has only customer part no barcode.
                             * while toyota has other information also.
                             * Toyota Barcode - 11 A285AA3A5434 68610-0K080-00 00001/00002,A285A,A,,2020012101,IL09,30,A3,01,Z,PC   -  17
                             * Here 68610-0K080-00 is customer part no which will be validated.
                             * Honda Export - 67410T9C K000M3
                             * Here 67410T9C K000M3 is customer part no but KOOO will be replace as K011
                             */
                            //Means Honda Export barcode
                            if (editCustomerKanban.Text.Trim().Length == 15)
                            {
                                string CustomerPartNo = editCustomerKanban.Text.Trim().Replace("K000", "K011");
                                //Validate Bin and customer kanban belong to same customer part no
                                if (_CustomerPartNo == CustomerPartNo)
                                {
                                    SaveFgPickingDataAsync();
                                }
                                else
                                {
                                    StartPlayingSound();
                                    ShowMessageBox("Customer kanban and bin belong to different customer part no, bin customer part no is " + _CustomerPartNo + " and kanban customer part no is " + CustomerPartNo, this);
                                    editCustomerKanban.Text = "";
                                    editCustomerKanban.RequestFocus();
                                }
                            }
                            else //Assuming Toyota Barcode
                            {
                                string[] ArrToyotaKanban = editCustomerKanban.Text.Trim().Split(' ');
                                if (ArrToyotaKanban.Length >= 3)
                                {
                                    if (_CustomerPartNo == ArrToyotaKanban[2])
                                    {
                                        SaveFgPickingDataAsync();
                                    }
                                    else
                                    {
                                        StartPlayingSound();
                                        ShowMessageBox("Customer kanban and bin belong to different customer part no, bin customer part no is " + _CustomerPartNo + " and kanban customer part no is " + ArrToyotaKanban[2], this);
                                        editCustomerKanban.Text = "";
                                        editCustomerKanban.RequestFocus();
                                    }
                                }
                                else
                                {
                                    StartPlayingSound();
                                    ShowMessageBox("Invalid customer kanban", this);
                                    editCustomerKanban.Text = "";
                                    editCustomerKanban.RequestFocus();
                                }
                            }
                        }
                        else
                            editCustomerKanban.Text = "";
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
                        if (IsValidForLocation())
                            editBinBarcode.RequestFocus();
                        else
                            editLocation.Text = "";
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
                pickingAdapter = new FGPickingAdapter(_ListPickList, this);
                pickingAdapter.ItemClick += PickingAdapter_ItemClick;
                recycleViewPicking.SetAdapter(pickingAdapter);
            }
            catch (Exception ex)
            {
                clsGLB.ShowMessage(ex.Message, this, MessageTitle.ERROR);
            }
        }

        private void PickingAdapter_ItemClick(object sender, int e)
        {
            try
            {
                clsGlobal.SelectedBackNo = pickingAdapter.lstItem[e].BackNo;
                if (clsGlobal.SelectedBackNo == null || clsGlobal.SelectedBackNo == "")
                {
                    clsGLB.ShowMessage("Back no is not valid " + clsGlobal.SelectedBackNo, this, MessageTitle.INFORMATION);
                    return;
                }
                Intent MenuIntent = new Intent(this, typeof(FGPickBackLocationActivity));
                StartActivity(MenuIntent);
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
                        editLocation.RequestFocus();
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
                string _MESSAGE = "GET_PICK_PICKLIST_NO~" + "}";
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
                                ScanQty = int.Parse(ArrCol[2]),
                                IsCustomerKanbanEnable = Convert.ToBoolean(ArrCol[3])
                            });
                        }
                        txtTotalQty.Text = "Total Qty : " + _TotalQty.ToString();
                        txtScanQty.Text = "Scan Qty : " + _ScanQty.ToString();
                        editLocation.RequestFocus();
                        //Customer Kanban scanning is depend on customer master mapping
                        if (_ListPickList.Count > 0)
                            editCustomerKanban.Enabled = _ListPickList[0].IsCustomerKanbanEnable;
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
                pickingAdapter.NotifyDataSetChanged();
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
                string _MESSAGE = "GET_PICK_PICKLIST_NO_DATA~" + spinnerPickListNo.SelectedItem.ToString() + "}";
                string[] _RESPONSE = oNetwork.fnSendReceiveData(_MESSAGE).Split('~');
                return _RESPONSE;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        async void ValidateBinAsync()
        {
            var progressDialog = ProgressDialog.Show(this, "", "Please wait...", true);
            try
            {
                txtMsg.Text = "";
                _CustomerPartNo = "";
                string[] _RESPONSE = await Task.Run(() => SendValidateBinDataToServer());

                progressDialog.Hide();

                switch (_RESPONSE[0])
                {
                    case "VALID":
                        _CustomerPartNo = _RESPONSE[1].Trim();
                        editCustomerKanban.RequestFocus();
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

        private string[] SendValidateBinDataToServer()
        {
            try
            {
                string _MESSAGE = "FGPICK_VALIDATE_BIN~" + spinnerPickListNo.SelectedItem.ToString() + "~" + editBinBarcode.Text.Trim() + "~" + editLocation.Text.Trim() + "~" + editCustomerKanban.Text.Trim() + "~" + clsGlobal.UserId + "}";
                string[] _RESPONSE = oNetwork.fnSendReceiveData(_MESSAGE).Split('~');

                return _RESPONSE;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        async void SaveFgPickingDataAsync()
        {
            var progressDialog = ProgressDialog.Show(this, "", "Please wait...", true);
            try
            {
                txtMsg.Text = "";
                string[] _RESPONSE = await Task.Run(() => SendFgPickingDataToServer());

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
                            clsGLB.ShowMessage("Picklist completed", this, MessageTitle.CONFIRM);
                        }
                        else
                        {
                            txtScanQty.Text = "Scan Qty : " + _ScanQty.ToString();
                            pickingAdapter.NotifyDataSetChanged();
                            editLocation.Text = "";
                            editBinBarcode.Text = "";
                            editCustomerKanban.Text = "";
                            editLocation.RequestFocus();
                        }
                        break;

                    case "INVALID":
                        if (editCustomerKanban.Enabled)
                        {
                            editCustomerKanban.Text = "";
                            editCustomerKanban.RequestFocus();
                        }
                        else
                        {
                            editBinBarcode.Text = "";
                            editBinBarcode.RequestFocus();
                        }
                        StartPlayingSound();
                        ShowMessageBox(_RESPONSE[1], this);
                        break;

                    case "ERROR":
                        if (editCustomerKanban.Enabled)
                        {
                            editCustomerKanban.Text = "";
                            editCustomerKanban.RequestFocus();
                        }
                        else
                        {
                            editBinBarcode.Text = "";
                            editBinBarcode.RequestFocus();
                        }
                        StartPlayingSound();
                        ShowMessageBox(_RESPONSE[1], this);
                        break;

                    case "NO_CONNECTION":
                        if (editCustomerKanban.Enabled)
                        {
                            editCustomerKanban.Text = "";
                            editCustomerKanban.RequestFocus();
                        }
                        else
                        {
                            editBinBarcode.Text = "";
                            editBinBarcode.RequestFocus();
                        }
                        StartPlayingSound();
                        ShowMessageBox("Communication server not connected", this);
                        break;

                    default:
                        if (editCustomerKanban.Enabled)
                        {
                            editCustomerKanban.Text = "";
                            editCustomerKanban.RequestFocus();
                        }
                        else
                        {
                            editBinBarcode.Text = "";
                            editBinBarcode.RequestFocus();
                        }
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

        private string[] SendFgPickingDataToServer()
        {
            try
            {
                bool IsKanban = editCustomerKanban.Enabled;
                string _MESSAGE = "FGPICK_SAVE_DATA~" + spinnerPickListNo.SelectedItem.ToString() + "~" + editBinBarcode.Text.Trim() + "~" + editLocation.Text.Trim() + "~" + editCustomerKanban.Text.Trim() + "~" + clsGlobal.UserId + "~" + IsKanban + "}";
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
                editBinBarcode.Text = "";
                editCustomerKanban.Text = "";
                editCustomerKanban.Enabled = true;
                txtMsg.Text = "";
                txtTotalQty.Text = "Total Qty : 0";
                txtScanQty.Text = "Scan Qty : 0";
                _ScanQty = 0;
                _TotalQty = 0;
                _CustomerPartNo = "";
                _ListPickList.Clear();
                pickingAdapter.NotifyDataSetChanged();
            }
            catch (Exception ex)
            {
                clsGLB.ShowMessage(ex.Message, this, MessageTitle.ERROR);
            }
        }

        private bool IsValidForLocation()
        {
            try
            {
                if (editLocation.Text.Trim() == "")
                {
                    StartPlayingSound();
                    ShowMessageBox("Scan Location barcode", this);
                    editLocation.Text = "";
                    editLocation.RequestFocus();
                    return false;
                }
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
                    editLocation.Text = "";
                    return false;
                }
                return true;
            }
            catch (Exception ex) { throw ex; }
        }

        private bool IsValidForBin()
        {
            try
            {
                //Validate Location validation
                if (IsValidForLocation() == false) return false;
                //Then move to other validation
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
                //Check bin barcode back no is same as picklist back no list(last 4 digit back no)
                string BinBackNo = editBinBarcode.Text.Trim().Substring(editBinBarcode.Text.Trim().Length - 4);
                var BinBackNoData = _ListPickList.Find(x => x.BackNo == BinBackNo);
                if (BinBackNoData == null)
                {
                    StartPlayingSound();
                    ShowMessageBox("Bin back no " + BinBackNo + " does not belong to pickllist", this);
                    editBinBarcode.Text = "";
                    editBinBarcode.RequestFocus();
                    return false;
                }
                //Check qty is not completed
                if (BinBackNoData.ScanQty >= BinBackNoData.Qty)
                {
                    StartPlayingSound();
                    ShowMessageBox("Pickling complete for back no " + BinBackNo, this);
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