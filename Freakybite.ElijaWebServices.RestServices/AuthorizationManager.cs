using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Freakybite.ElijaWebServices.Processing.Helpers;

namespace Freakybite.ElijaWebServices.RestServices
{
    using System.IdentityModel.Tokens;

    public class AuthorizationManager : ServiceAuthorizationManager
    {
        #region Public Methods and Operators

        /// <summary>
        ///     The check access.
        /// </summary>
        /// <param name="operationContext">
        ///     The operation context.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public override bool CheckAccess(OperationContext operationContext)
        {
            var requestProperties =
                (HttpRequestMessageProperty) operationContext.IncomingMessageProperties["httpRequest"];

            try
            {
                if (
                    OperationContext.Current.IncomingMessageProperties["HttpOperationName"].ToString()
                        .ToLower()
                        .Equals("userregistration"))
                {
                    return true;
                }

                var token = requestProperties.Headers["Token"];

                if (token == null)
                {
                    throw new FaultException("User token missing.");
                }

                var validationHelper = new ValidationHelper();

                if (!validationHelper.ValidateToken(token))
                {
                    throw new FaultException("Invalid user token.");
                }

                return true;
            }
            catch (Exception e)
            {
                throw new SecurityTokenException(e.Message);
            }
        }

        #endregion
    }
}