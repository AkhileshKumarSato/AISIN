using ClientCommunicationApp.Common;
using ClientCommunicationApp.Model;
using SatoLib;
using SatoLib.Communication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace ClientCommunicationApp
{
    public partial class winPassword : Window
    {
       
        public winPassword()
        {
            InitializeComponent();
            /* Enable Logging */
            GlobalVar.SetLoggerSetting();
        }
         
        #region Window Events
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                txtPass.Password = "";
                txtPass.Focus();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); GlobalVar.Logger.LogMessage(EventNotice.EventTypes.evtError, "Window_Loaded", ex.ToString()); }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
               
            }
            catch (Exception ex) { GlobalVar.Logger.LogMessage(EventNotice.EventTypes.evtError, "Window_Closing", ex.ToString()); }
        }

        #endregion

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                    if (txtPass.Password == Properties.Settings.Default.Password)
                    {
                        this.Close();
                    }
                    else
                    {
                    lblMessage.Content = "Invalid Password";
                    MessageBox.Show("Invalid Password");
                    txtPass.Password = "";
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
    }
}
