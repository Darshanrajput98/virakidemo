
namespace vb.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System;

    public class RetExportDayWiseSales
    {
        public List<RetDayWiseSalesList> lstDayWiseSales { get; set; }
        public List<RetDayWiseTaxList> lstDayWiseTax { get; set; }
        public List<RetDayWiseCreditMemoList> lstDayWiseCreditMemo { get; set; }
        public List<RetDayWiseSalesManList> lstDayWiseSalesMan { get; set; }
    }

    public class RetDayWiseSalesListForExp
    {
        public string InvoiceNumber { get; set; }
        public string InvoiceDate { get; set; }
        public long CustomerCode { get; set; }
        public string Party { get; set; }
        public string Area { get; set; }
        public decimal GrossAmt { get; set; }
        public decimal CGST { get; set; }
        public string TaxAmtCGST { get; set; }
        public decimal SGST { get; set; }
        public string TaxAmtSGST { get; set; }
        public decimal IGST { get; set; }
        public string TaxAmtIGST { get; set; }
        public decimal TaxCollectedAtSourcePayable { get; set; }
        public decimal RoundOff { get; set; }
        public decimal NetAmount { get; set; }
        public string SalesPerson { get; set; }
    }

    public class RetDayWiseCreditMemoListForExp
    {
        public string CreditMemoNumber { get; set; }
        public string CreditMemoDate { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceDate { get; set; }
        public string Party { get; set; }
        public string Area { get; set; }
        public decimal GrossAmt { get; set; }
        public decimal CGST { get; set; }
        public string TaxAmtCGST { get; set; }
        public decimal SGST { get; set; }
        public string TaxAmtSGST { get; set; }
        public decimal IGST { get; set; }
        public string TaxAmtIGST { get; set; }
        public decimal RoundOff { get; set; }
        public decimal NetAmount { get; set; }
        public string SalesPerson { get; set; }
        public string ReferenceNumber { get; set; }
    }

    public class RetDayWiseSalesManListForExp
    {
        public string SalesPerson { get; set; }
        public decimal GrossAmt { get; set; }
        public decimal TaxAmt { get; set; }
        public decimal RoundOff { get; set; }
        public decimal NetAmt { get; set; }
    }

    public class RetDayWiseTaxListForExp
    {
        public decimal GrossAmt { get; set; }
        public decimal CGST { get; set; }
        public string TaxAmtCGST { get; set; }
        public decimal SGST { get; set; }
        public string TaxAmtSGST { get; set; }
        public decimal IGST { get; set; }
        public string TaxAmtIGST { get; set; }
        public decimal TaxAmt { get; set; }
        public decimal TaxCollectedAtSourcePayable { get; set; }
        public decimal RoundOff { get; set; }
        public decimal NetAmt { get; set; }
    }

    public class RetDayWiseSalesList
    {
        public long UserID { get; set; }
        public string State { get; set; }
        public long OrderID { get; set; }
        public long CustomerNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvCode { get; set; }
        public string TaxNo { get; set; }
        public DateTime? InvDate { get; set; }
        public string InvoiceDate { get; set; }
        public string Party { get; set; }
        public string Area { get; set; }
        public decimal GrossAmt { get; set; }
        public decimal GrandGrossAmt { get; set; }
        public decimal GrandCGSTAmt { get; set; }
        public decimal GrandSGSTAmt { get; set; }
        public decimal GrandIGSTAmt { get; set; }
        public decimal GrandTCSAmt { get; set; }
        public decimal CGST { get; set; }
        public string TaxAmtCGST { get; set; }
        public decimal SGST { get; set; }
        public string TaxAmtSGST { get; set; }
        public string UserFullName { get; set; }
        public decimal IGST { get; set; }
        public string TaxAmtIGST { get; set; }
        public decimal RoundOff { get; set; }
        public decimal GrandRoundOff { get; set; }
        public decimal TCSTaxAmount { get; set; }
        public decimal NetAmount { get; set; }
        public decimal GrandNetAmount { get; set; }
        public decimal TotalTax { get; set; }
        public Boolean IsDelete { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsApprove { get; set; }
    }

    public class RetDayWiseCreditMemoList
    {
        public long OrderID { get; set; }
        public string Invoice { get; set; }
        public string InvoiceNumber { get; set; }
        public string Customer { get; set; }
        public string Area { get; set; }
        public string CreditMemo { get; set; }
        public decimal Amount { get; set; }
        public decimal GrandAmount { get; set; }
        public DateTime? InvDate { get; set; }
        public decimal CGST { get; set; }
        public string TaxAmtCGST { get; set; }
        public decimal GrandCGSTAmt { get; set; }
        public decimal SGST { get; set; }
        public string TaxAmtSGST { get; set; }
        public decimal GrandSGSTAmt { get; set; }
        public decimal IGST { get; set; }
        public string TaxAmtIGST { get; set; }
        public decimal GrandIGSTAmt { get; set; }
        public decimal RoundOff { get; set; }
        public decimal GrandRoundOff { get; set; }
        public decimal NetAmount { get; set; }
        public decimal GrandNetAmount { get; set; }
        public string UserFullName { get; set; }
        public decimal TotalTax { get; set; }
        public string InvoiceDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ReferenceNumber { get; set; }
    }

    public class RetDayWiseTaxList
    {
        public string Tax { get; set; }
        public decimal TaxPer { get; set; }
        public decimal GrandTaxPer { get; set; }
        public decimal GrossAmtTotal { get; set; }
        public decimal GrandGrossAmtTotal { get; set; }
        public decimal TaxAmtTotal { get; set; }
        public decimal GrandTaxAmtTotal { get; set; }
        public decimal TCSAmtTotal { get; set; }
        public decimal GrandTCSAmtTotal { get; set; }
        public decimal RoundOffTotal { get; set; }
        public decimal GrandRoundOffTotal { get; set; }
        public decimal NetAmtTotal { get; set; }
        public decimal GrandNetAmtTotal { get; set; }
        public DateTime? InvDate { get; set; }
        public decimal CGST { get; set; }
        public string TaxAmtCGST { get; set; }
        public decimal SGST { get; set; }
        public string TaxAmtSGST { get; set; }
        public decimal IGST { get; set; }
        public string TaxAmtIGST { get; set; }
        public decimal GrandCGSTAmt { get; set; }
        public decimal GrandSGSTAmt { get; set; }
        public decimal GrandIGSTAmt { get; set; }
    }

    public class RetDayWiseSalesManList
    {
        public long UserID { get; set; }
        public string SalesPerson { get; set; }
        public decimal GrossAmtTotal { get; set; }
        public decimal TaxAmtTotal { get; set; }
        public decimal RoundOffTotal { get; set; }
        public decimal NetAmtTotal { get; set; }
        public decimal GrandGrossAmtTotal { get; set; }
        public decimal GrandTaxAmtTotal { get; set; }
        public decimal GrandRoundOffTotal { get; set; }
        public decimal GrandNetAmtTotal { get; set; }
        public DateTime? InvDate { get; set; }
    }


    public class DayWiseSalesExportListForExp
    {
        public string InvCode { get; set; }
        public DateTime? InvDate { get; set; }
        public string Party { get; set; }
        public string Country { get; set; }
        public long OrderID { get; set; }
        public int TotalPkgs { get; set; }
        public string FrieghtText { get; set; }
        public decimal FreightAMount { get; set; }
        //public decimal InTotalAmount { get; set; }
        public decimal Rupees { get; set; }
        public bool IsDelete { get; set; }
        public long CustomerCode { get; set; }
        public string UserFullName { get; set; }

        public string InvoiceDate { get; set; }
        public string InsuranceText { get; set; }
        public decimal InsuranceAmount { get; set; }
        public decimal InvoiceTotalAmt { get; set; }
        public decimal GrossAmt { get; set; }
        public decimal CGST { get; set; }
        public string TaxAmtCGST { get; set; }
        public decimal SGST { get; set; }
        public string TaxAmtSGST { get; set; }
        public decimal IGST { get; set; }
        public string TaxAmtIGST { get; set; }
        public decimal RoundOff { get; set; }
        public decimal GrandRoundOff { get; set; }
        public decimal NetAmount { get; set; }
    }
    public class ExportDayWiseSalesExportList
    {
        public string InvCode { get; set; }
        public string InvoiceDate { get; set; }
        public string Party { get; set; }
        public string Country { get; set; }
        public int TotalPkgs { get; set; }
        public decimal GrossAmt { get; set; }
        public decimal CGST { get; set; }
        public string TaxAmtCGST { get; set; }
        public decimal SGST { get; set; }
        public string TaxAmtSGST { get; set; }
        public decimal IGST { get; set; }
        public string TaxAmtIGST { get; set; }
        public decimal RoundOff { get; set; }
        public string InsuranceText { get; set; }
        public decimal InsuranceAmount { get; set; }
        public string FrieghtText { get; set; }
        public decimal FreightAMount { get; set; }
        public decimal InvoiceTotalAmt { get; set; }
        public decimal NetAmount { get; set; }
        public string UserFullName { get; set; }
    }


}
