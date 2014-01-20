using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Freakybite.ElijaWebServices.RestServices
{
    using System.Collections.ObjectModel;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;

    public class ErrorServiceBehavior : Attribute, IServiceBehavior  
    {
        #region Fields

        Type errorHandlerType;

        #endregion

        #region Constructors and Destructors

        public ErrorServiceBehavior(Type errorHandlerType)
        {
            this.errorHandlerType = errorHandlerType;
        }

        #endregion

        #region Public Methods and Operators

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints,
            BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {                        
            IErrorHandler errorHandler;
            errorHandler = (IErrorHandler)Activator.CreateInstance(errorHandlerType);

            foreach (ChannelDispatcherBase channelDispatcherBase in serviceHostBase.ChannelDispatchers)
            {
                ChannelDispatcher channelDispatcher = channelDispatcherBase as ChannelDispatcher;
                channelDispatcher.ErrorHandlers.Add(errorHandler);
            }
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {           
        }

        #endregion
    }
}