namespace Freakybite.ElijaWebServices.Entities.DataContracts
{
    public class UserModel
    {
        public long UserId { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public string DateOfBirth { get; set; }

        public string TelephoneHome { get; set; }

        public string TelephoneOffice { get; set; }

        public string TelephoneMobile { get; set; }

        public string Email { get; set; }

        public string City { get; set; }

        public string RegistrationDate { get; set; }
    }
}
