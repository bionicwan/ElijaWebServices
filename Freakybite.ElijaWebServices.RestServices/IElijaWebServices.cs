using System.ServiceModel;
using System.ServiceModel.Web;
using Freakybite.ElijaWebServices.Entities;

namespace Freakybite.ElijaWebServices.RestServices
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IElijaWebServices" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface IElijaWebServices
    {
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "UserRegistration/")]
        Result UserRegistration(UserModel user);
    }
}
