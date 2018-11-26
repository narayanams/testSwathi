using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Web;

namespace TRIPWEB.TRAILSWEBServiceReference
{
   

    public class FillHeaderDataBehaviourExtension : BehaviorExtensionElement,IEndpointBehavior
    {
        #region BehaviorExtensionElement Implementation
        public override Type BehaviorType
        {
            get
            {
                return typeof(FillHeaderDataBehaviourExtension);
            }
        }
        protected override object CreateBehavior()
        {
            return this;
        }
        #endregion

        #region IServiceBehaviour Implementation
        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {

        }
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }
        
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.ClientMessageInspectors.Add(new MessageInspector());
        }
        #endregion
    }

    public class MessageInspector : IClientMessageInspector
    {
        //Below method will add a AppId in header before sending a request to service
        //This avoid code changes on individual calls
        //The same approach can be taken for passing other session values like ClientIP etc.
        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            MessageHeader<string> header = new MessageHeader<string>("TRIPWeb");
            MessageHeader untyped = header.GetUntypedHeader("AppId", "ns");
            request.Headers.Add(untyped);

            return null;
        }
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
        }
    }
}