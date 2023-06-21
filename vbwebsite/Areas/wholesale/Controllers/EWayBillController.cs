using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TaxProEInvoice.API;
using TaxProEWB.API;
using vb.Data;
using vb.Data.Model;
using vb.Data.ViewModel;
using vb.Service;
using vb.Service.Common;
using ZXing;

namespace vbwebsite.Areas.wholesale.Controllers
{
    public class EWayBillController : Controller
    {
        private static object Lock = new object();
        private OrderServices _orderService = new OrderServices();
        eInvoiceSession eInvSession = new eInvoiceSession(false, false);
        public EWBSession EwbSession = new EWBSession();

        ECredentials eCred = new ECredentials();

        // GET: /wholesale/EInvoice/
        public ActionResult Index()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            // SaveSettings();
            //  SaveLoginDetails();
            return View();
        }



        //public void SaveSettings()
        //{
        //    eInvSession.eInvApiSetting = new eInvoiceAPISetting();

        //    eInvSession.eInvApiSetting.GSPName = "TaxPro_Production";
        //    eInvSession.eInvApiSetting.AspUserId = "1658771921";
        //    eInvSession.eInvApiSetting.AspPassword = "Chetan@334";
        //    eInvSession.eInvApiSetting.client_id = "";
        //    eInvSession.eInvApiSetting.client_secret = "";
        //    eInvSession.eInvApiSetting.AuthUrl = "https://einvapi.charteredinfo.com/eivital/v1.03";
        //    eInvSession.eInvApiSetting.BaseUrl = "https://einvapi.charteredinfo.com/eicore/v1.03";
        //    eInvSession.eInvApiSetting.EwbByIRN = "https://einvapi.charteredinfo.com/eiewb/v1.03";
        //    eInvSession.eInvApiSetting.CancelEwbUrl = "https://einvapi.charteredinfo.com/v1.03";
        //}

        //public void SaveLoginDetails()
        //{
        //    eInvSession.eInvApiLoginDetails = new eInvoiceAPILoginDetails();
        //    eInvSession.eInvApiLoginDetails.UserName = ConfigurationManager.AppSettings["UserName"];
        //    eInvSession.eInvApiLoginDetails.Password = ConfigurationManager.AppSettings["Password"];
        //    eInvSession.eInvApiLoginDetails.GSTIN = "27AAAFV3761F1Z7";
        //    eInvSession.eInvApiLoginDetails.AppKey = "RvCyJnJZDGGd3EaxDyK+SAUtkAS/GqO4CsH+ScjykDU=";

        //    double TokenExpiredHours = Convert.ToDouble(ConfigurationManager.AppSettings["TokenExpiredHours"]);

        //    string Sek = "";
        //    string AuthToken = "";
        //    var detail = GetOldAuthToken();
        //    if (string.IsNullOrEmpty(detail.AuthToken))
        //    {
        //        // Generate Authentication Token
        //        Authentication obj2 = new Authentication();
        //        obj2 = GetAuthToken();

        //        // Insert Authentication Token
        //        DateTime ExpiredTime = DateTime.Now.AddHours(TokenExpiredHours);
        //        AuthToken_Mst obj = new AuthToken_Mst();


        //        //obj.ExpiredOn = ExpiredTime;
        //        obj.ExpiredOn = obj2.ExpiredOn;

        //        obj.Sek = obj2.Sek;
        //        obj.AuthToken = obj2.AuthToken;
        //        bool respose = AddAuthToken(obj);
        //        Sek = obj.Sek;
        //        AuthToken = obj.AuthToken;
        //    }
        //    else if (DateTime.Now < detail.ExpiredOn)
        //    {
        //        AuthToken = detail.AuthToken;
        //        Sek = detail.Sek;
        //    }
        //    else
        //    {
        //        // Generate Authentication Token
        //        Authentication obj2 = new Authentication();
        //        obj2 = GetAuthToken();

        //        // Update Authentication
        //        DateTime ExpiredTime = DateTime.Now.AddHours(TokenExpiredHours);
        //        AuthToken_Mst obj = new AuthToken_Mst();
        //        obj.AuthTokenID = detail.AuthTokenID;


