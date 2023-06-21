using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxProEInvoice.API;
using TaxProEWB.API;
using vb.Data;
using vb.Data.Model;
using vb.Data.ViewModel;

namespace vb.Service.Common
{
    public class ECredentials
    {
        public eInvoiceSession eInvSession = new eInvoiceSession(false, false);

        public EWBSession EwbSession = new EWBSession();

        public eInvoiceSession SaveSettings()
        {
            eInvSession.eInvApiSetting = new eInvoiceAPISetting();

            eInvSession.eInvApiSetting.GSPName = "TaxPro_Production";
            eInvSession.eInvApiSetting.AspUserId = "1658771921";
            eInvSession.eInvApiSetting.AspPassword = "Chetan@334";
            eInvSession.eInvApiSetting.client_id = "";
            eInvSession.eInvApiSetting.client_secret = "";
            // eInvSession.eInvApiSetting.AuthUrl = "https://einvapi.charteredinfo.com/eivital/v1.03";
            eInvSession.eInvApiSetting.AuthUrl = "https://einvapi.charteredinfo.com/eivital/v1.04";
            eInvSession.eInvApiSetting.BaseUrl = "https://einvapi.charteredinfo.com/eicore/v1.03";
            eInvSession.eInvApiSetting.EwbByIRN = "https://einvapi.charteredinfo.com/eiewb/v1.03";
            eInvSession.eInvApiSetting.CancelEwbUrl = "https://einvapi.charteredinfo.com/v1.03";

            return eInvSession;
        }

        public eInvoiceSession SaveLoginDetails()
        {
            eInvSession.eInvApiLoginDetails = new eInvoiceAPILoginDetails();
            eInvSession.eInvApiLoginDetails.UserName = ConfigurationManager.AppSettings["UserName"];
            eInvSession.eInvApiLoginDetails.Password = ConfigurationManager.AppSettings["Password"];
            eInvSession.eInvApiLoginDetails.GSTIN = "27AAAFV3761F1Z7";
            eInvSession.eInvApiLoginDetails.AppKey = "RvCyJnJZDGGd3EaxDyK+SAUtkAS/GqO4CsH+ScjykDU=";

            double TokenExpiredHours = Convert.ToDouble(ConfigurationManager.AppSettings["TokenExpiredHours"]);

            string Sek = "";
            string AuthToken = "";
            var detail = GetOldAuthToken();
            if (string.IsNullOrEmpty(detail.AuthToken))
            {
                // Generate Authentication Token
                Authentication obj2 = new Authentication();
                obj2 = GetAuthToken();

                // Insert Authentication Token
                DateTime ExpiredTime = DateTime.Now.AddHours(TokenExpiredHours);
                AuthToken_Mst obj = new AuthToken_Mst();


                //obj.ExpiredOn = ExpiredTime;
                obj.ExpiredOn = obj2.ExpiredOn;

                obj.Sek = obj2.Sek;
                obj.AuthToken = obj2.AuthToken;
                bool respose = AddAuthToken(obj);
                Sek = obj.Sek;
                AuthToken = obj.AuthToken;
            }
            else if (DateTime.Now < detail.ExpiredOn)
            {
                AuthToken = detail.AuthToken;
                Sek = detail.Sek;
            }
            else
            {
                // Generate Authentication Token
                Authentication obj2 = new Authentication();
                obj2 = GetAuthToken();

                // Update Authentication
                DateTime ExpiredTime = DateTime.Now.AddHours(TokenExpiredHours);
                AuthToken_Mst obj = new AuthToken_Mst();
                obj.AuthTokenID = detail.AuthTokenID;


                //obj.ExpiredOn = ExpiredTime;
                obj.ExpiredOn = obj2.ExpiredOn;


                obj.Sek = obj2.Sek;
                obj.AuthToken = obj2.AuthToken;
                bool respose = AddAuthToken(obj);
                Sek = obj.Sek;
                AuthToken = obj.AuthToken;
            }

            eInvSession.eInvApiLoginDetails.AuthToken = AuthToken;
            eInvSession.eInvApiLoginDetails.Sek = Sek;
            //eInvSession.eInvApiLoginDetails.E_InvoiceTokenExp = Convert.ToDateTime("3/23/2021 11:57:40 AM");
            eInvSession.eInvApiLoginDetails.E_InvoiceTokenExp = detail.ExpiredOn;

            return eInvSession;
        }

