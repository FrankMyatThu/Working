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

        [Required]
        public virtual Company Company { get; set; }
    }

    public class GroupPermission
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("Group")]
        public Guid GroupID { get; set; }

        [ForeignKey("Role")]
        public Guid RoleID { get; set; }

        [Timestamp]
        public Byte[] ExecutedTime { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        [Required]
        public virtual Group Group { get; set; }        
        public virtual IList<Role> Role { get; set; }
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
    }

    public class RolePermission
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("Role")]
        public Guid RoleID { get; set; }

        [ForeignKey("ProgramMenu")]
        public Guid ProgramMenuID { get; set; }

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

        [Required]
        public virtual Role Role { get; set; }
        [Required]
        public virtual IList<ProgramMenu> ProgramMenu { get; set; }
    }

    public class ProgramMenu
    {
        [Key]
        public Guid Id { get; set; }
    }
}