        //        //obj.ExpiredOn = ExpiredTime;
        //        obj.ExpiredOn = obj2.ExpiredOn;


        //        obj.Sek = obj2.Sek;
        //        obj.AuthToken = obj2.AuthToken;
        //        bool respose = AddAuthToken(obj);
        //        Sek = obj.Sek;
        //        AuthToken = obj.AuthToken;
        //    }

        //    eInvSession.eInvApiLoginDetails.AuthToken = AuthToken;
        //    eInvSession.eInvApiLoginDetails.Sek = Sek;
        //    eInvSession.eInvApiLoginDetails.E_InvoiceTokenExp = Convert.ToDateTime("3/23/2021 11:57:40 AM");
        //}

        //public void SaveEWBSettings()
        //{
        //    eInvSession.eInvApiSetting = new eInvoiceAPISetting();


        //    EwbSession.EwbApiSetting.GSPName = "TaxPro_Production";
        //    EwbSession.EwbApiSetting.AspUserId = "1658771921";
        //    EwbSession.EwbApiSetting.AspPassword = "Chetan@334";
        //    EwbSession.EwbApiSetting.EWBClientId = "";
        //    EwbSession.EwbApiSetting.EWBClientSecret = "";
        //    EwbSession.EwbApiSetting.EWBGSPUserID = "";
        //    EwbSession.EwbApiSetting.BaseUrl = "https://einvapi.charteredinfo.com/v1.03";

        //}

        //public void SaveEWBLoginDetails()
        //{
        //    EwbSession.EwbApiLoginDetails.EwbGstin = eInvSession.eInvApiLoginDetails.GSTIN;
        //    EwbSession.EwbApiLoginDetails.EwbUserID = ConfigurationManager.AppSettings["UserName"];
        //    EwbSession.EwbApiLoginDetails.EwbPassword = ConfigurationManager.AppSettings["Password"];
        //    EwbSession.EwbApiLoginDetails.EwbAppKey = eInvSession.eInvApiLoginDetails.AppKey;

        //    double TokenExpiredHours = Convert.ToDouble(ConfigurationManager.AppSettings["TokenExpiredHours"]);
        //    EwbSession.EwbApiLoginDetails.EwbAuthToken = eInvSession.eInvApiLoginDetails.AuthToken;
        //    EwbSession.EwbApiLoginDetails.EwbTokenExp = Convert.ToDateTime("3/23/2021 11:57:40 AM");
        //    EwbSession.EwbApiLoginDetails.EwbSEK = eInvSession.eInvApiLoginDetails.Sek;
        //}

        //public AuthTokenDetail GetOldAuthToken()
        //{
        //    SqlCommand cmdGet = new SqlCommand();
        //    BaseSqlManager objBaseSqlManager = new BaseSqlManager();
        //    cmdGet.CommandType = CommandType.StoredProcedure;
        //    cmdGet.CommandText = "GetAuthToken";
        //    SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
        //    AuthTokenDetail obj = new AuthTokenDetail();
        //    while (dr.Read())
        //    {
        //        obj.AuthTokenID = objBaseSqlManager.GetInt64(dr, "AuthTokenID");
        //        obj.AuthToken = objBaseSqlManager.GetTextValue(dr, "AuthToken");
        //        obj.Sek = objBaseSqlManager.GetTextValue(dr, "Sek");
        //        obj.ExpiredOn = objBaseSqlManager.GetDateTime(dr, "ExpiredOn");
        //    }
        //    dr.Close();
        //    objBaseSqlManager.ForceCloseConnection();
        //    return obj;
        //}

        //public Authentication GetAuthToken()
        //{
        //    Authentication obj = new Authentication();
        //    string AuthTokenResponse = string.Empty;
        //    TaxProEInvoice.API.TxnRespWithObj<eInvoiceSession> txnRespWithObj = Task.Run(() => eInvoiceAPI.GetAuthTokenAsync(eInvSession)).Result;
        //    // here store in DB
        //    obj.AuthToken = txnRespWithObj.RespObj.eInvApiLoginDetails.AuthToken;
        //    obj.Sek = txnRespWithObj.RespObj.eInvApiLoginDetails.Sek;

