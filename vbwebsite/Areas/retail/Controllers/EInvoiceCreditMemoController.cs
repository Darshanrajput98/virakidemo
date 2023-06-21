using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TaxProEInvoice.API;
using vb.Data;
using vb.Data.Model;
using vb.Data.ViewModel;
using vb.Service;
using vb.Service.Common;
using ZXing;

namespace vbwebsite.Areas.retail.Controllers
{
    public class EInvoiceCreditMemoController : Controller
    {
        private static object Lock = new object();
        private RetOrderServices _orderService = new RetOrderServices();
        eInvoiceSession eInvSession = new eInvoiceSession(false, false);

        ECredentials eCred = new ECredentials();

        // GET: /retail/EInvoice/
        public ActionResult Index()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            // SaveSettings();
            // SaveLoginDetails();
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
        //    eInvSession.eInvApiSetting.AuthUrl = "https://einvapi.charteredinfo.com/eivital/v1.03";  //"https://api.taxprogsp.co.in/eivital/v1.03";
        //    eInvSession.eInvApiSetting.BaseUrl = "https://einvapi.charteredinfo.com/eicore/v1.03";   //"https://api.taxprogsp.co.in/eicore/v1.03";
        //    eInvSession.eInvApiSetting.EwbByIRN = "https://einvapi.charteredinfo.com/eiewb/v1.03";   //"https://api.taxprogsp.co.in/eiewb/v1.03";
        //    eInvSession.eInvApiSetting.CancelEwbUrl = "https://einvapi.charteredinfo.com/v1.03";   //"https://api.taxprogsp.co.in/v1.03";
        //    //Shared.SaveAPISetting(eInvSession.eInvApiSetting);
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
        //    // Shared.SaveAPILoginDetails(eInvSession.eInvApiLoginDetails);
        //}

        //public Authentication GetAuthToken()
        //{
        //    Authentication obj = new Authentication();
        //    string AuthTokenResponse = string.Empty;
        //    TxnRespWithObj<eInvoiceSession> txnRespWithObj = Task.Run(() => eInvoiceAPI.GetAuthTokenAsync(eInvSession)).Result;
        //    // here store in DB
        //    obj.AuthToken = txnRespWithObj.RespObj.eInvApiLoginDetails.AuthToken;
        //    obj.Sek = txnRespWithObj.RespObj.eInvApiLoginDetails.Sek;

        //    // 21/05/2021 Get Expired date
        //    obj.ExpiredOn = txnRespWithObj.RespObj.eInvApiLoginDetails.E_InvoiceTokenExp;

        //    return obj;
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




        public RetEInvoiceCreditMemo GenIRN(RetOrderQtyInvoiceList orderData, List<RetOrderQtyInvoiceList> lstData)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            RetEInvoiceCreditMemo eInvoice = new RetEInvoiceCreditMemo();


            //SaveSettings();
            //SaveLoginDetails();

            eInvSession = eCred.SaveSettings();
            eInvSession = eCred.SaveLoginDetails();


            Int64 orderId = 0;
            double totalAmount = 0;
            double totalTaxAmount = 0;
            double totalIGSTTaxAmount = 0;
            string invoiceNumber = string.Empty;
            invoiceNumber = lstData[0].InvoiceNumber;
            orderId = lstData[0].OrderID;


