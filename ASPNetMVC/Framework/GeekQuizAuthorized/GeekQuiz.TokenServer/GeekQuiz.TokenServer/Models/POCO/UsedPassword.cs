using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GeekQuiz.TokenServer.Models.POCO
{
    public class UsedPassword
    {
        [Key, Column(Order = 0)]
        public string HashPassword { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        [Key, Column(Order = 1)]
        public string UserID { get; set; }
        public virtual ApplicationUser AppUser { get; set; }

        public UsedPassword()
        {
            CreatedDate = DateTimeOffset.Now;
        }
    }
}