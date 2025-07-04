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
    [Activity(Label = "Dispatch_Offline", MainLauncher = true, WindowSoftInputMode = SoftInput.StateHidden, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class PassKeyActivity : Activity
    {
        clsGlobal clsGLB;
        EditText txtUser;
        EditText txtPassword;

        public PassKeyActivity()
        {
            try
            {
                clsGLB = new clsGlobal();
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
                SetContentView(Resource.Layout.activity_passkey);

                Button btnLogin = FindViewById<Button>(Resource.Id.btnLogin);
                btnLogin.Click += btnLogin_Click;

                txtUser = FindViewById<EditText>(Resource.Id.editUserid);
                txtUser.KeyPress += TxtUser_KeyPress; ;
                txtPassword = FindViewById<EditText>(Resource.Id.editPassword);
                txtPassword.KeyPress += TxtPassword_KeyPress;

                Button btnClear = FindViewById<Button>(Resource.Id.btnClear);
                btnClear.Click += (e, a) =>
                {
                    txtUser.Text = "";
                    txtPassword.Text = "";
                    txtUser.RequestFocus();
                };

                Button btnBack = FindViewById<Button>(Resource.Id.btnBack);
                btnBack.Click += (e, a) =>
                {
                    ShowConfirmBox("Do you want to exit", this);
                    //Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
                };

                if (File.Exists(Path.Combine(clsGlobal.FilePath, "UIDP.SYS")))
                    ReadServerIP();
                else
                    WriteServerIP();
                txtUser.RequestFocus();
            }
            catch (Exception ex)
            {
                clsGLB.ShowMessage(ex.Message, this, MessageTitle.ERROR);
            }
        }


        private void TxtPassword_KeyPress(object sender, View.KeyEventArgs e)
        {
            try
            {
                if (e.Event.Action == KeyEventActions.Down)
                {
                    if (e.KeyCode == Keycode.Enter)
                    {
                        btnLogin_Click(null, null);
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

        private void TxtUser_KeyPress(object sender, View.KeyEventArgs e)
        {
            try
            {
                if (e.Event.Action == KeyEventActions.Down)
                {
                    if (e.KeyCode == Keycode.Enter)
                    {
                        txtPassword.RequestFocus();
                        txtPassword.SelectAll();
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

        public override void OnBackPressed()
        {
            ShowConfirmBox("Do you want to exit", this);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateControls())
                {
                    if (txtUser.Text.Trim().ToUpper() == clsGlobal.Userid.ToUpper() && txtPassword.Text.Trim().ToUpper() == clsGlobal.Password.ToUpper())
                        OpenActivity(typeof(MainActivity));
                    else
                    {
                        Toast.MakeText(this, "Incorrect user Id or Password", ToastLength.Long).Show();
                        txtUser.Text = "";
                        txtPassword.Text = "";
                        txtUser.RequestFocus();
                    }
                }
            }
            catch (Exception ex)
            { clsGLB.ShowMessage(ex.Message, this, MessageTitle.ERROR); }
        }

        private bool ValidateControls()
        {
            try
            {
                bool IsValidate = true;

                if (string.IsNullOrEmpty(txtUser.Text.Trim()))
                {
                    Toast.MakeText(this, "Input User Id", ToastLength.Long).Show();
                    txtUser.RequestFocus();
                    IsValidate = false;
                }
                if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
                {
                    Toast.MakeText(this, "Input Password", ToastLength.Long).Show();
                    txtPassword.RequestFocus();
                    IsValidate = false;
                }
                return IsValidate;
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

        public bool ReadServerIP()
        {
            try
            {
                string filename = Path.Combine(clsGlobal.FilePath, "UIDP.SYS");
                FileInfo ServerFile = new FileInfo(filename);
                StreamReader ReadServer;
                string[] strCon;
                if (ServerFile.Exists == true)
                {
                    ReadServer = new StreamReader(filename);
                    strCon = ReadServer.ReadLine().Split("~");
                    if (strCon.Length >= 1)
                    {
                        clsGlobal.Userid = strCon[0];
                        clsGlobal.Password = strCon[1].Trim();
                    }
                    else
                    {
                        ReadServer.Close();
                        ReadServer = null;
                        ServerFile = null;
                        return false;
                    }
                    ReadServer.Close();
                    ReadServer = null;
                    ServerFile = null;
                    return true;
                }
                else
                {
                    ServerFile = null;
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
                //return false;
            }
        }

        public void WriteServerIP()
        {
            try
            {
                string filename = Path.Combine(clsGlobal.FilePath, "UIDP.SYS");
                StreamWriter WriteServer = new StreamWriter(filename, false);
                //WriteServer.WriteLine(ModInit.GstrServerIP + "~" + ModInit.GintServerPort);
                WriteServer.WriteLine("1" + "~" + "1");
                WriteServer.Close();
                WriteServer = null;
                clsGlobal.Userid = clsGlobal.Password = "1";
            }
            catch (Exception ex)
            {
                throw ex;
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
            //this.FinishAffinity();
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }
        void handllerCancelButton(object sender, DialogClickEventArgs e)
        {

        }

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

        void handleOkMessage(object sender, DialogClickEventArgs e)
        {
            try
            {
                //vibrator.Cancel();
                //StopPlayingSound();
            }
            catch (Exception ex)
            {
                clsGLB.ShowMessage(ex.Message, this, MessageTitle.ERROR);
            }
        }




    }
}