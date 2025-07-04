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
using SatoScanningApp.ActivityClass;
using System.Collections.Generic;
using Android.Media;
using IOCLAndroidApp.Models;

namespace SatoScanningApp
{
    [Activity(Label = "Sato ScanningApp", WindowSoftInputMode = SoftInput.StateHidden, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity
    {
        clsGlobal clsGLB;
        clsNetwork oNetwork;
        MediaPlayer mediaPlayerSound;
        Vibrator vibrator;
        Button btnRejectWithPart, btnRejectWithOutPart, btnFractionWithPart, btnFractionWithOutPart, btnFgFeeding, btnFGPicking, btnDirectDispatch, btnAfterPickingDispatch;
        public MainActivity()
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
                SetContentView(Resource.Layout.activity_main);

                ImageView imgBack = FindViewById<ImageView>(Resource.Id.imgBack);
                imgBack.Click += (e, a) =>
                {
                    this.Finish();
                };

                TextView txtHeader = FindViewById<TextView>(Resource.Id.txtHeader);
                txtHeader.Text = "MENU";

                btnRejectWithPart = FindViewById<Button>(Resource.Id.btnRejectWithPart);
                btnRejectWithPart.Click += btnRejectWithPart_Click;

                btnRejectWithOutPart = FindViewById<Button>(Resource.Id.btnRejectWithOutPart);
                btnRejectWithOutPart.Click += btnRejectWithOutPart_Click;

                btnFractionWithPart = FindViewById<Button>(Resource.Id.btnFractionWithPart);
                btnFractionWithPart.Click += btnFractionWithPart_Click;

                btnFractionWithOutPart = FindViewById<Button>(Resource.Id.btnFractionWithOutPart);
                btnFractionWithOutPart.Click += btnFractionWithOutPart_Click;

                btnFgFeeding = FindViewById<Button>(Resource.Id.btnFgFeeding);
                btnFgFeeding.Click += btnFgFeeding_Click;

                btnFGPicking = FindViewById<Button>(Resource.Id.btnFGPicking);
                btnFGPicking.Click += btnFGPicking_Click;

                btnDirectDispatch = FindViewById<Button>(Resource.Id.btnDirectDispatch);
                btnDirectDispatch.Click += BtnDirectDispatch_Click;

                btnAfterPickingDispatch = FindViewById<Button>(Resource.Id.btnAfterPickingDispatch);
                btnAfterPickingDispatch.Click += BtnAfterPickingDispatch_Click;

                if (clsGlobal.UserId.Trim().ToUpper() != "SATO")
                {
                    DisableMenus();
                    GetUserRight();
                }

                vibrator = this.GetSystemService(VibratorService) as Vibrator;
               
            }
            catch (Exception ex)
            {
                clsGLB.ShowMessage(ex.Message, this, MessageTitle.ERROR);
            }
        }

        public override void OnBackPressed()
        {
        }

        #endregion

        #region Button Events
        private void btnRejectWithPart_Click(object sender, EventArgs e)
        {
            try
            {
                OpenActivity(typeof(RejectionWithPartActivity));
            }
            catch (Exception ex)
            {
                clsGLB.ShowMessage(ex.Message, this, MessageTitle.ERROR);
            }
        }

        private void btnRejectWithOutPart_Click(object sender, EventArgs e)
        {
            try
            {
                OpenActivity(typeof(RejectionWithOutPartActivity));
            }
            catch (Exception ex)
            {
                clsGLB.ShowMessage(ex.Message, this, MessageTitle.ERROR);
            }
        }
        private void btnFractionWithPart_Click(object sender, EventArgs e)
        {
            try
            {
               OpenActivity(typeof(FractionWithPartActivity));
            }
            catch (Exception ex)
            {
                clsGLB.ShowMessage(ex.Message, this, MessageTitle.ERROR);
            }
        }
        private void btnFractionWithOutPart_Click(object sender, EventArgs e)
        {
            try
            {
                OpenActivity(typeof(FractionWithOutPartActivity));
            }
            catch (Exception ex)
            {
                clsGLB.ShowMessage(ex.Message, this, MessageTitle.ERROR);
            }
        }
        private void btnFgFeeding_Click(object sender, EventArgs e)
        {
            try
            {
                OpenActivity(typeof(FGFeedingActivity));
            }
            catch (Exception ex)
            {
                clsGLB.ShowMessage(ex.Message, this, MessageTitle.ERROR);
            }
        }

        private void btnFGPicking_Click(object sender, EventArgs e)
        {
            try
            {
                OpenActivity(typeof(FGPickingActivity));
            }
            catch (Exception ex)
            {
                clsGLB.ShowMessage(ex.Message, this, MessageTitle.ERROR);
            }
        }

        private void BtnAfterPickingDispatch_Click(object sender, EventArgs e)
        {
            try
            {
                OpenActivity(typeof(DispatchActivity));
            }
            catch (Exception ex)
            {
                clsGLB.ShowMessage(ex.Message, this, MessageTitle.ERROR);
            }
        }

        private void BtnDirectDispatch_Click(object sender, EventArgs e)
        {
            try
            {
                OpenActivity(typeof(DirectDispatchActivity));
            }
            catch (Exception ex)
            {
                clsGLB.ShowMessage(ex.Message, this, MessageTitle.ERROR);
            }
        }

        #endregion

        #region Methods

        private void DisableMenus()
        {
            try
            {
                btnRejectWithPart.Enabled = false;
                btnRejectWithOutPart.Enabled = false;
                btnFractionWithPart.Enabled = false;
                btnFractionWithOutPart.Enabled = false;
                btnFgFeeding.Enabled = false;
                btnFGPicking.Enabled = false;
                btnDirectDispatch.Enabled = false;
                btnAfterPickingDispatch.Enabled = false;
            }
            catch (Exception ex)
            {
                clsGLB.ShowMessage(ex.Message, this, MessageTitle.ERROR);
            }
        }
        private void GetUserRight()
        {
            try
            {
                string _MESSAGE = "GET_USER_RIGHT~" + clsGlobal.UserGroup + "}";
                string[] _RESPONSE = oNetwork.fnSendReceiveData(_MESSAGE).Split('~');
                switch (_RESPONSE[0])
                {
                    case "VALID":
                        string[] sArr = _RESPONSE[1].Split('#');
                        for (int i = 0; i < sArr.Length; i++)
                        {
                            switch (sArr[i])
                            {
                                case "301":
                                    btnRejectWithPart.Enabled = true;
                                    break;
                                case "302":
                                    btnRejectWithOutPart.Enabled = true;
                                    break;
                                case "303":
                                    btnFractionWithPart.Enabled = true;
                                    break;
                                case "304":
                                    btnFractionWithOutPart.Enabled = true;
                                    break;
                                case "305":
                                    btnFgFeeding.Enabled = true;
                                    break;
                                case "306":
                                    btnFGPicking.Enabled = true;
                                    break;
                                case "307":
                                    btnDirectDispatch.Enabled = true;
                                    break;
                                case "308":
                                    btnAfterPickingDispatch.Enabled = true;
                                    break;
                                default:
                                    break;
                            }
                        }
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
            }
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
    }
}