namespace Freakybite.ElijaWebServices.Entities.DataContracts
{
    using System.Runtime.Serialization;

    [DataContract]
    public class Result
    {
        #region Public Properties

        [DataMember]
        public string Content { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public bool Success { get; set; }

        #endregion
    }
}