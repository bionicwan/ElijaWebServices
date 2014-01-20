using Freakybite.ElijaWebServices.Entities.DataContracts;
using Freakybite.ElijaWebServices.Processing.ServiceManagers;

namespace Freakybite.ElijaWebServices.RestServices
{
    [ErrorServiceBehavior(typeof(JsonErrorHandler))]
    public class ElijaWebServices : IElijaWebServices
    {
        #region Public Methods and Operators

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

        #endregion
    }
}