using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vb.Data;

namespace vb.Service
{
    public interface ISupplierService
    {
        bool AddSupplier(AddSupplier data);

        List<SupplierListResponse> GetAllSupplierList();

        List<SupplierContactDetail> GetSupplierAddressListBySupplierID(long SupplierID);

        bool DeleteSupplier(long SupplierID, bool IsDelete);

        List<SupplierListResponse> GetAllSupplierFSSAIExpireList();

        bool UpdateSupplierFSSAIDate(long SupplierID, DateTime FSSAIValidUpTo);

        List<SupplierListResponse> GetAllSupplierName();

        // Expense supplier 18-12-2019
        bool AddExpenseSupplier(AddExpenseSupplier data);

        List<ExpenseSupplierListResponse> GetAllExpenseSupplierList();

        List<ExpenseSupplierContactDetail> GetExpenseSupplierAddressListBySupplierID(long SupplierID);

        bool DeleteExpenseSupplier(long SupplierID, bool IsDelete);

        List<SupplierListResponse> GetAllExpenseSupplierName();

        // 18 Aug 2020 Piyush Limbani
        List<AllSupplierName> GetAllPurchaseAndExpenseSupplierName();
    }
}
