using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SG50.TokenService.Models.POCO
{
    public class WhiteListIP
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("Company")]        
        public string CompanyId { get; set; }

        [MaxLength(255)]
        public string IPAddress { get; set; }

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
}