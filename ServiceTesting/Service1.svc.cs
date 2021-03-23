using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Text;

namespace ServiceTesting
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class MyService : IMyContract,IMyOtherContract
    {
        //public static void Configure(ServiceConfiguration config)
        //{
        //    Binding wsBinding = new WSHttpBinding();
        //    Binding tcpBinding = new NetTcpBinding();

        //    config.AddServiceEndpoint(typeof(IMyContract), wsBinding,
        //    "http://localhost:8000/MyService");
        //    config.AddServiceEndpoint(typeof(IMyContract), tcpBinding,
        //    "http://localhost:8001/MyService");
        //    config.AddServiceEndpoint(typeof(IMyOtherContract), tcpBinding,
        //    "http://localhost:8002/MyService");
        //}
        public string MyMethod(string text)
        {
            return "Hello " + text;
        }
        public string MyOtherMethod(string text)
        {
            return "Cannot call this method over WCF";
        }
    }

    // Wrapper Service class

    //class MyClass : WcfWrapper<MyService, IMyContract>, IMyContract
    //{
    //    public string MyMethod(string text)
    //    {
    //        return Proxy.MyMethod();
    //    }


    //    public string MyOtherMethod(string text)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    public class ServiceHost<T> : ServiceHost
    {
        public ServiceHost() : base(typeof(T))
        { }
        public ServiceHost(params string[] baseAddresses) : base(typeof(T),
        baseAddresses.Select(address => new Uri(address)).ToArray())
        { }
        public ServiceHost(params Uri[] baseAddresses) : base(typeof(T), baseAddresses)
        { }

        public void EnableMetadataExchange(bool enableHttpGet = true)
        {
            
            if (State.Equals(CommunicationState.Opened))
            {
                throw new InvalidOperationException("Host is already opened");
            }
            ServiceMetadataBehavior metadataBehavior = Description.Behaviors.Find<ServiceMetadataBehavior>();
            if (metadataBehavior == null)
            {
                metadataBehavior = new ServiceMetadataBehavior();
                Description.Behaviors.Add(metadataBehavior);
                if (BaseAddresses.Any(uri => uri.Scheme == "http"))
                {
                    metadataBehavior.HttpGetEnabled = enableHttpGet;
                }
            }
            AddAllMexEndPoints();
        }
        public bool HasMexEndpoint
        {
            get
            {
                return Description.Endpoints.Any(
                endpoint => endpoint.Contract.ContractType ==
                typeof(IMetadataExchange));
            }
        }
        public void AddAllMexEndPoints()
        {
            Debug.Assert(HasMexEndpoint == false);
            foreach (Uri baseAddress in BaseAddresses)
            {
                Binding binding = null;
                switch (baseAddress.Scheme)
                {
                    case "net.tcp":
                        {
                            binding = MetadataExchangeBindings.CreateMexTcpBinding();
                            break;
                        }
                   // case "net.pipe":
                    //    { ...}
                    //case "http":
                    //    { ...}
                   // case "https":
                    //    { ...}
                }
                if (binding != null)
                {
                    AddServiceEndpoint(typeof(IMetadataExchange), binding, "MEX");
                }
            }
        }
    }


    //public class ServiceHost : ServiceHostBase
    //{
    //    public ServiceEndpoint AddServiceEndpoint(Type implementedContract,Binding binding,string address)
    //    {
    //        return AddServiceEndpoint(implementedContract, binding, address);
    //    }

    //    protected override ServiceDescription CreateDescription(out IDictionary<string, ContractDescription> implementedContracts)
    //    {
    //        throw new NotImplementedException();
    //    }
    //    //Additional members
    //}

    //public abstract class ChannelFactory
    //{
    //    public ServiceEndpoint Endpoint
    //    { get; }
    //    //More members
    //}
    //public class ChannelFactory<T> : ChannelFactory
    //{
    //    public ChannelFactory(ServiceEndpoint endpoint);
    //    public ChannelFactory(string configurationName);
    //    public ChannelFactory(Binding binding, EndpointAddress endpointAddress);
    //    public static T CreateChannel(Binding binding, EndpointAddress endpointAddress);
    //    public T CreateChannel();
    //    //More members
    //}

    //public static class InProcFactory
    //{
    //    static readonly string BaseAddress = "net.pipe://localhost/" + Guid.NewGuid();
    //    static readonly Binding Binding;
    //    static Dictionary<Type, Tuple<ServiceHost, EndpointAddress>> m_Hosts =
    //    new Dictionary<Type, Tuple<ServiceHost, EndpointAddress>>();
    //    static InProcFactory()
    //    {
    //        NetNamedPipeBinding binding = new NetNamedPipeBinding();
    //        binding.TransactionFlow = true;
    //        Binding = binding;
    //        AppDomain.CurrentDomain.ProcessExit += delegate
    //        {
    //            foreach (Tuple<ServiceHost, EndpointAddress>
    //            record in m_Hosts.Values)
    //            {
    //                record.Item1.Close();
    //            }
    //        };
    //    }
    //    public static I CreateInstance<S, I>() where I : class
    //    where S : I
    //    {
    //        EndpointAddress address = GetAddress<S, I>();
    //        return ChannelFactory<I>.CreateChannel(Binding, address);
    //    }
    //    static EndpointAddress GetAddress<S, I>() where I : class
    //    where S : class, I
    //    {
    //        Tuple<ServiceHost, EndpointAddress> record;
    //        if (m_Hosts.ContainsKey(typeof(S)))
    //        {
    //            hostRecord = m_Hosts[typeof(S)];
    //        }
    //        else
    //        {
    //            ServiceHost host = new ServiceHost(typeof(S));
    //            string address = BaseAddress + Guid.NewGuid();
    //            record = new Tuple<ServiceHost, EndpointAddress>(
    //            host, new EndpointAddress(address));
    //            m_Hosts[typeof(S)] = record;
    //            host.AddServiceEndpoint(typeof(I), Binding, address);
    //            host.Open();
    //        }
    //        return hostRecord;
    //    }
    //    public static void CloseProxy<I>(I instance) where I : class
    //    {
    //        ICommunicationObject proxy = instance as ICommunicationObject;
    //        Debug.Assert(proxy != null);
    //        proxy.Close();
    //    }
    //}

}