            ReqPlGenIRN reqPlGenIRN = new ReqPlGenIRN();
            reqPlGenIRN.Version = "1.1";
            reqPlGenIRN.TranDtls = new ReqPlGenIRN.TranDetails();
            reqPlGenIRN.TranDtls.TaxSch = "GST";
            reqPlGenIRN.TranDtls.SupTyp = "B2B";
            reqPlGenIRN.DocDtls = new ReqPlGenIRN.DocSetails();
            reqPlGenIRN.DocDtls.Typ = "INV";
            reqPlGenIRN.DocDtls.No = lstData[0].InvoiceNumber;
            reqPlGenIRN.DocDtls.Dt = lstData[0].DocDate;
            reqPlGenIRN.SellerDtls = new ReqPlGenIRN.SellerDetails();
            reqPlGenIRN.SellerDtls.Gstin = orderData.From_GSTIN;
            reqPlGenIRN.SellerDtls.LglNm = orderData.From_Name;
            reqPlGenIRN.SellerDtls.TrdNm = null;
            reqPlGenIRN.SellerDtls.Addr1 = orderData.From_Address1;
            reqPlGenIRN.SellerDtls.Addr2 = null;
            reqPlGenIRN.SellerDtls.Loc = orderData.From_Place;
            reqPlGenIRN.SellerDtls.Pin = orderData.From_PinCode;
            reqPlGenIRN.SellerDtls.Stcd = orderData.From_StateCode;
            reqPlGenIRN.SellerDtls.Ph = null;
            reqPlGenIRN.SellerDtls.Em = null;
            reqPlGenIRN.BuyerDtls = new ReqPlGenIRN.BuyerDetails();
            reqPlGenIRN.BuyerDtls.Gstin = orderData.TaxNo;
            reqPlGenIRN.BuyerDtls.LglNm = orderData.CustomerName;
            reqPlGenIRN.BuyerDtls.TrdNm = null;
            reqPlGenIRN.BuyerDtls.Pos = orderData.StateCode;
            reqPlGenIRN.BuyerDtls.Addr1 = orderData.ShipTo;
            reqPlGenIRN.BuyerDtls.Addr2 = null;
            reqPlGenIRN.BuyerDtls.Loc = orderData.State;
            reqPlGenIRN.BuyerDtls.Pin = Convert.ToInt32(orderData.PinCode);
            reqPlGenIRN.BuyerDtls.Stcd = orderData.StateCode;
            reqPlGenIRN.BuyerDtls.Ph = null;
            reqPlGenIRN.BuyerDtls.Em = null;
            reqPlGenIRN.DispDtls = new ReqPlGenIRN.DispatchedDetails();
            reqPlGenIRN.DispDtls.Nm = orderData.From_Name;
            reqPlGenIRN.DispDtls.Addr1 = orderData.From_Address1;
            reqPlGenIRN.DispDtls.Addr2 = null;
            reqPlGenIRN.DispDtls.Loc = orderData.From_Place;
            reqPlGenIRN.DispDtls.Pin = orderData.From_PinCode;
            reqPlGenIRN.DispDtls.Stcd = orderData.From_StateCode;
            reqPlGenIRN.ShipDtls = new ReqPlGenIRN.ShippedDetails();
            reqPlGenIRN.ShipDtls.Gstin = orderData.TaxNo;
            reqPlGenIRN.ShipDtls.LglNm = orderData.CustomerName;
            reqPlGenIRN.ShipDtls.TrdNm = null;
            reqPlGenIRN.ShipDtls.Addr1 = orderData.ShipTo;
            reqPlGenIRN.ShipDtls.Addr2 = null;
            reqPlGenIRN.ShipDtls.Loc = orderData.State;
            reqPlGenIRN.ShipDtls.Pin = Convert.ToInt32(orderData.PinCode);
            reqPlGenIRN.ShipDtls.Stcd = orderData.StateCode;
            reqPlGenIRN.ItemList = new List<ReqPlGenIRN.ItmList>();


