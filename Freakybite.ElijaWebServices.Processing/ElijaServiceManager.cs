using System;
using System.Linq;
using Freakybite.ElijaWebServices.DataAccess;
using Freakybite.ElijaWebServices.Entities;

namespace Freakybite.ElijaWebServices.Processing
{
    public class ElijaServiceManager
    {
        private ElijaContext db = new ElijaContext();

        public Result RegisterUser(UserModel user)
        {
            var result = new Result(){ Success = true };

            if (user == null)
            {
                result.Success = false;
                result.Message = "El usuario no puede ser nulo.";
                return result;
            }

            try
            {
                if (!db.User.Any(o => o.UserId == user.UserId))
                {
                    var userInsert = new User();

                    userInsert.UserId = user.UserId;
                    userInsert.Age = user.Age;
                    userInsert.City = user.City;

                    DateTime birthday;
                    if (DateTime.TryParse(user.DateOfBirth, out birthday))
                    {
                        userInsert.DateOfBirth = birthday;
                    }

                    userInsert.Email = user.Email;
                    userInsert.LastName = user.LastName;
                    userInsert.Name = user.Name;

                    DateTime registrationDate;
                    if (DateTime.TryParse(user.RegistrationDate, out registrationDate))
                    {
                        userInsert.RegistrationDate = registrationDate;
                    }

                    userInsert.TelephoneHome = user.TelephoneHome;
                    userInsert.TelephoneMobile = user.TelephoneMobile;
                    userInsert.TelephoneOffice = user.TelephoneOffice;

                    db.User.Add(userInsert);
                    db.SaveChanges();
                }

            }
            catch (Exception)
            {
                result.Success = false;
                result.Message = "No se pudo realizar la operación.";
            }

            return result;
        }
    }
}
