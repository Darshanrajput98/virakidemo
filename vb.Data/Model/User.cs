//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace vb.Data.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public User()
        {
            this.webpages_Roles = new HashSet<webpages_Roles>();
        }
    
        public int Id { get; set; }
        public Nullable<long> EmployeeCode { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Nullable<long> RoleID { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public Nullable<System.DateTime> DateOfJoining { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string MobileNumber { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string Branch { get; set; }
        public string IFSCCode { get; set; }
        public Nullable<long> PrimaryArea { get; set; }
        public string PrimaryAddress { get; set; }
        public Nullable<long> PrimaryPin { get; set; }
        public Nullable<long> SecondaryArea { get; set; }
        public string SecondaryAddress { get; set; }
        public Nullable<long> SecondaryPin { get; set; }
        public string PanNo { get; set; }
        public string PassportNo { get; set; }
        public Nullable<System.DateTime> PassportValiddate { get; set; }
        public Nullable<long> UIDAI { get; set; }
        public Nullable<long> UAN { get; set; }
        public string PF { get; set; }
        public string ESIC { get; set; }
        public string Drivinglicence { get; set; }
        public Nullable<System.DateTime> DrivingValidup { get; set; }
        public string ReferenceName { get; set; }
        public string FName { get; set; }
        public Nullable<System.DateTime> Fdob { get; set; }
        public Nullable<long> FUIDAI { get; set; }
        public string FRelation { get; set; }
        public string Flivingtogether { get; set; }
        public string MName { get; set; }
        public Nullable<System.DateTime> Mdob { get; set; }
        public Nullable<long> MUIDAI { get; set; }
        public string MRelation { get; set; }
        public string Mlivingtogether { get; set; }
        public string WName { get; set; }
        public Nullable<System.DateTime> Wdob { get; set; }
        public Nullable<long> WUIDAI { get; set; }
        public string WRelation { get; set; }
        public string Wlivingtogether { get; set; }
        public string C1Name { get; set; }
        public Nullable<System.DateTime> C1dob { get; set; }
        public Nullable<long> C1UIDAI { get; set; }
        public string C1Relation { get; set; }
        public string C1livingtogether { get; set; }
        public string C2Name { get; set; }
        public Nullable<System.DateTime> C2dob { get; set; }
        public Nullable<long> C2UIDAI { get; set; }
        public string C2Relation { get; set; }
        public string C2livingtogether { get; set; }
        public string C3Name { get; set; }
        public Nullable<System.DateTime> C3dob { get; set; }
        public Nullable<long> C3UIDAI { get; set; }
        public string C3Relation { get; set; }
        public string C3livingtogether { get; set; }
        public Nullable<long> GodownID { get; set; }
        public string ServiceTime { get; set; }
        public string Maritalstatus { get; set; }
        public string ProfilePicture { get; set; }
        public string FSSAIDoctorCertificate { get; set; }
        public Nullable<System.DateTime> FSSAIDoctorCertificateValidity { get; set; }
        public Nullable<System.DateTime> DateofLeaving { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    
        public virtual ICollection<webpages_Roles> webpages_Roles { get; set; }
    }
}
