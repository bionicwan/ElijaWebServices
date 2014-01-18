namespace Freakybite.ElijaWebServices.RestServices
{
    using Entities.DataContracts;
    using Processing.ServiceManagers;

    public class ElijaWebServices : IElijaWebServices
    {
        /// <summary>
        /// Handles the registration of a new User.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Result UserRegistration(UserModel user)
        {
            var service = new ElijaServiceManager();
            var result = service.RegisterUser(user);
            return result;
        }
        /// <summary>
        /// Handles the registration of a new Device.
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public Result DeviceRegistration(DeviceModel device)
        {
            var service = new ElijaServiceManager();
            var result = service.RegisterDevice(device);
            return result;
        }
    }
}
