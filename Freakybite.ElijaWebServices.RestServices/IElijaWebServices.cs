using System.ServiceModel;
using System.ServiceModel.Web;

namespace Freakybite.ElijaWebServices.RestServices
{
    using Freakybite.ElijaWebServices.Entities.DataContracts;

    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IElijaWebServices" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface IElijaWebServices
    {
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "UserRegistration/")]
        Result UserRegistration(UserDeviceModel userDevice);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "DeviceRegistration/")]
        Result DeviceRegistration(DeviceModel device);
    }
}
