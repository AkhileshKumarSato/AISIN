using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientCommunicationApp.Common;
using SATOPrinterAPI;

namespace ClientCommunicationApp
{

    class SatoPrinter
    {
        //public int PrintJobs { get { return SatoDriver.GetSpoolerPrintJobsNumber(Program.PrinterName); } }     

        // SATOPrinterAPI.Driver SatoDriver = null;
        // SATOPrinterAPI.Printer printer = null;
        Printer tcpPrinter = null;
        public SatoPrinter()
        {
            // SatoDriver = new Driver();
            //   printer = new Printer();
            tcpPrinter = new Printer();


            //var USBPorts = printer.GetUSBList();
            //if (USBPorts.Count > 0)
            //{
            //    printer.Interface = Printer.InterfaceType.USB;
            //    printer.USBPortID = USBPorts[0].PortID;
            //}
            //else
            //{
            FillTcpPTR();
            // throw new Exception("Printer not found.");
            // }

        }
        //public static string GetprinterStatus()
        //{
        //    string status = "Not Connected";
        //    SATOPrinterAPI.Printer.Status printerStatus = new Printer.Status();
        //    status = printerStatus.State;
        //    return status;
        //}
        public string QueryStatus()
        {
            byte[] qry = SATOPrinterAPI.Utils.StringToByteArray("");
            byte[] result = tcpPrinter.Query(qry);
            string status = SATOPrinterAPI.Utils.ByteArrayToString(result);
            return status;
        }

        public string GetprinterStatus()
        {
            string status = "Not Connected";

            try
            {
                tcpPrinter.Connect();
                //SATOPrinterAPI.Printer.Status printerStatus = tcpPrinter.GetPrinterStatus();
                //status = printerStatus.Code;
                //status = GetStatusMessages(status);
                //if (status.StartsWith("ONLINE"))
                //{
                    return "OK_" + status;
                //}
                //else
                //{
                //    return "NOT_OK_" + status;
                //}
            }
            catch (Exception ex)
            {

            }
            finally
            {
                tcpPrinter.Disconnect();
            }
            return status;
        }
        public void printProductCode(string PartNo,string Barcode)
        {
            string sbpl = "";
            try
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader("WHEELS_PRN_FILE.prn", Encoding.UTF8))
                {
                    sbpl = reader.ReadToEnd();
                    reader.Close();
                }
                sbpl = sbpl.Replace("{PRODUCTCODE}", PartNo);
                sbpl = sbpl.Replace("{BARCODE}", Barcode);
                //sbpl = sbpl.Replace("{BARCODE}", Properties.Settings.Default.VendorCode + " " + Properties.Settings.Default.Line + " " + productCode + " " + DateTime.Now.ToString("ddMMyy") + " " + "A" + "0083");

