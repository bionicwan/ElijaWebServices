namespace Freakybite.ElijaWebServices.Entities.DataContracts
{
    public class UserDeviceModel
    {
        public long FacebookId { get; set; }

        public string FacebookLink { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public string Birthday { get; set; }

        public string Gender { get; set; }

        public string Email { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public string RegistrationDate { get; set; }

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
    }
}