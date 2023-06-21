using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vb.Data
{
    public class AddSupplier
    {
        public long SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string GSTNo { get; set; }
        public long TaxID { get; set; }
        public string PanCardNumber { get; set; }
        public string FSSAI { get; set; }
        public DateTime? FSSAIValidUpTo { get; set; }
        public string BankName { get; set; }
        public string Branch { get; set; }
        public string AccountNumber { get; set; }
        public string IFSCCode { get; set; }
        public long TDSCategoryID { get; set; }
        public decimal TDSPercentage { get; set; }
        public string AddressOneLine1 { get; set; }
        public string AddressOneLine2 { get; set; }
        public long AreaIDOne { get; set; }
        public string StateOne { get; set; }
        public string AddressOnePincode { get; set; }
        public string AddressTwoLine1 { get; set; }
        public string AddressTwoLine2 { get; set; }
        public long AreaIDTwo { get; set; }
        public string StateTwo { get; set; }
        public string AddressTwoPincode { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
        public List<SupplierContactDetail> lstContactDetail { get; set; }
        public string NameAsBankAccount { get; set; }
    }

    public class SupplierContactDetail
    {
        public long SupplierContactID { get; set; }
        public long AddressID { get; set; }
        public long SupplierID { get; set; }
        public string Name { get; set; }
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
    }

    public class SupplierListResponse
    {
        public long SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string GSTNo { get; set; }
        public long TaxID { get; set; }
        public string TaxName { get; set; }
        public string PanCardNumber { get; set; }
        public string FSSAI { get; set; }
        public DateTime FSSAIValidUpTo { get; set; }
        public string FSSAIValidUpTostr { get; set; }
        public string BankName { get; set; }
        public string Branch { get; set; }
        public string AccountNumber { get; set; }
        public string IFSCCode { get; set; }
        public long TDSCategoryID { get; set; }
        public string TDSCategory { get; set; }
        public decimal TDSPercentage { get; set; }
        public string AddressOneLine1 { get; set; }
        public string AddressOneLine2 { get; set; }
        public long AreaIDOne { get; set; }
        public string AreaNameOne { get; set; }
        public string StateOne { get; set; }
        public string AddressOnePincode { get; set; }
        public string AddressTwoLine1 { get; set; }
        public string AddressTwoLine2 { get; set; }
        public long AreaIDTwo { get; set; }
        public string AreaNameTwo { get; set; }
        public string StateTwo { get; set; }
        public string AddressTwoPincode { get; set; }
        public string ContactName { get; set; }
        public Boolean IsDelete { get; set; }

        public string AreaOne { get; set; }
        public string AreaTwo { get; set; }
        public long DaysRemaining { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhoneNo { get; set; }
        public string ContactMobileNo { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string NameAsBankAccount { get; set; }
    }

    public class SupplierListExport
    {
        public string SupplierName { get; set; }
        public string GSTNo { get; set; }

        public string TaxName { get; set; }
        public string PanCardNumber { get; set; }
        public string FSSAI { get; set; }
        public string FSSAIValidUpTo { get; set; }
        public string BankName { get; set; }
        public string Branch { get; set; }
        public string AccountNumber { get; set; }
        public string IFSCCode { get; set; }
        public string TDSCategory { get; set; }
        public decimal TDSPercentage { get; set; }
        public string AddressOneLine1 { get; set; }
        public string AddressOneLine2 { get; set; }
        public string AreaOne { get; set; }
        public string StateOne { get; set; }
        public string AddressOnePincode { get; set; }
        public string AddressTwoLine1 { get; set; }
        public string AddressTwoLine2 { get; set; }
        public string AreaTwo { get; set; }
        public string StateTwo { get; set; }
        public string AddressTwoPincode { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhoneNo { get; set; }
        public string ContactMobileNo { get; set; }
    }


    // Expense supplier 18-12-2019
    public class AddExpenseSupplier
    {
        public long SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string GSTNo { get; set; }
        public long TaxID { get; set; }
        public string PanCardNumber { get; set; }
        public string BankName { get; set; }
        public string Branch { get; set; }
        public string AccountNumber { get; set; }
        public string IFSCCode { get; set; }
        public long TDSCategoryID { get; set; }
        public decimal TDSPercentage { get; set; }
        public string AddressOneLine1 { get; set; }
        public string AddressOneLine2 { get; set; }
        public long AreaIDOne { get; set; }
        public string StateOne { get; set; }
        public string AddressOnePincode { get; set; }
        public string AddressTwoLine1 { get; set; }
        public string AddressTwoLine2 { get; set; }
        public long AreaIDTwo { get; set; }
        public string StateTwo { get; set; }
        public string AddressTwoPincode { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
        public List<SupplierContactDetail> lstContactDetail { get; set; }
        public string NameAsBankAccount { get; set; }
    }

    public class ExpenseSupplierListResponse
    {
        public long SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string GSTNo { get; set; }
        public long TaxID { get; set; }
        public string TaxName { get; set; }
        public string PanCardNumber { get; set; }
        public string BankName { get; set; }
        public string Branch { get; set; }
        public string AccountNumber { get; set; }
        public string IFSCCode { get; set; }
        public long TDSCategoryID { get; set; }
        public string TDSCategory { get; set; }
        public decimal TDSPercentage { get; set; }
        public string AddressOneLine1 { get; set; }
        public string AddressOneLine2 { get; set; }
        public long AreaIDOne { get; set; }
        public string AreaNameOne { get; set; }
        public string StateOne { get; set; }
        public string AddressOnePincode { get; set; }
        public string AddressTwoLine1 { get; set; }
        public string AddressTwoLine2 { get; set; }
        public long AreaIDTwo { get; set; }
        public string AreaNameTwo { get; set; }
        public string StateTwo { get; set; }
        public string AddressTwoPincode { get; set; }
        public string ContactName { get; set; }
        public Boolean IsDelete { get; set; }
        public string AreaOne { get; set; }
        public string AreaTwo { get; set; }
        public long DaysRemaining { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhoneNo { get; set; }
        public string ContactMobileNo { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string NameAsBankAccount { get; set; }
    }

    public class ExpenseSupplierContactDetail
    {
        public long SupplierContactID { get; set; }
        public long AddressID { get; set; }
        public long SupplierID { get; set; }
        public string Name { get; set; }
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
    }

    public class ExpenseSupplierListExport
    {
        public string SupplierName { get; set; }
        public string GSTNo { get; set; }
        public string TaxName { get; set; }
        public string PanCardNumber { get; set; }
        public string BankName { get; set; }
        public string Branch { get; set; }
        public string AccountNumber { get; set; }
        public string IFSCCode { get; set; }
        public string TDSCategory { get; set; }
        public decimal TDSPercentage { get; set; }
        public string AddressOneLine1 { get; set; }
        public string AddressOneLine2 { get; set; }
        public string AreaOne { get; set; }
        public string StateOne { get; set; }
        public string AddressOnePincode { get; set; }
        public string AddressTwoLine1 { get; set; }
        public string AddressTwoLine2 { get; set; }
        public string AreaTwo { get; set; }
        public string StateTwo { get; set; }
        public string AddressTwoPincode { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhoneNo { get; set; }
        public string ContactMobileNo { get; set; }
    }




    public class AddCommonSupplier
    {
        public long SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string GSTNo { get; set; }
        public long TaxID { get; set; }
        public string PanCardNumber { get; set; }
        public string FSSAI { get; set; }
        public DateTime? FSSAIValidUpTo { get; set; }
        public string BankName { get; set; }
        public string Branch { get; set; }
        public string AccountNumber { get; set; }
        public string IFSCCode { get; set; }
        public long TDSCategoryID { get; set; }
        public decimal TDSPercentage { get; set; }
        public string AddressOneLine1 { get; set; }
        public string AddressOneLine2 { get; set; }
        public long AreaIDOne { get; set; }
        public string StateOne { get; set; }
        public string AddressOnePincode { get; set; }
        public string AddressTwoLine1 { get; set; }
        public string AddressTwoLine2 { get; set; }
        public long AreaIDTwo { get; set; }
        public string StateTwo { get; set; }
        public string AddressTwoPincode { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
        public List<CommonSupplierContactDetail> lstContactDetail { get; set; }
    }

    public class CommonSupplierContactDetail
    {
        public long SupplierContactID { get; set; }
        public long AddressID { get; set; }
        public long SupplierID { get; set; }
        public string Name { get; set; }
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
    }

    public class AllSupplierName
    {
        public string SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string Identification { get; set; }
    }
}
