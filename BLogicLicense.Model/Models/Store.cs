using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BLogicLicense.Model.Models
{
    public class Store
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [StringLength(100)]
        [Column(TypeName = "varchar")]
        public string Token { get; set; }
        [StringLength(50)]
        [Column(TypeName = "nvarchar")]
        public string Name { get; set; }

        [StringLength(50)]
        [Column(TypeName = "nvarchar")]
        public string Agent { get; set; }

        [StringLength(20)]
        [Column(TypeName = "varchar")]
        public string Phone { get; set; }
        [StringLength(50)]
        [Column(TypeName = "varchar")]
        public string Email { get; set; }
        [StringLength(150)]
        [Column(TypeName = "nvarchar")]
        public string Address { get; set; }

        public virtual ICollection<ProductKey> ProductKeys { get; set; }
        public int ExpriedType { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
