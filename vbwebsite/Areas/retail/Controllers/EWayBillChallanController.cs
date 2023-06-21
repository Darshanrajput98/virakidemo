using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TaxProEInvoice.API;
using TaxProEWB.API;
using vb.Data;
using vb.Data.ViewModel;
using vb.Service;
using vb.Service.Common;

namespace vbwebsite.Areas.retail.Controllers
{
    public class EWayBillChallanController : Controller
    {
        private static object Lock = new object();
        private RetOrderServices _orderService = new RetOrderServices();
        public eInvoiceSession eInvSession = new eInvoiceSession(false, false);
        public EWBSession EwbSession = new EWBSession();

        ECredentials eCred = new ECredentials();
        // GET: /retail/EWayBillChallan/
        public ActionResult Index()
        {
            return View();
        }

        public RetEWayBillChallan GenerateEWB(RetChallanDetailForEWB ChallanDetails, List<RetChallanItemForEWB> lstChallanItems, long UserId)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            eInvSession = eCred.SaveSettings();
            eInvSession = eCred.SaveLoginDetails();
            EwbSession = eCred.SaveEWBLoginDetails();
            EwbSession = eCred.SaveEWBSettings();

            RetEWayBillChallan eWBResponse = new RetEWayBillChallan();
            long ChallanID = lstChallanItems[0].ChallanID;
            string ChallanNumber = lstChallanItems[0].ChallanNumber;
            ReqGenEwbPl ewbGen = new ReqGenEwbPl();
            ewbGen.supplyType = ChallanDetails.SupplyType;
            ewbGen.subSupplyType = ChallanDetails.SubType;
            ewbGen.subSupplyDesc = "Others";
            ewbGen.docType = ChallanDetails.DocType;
            ewbGen.docNo = lstChallanItems[0].DocNo;
            ewbGen.docDate = lstChallanItems[0].DocDate;
            ewbGen.fromGstin = ChallanDetails.From_GSTIN;//"07AACCC1596Q1Z4";
            ewbGen.fromTrdName = ChallanDetails.DeliveryFrom;
            ewbGen.fromAddr1 = ChallanDetails.From_Address1;
            ewbGen.fromAddr2 = ChallanDetails.From_Address2;
            ewbGen.fromPlace = ChallanDetails.From_Place;
            ewbGen.fromPincode = Convert.ToInt32(ChallanDetails.From_PinCode);//263652;/*110001;*/
            ewbGen.fromStateCode = Convert.ToInt32(ChallanDetails.From_GSTIN.Substring(0, 2));
            ewbGen.actFromStateCode = Convert.ToInt32(ChallanDetails.From_GSTIN.Substring(0, 2));
            ewbGen.toGstin = ChallanDetails.To_GSTIN;
            ewbGen.toTrdName = ChallanDetails.DeliveryTo;
            ewbGen.toAddr1 = ChallanDetails.To_Address1;
            ewbGen.toAddr2 = ChallanDetails.To_Address2;
            ewbGen.toPlace = ChallanDetails.To_Place;
            ewbGen.toPincode = Convert.ToInt32(ChallanDetails.To_PinCode);/*110005;*/
            ewbGen.toStateCode = Convert.ToInt32(ChallanDetails.To_GSTIN.Substring(0, 2));
            ewbGen.actToStateCode = Convert.ToInt32(ChallanDetails.To_GSTIN.Substring(0, 2));
            ewbGen.transactionType = Convert.ToInt32(ChallanDetails.Transaction_Type);
            ewbGen.dispatchFromGSTIN = ChallanDetails.From_GSTIN; /*29AAAAA1303P1ZV*/
            ewbGen.dispatchFromTradeName = ChallanDetails.DeliveryFrom;
            ewbGen.shipToGSTIN = ChallanDetails.To_GSTIN; //29ALSPR1722R1Z3
            ewbGen.shipToTradeName = ChallanDetails.DeliveryTo;
            ewbGen.otherValue = 0;
            ewbGen.totalValue = Convert.ToDouble(Math.Round((ChallanDetails.FinalTotal), 2));
            ewbGen.cessValue = 0;
            ewbGen.cessNonAdvolValue = 0;
            ewbGen.transporterId = ChallanDetails.TransID;
            ewbGen.transporterName = ChallanDetails.TransName;
            ewbGen.transDocNo = ChallanDetails.TransDocNo;
            ewbGen.totInvValue = Convert.ToDouble(ChallanDetails.FinalTotal + ewbGen.otherValue);
            ewbGen.transMode = "1";//1
            ewbGen.transDistance = "0"; // ChallanDetails.DistanceKM.ToString(); /*1200*/
            ewbGen.transDocDate = ChallanDetails.TransDate;
            ChallanDetails.VehicleNo = ChallanDetails.VehicleNo.Replace("-", "");
            ewbGen.vehicleNo = ChallanDetails.VehicleNo;//PVC1234
            ewbGen.vehicleType = "R";//R
            ewbGen.itemList = new List<ReqGenEwbPl.ItemListInReqEWBpl>();
            ///data from database of perticular id

            foreach (var item in lstChallanItems)
            {
                ewbGen.itemList.Add(new ReqGenEwbPl.ItemListInReqEWBpl()
                {
                    productName = item.Product,
                    productDesc = item.Description,
                    hsnCode = Convert.ToInt32(item.HSN),
                    taxableAmount = Convert.ToDouble(Math.Round((item.ChallanTotal), 2))
                });
            }