            ReqPlGenIRN.ItmList itm = new ReqPlGenIRN.ItmList();
            int rowNumber = 2;
            foreach (var data in lstData)
            {
                if (data.ProductName != "")
                {
                    itm = new ReqPlGenIRN.ItmList();
                    itm.SlNo = rowNumber.ToString();
                    itm.IsServc = "N";
                    itm.PrdDesc = null;
                    itm.HsnCd = data.HSNNumber;
                    itm.BchDtls = null;
                    itm.Qty = Convert.ToDouble(data.Quantity);
                    itm.Unit = "NOS";
                    itm.UnitPrice = Convert.ToDouble(data.ProductPrice);
                    itm.TotAmt = Math.Round(Convert.ToDouble(data.TotalAmount), 2); // 22 April, 2021 Piyush Limbani (999999999999.99)
                    itm.Discount = 0;//Convert.ToDouble(data.BillDiscount);
                    itm.AssAmt = Math.Round(Convert.ToDouble(data.TotalAmount), 2); // 22 April, 2021 Piyush Limbani (999999999999.99)
                    if (data.TaxName == "IGST")
                        itm.GstRt = Convert.ToDouble(data.IGSTTaxRate);
                    else
                        itm.GstRt = Convert.ToDouble(data.SGSTTaxRate + data.CGSTTaxRate);
                    itm.SgstAmt = Convert.ToDouble(data.SGSTAmount);
                    itm.IgstAmt = Convert.ToDouble(data.IGSTAmount);
                    itm.CgstAmt = Convert.ToDouble(data.CGSTAmount);
                    itm.CesRt = Convert.ToDouble(data.CESSTaxRate);
                    itm.CesAmt = Convert.ToDouble(data.CESSAmount);
                    itm.CesNonAdvlAmt = Convert.ToDouble(data.Cess_Non_Advol_Amount);
                    itm.StateCesRt = 0.0;
                    itm.StateCesAmt = 0.0;
                    itm.StateCesNonAdvlAmt = 0.0;
                    itm.OthChrg = 0.0;
                    itm.TotItemVal = Convert.ToDouble(data.FinalTotal);
                    itm.AttribDtls = null;
                    totalAmount += itm.AssAmt;
                    if (data.TaxName == "IGST")
                    {
                        totalIGSTTaxAmount += Math.Round(Convert.ToDouble(data.IGSTAmount), 2);
                    }
                    else
                    {
                        totalTaxAmount += Math.Round((Convert.ToDouble(data.SGSTAmount) + Convert.ToDouble(data.CGSTAmount)), 2);
                    }
                    reqPlGenIRN.ItemList.Add(itm);
                    rowNumber += 1;
                }
            }
            reqPlGenIRN.PayDtls = null;
            reqPlGenIRN.RefDtls = null;
            reqPlGenIRN.AddlDocDtls = null;
            reqPlGenIRN.ExpDtls = null;
            reqPlGenIRN.EwbDtls = null;
            reqPlGenIRN.ValDtls = new ReqPlGenIRN.ValDetails();
            reqPlGenIRN.ValDtls.AssVal = Math.Round(totalAmount, 2);
            reqPlGenIRN.ValDtls.CgstVal = Math.Round((totalTaxAmount / 2), 2);
            reqPlGenIRN.ValDtls.SgstVal = Math.Round((totalTaxAmount / 2), 2);
            reqPlGenIRN.ValDtls.IgstVal = Math.Round(totalIGSTTaxAmount, 2);
            reqPlGenIRN.ValDtls.CesVal = Convert.ToDouble(lstData[0].CESSAmount);
            reqPlGenIRN.ValDtls.StCesVal = 0.0;
            reqPlGenIRN.ValDtls.RndOffAmt = 0.0;
            reqPlGenIRN.ValDtls.TotInvVal = Math.Round((totalTaxAmount + totalIGSTTaxAmount + totalAmount), 2);


            string name = invoiceNumber.Replace('/', '-');
            //string path = Path.Combine(Server.MapPath("~/QRCode/") + name);


            string result = "";
            string pas = JsonConvert.SerializeObject(reqPlGenIRN);
            TxnRespWithObj<RespPlGenIRN> txnRespWithObj = Task.Run(() => eInvoiceAPI.GenIRNAsync(eInvSession, pas, 150)).Result;

            //Console.Write(result);

            RespPlGenIRN respPlGenIRN = txnRespWithObj.RespObj;
            string ErrorCodes = "";
            string ErrorDesc = "";
            //rtbResponce.Text = "";
            if (txnRespWithObj.IsSuccess)
            {
                //Process the Response here
                //For DEMO, just showing the searilized Response Object in text box
                result = JsonConvert.SerializeObject(respPlGenIRN.ExtractedSignedQrCode);

                //string path1 = Path.Combine(Server.MapPath("~/QRCode/"), name);
                //string path = @"F:\VirakiBrothers\VirakiBrothers\Source\vbwebsite\QRCode\";
                //path = path + name + ".png";


                string DestinationFile = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/QRCode/") + name + ".png");


                //string name = path + reqPlGenIRN.DocDtls.No + ".png";
                respPlGenIRN.QrCodeImage.Save(DestinationFile);

                var barcodeWriter = new BarcodeWriter();
                barcodeWriter.Format = BarcodeFormat.QR_CODE;

                var res = barcodeWriter.Write(result);

                var barcodeBitmap = new Bitmap(res);
                using (MemoryStream memory = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(DestinationFile, FileMode.Create, FileAccess.ReadWrite))
                    {
                        barcodeBitmap.Save(memory, ImageFormat.Png);
                        byte[] bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                    }
                }

                var barcodeReader = new BarcodeReader();

                var resultDecode = barcodeReader.Decode(new Bitmap(DestinationFile));
                if (resultDecode != null)
                {
                    respPlGenIRN.ExtractedSignedQrCode = JsonConvert.DeserializeObject<RespGenIRNQrCodeData>(resultDecode.Text);
                    respPlGenIRN.SignedQRCode = resultDecode.Text;
                }


                //Store respPlGenIRN (manditory - AckDate and SignedInvoice) to verify signed invoice
                //below code is to show how to verify signed invoice
                respPlGenIRN.QrCodeImage = null;
                //TxnRespWithObj<VerifyRespPl> txnRespWithObj1 = Task.Run(() => eInvoiceAPI.VerifySignedInvoice(eInvSession, respPlGenIRN)).Result;


