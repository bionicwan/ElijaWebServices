using Freakybite.ElijaWebServices.Entities.DataContracts;
using Freakybite.ElijaWebServices.Processing.ServiceManagers;

namespace Freakybite.ElijaWebServices.RestServices
{
    [ErrorServiceBehavior(typeof(JsonErrorHandler))]
    public class ElijaWebServices : IElijaWebServices
    {
        #region Public Methods and Operators

        /// <summary>
        /// Resizes an image from the specified url.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public Result ImageResize(string url)
        {
            var service = new ElijaServiceManager();
            var result = service.ImageResize(url);
            return result;
        }

        /// <summary>
        /// Handles the registration of a new User.
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