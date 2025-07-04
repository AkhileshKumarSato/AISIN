using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Text;
using AEPApi.Models;

namespace AEPApi.Dal
{
    public class DataAccess
    {
        StringBuilder _SbQry;
        #region Get Back No using Scalar Fucntion
        public DataTable GetBackNoUsingFucntion(string kanbaanBarcode)
        {
            ClsDB oDb = new ClsDB();
            try
            {
                _SbQry = new StringBuilder("Exec PRC_GET_BACK_NO '" + kanbaanBarcode + "'");
                oDb.Connect();
                DataTable dt = oDb.GetDataTable(_SbQry.ToString());
                return dt;
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
        }
        #endregion

        #region Part Master

        public DataTable ManagePart(Part part)
        {
            ClsDB oDb = new ClsDB();
            try
            {
                _SbQry = new StringBuilder("Exec [Prc_PartMaster] '" + part.DbType + "','" + part.BackNo + "','" + part.Description + "'");
                _SbQry.AppendLine(",'" + part.CreatedBy + "'," + part.StandardBinQty + ",'" + part.PartNo + "','" + part.CustomerPartNo + "','" + part.IsBarcodeAvailable + "'");
                oDb.Connect();
                return oDb.GetDataTable(_SbQry.ToString());
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
        }

        #endregion

        #region Production

        //It will return multiple datatable while loading data
        public DataSet GetPendingKanBanData(Production prod)
        {
            ClsDB oDb = new ClsDB();
            try
            {
                _SbQry = new StringBuilder("Exec Prc_Api_GetProductionData '" + prod.LineNo + "','" + prod.BinBarcode + "','" + prod.IsBinBarcode + "'");
                oDb.Connect();
                return oDb.GetDataSet(_SbQry.ToString());
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
        }

        public DataTable SaveProductionData(Production prod)
        {
            ClsDB oDb = new ClsDB();
            try
            {
                _SbQry = new StringBuilder("Exec Prc_Api_SaveProductionData '" + prod.DbType + "','" + prod.ProductionId + "'");
                _SbQry.AppendLine(",'" + prod.LineNo + "','" + prod.BackNo + "','" + prod.KanBanBarcode + "','" + prod.BinBarcode + "','" + prod.PartBarcode + "'");
                _SbQry.AppendLine("," + prod.StandardBinQty + ",'" + prod.CreatedBy + "'," + prod.Status + "");

                oDb.Connect();
                return oDb.GetDataTable(_SbQry.ToString());
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
        }

        public DataTable PartialPrint(Production prod)
        {
            ClsDB oDb = new ClsDB();
            try
            {
                _SbQry = new StringBuilder("Exec Prc_Api_PartialProductionDataPrint '" + prod.IsServicePart + "','" + prod.ProductionId + "'");
                _SbQry.AppendLine(",'" + prod.LineNo + "','" + prod.BackNo + "','" + prod.CreatedBy + "'," + prod.Status + "");

                oDb.Connect();
                return oDb.GetDataTable(_SbQry.ToString());
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
        }

        public DataTable SaveProductionNonBarcode(Production prod)
        {
            ClsDB oDb = new ClsDB();
            try
            {
                _SbQry = new StringBuilder("Exec [Prc_Api_SaveProductionData_NonBarcode] '" + prod.BackNo + "','" + prod.KanBanBarcode + "'");
                _SbQry.AppendLine("," + prod.StandardBinQty + "," + prod.ScanQty + "," + prod.Status + "");

                oDb.Connect();
                return oDb.GetDataTable(_SbQry.ToString());
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                oDb.DisConnect();
                oDb = null;
            }
        }

        #endregion
    }
}