                if (sbpl == "")
                {
                    //  throw new InfoException("Printing data can not be empty.");
                }
                byte[] sbplByte = SATOPrinterAPI.Utils.StringToByteArray(sbpl);
                // SatoDriver.SendRawData(Program.PrinterName, sbplByte);
                FillTcpPTR();
                tcpPrinter.Send(sbplByte);

            }
            catch (Exception ex)
            {
                //ClsMessage.ShowError(ex.Message);
                throw new Exception("Printing Error : " + ex.Message);
            }
        }

        private void FillTcpPTR()
        {
            try
            {
                tcpPrinter.Interface = Printer.InterfaceType.TCPIP;
                tcpPrinter.TCPIPAddress = GlobalVar.mPrinterIP;
                tcpPrinter.TCPIPPort = "9100";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetPrinterStatusBeforePrinting()
        {
            try
            {
                //MessageBox.Show("before status");
                Printer.Status st = tcpPrinter.GetPrinterStatus();
                string status = GetStatusMessages(st.Code);
                if (status.StartsWith("ONLINE"))
                {
                    return "OK_" + status;
                }
                else
                {
                    return "NOT_OK_" + status;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                throw ex;
            }
        }
        private static string GetStatusMessages(string data)
        {
            switch (Convert.ToChar(data))//HexToInt(data)))
            {
                case '0':
                    return ("OFFLINE_STATE" + " : " + "STATUS_NO_ERROR");

                case '1':
                    return ("OFFLINE_STATE" + " : " + "STATUS_RIBBON_LABEL_NEAR_END");

                case '2':
                    return ("OFFLINE_STATE" + " : " + "STATUS_BUFFER_NEAR_FULL");

                case '3':
                    return ("OFFLINE_STATE" + " : " + "STATUS_RIBBON_LABEL_NEAR_END_BUFFER_NEAR_FULL");

                case '4':
                    return ("OFFLINE_STATE" + " : " + "STATUS_PRINTER_PAUSE");

                case 'A':
                    return ("ONLINE_STATE" + " : " + "STATUS_WAIT_TO_RECEIVE" + " : " + "STATUS_NO_ERROR");

                case 'B':
                    return ("ONLINE_STATE" + " : " + "STATUS_WAIT_TO_RECEIVE" + " : " + "STATUS_RIBBON_LABEL_NEAR_END");

                case 'C':
                    return ("ONLINE_STATE" + " : " + "STATUS_WAIT_TO_RECEIVE" + " : " + "STATUS_BUFFER_NEAR_FULL");

                case 'D':
                    return ("ONLINE_STATE" + " : " + "STATUS_WAIT_TO_RECEIVE" + " : " + "STATUS_RIBBON_LABEL_NEAR_END_BUFFER_NEAR_FULL");

                case 'E':
                    return ("ONLINE_STATE" + " : " + "STATUS_WAIT_TO_RECEIVE" + " : " + "STATUS_PRINTER_PAUSE");

                case 'G':
                    return ("ONLINE_STATE" + " : " + "STATUS_PRINTING");

                case 'H':
                    return ("ONLINE_STATE" + " : " + "STATUS_PRINTING" + " : " + "STATUS_RIBBON_LABEL_NEAR_END");

                case 'I':
                    return ("ONLINE_STATE" + " : " + "STATUS_PRINTING" + " : " + "STATUS_BUFFER_NEAR_FULL");

                case 'J':
                    return ("ONLINE_STATE" + " : " + "STATUS_PRINTING" + " : " + "STATUS_RIBBON_LABEL_NEAR_END_BUFFER_NEAR_FULL");

                case 'K':
                    return ("ONLINE_STATE" + " : " + "STATUS_PRINTING" + " : " + "STATUS_PRINTER_PAUSE");

                case 'M':
                    return ("ONLINE_STATE" + " : " + "STATUS_STANDBY");

                case 'N':
                    return ("ONLINE_STATE" + " : " + "STATUS_STANDBY" + " : " + "STATUS_RIBBON_LABEL_NEAR_END");

                case 'O':
                    return ("ONLINE_STATE" + " : " + "STATUS_STANDBY" + " : " + "STATUS_BUFFER_NEAR_FULL");

                case 'P':
                    return ("ONLINE_STATE" + " : " + "STATUS_STANDBY" + " : " + "STATUS_RIBBON_LABEL_NEAR_END_BUFFER_NEAR_FULL");

                case 'Q':
                    return ("ONLINE_STATE" + " : " + "STATUS_STANDBY" + " : " + "STATUS_PRINTER_PAUSE");

                case 'S':
                    return ("ONLINE_STATE" + " : " + "STATUS_ANALYZING");

                case 'T':
                    return ("ONLINE_STATE" + " : " + "STATUS_ANALYZING" + " : " + "STATUS_RIBBON_LABEL_NEAR_END");

                case 'U':
                    return ("ONLINE_STATE" + " : " + "STATUS_ANALYZING" + " : " + "STATUS_BUFFER_NEAR_FULL");

                case 'V':
                    return ("ONLINE_STATE" + " : " + "STATUS_ANALYZING" + " : " + "STATUS_RIBBON_LABEL_NEAR_END_BUFFER_NEAR_FULL");

                case 'W':
                    return ("ONLINE_STATE" + " : " + "STATUS_ANALYZING" + " : " + "STATUS_PRINTER_PAUSE");

                case 'b':
                    return ("ERROR_DETECTION" + " : " + "STATUS_HEAD_OPEN");

                case 'c':
                    return ("ERROR_DETECTION" + " : " + "STATUS_PAPER_END");

                case 'd':
                    return ("ERROR_DETECTION" + " : " + "STATUS_RIBBON_END");

                case 'e':
                    return ("ERROR_DETECTION" + " : " + "STATUS_MEDIA_ERROR");

                case 'f':
                    return ("ERROR_DETECTION" + " : " + "STATUS_SENSOR_ERROR");

                case 'g':
                    return ("ERROR_DETECTION" + " : " + "STATUS_HEAD_ERROR");

                case 'h':
                    return ("ERROR_DETECTION" + " : " + "STATUS_CUTTER_OPEN_ERROR");

                case 'i':
                    return ("ERROR_DETECTION" + " : " + "STATUS_CARD_ERROR");

                case 'j':
                    return ("ERROR_DETECTION" + " : " + "STATUS_CUTTER_ERROR");

                case 'k':
                    return ("ERROR_DETECTION" + " : " + "STATUS_OTHER_ERRORS");

                case 'o':
                    return ("ERROR_DETECTION" + " : " + "STATUS_OTHER_IC_TAG_ERROR");

                case 'q':
                    return ("ERROR_DETECTION" + " : " + "STATUS_BATTER_ERROR");
            }
            return "UNEXPECTED_VALUE";
        }
    }
}

