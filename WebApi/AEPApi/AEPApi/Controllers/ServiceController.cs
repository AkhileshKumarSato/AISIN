using AEPApi.Dal;
using AEPApi.Models;
using SatoLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace AEPApi.Controllers
{
    public class ServiceController : ApiController
    {
        [HttpGet]
        public string GetTestValue()
        {
            return "Working";
        }

        #region Door Lock Lines(6)
        /*
         * This action method will return details based on kanban(This is for 6  door lines)
         * depend on barcode length app will idenfity whether it is kanban barcode or bin barcode
         * bin barcode will be scanned in case of fill fraction bin from fresh part or rejection
         */

        [HttpGet]
        [Route("api/service/GetKanBanBinData/{LineNo}/{BarcodeData}")]
        public Production GetKanBanBinData(string LineNo, string BarcodeData)
        {
            Production production = new Production();
            DataAccess dataAccess = new DataAccess();
            try
            {
                production.LineNo = LineNo;
                //Check Barcode is Kanban Or BinBarcode(Length 18 ddMMyyhhMMss01PART)
                production.IsBinBarcode = BarcodeData.Length == 18 ? true : false;
                //If bin barcode then fetch data details
                #region If Bin Barcode
                if (production.IsBinBarcode)
                {
                    production.BinBarcode = BarcodeData;
                    DataTable dt = dataAccess.GetPendingKanBanData(production).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        production.ProductionId = dt.Rows[0]["ProductionId"].ToString();
                        production.BackNo = dt.Rows[0]["BackNo"].ToString();
                        production.StandardBinQty = int.Parse(dt.Rows[0]["StandardBinQty"].ToString());
                        production.ScanQty = int.Parse(dt.Rows[0]["ScanQty"].ToString());

                        production.Response = "Y";
                        production.ErrorMessage = "";
                    }
                    else
                    {
                        production.Response = "N";
                        production.ErrorMessage = "Wrong BinBarcode " + BarcodeData;
                    }
                }
                #endregion
                #region When Kanban Barcode
                else
                {
                    /*
                     *  not bin barcode means kanban barcode so fetch other details
                     *  Export & Domestic barcode will have same data format
                     * AIGSYSGL292   LJ10  LJAS0016   LJ        411340-17411          SE1000   LJ       XXXXXXXXX  0       Y390 0000008000000000003530                                0                              0000008                  PCS
                     */
                    string[] sArrKanBan = BarcodeData.Split(' '); //Split with Space
                    if (sArrKanBan.Length >= 46)
                    {
                        var BackNo = sArrKanBan[45].Trim();
                        if (BackNo.Length == 4)
                        {
                            DataTable dtPart = dataAccess.ManagePart(new Part { DbType = EnumDbType.SELECTBYID, BackNo = BackNo });
                            if (dtPart.Rows.Count > 0)
                            {
                                production.ProductionId = "";
                                production.StandardBinQty = int.Parse(dtPart.Rows[0]["StandardBinQty"].ToString());
                                production.ScanQty = 0;
                                production.Response = "Y";
                                production.ErrorMessage = "";
                            }
                            else
                            {
                                production.Response = "N";
                                production.ErrorMessage = "BackNo " + BackNo + "  details not found from kanban";
                            }
                        }
                        else
                        {
                            production.Response = "N";
                            production.ErrorMessage = "Invalid backNo in kanban barcode " + BackNo;
                        }
                    }
                    else
                    {
                        production.Response = "N";
                        production.ErrorMessage = "Invalid Kanban Barcode Length";
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                production.Response = "N";
                production.ErrorMessage = "Error : " + ex.Message;
            }
            return production;
        }

        /*
        * this action method will save the scanned part along with kanban.
        * If ProductionId is blank it means this is the first part of bin. After saving first part 
        * method will return the production no so when the next part will be saved then we know that 
        * production no is already generated means kanban details already saved, now only part details will be saved
        */

        [HttpPost]
        //[Route("api/service/SavePart/{LineNo}/{KanbanBarcode}/{PartBarcode}/{BackNo}/{ProductionId}/{StandardBinQty}")]
        public Production SavePart(Production prodRequest)
        {
            Production production = new Production();
            DataAccess dataAccess = new DataAccess();
            try
            {
                if (prodRequest.PartBarcode.Length >= 4)
                {
                    //Part barcode belong to same back no, last 4 digit of all type of barcode(Domestic,Export) are backno
                    string PartBackNo = prodRequest.PartBarcode.Substring(prodRequest.PartBarcode.Length - 4);
                    DataTable dt;
                    //If kanban back no and part back no are same
                    if (prodRequest.BackNo == PartBackNo)
                    {
                        production.BackNo = prodRequest.BackNo;
                        production.CreatedBy = "Api";
                        production.KanBanBarcode = prodRequest.KanBanBarcode;
                        production.LineNo = prodRequest.LineNo;
                        production.PartBarcode = prodRequest.PartBarcode;
                        production.ProductionId = prodRequest.ProductionId;
                        production.StandardBinQty = prodRequest.StandardBinQty;
                        //Production id is not generated, means this is the first part for kanban
                        if (prodRequest.ProductionId == "" || prodRequest.ProductionId == "0")
                        {
                            production.ProductionId = "0";
                            production.DbType = EnumDbType.KANBAN;
                            //When First part is saving so it is consider as fractiion part unitl unless scan complete
                            production.Status = Convert.ToInt32(EnumProductionStatus.FRACTION);
                            dt = dataAccess.SaveProductionData(production);
                            if (dt.Rows[0]["Result"].ToString() == "Y")
                            {
                                production.ProductionId = dt.Rows[0]["Msg"].ToString();
                                production.Response = "Y";
                                production.ErrorMessage = "";
                            }
                            else
                            {
                                production.Response = "N";
                                production.ErrorMessage = dt.Rows[0]["Msg"].ToString();
                            }
                        }
                        else // if part is saved not kanban
                        {
                            production.DbType = EnumDbType.PART;
                            //Assuming scanning complete so bin complete status will be updated
                            production.Status = Convert.ToInt32(EnumProductionStatus.COMPLETE);
                            dt = dataAccess.SaveProductionData(production);
                            if (dt.Rows[0]["Result"].ToString() == "Y")
                            {
                                production.Response = "Y";
                                production.ErrorMessage = "";
                            }
                            else if (dt.Rows[0]["Result"].ToString() == "P") // When Barcode generated/read for print
                            {
                                production.BinBarcode = dt.Rows[0]["Msg"].ToString();
                                production.Response = "P";
                                production.ErrorMessage = "";
                            }
                            else
                            {
                                production.Response = "N";
                                production.ErrorMessage = dt.Rows[0]["Msg"].ToString();
                            }
                        }
                    }
                    else
                    {
                        production.Response = "N";
                        production.ErrorMessage = "Wrong back no " + PartBackNo + " in part barcode " + prodRequest.PartBarcode;
                    }
                }
                else
                {
                    production.Response = "N";
                    production.ErrorMessage = "Invalid Part Barcode length " + prodRequest.PartBarcode;
                }
            }
            catch (Exception ex)
            {
                production.Response = "N";
                production.ErrorMessage = "Error : " + ex.Message;
            }
            return production;
        }

        /*
       * This action method will return details based on kanban(This is for 6  door lines)
       * depend on barcode length app will idenfity whether it is kanban barcode or bin barcode
       * bin barcode will be scanned in case of fill fraction bin from fresh part or rejection
       */

        //[HttpGet]
        //[Route("api/service/RejectPart/{PartBarcode}")]
        //public Production RejectPart(string PartBarcode)
        //{
        //    Production production = new Production();
        //    DataAccess dataAccess = new DataAccess();
        //    try
        //    {
        //        production.LineNo = LineNo;
        //        //Check Barcode is Kanban Or BinBarcode(Length 18 ddMMyyhhMMss01PART)
        //        production.IsBinBarcode = BarcodeData.Length == 18 ? true : false;
        //        //If bin barcode then fetch data details
        //        #region If Bin Barcode
        //        if (production.IsBinBarcode)
        //        {
        //            production.BinBarcode = BarcodeData;
        //            DataTable dt = dataAccess.GetPendingKanBanData(production).Tables[0];
        //            if (dt.Rows.Count > 0)
        //            {
        //                production.ProductionId = dt.Rows[0]["ProductionId"].ToString();
        //                production.BackNo = dt.Rows[0]["BackNo"].ToString();
        //                production.StandardBinQty = int.Parse(dt.Rows[0]["StandardBinQty"].ToString());
        //                production.ScanQty = int.Parse(dt.Rows[0]["ScanQty"].ToString());
        //            }
        //            else
        //            {
        //                production.Response = "N";
        //                production.ErrorMessage = "Wrong BinBarcode " + BarcodeData;
        //            }
        //        }
        //        #endregion
        //        #region When Kanban Barcode
        //        else
        //        {
        //            /*
        //             *  not bin barcode means kanban barcode so fetch other details
        //             *  Export & Domestic barcode will have same data format
        //             * AIGSYSGL292   LJ10  LJAS0016   LJ        411340-17411          SE1000   LJ       XXXXXXXXX  0       Y390 0000008000000000003530                                0                              0000008                  PCS
        //             */
        //            string[] sArrKanBan = BarcodeData.Split(' '); //Split with Space
        //            if (sArrKanBan.Length >= 46)
        //            {
        //                var BackNo = sArrKanBan[45].Trim();
        //                if (BackNo.Length == 4)
        //                {
        //                    DataTable dtPart = dataAccess.ManagePart(new Part { DbType = EnumDbType.SELECTBYID, BackNo = BackNo });
        //                    if (dtPart.Rows.Count > 0)
        //                    {
        //                        production.ProductionId = "";
        //                        production.StandardBinQty = int.Parse(dtPart.Rows[0]["StandardBinQty"].ToString());
        //                        production.ScanQty = 0;
        //                        production.Response = "Y";
        //                        production.ErrorMessage = "";
        //                    }
        //                    else
        //                    {
        //                        production.Response = "N";
        //                        production.ErrorMessage = "BackNo " + BackNo + "  details not found from kanban";
        //                    }
        //                }
        //                else
        //                {
        //                    production.Response = "N";
        //                    production.ErrorMessage = "Invalid backNo in kanban barcode " + BackNo;
        //                }
        //            }
        //            else
        //            {
        //                production.Response = "N";
        //                production.ErrorMessage = "Invalid Kanban Barcode Length";
        //            }
        //        }
        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {
        //        production.Response = "N";
        //        production.ErrorMessage = "Error : " + ex.Message;
        //    }
        //    return production;
        //}

        /*
        * This action method will print the binbarcode in case Service Part or Fraction Bin
        */

        [HttpGet]
        [Route("api/service/PartialPrint/{IsServicePart}/{ProductionId}/{LineNo}/{BackNo}")]
        public Production PartialPrint(bool IsServicePart, string ProductionId, string LineNo, string BackNo)
        {
            Production production = new Production();
            DataAccess dataAccess = new DataAccess();
            try
            {
                production.IsServicePart = IsServicePart;
                production.ProductionId = ProductionId;
                production.LineNo = LineNo;
                production.BackNo = BackNo;
                production.CreatedBy = "Api";
                production.Status = IsServicePart ? Convert.ToInt32(EnumProductionStatus.SERVICE_PART) : Convert.ToInt32(EnumProductionStatus.FRACTION);

                DataTable dt = dataAccess.PartialPrint(production);
                if (dt.Rows.Count > 0)
                {
                    production.BinBarcode = dt.Rows[0]["Msg"].ToString();
                    production.Response = "Y";
                    production.ErrorMessage = "";
                }
                else
                {
                    production.Response = "N";
                    production.ErrorMessage = "No data retured from server";
                }
            }
            catch (Exception ex)
            {
                production.Response = "N";
                production.ErrorMessage = "Error : " + ex.Message;
            }
            return production;
        }

        #endregion

        #region Other Lines AEP(PartNo WithOut barcode)

        /*
      * This action method will return the back no details
      */

        [HttpGet]
        [Route("api/service/GetBackNoDetails/{KanbanBarcode}")]
        public Part GetBackNoDetails(string KanbanBarcode)
        {

            var BackNo = "";
            Part part = new Part();
            DataAccess dataAccess = new DataAccess();
            try
            {
                
                GlobalVar.Logger.LogMessage(SatoLib.EventNotice.EventTypes.evtError, "KanbanBarcode", KanbanBarcode);
              
                if (KanbanBarcode.Length <= 230)
                {
                    DataTable dtBackNo = dataAccess.GetBackNoUsingFucntion(KanbanBarcode);
                    if (dtBackNo.Rows.Count > 0)
                    {
                        BackNo = dtBackNo.Rows[0][0].ToString();
                        if (BackNo == "ZZZZ")
                        {

                            part.Response = "N";
                            part.ErrorMessage = "Invalid Kanban Barcode or Back No Not Found!!!";
                        }
                        DataTable dtPart = dataAccess.ManagePart(new Part { DbType = EnumDbType.SELECTBYID, BackNo = BackNo });
                        if (dtPart.Rows.Count > 0)
                        {
                            if (Convert.ToBoolean(dtPart.Rows[0]["IsBarcodeAvailable"]))
                            {
                                part.Response = "N";
                                part.ErrorMessage = "BackNo " + BackNo + "  belong to barcoded lines";
                            }
                            else
                            {
                                part.BackNo = BackNo;
                                part.StandardBinQty = int.Parse(dtPart.Rows[0]["StandardBinQty"].ToString());
                                part.Response = "Y";
                                part.ErrorMessage = "";
                            }
                        }
                        else
                        {
                            part.Response = "N";
                            part.ErrorMessage = "BackNo " + BackNo + "  details not found from kanban";
                        }
                    }
                    else
                    {
                        part.Response = "N";
                        part.ErrorMessage = "Invalid Kanban Barcode or Back No Not Found!!!";
                    }
                }
                else
                {
                    part.Response = "N";
                    part.ErrorMessage = "Invalid Kanban Barcode Length " + KanbanBarcode.Length + " !!!";
                }
            }
            catch (Exception ex)
            {
                part.Response = "N";
                part.ErrorMessage = "Error : " + ex.Message;
            }
            return part;
        }

        /*
        * This action method will save kanban details and return the binbarcode
        * Kanbanbarcode parameter shoule be in the last otherwise AEP is having some problem
        */

        [HttpGet]
        [Route("api/service/SaveBin/{BackNo}/{StandardBinQty}/{ScanQty}/{KanbanBarcode}")]
        public Production SaveBin(string BackNo, string StandardBinQty, string ScanQty, string KanbanBarcode)
        {
            Production production = new Production();
            DataAccess dataAccess = new DataAccess();
            try
            {
                production.BackNo = BackNo;
                production.KanBanBarcode = KanbanBarcode;
                production.StandardBinQty = int.Parse(StandardBinQty);
                production.ScanQty = int.Parse(ScanQty);
                production.Status = int.Parse(StandardBinQty) > int.Parse(ScanQty) ? Convert.ToInt32(EnumProductionStatus.FRACTION) : Convert.ToInt32(EnumProductionStatus.COMPLETE);

                DataTable dt = dataAccess.SaveProductionNonBarcode(production);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Result"].ToString() == "P")
                    {
                        production.BinBarcode = dt.Rows[0]["Msg"].ToString();
                        production.Response = "Y";
                        production.ErrorMessage = "";
                    }
                    else
                    {
                        production.Response = "N";
                        production.ErrorMessage = dt.Rows[0]["Msg"].ToString();
                    }
                }
                else
                {
                    production.Response = "N";
                    production.ErrorMessage = "Could not be saved, details not returned";
                }
            }
            catch (Exception ex)
            {
                production.Response = "N";
                production.ErrorMessage = "Error : " + ex.Message;
            }
            return production;
        }


        #endregion
    }
}
