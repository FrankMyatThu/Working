using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SG50.TokenService.Models.POCO
{
    public class ActiveUser
    {   
        [Key]        
        public Guid Id { get; set; }

        [ForeignKey("ApplicationUser")]        
        public string AppUserId { get; set; }

        public string IP { get; set; }
        public string UserAgent { get; set; }
        public string JwtHMACKey { get; set; }

        [Timestamp]
        public Byte[] ExecutedTime { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime LastRequestedTime { get; set; }

        [Required]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}