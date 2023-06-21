using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vb.Data;
using vb.Data.Model;

namespace vb.Service
{
    public interface IPadtarService
    {
        //Whole Material
        List<GetProductDetailForWholeMaterial> GetAutoCompleteProductDetailsForWholeMaterial(long ProductID);

        long ManageWholeMaterial(WholeMaterial_Mst Obj);

        List<WholeMaterialListResponse> GetAllWholeMaterialList();

        bool DeleteWholeMaterial(long MaterialID, bool IsDelete);

        bool UpdateMaterial(List<WholeMaterialListResponse> data);



        //Powder Spices
        List<GetDetailForPowderSpices> GetAutoCompleteDetailsForPowderSpices(long ProductID);
        
        long ManagePowderSpices(PowderSpices_Mst Obj);

        List<PowderSpicesListResponse> GetAllPowderSpicesList();

        bool UpdateSpices(List<PowderSpicesListResponse> data);

        bool DeletePowderSpices(long SpicesID, bool IsDelete);

        //Premix
        string AddPremix(PremixRequest data);

        List<PremixListResponse> GetAllPremixList();

        List<PremixItemRequest> GetPremixDetailsByPremixID(long PremixID);

        bool DeletePremix(long PremixID, bool IsDelete);

        bool UpdatePremix(List<PremixListResponse> data);
    }
}
