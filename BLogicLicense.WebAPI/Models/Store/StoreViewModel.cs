using BLogicLicense.Model.Models;
using System;
using System.Collections.Generic;

namespace BLogicLicense.Web.Models.Store
{
    public class StoreViewModel
    {
        public int ID { get; set; }

        public string Token { get; set; }

        public string Name { get; set; }

        public string Agent { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }


        public int ExpriedType { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public ICollection<ProductKey> ProductKeys { get; set; }
    }
}