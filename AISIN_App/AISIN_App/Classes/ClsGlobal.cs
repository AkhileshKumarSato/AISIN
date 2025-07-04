using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AISIN_App
{
    public enum EnumDbType { SELECT, INSERT, UPDATE, DELETE, SELECTBYID, SEARCH, VALIDATEUSER, UPDATEPASSWORD, KANBAN, PART, REJECT, PRINTSERVICEPART, PRINTFRACTIONPART, COMPLETE,LINE, BACKNO, LOADINGSLIP, TRUCKNO, DISPATCHID, PROCESS,LINEMAPPED, CHILD_PART_REPORT, PLC_PartDetails };
    public enum EnumProductionStatus { FRACTION = 1, COMPLETE = 2, SERVICE_PART = 3, REJECT = 4 };
    public enum EnumTrolleyStatus { Receive = 0, Dispatch = 1 };
    public enum EnumAppType { DESKTOPAPP, COMMSERVER, ANDROIDAPP };
    public enum EnumMachiningStatus { Machining = 1, FinalPacking = 2 };
    public enum EnumFinalPackingStatus { FinalPacking = 1, Dispatch = 2 };
    public enum EnumDashBoardName { TROLLEYCOUNT, PRODUCTION, INVENTORY, RECEIVING_PENDING }

    public class ClsGlobal
    {
        #region Static Variables

        public static string AppName { get; set; } = "AISIN";
        public static string UserId { get; set; }
        public static string UserName { get; set; }
        public static string Shift { get; set; }
        public static string LineNo { get; set; }
        public static string Barcode { get; set; }
        public static string PrintStatus { get; set; }


        public static string PCL_Used { get; set; }
       


        public static string BinBarcode { get; set; }

        public static int ProductionDisableTimeInSeconds { get; set; }
        public static string UserGroup { get; set; }
        public static string BinBarcodePrnName = "BINBARCODE.prn";
        public static string LocationPrnName = "LOCATION.prn";

        #endregion

        #region Static Methods
        public static void ClearMessage(Label label)
        {
            label.Text = "";
            label.Visible = false;
        }
        public static void SetErrorMessage(string Message, Label label)
        {
            label.Text = "ERROR : " + Message;
            label.BackColor = System.Drawing.Color.Red;
            label.Visible = true;
        }
        public static void SetInfoMessage(string Message, Label label)
        {
            label.Text = "INFO : " + Message;
            label.BackColor = System.Drawing.Color.Red;
            label.Visible = true;
        }
        public static void SetSuccessMessage(string Message, Label label)
        {
            label.Text = "SUCCESS : " + Message;
            label.BackColor = System.Drawing.Color.Green;
            label.Visible = true;
        }
        public static void ShowErrorMessageBox(string Message)
        {
            MessageBox.Show(Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public static void ShowInfoMessageBox(string Message)
        {
            MessageBox.Show(Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        public static void ShowConfirmMessageBox(string Message)
        {
            MessageBox.Show(Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void ExportCsv(DataTable dt, string FileName)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(FileName);
                string StrColumns = "";
                //Add Columns
                foreach (DataColumn column in dt.Columns)
                {
                    StrColumns += column.ColumnName + ",";
                }
                StrColumns = StrColumns.TrimEnd(',');
                sw.WriteLine(StrColumns);
                //Add Row
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string strRowData = "";
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        string Data = dt.Rows[i][j].ToString().Replace(',', '@');
                        //if (dt.Columns[j].ColumnName == "LotNo")
                        //    Data = "'" + Data;
                        strRowData += Data + ",";
                    }
                    strRowData = strRowData.TrimEnd(',');
                    sw.WriteLine(strRowData);
                }
                sw.Flush();
                sw.Close();
                sw = null;

                ClsGlobal.ShowConfirmMessageBox("File export successfully");
            }
            catch (Exception fex)
            { ClsGlobal.ShowErrorMessageBox(fex.Message); }
            finally
            {
                if (sw != null)
                {
                    sw.Flush();
                    sw.Close();
                    sw = null;
                }
            }
        }

        public static void ExportExcel(DataTable dt, string FileName)
        {
            try
            {
                Microsoft.Office.Interop.Excel.Application obj = new Microsoft.Office.Interop.Excel.Application();
                obj.Workbooks.Add(Type.Missing);

                Microsoft.Office.Interop.Excel.Range FormatRange;
                FormatRange = obj.Worksheets[1].Cells;
                FormatRange.NumberFormat = "@";

                //AddColumn
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    obj.Cells[1, j + 1] = dt.Columns[j].ColumnName;
                }
                //Add Row
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        obj.Cells[i + 2, j + 1] = dt.Rows[i][j].ToString();
                    }
                }

                obj.ActiveWorkbook.SaveCopyAs(FileName);

                obj.ActiveWorkbook.Saved = true;

                obj.Quit();

                ClsGlobal.ShowConfirmMessageBox("File export successfully");
            }
            catch (Exception fex)
            { ClsGlobal.ShowErrorMessageBox(fex.Message); }
        }

        public static string PrintLocationLabel(string BarCode)
        {
            try
            {
                string PrinterName = Properties.Settings.Default.PrinterName;

                StreamReader sr = new StreamReader(Application.StartupPath + "\\" + ClsGlobal.LocationPrnName);
                string PrnFileTemp = sr.ReadToEnd();
                sr.Close();
                PrnFileTemp = PrnFileTemp.Replace("{VAR1LEN}", BarCode.Length.ToString());
                PrnFileTemp = PrnFileTemp.Replace("{VAR1}", BarCode);
                return PrintBarcode.PrintCommand(PrnFileTemp, PrinterName);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static string PrintBinLabel(string Binbarcode)
        {
            try
            {
                string PrinterName = Properties.Settings.Default.PrinterName;
                if (File.Exists(Application.StartupPath + "\\" + ClsGlobal.BinBarcodePrnName))
                {
                    StreamReader sr = new StreamReader(Application.StartupPath + "\\" + ClsGlobal.BinBarcodePrnName);
                    string PrnFileTemp = sr.ReadToEnd();
                    sr.Close();
                    PrnFileTemp = PrnFileTemp.Replace("{VAR1LEN}", Binbarcode.Length.ToString());
                    PrnFileTemp = PrnFileTemp.Replace("{VAR1}", Binbarcode);
                    return PrintBarcode.PrintCommand(PrnFileTemp, PrinterName);
                }
                else
                    return "Prn File " + ClsGlobal.BinBarcodePrnName + " not found";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
