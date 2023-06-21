using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using vb.Data;
using vb.Data.ViewModel;
using WAProAPI;

namespace vb.Service.Common
{
    public class WAAPI
    {
        public void SendWAMessage(string message, string CellNo)
        {
            try
            {
                SendTextMsgJson body = new SendTextMsgJson { text = message, sendLinkPreview = false };
                TxnRespWithSendMessageDtls txnResp = System.Threading.Tasks.Task.Run(() => APIMethods.SendTextMessageAsync("91" + CellNo, body)).Result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string SendEmailToCustomer(CustomerDetailModel data, SendMessageData paymentData)
        {
            try
            {
                string FromMail = ConfigurationManager.AppSettings["FromMail"];

                string Tomail = data.Email;
                MailMessage mailmessage = new MailMessage();
                mailmessage.From = new MailAddress(FromMail);
                mailmessage.Subject = "Payment Updation From Viraki Brothers";

                string msg = string.Empty;
                msg += "Hello " + data.Name + ",\n\n";
                msg += "Thank you for payment of Rs. " + paymentData.ByCash + " Bill No. " + paymentData.InvoiceNumber + " By ";

                if (paymentData.ByCheque)
                {
                    msg += "Cheque.";
                    if (!string.IsNullOrEmpty(paymentData.BankName))
                    {
                        msg += "\n Bank Name : " + paymentData.BankName;
                    }

                    if (!string.IsNullOrEmpty(paymentData.BankBranch))
                    {
                        msg += "\n Bank Branch : " + paymentData.BankBranch;
                    }

                    if (!string.IsNullOrEmpty(paymentData.ChequeNo))
                    {
                        msg += "\n Cheque No : " + paymentData.ChequeNo;
                    }

                    if (!string.IsNullOrEmpty(paymentData.ChequeDate.ToString()))
                    {
                        string day = paymentData.ChequeDate.Value.Day.ToString();
                        string month = paymentData.ChequeDate.Value.Month.ToString();
                        string year = paymentData.ChequeDate.Value.Year.ToString();

                        msg += "\n Cheque Date : " + (day + "/" + month + "/" + year);
                    }

                    if (!string.IsNullOrEmpty(paymentData.IFCCode))
                    {
                        msg += "\n IFC Code : " + paymentData.IFCCode;
                    }

                }
                else if (paymentData.ByCard)
                {
                    msg += "Card.";
                    if (!string.IsNullOrEmpty(paymentData.BankNameForCard))
                    {
                        msg += "\n Bank Name : " + paymentData.BankNameForCard;
                    }
                    if (!string.IsNullOrEmpty(paymentData.TypeOfCard))
                    {
                        msg += "\n Type Of Card : " + paymentData.TypeOfCard;
                    }
                }
                else if (paymentData.ByOnline)
                {
                    msg += "Online.";
                    if (!string.IsNullOrEmpty(paymentData.BankNameForCard))
                    {
                        msg += "\n Bank Name : " + paymentData.BankNameForCard;
                    }
                    if (!string.IsNullOrEmpty(paymentData.UTRNumber))
                    {
                        msg += "\n UTR Number : " + paymentData.UTRNumber;
                    }
                    if (!string.IsNullOrEmpty(paymentData.OnlinePaymentDate.ToString()))
                    {
                        string day = paymentData.OnlinePaymentDate.Value.Day.ToString();
                        string month = paymentData.OnlinePaymentDate.Value.Month.ToString();
                        string year = paymentData.OnlinePaymentDate.Value.Year.ToString();

                        msg += "\n Online Payment Date : " + (day + "/" + month + "/" + year);
                    }
                }
                else
                {
                    msg += "Cash.";
                }

                if (!string.IsNullOrEmpty(data.CellNo))
                {
                    SendWAMessage(msg, data.CellNo);
                }

                //With Table

                mailmessage.Body = msg;
                mailmessage.To.Add(Tomail);
                // mailmessage.CC.Add(System.Configuration.ConfigurationManager.AppSettings["CCMail"]);
                mailmessage.IsBodyHtml = true;
                var smtp = new System.Net.Mail.SmtpClient();
                {
                    smtp = GetSMPTP(smtp);
                }
                try
                {
                    if (!string.IsNullOrEmpty(data.Email))
                    {
                        smtp.Send(mailmessage);
                    }
                }
                catch
                {
                }
            }
            catch
            {
            }
            return "Send Email Sent succesfully..";
        }

        public static System.Net.Mail.SmtpClient GetSMPTP(System.Net.Mail.SmtpClient OLDSMTP)
        {
            string FromMail = ConfigurationManager.AppSettings["FromMail"];
            string FromPassword = ConfigurationManager.AppSettings["FromPassword"];
            //OLDSMTP.Host = "smtp.gmail.com";
            //if (FromMail == "purchase@virakibrothers.com")
            if (FromMail == "purchase@virakibrothers.com")
            {
                OLDSMTP.Host = "smtp.office365.com";
            }
            else
            {
                OLDSMTP.Host = "smtp.gmail.com";
            }
            OLDSMTP.Port = 587;
            OLDSMTP.EnableSsl = true;
            OLDSMTP.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            OLDSMTP.Credentials = new System.Net.NetworkCredential(FromMail, FromPassword);
            OLDSMTP.Timeout = 20000;
            return OLDSMTP;
        }

    }
}
