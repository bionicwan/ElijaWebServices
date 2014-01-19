using System;
using System.Globalization;
using Freakybite.ElijaWebServices.DataAccess.Model;
using Freakybite.ElijaWebServices.DataAccess.Repositories.Implementations;
using Freakybite.ElijaWebServices.Entities.DataContracts;

namespace Freakybite.ElijaWebServices.Processing.ServiceManagers
{
    public class ElijaServiceManager
    {
        #region Fields

        private readonly DbContextFactory context = new DbContextFactory();

        private DeviceRepository deviceRepository;
        private UserDeviceRepository userDeviceRepository;
        private UserRepository userRepository;

        #endregion

        #region Public Properties

        public UserRepository UserRepository
        {
            get
            {
                if (userRepository == null)
                {
                    userRepository = new UserRepository(context);
                }

                return userRepository;
            }
        }

        public UserDeviceRepository UserDeviceRepository
        {
            get
            {
                if (userDeviceRepository == null)
                {
                    userDeviceRepository = new UserDeviceRepository(context);
                }

                return userDeviceRepository;
            }
        }

        public DeviceRepository DeviceRepository
        {
            get
            {
                if (deviceRepository == null)
                {
                    deviceRepository = new DeviceRepository(context);
                }

                return deviceRepository;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Registers a new user into the Data Base.
        /// </summary>
        /// <param name="userDevice"></param>
        /// <returns></returns>
        public Result RegisterUser(UserDeviceModel userDevice)
        {
            var result = new Result {Success = true};
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
                Guid userToken;
                if (userDb == null)
                {
                    userToken = InsertNewUser(userDevice, dateOfBirth, registrationDate);
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

                    userToken = userDb.Token;
                }

                result.Content = "{\"UserToken\": \"" + userToken.ToString().ToUpper() + "\"}";
            }
            catch (Exception)
            {
                result.Success = false;
                result.Message = "The operation was unsuccessful.";
            }

            return result;
        }

        public User FindUserByToken(Guid token)
        {
            var user = UserRepository.FindFirstBy(e => e.Token == token);

            return user;
        }

        #endregion

        private Guid InsertNewUser(UserDeviceModel user, DateTime dateOfBirth, DateTime registrationDate)
        {
            var userDevice = new UserDevice();
            var userToken = Guid.NewGuid();
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
                RegistrationDate = registrationDate,
                Token = userToken
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

            return userDevice.User.Token;
        }

        private void InsertNewDevice(User user, UserDeviceModel userDevice, DateTime registrationDate)
        {
            long maxDeviceId = 1L;
            Device deviceId = DeviceRepository.MaxEntity();
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