namespace Freakybite.ElijaWebServices.Entities.DataContracts
{
    public class DeviceModel
    {
        public int DeviceId { get; set; }

        public long Imei { get; set; }

        public string Brand { get; set; }

        public string Device { get; set; }

        public string Display { get; set; }

        public string Manufacturer { get; set; }

        public string Model { get; set; }

        public string Operator { get; set; }

        public string OsVersion { get; set; }

        public string Product { get; set; }

        public string AndroidId { get; set; }

        public string CodeVersion { get; set; }

        public string ReleaseVersion { get; set; }

        public bool IsPhone { get; set; }

        public string RegistrationDate { get; set; }
    }
}