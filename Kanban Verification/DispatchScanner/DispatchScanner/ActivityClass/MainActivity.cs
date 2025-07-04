using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Content.PM;
using Android.Views;
using IOCLAndroidApp;
using Android.Content;
using System;
using System.IO;
using ScanAndSaveApp.ActivityClass;
using System.Collections.Generic;
using Honda_Device_Android;
using Android.Media;
using IOCLAndroidApp.Models;
using Android;
using System.Threading.Tasks;

namespace ScanAndSaveApp
{
    [Activity(Label = "Kanban Verification", WindowSoftInputMode = SoftInput.StateHidden, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity
    {
        clsGlobal clsGLB;
        // clsNetwork oNetwork;
        int _iScanCount = 0;
        List<string> _listScanCase;

        EditText txtAHlkanban;
        EditText txtCustomerKanban;
        EditText txtFrameNo;
        TextView lblScanCount;
        TextView lblLastScan;
        MediaPlayer mediaPlayerSound;
        Vibrator vibrator;

        const int RequestId = 1;

        readonly string[] PermissionsGroup =
            {
                            //TODO add more permissions
                            Manifest.Permission.ReadExternalStorage,
                            Manifest.Permission.WriteExternalStorage,
             };
        private void allowThePermission()
        {
            RequestPermissions(PermissionsGroup, RequestId);
        }
        async void GetPermissionDataAsync(object sender, DialogClickEventArgs e)
        {
            var progressDialog = ProgressDialog.Show(this, "", "Please wait...", true);
            try
            {
                await Task.Run(() => allowThePermission());
            }
            catch (Exception)
            {
                progressDialog.Hide();
                throw;
            }
            finally
            {
                progressDialog.Hide();
            }


        }
        public MainActivity()
        {
            try
            {
                clsGLB = new clsGlobal();
                //oNetwork = new clsNetwork();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
            }
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                // Set our view from the "main" layout resource
                SetContentView(Resource.Layout.activity_main);

                if ((int)Build.VERSION.SdkInt >= 23)
                {
                    allowThePermission();
                }

                Button btnClear = FindViewById<Button>(Resource.Id.btnClear);
                btnClear.Click += BtnServerSetting_Click;

                Button btnExit = FindViewById<Button>(Resource.Id.btnExit);
                btnExit.Click += BtnExit_Click;

                txtCustomerKanban = FindViewById<EditText>(Resource.Id.editTruckNo);
                //txtCustomerKanban.FocusChange += txtCustomerKanban_FocusChange;
                txtCustomerKanban.KeyPress += txtCustomerKanban_KeyPress;

                lblLastScan = FindViewById<TextView>(Resource.Id.lblLastScan1);

                txtAHlkanban = FindViewById<EditText>(Resource.Id.editDeliveryNo);
                txtAHlkanban.KeyPress += txtAHlkanban_KeyPress;
                //txtAHlkanban.FocusChange += txtAHlkanban_FocusChange;

                txtFrameNo = FindViewById<EditText>(Resource.Id.editFrameNo);
                txtFrameNo.KeyPress += txtFrameNo_KeyPress;

                lblScanCount = FindViewById<TextView>(Resource.Id.lblScanneCount);

                vibrator = this.GetSystemService(VibratorService) as Vibrator;

                _listScanCase = new List<string>();

                ReadmappingFile();
                txtAHlkanban.RequestFocus();
            }
            catch (Exception ex)
            {
                clsGLB.ShowMessage(ex.Message, this, MessageTitle.ERROR);
            }
        }

        //private void txtAHlkanban_FocusChange(object sender, View.FocusChangeEventArgs e)
        //{
        //    try
        //    {
        //        if (txtAHlkanban.Text.Length > 0)
        //        {
        //            //if ((txtAHlkanban.Text.Trim().Length != 8))
        //            //{
        //            //    Toast.MakeText(this, "Invalid Delivery No.", ToastLength.Long).Show();//View.SetBackgroundColor(Android.Graphics.Color.Red);
        //            //}
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        clsGLB.ShowMessage(ex.Message, this, MessageTitle.ERROR);
        //    }
        //}

        private void txtAHlkanban_KeyPress(object sender, View.KeyEventArgs e)
        {
            try
            {
                if (e.Event.Action == KeyEventActions.Down)
                {
                    if (e.KeyCode == Keycode.Enter)
                    {
                      
                        try
                        {
                            if (!string.IsNullOrEmpty(txtAHlkanban.Text))
                            {

                                AHLBackNo = "";
                                CustpartNo = "";
                                ArrAHLkanban = txtAHlkanban.Text.Split(' ');
                                if (ArrAHLkanban.Length >= 10)
                                { 
                                    int index = 0;
                                    while (index < ArrAHLkanban.Length)
                                    { 
                                        if (dicMapping.ContainsKey(ArrAHLkanban[index].ToString()))
                                        {
                                            CustpartNo = dicMapping[ArrAHLkanban[index].ToString()].ToString();
                                            AHLBackNo = ArrAHLkanban[index].ToString();
                                            break;

                                        }
                                        index++;
                                        
                                    }
                                    
                                    txtCustomerKanban.Text = "";
                                    txtCustomerKanban.RequestFocus();
                                }
                                if (AHLBackNo.Length == 0 && CustpartNo.Length == 0)
                                {
                                    StartPlayingSound();
                                    ShowMessageBox("Invalid AHL kanabn", this);
                                    txtAHlkanban.Text = "";
                                    txtCustomerKanban.Text = "";
                                    txtAHlkanban.RequestFocus();
                                    return;
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            StartPlayingSound();
                            ShowMessageBox(ex.Message, this);
                        }

                    }
                    else
                        e.Handled = false;
                }
            }
            catch (Exception ex)
            {
                clsGLB.ShowMessage(ex.Message, this, MessageTitle.ERROR);
            }
        }

        private void txtCustomerKanban_KeyPress(object sender, View.KeyEventArgs e)
        {
            try
            {
                if (e.Event.Action == KeyEventActions.Down)
                {
                    if (e.KeyCode == Keycode.Enter)
                    {
                        try
                        {
                            string CustomerPartno = "";
                            if (!string.IsNullOrEmpty(txtAHlkanban.Text))
                            {
                                if (!string.IsNullOrEmpty(txtCustomerKanban.Text))
                                {
                                    Arrcustkanban = txtCustomerKanban.Text.Split(' ');

                                    int index = 0;
                                    while (index < Arrcustkanban.Length)
                                    {
                                        if (dicMapping.ContainsValue(Arrcustkanban[index].ToString()))
                                        {
                                            CustomerPartno = Arrcustkanban[index].ToString();
                                            break;

                                        }
                                        index++;
                                    }
                                  
                                    if (!string.IsNullOrEmpty(CustomerPartno))
                                    {
                                        if (CustpartNo.ToUpper() == CustomerPartno.ToUpper())
                                        {
                                            lblLastScan.Text = "Kanban Match";
                                            WriteFile(AHLBackNo.Trim(), CustomerPartno.Trim(), "Match");
                                            txtAHlkanban.Text = "";
                                            txtCustomerKanban.Text = "";
                                            txtAHlkanban.RequestFocus();
                                        }
                                        else
                                        {
                                            StartPlayingSound();
                                            ShowMessageBox("Kanban Not Match", this);
                                           // WriteFile(AHLBackNo.Trim(), CustomerPartno.Trim(), "Not Match");
                                            txtAHlkanban.Text = "";
                                            txtCustomerKanban.Text = "";
                                            txtAHlkanban.RequestFocus();
                                        }
                                    }
                                    else
                                    {
                                        StartPlayingSound();
                                        ShowMessageBox("Kanban Not Match", this);
                                       // WriteFile(AHLBackNo.Trim(), CustomerPartno.Trim(), "Not Match");
                                        txtAHlkanban.Text = "";
                                        txtCustomerKanban.Text = "";
                                        txtAHlkanban.RequestFocus();
                                    }
                                }
                                else
                                    e.Handled = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            StartPlayingSound();
                            ShowMessageBox(ex.Message, this);
                        }

                    }
                    else
                        e.Handled = false;
                }

            }
            catch (Exception ex)
            {
                clsGLB.ShowMessage(ex.Message, this, MessageTitle.ERROR);
            }
        }

        //private void txtCustomerKanban_FocusChange(object sender, View.FocusChangeEventArgs e)
        //{
        //    try
        //    {
        //        //if (txtCustomerKanban.Text.Length == 4)
        //        //{
        //        //    if (!string.IsNullOrEmpty(txtCustomerKanban.Text))
        //        //    {
        //        //        clsGlobal.CaseFileName =  txtCustomerKanban.Text + ".txt";
        //        //        ReadCaseFile();
        //        //    }
        //        //}
        //        //else
        //        //    Toast.MakeText(this, "Invalid Truck No.", ToastLength.Long).Show();
        //    }
        //    catch (Exception ex)
        //    {
        //        clsGLB.ShowMessage(ex.Message, this, MessageTitle.ERROR);
        //    }
        //}

        private void txtCustomerKanban_Click(object sender, EventArgs e)
        {

        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            try
            {
                this.Finish();
            }
            catch (Exception ex)
            {
                clsGLB.ShowMessage(ex.Message, this, MessageTitle.ERROR);
            }
        }

        private void BtnServerSetting_Click(object sender, EventArgs e)
        {
            try
            {
                Clear();
            }
            catch (Exception ex)
            {
                clsGLB.ShowMessage(ex.Message, this, MessageTitle.ERROR);
            }
        }
        string CustpartNo = "";
        string AHLBackNo = "";
        string[] ArrAHLkanban;
        string[] Arrcustkanban;
        private void txtFrameNo_KeyPress(object sender, View.KeyEventArgs e)
        {
            
        }

        #region Methods
        public void ShowMessageBox(string msg, Activity activity)
        {
            Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(activity);
            builder.SetTitle("Message");
            builder.SetMessage(msg);
            builder.SetCancelable(false);
            builder.SetPositiveButton("Ok", handleOkMessage);
            // builder.SetNegativeButton("No", handllerCancelButton);
            builder.Show();
        }




        public void WriteFile(string AHLBackNo, string CustomerpartNo, string result)
        {
            try
            {
                DateTime d = DateTime.Now;
                string date = DateTime.Now.ToString("ddMMyy");
                string Files = date.ToString();

                if (!Directory.Exists(clsGlobal.mDeviceRootDir))
                {
                    Directory.CreateDirectory(clsGlobal.mDeviceRootDir);
                }

                if (!File.Exists(Path.Combine(clsGlobal.mDeviceRootDir, Files + clsGlobal.FileName)))
                {
                    File.Create(Path.Combine(clsGlobal.mDeviceRootDir, Files + clsGlobal.FileName)).Dispose();

                    using (StreamWriter writer1 = new StreamWriter(Path.Combine(clsGlobal.mDeviceRootDir, Files + clsGlobal.FileName), append: true))
                    {
                        writer1.WriteLine("AHL BackNo" + ',' + "Customer Part No." + ',' + "Result" + ',' + "Date" + ',' + "User" + ',');
                        writer1.WriteLine(AHLBackNo + ',' + CustomerpartNo + ',' + result + ',' + DateTime.Now + ',' + clsGlobal.Userid + ',');
                        //writer1.WriteLine("{0}{1}{2}{3}{4}","AHLBackNo","CustomerpartNo","Result","Date","User");
                        //writer1.WriteLine("{0}{1}{2}{3}{4}",AHLBackNo , CustomerpartNo , result , DateTime.Now ,clsGlobal.Userid);
                        writer1.Dispose();
                    }

                }
                else if (File.Exists(Path.Combine(clsGlobal.mDeviceRootDir, Files + clsGlobal.FileName)))
                {
                    using (StreamWriter writer1 = new StreamWriter(Path.Combine(clsGlobal.mDeviceRootDir, Files + clsGlobal.FileName), append: true))
                    {
                        writer1.WriteLine(AHLBackNo + ',' + CustomerpartNo + ',' + result + ',' + DateTime.Now + ',' + clsGlobal.Userid + ',');
                        //writer1.WriteLine("{0}{1}{2}{3}{4}",AHLBackNo, CustomerpartNo, result, DateTime.Now, clsGlobal.Userid);
                        writer1.Dispose();

                    }
                    //MediaScannerConnection.ScanFile(Android.App.Application.Context, new string[] { Files + FileName }, null, null);
                    // MediaScannerConnection.ScanFile(this, new String[] { Path.Combine(mDeviceRootDir ) }, null, null);
                }
                _iScanCount++;
                lblScanCount.Text = _iScanCount.ToString();
                MediaScannerConnection.ScanFile(this, new String[] { clsGlobal.mDeviceRootDir, Files + clsGlobal.FileName }, null, null);
            }
            catch (Exception ex)
            {
                throw ex;

            }
            
        }


        //private void WriteFile(string AHLBackNo, string CustomerpartNo, string result)
        //{
        //    StreamWriter sw = null;
        //    try
        //    {
        //        string folderPath = Path.Combine(clsGlobal.FilePath, clsGlobal.FileFolder);
        //        //if (!Directory.Exists(folderPath))
        //        //    Directory.CreateDirectory(folderPath);

        //        //string filename = Path.Combine(folderPath, clsGlobal.CaseFileName + DateTime.Now.ToString("dd-MMM-yy")+".csv");
        //        //sw = new StreamWriter(filename, true);

        //        //sw.WriteLine(AHLBackNo, CustomerpartNo, result, DateTime.Now, clsGlobal.Userid);



        //        if (!File.Exists(Path.Combine(folderPath)))
        //        {
        //           // File.Create(Path.Combine(folderPath)).Dispose();

        //            using (StreamWriter writer1 = new StreamWriter(Path.Combine(folderPath), append: true))
        //            {
        //                writer1.WriteLine("AHLBackNo" + ',' + "CustomerpartNo" + ',' + "Result" + ',' + "Date" + ',' + "User" + ',');
        //                writer1.WriteLine(AHLBackNo + ',' + CustomerpartNo + ',' + result + ',' + DateTime.Now + ',' + clsGlobal.Userid + ',');
        //                writer1.Dispose();
        //            }

        //        }
        //        else if (File.Exists(Path.Combine(folderPath)))
        //        {
        //            using (StreamWriter writer1 = new StreamWriter(Path.Combine(folderPath), append: true))
        //            {
        //                writer1.WriteLine(AHLBackNo + ',' + CustomerpartNo + ',' + result + ',' + DateTime.Now + ',' + clsGlobal.Userid + ',');
        //                writer1.Dispose();

        //            }
        //            //MediaScannerConnection.ScanFile(Android.App.Application.Context, new string[] { Files + FileName }, null, null);
        //            // MediaScannerConnection.ScanFile(this, new String[] { Path.Combine(mDeviceRootDir ) }, null, null);
        //        }




        //        _iScanCount++;
        //        lblScanCount.Text = _iScanCount.ToString();

        //        MediaScannerConnection.ScanFile(this, new String[] { folderPath }, null, null);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (sw != null)
        //        {
        //            sw.Flush();
        //            sw.Close();
        //            sw = null;
        //        }
        //    }
        //}

        private bool ReadSettingFile()
        {
            StreamReader sr = null;
            try
            {
                string folderPath = Path.Combine(clsGlobal.FilePath, clsGlobal.FileFolder);
                string filename = Path.Combine(folderPath, clsGlobal.ServerIpFileName);

                if (File.Exists(filename))
                {
                    sr = new StreamReader(filename);
                    //clsGlobal.mSockIp = sr.ReadLine();
                    //clsGlobal.mSockPort = Convert.ToInt32(sr.ReadLine());

                    try
                    {
                        string Line = sr.ReadLine().Split('=')[1].Trim();
                        clsGlobal.LineId = Line.Split('-')[0].Trim();
                    }
                    catch (Exception exfil)
                    {

                    }

                    sr.Close();
                    sr.Dispose();
                    sr = null;

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            { throw ex; }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                    sr.Dispose();
                    sr = null;
                }
            }
        }

        private void ReadCaseFile()
        {
            StreamReader sr = null;
            _iScanCount = 0;
            try
            {
                string folderPath = Path.Combine(clsGlobal.FilePath, clsGlobal.FileFolder);
                string filename = Path.Combine(folderPath, clsGlobal.CaseFileName);

                if (File.Exists(filename))
                {
                    sr = new StreamReader(filename);
                    while (!sr.EndOfStream)
                    {
                        string[] str = sr.ReadLine().ToString().Split('\t');
                        _listScanCase.Add(str[2].Trim().ToUpper());
                        _iScanCount++;
                    }

                    lblScanCount.Text = _iScanCount.ToString();

                    sr.Close();
                    sr.Dispose();
                    sr = null;
                }
                else
                {
                    _iScanCount = 0;
                    _listScanCase.Clear();
                    lblScanCount.Text = _iScanCount.ToString();
                }

            }
            catch (Exception ex)
            { throw ex; }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                    sr.Dispose();
                    sr = null;
                }
            }
        }

        public void ShowConfirmBox(string msg, Activity activity)
        {
            Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(activity);
            builder.SetTitle("Message");
            builder.SetMessage(msg);
            builder.SetCancelable(false);
            builder.SetPositiveButton("Yes", handllerOkButton);
            builder.SetNegativeButton("No", handllerCancelButton);
            builder.Show();
        }
        void handllerOkButton(object sender, DialogClickEventArgs e)
        {
            this.FinishAffinity();
        }
        void handllerCancelButton(object sender, DialogClickEventArgs e)
        {

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

        public void OpenActivity(Type t)
        {
            try
            {
                Intent MenuIntent = new Intent(this, t);
                StartActivity(MenuIntent);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        public override void OnBackPressed()
        {
            //ShowConfirmBox("Do you want to exit", this);
        }

        private void Clear()
        {
            txtAHlkanban.Text = "";
            txtFrameNo.Text = "";
            txtCustomerKanban.Text = "";
            txtAHlkanban.RequestFocus();
        }

        //public class Foo
        //{
        //    // obviously you find meaningful names of the 2 properties

        //    public string BackNo { get; set; }
        //    public string AHLPartNo { get; set; }
        //    public string CustomerPartNo { get; set; }
        //}
        //List<Foo> _mapping = new List<Foo>();

        Dictionary<string, string> dicMapping = new Dictionary<string, string>();
        public bool ReadmappingFile()
        {
            try
            {
                dicMapping.Clear();
                string filename = Path.Combine(clsGlobal.FilePath + "/" + clsGlobal.FileFolder, "PARTMASTER.csv");
                FileInfo ServerFile = new FileInfo(filename);

                if (ServerFile.Exists == true)
                {
                    StreamReader ReadServer = new StreamReader(filename);
                    while (!ReadServer.EndOfStream)
                    {
                        var line = ReadServer.ReadLine();
                        var values = line.Split(',');
                        if (values.Length == 2)
                            //_mapping.Add(new Foo { BackNo = values[0], AHLPartNo = values[1], CustomerPartNo = values[2] });
                            dicMapping.Add(values[1], values[0]);
                    }

                    ReadServer.Close();
                    ReadServer = null;
                    ServerFile = null;

                    if (dicMapping.Count > 0)
                        return true;
                    else
                        return false;
                }
                else
                {
                    ServerFile = null;
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {

            }
        }

        Dictionary<string, string> dicBackNo = new Dictionary<string, string>();
        public bool ReadBackNoFile()
        {
            try
            {
                dicBackNo.Clear();
                string filename = Path.Combine(clsGlobal.FilePath, "PARTMASTER.csv");
                FileInfo ServerFile = new FileInfo(filename);

                if (ServerFile.Exists == true)
                {
                    StreamReader ReadServer = new StreamReader(filename);
                    while (!ReadServer.EndOfStream)
                    {
                        var line = ReadServer.ReadLine();
                        var values = line.Split(',');
                        if (values.Length == 2)
                            //_mapping.Add(new Foo { BackNo = values[0], AHLPartNo = values[1], CustomerPartNo = values[2] });
                            dicBackNo.Add(values[1], values[1]);
                    }

                    ReadServer.Close();
                    ReadServer = null;
                    ServerFile = null;

                    if (dicBackNo.Count > 0)
                        return true;
                    else
                        return false;
                }
                else
                {
                    ServerFile = null;
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {

            }
        }


    }
}