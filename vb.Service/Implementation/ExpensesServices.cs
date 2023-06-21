
namespace vb.Service
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using vb.Data;
    using vb.Data.Model;
    using vb.Data.ViewModel;

    public class ExpensesServices : IExpensesService
    {
        public List<DebitAccountTypeNameList> GetAllDebitAccountTypeName()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllDebitAccountTypeName";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<DebitAccountTypeNameList> lst = new List<DebitAccountTypeNameList>();
                while (dr.Read())
                {
                    DebitAccountTypeNameList obj = new DebitAccountTypeNameList();
                    obj.DebitAccountTypeID = objBaseSqlManager.GetInt64(dr, "DebitAccountTypeID");
                    obj.DebitAccountType = objBaseSqlManager.GetTextValue(dr, "DebitAccountType");
                    lst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lst;
            }
        }

        public long AddExpensesVoucher(ExpensesVoucher_Mst Obj)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (Obj.ExpensesVoucherID == 0)
                {
                    context.ExpensesVoucher_Mst.Add(Obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(Obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (Obj.ExpensesVoucherID > 0)
                {
                    return Obj.ExpensesVoucherID;
                    //return true;
                }
                else
                {
                    return 0;
                }
            }
        }

        public List<ExpensesVoucherListResponse> GetAllExpensesVoucherList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllExpensesVoucherList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ExpensesVoucherListResponse> objlst = new List<ExpensesVoucherListResponse>();
                while (dr.Read())
                {
                    ExpensesVoucherListResponse objExpensesVoucherList = new ExpensesVoucherListResponse();
                    objExpensesVoucherList.ExpensesVoucherID = objBaseSqlManager.GetInt64(dr, "ExpensesVoucherID");
                    objExpensesVoucherList.DateofVoucher = objBaseSqlManager.GetDateTime(dr, "DateofVoucher");
                    if (objExpensesVoucherList.DateofVoucher != Convert.ToDateTime("10/10/2014"))
                    {
                        objExpensesVoucherList.DateofVoucherstr = objExpensesVoucherList.DateofVoucher.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        objExpensesVoucherList.DateofVoucherstr = "";
                    }
                    objExpensesVoucherList.VoucherNumber = objBaseSqlManager.GetTextValue(dr, "VoucherNumber");
                    objExpensesVoucherList.Pay = objBaseSqlManager.GetTextValue(dr, "Pay");
                    objExpensesVoucherList.RemarksL1 = objBaseSqlManager.GetTextValue(dr, "RemarksL1");
                    objExpensesVoucherList.RemarksL2 = objBaseSqlManager.GetTextValue(dr, "RemarksL2");
                    objExpensesVoucherList.RemarksL3 = objBaseSqlManager.GetTextValue(dr, "RemarksL3");

                    if (objBaseSqlManager.GetInt64(dr, "CustomerID") == 0)
                    {
                        objExpensesVoucherList.DebitAccountTypeID = objBaseSqlManager.GetInt64(dr, "DebitAccountTypeID");
                        objExpensesVoucherList.DebitAccountType = objBaseSqlManager.GetTextValue(dr, "DebitAccountType");
                    }
                    else
                    {
                        if (objBaseSqlManager.GetTextValue(dr, "Identification") == "Wholesale")
                        {
                            objExpensesVoucherList.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                            objExpensesVoucherList.DebitAccountType = GetWholesaleEmployeeCustomerNameForExpensesVoucher(objExpensesVoucherList.CustomerID);
                            objExpensesVoucherList.Identification = objBaseSqlManager.GetTextValue(dr, "Identification");
                        }
                        else
                        {
                            objExpensesVoucherList.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                            objExpensesVoucherList.DebitAccountType = GetRetailEmployeeCustomerNameForExpensesVoucher(objExpensesVoucherList.CustomerID);
                            objExpensesVoucherList.Identification = objBaseSqlManager.GetTextValue(dr, "Identification");
                        }
                    }

                    objExpensesVoucherList.Amount = objBaseSqlManager.GetDecimal(dr, "Amount");
                    objExpensesVoucherList.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    objExpensesVoucherList.PreparedBy = objBaseSqlManager.GetTextValue(dr, "PreparedBy");
                    objExpensesVoucherList.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    objExpensesVoucherList.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objExpensesVoucherList.GodownID = objBaseSqlManager.GetInt64(dr, "GodownID");
                    objExpensesVoucherList.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    objExpensesVoucherList.BillNumber = objBaseSqlManager.GetTextValue(dr, "BillNumber");
                    objlst.Add(objExpensesVoucherList);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        // 09 Sep 2020 Piyush Limbani
        public string GetWholesaleEmployeeCustomerNameForExpensesVoucher(long CustomerID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetWholesaleEmployeeCustomerNameForExpensesVoucher";
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                string CustomerName = "";
                while (dr.Read())
                {
                    CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return CustomerName;
            }
        }

        public string GetRetailEmployeeCustomerNameForExpensesVoucher(long CustomerID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetailEmployeeCustomerNameForExpensesVoucher";
                cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                string CustomerName = "";
                while (dr.Read())
                {
                    CustomerName = objBaseSqlManager.GetTextValue(dr, "CustomerName");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return CustomerName;
            }
        }
        // 09 Sep 2020 Piyush Limbani


        public bool DeleteExpensesVoucher(long ExpensesVoucherID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteExpensesVoucher";
                cmdGet.Parameters.AddWithValue("@ExpensesVoucherID", ExpensesVoucherID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        public ExpensesVoucherListResponse GetDataForExpensesVoucherPrint(long ExpensesVoucherID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetDataForExpensesVoucherPrint";
                cmdGet.Parameters.AddWithValue("@ExpensesVoucherID", ExpensesVoucherID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                ExpensesVoucherListResponse objExpenses = new ExpensesVoucherListResponse();
                while (dr.Read())
                {
                    objExpenses.ExpensesVoucherID = objBaseSqlManager.GetInt64(dr, "ExpensesVoucherID");
                    objExpenses.DateofVoucher = objBaseSqlManager.GetDateTime(dr, "DateofVoucher");

                    if (objExpenses.DateofVoucher != Convert.ToDateTime("10/10/2014"))
                    {
                        objExpenses.DateofVoucherstr = objExpenses.DateofVoucher.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        objExpenses.DateofVoucherstr = "";
                    }
                    objExpenses.VoucherNumber = objBaseSqlManager.GetTextValue(dr, "VoucherNumber");
                    objExpenses.Pay = objBaseSqlManager.GetTextValue(dr, "Pay");
                    objExpenses.RemarksL1 = objBaseSqlManager.GetTextValue(dr, "RemarksL1");
                    objExpenses.RemarksL2 = objBaseSqlManager.GetTextValue(dr, "RemarksL2");
                    objExpenses.RemarksL3 = objBaseSqlManager.GetTextValue(dr, "RemarksL3");


                    //objExpenses.DebitAccountTypeID = objBaseSqlManager.GetInt64(dr, "DebitAccountTypeID");
                    //objExpenses.DebitAccountType = objBaseSqlManager.GetTextValue(dr, "DebitAccountType");


                    if (objBaseSqlManager.GetInt64(dr, "CustomerID") == 0)
                    {
                        objExpenses.DebitAccountTypeID = objBaseSqlManager.GetInt64(dr, "DebitAccountTypeID");
                        objExpenses.DebitAccountType = objBaseSqlManager.GetTextValue(dr, "DebitAccountType");
                    }
                    else
                    {
                        if (objBaseSqlManager.GetTextValue(dr, "Identification") == "Wholesale")
                        {
                            objExpenses.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                            objExpenses.DebitAccountType = GetWholesaleEmployeeCustomerNameForExpensesVoucher(objExpenses.CustomerID);
                            //objExpenses.Identification = objBaseSqlManager.GetTextValue(dr, "Identification");
                        }
                        else
                        {
                            objExpenses.CustomerID = objBaseSqlManager.GetInt64(dr, "CustomerID");
                            objExpenses.DebitAccountType = GetRetailEmployeeCustomerNameForExpensesVoucher(objExpenses.CustomerID);
                            //objExpenses.Identification = objBaseSqlManager.GetTextValue(dr, "Identification");
                        }
                    }


                    objExpenses.Amount = objBaseSqlManager.GetDecimal(dr, "Amount");
                    objExpenses.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    objExpenses.PreparedBy = objBaseSqlManager.GetTextValue(dr, "PreparedBy");
                    objExpenses.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    objExpenses.AmountInWords = objBaseSqlManager.GetTextValue(dr, "AmountInWords");
                    objExpenses.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    objExpenses.BillNumber = objBaseSqlManager.GetTextValue(dr, "BillNumber");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objExpenses;
            }
        }

        public long AddDebitAccountType(DebitAccountType_Mst Obj)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (Obj.DebitAccountTypeID == 0)
                {
                    context.DebitAccountType_Mst.Add(Obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(Obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (Obj.DebitAccountTypeID > 0)
                {
                    return Obj.DebitAccountTypeID;
                    //return true;
                }
                else
                {
                    return 0;
                }
            }
        }

        public List<DebitAccountTypeListResponse> GetAllDebitAccountTypeList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllDebitAccountTypeList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<DebitAccountTypeListResponse> objlst = new List<DebitAccountTypeListResponse>();
                while (dr.Read())
                {
                    DebitAccountTypeListResponse obj = new DebitAccountTypeListResponse();
                    obj.DebitAccountTypeID = objBaseSqlManager.GetInt64(dr, "DebitAccountTypeID");
                    obj.DebitAccountType = objBaseSqlManager.GetTextValue(dr, "DebitAccountType");
                    obj.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    obj.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool DeleteDebitAccountType(long DebitAccountTypeID, bool IsDelete)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "DeleteDebitAccountType";
                cmdGet.Parameters.AddWithValue("@DebitAccountTypeID", DebitAccountTypeID);
                cmdGet.Parameters.AddWithValue("@IsDelete", IsDelete);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
            }
            return true;
        }

        public List<ExpensesVoucherListResponse> GetExpensesVoucherListByDate(DateTime? FromDate, DateTime? ToDate, long GodownID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetExpensesVoucherListByDate";
                cmdGet.Parameters.AddWithValue("@FromDate", FromDate);
                cmdGet.Parameters.AddWithValue("@ToDate", ToDate);
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ExpensesVoucherListResponse> objlst = new List<ExpensesVoucherListResponse>();
                decimal TotalAmount = 0;
                while (dr.Read())
                {
                    ExpensesVoucherListResponse obj = new ExpensesVoucherListResponse();
                    obj.ExpensesVoucherID = objBaseSqlManager.GetInt64(dr, "ExpensesVoucherID");
                    obj.DateofVoucher = objBaseSqlManager.GetDateTime(dr, "DateofVoucher");
                    if (obj.DateofVoucher != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.DateofVoucherstr = obj.DateofVoucher.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        obj.DateofVoucherstr = "";
                    }
                    obj.VoucherNumber = objBaseSqlManager.GetTextValue(dr, "VoucherNumber");
                    obj.Pay = objBaseSqlManager.GetTextValue(dr, "Pay");
                    obj.DebitAccountType = objBaseSqlManager.GetTextValue(dr, "DebitAccountType");
                    obj.Amount = objBaseSqlManager.GetDecimal(dr, "Amount");
                    obj.PreparedBy = objBaseSqlManager.GetTextValue(dr, "PreparedBy");
                    obj.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    obj.GodownNamestr = "Cash" + " - " + obj.GodownName;
                    obj.RemarksL1 = objBaseSqlManager.GetTextValue(dr, "RemarksL1");
                    obj.RemarksL2 = objBaseSqlManager.GetTextValue(dr, "RemarksL2");
                    obj.RemarksL3 = objBaseSqlManager.GetTextValue(dr, "RemarksL3");
                    obj.Remarks = obj.RemarksL1 + " , " + obj.RemarksL2 + " , " + obj.RemarksL3;
                    TotalAmount += obj.Amount;
                    obj.TotalAmount = TotalAmount;
                    obj.BillNumber = objBaseSqlManager.GetTextValue(dr, "BillNumber");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<ExpensesVoucherTotal> GetExpensesVoucherTotal(DateTime? FromDate, DateTime? ToDate)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetExpensesVoucherTotal";
                cmdGet.Parameters.AddWithValue("@FromDate", FromDate);
                cmdGet.Parameters.AddWithValue("@ToDate", ToDate);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ExpensesVoucherTotal> objlst = new List<ExpensesVoucherTotal>();
                while (dr.Read())
                {
                    ExpensesVoucherTotal obj = new ExpensesVoucherTotal();
                    obj.TotalAmount = objBaseSqlManager.GetDecimal(dr, "TotalAmount"); ;
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public GetWholesaleAmount GetOpeningAmountByGodownID(long GodownID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetOpeningAmountByGodownID";
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                GetWholesaleAmount obj = new GetWholesaleAmount();
                while (dr.Read())
                {
                    obj.OpeningAmount = objBaseSqlManager.GetDecimal(dr, "OpeningAmount");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        public GetWholesaleAmount GetOpeningChillarByGodownID(long GodownID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetOpeningChillarByGodownID";
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                GetWholesaleAmount obj = new GetWholesaleAmount();
                while (dr.Read())
                {
                    obj.OpeningChillar = objBaseSqlManager.GetDecimal(dr, "OpeningChillar");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        public GetWholesaleAmount GetWholesaleAmountByCustID(DateTime WholesaleAssignedDate, long GodownID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetWholesaleAmountByCustID";
                cmdGet.Parameters.AddWithValue("@WholesaleAssignedDate", WholesaleAssignedDate);
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                GetWholesaleAmount obj = new GetWholesaleAmount();
                while (dr.Read())
                {
                    obj.CashTotal = objBaseSqlManager.GetDecimal(dr, "TotalCash");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        public List<SalesManWiseCash> GetSalesManWiseCashList(DateTime WholesaleAssignedDate, long GodownID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetSalesManWiseCashList";
                cmdGet.Parameters.AddWithValue("@WholesaleAssignedDate", WholesaleAssignedDate);
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<SalesManWiseCash> objlst = new List<SalesManWiseCash>();
                while (dr.Read())
                {
                    SalesManWiseCash obj = new SalesManWiseCash();
                    obj.SalesPerson = objBaseSqlManager.GetTextValue(dr, "SalesPerson");
                    obj.Cash = objBaseSqlManager.GetDecimal(dr, "Cash");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public GetWholesaleAmount GetRetailAmountTotalByAssignedDate(DateTime RetailAssignedDate, long GodownID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetailAmountTotalByAssignedDate";
                cmdGet.Parameters.AddWithValue("@RetailAssignedDate", RetailAssignedDate);
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                GetWholesaleAmount obj = new GetWholesaleAmount();
                while (dr.Read())
                {
                    obj.CashTotalRetail = objBaseSqlManager.GetDecimal(dr, "TotalCash");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        public List<SalesManWiseCash> GetRetSalesManWiseCashList(DateTime RetailAssignedDate, long GodownID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetSalesManWiseCashList";
                cmdGet.Parameters.AddWithValue("@RetailAssignedDate", RetailAssignedDate);
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<SalesManWiseCash> objlst = new List<SalesManWiseCash>();
                while (dr.Read())
                {
                    SalesManWiseCash obj = new SalesManWiseCash();
                    obj.SalesPerson = objBaseSqlManager.GetTextValue(dr, "SalesPerson");
                    obj.Cash = objBaseSqlManager.GetDecimal(dr, "Cash");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        // 18 June 2020
        public VehicleNoListInwardResponse GetAllVehicleNoForInwardByAssignedDate(DateTime AssignedDate)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllVehicleNoForInwardByAssignedDate";
                cmdGet.Parameters.AddWithValue("@AssignedDate", AssignedDate);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                VehicleNoListInwardResponse objOrder = new VehicleNoListInwardResponse();
                while (dr.Read())
                {
                    objOrder.VehicleNo = objBaseSqlManager.GetTextValue(dr, "VehicleNo");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objOrder;
            }
        }

        public RetailVehicleNoListInwardResponse GetAllRetailVehicleNoForInwardByAssignedDate(DateTime AssignedDate)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllRetailVehicleNoForInwardByAssignedDate";
                cmdGet.Parameters.AddWithValue("@AssignedDate", AssignedDate);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                RetailVehicleNoListInwardResponse objOrder = new RetailVehicleNoListInwardResponse();
                while (dr.Read())
                {
                    objOrder.VehicleNo = objBaseSqlManager.GetTextValue(dr, "VehicleNo");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objOrder;
            }
        }

        public decimal GetTempoCashAmountTotal(DateTime TempoDateWholesale, string VehicleNo, long GodownID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetTempoCashAmountTotal";
                cmdGet.Parameters.AddWithValue("@TempoDateWholesale", TempoDateWholesale);
                cmdGet.Parameters.AddWithValue("@VehicleNo", VehicleNo);
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                decimal TempoTotalAmount = 0;
                while (dr.Read())
                {
                    TempoTotalAmount = objBaseSqlManager.GetDecimal(dr, "TempoTotalAmount");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return TempoTotalAmount;
            }
        }

        public decimal GetTempoCashAmountTotalRetail(DateTime TempoDateRetail, string VehicleNo, long GodownID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetTempoCashAmountTotalRetail";
                cmdGet.Parameters.AddWithValue("@TempoDateRetail", TempoDateRetail);
                cmdGet.Parameters.AddWithValue("@VehicleNo", VehicleNo);
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                decimal TempoTotalAmount = 0;
                while (dr.Read())
                {
                    TempoTotalAmount = objBaseSqlManager.GetDecimal(dr, "TempoTotalAmount");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return TempoTotalAmount;
            }
        }

        public GetWholesaleAmount GetTotalExpenseByGodownwise(DateTime ExpensesDate, long GodownID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetTotalExpenseByGodownwise";
                cmdGet.Parameters.AddWithValue("@ExpensesDate", ExpensesDate);
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                GetWholesaleAmount obj = new GetWholesaleAmount();
                while (dr.Read())
                {
                    obj.TotalExpenses = objBaseSqlManager.GetDecimal(dr, "TotalExpenses");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        public long AddInwardOutWard(AddInwardOutWard data)
        {
            InwardOutward_Mst obj = new InwardOutward_Mst();
            obj.InwardID = data.InwardID;
            obj.GodownID = data.GodownID;
            obj.OpeningAmount = data.OpeningAmount;
            obj.ChillarInward = data.ChillarInward;
            obj.WholesaleAssignedDate = data.WholesaleAssignedDate;
            obj.WholesaleCash = data.WholesaleCash;
            obj.RetailAssignedDate = data.RetailAssignedDate;
            obj.RetailCash = data.RetailCash;
            obj.WholesaleTempoDate1 = data.WholesaleTempoDate1;
            obj.WholesaleVehicleNo1 = data.WholesaleVehicleNo1;
            obj.WholesaleTempoAmount1 = data.WholesaleTempoAmount1;
            obj.WholesaleTempoDate2 = data.WholesaleTempoDate2;
            obj.WholesaleVehicleNo2 = data.WholesaleVehicleNo2;
            obj.WholesaleTempoAmount2 = data.WholesaleTempoAmount2;
            obj.WholesaleTempoDate3 = data.WholesaleTempoDate3;
            obj.WholesaleVehicleNo3 = data.WholesaleVehicleNo3;
            obj.WholesaleTempoAmount3 = data.WholesaleTempoAmount3;
            obj.RetailTempoDate1 = data.RetailTempoDate1;
            obj.RetailVehicleNo1 = data.RetailVehicleNo1;
            obj.RetailTempoAmount1 = data.RetailTempoAmount1;
            obj.RetailTempoDate2 = data.RetailTempoDate2;
            obj.RetailVehicleNo2 = data.RetailVehicleNo2;
            obj.RetailTempoAmount2 = data.RetailTempoAmount2;
            obj.RetailTempoDate3 = data.RetailTempoDate3;
            obj.RetailVehicleNo3 = data.RetailVehicleNo3;
            obj.RetailTempoAmount3 = data.RetailTempoAmount3;
            obj.TotalInward = data.TotalInward;
            obj.ChillarOutward = data.ChillarOutward;
            obj.ExpensesDate = data.ExpensesDate;
            obj.TotalExpenses = data.TotalExpenses;
            obj.TransferVB2 = data.TransferVB2;
            obj.TransferVB3 = data.TransferVB3;
            obj.BankID1 = data.BankID1;
            obj.BankDepositeAmount1 = data.BankDepositeAmount1;
            obj.BankID2 = data.BankID2;
            obj.BankDepositeAmount2 = data.BankDepositeAmount2;
            obj.BankID3 = data.BankID3;
            obj.BankDepositeAmount3 = data.BankDepositeAmount3;
            obj.TotalOutward = data.TotalOutward;
            obj.GrandTotal = data.GrandTotal;
            obj.OpeningChillar = data.OpeningChillar;
            obj.ClosingChillar = data.ClosingChillar;
            obj.CreatedBy = data.CreatedBy;
            obj.CreatedOn = data.CreatedOn;
            obj.UpdatedBy = data.UpdatedBy;
            obj.UpdatedOn = data.UpdatedOn;
            obj.IsDelete = data.IsDelete;
            obj.IsDactive = data.IsDactive;
            // Wholesale
            if (obj.WholesaleTempoDate1 != null && obj.WholesaleVehicleNo1 != null)
            {
                string[] WholesaleVehicleNo1 = obj.WholesaleVehicleNo1.Split(',');
                foreach (var item in WholesaleVehicleNo1)
                {
                    SqlCommand cmdGet = new SqlCommand();
                    using (var objBaseSqlManager = new BaseSqlManager())
                    {
                        cmdGet.CommandType = CommandType.StoredProcedure;
                        cmdGet.CommandText = "UpdateWholesaleIsInwardTempoStatus";
                        cmdGet.Parameters.AddWithValue("@AssignedDate", obj.WholesaleTempoDate1);
                        cmdGet.Parameters.AddWithValue("@VehicleNo", item);
                        cmdGet.Parameters.AddWithValue("@IsInwardTempo", false);
                        objBaseSqlManager.ExecuteNonQuery(cmdGet);
                        objBaseSqlManager.ForceCloseConnection();
                    }
                }

            }
            if (obj.WholesaleTempoDate2 != null && obj.WholesaleVehicleNo2 != null)
            {
                string[] WholesaleVehicleNo2 = obj.WholesaleVehicleNo2.Split(',');
                foreach (var item in WholesaleVehicleNo2)
                {
                    SqlCommand cmdGet = new SqlCommand();
                    using (var objBaseSqlManager = new BaseSqlManager())
                    {
                        cmdGet.CommandType = CommandType.StoredProcedure;
                        cmdGet.CommandText = "UpdateWholesaleIsInwardTempoStatus";
                        cmdGet.Parameters.AddWithValue("@AssignedDate", obj.WholesaleTempoDate2);
                        cmdGet.Parameters.AddWithValue("@VehicleNo", item);
                        cmdGet.Parameters.AddWithValue("@IsInwardTempo", false);
                        objBaseSqlManager.ExecuteNonQuery(cmdGet);
                        objBaseSqlManager.ForceCloseConnection();
                    }
                }
            }
            if (obj.WholesaleTempoDate3 != null && obj.WholesaleVehicleNo3 != null)
            {
                string[] WholesaleVehicleNo3 = obj.WholesaleVehicleNo3.Split(',');
                foreach (var item in WholesaleVehicleNo3)
                {
                    SqlCommand cmdGet = new SqlCommand();
                    using (var objBaseSqlManager = new BaseSqlManager())
                    {
                        cmdGet.CommandType = CommandType.StoredProcedure;
                        cmdGet.CommandText = "UpdateWholesaleIsInwardTempoStatus";
                        cmdGet.Parameters.AddWithValue("@AssignedDate", obj.WholesaleTempoDate3);
                        cmdGet.Parameters.AddWithValue("@VehicleNo", item);
                        cmdGet.Parameters.AddWithValue("@IsInwardTempo", false);
                        objBaseSqlManager.ExecuteNonQuery(cmdGet);
                        objBaseSqlManager.ForceCloseConnection();
                    }
                }
            }
            // Retail
            if (obj.RetailTempoDate1 != null && obj.RetailVehicleNo1 != null)
            {
                string[] RetailVehicleNo1 = obj.RetailVehicleNo1.Split(',');
                foreach (var item in RetailVehicleNo1)
                {
                    SqlCommand cmdGet = new SqlCommand();
                    using (var objBaseSqlManager = new BaseSqlManager())
                    {
                        cmdGet.CommandType = CommandType.StoredProcedure;
                        cmdGet.CommandText = "UpdateRetailIsInwardTempoStatus";
                        cmdGet.Parameters.AddWithValue("@AssignedDate", obj.RetailTempoDate1);
                        cmdGet.Parameters.AddWithValue("@VehicleNo", item);
                        cmdGet.Parameters.AddWithValue("@IsInwardTempo", false);
                        objBaseSqlManager.ExecuteNonQuery(cmdGet);
                        objBaseSqlManager.ForceCloseConnection();
                    }
                }

            }
            if (obj.RetailTempoDate2 != null && obj.RetailVehicleNo2 != null)
            {
                string[] RetailVehicleNo2 = obj.RetailVehicleNo2.Split(',');
                foreach (var item in RetailVehicleNo2)
                {
                    SqlCommand cmdGet = new SqlCommand();
                    using (var objBaseSqlManager = new BaseSqlManager())
                    {
                        cmdGet.CommandType = CommandType.StoredProcedure;
                        cmdGet.CommandText = "UpdateRetailIsInwardTempoStatus";
                        cmdGet.Parameters.AddWithValue("@AssignedDate", obj.RetailTempoDate2);
                        cmdGet.Parameters.AddWithValue("@VehicleNo", item);
                        cmdGet.Parameters.AddWithValue("@IsInwardTempo", false);
                        objBaseSqlManager.ExecuteNonQuery(cmdGet);
                        objBaseSqlManager.ForceCloseConnection();
                    }
                }
            }
            if (obj.RetailTempoDate3 != null && obj.RetailVehicleNo3 != null)
            {
                string[] RetailVehicleNo3 = obj.RetailVehicleNo3.Split(',');
                foreach (var item in RetailVehicleNo3)
                {
                    SqlCommand cmdGet = new SqlCommand();
                    using (var objBaseSqlManager = new BaseSqlManager())
                    {
                        cmdGet.CommandType = CommandType.StoredProcedure;
                        cmdGet.CommandText = "UpdateRetailIsInwardTempoStatus";
                        cmdGet.Parameters.AddWithValue("@AssignedDate", obj.RetailTempoDate3);
                        cmdGet.Parameters.AddWithValue("@VehicleNo", item);
                        cmdGet.Parameters.AddWithValue("@IsInwardTempo", false);
                        objBaseSqlManager.ExecuteNonQuery(cmdGet);
                        objBaseSqlManager.ForceCloseConnection();
                    }
                }
            }
            using (VirakiEntities context = new VirakiEntities())
            {
                if (obj.InwardID == 0)
                {
                    context.InwardOutward_Mst.Add(obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (obj.InwardID > 0)
                {
                    if (data.lstVehicleInwardCost != null)
                    {
                        foreach (var item in data.lstVehicleInwardCost)
                        {
                            VehicleInwardCost_Mst objInwardCost = new VehicleInwardCost_Mst();
                            objInwardCost.VehicleInwardCostID = item.VehicleInwardCostID;
                            objInwardCost.ID = item.ID;
                            objInwardCost.InwardID = obj.InwardID;
                            objInwardCost.VehicleDetailID = item.VehicleDetailID;

                            // 27 Aug 2020 Piyush Limbani
                            //if (objInwardCost.ID == 1 && obj.InwardID != 0)
                            //{
                            //    string[] VehicleNumber = item.VehicleNumber.Split('/');
                            //    string VehicleNo = VehicleNumber[0];
                            //    objInwardCost.AssignedDate = Convert.ToDateTime(VehicleNumber[1]); 
                            //}
                            //else
                            //{
                            //    objInwardCost.AssignedDate = item.AssignedDate;
                            //}
                            objInwardCost.AssignedDate = item.AssignedDate;
                            if (objInwardCost.ID == 1)
                            {
                                objInwardCost.IsVehicleStatus = true;
                                if (objInwardCost.VehicleDetailID != 0 && objInwardCost.AssignedDate != null)
                                {
                                    bool success = UpdateIsVehicleStatus(objInwardCost.VehicleDetailID, objInwardCost.AssignedDate);
                                }
                            }

                            if (objInwardCost.ID == 2)
                            {
                                objInwardCost.IsVehicleStatus = false;
                            }
                            // 27 Aug 2020 Piyush Limbani

                            objInwardCost.Amount = item.Amount;
                            objInwardCost.CreatedBy = data.CreatedBy;
                            objInwardCost.CreatedOn = data.CreatedOn;
                            objInwardCost.UpdatedBy = data.UpdatedBy;
                            objInwardCost.UpdatedOn = data.UpdatedOn;
                            objInwardCost.IsDelete = data.IsDelete;
                            if (objInwardCost.VehicleInwardCostID == 0)
                            {
                                context.VehicleInwardCost_Mst.Add(objInwardCost);
                                context.SaveChanges();
                            }
                            else
                            {
                                context.Entry(objInwardCost).State = EntityState.Modified;
                                context.SaveChanges();
                            }
                        }
                    }
                    return obj.InwardID;
                }
                else
                {
                    return 0;
                }
            }
        }

        // 28 Aug 2020 Piyush Limbani
        public bool UpdateIsVehicleStatus(long? VehicleDetailID, DateTime? AssignedDate)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                SqlCommand cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "UpdateIsVehicleStatus";
                    cmdGet.Parameters.AddWithValue("@VehicleDetailID", VehicleDetailID);
                    cmdGet.Parameters.AddWithValue("@AssignedDate", AssignedDate);
                    cmdGet.Parameters.AddWithValue("@IsVehicleStatus", true);
                    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                    objBaseSqlManager.ForceCloseConnection();
                }
            }
            return true;
        }

        public List<InwardOutWardListResponse> GetAllInwardList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllInwardList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<InwardOutWardListResponse> objlst = new List<InwardOutWardListResponse>();
                while (dr.Read())
                {
                    InwardOutWardListResponse obj = new InwardOutWardListResponse();
                    obj.InwardID = objBaseSqlManager.GetInt64(dr, "InwardID");
                    obj.GodownID = objBaseSqlManager.GetInt64(dr, "GodownID");
                    obj.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    obj.OpeningAmount = objBaseSqlManager.GetDecimal(dr, "OpeningAmount");
                    obj.ChillarInward = objBaseSqlManager.GetDecimal(dr, "ChillarInward");
                    obj.WholesaleAssignedDate = objBaseSqlManager.GetDateTime(dr, "WholesaleAssignedDate");
                    if (obj.WholesaleAssignedDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.WholesaleAssignedDatestr = obj.WholesaleAssignedDate.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.WholesaleAssignedDatestr = "";
                    }
                    obj.WholesaleCash = objBaseSqlManager.GetDecimal(dr, "WholesaleCash");
                    obj.RetailAssignedDate = objBaseSqlManager.GetDateTime(dr, "RetailAssignedDate");
                    if (obj.RetailAssignedDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.RetailAssignedDatestr = obj.WholesaleAssignedDate.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.RetailAssignedDatestr = "";
                    }
                    obj.RetailCash = objBaseSqlManager.GetDecimal(dr, "RetailCash");
                    // Wholesale Tempo
                    obj.WholesaleTempoDate1 = objBaseSqlManager.GetDateTime(dr, "WholesaleTempoDate1");
                    if (obj.WholesaleTempoDate1 != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.WholesaleTempoDate1str = obj.WholesaleTempoDate1.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.WholesaleTempoDate1str = "";
                    }
                    obj.WholesaleVehicleNo1 = objBaseSqlManager.GetTextValue(dr, "WholesaleVehicleNo1");
                    obj.WholesaleTempoAmount1 = objBaseSqlManager.GetDecimal(dr, "WholesaleTempoAmount1");
                    obj.WholesaleTempoDate2 = objBaseSqlManager.GetDateTime(dr, "WholesaleTempoDate2");
                    if (obj.WholesaleTempoDate2 != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.WholesaleTempoDate2str = obj.WholesaleTempoDate2.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.WholesaleTempoDate2str = "";
                    }
                    obj.WholesaleVehicleNo2 = objBaseSqlManager.GetTextValue(dr, "WholesaleVehicleNo2");
                    obj.WholesaleTempoAmount2 = objBaseSqlManager.GetDecimal(dr, "WholesaleTempoAmount2");
                    obj.WholesaleTempoDate3 = objBaseSqlManager.GetDateTime(dr, "WholesaleTempoDate3");
                    if (obj.WholesaleTempoDate3 != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.WholesaleTempoDate3str = obj.WholesaleTempoDate3.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.WholesaleTempoDate3str = "";
                    }
                    obj.WholesaleVehicleNo3 = objBaseSqlManager.GetTextValue(dr, "WholesaleVehicleNo3");
                    obj.WholesaleTempoAmount3 = objBaseSqlManager.GetDecimal(dr, "WholesaleTempoAmount3");
                    obj.WholesaleTempoAmount = obj.WholesaleTempoAmount1 + obj.WholesaleTempoAmount2 + obj.WholesaleTempoAmount3;
                    //Retail Tempo
                    obj.RetailTempoDate1 = objBaseSqlManager.GetDateTime(dr, "RetailTempoDate1");
                    if (obj.RetailTempoDate1 != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.RetailTempoDate1str = obj.RetailTempoDate1.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.RetailTempoDate1str = "";
                    }
                    obj.RetailVehicleNo1 = objBaseSqlManager.GetTextValue(dr, "RetailVehicleNo1");
                    obj.RetailTempoAmount1 = objBaseSqlManager.GetDecimal(dr, "RetailTempoAmount1");
                    obj.RetailTempoDate2 = objBaseSqlManager.GetDateTime(dr, "RetailTempoDate2");
                    if (obj.RetailTempoDate2 != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.RetailTempoDate2str = obj.RetailTempoDate2.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.RetailTempoDate2str = "";
                    }
                    obj.RetailVehicleNo2 = objBaseSqlManager.GetTextValue(dr, "RetailVehicleNo2");
                    obj.RetailTempoAmount2 = objBaseSqlManager.GetDecimal(dr, "RetailTempoAmount2");
                    obj.RetailTempoDate3 = objBaseSqlManager.GetDateTime(dr, "RetailTempoDate3");
                    if (obj.RetailTempoDate3 != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.RetailTempoDate2str = obj.RetailTempoDate3.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.RetailTempoDate3str = "";
                    }
                    obj.RetailVehicleNo3 = objBaseSqlManager.GetTextValue(dr, "RetailVehicleNo3");
                    obj.RetailTempoAmount3 = objBaseSqlManager.GetDecimal(dr, "RetailTempoAmount3");
                    obj.RetailTempoAmount = obj.RetailTempoAmount1 + obj.RetailTempoAmount2 + obj.RetailTempoAmount3;
                    obj.TotalInwardVehicleExpenses = objBaseSqlManager.GetDecimal(dr, "TotalInwardVehicleExpenses");
                    obj.TotalInward = objBaseSqlManager.GetDecimal(dr, "TotalInward");
                    obj.TransferAmountInward = objBaseSqlManager.GetDecimal(dr, "TransferAmountInward");
                    //  obj.FinalTotalInward = obj.TotalInward + obj.TransferAmountInward;
                    // Outward
                    obj.ChillarOutward = objBaseSqlManager.GetDecimal(dr, "ChillarOutward");
                    obj.ExpensesDate = objBaseSqlManager.GetDateTime(dr, "ExpensesDate");
                    if (obj.ExpensesDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.ExpensesDatestr = obj.ExpensesDate.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.ExpensesDatestr = "";
                    }
                    obj.TotalExpenses = objBaseSqlManager.GetDecimal(dr, "TotalExpenses");
                    obj.TransferVB2 = objBaseSqlManager.GetDecimal(dr, "TransferVB2");
                    obj.TransferVB3 = objBaseSqlManager.GetDecimal(dr, "TransferVB3");
                    obj.TransferAmount = objBaseSqlManager.GetDecimal(dr, "TransferAmount");
                    obj.BankID1 = objBaseSqlManager.GetInt64(dr, "BankID1");
                    obj.BankName1 = objBaseSqlManager.GetTextValue(dr, "BankName1");
                    obj.BankDepositeAmount1 = objBaseSqlManager.GetDecimal(dr, "BankDepositeAmount1");
                    obj.BankID2 = objBaseSqlManager.GetInt64(dr, "BankID2");
                    obj.BankName2 = objBaseSqlManager.GetTextValue(dr, "BankName2");
                    obj.BankDepositeAmount2 = objBaseSqlManager.GetDecimal(dr, "BankDepositeAmount2");
                    obj.BankID3 = objBaseSqlManager.GetInt64(dr, "BankID3");
                    obj.BankName3 = objBaseSqlManager.GetTextValue(dr, "BankName3");
                    obj.BankDepositeAmount3 = objBaseSqlManager.GetDecimal(dr, "BankDepositeAmount3");
                    obj.BankDepositeAmount = obj.BankDepositeAmount1 + obj.BankDepositeAmount2 + obj.BankDepositeAmount3;
                    obj.TotalOutwardVehicleExpenses = objBaseSqlManager.GetDecimal(dr, "TotalOutwardVehicleExpenses");
                    obj.TotalOutward = objBaseSqlManager.GetDecimal(dr, "TotalOutward");
                    obj.FinalTotalOutward = (obj.TotalOutward + obj.TransferAmount);
                    obj.GrandTotal = objBaseSqlManager.GetDecimal(dr, "GrandTotal");
                    obj.OpeningChillar = objBaseSqlManager.GetDecimal(dr, "OpeningChillar");
                    obj.ClosingChillar = objBaseSqlManager.GetDecimal(dr, "ClosingChillar");
                    obj.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<VehicleInwardCost> GetVehicleCostListByInwardID(long InwardID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetVehicleCostListByInwardID";
                cmdGet.Parameters.AddWithValue("@InwardID", InwardID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<VehicleInwardCost> objlst = new List<VehicleInwardCost>();
                while (dr.Read())
                {
                    VehicleInwardCost obj = new VehicleInwardCost();
                    obj.VehicleInwardCostID = objBaseSqlManager.GetInt64(dr, "VehicleInwardCostID");
                    obj.ID = objBaseSqlManager.GetInt64(dr, "ID");
                    obj.VehicleDetailID = objBaseSqlManager.GetInt64(dr, "VehicleDetailID");

                    obj.AssignedDate = objBaseSqlManager.GetDateTime(dr, "AssignedDate");
                    // obj.AssignedDatestr = string.Format("{0:G}", obj.AssignedDate);

                    if (obj.AssignedDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.AssignedDatestr = obj.AssignedDate.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.AssignedDatestr = null;
                    }

                    obj.Amount = objBaseSqlManager.GetDecimal(dr, "Amount");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<InwardOutWardListResponse> GetInwardListByDate(DateTime? FromDate)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetInwardListByDate";
                cmdGet.Parameters.AddWithValue("@FromDate", FromDate);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<InwardOutWardListResponse> objlst = new List<InwardOutWardListResponse>();
                while (dr.Read())
                {
                    InwardOutWardListResponse obj = new InwardOutWardListResponse();
                    obj.InwardID = objBaseSqlManager.GetInt64(dr, "InwardID");
                    obj.GodownID = objBaseSqlManager.GetInt64(dr, "GodownID");
                    obj.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    obj.OpeningAmount = objBaseSqlManager.GetDecimal(dr, "OpeningAmount");
                    obj.ChillarInward = objBaseSqlManager.GetDecimal(dr, "ChillarInward");
                    obj.WholesaleCash = objBaseSqlManager.GetDecimal(dr, "WholesaleCash");
                    obj.RetailCash = objBaseSqlManager.GetDecimal(dr, "RetailCash");
                    // Wholesale Tempo
                    obj.WholesaleTempoDate1 = objBaseSqlManager.GetDateTime(dr, "WholesaleTempoDate1");
                    if (obj.WholesaleTempoDate1 != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.WholesaleTempoDate1str = obj.WholesaleTempoDate1.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        obj.WholesaleTempoDate1str = "";
                    }
                    obj.WholesaleVehicleNo1 = objBaseSqlManager.GetTextValue(dr, "WholesaleVehicleNo1");
                    obj.WholesaleTempoAmount1 = objBaseSqlManager.GetDecimal(dr, "WholesaleTempoAmount1");
                    obj.WholesaleTempoDate2 = objBaseSqlManager.GetDateTime(dr, "WholesaleTempoDate2");
                    if (obj.WholesaleTempoDate2 != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.WholesaleTempoDate2str = obj.WholesaleTempoDate2.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        obj.WholesaleTempoDate2str = "";
                    }
                    obj.WholesaleVehicleNo2 = objBaseSqlManager.GetTextValue(dr, "WholesaleVehicleNo2");
                    obj.WholesaleTempoAmount2 = objBaseSqlManager.GetDecimal(dr, "WholesaleTempoAmount2");
                    obj.WholesaleTempoDate3 = objBaseSqlManager.GetDateTime(dr, "WholesaleTempoDate3");
                    if (obj.WholesaleTempoDate3 != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.WholesaleTempoDate3str = obj.WholesaleTempoDate3.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        obj.WholesaleTempoDate3str = "";
                    }
                    obj.WholesaleVehicleNo3 = objBaseSqlManager.GetTextValue(dr, "WholesaleVehicleNo3");
                    obj.WholesaleTempoAmount3 = objBaseSqlManager.GetDecimal(dr, "WholesaleTempoAmount3");
                    obj.WholesaleTempoAmount = obj.WholesaleTempoAmount1 + obj.WholesaleTempoAmount2 + obj.WholesaleTempoAmount3;
                    //Retail Tempo
                    obj.RetailTempoDate1 = objBaseSqlManager.GetDateTime(dr, "RetailTempoDate1");
                    if (obj.RetailTempoDate1 != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.RetailTempoDate1str = obj.RetailTempoDate1.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        obj.RetailTempoDate1str = "";
                    }
                    obj.RetailVehicleNo1 = objBaseSqlManager.GetTextValue(dr, "RetailVehicleNo1");
                    obj.RetailTempoAmount1 = objBaseSqlManager.GetDecimal(dr, "RetailTempoAmount1");
                    obj.RetailTempoDate2 = objBaseSqlManager.GetDateTime(dr, "RetailTempoDate2");
                    if (obj.RetailTempoDate2 != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.RetailTempoDate2str = obj.RetailTempoDate2.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        obj.RetailTempoDate2str = "";
                    }
                    obj.RetailVehicleNo2 = objBaseSqlManager.GetTextValue(dr, "RetailVehicleNo2");
                    obj.RetailTempoAmount2 = objBaseSqlManager.GetDecimal(dr, "RetailTempoAmount2");
                    obj.RetailTempoDate3 = objBaseSqlManager.GetDateTime(dr, "RetailTempoDate3");
                    if (obj.RetailTempoDate3 != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.RetailTempoDate2str = obj.RetailTempoDate3.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        obj.RetailTempoDate3str = "";
                    }
                    obj.RetailVehicleNo3 = objBaseSqlManager.GetTextValue(dr, "RetailVehicleNo3");
                    obj.RetailTempoAmount3 = objBaseSqlManager.GetDecimal(dr, "RetailTempoAmount3");
                    obj.RetailTempoAmount = obj.RetailTempoAmount1 + obj.RetailTempoAmount2 + obj.RetailTempoAmount3;
                    obj.TransferAmountInward = objBaseSqlManager.GetDecimal(dr, "TransferAmountInward");
                    obj.TotalInwardVehicleExpenses = objBaseSqlManager.GetDecimal(dr, "TotalInwardVehicleExpenses");
                    obj.TotalInward = objBaseSqlManager.GetDecimal(dr, "TotalInward");
                    // obj.FinalTotalInward = obj.TotalInward + obj.TransferAmountInward;
                    // Outward
                    obj.ChillarOutward = objBaseSqlManager.GetDecimal(dr, "ChillarOutward");
                    obj.ExpensesDate = objBaseSqlManager.GetDateTime(dr, "ExpensesDate");
                    if (obj.ExpensesDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.ExpensesDatestr = obj.ExpensesDate.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.ExpensesDatestr = "";
                    }
                    obj.TotalExpenses = objBaseSqlManager.GetDecimal(dr, "TotalExpenses");
                    obj.TransferVB2 = objBaseSqlManager.GetDecimal(dr, "TransferVB2");
                    obj.TransferVB3 = objBaseSqlManager.GetDecimal(dr, "TransferVB3");
                    obj.BankID1 = objBaseSqlManager.GetInt64(dr, "BankID1");
                    obj.BankName1 = objBaseSqlManager.GetTextValue(dr, "BankName1");
                    obj.BankDepositeAmount1 = objBaseSqlManager.GetDecimal(dr, "BankDepositeAmount1");
                    obj.BankID2 = objBaseSqlManager.GetInt64(dr, "BankID2");
                    obj.BankName2 = objBaseSqlManager.GetTextValue(dr, "BankName2");
                    obj.BankDepositeAmount2 = objBaseSqlManager.GetDecimal(dr, "BankDepositeAmount2");
                    obj.BankID3 = objBaseSqlManager.GetInt64(dr, "BankID3");
                    obj.BankName3 = objBaseSqlManager.GetTextValue(dr, "BankName3");
                    obj.BankDepositeAmount3 = objBaseSqlManager.GetDecimal(dr, "BankDepositeAmount3");
                    obj.BankDepositeAmount = obj.BankDepositeAmount1 + obj.BankDepositeAmount2 + obj.BankDepositeAmount3;
                    obj.TransferAmount = objBaseSqlManager.GetDecimal(dr, "TransferAmountOutward");
                    obj.TotalOutwardVehicleExpenses = objBaseSqlManager.GetDecimal(dr, "TotalOutwardVehicleExpenses");
                    obj.TotalOutward = objBaseSqlManager.GetDecimal(dr, "TotalOutward");
                    //  obj.FinalTotalOutward = (obj.TotalOutward + obj.TransferAmount);
                    obj.GrandTotal = objBaseSqlManager.GetDecimal(dr, "GrandTotal");
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<ExpensesVoucherListResponse> GetExpensesVoucherListByGodownID(DateTime ExpensesDate, long GodownID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetExpensesVoucherListByGodownID";
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                cmdGet.Parameters.AddWithValue("@ExpensesDate", ExpensesDate);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<ExpensesVoucherListResponse> objlst = new List<ExpensesVoucherListResponse>();
                while (dr.Read())
                {
                    ExpensesVoucherListResponse obj = new ExpensesVoucherListResponse();
                    obj.Amount = objBaseSqlManager.GetDecimal(dr, "Amount");
                    obj.Pay = objBaseSqlManager.GetTextValue(dr, "Pay");
                    obj.DebitAccountType = objBaseSqlManager.GetTextValue(dr, "DebitAccountType");
                    obj.TotalAmount = objBaseSqlManager.GetDecimal(dr, "TotalAmount");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<InwardOutWardListResponse> GetInwardOutwardDetailForPrint(long InwardID, long GodownID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetInwardOutwardDetailForPrint";
                cmdGet.Parameters.AddWithValue("@InwardID", InwardID);
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<InwardOutWardListResponse> objlst = new List<InwardOutWardListResponse>();
                while (dr.Read())
                {
                    InwardOutWardListResponse obj = new InwardOutWardListResponse();
                    obj.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    obj.TransferVB2 = objBaseSqlManager.GetDecimal(dr, "TransferVB2");
                    obj.TransferVB3 = objBaseSqlManager.GetDecimal(dr, "TransferVB3");
                    obj.TotalTransferAmount = objBaseSqlManager.GetDecimal(dr, "TotalTransferAmount");
                    obj.OpeningAmount = objBaseSqlManager.GetDecimal(dr, "OpeningAmount");
                    obj.WholesaleCash = objBaseSqlManager.GetDecimal(dr, "WholesaleCash");
                    obj.RetailCash = objBaseSqlManager.GetDecimal(dr, "RetailCash");
                    obj.WholesaleTempoAmount1 = objBaseSqlManager.GetDecimal(dr, "WholesaleTempoAmount1");
                    obj.WholesaleTempoAmount2 = objBaseSqlManager.GetDecimal(dr, "WholesaleTempoAmount2");
                    obj.WholesaleTempoAmount3 = objBaseSqlManager.GetDecimal(dr, "WholesaleTempoAmount3");
                    obj.RetailTempoAmount1 = objBaseSqlManager.GetDecimal(dr, "RetailTempoAmount1");
                    obj.RetailTempoAmount2 = objBaseSqlManager.GetDecimal(dr, "RetailTempoAmount2");
                    obj.RetailTempoAmount3 = objBaseSqlManager.GetDecimal(dr, "RetailTempoAmount3");
                    obj.WholesaleTempoAmount = objBaseSqlManager.GetDecimal(dr, "WholesaleTempoAmount");
                    obj.RetailTempoAmount = objBaseSqlManager.GetDecimal(dr, "RetailTempoAmount");
                    obj.Total = objBaseSqlManager.GetDecimal(dr, "Total");
                    obj.InwardTotal = objBaseSqlManager.GetDecimal(dr, "InwardTotal");
                    obj.OpeningChillar = objBaseSqlManager.GetDecimal(dr, "OpeningChillar");
                    obj.ChillarInward = objBaseSqlManager.GetDecimal(dr, "ChillarInward");
                    obj.ChillarOutward = objBaseSqlManager.GetDecimal(dr, "ChillarOutward");
                    obj.ClosingChillar = objBaseSqlManager.GetDecimal(dr, "ClosingChillar");
                    obj.TotalExpenses = objBaseSqlManager.GetDecimal(dr, "TotalExpenses");
                    obj.BankName1 = objBaseSqlManager.GetTextValue(dr, "BankName1");
                    obj.BankDepositeAmount1 = objBaseSqlManager.GetDecimal(dr, "BankDepositeAmount1");
                    obj.BankName2 = objBaseSqlManager.GetTextValue(dr, "BankName2");
                    obj.BankDepositeAmount2 = objBaseSqlManager.GetDecimal(dr, "BankDepositeAmount2");
                    obj.BankName3 = objBaseSqlManager.GetTextValue(dr, "BankName3");
                    obj.BankDepositeAmount3 = objBaseSqlManager.GetDecimal(dr, "BankDepositeAmount3");
                    obj.OutwardTotal = objBaseSqlManager.GetDecimal(dr, "OutwardTotal");
                    obj.GrandTotal = objBaseSqlManager.GetDecimal(dr, "GrandTotal");
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<GodownListResponse> GetGodownForToGodown(long FromGodownID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetGodownForToGodown";
                cmdGet.Parameters.AddWithValue("@FromGodownID", FromGodownID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<GodownListResponse> objlst = new List<GodownListResponse>();
                while (dr.Read())
                {
                    GodownListResponse objAreaList = new GodownListResponse();
                    objAreaList.GodownID = objBaseSqlManager.GetInt64(dr, "GodownID");
                    objAreaList.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    objlst.Add(objAreaList);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public long AddTransferAmount(TransferAmount_Mst Obj)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (Obj.TransferID == 0)
                {
                    context.TransferAmount_Mst.Add(Obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(Obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (Obj.TransferID > 0)
                {
                    return Obj.TransferID;
                }
                else
                {
                    return 0;
                }
            }
        }

        public GetWholesaleAmount GetTopGrandTotalAmountByGodownIDandCreatedDateForOutward(long FromGodownID, DateTime CreatedOn)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetTopGrandTotalAmountByGodownIDandCreatedDateForOutward";
                cmdGet.Parameters.AddWithValue("@FromGodownID", FromGodownID);
                cmdGet.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                GetWholesaleAmount obj = new GetWholesaleAmount();
                while (dr.Read())
                {
                    obj.InwardID = objBaseSqlManager.GetInt64(dr, "InwardID");
                    obj.OpeningAmount = objBaseSqlManager.GetDecimal(dr, "GrandTotal");
                    obj.TotalOutward = objBaseSqlManager.GetDecimal(dr, "TotalOutward");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        public bool UpdateGrandTotalForOutwardGodown(long InwardID, long GodownID, decimal GrandTotal, decimal TotalOutward)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "UpdateGrandTotalForOutwardGodown";
                cmdGet.Parameters.AddWithValue("@InwardID", InwardID);
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                cmdGet.Parameters.AddWithValue("@GrandTotal", GrandTotal);
                cmdGet.Parameters.AddWithValue("@TotalOutward", TotalOutward);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
                return true;
            }
        }

        public GetWholesaleAmount GetTopGrandTotalAmountByGodownIDandCreatedDateForInward(long ToGodownID, DateTime CreatedOn)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetTopGrandTotalAmountByGodownIDandCreatedDateForInward";
                cmdGet.Parameters.AddWithValue("@ToGodownID", ToGodownID);
                cmdGet.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                GetWholesaleAmount obj = new GetWholesaleAmount();
                while (dr.Read())
                {
                    obj.InwardID = objBaseSqlManager.GetInt64(dr, "InwardID");
                    obj.OpeningAmount = objBaseSqlManager.GetDecimal(dr, "GrandTotal");
                    obj.TotalInward = objBaseSqlManager.GetDecimal(dr, "TotalInward");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        public bool UpdateGrandTotalForInwardGodown(long InwardID, long GodownID, decimal GrandTotal, decimal TotalInward)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "UpdateGrandTotalForInwardGodown";
                cmdGet.Parameters.AddWithValue("@InwardID", InwardID);
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                cmdGet.Parameters.AddWithValue("@GrandTotal", GrandTotal);
                cmdGet.Parameters.AddWithValue("@TotalInward", TotalInward);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
                return true;
            }
        }

        public List<TransferAmountListResponse> GetAllTrasferAmountList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllTrasferAmountList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<TransferAmountListResponse> objlst = new List<TransferAmountListResponse>();
                while (dr.Read())
                {
                    TransferAmountListResponse obj = new TransferAmountListResponse();
                    obj.TransferID = objBaseSqlManager.GetInt64(dr, "TransferID");
                    obj.FromGodownID = objBaseSqlManager.GetInt64(dr, "FromGodownID");
                    obj.FromGodownName = objBaseSqlManager.GetTextValue(dr, "FromGodownName");
                    obj.ToGodownID = objBaseSqlManager.GetInt64(dr, "ToGodownID");
                    obj.ToGodownName = objBaseSqlManager.GetTextValue(dr, "ToGodownName");
                    obj.Amount = objBaseSqlManager.GetDecimal(dr, "Amount");
                    obj.CreatedName = objBaseSqlManager.GetTextValue(dr, "CreatedName");
                    obj.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<TransferAmountListResponse> GetPopupTransferAmountList(long GodownID, DateTime CreatedOn)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetPopupTransferAmountList";
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                cmdGet.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<TransferAmountListResponse> objlst = new List<TransferAmountListResponse>();
                while (dr.Read())
                {
                    TransferAmountListResponse objCustomer = new TransferAmountListResponse();
                    objCustomer.TransferID = objBaseSqlManager.GetInt64(dr, "TransferID");
                    objCustomer.FromGodownID = objBaseSqlManager.GetInt64(dr, "FromGodownID");
                    objCustomer.FromGodownName = objBaseSqlManager.GetTextValue(dr, "FromGodownName");
                    objCustomer.ToGodownID = objBaseSqlManager.GetInt64(dr, "ToGodownID");
                    objCustomer.ToGodownName = objBaseSqlManager.GetTextValue(dr, "ToGodownName");
                    objCustomer.Amount = objBaseSqlManager.GetDecimal(dr, "Amount");
                    objlst.Add(objCustomer);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<TransferAmountListResponse> GetTransferAmountListForPrint(DateTime CreatedOn, long GodownID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetTransferAmountListForPrint";
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                cmdGet.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<TransferAmountListResponse> objlst = new List<TransferAmountListResponse>();
                while (dr.Read())
                {
                    TransferAmountListResponse objCustomer = new TransferAmountListResponse();
                    objCustomer.TransferID = objBaseSqlManager.GetInt64(dr, "TransferID");
                    objCustomer.FromGodownID = objBaseSqlManager.GetInt64(dr, "FromGodownID");
                    objCustomer.FromGodownName = objBaseSqlManager.GetTextValue(dr, "FromGodownName");
                    objCustomer.ToGodownID = objBaseSqlManager.GetInt64(dr, "ToGodownID");
                    objCustomer.ToGodownName = objBaseSqlManager.GetTextValue(dr, "ToGodownName");
                    objCustomer.Amount = objBaseSqlManager.GetDecimal(dr, "Amount");
                    objlst.Add(objCustomer);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<TransferAmountListResponse> GetTransferAmountListForPrintInward(DateTime CreatedOn, long GodownID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetTransferAmountListForPrintInward";
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                cmdGet.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<TransferAmountListResponse> objlst = new List<TransferAmountListResponse>();
                while (dr.Read())
                {
                    TransferAmountListResponse objCustomer = new TransferAmountListResponse();
                    objCustomer.TransferID = objBaseSqlManager.GetInt64(dr, "TransferID");
                    objCustomer.FromGodownID = objBaseSqlManager.GetInt64(dr, "FromGodownID");
                    objCustomer.FromGodownName = objBaseSqlManager.GetTextValue(dr, "FromGodownName");
                    objCustomer.ToGodownID = objBaseSqlManager.GetInt64(dr, "ToGodownID");
                    objCustomer.ToGodownName = objBaseSqlManager.GetTextValue(dr, "ToGodownName");
                    objCustomer.Amount = objBaseSqlManager.GetDecimal(dr, "Amount");
                    objlst.Add(objCustomer);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<VehicleExpensesList> GetInwardVehicleExpensesList(long InwardID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetInwardVehicleExpensesList";
                cmdGet.Parameters.AddWithValue("@InwardID", InwardID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<VehicleExpensesList> objlst = new List<VehicleExpensesList>();
                while (dr.Read())
                {
                    VehicleExpensesList objCustomer = new VehicleExpensesList();
                    objCustomer.VehicleNumber = objBaseSqlManager.GetTextValue(dr, "VehicleNumber");
                    objCustomer.Amount = objBaseSqlManager.GetDecimal(dr, "Amount");
                    objlst.Add(objCustomer);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<VehicleExpensesList> GetOutwardVehicleExpensesList(long InwardID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetOutwardVehicleExpensesList";
                cmdGet.Parameters.AddWithValue("@InwardID", InwardID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<VehicleExpensesList> objlst = new List<VehicleExpensesList>();
                while (dr.Read())
                {
                    VehicleExpensesList objCustomer = new VehicleExpensesList();
                    objCustomer.VehicleNumber = objBaseSqlManager.GetTextValue(dr, "VehicleNumber");
                    objCustomer.Amount = objBaseSqlManager.GetDecimal(dr, "Amount");
                    objlst.Add(objCustomer);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public InwardOutWardListResponse CheckTodaysGodownIsExists(long GodownID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "CheckTodaysGodownIsExists";
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                InwardOutWardListResponse obj = new InwardOutWardListResponse();
                while (dr.Read())
                {
                    obj.InwardID = objBaseSqlManager.GetInt64(dr, "InwardID");
                    obj.GodownID = objBaseSqlManager.GetInt64(dr, "GodownID");
                    obj.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    obj.OpeningAmount = objBaseSqlManager.GetDecimal(dr, "OpeningAmount");
                    obj.ChillarInward = objBaseSqlManager.GetDecimal(dr, "ChillarInward");
                    obj.WholesaleAssignedDate = objBaseSqlManager.GetDateTime(dr, "WholesaleAssignedDate");
                    if (obj.WholesaleAssignedDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.WholesaleAssignedDatestr = obj.WholesaleAssignedDate.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.WholesaleAssignedDatestr = "";
                    }
                    obj.WholesaleCash = objBaseSqlManager.GetDecimal(dr, "WholesaleCash");
                    obj.RetailAssignedDate = objBaseSqlManager.GetDateTime(dr, "RetailAssignedDate");
                    if (obj.RetailAssignedDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.RetailAssignedDatestr = obj.WholesaleAssignedDate.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.RetailAssignedDatestr = "";
                    }
                    obj.RetailCash = objBaseSqlManager.GetDecimal(dr, "RetailCash");
                    // Wholesale Tempo
                    obj.WholesaleTempoDate1 = objBaseSqlManager.GetDateTime(dr, "WholesaleTempoDate1");
                    if (obj.WholesaleTempoDate1 != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.WholesaleTempoDate1str = obj.WholesaleTempoDate1.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.WholesaleTempoDate1str = "";
                    }
                    obj.WholesaleVehicleNo1 = objBaseSqlManager.GetTextValue(dr, "WholesaleVehicleNo1");
                    obj.WholesaleTempoAmount1 = objBaseSqlManager.GetDecimal(dr, "WholesaleTempoAmount1");
                    obj.WholesaleTempoDate2 = objBaseSqlManager.GetDateTime(dr, "WholesaleTempoDate2");
                    if (obj.WholesaleTempoDate2 != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.WholesaleTempoDate2str = obj.WholesaleTempoDate2.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.WholesaleTempoDate2str = "";
                    }
                    obj.WholesaleVehicleNo2 = objBaseSqlManager.GetTextValue(dr, "WholesaleVehicleNo2");
                    obj.WholesaleTempoAmount2 = objBaseSqlManager.GetDecimal(dr, "WholesaleTempoAmount2");
                    obj.WholesaleTempoDate3 = objBaseSqlManager.GetDateTime(dr, "WholesaleTempoDate3");
                    if (obj.WholesaleTempoDate3 != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.WholesaleTempoDate3str = obj.WholesaleTempoDate3.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.WholesaleTempoDate3str = "";
                    }
                    obj.WholesaleVehicleNo3 = objBaseSqlManager.GetTextValue(dr, "WholesaleVehicleNo3");
                    obj.WholesaleTempoAmount3 = objBaseSqlManager.GetDecimal(dr, "WholesaleTempoAmount3");
                    obj.WholesaleTempoAmount = obj.WholesaleTempoAmount1 + obj.WholesaleTempoAmount2 + obj.WholesaleTempoAmount3;
                    //Retail Tempo
                    obj.RetailTempoDate1 = objBaseSqlManager.GetDateTime(dr, "RetailTempoDate1");
                    if (obj.RetailTempoDate1 != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.RetailTempoDate1str = obj.RetailTempoDate1.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.RetailTempoDate1str = "";
                    }
                    obj.RetailVehicleNo1 = objBaseSqlManager.GetTextValue(dr, "RetailVehicleNo1");
                    obj.RetailTempoAmount1 = objBaseSqlManager.GetDecimal(dr, "RetailTempoAmount1");
                    obj.RetailTempoDate2 = objBaseSqlManager.GetDateTime(dr, "RetailTempoDate2");
                    if (obj.RetailTempoDate2 != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.RetailTempoDate2str = obj.RetailTempoDate2.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.RetailTempoDate2str = "";
                    }
                    obj.RetailVehicleNo2 = objBaseSqlManager.GetTextValue(dr, "RetailVehicleNo2");
                    obj.RetailTempoAmount2 = objBaseSqlManager.GetDecimal(dr, "RetailTempoAmount2");
                    obj.RetailTempoDate3 = objBaseSqlManager.GetDateTime(dr, "RetailTempoDate3");
                    if (obj.RetailTempoDate3 != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.RetailTempoDate2str = obj.RetailTempoDate3.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.RetailTempoDate3str = "";
                    }
                    obj.RetailVehicleNo3 = objBaseSqlManager.GetTextValue(dr, "RetailVehicleNo3");
                    obj.RetailTempoAmount3 = objBaseSqlManager.GetDecimal(dr, "RetailTempoAmount3");
                    obj.RetailTempoAmount = obj.RetailTempoAmount1 + obj.RetailTempoAmount2 + obj.RetailTempoAmount3;
                    obj.TotalInward = objBaseSqlManager.GetDecimal(dr, "TotalInward");
                    obj.TransferAmountInward = objBaseSqlManager.GetDecimal(dr, "TransferAmountInward");
                    //  obj.FinalTotalInward = obj.TotalInward + obj.TransferAmountInward;
                    // Outward
                    obj.ChillarOutward = objBaseSqlManager.GetDecimal(dr, "ChillarOutward");
                    obj.ExpensesDate = objBaseSqlManager.GetDateTime(dr, "ExpensesDate");
                    if (obj.ExpensesDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.ExpensesDatestr = obj.ExpensesDate.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.ExpensesDatestr = "";
                    }
                    obj.TotalExpenses = objBaseSqlManager.GetDecimal(dr, "TotalExpenses");
                    obj.TransferVB2 = objBaseSqlManager.GetDecimal(dr, "TransferVB2");
                    obj.TransferVB3 = objBaseSqlManager.GetDecimal(dr, "TransferVB3");
                    obj.TransferAmount = objBaseSqlManager.GetDecimal(dr, "TransferAmount");
                    obj.BankID1 = objBaseSqlManager.GetInt64(dr, "BankID1");
                    obj.BankName1 = objBaseSqlManager.GetTextValue(dr, "BankName1");
                    obj.BankDepositeAmount1 = objBaseSqlManager.GetDecimal(dr, "BankDepositeAmount1");
                    obj.BankID2 = objBaseSqlManager.GetInt64(dr, "BankID2");
                    obj.BankName2 = objBaseSqlManager.GetTextValue(dr, "BankName2");
                    obj.BankDepositeAmount2 = objBaseSqlManager.GetDecimal(dr, "BankDepositeAmount2");
                    obj.BankID3 = objBaseSqlManager.GetInt64(dr, "BankID3");
                    obj.BankName3 = objBaseSqlManager.GetTextValue(dr, "BankName3");
                    obj.BankDepositeAmount3 = objBaseSqlManager.GetDecimal(dr, "BankDepositeAmount3");
                    obj.BankDepositeAmount = obj.BankDepositeAmount1 + obj.BankDepositeAmount2 + obj.BankDepositeAmount3;
                    obj.TotalOutward = objBaseSqlManager.GetDecimal(dr, "TotalOutward");
                    // obj.FinalTotalOutward = (obj.TotalOutward + obj.TransferAmount);
                    obj.GrandTotal = objBaseSqlManager.GetDecimal(dr, "GrandTotal");
                    obj.OpeningChillar = objBaseSqlManager.GetDecimal(dr, "OpeningChillar");
                    obj.ClosingChillar = objBaseSqlManager.GetDecimal(dr, "ClosingChillar");
                    obj.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    obj.CreatedOnstr = string.Format("{0:G}", obj.CreatedOn);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        public List<VehicleNoListResponse> GetAllVehicleNoListForVehicleCosting()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllVehicleNoListForVehicleCosting";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<VehicleNoListResponse> objlst = new List<VehicleNoListResponse>();
                while (dr.Read())
                {
                    VehicleNoListResponse objDriverList = new VehicleNoListResponse();
                    objDriverList.VehicleNo = objBaseSqlManager.GetTextValue(dr, "VehicleNo");
                    objlst.Add(objDriverList);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<RetVehicleNoListResponse> GetAllRetVehicleNoListForVehicleCosting()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllRetVehicleNoListForVehicleCosting";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RetVehicleNoListResponse> objlst = new List<RetVehicleNoListResponse>();
                while (dr.Read())
                {
                    RetVehicleNoListResponse objDriverList = new RetVehicleNoListResponse();
                    objDriverList.VehicleNo = objBaseSqlManager.GetTextValue(dr, "VehicleNo");
                    objlst.Add(objDriverList);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public GetVehicleDetails GetWholesaleVehicleDetailsForVehicleCosting(long WholesaleVehicleNo1, DateTime CreatedOn)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetWholesaleVehicleDetailsForVehicleCosting";
                cmdGet.Parameters.AddWithValue("@VehicleNo", WholesaleVehicleNo1);
                cmdGet.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                GetVehicleDetails obj = new GetVehicleDetails();
                while (dr.Read())
                {
                    obj.VehicleDetailID = objBaseSqlManager.GetInt64(dr, "VehicleDetailID");
                    obj.TempoNo = objBaseSqlManager.GetTextValue(dr, "TempoNo");
                    obj.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    obj.DeliveryPerson1 = objBaseSqlManager.GetInt64(dr, "DeliveryPerson1");
                    obj.DeliveryPerson2 = objBaseSqlManager.GetInt64(dr, "DeliveryPerson2");
                    obj.DeliveryPerson3 = objBaseSqlManager.GetInt64(dr, "DeliveryPerson3");
                    obj.DeliveryPerson4 = objBaseSqlManager.GetInt64(dr, "DeliveryPerson4");
                    decimal WholesaleInvioceAmount1 = GetTempoTotalAmount(WholesaleVehicleNo1, CreatedOn);
                    obj.WholesaleInvioceAmount1 = WholesaleInvioceAmount1;
                    decimal WholesaleTotalKG1 = GetTempoTotalKg(WholesaleVehicleNo1, CreatedOn);
                    obj.WholesaleTotalKG1 = WholesaleTotalKG1;
                    obj.OpeningKM = GetVehicleOpeningKM(obj.VehicleDetailID);
                    obj.DieselOpeningKM = GetDieselOpeningKM(obj.VehicleDetailID);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        public GetVehicleDetails GetWholesaleTotalInvoiceAmountVehicle2(long WholesaleVehicleNo2, DateTime CreatedOn)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetWholesaleVehicleDetailsForVehicleCosting";
                cmdGet.Parameters.AddWithValue("@VehicleNo", WholesaleVehicleNo2);
                cmdGet.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                GetVehicleDetails obj = new GetVehicleDetails();
                while (dr.Read())
                {
                    obj.VehicleDetailID = objBaseSqlManager.GetInt64(dr, "VehicleDetailID");
                    obj.TempoNo = objBaseSqlManager.GetTextValue(dr, "TempoNo");
                    obj.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    obj.DeliveryPerson1 = objBaseSqlManager.GetInt64(dr, "DeliveryPerson1");
                    obj.DeliveryPerson2 = objBaseSqlManager.GetInt64(dr, "DeliveryPerson2");
                    obj.DeliveryPerson3 = objBaseSqlManager.GetInt64(dr, "DeliveryPerson3");
                    obj.DeliveryPerson4 = objBaseSqlManager.GetInt64(dr, "DeliveryPerson4");
                    decimal WholesaleInvioceAmount2 = GetTempoTotalAmount(WholesaleVehicleNo2, CreatedOn);
                    obj.WholesaleInvioceAmount2 = WholesaleInvioceAmount2;
                    decimal WholesaleTotalKG2 = GetTempoTotalKg(WholesaleVehicleNo2, CreatedOn);
                    obj.WholesaleTotalKG2 = WholesaleTotalKG2;
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        public decimal GetTempoTotalAmount(long WholesaleVehicleNo1, DateTime CreatedOn)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetTempoTotalAmount";
                cmdGet.Parameters.AddWithValue("@VehicleNo", WholesaleVehicleNo1);
                cmdGet.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                decimal InvoiceTotalAmt = 0;
                decimal FinalInvoiceTotal = 0;
                while (dr.Read())
                {
                    InvoiceTotalAmt = objBaseSqlManager.GetDecimal(dr, "InvoiceTotal");
                    FinalInvoiceTotal += InvoiceTotalAmt;
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return FinalInvoiceTotal;
            }
        }

        // Get Total KG
        public decimal GetTempoTotalKg(long WholesaleVehicleNo1, DateTime CreatedOn)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetInvoiceNumbersTempoWiseForKG";
                cmdGet.Parameters.AddWithValue("@VehicleNo", WholesaleVehicleNo1);
                cmdGet.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                decimal InvoiceTotalKg = 0;
                decimal TempoTotalKG = 0;
                while (dr.Read())
                {
                    long OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    string InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    InvoiceTotalKg = GetInvoiceTotalKg(OrderID, InvoiceNumber);
                    TempoTotalKG += InvoiceTotalKg;
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return TempoTotalKG;
            }
        }

        public decimal GetInvoiceTotalKg(long OrderID, string InvoiceNumber)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetInvoiceTotalKg";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                decimal InvoiceTotalKg = 0;
                while (dr.Read())
                {
                    InvoiceTotalKg = objBaseSqlManager.GetDecimal(dr, "TotalKG");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return InvoiceTotalKg;
            }
        }

        public GetVehicleDetails GetRetailVehicleDetailsForVehicleCosting(long RetailVehicleNo1, DateTime CreatedOn)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetailVehicleDetailsForVehicleCosting";
                cmdGet.Parameters.AddWithValue("@VehicleNo", RetailVehicleNo1);
                cmdGet.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                GetVehicleDetails obj = new GetVehicleDetails();
                while (dr.Read())
                {
                    obj.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    obj.DeliveryPerson1 = objBaseSqlManager.GetInt64(dr, "DeliveryPerson1");
                    obj.DeliveryPerson2 = objBaseSqlManager.GetInt64(dr, "DeliveryPerson2");
                    obj.DeliveryPerson3 = objBaseSqlManager.GetInt64(dr, "DeliveryPerson3");
                    obj.DeliveryPerson4 = objBaseSqlManager.GetInt64(dr, "DeliveryPerson4");
                    obj.VehicleDetailID = objBaseSqlManager.GetInt64(dr, "VehicleDetailID");
                    obj.TempoNo = objBaseSqlManager.GetTextValue(dr, "TempoNo");
                    decimal RetailInvioceAmount1 = GetRetailTotalInvoiceAmountVehicle(RetailVehicleNo1, CreatedOn);
                    obj.RetailInvioceAmount1 = RetailInvioceAmount1;
                    decimal RetailTotalKG1 = GetRetTempoTotalKg(RetailVehicleNo1, CreatedOn);
                    obj.RetailTotalKG1 = Math.Round(RetailTotalKG1, 3);
                    obj.OpeningKM = GetVehicleOpeningKM(obj.VehicleDetailID);
                    obj.DieselOpeningKM = GetDieselOpeningKM(obj.VehicleDetailID);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        public GetVehicleDetails GetRetailTotalInvoiceAmountVehicle1(long RetailVehicleNo1, DateTime CreatedOn)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetailVehicleDetailsForVehicleCosting";
                cmdGet.Parameters.AddWithValue("@VehicleNo", RetailVehicleNo1);
                cmdGet.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                GetVehicleDetails obj = new GetVehicleDetails();
                while (dr.Read())
                {
                    obj.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    obj.DeliveryPerson1 = objBaseSqlManager.GetInt64(dr, "DeliveryPerson1");
                    obj.DeliveryPerson2 = objBaseSqlManager.GetInt64(dr, "DeliveryPerson2");
                    obj.DeliveryPerson3 = objBaseSqlManager.GetInt64(dr, "DeliveryPerson3");
                    obj.DeliveryPerson4 = objBaseSqlManager.GetInt64(dr, "DeliveryPerson4");
                    obj.VehicleDetailID = objBaseSqlManager.GetInt64(dr, "VehicleDetailID");
                    obj.TempoNo = objBaseSqlManager.GetTextValue(dr, "TempoNo");
                    decimal RetailInvioceAmount1 = GetRetailTotalInvoiceAmountVehicle(RetailVehicleNo1, CreatedOn);
                    obj.RetailInvioceAmount1 = RetailInvioceAmount1;
                    decimal RetailTotalKG1 = GetRetTempoTotalKg(RetailVehicleNo1, CreatedOn);
                    obj.RetailTotalKG1 = Math.Round(RetailTotalKG1, 3);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        public GetVehicleDetails GetRetailTotalInvoiceAmountVehicle2(long RetailVehicleNo2, DateTime CreatedOn)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetailVehicleDetailsForVehicleCosting";
                cmdGet.Parameters.AddWithValue("@VehicleNo", RetailVehicleNo2);
                cmdGet.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                GetVehicleDetails obj = new GetVehicleDetails();
                while (dr.Read())
                {
                    obj.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    obj.DeliveryPerson1 = objBaseSqlManager.GetInt64(dr, "DeliveryPerson1");
                    obj.DeliveryPerson2 = objBaseSqlManager.GetInt64(dr, "DeliveryPerson2");
                    obj.DeliveryPerson3 = objBaseSqlManager.GetInt64(dr, "DeliveryPerson3");
                    obj.DeliveryPerson4 = objBaseSqlManager.GetInt64(dr, "DeliveryPerson4");
                    obj.VehicleDetailID = objBaseSqlManager.GetInt64(dr, "VehicleDetailID");
                    obj.TempoNo = objBaseSqlManager.GetTextValue(dr, "TempoNo");
                    decimal RetailInvioceAmount2 = GetRetailTotalInvoiceAmountVehicle(RetailVehicleNo2, CreatedOn);
                    obj.RetailInvioceAmount2 = RetailInvioceAmount2;
                    decimal RetailTotalKG2 = GetRetTempoTotalKg(RetailVehicleNo2, CreatedOn);
                    obj.RetailTotalKG2 = Math.Round(RetailTotalKG2, 3);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        public decimal GetRetailTotalInvoiceAmountVehicle(long RetailVehicleNo1, DateTime CreatedOn)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetTempoTotalAmountRetail";
                cmdGet.Parameters.AddWithValue("@VehicleNo", RetailVehicleNo1);
                cmdGet.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                decimal InvoiceTotalAmt = 0;
                decimal FinalInvoiceTotal = 0;
                while (dr.Read())
                {
                    InvoiceTotalAmt = objBaseSqlManager.GetDecimal(dr, "InvoiceTotal");
                    FinalInvoiceTotal += InvoiceTotalAmt;
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return FinalInvoiceTotal;
            }
        }

        // Get Retail Total KG
        public decimal GetRetTempoTotalKg(long RetailVehicleNo1, DateTime CreatedOn)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetailInvoiceNumbersTempoWiseForKG";
                cmdGet.Parameters.AddWithValue("@VehicleNo", RetailVehicleNo1);
                cmdGet.Parameters.AddWithValue("@CreatedOn", CreatedOn);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                decimal InvoiceTotalKg = 0;
                decimal TempoTotalKG = 0;
                while (dr.Read())
                {
                    long OrderID = objBaseSqlManager.GetInt64(dr, "OrderID");
                    string InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    InvoiceTotalKg = GetRetailInvoiceTotalKg(OrderID, InvoiceNumber);
                    TempoTotalKG += InvoiceTotalKg;
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return TempoTotalKG;
            }
        }

        public decimal GetRetailInvoiceTotalKg(long OrderID, string InvoiceNumber)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetRetailInvoiceTotalKg";
                cmdGet.Parameters.AddWithValue("@OrderID", OrderID);
                cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                decimal InvoiceTotalKg = 0;
                decimal TotalKg = 0;
                while (dr.Read())
                {
                    TotalKg = objBaseSqlManager.GetDecimal(dr, "TotalKG");
                    InvoiceTotalKg += TotalKg;
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return InvoiceTotalKg;
            }
        }

        public decimal GetVehicleOpeningKM(long VehicleDetailID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetVehicleOpeningKM";
                cmdGet.Parameters.AddWithValue("@VehicleDetailID", VehicleDetailID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                decimal OpeningKM = 0;
                while (dr.Read())
                {
                    OpeningKM = objBaseSqlManager.GetDecimal(dr, "OpeningKM");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return OpeningKM;
            }
        }

        public decimal GetDieselOpeningKM(long VehicleDetailID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetDieselOpeningKM";
                cmdGet.Parameters.AddWithValue("@VehicleDetailID", VehicleDetailID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                decimal DieselOpeningKM = 0;
                while (dr.Read())
                {
                    DieselOpeningKM = objBaseSqlManager.GetDecimal(dr, "DieselOpeningKM");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return DieselOpeningKM;
            }
        }

        public long AddVehicleCosting(VehicleCosting_Mst Obj)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (Obj.VehicleCostingID == 0)
                {
                    context.VehicleCosting_Mst.Add(Obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(Obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (Obj.VehicleCostingID > 0)
                {
                    return Obj.VehicleCostingID;
                }
                else
                {
                    return 0;
                }
            }
        }

        public bool UpdateWholesaleVehicleNo1(int WholesaleVehicleNo1, DateTime WholesaleVehicleNo1Date)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "UpdateWholesaleVehicleNo";
                cmdGet.Parameters.AddWithValue("@VehicleNo", WholesaleVehicleNo1);
                cmdGet.Parameters.AddWithValue("@CreatedOn", WholesaleVehicleNo1Date);
                cmdGet.Parameters.AddWithValue("@IsVehicleCosting", true);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
                return true;
            }
        }

        public bool UpdateWholesaleVehicleNo2(int WholesaleVehicleNo2, DateTime WholesaleVehicleNo2Date)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "UpdateWholesaleVehicleNo";
                cmdGet.Parameters.AddWithValue("@VehicleNo", WholesaleVehicleNo2);
                cmdGet.Parameters.AddWithValue("@CreatedOn", WholesaleVehicleNo2Date);
                cmdGet.Parameters.AddWithValue("@IsVehicleCosting", true);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
                return true;
            }
        }

        public bool UpdateRetailVehicleNo1(int RetailVehicleNo1, DateTime RetailVehicleNo1Date)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "UpdateRetailVehicleNo";
                cmdGet.Parameters.AddWithValue("@VehicleNo", RetailVehicleNo1);
                cmdGet.Parameters.AddWithValue("@CreatedOn", RetailVehicleNo1Date);
                cmdGet.Parameters.AddWithValue("@IsVehicleCosting", true);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
                return true;
            }
        }

        public bool UpdateRetailVehicleNo2(int RetailVehicleNo2, DateTime RetailVehicleNo2Date)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "UpdateRetailVehicleNo";
                cmdGet.Parameters.AddWithValue("@VehicleNo", RetailVehicleNo2);
                cmdGet.Parameters.AddWithValue("@CreatedOn", RetailVehicleNo2Date);
                cmdGet.Parameters.AddWithValue("@IsVehicleCosting", true);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
                return true;
            }
        }

        public List<VehicleCostingListResponse> GetAllVehicleCostingList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllVehicleCostingList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<VehicleCostingListResponse> objlst = new List<VehicleCostingListResponse>();
                string TotalHrs = "";
                string InDatestr = "";
                string InTimestr = "";
                while (dr.Read())
                {
                    VehicleCostingListResponse obj = new VehicleCostingListResponse();
                    obj.VehicleCostingID = objBaseSqlManager.GetInt64(dr, "VehicleCostingID");
                    obj.WholesaleVehicleNo1 = objBaseSqlManager.GetTextValue(dr, "WholesaleVehicleNo1");
                    obj.WholesaleInvioceAmount1 = objBaseSqlManager.GetDecimal(dr, "WholesaleInvioceAmount1");
                    obj.WholesaleTotalKG1 = objBaseSqlManager.GetDecimal(dr, "WholesaleTotalKG1");
                    obj.WholesaleVehicleNo2 = objBaseSqlManager.GetTextValue(dr, "WholesaleVehicleNo2");
                    obj.WholesaleInvioceAmount2 = objBaseSqlManager.GetDecimal(dr, "WholesaleInvioceAmount2");
                    obj.WholesaleTotalKG2 = objBaseSqlManager.GetDecimal(dr, "WholesaleTotalKG2");
                    obj.RetailVehicleNo1 = objBaseSqlManager.GetTextValue(dr, "RetailVehicleNo1");
                    obj.RetailInvioceAmount1 = objBaseSqlManager.GetDecimal(dr, "RetailInvioceAmount1");
                    obj.RetailTotalKG1 = objBaseSqlManager.GetDecimal(dr, "RetailTotalKG1");
                    obj.RetailVehicleNo2 = objBaseSqlManager.GetTextValue(dr, "RetailVehicleNo2");
                    obj.RetailInvioceAmount2 = objBaseSqlManager.GetDecimal(dr, "RetailInvioceAmount2");
                    obj.RetailTotalKG2 = objBaseSqlManager.GetDecimal(dr, "RetailTotalKG2");
                    obj.GrandInvoiceAmount = objBaseSqlManager.GetDecimal(dr, "GrandInvoiceAmount");
                    obj.GrandTotalKG = objBaseSqlManager.GetDecimal(dr, "GrandTotalKG");
                    obj.VehicleDetailID = objBaseSqlManager.GetInt64(dr, "VehicleDetailID");
                    obj.VehicleNumber = objBaseSqlManager.GetTextValue(dr, "VehicleNumber");
                    obj.AreaName = objBaseSqlManager.GetTextValue(dr, "AreaName");
                    obj.DeliveryPerson1 = objBaseSqlManager.GetInt64(dr, "DeliveryPerson1");
                    obj.DeliveryPersonName1 = objBaseSqlManager.GetTextValue(dr, "DeliveryPersonName1");
                    obj.DeliveryPerson2 = objBaseSqlManager.GetInt64(dr, "DeliveryPerson2");
                    obj.DeliveryPersonName2 = objBaseSqlManager.GetTextValue(dr, "DeliveryPersonName2");
                    obj.DeliveryPerson3 = objBaseSqlManager.GetInt64(dr, "DeliveryPerson3");
                    obj.DeliveryPersonName3 = objBaseSqlManager.GetTextValue(dr, "DeliveryPersonName3");
                    obj.DeliveryPerson4 = objBaseSqlManager.GetInt64(dr, "DeliveryPerson4");
                    obj.DeliveryPersonName4 = objBaseSqlManager.GetTextValue(dr, "DeliveryPersonName4");
                    string DeliveryPersons = string.Join(",", obj.DeliveryPersonName1, obj.DeliveryPersonName2, obj.DeliveryPersonName3, obj.DeliveryPersonName4).TrimEnd(',');
                    obj.DeliveryPersons = DeliveryPersons;
                    obj.OpeningKM = objBaseSqlManager.GetDecimal(dr, "OpeningKM");
                    obj.ClosingKM = objBaseSqlManager.GetDecimal(dr, "ClosingKM");
                    obj.OutDateTime = objBaseSqlManager.GetDateTime(dr, "OutDateTime");
                    obj.OutDatestr = obj.OutDateTime.ToString("MM/dd/yyyy");
                    obj.OutTimestr = obj.OutDateTime.ToString("hh:mm tt");
                    obj.InDateTime = objBaseSqlManager.GetDateTime(dr, "InDateTime");
                    if (obj.InDateTime == Convert.ToDateTime("10/10/2014"))
                    {
                        obj.InDateTimestr = "";
                    }
                    else
                    {
                        obj.InDateTimestr = obj.InDateTime.ToString("MM/dd/yyyy hh:mm tt");
                    }
                    if (obj.InDateTime != Convert.ToDateTime("10/10/2014 12:00:00 AM"))
                    {
                        TotalHrs = GetTotalVehicleHrs(obj.OutDateTime, obj.InDateTime);
                        InDatestr = obj.InDateTime.ToString("MM/dd/yyyy");
                        InTimestr = obj.InDateTime.ToString("hh:mm tt");
                    }
                    else
                    {
                        TotalHrs = "";
                        InDatestr = "";
                        InTimestr = "";
                    }
                    obj.InDatestr = InDatestr;
                    obj.InTimestr = InTimestr;
                    obj.TotalHrs = TotalHrs;
                    obj.DieselOpeningKM = objBaseSqlManager.GetDecimal(dr, "DieselOpeningKM");
                    obj.DieselKM = objBaseSqlManager.GetDecimal(dr, "DieselKM");
                    obj.DieselAmount = objBaseSqlManager.GetDecimal(dr, "DieselAmount");
                    obj.DieselLiter = objBaseSqlManager.GetDecimal(dr, "DieselLiter");
                    obj.RepairingAmount = objBaseSqlManager.GetDecimal(dr, "RepairingAmount");
                    obj.RepairingDetail = objBaseSqlManager.GetTextValue(dr, "RepairingDetail");
                    obj.RepairingAmount2 = objBaseSqlManager.GetDecimal(dr, "RepairingAmount2");
                    obj.RepairingDetail2 = objBaseSqlManager.GetTextValue(dr, "RepairingDetail2");
                    obj.RepairingAmount3 = objBaseSqlManager.GetDecimal(dr, "RepairingAmount3");
                    obj.RepairingDetail3 = objBaseSqlManager.GetTextValue(dr, "RepairingDetail3");
                    obj.TollAmount = objBaseSqlManager.GetDecimal(dr, "TollAmount");
                    obj.TollDetail = objBaseSqlManager.GetTextValue(dr, "TollDetail");
                    obj.BharaiAmount = objBaseSqlManager.GetDecimal(dr, "BharaiAmount");
                    obj.BharaiDetail = objBaseSqlManager.GetTextValue(dr, "BharaiDetail");
                    obj.MiscellaneousAmount1 = objBaseSqlManager.GetDecimal(dr, "MiscellaneousAmount1");
                    obj.MiscellaneousAmount2 = objBaseSqlManager.GetDecimal(dr, "MiscellaneousAmount2");
                    obj.MiscellaneousAmount3 = objBaseSqlManager.GetDecimal(dr, "MiscellaneousAmount3");
                    obj.MiscellaneousDetail1 = objBaseSqlManager.GetTextValue(dr, "MiscellaneousDetail1");
                    obj.MiscellaneousDetail2 = objBaseSqlManager.GetTextValue(dr, "MiscellaneousDetail2");
                    obj.MiscellaneousDetail3 = objBaseSqlManager.GetTextValue(dr, "MiscellaneousDetail3");
                    obj.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public string GetTotalVehicleHrs(DateTime OutDateTime, DateTime InDateTime)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetTotalVehicleHrs";
                cmdGet.Parameters.AddWithValue("@OutDateTime", OutDateTime);
                cmdGet.Parameters.AddWithValue("@InDateTime", InDateTime);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                string TotalHrs = "";
                while (dr.Read())
                {
                    TotalHrs = objBaseSqlManager.GetTextValue(dr, "TotalHrs");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return TotalHrs;
            }
        }

        public List<VehicleCostingListReport> GetVehicleCostingListByDate(DateTime FromDate, DateTime ToDate, string VehicleDetailIDs)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetVehicleCostingListByDate";
                cmdGet.Parameters.AddWithValue("@FromDate", FromDate);
                cmdGet.Parameters.AddWithValue("@ToDate", ToDate);
                cmdGet.Parameters.AddWithValue("@VehicleDetailIDs", VehicleDetailIDs);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<VehicleCostingListReport> objlst = new List<VehicleCostingListReport>();
                while (dr.Read())
                {
                    VehicleCostingListReport obj = new VehicleCostingListReport();
                    obj.VehicleDetailID = objBaseSqlManager.GetInt64(dr, "VehicleDetailID");
                    obj.VehicleNumber = objBaseSqlManager.GetTextValue(dr, "VehicleNumber");
                    obj.DieselOpeningKM = objBaseSqlManager.GetDecimal(dr, "DieselOpeningKM");
                    obj.DieselClosingKM = objBaseSqlManager.GetDecimal(dr, "DieselClosingKM");
                    obj.TotalDieselAmount = objBaseSqlManager.GetDecimal(dr, "TotalDieselAmount");
                    obj.TotalDieselLiter = objBaseSqlManager.GetDecimal(dr, "TotalDieselLiter");
                    obj.DieselDiffKM = objBaseSqlManager.GetDecimal(dr, "DieselDiffKM");
                    if (obj.DieselDiffKM != 0 && obj.TotalDieselAmount != 0 && obj.TotalDieselLiter != 0)
                    {
                        obj.AvgDieselCost = Math.Round(obj.TotalDieselAmount / obj.DieselDiffKM, 2);
                        obj.AvgKMPerLtr = Math.Round(obj.DieselDiffKM / obj.TotalDieselLiter, 2);
                    }
                    else
                    {
                        obj.AvgDieselCost = 0;
                        obj.AvgKMPerLtr = 0;
                    }
                    obj.OpeningKM = objBaseSqlManager.GetDecimal(dr, "OpeningKM");
                    obj.ClosingKM = objBaseSqlManager.GetDecimal(dr, "ClosingKM");
                    obj.DiffKM = objBaseSqlManager.GetDecimal(dr, "DifferenceKM");
                    obj.TotalRepairingAmount = objBaseSqlManager.GetDecimal(dr, "TotalRepairingAmount");
                    if (obj.TotalRepairingAmount != 0 && obj.DiffKM != 0)
                    {
                        obj.RepairingCostPerKM = Math.Round(obj.TotalRepairingAmount / obj.DiffKM, 2);
                    }
                    else
                    {
                        obj.RepairingCostPerKM = 0;
                    }
                    obj.TotalTollAmount = objBaseSqlManager.GetDecimal(dr, "TotalTollAmount");
                    if (obj.TotalTollAmount != 0 && obj.DiffKM != 0)
                    {
                        obj.TollCostPerKM = Math.Round(obj.TotalTollAmount / obj.DiffKM, 2);
                    }
                    else
                    {
                        obj.TollCostPerKM = 0;
                    }
                    obj.TotalBharaiAmount = objBaseSqlManager.GetDecimal(dr, "TotalBharaiAmount");
                    if (obj.TotalBharaiAmount != 0 && obj.DiffKM != 0)
                    {
                        obj.BharaiCostPerKM = Math.Round(obj.TotalBharaiAmount / obj.DiffKM, 2);
                    }
                    else
                    {
                        obj.BharaiCostPerKM = 0;
                    }
                    obj.TotalMiscellaneousAmount = objBaseSqlManager.GetDecimal(dr, "TotalMiscellaneousAmount");
                    if (obj.TotalMiscellaneousAmount != 0 && obj.DiffKM != 0)
                    {
                        obj.MiscellaneousCostPerKM = Math.Round(obj.TotalMiscellaneousAmount / obj.DiffKM, 2);
                    }
                    else
                    {
                        obj.MiscellaneousCostPerKM = 0;
                    }
                    //  Remove + obj.AvgKMPerLtr
                    obj.TotalCostPerKM = Math.Round(obj.AvgDieselCost + obj.RepairingCostPerKM + obj.TollCostPerKM + obj.BharaiCostPerKM + obj.MiscellaneousCostPerKM, 2);
                    obj.ActualTotalCost = Math.Round(obj.TotalDieselAmount + obj.TotalRepairingAmount + obj.TotalTollAmount + obj.TotalBharaiAmount + obj.TotalMiscellaneousAmount, 2);
                    obj.GrandInvoiceAmount = objBaseSqlManager.GetDecimal(dr, "GrandInvoiceAmount");

                    obj.PerOfInvoiceAmount = Math.Round(((obj.ActualTotalCost * 100) / obj.GrandInvoiceAmount), 2);

                    obj.GrandTotalKG = objBaseSqlManager.GetDecimal(dr, "GrandTotalKG");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool DeActiveInwardANDVoucherByDate(DateTime? FromDate, DateTime? ToDate, bool IsDactive)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "UpdateDeActiveInwardANDVoucherByDate";
                cmdGet.Parameters.AddWithValue("@FromDate", FromDate);
                cmdGet.Parameters.AddWithValue("@ToDate", ToDate);
                cmdGet.Parameters.AddWithValue("@IsDactive", IsDactive);
                object dr = objBaseSqlManager.ExecuteNonQuery(cmdGet);
                objBaseSqlManager.ForceCloseConnection();
                return true;
            }
        }

        public long AddDeActiveInwardANDVoucherHistory(DeActiveHistory_Mst Obj)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                if (Obj.DeActiveHistoryID == 0)
                {
                    context.DeActiveHistory_Mst.Add(Obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(Obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (Obj.DeActiveHistoryID > 0)
                {
                    return Obj.DeActiveHistoryID;
                }
                else
                {
                    return 0;
                }
            }
        }

        public List<DeActiveInwardVoucherListResponse> GetAllDeActiveInwardVoucherList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllDeActiveInwardVoucherList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<DeActiveInwardVoucherListResponse> objlst = new List<DeActiveInwardVoucherListResponse>();
                while (dr.Read())
                {
                    DeActiveInwardVoucherListResponse obj = new DeActiveInwardVoucherListResponse();
                    obj.FromDate = objBaseSqlManager.GetDateTime(dr, "FromDate");
                    obj.ToDate = objBaseSqlManager.GetDateTime(dr, "ToDate");
                    obj.Status = objBaseSqlManager.GetBoolean(dr, "Status");
                    if (obj.Status == true)
                    {
                        obj.StatusStr = "De-Active";
                    }
                    else
                    {
                        obj.StatusStr = "Active";
                    }
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public string AddPouchInward(PouchInwardOutward data)
        {
            string Message = "";
            long PouchInwardID = 0;
            PouchInwardOutward_Mst obj = new PouchInwardOutward_Mst();
            obj.PouchInwardID = data.PouchInwardID;
            obj.GodownID = data.GodownID;

            obj.PouchNameID = data.PouchNameID;

            obj.PouchID = data.PouchID;
            obj.OpeningPouch = data.OpeningPouch;
            obj.NoofPcs = data.NoofPcs;
            obj.TotalPouch = data.TotalPouch;
            obj.PurchaseDate = data.PurchaseDate;
            obj.SupplierID = data.SupplierID;
            obj.InvoiceNumber = data.InvoiceNumber;
            obj.TotalInwardCost = data.TotalInwardCost;
            obj.CreditDebitStatusID = data.CreditDebitStatusID;
            obj.CreditDebitStatus = data.CreditDebitStatus;
            obj.CreatedBy = data.CreatedBy;
            obj.CreatedOn = data.CreatedOn;
            obj.UpdatedBy = data.UpdatedBy;
            obj.UpdatedOn = data.UpdatedOn;
            obj.IsDelete = data.IsDelete;
            using (VirakiEntities context = new VirakiEntities())
            {
                if (obj.PouchInwardID == 0)
                {
                    PouchInwardID = CheckPouchBillNumberForSupplierIsExist(data.SupplierID, data.InvoiceNumber);
                    if (PouchInwardID == 0)
                    {
                        context.PouchInwardOutward_Mst.Add(obj);
                        context.SaveChanges();
                        Message = "Insert Sucessfully";
                    }
                    else
                    {
                        Message = "Bill Exist";
                    }
                }
                else
                {
                    context.Entry(obj).State = EntityState.Modified;
                    context.SaveChanges();
                    Message = "Updated Sucessfully";
                }
                if (obj.PouchInwardID > 0)
                {
                    try
                    {
                        if (data.lstPouchInwardCost != null)
                        {
                            foreach (var item in data.lstPouchInwardCost)
                            {
                                PouchCost_Mst objInwardCost = new PouchCost_Mst();
                                objInwardCost.PouchCostID = item.PouchCostID;
                                objInwardCost.PouchInwardID = obj.PouchInwardID;
                                objInwardCost.Description = item.Description;
                                objInwardCost.Amount = item.Amount;
                                objInwardCost.CreatedBy = data.CreatedBy;
                                objInwardCost.CreatedOn = data.CreatedOn;
                                objInwardCost.UpdatedBy = data.UpdatedBy;
                                objInwardCost.UpdatedOn = data.UpdatedOn;
                                objInwardCost.IsDelete = data.IsDelete;
                                if (objInwardCost.PouchCostID == 0)
                                {
                                    context.PouchCost_Mst.Add(objInwardCost);
                                    context.SaveChanges();
                                }
                                else
                                {
                                    context.Entry(objInwardCost).State = EntityState.Modified;
                                    context.SaveChanges();
                                }
                            }
                        }
                        return Message;
                    }
                    catch
                    {
                        using (VirakiEntities context1 = new VirakiEntities())
                        {
                            PouchInwardOutward_Mst data1 = context.PouchInwardOutward_Mst.Where(x => x.PouchInwardID == obj.PouchInwardID).FirstOrDefault();
                            if (data != null)
                            {
                                context.PouchInwardOutward_Mst.Remove(data1);
                                context.SaveChanges();
                                Message = "Error";
                                return Message;
                            }
                            else
                            {
                                Message = "Error";
                                return Message;
                            }
                        }
                    }
                }
                else
                {
                    if (PouchInwardID > 0)
                    {
                        Message = "Bill Exist";
                    }
                    else
                    {
                        Message = "Error";
                    }
                    return Message;
                }
            }
        }

        public long CheckPouchBillNumberForSupplierIsExist(long SupplierID, string InvoiceNumber)
        {
            long PouchInwardID = 0;
            try
            {
                SqlCommand cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "CheckPouchBillNumberForSupplierIsExist";
                    cmdGet.Parameters.AddWithValue("@SupplierID", SupplierID);
                    cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                    SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                    while (dr.Read())
                    {
                        PouchInwardID = objBaseSqlManager.GetInt64(dr, "PouchInwardID");
                    }
                    dr.Close();
                    objBaseSqlManager.ForceCloseConnection();
                }
            }
            catch
            {
                PouchInwardID = 0;
            }
            return PouchInwardID;
        }

        public GetOpeningPouch GetOpeningPouchByPouchID(long GodownID, long PouchNameID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetOpeningPouchByPouchID";
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                cmdGet.Parameters.AddWithValue("@PouchNameID", PouchNameID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                GetOpeningPouch obj = new GetOpeningPouch();
                while (dr.Read())
                {
                    obj.OpeningPouch = objBaseSqlManager.GetInt64(dr, "OpeningPouch");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        public List<PouchInwardListResponse> GetAllPouchInwardList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllPouchInwardList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<PouchInwardListResponse> objlst = new List<PouchInwardListResponse>();
                while (dr.Read())
                {
                    PouchInwardListResponse obj = new PouchInwardListResponse();
                    obj.PouchInwardID = objBaseSqlManager.GetInt64(dr, "PouchInwardID");
                    obj.GodownID = objBaseSqlManager.GetInt64(dr, "GodownID");
                    obj.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    obj.PouchID = objBaseSqlManager.GetInt64(dr, "PouchID");
                    obj.PouchNameID = objBaseSqlManager.GetInt64(dr, "PouchNameID");
                    obj.PouchName = objBaseSqlManager.GetTextValue(dr, "PouchName");
                    obj.HSNNumber = objBaseSqlManager.GetTextValue(dr, "HSNNumber");
                    obj.OpeningPouch = objBaseSqlManager.GetInt64(dr, "OpeningPouch");
                    obj.NoofPcs = objBaseSqlManager.GetInt64(dr, "NoofPcs");
                    obj.TotalPouch = objBaseSqlManager.GetInt64(dr, "TotalPouch");
                    obj.PurchaseDate = objBaseSqlManager.GetDateTime(dr, "PurchaseDate");
                    if (obj.PurchaseDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.PurchaseDatestr = obj.PurchaseDate.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.PurchaseDatestr = "";
                    }
                    obj.SupplierID = objBaseSqlManager.GetInt64(dr, "SupplierID");
                    obj.SupplierName = objBaseSqlManager.GetTextValue(dr, "SupplierName");
                    obj.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    //  obj.TotalInwardCost = objBaseSqlManager.GetDecimal(dr, "TotalInwardCost");

                    obj.TotalInwardCost = Math.Round(objBaseSqlManager.GetDecimal(dr, "TotalInwardCost"), 2);


                    obj.CreditDebitStatusID = objBaseSqlManager.GetInt32(dr, "CreditDebitStatusID");
                    obj.CreditDebitStatus = objBaseSqlManager.GetTextValue(dr, "CreditDebitStatusID");
                    obj.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    obj.FullName = objBaseSqlManager.GetTextValue(dr, "FullName");
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    obj.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public long AddPouchOutward(PouchInwardOutward data)
        {
            PouchInwardOutward_Mst obj = new PouchInwardOutward_Mst();
            obj.PouchInwardID = data.PouchInwardID;
            obj.GodownID = data.GodownID;

            obj.PouchNameID = data.PouchNameID;

            obj.PouchID = data.PouchID;
            obj.OpeningPouch = data.OpeningPouch;
            obj.NoofPcs = data.NoofPcs;
            obj.TotalPouch = data.TotalPouch;
            obj.PurchaseDate = data.PurchaseDate;
            obj.CreditDebitStatusID = data.CreditDebitStatusID;
            obj.CreditDebitStatus = data.CreditDebitStatus;
            obj.CreatedBy = data.CreatedBy;
            obj.CreatedOn = data.CreatedOn;
            obj.UpdatedBy = data.UpdatedBy;
            obj.UpdatedOn = data.UpdatedOn;
            obj.IsDelete = data.IsDelete;
            using (VirakiEntities context = new VirakiEntities())
            {
                if (obj.PouchInwardID == 0)
                {
                    context.PouchInwardOutward_Mst.Add(obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (obj.PouchInwardID > 0)
                {
                    return obj.PouchInwardID;
                }
                else
                {
                    return 0;
                }
            }
        }

        public List<PouchInwardCost> GetLastPouchCostByPouchID(long PouchNameID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetLastPouchCostByPouchID";
                cmdGet.Parameters.AddWithValue("@PouchNameID", PouchNameID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<PouchInwardCost> objlst = new List<PouchInwardCost>();
                while (dr.Read())
                {
                    PouchInwardCost obj = new PouchInwardCost();
                    obj.PouchCostID = objBaseSqlManager.GetInt64(dr, "PouchCostID");
                    obj.Description = objBaseSqlManager.GetTextValue(dr, "Description");
                    obj.Amount = objBaseSqlManager.GetDecimal(dr, "Amount");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<PouchInwardCost> GetPouchCostByPouchInwardID(long PouchInwardID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetPouchCostByPouchInwardID";
                cmdGet.Parameters.AddWithValue("@PouchInwardID", PouchInwardID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<PouchInwardCost> objlst = new List<PouchInwardCost>();
                while (dr.Read())
                {
                    PouchInwardCost obj = new PouchInwardCost();
                    obj.PouchCostID = objBaseSqlManager.GetInt64(dr, "PouchCostID");
                    obj.Description = objBaseSqlManager.GetTextValue(dr, "Description");
                    obj.Amount = objBaseSqlManager.GetDecimal(dr, "Amount");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<PouchOutwardListResponse> GetAllPouchOutwardList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllPouchOutwardList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<PouchOutwardListResponse> objlst = new List<PouchOutwardListResponse>();
                while (dr.Read())
                {
                    PouchOutwardListResponse obj = new PouchOutwardListResponse();
                    obj.PouchInwardID = objBaseSqlManager.GetInt64(dr, "PouchInwardID");
                    obj.GodownID = objBaseSqlManager.GetInt64(dr, "GodownID");
                    obj.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    obj.PouchID = objBaseSqlManager.GetInt64(dr, "PouchID");
                    obj.PouchNameID = objBaseSqlManager.GetInt64(dr, "PouchNameID");
                    obj.PouchName = objBaseSqlManager.GetTextValue(dr, "PouchName");
                    obj.HSNNumber = objBaseSqlManager.GetTextValue(dr, "HSNNumber");
                    obj.OpeningPouch = objBaseSqlManager.GetInt64(dr, "OpeningPouch");
                    obj.NoofPcs = objBaseSqlManager.GetInt64(dr, "NoofPcs");
                    obj.TotalPouch = objBaseSqlManager.GetInt64(dr, "TotalPouch");
                    obj.PurchaseDate = objBaseSqlManager.GetDateTime(dr, "PurchaseDate");
                    if (obj.PurchaseDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.PurchaseDatestr = obj.PurchaseDate.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.PurchaseDatestr = "";
                    }
                    obj.CreditDebitStatusID = objBaseSqlManager.GetInt32(dr, "CreditDebitStatusID");
                    obj.CreditDebitStatus = objBaseSqlManager.GetTextValue(dr, "CreditDebitStatusID");
                    obj.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    obj.FullName = objBaseSqlManager.GetTextValue(dr, "FullName");
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    obj.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public long AddNoOfPouchTransfer(AddNoOfPouchTransfer data)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                PouchInwardOutward_Mst objInward = new PouchInwardOutward_Mst();
                objInward.PouchInwardID = 0;
                objInward.GodownID = data.FromGodownID;

                objInward.PouchNameID = data.PouchNameID;

                objInward.PouchID = data.PouchID;
                objInward.OpeningPouch = data.OpeningPouch;
                objInward.NoofPcs = data.TransferNoofPcs;
                objInward.TotalPouch = data.TotalPouch;
                objInward.PurchaseDate = data.TransferDate;
                objInward.CreditDebitStatusID = 3;
                objInward.CreditDebitStatus = "TransferDebit";
                objInward.CreatedBy = data.CreatedBy;
                objInward.CreatedOn = data.CreatedOn;
                objInward.UpdatedBy = data.UpdatedBy;
                objInward.UpdatedOn = data.UpdatedOn;
                objInward.IsDelete = data.IsDelete;
                if (objInward.PouchInwardID == 0)
                {
                    context.PouchInwardOutward_Mst.Add(objInward);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(objInward).State = EntityState.Modified;
                    context.SaveChanges();
                }

                PouchInwardOutward_Mst objInwardCost2 = new PouchInwardOutward_Mst();
                objInwardCost2.PouchInwardID = 0;
                objInwardCost2.GodownID = data.ToGodownID;

                objInwardCost2.PouchNameID = data.PouchNameID;

                objInwardCost2.PouchID = data.PouchID;
                GetOpeningPouch opneningpouch = GetOpeningPouchByPouchID(data.ToGodownID, data.PouchNameID);
                objInwardCost2.OpeningPouch = opneningpouch.OpeningPouch;
                objInwardCost2.NoofPcs = data.TransferNoofPcs;
                objInwardCost2.TotalPouch = objInwardCost2.OpeningPouch + objInwardCost2.NoofPcs;
                objInwardCost2.PurchaseDate = data.TransferDate;
                objInwardCost2.CreditDebitStatusID = 4;
                objInwardCost2.CreditDebitStatus = "TransferCredit";
                objInwardCost2.CreatedBy = data.CreatedBy;
                objInwardCost2.CreatedOn = data.CreatedOn;
                objInwardCost2.UpdatedBy = data.UpdatedBy;
                objInwardCost2.UpdatedOn = data.UpdatedOn;
                objInwardCost2.IsDelete = data.IsDelete;
                if (objInwardCost2.PouchInwardID == 0)
                {
                    context.PouchInwardOutward_Mst.Add(objInwardCost2);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(objInwardCost2).State = EntityState.Modified;
                    context.SaveChanges();
                }

                if (objInwardCost2.PouchInwardID > 0)
                {
                    PouchTransfer_Mst obj = new PouchTransfer_Mst();
                    if (data.PouchTransferID == 0)
                    {
                        obj.PouchTransferID = data.PouchTransferID;
                        obj.FromGodownID = data.FromGodownID;

                        obj.PouchNameID = data.PouchNameID;

                        obj.OpeningPouch = data.OpeningPouch;
                        obj.TransferNoofPcs = data.TransferNoofPcs;
                        obj.TotalPouch = data.TotalPouch;
                        obj.TransferDate = data.TransferDate;
                        obj.ToGodownID = data.ToGodownID;
                        obj.PouchInwardID = objInwardCost2.PouchInwardID;
                        obj.IsAccept = false;
                        obj.CreatedBy = data.CreatedBy;
                        obj.CreatedOn = data.CreatedOn;
                        obj.UpdatedBy = data.UpdatedBy;
                        obj.UpdatedOn = data.UpdatedOn;
                        obj.IsDelete = data.IsDelete;
                        context.PouchTransfer_Mst.Add(obj);
                        context.SaveChanges();
                    }
                    else
                    {
                        context.Entry(obj).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                    return obj.PouchTransferID;
                }
                else
                {
                    return 0;
                }
            }
        }

        public List<GetOpeningPouch> GetPouchForTransferByGodownID(long GodownID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetPouchForTransferByGodownID";
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<GetOpeningPouch> objlst = new List<GetOpeningPouch>();
                while (dr.Read())
                {
                    GetOpeningPouch obj = new GetOpeningPouch();
                    obj.GodownID = objBaseSqlManager.GetInt64(dr, "GodownID");
                    obj.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    obj.PouchNameID = objBaseSqlManager.GetInt64(dr, "PouchNameID");
                    obj.PouchTransferID = objBaseSqlManager.GetInt64(dr, "PouchTransferID");
                    obj.TransferNoofPcs = objBaseSqlManager.GetInt64(dr, "TransferNoofPcs");
                    obj.PouchName = objBaseSqlManager.GetTextValue(dr, "PouchName");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public bool UpdatePouchTransferAcceptStatusByPouchTransferID(List<GetOpeningPouch> data, long SessionUserID)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                foreach (var item in data)
                {

                    SqlCommand cmdGet = new SqlCommand();
                    using (var objBaseSqlManager = new BaseSqlManager())
                    {
                        cmdGet.CommandType = CommandType.StoredProcedure;
                        cmdGet.CommandText = "UpdatePouchTransferAcceptStatusByPouchTransferID";
                        cmdGet.Parameters.AddWithValue("@PouchTransferID", item.PouchTransferID);
                        cmdGet.Parameters.AddWithValue("@AcceptBy", SessionUserID);
                        cmdGet.Parameters.AddWithValue("@AcceptDate", DateTime.Now);
                        objBaseSqlManager.ExecuteNonQuery(cmdGet);
                        objBaseSqlManager.ForceCloseConnection();
                    }
                }

            }
            return true;
        }

        public List<PouchTransferListResponse> GetAllPouchStockTransferList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllPouchStockTransferList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<PouchTransferListResponse> objlst = new List<PouchTransferListResponse>();
                while (dr.Read())
                {
                    PouchTransferListResponse obj = new PouchTransferListResponse();
                    obj.PouchTransferID = objBaseSqlManager.GetInt64(dr, "PouchTransferID");
                    obj.FromGodownID = objBaseSqlManager.GetInt64(dr, "FromGodownID");
                    obj.FromGodownName = objBaseSqlManager.GetTextValue(dr, "FromGodownName");
                    obj.PouchNameID = objBaseSqlManager.GetInt64(dr, "PouchNameID");
                    obj.PouchName = objBaseSqlManager.GetTextValue(dr, "PouchName");
                    obj.HSNNumber = objBaseSqlManager.GetTextValue(dr, "HSNNumber");
                    obj.OpeningPouch = objBaseSqlManager.GetInt64(dr, "OpeningPouch");
                    obj.TransferNoofPcs = objBaseSqlManager.GetInt64(dr, "TransferNoofPcs");
                    obj.TotalPouch = objBaseSqlManager.GetInt64(dr, "TotalPouch");
                    obj.TransferDate = objBaseSqlManager.GetDateTime(dr, "TransferDate");
                    if (obj.TransferDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.TransferDatestr = obj.TransferDate.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.TransferDatestr = "";
                    }
                    obj.ToGodownID = objBaseSqlManager.GetInt64(dr, "ToGodownID");
                    obj.ToGodownName = objBaseSqlManager.GetTextValue(dr, "ToGodownName");
                    obj.IsAccept = objBaseSqlManager.GetBoolean(dr, "IsAccept");
                    if (obj.IsAccept == true)
                    {
                        obj.Status = "Accepted";
                    }
                    else
                    {
                        obj.Status = "Pending";
                    }
                    obj.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    obj.FullName = objBaseSqlManager.GetTextValue(dr, "FullName");
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");

                    obj.AcceptBy = objBaseSqlManager.GetInt64(dr, "AcceptBy");
                    obj.AcceptedName = objBaseSqlManager.GetTextValue(dr, "AcceptedName");
                    obj.AcceptDate = objBaseSqlManager.GetDateTime(dr, "AcceptDate");

                    if (obj.AcceptDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.AcceptDatestr = obj.AcceptDate.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.AcceptDatestr = "";
                    }
                    obj.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        // Utility Stock 02-03-2020    
        public List<GetOpeningUtility> GetUtilityForTransferByGodownID(long GodownID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetUtilityForTransferByGodownID";
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<GetOpeningUtility> objlst = new List<GetOpeningUtility>();
                while (dr.Read())
                {
                    GetOpeningUtility obj = new GetOpeningUtility();
                    obj.GodownID = objBaseSqlManager.GetInt64(dr, "GodownID");
                    obj.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    obj.UtilityNameID = objBaseSqlManager.GetInt64(dr, "UtilityNameID");
                    obj.UtilityTransferID = objBaseSqlManager.GetInt64(dr, "UtilityTransferID");
                    obj.TransferNoofPcs = objBaseSqlManager.GetInt64(dr, "TransferNoofPcs");
                    obj.UtilityName = objBaseSqlManager.GetTextValue(dr, "UtilityName");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public GetOpeningUtility GetOpeningUtilityByUtilityID(long GodownID, long UtilityNameID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetOpeningUtilityByUtilityID";
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                cmdGet.Parameters.AddWithValue("@UtilityNameID", UtilityNameID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                GetOpeningUtility obj = new GetOpeningUtility();
                while (dr.Read())
                {
                    obj.OpeningUtility = objBaseSqlManager.GetInt64(dr, "OpeningUtility");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        public List<UtilityInwardCost> GetLastUtilityCostByUtilityID(long UtilityNameID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetLastUtilityCostByUtilityID";
                cmdGet.Parameters.AddWithValue("@UtilityNameID", UtilityNameID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<UtilityInwardCost> objlst = new List<UtilityInwardCost>();
                while (dr.Read())
                {
                    UtilityInwardCost obj = new UtilityInwardCost();
                    obj.UtilityCostID = objBaseSqlManager.GetInt64(dr, "UtilityCostID");
                    obj.Description = objBaseSqlManager.GetTextValue(dr, "Description");
                    obj.Amount = objBaseSqlManager.GetDecimal(dr, "Amount");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<UtilityInwardCost> GetUtilityCostByUtilityInwardID(long UtilityInwardID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetUtilityCostByUtilityInwardID";
                cmdGet.Parameters.AddWithValue("@UtilityInwardID", UtilityInwardID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<UtilityInwardCost> objlst = new List<UtilityInwardCost>();
                while (dr.Read())
                {
                    UtilityInwardCost obj = new UtilityInwardCost();
                    obj.UtilityCostID = objBaseSqlManager.GetInt64(dr, "UtilityCostID");
                    obj.Description = objBaseSqlManager.GetTextValue(dr, "Description");
                    obj.Amount = objBaseSqlManager.GetDecimal(dr, "Amount");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public string AddUtilityInward(UtilityInwardOutward data)
        {
            string Message = "";
            long UtilityInwardID = 0;
            UtilityInwardOutward_Mst obj = new UtilityInwardOutward_Mst();
            obj.UtilityInwardID = data.UtilityInwardID;
            obj.GodownID = data.GodownID;

            obj.UtilityNameID = data.UtilityNameID;

            obj.UtilityID = data.UtilityID;
            obj.OpeningUtility = data.OpeningUtility;
            obj.NoofPcs = data.NoofPcs;
            obj.TotalUtility = data.TotalUtility;
            obj.PurchaseDate = data.PurchaseDate;
            obj.SupplierID = data.SupplierID;
            obj.Identification = data.Identification;
            obj.InvoiceNumber = data.InvoiceNumber;
            obj.TotalInwardCost = data.TotalInwardCost;
            obj.CreditDebitStatusID = data.CreditDebitStatusID;
            obj.CreditDebitStatus = data.CreditDebitStatus;
            obj.CreatedBy = data.CreatedBy;
            obj.CreatedOn = data.CreatedOn;
            obj.UpdatedBy = data.UpdatedBy;
            obj.UpdatedOn = data.UpdatedOn;
            obj.IsDelete = data.IsDelete;
            using (VirakiEntities context = new VirakiEntities())
            {
                if (obj.UtilityInwardID == 0)
                {
                    UtilityInwardID = CheckUtilityBillNumberForSupplierIsExist(data.SupplierID, data.InvoiceNumber);
                    if (UtilityInwardID == 0)
                    {
                        context.UtilityInwardOutward_Mst.Add(obj);
                        context.SaveChanges();
                        Message = "Insert Sucessfully";
                    }
                    else
                    {
                        Message = "Bill Exist";
                    }
                }
                else
                {
                    context.Entry(obj).State = EntityState.Modified;
                    context.SaveChanges();
                    Message = "Updated Sucessfully";
                }
                if (obj.UtilityInwardID > 0)
                {
                    try
                    {
                        if (data.lstUtilityInwardCost != null)
                        {
                            foreach (var item in data.lstUtilityInwardCost)
                            {
                                UtilityCost_Mst objInwardCost = new UtilityCost_Mst();
                                objInwardCost.UtilityCostID = item.UtilityCostID;
                                objInwardCost.UtilityInwardID = obj.UtilityInwardID;
                                objInwardCost.Description = item.Description;
                                objInwardCost.Amount = item.Amount;
                                objInwardCost.CreatedBy = data.CreatedBy;
                                objInwardCost.CreatedOn = data.CreatedOn;
                                objInwardCost.UpdatedBy = data.UpdatedBy;
                                objInwardCost.UpdatedOn = data.UpdatedOn;
                                objInwardCost.IsDelete = data.IsDelete;
                                if (objInwardCost.UtilityCostID == 0)
                                {
                                    context.UtilityCost_Mst.Add(objInwardCost);
                                    context.SaveChanges();
                                }
                                else
                                {
                                    context.Entry(objInwardCost).State = EntityState.Modified;
                                    context.SaveChanges();
                                }
                            }
                        }
                        return Message;
                    }
                    catch
                    {
                        using (VirakiEntities context1 = new VirakiEntities())
                        {
                            UtilityInwardOutward_Mst data1 = context.UtilityInwardOutward_Mst.Where(x => x.UtilityInwardID == obj.UtilityInwardID).FirstOrDefault();
                            if (data != null)
                            {
                                context.UtilityInwardOutward_Mst.Remove(data1);
                                context.SaveChanges();
                                Message = "Error";
                                return Message;
                            }
                            else
                            {
                                Message = "Error";
                                return Message;
                            }
                        }
                    }
                }
                else
                {
                    if (UtilityInwardID > 0)
                    {
                        Message = "Bill Exist";
                    }
                    else
                    {
                        Message = "Error";
                    }
                    return Message;
                }
            }
        }

        public long CheckUtilityBillNumberForSupplierIsExist(long SupplierID, string InvoiceNumber)
        {
            long UtilityInwardID = 0;
            try
            {
                SqlCommand cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "CheckUtilityBillNumberForSupplierIsExist";
                    cmdGet.Parameters.AddWithValue("@SupplierID", SupplierID);
                    cmdGet.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                    SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                    while (dr.Read())
                    {
                        UtilityInwardID = objBaseSqlManager.GetInt64(dr, "UtilityInwardID");
                    }
                    dr.Close();
                    objBaseSqlManager.ForceCloseConnection();
                }
            }
            catch
            {
                UtilityInwardID = 0;
            }
            return UtilityInwardID;
        }

        public List<UtilityInwardListResponse> GetAllUtilityInwardList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllUtilityInwardList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<UtilityInwardListResponse> objlst = new List<UtilityInwardListResponse>();
                while (dr.Read())
                {
                    UtilityInwardListResponse obj = new UtilityInwardListResponse();
                    obj.UtilityInwardID = objBaseSqlManager.GetInt64(dr, "UtilityInwardID");
                    obj.GodownID = objBaseSqlManager.GetInt64(dr, "GodownID");
                    obj.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    obj.UtilityID = objBaseSqlManager.GetInt64(dr, "UtilityID");
                    obj.UtilityNameID = objBaseSqlManager.GetInt64(dr, "UtilityNameID");
                    obj.UtilityName = objBaseSqlManager.GetTextValue(dr, "UtilityName");
                    obj.HSNNumber = objBaseSqlManager.GetTextValue(dr, "HSNNumber");
                    obj.OpeningUtility = objBaseSqlManager.GetInt64(dr, "OpeningUtility");
                    obj.NoofPcs = objBaseSqlManager.GetInt64(dr, "NoofPcs");
                    obj.TotalUtility = objBaseSqlManager.GetInt64(dr, "TotalUtility");
                    obj.PurchaseDate = objBaseSqlManager.GetDateTime(dr, "PurchaseDate");
                    if (obj.PurchaseDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.PurchaseDatestr = obj.PurchaseDate.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.PurchaseDatestr = "";
                    }
                    obj.SupplierID = objBaseSqlManager.GetTextValue(dr, "SupplierID");

                    if (obj.SupplierID != "" && obj.SupplierID != null)
                    {
                        string[] array = obj.SupplierID.Split(',');
                        long SupplierID = Convert.ToInt64(array[0]);
                        string Identification = array[1];

                        if (Identification == "Expense")
                        {
                            string SupplierName = GetExpenseSupplierNameBySupplierID(SupplierID);
                            obj.SupplierName = SupplierName;
                        }
                        else
                        {
                            string SupplierName = GetPurchaseSupplierNameBySupplierID(SupplierID);
                            obj.SupplierName = SupplierName;
                        }
                    }
                    else
                    {
                        obj.SupplierName = "Transfer";
                    }
                    obj.InvoiceNumber = objBaseSqlManager.GetTextValue(dr, "InvoiceNumber");
                    obj.TotalInwardCost = Math.Round(objBaseSqlManager.GetDecimal(dr, "TotalInwardCost"), 2);
                    obj.CreditDebitStatusID = objBaseSqlManager.GetInt32(dr, "CreditDebitStatusID");
                    obj.CreditDebitStatus = objBaseSqlManager.GetTextValue(dr, "CreditDebitStatusID");
                    obj.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    obj.FullName = objBaseSqlManager.GetTextValue(dr, "FullName");
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    obj.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public long AddUtilityOutward(UtilityInwardOutward data)
        {
            UtilityInwardOutward_Mst obj = new UtilityInwardOutward_Mst();
            obj.UtilityInwardID = data.UtilityInwardID;
            obj.GodownID = data.GodownID;

            obj.UtilityNameID = data.UtilityNameID;

            obj.UtilityID = data.UtilityID;
            obj.OpeningUtility = data.OpeningUtility;
            obj.NoofPcs = data.NoofPcs;
            obj.TotalUtility = data.TotalUtility;
            obj.PurchaseDate = data.PurchaseDate;
            obj.CreditDebitStatusID = data.CreditDebitStatusID;
            obj.CreditDebitStatus = data.CreditDebitStatus;
            obj.CreatedBy = data.CreatedBy;
            obj.CreatedOn = data.CreatedOn;
            obj.UpdatedBy = data.UpdatedBy;
            obj.UpdatedOn = data.UpdatedOn;
            obj.IsDelete = data.IsDelete;
            using (VirakiEntities context = new VirakiEntities())
            {
                if (obj.UtilityInwardID == 0)
                {
                    context.UtilityInwardOutward_Mst.Add(obj);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(obj).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (obj.UtilityInwardID > 0)
                {
                    return obj.UtilityInwardID;
                }
                else
                {
                    return 0;
                }
            }
        }

        public List<UtilityOutwardListResponse> GetAllUtilityOutwardList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllUtilityOutwardList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<UtilityOutwardListResponse> objlst = new List<UtilityOutwardListResponse>();
                while (dr.Read())
                {
                    UtilityOutwardListResponse obj = new UtilityOutwardListResponse();
                    obj.UtilityInwardID = objBaseSqlManager.GetInt64(dr, "UtilityInwardID");
                    obj.GodownID = objBaseSqlManager.GetInt64(dr, "GodownID");
                    obj.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    obj.UtilityNameID = objBaseSqlManager.GetInt64(dr, "UtilityNameID");
                    obj.UtilityID = objBaseSqlManager.GetInt64(dr, "UtilityID");
                    obj.UtilityName = objBaseSqlManager.GetTextValue(dr, "UtilityName");
                    obj.HSNNumber = objBaseSqlManager.GetTextValue(dr, "HSNNumber");
                    obj.OpeningUtility = objBaseSqlManager.GetInt64(dr, "OpeningUtility");
                    obj.NoofPcs = objBaseSqlManager.GetInt64(dr, "NoofPcs");
                    obj.TotalUtility = objBaseSqlManager.GetInt64(dr, "TotalUtility");
                    obj.PurchaseDate = objBaseSqlManager.GetDateTime(dr, "PurchaseDate");
                    if (obj.PurchaseDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.PurchaseDatestr = obj.PurchaseDate.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.PurchaseDatestr = "";
                    }
                    obj.CreditDebitStatusID = objBaseSqlManager.GetInt32(dr, "CreditDebitStatusID");
                    obj.CreditDebitStatus = objBaseSqlManager.GetTextValue(dr, "CreditDebitStatusID");
                    obj.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    obj.FullName = objBaseSqlManager.GetTextValue(dr, "FullName");
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    obj.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public long AddNoOfUtilityTransfer(AddNoOfUtilityTransfer data)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                UtilityInwardOutward_Mst objInward = new UtilityInwardOutward_Mst();
                objInward.UtilityInwardID = 0;
                objInward.GodownID = data.FromGodownID;

                objInward.UtilityNameID = data.UtilityNameID;

                objInward.UtilityID = data.UtilityID;
                objInward.OpeningUtility = data.OpeningUtility;
                objInward.NoofPcs = data.TransferNoofPcs;
                objInward.TotalUtility = data.TotalUtility;
                objInward.PurchaseDate = data.TransferDate;
                objInward.CreditDebitStatusID = 3;
                objInward.CreditDebitStatus = "TransferDebit";
                objInward.CreatedBy = data.CreatedBy;
                objInward.CreatedOn = data.CreatedOn;
                objInward.UpdatedBy = data.UpdatedBy;
                objInward.UpdatedOn = data.UpdatedOn;
                objInward.IsDelete = data.IsDelete;
                if (objInward.UtilityInwardID == 0)
                {
                    context.UtilityInwardOutward_Mst.Add(objInward);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(objInward).State = EntityState.Modified;
                    context.SaveChanges();
                }
                UtilityInwardOutward_Mst objInwardCost2 = new UtilityInwardOutward_Mst();
                objInwardCost2.UtilityInwardID = 0;
                objInwardCost2.GodownID = data.ToGodownID;

                objInwardCost2.UtilityNameID = data.UtilityNameID;

                objInwardCost2.UtilityID = data.UtilityID;
                GetOpeningUtility opneningutility = GetOpeningUtilityByUtilityID(data.ToGodownID, data.UtilityNameID);
                objInwardCost2.OpeningUtility = opneningutility.OpeningUtility;
                objInwardCost2.NoofPcs = data.TransferNoofPcs;
                objInwardCost2.TotalUtility = objInwardCost2.OpeningUtility + objInwardCost2.NoofPcs;
                objInwardCost2.PurchaseDate = data.TransferDate;
                objInwardCost2.CreditDebitStatusID = 4;
                objInwardCost2.CreditDebitStatus = "TransferCredit";
                objInwardCost2.CreatedBy = data.CreatedBy;
                objInwardCost2.CreatedOn = data.CreatedOn;
                objInwardCost2.UpdatedBy = data.UpdatedBy;
                objInwardCost2.UpdatedOn = data.UpdatedOn;
                objInwardCost2.IsDelete = data.IsDelete;
                if (objInwardCost2.UtilityInwardID == 0)
                {
                    context.UtilityInwardOutward_Mst.Add(objInwardCost2);
                    context.SaveChanges();
                }
                else
                {
                    context.Entry(objInwardCost2).State = EntityState.Modified;
                    context.SaveChanges();
                }
                if (objInwardCost2.UtilityInwardID > 0)
                {
                    UtilityTransfer_Mst obj = new UtilityTransfer_Mst();
                    if (data.UtilityTransferID == 0)
                    {
                        obj.UtilityTransferID = data.UtilityTransferID;
                        obj.FromGodownID = data.FromGodownID;

                        obj.UtilityNameID = data.UtilityNameID;

                        //  obj.UtilityID = data.UtilityID;
                        obj.OpeningUtility = data.OpeningUtility;
                        obj.TransferNoofPcs = data.TransferNoofPcs;
                        obj.TotalUtility = data.TotalUtility;
                        obj.TransferDate = data.TransferDate;
                        obj.ToGodownID = data.ToGodownID;
                        obj.UtilityInwardID = objInwardCost2.UtilityInwardID;
                        obj.IsAccept = false;
                        obj.CreatedBy = data.CreatedBy;
                        obj.CreatedOn = data.CreatedOn;
                        obj.UpdatedBy = data.UpdatedBy;
                        obj.UpdatedOn = data.UpdatedOn;
                        obj.IsDelete = data.IsDelete;
                        context.UtilityTransfer_Mst.Add(obj);
                        context.SaveChanges();
                    }
                    else
                    {
                        context.Entry(obj).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                    return obj.UtilityTransferID;
                }
                else
                {
                    return 0;
                }
            }
        }

        public bool UpdateUtilityTransferAcceptStatusByUtilityTransferID(List<GetOpeningUtility> data, long SessionUserID)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                foreach (var item in data)
                {
                    SqlCommand cmdGet = new SqlCommand();
                    using (var objBaseSqlManager = new BaseSqlManager())
                    {
                        cmdGet.CommandType = CommandType.StoredProcedure;
                        cmdGet.CommandText = "UpdateUtilityTransferAcceptStatusByUtilityTransferID";
                        cmdGet.Parameters.AddWithValue("@UtilityTransferID", item.UtilityTransferID);
                        cmdGet.Parameters.AddWithValue("@AcceptBy", SessionUserID);
                        cmdGet.Parameters.AddWithValue("@AcceptDate", DateTime.Now);
                        objBaseSqlManager.ExecuteNonQuery(cmdGet);
                        objBaseSqlManager.ForceCloseConnection();
                    }
                }

            }
            return true;
        }

        public List<UtilityTransferListResponse> GetAllUtilityStockTransferList()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllUtilityStockTransferList";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<UtilityTransferListResponse> objlst = new List<UtilityTransferListResponse>();
                while (dr.Read())
                {
                    UtilityTransferListResponse obj = new UtilityTransferListResponse();
                    obj.UtilityTransferID = objBaseSqlManager.GetInt64(dr, "UtilityTransferID");
                    obj.FromGodownID = objBaseSqlManager.GetInt64(dr, "FromGodownID");
                    obj.FromGodownName = objBaseSqlManager.GetTextValue(dr, "FromGodownName");
                    obj.UtilityNameID = objBaseSqlManager.GetInt64(dr, "UtilityNameID");
                    obj.UtilityName = objBaseSqlManager.GetTextValue(dr, "UtilityName");
                    obj.HSNNumber = objBaseSqlManager.GetTextValue(dr, "HSNNumber");
                    obj.OpeningUtility = objBaseSqlManager.GetInt64(dr, "OpeningUtility");
                    obj.TransferNoofPcs = objBaseSqlManager.GetInt64(dr, "TransferNoofPcs");
                    obj.TotalUtility = objBaseSqlManager.GetInt64(dr, "TotalUtility");
                    obj.TransferDate = objBaseSqlManager.GetDateTime(dr, "TransferDate");
                    if (obj.TransferDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.TransferDatestr = obj.TransferDate.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.TransferDatestr = "";
                    }
                    obj.ToGodownID = objBaseSqlManager.GetInt64(dr, "ToGodownID");
                    obj.ToGodownName = objBaseSqlManager.GetTextValue(dr, "ToGodownName");
                    obj.IsAccept = objBaseSqlManager.GetBoolean(dr, "IsAccept");
                    if (obj.IsAccept == true)
                    {
                        obj.Status = "Accepted";
                    }
                    else
                    {
                        obj.Status = "Pending";
                    }
                    obj.CreatedBy = objBaseSqlManager.GetInt64(dr, "CreatedBy");
                    obj.FullName = objBaseSqlManager.GetTextValue(dr, "FullName");
                    obj.CreatedOn = objBaseSqlManager.GetDateTime(dr, "CreatedOn");
                    obj.AcceptBy = objBaseSqlManager.GetInt64(dr, "AcceptBy");
                    obj.AcceptedName = objBaseSqlManager.GetTextValue(dr, "AcceptedName");
                    obj.AcceptDate = objBaseSqlManager.GetDateTime(dr, "AcceptDate");
                    if (obj.AcceptDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.AcceptDatestr = obj.AcceptDate.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        obj.AcceptDatestr = "";
                    }
                    obj.IsDelete = objBaseSqlManager.GetBoolean(dr, "IsDelete");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public string GetExpenseSupplierNameBySupplierID(long SupplierID)
        {
            string SupplierName = "";
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetExpenseSupplierNameBySupplierID";
                cmdGet.Parameters.AddWithValue("@SupplierID", SupplierID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    SupplierName = objBaseSqlManager.GetTextValue(dr, "SupplierName");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
            }
            return SupplierName;
        }

        public string GetPurchaseSupplierNameBySupplierID(long SupplierID)
        {
            string SupplierName = "";
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetPurchaseSupplierNameBySupplierID";
                cmdGet.Parameters.AddWithValue("@SupplierID", SupplierID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                while (dr.Read())
                {
                    SupplierName = objBaseSqlManager.GetTextValue(dr, "SupplierName");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
            }
            return SupplierName;
        }

        // 27 Aug 2020 Piyush Limbani
        public GetVehicleExpensesInwardAmount GetVehicleExpensesInwardDetail(long VehicleDetailID, DateTime AssignedDate, long GodownID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetVehicleExpensesInwardDetail";
                cmdGet.Parameters.AddWithValue("@VehicleDetailID", VehicleDetailID);
                cmdGet.Parameters.AddWithValue("@AssignedDate", AssignedDate);
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                GetVehicleExpensesInwardAmount obj = new GetVehicleExpensesInwardAmount();
                while (dr.Read())
                {
                    obj.Amount = objBaseSqlManager.GetDecimal(dr, "Amount");
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return obj;
            }
        }

        // 09 Sep 2020 Piyush Limbani
        public List<WCustomerNameList> GetAllWholesaleCustomerNameForVoucher()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllWholesaleCustomerNameForVoucher";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<WCustomerNameList> lst = new List<WCustomerNameList>();
                while (dr.Read())
                {
                    WCustomerNameList obj = new WCustomerNameList();
                    obj.WCustomerID = objBaseSqlManager.GetInt64(dr, "WCustomerID");
                    obj.WCustomerName = objBaseSqlManager.GetTextValue(dr, "WCustomerName");
                    lst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lst;
            }
        }

        public List<RCustomerNameList> GetAllRetailCustomerNameForVoucher()
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetAllRetailCustomerNameForVoucher";
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<RCustomerNameList> lst = new List<RCustomerNameList>();
                while (dr.Read())
                {
                    RCustomerNameList obj = new RCustomerNameList();
                    obj.RCustomerID = objBaseSqlManager.GetInt64(dr, "RCustomerID");
                    obj.RCustomerName = objBaseSqlManager.GetTextValue(dr, "RCustomerName");
                    lst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return lst;
            }
        }

        // 16 Sep 2020 Piyush Limbani
        public List<UtilityStockResponse> GetUtilityStockReport(long? GodownID, long? UtilityNameID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetUtilityStockReport";
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                cmdGet.Parameters.AddWithValue("@UtilityNameID", UtilityNameID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<UtilityStockResponse> objlst = new List<UtilityStockResponse>();
                while (dr.Read())
                {
                    UtilityStockResponse obj = new UtilityStockResponse();
                    obj.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    obj.UtilityName = objBaseSqlManager.GetTextValue(dr, "UtilityName");
                    obj.HSNNumber = objBaseSqlManager.GetTextValue(dr, "HSNNumber");
                    obj.MinUtility = objBaseSqlManager.GetInt64(dr, "MinUtility");
                    obj.TotalUtility = objBaseSqlManager.GetInt64(dr, "TotalUtility");
                    obj.PurchaseDate = objBaseSqlManager.GetDateTime(dr, "PurchaseDate");
                    if (obj.PurchaseDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.PurchaseDatestr = obj.PurchaseDate.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        obj.PurchaseDatestr = "";
                    }
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        public List<PouchStockResponse> GetPouchStockReport(long? GodownID, long? PouchNameID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetPouchStockReport";
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                cmdGet.Parameters.AddWithValue("@PouchNameID", PouchNameID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<PouchStockResponse> objlst = new List<PouchStockResponse>();
                while (dr.Read())
                {
                    PouchStockResponse obj = new PouchStockResponse();
                    obj.GodownName = objBaseSqlManager.GetTextValue(dr, "GodownName");
                    obj.PouchName = objBaseSqlManager.GetTextValue(dr, "PouchName");
                    obj.HSNNumber = objBaseSqlManager.GetTextValue(dr, "HSNNumber");
                    obj.MinPouchQuantity = objBaseSqlManager.GetInt64(dr, "MinPouchQuantity");
                    obj.TotalPouch = objBaseSqlManager.GetInt64(dr, "TotalPouch");
                    obj.PurchaseDate = objBaseSqlManager.GetDateTime(dr, "PurchaseDate");
                    if (obj.PurchaseDate != Convert.ToDateTime("10/10/2014"))
                    {
                        obj.PurchaseDatestr = obj.PurchaseDate.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        obj.PurchaseDatestr = "";
                    }
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        // 05 Oct 2020 Piyush Limbani
        public List<UtilityListResponse> GetUtilityByGodownID(long GodownID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetUtilityByGodownID";
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<UtilityListResponse> objlst = new List<UtilityListResponse>();
                while (dr.Read())
                {
                    UtilityListResponse obj = new UtilityListResponse();
                    obj.UtilityNameID = objBaseSqlManager.GetInt64(dr, "UtilityNameID");
                    obj.UtilityName = objBaseSqlManager.GetTextValue(dr, "UtilityName");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        // 08 Oct 2020 Piyush Limbani
        public List<PouchListResponse> GetPouchByGodownID(long GodownID)
        {
            SqlCommand cmdGet = new SqlCommand();
            using (var objBaseSqlManager = new BaseSqlManager())
            {
                cmdGet.CommandType = CommandType.StoredProcedure;
                cmdGet.CommandText = "GetPouchByGodownID";
                cmdGet.Parameters.AddWithValue("@GodownID", GodownID);
                SqlDataReader dr = objBaseSqlManager.ExecuteDataReader(cmdGet);
                List<PouchListResponse> objlst = new List<PouchListResponse>();
                while (dr.Read())
                {
                    PouchListResponse obj = new PouchListResponse();
                    obj.PouchNameID = objBaseSqlManager.GetInt64(dr, "PouchNameID");
                    obj.PouchName = objBaseSqlManager.GetTextValue(dr, "PouchName");
                    objlst.Add(obj);
                }
                dr.Close();
                objBaseSqlManager.ForceCloseConnection();
                return objlst;
            }
        }

        //  Jan 21, 2021 Piyush Limbani
        public bool UpdateTransferDetail(TransferPaymentModel data, long UserID)
        {
            using (VirakiEntities context = new VirakiEntities())
            {
                SqlCommand cmdGet = new SqlCommand();
                using (var objBaseSqlManager = new BaseSqlManager())
                {
                    cmdGet.CommandType = CommandType.StoredProcedure;
                    cmdGet.CommandText = "UpdateTransferDetail";
                    cmdGet.Parameters.AddWithValue("@TransferID", data.TransferID);
                    cmdGet.Parameters.AddWithValue("@FromGodownID", data.FromGodownID);
                    cmdGet.Parameters.AddWithValue("@ToGodownID", data.ToGodownID);
                    cmdGet.Parameters.AddWithValue("@Amount", data.Amount);
                    cmdGet.Parameters.AddWithValue("@UpdatedBy", UserID);
                    cmdGet.Parameters.AddWithValue("@UpdatedOn", DateTime.Now);
                    objBaseSqlManager.ExecuteNonQuery(cmdGet);
                    objBaseSqlManager.ForceCloseConnection();
                }
            }
            return true;
        }

        // 22 Oct 2020 Piyush Limbani For Delete Vehicle Cost Uncommented on 01 Feb 2022
        //public bool DeleteVehicleInwardCost(long? VehicleInwardCostID, long? VehicleDetailID, DateTime? AssignedDate, long UserID)
        //{
        //    using (VirakiEntities context = new VirakiEntities())
        //    {
        //        SqlCommand cmdGet = new SqlCommand();
        //        BaseSqlManager objBaseSqlManager = new BaseSqlManager();
        //        cmdGet.CommandType = CommandType.StoredProcedure;
        //        cmdGet.CommandText = "DeleteVehicleInwardCost";
        //        cmdGet.Parameters.AddWithValue("@VehicleInwardCostID", VehicleInwardCostID);
        //        cmdGet.Parameters.AddWithValue("@UpdatedBy", UserID);
        //        cmdGet.Parameters.AddWithValue("@UpdatedOn", DateTime.Now);
        //        cmdGet.Parameters.AddWithValue("@IsDelete", true);
        //        //cmdGet.Parameters.AddWithValue("@VehicleDetailID", VehicleDetailID);
        //        //cmdGet.Parameters.AddWithValue("@AssignedDate", AssignedDate);
        //        //cmdGet.Parameters.AddWithValue("@IsVehicleStatus", false); 
        //        objBaseSqlManager.ExecuteNonQuery(cmdGet);
        //        objBaseSqlManager.ForceCloseConnection();
        //    }
        //    return true;
        //}

    }
}
