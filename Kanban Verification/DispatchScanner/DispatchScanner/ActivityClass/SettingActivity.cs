using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using IOCLAndroidApp;

namespace ScanAndSaveApp.ActivityClass
{
    [Activity(Label = "Setting", WindowSoftInputMode = SoftInput.StateHidden, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class SettingActivity : Activity
    {
        clsNetwork oNetwork;
        clsGlobal clsGLB;
        EditText editServerIP;
        EditText editPort;
        Spinner cmbSite;
        List<string> _ListSite = new List<string>();
        public SettingActivity()
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
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.activity_setting);

                cmbSite = FindViewById<Spinner>(Resource.Id.cmbSite);

                Button btnSave = FindViewById<Button>(Resource.Id.btnSave);
                btnSave.Click += BtnSave_Click;

                editServerIP = FindViewById<EditText>(Resource.Id.txtServerIP);
                editPort = FindViewById<EditText>(Resource.Id.txtPort);


                Button btnBack = FindViewById<Button>(Resource.Id.btnBack);
                btnBack.Click += (e, a) =>
                {
                    this.Finish();
                };

                _ListSite.Add("--Select--");
                _ListSite.Add("0-TEST");
                ArrayAdapter<string> arrayAdapter = new ArrayAdapter<string>(this, Resource.Layout.Spiner, _ListSite);
                cmbSite.Adapter = arrayAdapter;
                cmbSite.SetSelection(0);

                BindCombo();

                ReadSettingFile();

                editServerIP.RequestFocus();
            }
            catch (Exception ex)
            {
                clsGLB.ShowMessage(ex.Message, this, MessageTitle.ERROR);
            }
        }

        public override void OnBackPressed() { }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateControls())
                {

                    string folderPath = Path.Combine(clsGlobal.FilePath, clsGlobal.FileFolder);
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    string filename = Path.Combine(folderPath, clsGlobal.ServerIpFileName);

                    using (var streamWriter = new StreamWriter(filename, false))
                    {
                        streamWriter.WriteLine(editServerIP.Text.Trim());
                        streamWriter.WriteLine(editPort.Text.Trim());
                        streamWriter.WriteLine("LINE=" + cmbSite.SelectedItem.ToString());

                        MediaScannerConnection.ScanFile(this, new string[] { filename }, null, null);

                        Toast.MakeText(this, "Setting saved", ToastLength.Long).Show();

                        clsGlobal.mSockIp = editServerIP.Text.Trim();
                        clsGlobal.mSockPort = Convert.ToInt32(editPort.Text.Trim());

                        Finish();
                    }
                }
            }
            catch (Exception ex)
            { clsGLB.ShowMessage(ex.Message, this, MessageTitle.ERROR); }
        }

        private void ReadSettingFile()
        {
            StreamReader sr = null;
            try
            {
                string folderPath = Path.Combine(clsGlobal.FilePath, clsGlobal.FileFolder);
                string filename = Path.Combine(folderPath, clsGlobal.ServerIpFileName);

                if (File.Exists(filename))
                {
                    sr = new StreamReader(filename);
                    editServerIP.Text = sr.ReadLine();
                    editPort.Text = sr.ReadLine();
                    try
                    {
                        string Line = sr.ReadLine().Split('=')[1].Trim();
                        if (_ListSite.Count > 0)
                        {
                            int Index = 0;
                            for (int i = 0; i < _ListSite.Count; i++)
                            {
                                if (_ListSite[i].Trim() == Line)
                                {
                                    Index = i;
                                    break;
                                }
                            }
                            cmbSite.SetSelection(Index);
                        }
                        clsGlobal.LineId = Line.Split('-')[0].Trim();
                    }
                    catch (Exception exfil)
                    {

                    }
                    sr.Close();
                    sr.Dispose();
                    sr = null;

                    clsGlobal.mSockIp = editServerIP.Text.Trim();
                    clsGlobal.mSockPort = Convert.ToInt32(editPort.Text.Trim());
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

        private bool ValidateControls()
        {
            try
            {
                bool IsValidate = true;

                if (string.IsNullOrEmpty(editServerIP.Text.Trim()))
                {
                    Toast.MakeText(this, "Input server ip", ToastLength.Long).Show();
                    editServerIP.RequestFocus();
                    IsValidate = false;
                }
                if (string.IsNullOrEmpty(editPort.Text.Trim()))
                {
                    Toast.MakeText(this, "Input server port", ToastLength.Long).Show();
                    editPort.RequestFocus();
                    IsValidate = false;
                }
                if (cmbSite.SelectedItemPosition <= 0)
                {
                    Toast.MakeText(this, "Select Line", ToastLength.Long).Show();
                    IsValidate = false;
                }
                return IsValidate;
            }
            catch (Exception ex) { throw ex; }
        }

        private void BindCombo()
        {
            try
            {
                string _MESSAGE = "GET_LINE~" + "}";
                string[] _RESPONSE = oNetwork.fnSendReceiveData(_MESSAGE).Split('~');
                // string[] _RESPONSE = "VALID~1-LINE1~2-LINE2~3-LINE3".Split('~');
                switch (_RESPONSE[0])
                {
                    case "VALID":
                        for (int i = 1; i < _RESPONSE.Length; i++)
                        {
                            _ListSite.Add(_RESPONSE[i]);
                        }
                        ArrayAdapter<string> arrayAdapter = new ArrayAdapter<string>(this, Resource.Layout.Spiner, _ListSite);
                        cmbSite.Adapter = arrayAdapter;
                        if (_ListSite.Count > 0)
                            cmbSite.SetSelection(0);
                        break;

                    case "INVALID":
                        clsGLB.ShowMessage(_RESPONSE[1], this, MessageTitle.INFORMATION);
                        break;

                    case "ERROR":
                        clsGLB.ShowMessage(_RESPONSE[1], this, MessageTitle.INFORMATION);
                        break;

                    case "NO_CONNECTION":
                        clsGLB.ShowMessage("Communication server not connected", this, MessageTitle.INFORMATION);
                        break;

                    default:
                        clsGLB.ShowMessage("No option match from comm server", this, MessageTitle.INFORMATION);
                        break;

                }

            }
            catch (Exception ex)
            {
                clsGLB.ShowMessage(ex.Message, this, MessageTitle.ERROR);
            }
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
    }
}