using System;
using System.Globalization;
using Freakybite.ElijaWebServices.DataAccess.Model;
using Freakybite.ElijaWebServices.DataAccess.Repositories.Implementations;
using Freakybite.ElijaWebServices.Entities.DataContracts;
using log4net;

namespace Freakybite.ElijaWebServices.Processing.ServiceManagers
{
    using Helpers;
    using Core.Resources;

    public class ElijaServiceManager
    {
        #region Fields

        private readonly DbContextFactory context = new DbContextFactory();

        private DeviceRepository deviceRepository;
        private UserDeviceRepository userDeviceRepository;
        private UserRepository userRepository;
        private static readonly ILog logger = LogManager.GetLogger(typeof(ElijaServiceManager));

        #endregion

        #region Public Properties

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

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Handles the process of resizing an image.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public Result ImageResize(string url)
        {
            var result = new Result();
            try
            {
                var image = ImageProcessingHelper.ResizeImage(url);
                result.Content = image;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.InnerException.InnerException != null ? ex.InnerException.Message : ex.Message;
            }

            return result;
        }

        /// <summary>
        /// Registers a new user into the platform. If it's a returning user, it logs them in and returns the user token.
        /// </summary>
        /// <param name="userDevice">
        /// Contains the necessary information about the user and their device to register them
        /// into the platform.
        /// </param>
        /// <returns>If the process is successful, it returns the user's token inside the Content field.</returns>
        public Result RegisterUser(UserDeviceModel userDevice)
        {
            var result = new Result();

            if (userDevice == null)
            {
                result.Success = false;
                result.Message = ErrorMessages.NullUserError;
                return result;
            }

            try
            {
                // Check whether the user is registered in the Data Base.
                var userDb = UserRepository.FindFirstBy(e => e.FacebookId == userDevice.FacebookId);
                if (userDb == null)
                {
                    return InsertNewUser(userDevice);
                }

                // If it's a registered user who made the request but from a non registered device, create a new record for
                // the new device.
                var device =
                    this.UserDeviceRepository.FindFirstBy(
                        e => e.UserId == userDb.UserId && e.Device.AndroidId == userDevice.AndroidId);
                if (device == null)
                {
                    return this.InsertNewDevice(userDb, userDevice);
                }

                //if the user and device are already registered, return the user's token.
                result.Content = userDb.Token.ToString();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ErrorMessages.ErrorMessage;
                logger.Error(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }

            return result;
        }

        #endregion

        #region Methods

        /// <summary>
        /// If the registration request comes from a registered user but with a new device, it registers it into the platform.
        /// </summary>
        /// <param name="userDb">The user's information that is already registered.</param>
        /// <param name="userDevice">Information about the device from which the request was made.</param>
        /// <returns>If the process is successful, it return the user's token inside the Content field.</returns>
        private Result InsertNewDevice(User userDb, UserDeviceModel userDevice)
        {
            var result = new Result();
            DateTime registrationDate;

            if (!DateTime.TryParse(userDevice.RegistrationDate, out registrationDate))
            {
                result.Success = false;
                result.Message = ErrorMessages.WrongRegistrationDateFormat;
                return result;
            }

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

            var maxUserDeviceId = 1L;
            var userDeviceId = UserDeviceRepository.MaxEntity();

            if (userDeviceId != null)
            {
                maxUserDeviceId = userDeviceId.UserDeviceId + 1;
            }

            var userDeviceDb = new UserDevice
            {
                UserDeviceId = maxUserDeviceId,
                UserId = userDb.UserId,
                DeviceId = device.DeviceId,
                CreatedAt = DateTime.Now,
                LastActivityDate = DateTime.Now
            };

            UserDeviceRepository.Add(userDeviceDb);
            var insertResult = UserDeviceRepository.Save();

            // If something goes wrong, undo the change.
            if (insertResult == 0)
            {
                DeviceRepository.Delete(device);
                DeviceRepository.Save();
            }

            result.Content = userDb.Token.ToString();
            result.Success = true;

            return result;
        }

        /// <summary>
        /// Registers a new user into the platform.
        /// </summary>
        /// <param name="user">The new user's information.</param>
        /// <returns>If the process is successful, it return the user's token inside the Content field.</returns>
        private Result InsertNewUser(UserDeviceModel user)
        {
            var result = new Result();
            DateTime registrationDate;
            var dateOfBirth = DateTime.MinValue;

            if (!string.IsNullOrEmpty(user.Birthday))
            {
                if (
                    !DateTime.TryParseExact(
                        user.Birthday,
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out dateOfBirth))
                {
                    result.Success = false;
                    result.Message = ErrorMessages.WrongDateOfBirthFormat;
                    return result;
                }
            }

            if (!DateTime.TryParse(user.RegistrationDate, out registrationDate))
            {
                result.Success = false;
                result.Message = ErrorMessages.WrongRegistrationDateFormat;
                return result;
            }

            var userDevice = new UserDevice();
            var maxUserDeviceId = 1L;
            var userDeviceId = UserDeviceRepository.MaxEntity();

            if (userDeviceId != null)
            {
                maxUserDeviceId = userDeviceId.UserDeviceId + 1;
            }

            userDevice.UserDeviceId = maxUserDeviceId;

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
                                      RegistrationDate = registrationDate,
                                      Token = userToken
                                  };

            // If the dateOfBirth field was included in the request assign it to the user's birthday field.
            if (dateOfBirth != DateTime.MinValue)
            {
                userDevice.User.Birthday = dateOfBirth;
            }

            // Add the new device's information.
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

            result.Content = userDevice.User.Token.ToString();
            result.Success = true;

            return result;
        }

        #endregion
    }
}