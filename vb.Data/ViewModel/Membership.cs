using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace vb.Data
{

    public class AssignRoleVM
    {
        public string RoleName { get; set; }
        public string UserName { get; set; }
        public List<SelectListItem> Userlist { get; set; }
        public List<SelectListItem> RolesList { get; set; }
    }

    public class AllroleandUser
    {
        public string RoleName { get; set; }
        public string UserName { get; set; }
        public List<AllroleandUser> AllDetailsUserlist { get; set; }
    }

    public class Login
    {
        public string password { get; set; }
        public string username { get; set; }
    }

    public class Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }

    public class LoginResponse
    {
        public int UserID { get; set; }
        public long RoleID { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string GodownName { get; set; }
        public string ProfilePicture { get; set; }
        public string ProfilePicturePath { get; set; }
        public long GodownID { get; set; }
    }

    public class Register
    {
        //public long UserID { get; set; }
        //public string UserFullName { get; set; }
        //public string UserName { get; set; }
        //public string Password { get; set; }
        //public string ConfirmPassword { get; set; }
        //public long RoleID { get; set; }
        //public string RoleName { get; set; }
        //public string UserCode { get; set; }
        //public string UserEmail { get; set; }
        //public string UserMobile { get; set; }
        //public string UserPhone { get; set; }
        //public string UserPhoneExtn { get; set; }
        //public string UserDesignation { get; set; }
        //public string UserDepartment { get; set; }
        //public long GodownID { get; set; }
        //public string UserLocation { get; set; }
        //public string UserRemark { get; set; }
        //public long CreatedBy { get; set; }
        //public DateTime CreatedOn { get; set; }
        //public long UpdatedBy { get; set; }
        //public DateTime UpdatedOn { get; set; }
        //public Boolean IsDelete { get; set; }


        //public int UserID { get; set; }
        //public string FullName { get; set; }
        //public long EmployeeCode { get; set; }
        //public string UserName { get; set; }
        //public string Password { get; set; }
        //public DateTime BirthDate { get; set; }
        //public string Age { get; set; }
        //public string Gender { get; set; }
        //public DateTime DateOfJoining { get; set; }

        //public string Email { get; set; }
        //public long RoleID { get; set; }
        //public string RoleName { get; set; }
        //public string Address { get; set; }
        //public string MobileNumber { get; set; }
        //public string BankName { get; set; }
        //public string AccountNumber { get; set; }
        //public string Branch { get; set; }
        //public string IFSCCode { get; set; }
        //public Nullable<long> PrimaryArea { get; set; }
        //public string PrimaryAddress { get; set; }
        //public Nullable<long> PrimaryPin { get; set; }
        //public Nullable<long> SecondaryArea { get; set; }
        //public string SecondaryAddress { get; set; }
        //public Nullable<long> SecondaryPin { get; set; }
        //public string PanNo { get; set; }
        //public string PassportNo { get; set; }
        //public Nullable<System.DateTime> PassportValiddate { get; set; }
        //public Nullable<long> UIDAI { get; set; }
        //public Nullable<long> UAN { get; set; }
        //public string PF { get; set; }
        //public string ESIC { get; set; }
        //public string Drivinglicence { get; set; }
        //public Nullable<System.DateTime> DrivingValidup { get; set; }
        ////  public Nullable<System.DateTime> DrivingValiddate { get; set; }
        //public string ReferenceName { get; set; }
        //public string FName { get; set; }
        //public Nullable<System.DateTime> Fdob { get; set; }
        //public Nullable<long> FUIDAI { get; set; }
        //public string FRelation { get; set; }
        //public string Flivingtogether { get; set; }
        //public string MName { get; set; }
        //public Nullable<System.DateTime> Mdob { get; set; }
        //public Nullable<long> MUIDAI { get; set; }
        //public string MRelation { get; set; }
        //public string Mlivingtogether { get; set; }
        //public string WName { get; set; }
        //public Nullable<System.DateTime> Wdob { get; set; }
        //public Nullable<long> WUIDAI { get; set; }
        //public string WRelation { get; set; }
        //public string Wlivingtogether { get; set; }
        //public string C1Name { get; set; }
        //public Nullable<System.DateTime> C1dob { get; set; }
        //public Nullable<long> C1UIDAI { get; set; }
        //public string C1Relation { get; set; }
        //public string C1livingtogether { get; set; }
        //public string C2Name { get; set; }
        //public Nullable<System.DateTime> C2dob { get; set; }
        //public Nullable<long> C2UIDAI { get; set; }
        //public string C2Relation { get; set; }
        //public string C2livingtogether { get; set; }
        //public string C3Name { get; set; }
        //public Nullable<System.DateTime> C3dob { get; set; }
        //public Nullable<long> C3UIDAI { get; set; }
        //public string C3Relation { get; set; }
        //public string C3livingtogether { get; set; }
        //public Nullable<long> GodownID { get; set; }
        //public string ServiceTime { get; set; }
        //public string Maritalstatus { get; set; }
        //public string ProfilePicture { get; set; }
        //public Nullable<long> CreatedBy { get; set; }
        //public Nullable<System.DateTime> CreatedOn { get; set; }
        //public Nullable<long> UpdatedBy { get; set; }
        //public Nullable<System.DateTime> UpdatedOn { get; set; }
        //public Nullable<bool> IsDelete { get; set; }





        public int UserID { get; set; }
        public int EmployeeCode { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string UserNameUpdate { get; set; }
        public string Password { get; set; }
        public long RoleID { get; set; }
        public string RoleName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string MobileNumber { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string Branch { get; set; }
        public string IFSCCode { get; set; }
        public long PrimaryArea { get; set; }
        public string PrimaryAddress { get; set; }
        public long PrimaryPin { get; set; }
        public long SecondaryArea { get; set; }
        public string SecondaryAddress { get; set; }
        public long SecondaryPin { get; set; }
        public string PanNo { get; set; }
        public string PassportNo { get; set; }
        public DateTime? PassportValiddate { get; set; }
        public long UIDAI { get; set; }
        public long UAN { get; set; }
        public string PF { get; set; }
        public string ESIC { get; set; }
        public string Drivinglicence { get; set; }
        public DateTime? DrivingValidup { get; set; }
        public string ReferenceName { get; set; }
        public string FName { get; set; }
        public DateTime? Fdob { get; set; }
        public long FUIDAI { get; set; }
        public string FRelation { get; set; }
        public string Flivingtogether { get; set; }
        public string MName { get; set; }
        public DateTime? Mdob { get; set; }
        public long MUIDAI { get; set; }
        public string MRelation { get; set; }
        public string Mlivingtogether { get; set; }
        public string WName { get; set; }
        public DateTime? Wdob { get; set; }
        public long WUIDAI { get; set; }
        public string WRelation { get; set; }
        public string Wlivingtogether { get; set; }
        public string C1Name { get; set; }
        public DateTime? C1dob { get; set; }
        public long C1UIDAI { get; set; }
        public string C1Relation { get; set; }
        public string C1livingtogether { get; set; }
        public string C2Name { get; set; }
        public DateTime? C2dob { get; set; }
        public long C2UIDAI { get; set; }
        public string C2Relation { get; set; }
        public string C2livingtogether { get; set; }
        public string C3Name { get; set; }
        public DateTime? C3dob { get; set; }
        public long C3UIDAI { get; set; }
        public string C3Relation { get; set; }
        public string C3livingtogether { get; set; }
        public long GodownID { get; set; }
        public string ServiceTime { get; set; }
        public string Maritalstatus { get; set; }
        public string ProfilePicture { get; set; }


        public DateTime? FSSAIDoctorCertificateValidity { get; set; }
        public string FSSAIDoctorCertificate { get; set; }

        public DateTime? DateofLeaving { get; set; }

        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public Boolean IsDelete { get; set; }


    }

    public class InvoiceTotal
    {
        public long OrderID { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal FinalTotal { get; set; }
    }

    public class RetInvoiceTotal
    {
        public long OrderID { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class MenuList
    {
        public long MenuID { get; set; }      
    }

}