        //    // 21/05/2021 Get Expired date
        //    obj.ExpiredOn = txnRespWithObj.RespObj.eInvApiLoginDetails.E_InvoiceTokenExp;

        //    return obj;
        //}

        //public bool AddAuthToken(AuthToken_Mst ObjEvent)
        //{
        //    using (VirakiEntities context = new VirakiEntities())
        //    {
        //        if (ObjEvent.AuthTokenID == 0)
        //        {
        //            context.AuthToken_Mst.Add(ObjEvent);
        //            context.SaveChanges();
        //        }
        //        else
        //        {
        //            context.Entry(ObjEvent).State = EntityState.Modified;
        //            context.SaveChanges();
        //        }
        //        if (ObjEvent.AuthTokenID > 0)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //}




        public EWayBill GenerateEWB(DetailsForEWB data, long orderId, string invoiceNumber, long UserId = 1)
        {
            // SaveSettings();
            //  SaveLoginDetails();


            eInvSession = eCred.SaveSettings();
            eInvSession = eCred.SaveLoginDetails();

            EWayBill eWBResponse = new EWayBill();
            //Generate Ewb By IRN
            ReqPlGenEwbByIRN reqPlGenEwbByIRN = new ReqPlGenEwbByIRN();
            //foreach (var item in data)
            //{
            reqPlGenEwbByIRN.Irn = data.IRN;
            reqPlGenEwbByIRN.TransId = data.TransID;
            reqPlGenEwbByIRN.TransMode = "1";
            reqPlGenEwbByIRN.TransDocNo = data.TransDocNo;
            reqPlGenEwbByIRN.TransDocDt = data.TransDate;
            data.VehicleNo = data.VehicleNo.Replace("-", "");
            reqPlGenEwbByIRN.VehNo = data.VehicleNo;

            // reqPlGenEwbByIRN.Distance = Convert.ToInt32(data.DistanceKM);
            reqPlGenEwbByIRN.Distance = 0;


            reqPlGenEwbByIRN.VehType = "R";
            reqPlGenEwbByIRN.TransName = data.TransName;
            reqPlGenEwbByIRN.ExpShipDtls = new ExportShipDetails();
            reqPlGenEwbByIRN.ExpShipDtls.Addr1 = data.To_Address1;

            //reqPlGenEwbByIRN.ExpShipDtls.Addr2 = string.IsNullOrEmpty(data.To_Address2) ? data.To_Address1 : data.To_Address2;
            reqPlGenEwbByIRN.ExpShipDtls.Addr2 = string.IsNullOrEmpty(data.To_Address2) || data.To_Address2.Length < 3 ? data.To_Address1 : data.To_Address2;

            reqPlGenEwbByIRN.ExpShipDtls.Loc = data.To_Place;
            reqPlGenEwbByIRN.ExpShipDtls.Pin = Convert.ToInt32(data.To_PinCode);
            reqPlGenEwbByIRN.ExpShipDtls.Stcd = data.To_GSTIN.Substring(0, 2);
            reqPlGenEwbByIRN.DispDtls = new DispatchedDetails();
            reqPlGenEwbByIRN.DispDtls.Nm = data.From_OtherPartyName;
            reqPlGenEwbByIRN.DispDtls.Addr1 = data.From_Address1;
            reqPlGenEwbByIRN.DispDtls.Addr2 = string.IsNullOrEmpty(data.From_Address2) ? data.From_Address1 : data.From_Address2;
            reqPlGenEwbByIRN.DispDtls.Loc = data.From_Place;
            reqPlGenEwbByIRN.DispDtls.Pin = Convert.ToInt32(data.From_PinCode);
            reqPlGenEwbByIRN.DispDtls.Stcd = data.From_GSTIN.Substring(0, 2);
            //}
            TaxProEInvoice.API.TxnRespWithObj<RespPlGenEwbByIRN> txnRespWithObj = Task.Run(() => eInvoiceAPI.GenEwbByIRNAsync(eInvSession, reqPlGenEwbByIRN)).Result;
            string ErrorCodes = "";
            string ErrorDesc = "";

            var logFilePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(@"~/EWBPrintPdf/log.txt"));
            using (StreamWriter w = System.IO.File.AppendText(logFilePath))
            {
                string response = JsonConvert.SerializeObject(txnRespWithObj.RespObj);
                Log(string.Format("Generate EWB : {0} \n\n ResultObject : {1}", txnRespWithObj.TxnOutcome, response), w);
            }

