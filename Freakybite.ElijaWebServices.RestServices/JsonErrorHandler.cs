using System;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;

namespace Freakybite.ElijaWebServices.RestServices
{
    using Freakybite.ElijaWebServices.Entities.DataContracts;

    public class JsonErrorHandler : IErrorHandler
    {
        #region Public Methods and Operators

        public bool HandleError(Exception error)
        {            
            return true; // We will handle the exception!
        }

        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            
            // Creating message
            //var jsonError = new JsonErrorDetails
            //{
            //    Message = error.Message, 
            //    ExceptionType = error.GetType().FullName,
            //    ErrorCode = null//Convert.ToString(ErrorMessages.GetErrorCode(error.Message))
            //}; 

            var result = new Result { Message = error.Message, Success = false };

            fault = Message.CreateMessage(version, string.Empty, result,
                                          new DataContractJsonSerializer(typeof(Result)));

            // Tell WCF to use JSON encoding rather than default XML
            var wbf = new WebBodyFormatMessageProperty(WebContentFormat.Json);
            fault.Properties.Add(WebBodyFormatMessageProperty.Name, wbf);
            
            // Modifying the response
            var rmp = new HttpResponseMessageProperty
            {                
                StatusDescription = "Custom Fault"
            };

            //Mark the jsonerror and json content
            rmp.Headers[HttpResponseHeader.ContentType] = "application/json";
            rmp.Headers["jsonerror"] = "true";

            WebOperationContext.Current.OutgoingResponse.Format = WebMessageFormat.Json;
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json";

            fault.Properties.Add(HttpResponseMessageProperty.Name, rmp);
            
            //Debug ErrorHandlerHelper.LogError(new Exception("Message created for exception: " + error.Message + ", is ready to send as json")); //view on log!
        }

        #endregion
    }

    //[DataContract]
    //public class JsonErrorDetails
    //{
    //    [DataMember(Name = "message")]
    //    public string Message { get; set; }

    //    [DataMember(Name = "exception-type")]
    //    public string ExceptionType { get; set; }

    //    [DataMember(Name = "error-code")]
    //    public string ErrorCode { get; set; }
    //}        
}
