namespace Freakybite.ElijaWebServices.Entities.DataContracts
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Data Contract used to carry any information from the server to the client in response to a service request.
    /// </summary>
    [DataContract]
    public class Result
    {
        #region Public Properties

        /// <summary>
        /// Carries any information returned by the service. The information is set as a JSON string.
        /// </summary>
        [DataMember]
        public string Content { get; set; }

        /// <summary>
        /// In case an error occurs, the message of the error is contained in the Message field.
        /// </summary>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// A flag indicating the status of an operation.
        /// </summary>
        [DataMember]
        public bool Success { get; set; }

        #endregion
    }
}