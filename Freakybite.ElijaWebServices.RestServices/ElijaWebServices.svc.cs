using Freakybite.ElijaWebServices.Entities.DataContracts;
using Freakybite.ElijaWebServices.Processing.ServiceManagers;

namespace Freakybite.ElijaWebServices.RestServices
{
    public class ElijaWebServices : IElijaWebServices
    {
        /// <summary>
        ///     Handles the registration of a new User.
        /// </summary>
        /// <returns></returns>
        public Result UserRegistration(UserDeviceModel userDevice)
        {
            var service = new ElijaServiceManager();
            var result = service.RegisterUser(userDevice);
            return result;
        }
    }
}