using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Freakybite.ElijaWebServices.Entities;
using Freakybite.ElijaWebServices.Processing;

namespace Freakybite.ElijaWebServices.RestServices
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "ElijaWebServices" en el código, en svc y en el archivo de configuración a la vez.
    // NOTA: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione ElijaWebServices.svc o ElijaWebServices.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class ElijaWebServices : IElijaWebServices
    {
        public Result UserRegistration(UserModel user)
        {
            var service = new ElijaServiceManager();
            var result = service.RegisterUser(user);
            return result;
        }
    }
}
