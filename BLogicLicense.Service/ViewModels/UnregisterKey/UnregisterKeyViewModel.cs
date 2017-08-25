using System;

namespace BLogicLicense.Service.ViewModels.UnregisterKey
{
    public class UnregisterKeyViewModel
    {
        public int ID { get; set; }
        public string Key { get; set; }
        public int SoftwareID { get; set; }
        public DateTime DateConnected { get; set; }
        public string DeviceID { get; set; }
        public string DeviceName { get; set; }
        public string DateExpried { get; set; }
        public int StoreID { get; set; }
    }
}