            if (txnRespWithObj.IsSuccess)
            {
                var responce = JsonConvert.SerializeObject(txnRespWithObj.RespObj);
                long EwbNo = 0;
                EwbNo = txnRespWithObj.RespObj.EwbNo;
                eWBResponse.OrderId = orderId;
                eWBResponse.InvoiceNumber = invoiceNumber;
                eWBResponse.EWayBillNumber = EwbNo;
                eWBResponse.EWBDate = Convert.ToDateTime(txnRespWithObj.RespObj.EwbDt);
                eWBResponse.EWBValidTill = Convert.ToDateTime(txnRespWithObj.RespObj.EwbValidTill);
                eWBResponse.CreatedBy = UserId;
                _orderService.AddEWayBill(eWBResponse);

                UpdateEWBNumber(invoiceNumber, EwbNo);
                EWBDetailedPrint(EwbNo);
            }
            else
            {
                if (txnRespWithObj.ErrorDetails != null)
                {
                    foreach (TaxProEInvoice.API.RespErrDetailsPl errPl in txnRespWithObj.ErrorDetails)
                    {
                        ErrorCodes += errPl.ErrorCode + ",";
                        ErrorDesc += errPl.ErrorCode + ": " + errPl.ErrorMessage + Environment.NewLine;
                    }
                }
            }
            return eWBResponse;
        }



        public void UpdateEWBNumber(string invoiceNumber, long EwbNo)
        {
            SqlCommand cmdGet = new SqlCommand();
            BaseSqlManager objBaseSqlManager = new BaseSqlManager();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "UpdateEWBNumber";
            cmdGet.Parameters.AddWithValue("@InvoiceNo", invoiceNumber);
            cmdGet.Parameters.AddWithValue("@EwbNo", EwbNo.ToString());
            objBaseSqlManager.ExecuteNonQuery(cmdGet);
            objBaseSqlManager.ForceCloseConnection();
        }


        //public void EWBDetailedPrint(long EwbNo = 0)
        //{
        //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


        //    var pdfFolderPath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(@"~/EWBPrintPdf/"));
        //    string filename = EwbNo + ".pdf";

        //    if (System.IO.File.Exists(pdfFolderPath + filename))
        //    {
        //        string path = string.Empty;
        //        if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
        //        {
        //            path = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/EWBPrintPdf/" + filename;
        //        }
        //        else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
        //        {
        //            path = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/EWBPrintPdf/" + filename;
        //        }
        //        else
        //        {
        //            path = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/EWBPrintPdf/" + filename;
        //        }
        //        WebRequest webRequest = WebRequest.Create(path);
        //        webRequest.Method = WebRequestMethods.Ftp.UploadFile;
        //        Stream reqStream = webRequest.GetRequestStream();
        //        reqStream.Close();
        //        Process.Start(path);
        //    }
        //    else
        //    {
        //        SaveSettings();
        //        SaveLoginDetails();
        //        SaveEWBLoginDetails();
        //        SaveEWBSettings();

        //        TxnRespWithObjAndInfo<RespGetEWBDetail> TxnResp = Task.Run(() => EWBAPI.GetEWBDetailAsync(EwbSession, EwbNo)).Result;


        //        string strOutcome = "Errors in Printing E-Way Bill.";

        //        TxnResp.RespObj.transactionType = 1;
        //        RestClient client = new RestClient(EWBSession.GspApiBaseUrl + "/printewb");
        //        RestRequest request = new RestRequest(Method.POST);

        //        request.AddHeader("Gstin", EwbSession.EwbApiLoginDetails.EwbGstin);
        //        request.AddHeader("aspid", EwbSession.EwbApiSetting.AspUserId);
        //        request.AddHeader("password", EwbSession.EwbApiSetting.AspPassword);
        //        request.AddHeader("Content-Type", "application/json");//"application/json; charset=utf-8");
        //        request.RequestFormat = RestSharp.DataFormat.Json;
        //        request.DateFormat = RestSharp.DataFormat.Json.ToString();
        //        request.AddJsonBody(TxnResp.RespObj);
        //        //string strewbDetail = JsonConvert.SerializeObject(TxnResp.RespObj);
        //        byte[] res = client.DownloadData(request);
        //        try
        //        {
        //            string path = string.Empty;
        //            if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
        //            {
        //                path = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/EWBPrintPdf/" + filename;
        //            }
        //            else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
        //            {
        //                path = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/EWBPrintPdf/" + filename;
        //            }
        //            else
        //            {
        //                path = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/EWBPrintPdf/" + filename;
        //            }
        //            System.IO.File.WriteAllBytes(pdfFolderPath + filename, res);
        //            WebRequest webRequest = WebRequest.Create(path);
        //            webRequest.Method = WebRequestMethods.Ftp.UploadFile;
        //            Stream reqStream = webRequest.GetRequestStream();
        //            reqStream.Close();
        //            System.Diagnostics.Process.Start(path);
        //            strOutcome = "EWB .pdf generated: " + path;

        //        }
        //        catch (Exception ex)
        //        {
        //            strOutcome = ex.Message;
        //        }
        //    }

        //}


        public void EWBDetailedPrint(long EwbNo = 0)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var pdfFolderPath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(@"~/EWBPrintPdf/"));
            string filename = EwbNo + ".pdf";
            var logFilePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(@"~/EWBPrintPdf/log.txt"));


            //SaveSettings();
            //SaveLoginDetails();
            //SaveEWBLoginDetails();
            //SaveEWBSettings();


            eInvSession = eCred.SaveSettings();
            eInvSession = eCred.SaveLoginDetails();
            EwbSession = eCred.SaveEWBLoginDetails();
            EwbSession = eCred.SaveEWBSettings();


            TxnRespWithObjAndInfo<RespGetEWBDetail> TxnResp = Task.Run(() => EWBAPI.GetEWBDetailAsync(EwbSession, EwbNo)).Result;
            using (StreamWriter w = System.IO.File.AppendText(logFilePath))
            {
                Log(TxnResp.TxnOutcome, w);
                Log(JsonConvert.SerializeObject(TxnResp.RespObj), w);
            }
            string strOutcome = "Errors in Printing E-Way Bill.";
            // TxnResp.RespObj.fromGstin = "27AAAFV3761F1Z7";
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

        public ActionResult EWayBillPrint(long EwbNo = 0)
        {
            string url = string.Empty;
            var pdfFolderPath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(@"~/EWBPrintPdf/"));
            string filename = EwbNo + ".pdf";
            if (System.IO.File.Exists(pdfFolderPath + filename))
            {

                if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
                {
                    url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":6551/EWBPrintPdf/" + filename;
                }
                else if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.staging.ToString())
                {
                    url = "http://" + System.Web.HttpContext.Current.Request.Url.Host + "/EWBPrintPdf/" + filename;
                }
                else
                {
                    url = "https://" + System.Web.HttpContext.Current.Request.Url.Host + "/EWBPrintPdf/" + filename;
                }
            }
            else
            {
                EWBDetailedPrint(EwbNo);
            }
            return Json(url, JsonRequestBehavior.AllowGet);
        }

        public void Log(string logMessage, TextWriter w)
        {
            w.Write("\r\nLog Entry : ");
            w.WriteLine(string.Format("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString()));
            //w.WriteLine("  :");
            w.WriteLine(string.Format("  :{0}", logMessage));
            w.WriteLine("=====================================");
        }


    }
}