                //VerifyRespPl verifyRespPl = new VerifyRespPl();
                //if (txnRespWithObj.IsSuccess)
                //{
                //    verifyRespPl.IsVerified = txnRespWithObj1.RespObj.IsVerified;
                //    verifyRespPl.JwtIssuerIRP = txnRespWithObj1.RespObj.JwtIssuerIRP;
                //    verifyRespPl.VerifiedWithCertificateEffectiveFrom = txnRespWithObj1.RespObj.VerifiedWithCertificateEffectiveFrom;
                //    verifyRespPl.CertificateName = txnRespWithObj1.RespObj.CertificateName;
                //    verifyRespPl.CertStartDate = txnRespWithObj1.RespObj.CertStartDate;
                //    verifyRespPl.CertExpiryDate = txnRespWithObj1.RespObj.CertExpiryDate;
                //}

                eInvoice.ReturnOrderId = orderId;
                eInvoice.CreditMemoNumber = invoiceNumber;
                eInvoice.IRN = respPlGenIRN.Irn;
                eInvoice.QRCode = name + ".png";
                eInvoice.AckDt = Convert.ToDateTime(respPlGenIRN.AckDt);
                eInvoice.AckNo = Convert.ToInt64(respPlGenIRN.AckNo);
                eInvoice.CreatedBy = orderData.CreatedBy;
                _orderService.AddRetEInvoiceCreditMemo(eInvoice);

            }
            else
            {
                //Error has occured
                //Display TxnOutCome in text box - process or show msg to user

                //Process error codes
                if (txnRespWithObj.ErrorDetails != null)
                {
                    foreach (RespErrDetailsPl errPl in txnRespWithObj.ErrorDetails)
                    {
                        //Process errPl item here
                        ErrorCodes += errPl.ErrorCode + ",";
                        ErrorDesc += errPl.ErrorCode + ": " + errPl.ErrorMessage + Environment.NewLine;
                        result = ErrorDesc;
                    }
                }

                //Process InfoDetails here
                RespInfoDtlsPl respInfoDtlsPl = new RespInfoDtlsPl();
                //Serialize Desc object from InfoDtls as per InfCd
                if (txnRespWithObj.InfoDetails != null)
                {
                    foreach (RespInfoDtlsPl infoPl in txnRespWithObj.InfoDetails)
                    {
                        var strDupIrnPl = JsonConvert.SerializeObject(infoPl.Desc);   //Convert object type to json string
                        switch (infoPl.InfCd)
                        {
                            case "DUPIRN":
                                DupIrnPl dupIrnPl = JsonConvert.DeserializeObject<DupIrnPl>(strDupIrnPl);
                                break;
                            case "EWBERR":
                                List<EwbErrPl> ewbErrPl = JsonConvert.DeserializeObject<List<EwbErrPl>>(strDupIrnPl);
                                break;
                            case "ADDNLNFO":
                                //Deserialize infoPl.Desc as string type and then if this string contains json object, it may be desirilized again as per future releases
                                string strDesc = (string)infoPl.Desc;
                                break;
                        }
                    }
                }
            }
            Console.Write(result);
            return eInvoice;
        }







        // 08 April 2021 Piyush Limbani
        public bool DeleteEInvoiceCreditMemo(string IRN, string CreditMemoNumber, bool IsDelete, long UpdatedBy, DateTime UpdatedOn)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // SaveSettings();
            // SaveLoginDetails();

            eInvSession = eCred.SaveSettings();
            eInvSession = eCred.SaveLoginDetails();

            ReqPlCancelIRN reqPlCancelIRN = new ReqPlCancelIRN();
            reqPlCancelIRN.CnlRem = "Wrong entry";
            reqPlCancelIRN.CnlRsn = "1";
            reqPlCancelIRN.Irn = IRN;

            TxnRespWithObj<RespPlCancelIRN> txnRespWithObj = Task.Run(() => eInvoiceAPI.CancelIRNIRNAsync(eInvSession, reqPlCancelIRN)).Result;
            bool success = false;
            if (txnRespWithObj.IsSuccess)
            {
                string Irn = txnRespWithObj.RespObj.Irn;
                string CancelDate = txnRespWithObj.RespObj.CancelDate;

                SqlCommand cmdGet = new SqlCommand();
                BaseSqlManager objBaseSqlManager = new BaseSqlManager();
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteRetEInvoiceCreditMemo";
                cmdGet.Parameters.AddWithValue("@CreditMemoNumber", CreditMemoNumber);
                cmdGet.Parameters.AddWithValue("@IsCancel", true);
                cmdGet.Parameters.AddWithValue("@UpdatedBy", UpdatedBy);
                cmdGet.Parameters.AddWithValue("@UpdatedOn", UpdatedOn);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();

                SqlCommand cmdGet1 = new SqlCommand();
                BaseSqlManager objBaseSqlManager1 = new BaseSqlManager();
                cmdGet1.CommandType = CommandType.StoredProcedure;
                cmdGet1.CommandText = "DeleteRetailCreditMemo";
                cmdGet1.Parameters.AddWithValue("@CreditMemoNumber", CreditMemoNumber);
                cmdGet1.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr1 = objBaseSqlManager1.ExecuteNonQuery(cmdGet1);
                objBaseSqlManager1.ForceCloseConnection();

                success = true;
            }
            else
            {
                success = false;
            }
            return success;
        }




        //public void SaveSettings()
        //{
        //    eInvSession.eInvApiSetting = new eInvoiceAPISetting();
        //    if (System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString() == HostingEnvironment.localhost.ToString())
        //    {
        //        eInvSession.eInvApiSetting.GSPName = "TaxPro_Sandbox";
        //        eInvSession.eInvApiSetting.AspUserId = "1655841121";
        //        eInvSession.eInvApiSetting.AspPassword = "M@tizS0ft";
        //        eInvSession.eInvApiSetting.client_id = "";
        //        eInvSession.eInvApiSetting.client_secret = "";
        //        eInvSession.eInvApiSetting.AuthUrl = "http://testapi.taxprogsp.co.in/eivital/v1.03";
        //        eInvSession.eInvApiSetting.BaseUrl = "http://testapi.taxprogsp.co.in/eicore/v1.03";
        //        eInvSession.eInvApiSetting.EwbByIRN = "http://testapi.taxprogsp.co.in/eiewb/v1.03";
        //        eInvSession.eInvApiSetting.CancelEwbUrl = "http://testapi.taxprogsp.co.in/v1.03";
        //    }
        //    else
        //    {
        //        eInvSession.eInvApiSetting.GSPName = "TaxPro_Production";
        //        eInvSession.eInvApiSetting.AspUserId = "1658771921";
        //        eInvSession.eInvApiSetting.AspPassword = "Chetan@334";
        //        eInvSession.eInvApiSetting.client_id = "";
        //        eInvSession.eInvApiSetting.client_secret = "";
        //        eInvSession.eInvApiSetting.AuthUrl = "https://api.taxprogsp.co.in/eivital/v1.03";
        //        eInvSession.eInvApiSetting.BaseUrl = "https://api.taxprogsp.co.in/eicore/v1.03";
        //        eInvSession.eInvApiSetting.EwbByIRN = "https://api.taxprogsp.co.in/eiewb/v1.03";
        //        eInvSession.eInvApiSetting.CancelEwbUrl = "https://api.taxprogsp.co.in/v1.03";
        //    }
        //    Shared.SaveAPISetting(eInvSession.eInvApiSetting);
        //}

        //public void SaveLoginDetails()
        //{
        //    eInvSession.eInvApiLoginDetails = new eInvoiceAPILoginDetails();
        //    eInvSession.eInvApiLoginDetails.UserName = ConfigurationManager.AppSettings["UserName"];
        //    eInvSession.eInvApiLoginDetails.Password = ConfigurationManager.AppSettings["Password"];
        //    eInvSession.eInvApiLoginDetails.GSTIN = "27AAAFV3761F1Z7";
        //    eInvSession.eInvApiLoginDetails.AppKey = "RvCyJnJZDGGd3EaxDyK+SAUtkAS/GqO4CsH+ScjykDU=";
        //    eInvSession.eInvApiLoginDetails.AuthToken = GetAuthToken();
        //    eInvSession.eInvApiLoginDetails.Sek = "Nevbc2jtP2UXjsfG9nkrfJA9AqDZTCGcgd8R5TSz5XI=";
        //    eInvSession.eInvApiLoginDetails.E_InvoiceTokenExp = Convert.ToDateTime("3/23/2021 11:57:40 AM");
        //    // Shared.SaveAPILoginDetails(eInvSession.eInvApiLoginDetails);
        //}

        //public string GetAuthToken()
        //{
        //    string AuthTokenResponse = string.Empty;
        //    TxnRespWithObj<eInvoiceSession> txnRespWithObj = Task.Run(() => eInvoiceAPI.GetAuthTokenAsync(eInvSession)).Result;
        //    // here store in DB
        //    return AuthTokenResponse;
        //}
    }
}