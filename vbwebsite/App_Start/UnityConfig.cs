using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc5;
using vb.Data;
using vb.Service;

namespace vbwebsite
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            //  container.RegisterType<ILogin, LoginSevices>();
            container.RegisterType<IAdminService, AdminServices>();
            container.RegisterType<IProductService, ProductService>();
            container.RegisterType<ICustomerService, CustomerServices>();
            container.RegisterType<IOrderService, OrderServices>();
            container.RegisterType<IPaymentService, PaymentServices>();
            container.RegisterType<ICommonService, CommonService>();
            container.RegisterType<IDeliveryService, DeliveryServices>();
            container.RegisterType<IRetAdminService, RetAdminServices>();
            container.RegisterType<IRetProductService, RetProductServices>();
            container.RegisterType<IRetCustomerService, RetCustomerServices>();
            container.RegisterType<IRetOrderService, RetOrderServices>();
            container.RegisterType<IReportService, ReportServices>();
            container.RegisterType<IRetReportService, RetReportServices>();
            container.RegisterType<IRetDeliveryService, RetDeliveryServices>();
            container.RegisterType<IRetPaymentService, RetPaymentServices>();
            container.RegisterType<IAttandanceService, AttandanceServices>();
            container.RegisterType<IExpensesService, ExpensesServices>();
            container.RegisterType<ISupplierService, SupplierServices>();
            container.RegisterType<IPurchaseService, PurchaseServices>();
            container.RegisterType<IColdStorageService, ColdStorageServices>();
            container.RegisterType<IGroundStockService, GroundStockService>();
            container.RegisterType<IPadtarService, PadtarServices>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}