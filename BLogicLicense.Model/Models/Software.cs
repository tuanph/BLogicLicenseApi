using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BLogicLicense.Model.Models
{
    public class Software
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int Code { get; set; }

        [StringLength(50)]
        [Column(TypeName = "nvarchar")]
        public string Name { get; set; }
    }
}
