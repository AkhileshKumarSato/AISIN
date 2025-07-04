﻿using ClientCommunicationApp.Common;
using SatoLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace ClientCommunicationApp.Model
{
    public class DL_PLC_ASSEY_TRANSCATION
    {
       
        public string SendToServer(string receiveString, string IP)
        {
            string sReturnToServer = string.Empty;
            PL_ASSEY_TRANSCATION plObj = new PL_ASSEY_TRANSCATION();
            try
            {
                GlobalVar.Logger.LogMessage(EventNotice.EventTypes.evtInfo, $"SendToServer:{IP}", $"Received data for db::{receiveString}");
                if (receiveString.Trim().Length>0)
                {
                    string removeDataDelimeter = receiveString.TrimEnd('^');
                    string[] arrData = removeDataDelimeter.Split('~');
                    plObj.ModelNo = arrData[0];
                    //plObj.TestingStatus = arrData[1];

                    plObj.CreatedBy = IP;
                    //plObj.VendorCode = Properties.Settings.Default.VendorCode;
                   // plObj.PartNo = Properties.Settings.Default.ProductCode;
                    plObj.LineNo = Properties.Settings.Default.Line;
                    DataTable dataTable = null;
                   
                    dataTable = DL_ExecuteTask(plObj);
                   
                    if (dataTable.Rows.Count > 0)
                    {
                        sReturnToServer = dataTable.Rows[0][0].ToString();
                    }
                   
                }
                else
                {
                    sReturnToServer = "02" + "~" + "00-00000000" + "~" + "00-00000000" + "~" + "00-00000000" + "~" + "00-00000000" + "~" + "0-00000000" + "~" + "0-00000000" + "~Request data is not correct!!";
                }
            }
            catch (Exception ex)
            {

                sReturnToServer = "02" + "~" + "00-00000000" + "~" + "00-00000000" + "~" + "00-00000000" + "~" + "00-00000000" + "~" + "0-00000000" + "~" + "0-00000000" + ex.Message;
            }
            return sReturnToServer;
        }
     


        private SqlHelper _SqlHelper = new SqlHelper();
        #region MyFuncation
        /// <summary>
        /// Execute Operation 
        /// </summary>
        /// <returns></returns>
        public DataTable DL_ExecuteTask(PL_ASSEY_TRANSCATION obj)
        {
            _SqlHelper = new SqlHelper();
            try
            {
                SqlParameter[] param = new SqlParameter[6];

                param[0] = new SqlParameter("@TYPE", SqlDbType.VarChar, 100);
                param[0].Value = "INSERT";
                param[1] = new SqlParameter("@LineNo", SqlDbType.VarChar, 100);
                param[1].Value = obj.LineNo;
                param[2] = new SqlParameter("@PartBarcode", SqlDbType.VarChar, 100);
                param[2].Value = obj.ModelNo;
                param[3] = new SqlParameter("@CreatedBy", SqlDbType.VarChar, 100);
                param[3].Value = obj.CreatedBy;
                return _SqlHelper.ExecuteDataset(GlobalVar.mMainSqlConString, CommandType.StoredProcedure, "[PRC_INSERT_BARCODE_FROM_PLC]", param).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
                
        public DataTable DL_ExecuteTask_For_Bottom(PL_ASSEY_TRANSCATION obj)
        {
            _SqlHelper = new SqlHelper();
            try
            {
                SqlParameter[] param = new SqlParameter[10];

                param[0] = new SqlParameter("@TYPE", SqlDbType.VarChar, 100);
                param[0].Value = obj.DbType;
                param[1] = new SqlParameter("@STATION_NO", SqlDbType.VarChar, 100);
                param[1].Value = obj.StationNo;
                param[2] = new SqlParameter("@BARCODE", SqlDbType.VarChar, 100);
                param[2].Value = obj.ParentBarcode;
                param[3] = new SqlParameter("@TESTING_STATUS", SqlDbType.VarChar, 100);
                param[3].Value = obj.TestingStatus;
                param[4] = new SqlParameter("@MACHINE_PARAM", SqlDbType.VarChar, 3000);
                param[4].Value = obj.MachineParam;
                param[5] = new SqlParameter("@CREATED_BY", SqlDbType.VarChar, 100);
                param[5].Value = obj.CreatedBy;
                return _SqlHelper.ExecuteDataset(GlobalVar.mMainSqlConString, CommandType.StoredProcedure, "[PRC_CNG_ASSEMBLY_SCANNING_P2]", param).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable DL_ExecuteTask_For_FG_CNG(PL_ASSEY_TRANSCATION obj)
        {
            _SqlHelper = new SqlHelper();
            try
            {
                SqlParameter[] param = new SqlParameter[7];

                param[0] = new SqlParameter("@TYPE", SqlDbType.VarChar, 100);
                param[0].Value = obj.DbType;
                param[1] = new SqlParameter("@FG_BARCODE", SqlDbType.VarChar, 100);
                param[1].Value = obj.FG_Barcode;
                param[2] = new SqlParameter("@IS_MANUAL", SqlDbType.VarChar, 50);
                param[2].Value = obj.Is_Manual;
                param[3] = new SqlParameter("@TESTING_STATUS", SqlDbType.VarChar, 100);
                param[3].Value = obj.TestingStatus;
                param[4] = new SqlParameter("@MACHINE_PARAM", SqlDbType.VarChar, 3000);
                param[4].Value = obj.MachineParam;
                param[5] = new SqlParameter("@CREATED_BY", SqlDbType.VarChar, 50);
                param[5].Value = obj.CreatedBy;
                return _SqlHelper.ExecuteDataset(GlobalVar.mMainSqlConString, CommandType.StoredProcedure, "[PRC_FG_LABEL_PRINTING]", param).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable DL_ExecuteTask_UpdateLastSerial(string Type,string Model,string Serial)
        {
            _SqlHelper = new SqlHelper();
            try
            {
                SqlParameter[] param = new SqlParameter[3];

                param[0] = new SqlParameter("@TYPE", SqlDbType.VarChar, 100);
                param[0].Value = Type;
                param[1] = new SqlParameter("@ModelNo", SqlDbType.VarChar, 100);
                param[1].Value = Model;
                param[2] = new SqlParameter("@SerialNo", SqlDbType.VarChar, 100);
                param[2].Value = Serial;
                return _SqlHelper.ExecuteDataset(GlobalVar.mMainSqlConString, CommandType.StoredProcedure, "[Update_SerialNo]", param).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
