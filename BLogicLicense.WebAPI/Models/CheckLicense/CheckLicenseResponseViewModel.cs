namespace BLogicLicense.Web.Models.CheckLicense
{
    public class CheckLicenseResponseViewModel
    {
        public string ReturnCode { get; set; }
        public string Key { get; set; }
        public string Message { get; set; }
        public string ActionCode { get; set; }
        public int CountDays { get; set; }
        public int StoreID { get; set; }
    }
}