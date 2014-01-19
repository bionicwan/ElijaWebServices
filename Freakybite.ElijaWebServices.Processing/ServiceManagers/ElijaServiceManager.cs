using System;

using Freakybite.ElijaWebServices.Entities;

namespace Freakybite.ElijaWebServices.Processing.ServiceManagers
{
    using System.Globalization;

    using DataAccess.Model;
    using DataAccess.Repositories.Implementations;
    using Entities.DataContracts;

    public class ElijaServiceManager
    {
        #region Fields

        private readonly DbContextFactory context = new DbContextFactory();

        private DeviceRepository deviceRepository;

        private UserRepository userRepository;

        private UserDeviceRepository userDeviceRepository;

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

        public UserDeviceRepository UserDeviceRepository
        {
            get
            {
                if (this.userDeviceRepository == null)
                {
                    this.userDeviceRepository = new UserDeviceRepository(context);
                }

                return userDeviceRepository;
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
        /// <param name="userDevice"></param>
        /// <returns></returns>
        public Result RegisterUser(UserDeviceModel userDevice)
        {
            var result = new Result() {Success = true};
            DateTime dateOfBirth;
            DateTime registrationDate;

            if (userDevice == null)
            {
                result.Success = false;
                result.Message = "User information cannot be null.";
                return result;
            }

            if (!DateTime.TryParseExact(
                        userDevice.Birthday,
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out dateOfBirth))
            {
                result.Success = false;
                result.Message = "The date of birth is in the wrong format.";
                return result;
            }

            if (!DateTime.TryParse(userDevice.RegistrationDate, out registrationDate))
            {
                result.Success = false;
                result.Message = "The registration date is in the wrong format.";
                return result;
            }

            try
            {
                var userDb = UserRepository.FindFirstBy(e => e.FacebookId == userDevice.FacebookId);
                if (userDb == null)
                {
                    InsertNewUser(userDevice, dateOfBirth, registrationDate);
                }
                else
                {
                    var device =
                        UserDeviceRepository.FindFirstBy(
                            e => e.UserId == userDb.UserId && e.Device.AndroidId == userDevice.AndroidId);
                    if (device == null)
                    {
                        InsertNewDevice(userDb, userDevice, registrationDate);
                    }
                }
            }
            catch (Exception)
            {
                result.Success = false;
                result.Message = "The operation was unsuccessful.";
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
            var result = new Result() {Success = true};

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
                        CodeVersion = device.CodeVersion,
                        AndroidId = device.AndroidId,
                        IsPhone = device.IsPhone
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

        private void InsertNewUser(UserDeviceModel user, DateTime dateOfBirth, DateTime registrationDate)
        {
            var userDevice = new UserDevice();
            var maxUserId = 1L;
            var userId = UserRepository.MaxEntity();
            if (userId != null)
            {
                maxUserId = maxUserId + userId.UserId;
            }
            userDevice.User = new User
            {
                UserId = maxUserId,
                FacebookId = user.FacebookId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Age = user.Age,
                City = user.City,
                Email = user.Email,
                Gender = user.Gender,
                FacebookLink = user.FacebookLink,
                Address = user.Address,
                Birthday = dateOfBirth,
                RegistrationDate = registrationDate
            };

            var maxDeviceId = 1L;
            var deviceId = DeviceRepository.MaxEntity();
            if (deviceId != null)
            {
                maxDeviceId = maxDeviceId + deviceId.DeviceId;
            }

            userDevice.Device = new Device
            {
                DeviceId = maxDeviceId,
                IMEI = user.Imei,
                AndroidId = user.AndroidId,
                Brand = user.Brand,
                CodeVersion = user.CodeVersion,
                Device1 = user.Device,
                Display = user.Display,
                IsPhone = user.IsPhone,
                Manufacturer = user.Manufacturer,
                Model = user.Model,
                Operator = user.Operator,
                OsVersion = user.OsVersion,
                Product = user.Product,
                ReleaseVersion = user.ReleaseVersion,
                RegistrationDate = registrationDate
            };

            userDevice.CreatedAt = DateTime.Now;
            userDevice.LastActivityDate = DateTime.Now;

            UserDeviceRepository.Add(userDevice);
            UserDeviceRepository.Save();
        }

        private void InsertNewDevice(User user, UserDeviceModel userDevice, DateTime registrationDate)
        {
            var maxDeviceId = 1L;
            var deviceId = DeviceRepository.MaxEntity();
            if (deviceId != null)
            {
                maxDeviceId = maxDeviceId + deviceId.DeviceId;
            }

            var device = new Device
            {
                DeviceId = maxDeviceId,
                IMEI = userDevice.Imei,
                AndroidId = userDevice.AndroidId,
                Brand = userDevice.Brand,
                CodeVersion = userDevice.CodeVersion,
                Device1 = userDevice.Device,
                Display = userDevice.Display,
                IsPhone = userDevice.IsPhone,
                Manufacturer = userDevice.Manufacturer,
                Model = userDevice.Model,
                Operator = userDevice.Operator,
                OsVersion = userDevice.OsVersion,
                Product = userDevice.Product,
                ReleaseVersion = userDevice.ReleaseVersion,
                RegistrationDate = registrationDate
            };

            DeviceRepository.Add(device);
            DeviceRepository.Save();

            var userDeviceDb = new UserDevice
            {
                UserId = user.UserId,
                DeviceId = device.DeviceId,
                CreatedAt = DateTime.Now,
                LastActivityDate = DateTime.Now
            };

            UserDeviceRepository.Add(userDeviceDb);
            userDeviceRepository.Save();
        }
    }
}