        public EWBSession SaveEWBSettings()
        {
            eInvSession.eInvApiSetting = new eInvoiceAPISetting();


            EwbSession.EwbApiSetting.GSPName = "TaxPro_Production";
            EwbSession.EwbApiSetting.AspUserId = "1658771921";
            EwbSession.EwbApiSetting.AspPassword = "Chetan@334";
            EwbSession.EwbApiSetting.EWBClientId = "";
            EwbSession.EwbApiSetting.EWBClientSecret = "";
            EwbSession.EwbApiSetting.EWBGSPUserID = "";
            EwbSession.EwbApiSetting.BaseUrl = "https://einvapi.charteredinfo.com/v1.03";

            return EwbSession;
        }

        public EWBSession SaveEWBLoginDetails()
        {
            EwbSession.EwbApiLoginDetails.EwbGstin = eInvSession.eInvApiLoginDetails.GSTIN;
            EwbSession.EwbApiLoginDetails.EwbUserID = ConfigurationManager.AppSettings["UserName"];
            EwbSession.EwbApiLoginDetails.EwbPassword = ConfigurationManager.AppSettings["Password"];
            EwbSession.EwbApiLoginDetails.EwbAppKey = eInvSession.eInvApiLoginDetails.AppKey;

            double TokenExpiredHours = Convert.ToDouble(ConfigurationManager.AppSettings["TokenExpiredHours"]);
            EwbSession.EwbApiLoginDetails.EwbAuthToken = eInvSession.eInvApiLoginDetails.AuthToken;

            //EwbSession.EwbApiLoginDetails.EwbTokenExp = Convert.ToDateTime("3/23/2021 11:57:40 AM");
            EwbSession.EwbApiLoginDetails.EwbTokenExp = eInvSession.eInvApiLoginDetails.E_InvoiceTokenExp;

            EwbSession.EwbApiLoginDetails.EwbSEK = eInvSession.eInvApiLoginDetails.Sek;

            return EwbSession;
        }

        public AuthTokenDetail GetOldAuthToken()
        {
            SqlCommand cmdGet = new SqlCommand();
            BaseSqlManager objBaseSqlManager = new BaseSqlManager();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "GetAuthToken";
            SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
            AuthTokenDetail obj = new AuthTokenDetail();
            while (dr.Read())
            {
                obj.AuthTokenID = objBaseSqlManager.GetInt64(dr, "AuthTokenID");
                obj.AuthToken = objBaseSqlManager.GetTextValue(dr, "AuthToken");
                obj.Sek = objBaseSqlManager.GetTextValue(dr, "Sek");
                obj.ExpiredOn = objBaseSqlManager.GetDateTime(dr, "ExpiredOn");
            }
            dr.Close();
            objBaseSqlManager.ForceCloseConnection();
            return obj;
        }

        public Authentication GetAuthToken()
        {
            Authentication obj = new Authentication();
            string AuthTokenResponse = string.Empty;
            TaxProEInvoice.API.TxnRespWithObj<eInvoiceSession> txnRespWithObj = Task.Run(() => eInvoiceAPI.GetAuthTokenAsync(eInvSession)).Result;
            // here store in DB
            obj.AuthToken = txnRespWithObj.RespObj.eInvApiLoginDetails.AuthToken;
            obj.Sek = txnRespWithObj.RespObj.eInvApiLoginDetails.Sek;

            // 21/05/2021 Get Expired date
            obj.ExpiredOn = txnRespWithObj.RespObj.eInvApiLoginDetails.E_InvoiceTokenExp;

            return obj;
        }

        public bool AddAuthToken(AuthToken_Mst ObjEvent)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (ObjEvent.AuthTokenID == 0)
                {
                    context.AuthToken_Mst.Add(ObjEvent);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(ObjEvent).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (ObjEvent.AuthTokenID > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void Log(string logMessage, TextWriter w)
        {
            w.Write("\r\nLog Entry : ");
            w.WriteLine(string.Format("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString()));
            w.WriteLine(string.Format("  :{0}", logMessage));
            w.WriteLine("==================================================================");
        }


    }
}
