using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BLogicLicense.Model.Models
{
    public class UnRegisterKey
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Key { get; set; }
        public int SoftwareID { get; set; }
        [ForeignKey("SoftwareID")]
        public virtual Software Software { set; get; }
        public DateTime DateConnected { get; set; }

        [StringLength(50)]
        [Column(TypeName = "varchar")]
        public string DeviceID { get; set; }                          
        [StringLength(50)]
        [Column(TypeName = "nvarchar")]
        public string DeviceName { get; set; }
    }
}
