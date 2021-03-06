﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SG50.TokenService.Models.POCO
{
    public class UserUsedPassword
    {
        [Key]        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [ForeignKey("ApplicationUser")]
        public string AppUserId { get; set; }

        public string Password { get; set; }
        public string SaltKey { get; set; }
        [Timestamp]
        public Byte[] ExecutedTime { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}