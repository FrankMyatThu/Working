//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ngNetCore.Model.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_User()
        {
            this.tbl_LoggedInUser = new HashSet<tbl_LoggedInUser>();
        }
    
        public System.Guid Id { get; set; }
        public string UserName { get; set; }
        public string SaltKey { get; set; }
        public string PasswordHashed { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<System.Guid> UpdatedBy { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_LoggedInUser> tbl_LoggedInUser { get; set; }
    }
}
