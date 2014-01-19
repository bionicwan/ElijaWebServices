using System;
using Freakybite.ElijaWebServices.Processing.ServiceManagers;

namespace Freakybite.ElijaWebServices.Processing.Helpers
{
    public static class ValidationHelper
    {
        public static bool ValidateToken(string userToken)
        {
            var token = new Guid();

            if (!Guid.TryParse(userToken, out token))
            {
                return false;
            }
            var service = new ElijaServiceManager();
            var user = service.FindUserByToken(token);
            return user != null;
        }
    }
}
