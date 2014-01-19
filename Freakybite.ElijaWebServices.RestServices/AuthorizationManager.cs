using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Web;
using Freakybite.ElijaWebServices.Entities.DataContracts;
using Freakybite.ElijaWebServices.Processing.Helpers;

namespace Freakybite.ElijaWebServices.RestServices
{
    public class AuthorizationManager : ServiceAuthorizationManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// The check access.
        /// </summary>
        /// <param name="operationContext">
        /// The operation context.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool CheckAccess(OperationContext operationContext)
        {
            var requestProperties =
                (HttpRequestMessageProperty)operationContext.IncomingMessageProperties["httpRequest"];

            try
            {
                if (OperationContext.Current.IncomingMessageProperties["HttpOperationName"].ToString().ToLower().Equals("userregistration"))
                {
                    return true;
                }

                var token = requestProperties.Headers["Token"];

                if (token == null)
                {
                    throw new FaultException("User token missing.");   
                }

                if (!ValidationHelper.ValidateToken(token))
                {
                    throw new FaultException("Invalid user token.");
                }

                return true;
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }

        #endregion
    }
}