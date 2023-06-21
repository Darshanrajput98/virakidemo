

namespace vb.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class RetAreaViewModel
    {
        public long AreaID { get; set; }
        public string AreaName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PinCode { get; set; }
        public int DaysofWeek { get; set; }
    }

    public class RetAreaListResponse
    {
        public long AreaID { get; set; }
        public string AreaName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PinCode { get; set; }
        public int DaysofWeek { get; set; }
        public string DaysofWeekstr { get; set; }
        public Boolean IsDelete { get; set; }       
    }

    public enum RetWeekdays
    {
        [Description("Monday")]
        Monday = 1,
        [Description("Tuesday")]
        Tuesday = 2,
        [Description("Wednesday")]
        Wednesday = 3,
        [Description("Thursday")]
        Thursday = 4,
        [Description("Friday")]
        Friday = 5,
        [Description("Saturday")]
        Saturday = 6,
        [Description("Sunday")]
        Sunday = 7
    }

    public class RetGodownViewModel
    {
        public long GodownID { get; set; }
        public string GodownName { get; set; }
        public string GodownPhone { get; set; }
        public string GodownAddress1 { get; set; }
        public string GodownAddress2 { get; set; }
        public string GodownFSSAINumber { get; set; }
        public string GodownCode { get; set; }
        public string GodownNote { get; set; }
        public string Place { get; set; }
        public string Pincode { get; set; }
        public string State { get; set; }
        public string CashOption { get; set; }
    }

    public class RetGodownListResponse
    {
        public long GodownID { get; set; }
        public string GodownName { get; set; }
        public string GodownPhone { get; set; }
        public string GodownAddress1 { get; set; }
        public string GodownAddress2 { get; set; }
        public string GodownFSSAINumber { get; set; }
        public string GodownCode { get; set; }
        public string GodownNote { get; set; }
        public string Place { get; set; }
        public string Pincode { get; set; }
        public string State { get; set; }
        public string CashOption { get; set; }
        public Boolean IsDelete { get; set; }
    }

    public class RetTaxViewModel
    {
        public long TaxID { get; set; }
        public string TaxCode { get; set; }
        public string TaxName { get; set; }
        public string TaxDescription { get; set; }
    }

    public class RetTaxListResponse
    {
        public long TaxID { get; set; }
        public string TaxCode { get; set; }
        public string TaxName { get; set; }
        public string TaxDescription { get; set; }
        public Boolean IsDelete { get; set; }
    }

    public class RetUnitViewModel
    {
        public long UnitID { get; set; }
        public long GuiID { get; set; }
        public string UnitCode { get; set; }
        public string UnitName { get; set; }
        public string UnitDescription { get; set; }
    }

    public class RetUnitListResponse
    {
        public long UnitID { get; set; }
        public string UnitCode { get; set; }
        public string UnitName { get; set; }
        public string UnitDescription { get; set; }
        public string LanguageName { get; set; }
        public long GuiID { get; set; }
        public Boolean IsDelete { get; set; }
    }

    public class RetRoleViewModel
    {
        public long RoleID { get; set; }
        public string RoleName { get; set; }
        public Boolean IsDelete { get; set; }
    }

    public class RetRoleListResponse
    {
        public long RoleID { get; set; }
        public string RoleName { get; set; }
        public Boolean IsDelete { get; set; }
    }

    public class RetUsersViewModel
    {
        public long UserID { get; set; }
        public string UserFullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public long RoleID { get; set; }
        public string UserCode { get; set; }
        public string UserEmail { get; set; }
        public string UserMobile { get; set; }
        public string UserPhone { get; set; }
        public string UserPhoneExtn { get; set; }
        public string UserDesignation { get; set; }
        public string UserDepartment { get; set; }
        public string UserBranch { get; set; }
        public string UserLocation { get; set; }
        public string UserRemark { get; set; }
    }

    public class RetUserListResponse
    {
        public long UserID { get; set; }
        public string UserFullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public long RoleID { get; set; }
        public string RoleName { get; set; }
        public string UserCode { get; set; }
        public string UserEmail { get; set; }
        public string UserMobile { get; set; }
        public string UserPhone { get; set; }
        public string UserPhoneExtn { get; set; }
        public string UserDesignation { get; set; }
        public string UserDepartment { get; set; }
        public string UserBranch { get; set; }
        public string UserLocation { get; set; }
        public string UserRemark { get; set; }
        public Boolean IsDelete { get; set; }
    }

    public class RetMonthListResponse
    {
        public long BestBeforeMonthID { get; set; }
        public string MonthNumber { get; set; }
    }



    public class RetTransportViewModel
    {
        public long TransportID { get; set; }
        public string TransID { get; set; }
        public string TransportName { get; set; }
        public string TransportGSTNumber { get; set; }
        public string ContactNumber { get; set; }
    }

    public class RetTransportListResponse
    {
        public long TransportID { get; set; }
        public string TransID { get; set; }
        public string TransportName { get; set; }
        public string TransportGSTNumber { get; set; }
        public string ContactNumber { get; set; }
        public Boolean IsDelete { get; set; }
    }

}
