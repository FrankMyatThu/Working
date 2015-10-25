using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SG50.TokenService.Models.POCO
{
    public class Group
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("Company")]
        public Guid CompanyId { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }

        [Timestamp]
        public Byte[] ExecutedTime { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public virtual Company Company { get; set; }
        public IList<GroupPermission> GroupPermission { get; set; }
    }

    public class GroupPermission
    {
        [Key, Column(Order = 1)]
        public Guid Id { get; set; }

        [ForeignKey("Group")]
        public Guid GroupId { get; set; }
        public Group Group { get; set; }

        [ForeignKey("Role")]
        public Guid RoleId { get; set; }
        public Role Role { get; set; }

        [Timestamp]
        public Byte[] ExecutedTime { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }

    public class Role
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }

        [Timestamp]
        public Byte[] ExecutedTime { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public IList<GroupPermission> GroupPermission { get; set; }
        public IList<RolePermission> RolePermission { get; set; }
    }

    public class RolePermission
    {
        [Key, Column(Order = 1)]
        public Guid Id { get; set; }

        [ForeignKey("Role")]
        public Guid RoleId { get; set; }
        public Role Role { get; set; }

        [ForeignKey("ProgramMenu")]
        public Guid ProgramMenuId { get; set; }
        public ProgramMenu ProgramMenu { get; set; }

        public bool IsAllowedCreate { get; set; }
        public bool IsAllowedRetrieve { get; set; }
        public bool IsAllowedUpdate { get; set; }
        public bool IsAllowedDelete { get; set; }

        [Timestamp]
        public Byte[] ExecutedTime { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

    }

    public class ProgramMenu
    {
        [Key]
        public Guid Id { get; set; }

        public IList<RolePermission> RolePermission { get; set; }
    }
}