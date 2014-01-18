using System;

using Freakybite.ElijaWebServices.Entities;

namespace Freakybite.ElijaWebServices.Processing.ServiceManagers
{
    using System.Globalization;

    using Freakybite.ElijaWebServices.DataAccess.Model;
    using Freakybite.ElijaWebServices.DataAccess.Repositories.Implementations;
    using Freakybite.ElijaWebServices.Entities.DataContracts;

    public class ElijaServiceManager
    {
        #region Fields

        private readonly DbContextFactory context = new DbContextFactory();

        private DeviceRepository deviceRepository;

        private UserRepository userRepository;

        #endregion

        #region Public Properties

        public UserRepository UserRepository
        {
            get
            {
                if (this.userRepository == null)
                {
                    this.userRepository = new UserRepository(context);
                }

                return userRepository;
            }
        }

        public DeviceRepository DeviceRepository
        {
            get
            {
                if (this.deviceRepository == null)
                {
                    this.deviceRepository = new DeviceRepository(context);
                }

                return deviceRepository;
            }
        }
        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Registers a new user into the Data Base.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
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
                var userDb = UserRepository.FindFirstBy(e => e.FacebookId == user.FacebookId);
                if (userDb == null)
                {
                    userDb = new User
                                 {
                                     FacebookId = user.FacebookId,
                                     FirstName = user.FirstName,
                                     LastName = user.LastName,
                                     Age = user.Age,
                                     City = user.City,
                                     Email = user.Email,
                                     TelephoneHome = user.TelephoneHome,
                                     TelephoneMobile = user.TelephoneMobile,
                                     TelephoneOffice = user.TelephoneOffice
                                 };

                    DateTime dateOfBirth;
                    DateTime registrationDate;

                    if (DateTime.TryParseExact(
                        user.Birthday,
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out dateOfBirth))
                    {
                        userDb.Birthday = dateOfBirth;
                    }

                    if (DateTime.TryParse(user.RegistrationDate, out registrationDate))
                    {
                        userDb.RegistrationDate = registrationDate;
                    }

                    UserRepository.Add(userDb);
                    UserRepository.Save();
                }
            }
            catch (Exception)
            {
                result.Success = false;
                result.Message = "No se pudo realizar la operación.";
            }

            return result;
        }

        /// <summary>
        /// Registers a new device into the Data Base.
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public Result RegisterDevice(DeviceModel device)
        {
            var result = new Result() { Success = true };

            if (device == null)
            {
                result.Success = false;
                result.Message = "El dispositivo no puede ser nulo.";
                return result;
            }

            try
            {
                var deviceDb = DeviceRepository.FindFirstBy(e => e.IMEI == device.Imei);
                if (deviceDb == null)
                {
                    deviceDb = new Device
                                   {
                                       IMEI = device.Imei,
                                       Brand = device.Brand,
                                       Device1 = device.Device,
                                       Display = device.Display,
                                       Manufacturer = device.Manufacturer,
                                       Model = device.Model,
                                       Operator = device.Operator,
                                       OsVersion = device.OsVersion,
                                       Product = device.Product,
                                       ReleaseVersion = device.ReleaseVersion,
                                       CodeVersion = device.CodeVersion
                                   };

                    DateTime registrationDate;

                    if (DateTime.TryParse(device.RegistrationDate, out registrationDate))
                    {
                        deviceDb.RegistrationDate = registrationDate;
                    }

                    DeviceRepository.Add(deviceDb);
                    DeviceRepository.Save();
                }
            }
            catch (Exception)
            {
                result.Success = false;
                result.Message = "No se pudo realizar la operación.";
            }

            return result;
        }

        #endregion
    }
}