            string a = JsonConvert.SerializeObject(ewbGen);
            string result = string.Empty;
            TxnRespWithObjAndInfo<RespGenEwbPl> TxnResp = Task.Run(() => EWBAPI.GenEWBAsync(EwbSession, ewbGen)).Result;

            var logFilePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(@"~/EWBChallanPrintPdf/log.txt"));
            using (StreamWriter w = System.IO.File.AppendText(logFilePath))
            {
                string response = JsonConvert.SerializeObject(TxnResp.RespObj);
                eCred.Log(string.Format("Generate EWB : {0} \n\n ResultObject : {1}", TxnResp.TxnOutcome, response), w);
            }

            if (TxnResp.IsSuccess)
            {
                var responce = JsonConvert.SerializeObject(TxnResp.RespObj);
                long EwbNo = 0;
                EwbNo = Convert.ToInt64(TxnResp.RespObj.ewayBillNo);
                eWBResponse.ChallanID = ChallanID;
                eWBResponse.ChallanNumber = ChallanNumber;
                eWBResponse.EWayBillNumber = EwbNo;

                string date = TxnResp.RespObj.ewayBillDate;
                var splitDateTime = date.Split(' ');
                var splitDate = splitDateTime[0].Split('/');
                var time = splitDateTime[1];
                var Year = splitDate[2];
                var month = splitDate[1];
                var day = splitDate[0];
                string ewayBillDate = Year + '-' + month + '-' + day + ' ' + time;
                eWBResponse.EWBDate = ewayBillDate;


                string date2 = TxnResp.RespObj.validUpto;
                var splitDateTime2 = date2.Split(' ');
                var splitDate2 = splitDateTime2[0].Split('/');
                var time2 = splitDateTime2[1];
                var Year2 = splitDate2[2];
                var month2 = splitDate2[1];
                var day2 = splitDate2[0];
                string validUpto = Year2 + '-' + month2 + '-' + day2 + ' ' + time2;
                eWBResponse.EWBValidTill = validUpto;

                eWBResponse.CreatedBy = UserId;
                _orderService.AddRetEWayBillChallan(eWBResponse);

                _orderService.UpdateEWayNumberForChallanByChallanNumber(ChallanID, ChallanNumber, EwbNo.ToString());
                EWBChallanSavePDF(EwbNo);
            }

            return eWBResponse;

        }



        public void EWBChallanSavePDF(long EwbNo = 0)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var pdfFolderPath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(@"~/EWBChallanPrintPdf/"));
            string filename = EwbNo + ".pdf";
            var logFilePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(@"~/EWBChallanPrintPdf/log.txt"));

            eInvSession = eCred.SaveSettings();
            eInvSession = eCred.SaveLoginDetails();
            EwbSession = eCred.SaveEWBLoginDetails();
            EwbSession = eCred.SaveEWBSettings();

            TxnRespWithObjAndInfo<RespGetEWBDetail> TxnResp = Task.Run(() => EWBAPI.GetEWBDetailAsync(EwbSession, EwbNo)).Result;

            using (StreamWriter w = System.IO.File.AppendText(logFilePath))
            {
                string response = JsonConvert.SerializeObject(TxnResp.RespObj);
                eCred.Log(string.Format("Get EWB Details : {0} \n\n ResultObject : {1}", TxnResp.TxnOutcome, response), w);
            }

            string strOutcome = "Errors in Printing E-Way Bill.";
            TxnResp.RespObj.transactionType = 1;
            RestClient client = new RestClient(EWBSession.GspApiBaseUrl + "/printewb");
            RestRequest request = new RestRequest(Method.POST);

            request.AddHeader("Gstin", EwbSession.EwbApiLoginDetails.EwbGstin);
            request.AddHeader("aspid", EwbSession.EwbApiSetting.AspUserId);
            request.AddHeader("password", EwbSession.EwbApiSetting.AspPassword);
            request.AddHeader("Content-Type", "application/json");//"application/json; charset=utf-8");
            request.RequestFormat = RestSharp.DataFormat.Json;
            request.DateFormat = RestSharp.DataFormat.Json.ToString();
            request.AddJsonBody(TxnResp.RespObj);
            //string strewbDetail = JsonConvert.SerializeObject(TxnResp.RespObj);
            byte[] res = client.DownloadData(request);
            try
            {
                string path = pdfFolderPath + filename;
                System.IO.File.WriteAllBytes(path, res);
            }
            catch (Exception ex)
            {
                strOutcome = ex.Message;
            }
        }

        public ActionResult EWayBillChallanPrint(long EwbNo = 0)
        {
            string url = string.Empty;
            var pdfFolderPath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(@"~/EWBChallanPrintPdf/"));
            string filename = EwbNo + ".pdf";
            if (System.IO.File.Exists(pdfFolderPath + filename))
            {

                if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                {
                    url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/EWBChallanPrintPdf/" + filename;
                }
                else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                {
                    url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/EWBChallanPrintPdf/" + filename;
                }
                else
                {
                    url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/EWBChallanPrintPdf/" + filename;
                }
            }
            else
            {
                EWBChallanSavePDF(EwbNo);
            }
            return Json(url, JsonRequestBehavior.AllowGet);
        }
    }
}