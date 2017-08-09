using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BLogicLicense.Model.Models
{
    public class ProductKey
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int StoreID { get; set; }
        [ForeignKey("StoreID")]
        public virtual Store Store { get; set; }

        public int SoftwareID { get; set; }
        [ForeignKey("SoftwareID")]
        public virtual Software Software { get; set; }

        [StringLength(50)]
        [Column(TypeName = "varchar")]
        public string Key { get; set; }

        public DateTime DateExpire { get; set; }
        public DateTime LastRenewal { get; set; }

        [StringLength(50)]
        [Column(TypeName = "varchar")]
        public string DeviceID { get; set; }

        [StringLength(50)]
        [Column(TypeName = "nvarchar")]
        public string DeviceName { get; set; }
        public bool IsLock { get; set; }
    

    }
}
