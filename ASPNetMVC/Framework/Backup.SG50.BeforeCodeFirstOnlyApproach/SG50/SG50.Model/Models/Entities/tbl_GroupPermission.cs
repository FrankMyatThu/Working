//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SG50.Model.Models.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_GroupPermission
    {
        public System.Guid Id { get; set; }
        public System.Guid GroupId { get; set; }
        public System.Guid RoleId { get; set; }
        public byte[] ExecutedTime { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    
        public virtual tbl_Group tbl_Group { get; set; }
        public virtual tbl_Role tbl_Role { get; set; }
    }
}
