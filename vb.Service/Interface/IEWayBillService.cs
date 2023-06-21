using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vb.Data;

namespace vb.Service
{
    public interface IEWayBillService
    {
        List<EwayBillList> GetEWayBill(long OrderID, long GodownID, long TransportID);

        List<EwayBillItemList> GetEWayBillItemList(long OrderID);
    }
}
