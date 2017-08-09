using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BLogicLicense.Model.Models
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [StringLength(128)]
        public string UserID { set; get; }
        [ForeignKey("UserID")]
        public virtual AppUser User { get; set; }

        public int ProductKeyID { get; set; }
        [ForeignKey("ProductKeyID")]
        public virtual ProductKey ProductKey { get; set; }
        public DateTime DateCreated { get; set; }
        public string Type { get; set; } // New / Renew / Lock, Unclock
    }
}
