using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BLogicLicense.Service.ViewModels.UnregisterKey
{
    public class RegisterKeyViewModel
    {
        public string Key { get; set; }
        public int SoftwareID { get; set; }
        public int StoreID { get; set; }
        public DateTime DateExpried { get; set; }
        public int EmployeeID { get; set; }
        public bool IsNeverExpried { get; set; }
    }
}