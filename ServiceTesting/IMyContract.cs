using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ServiceTesting
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract(Namespace = "IMyContract")]
    public interface IMyContract
    {

        [OperationContract(Name = "SomeOperation")]
        string MyMethod(string text);
        //Will not be part of the contract
        string MyOtherMethod(string text);
    }
    [ServiceContract(Namespace = "IMyContract2")]
    public interface IMyOtherContract
    {

        [OperationContract(Name = "SomeOperation2")]
        string MyMethod(string text);

    }

    // Abstract class to wrap a class with a service

    //public abstract class WcfWrapper<S, I> : IDisposable, ICommunicationObject
    //where I : class
    //where S : class, I
    //{
    //    protected I Proxy
    //    { get; private set; }

    //    public CommunicationState State => throw new NotImplementedException();

    //    protected WcfWrapper()
    //    {
    //        Proxy = InProcFactory.CreateInstance<S, I>();
    //    }

    //    public event EventHandler Closed;
    //    public event EventHandler Closing;
    //    public event EventHandler Faulted;
    //    public event EventHandler Opened;
    //    public event EventHandler Opening;

    //    public void Dispose()
    //    {
    //        Close();
    //    }
    //    public void Close()
    //    {
    //        InProcFactory.CloseProxy(Proxy);
    //    }
    //    void ICommunicationObject.Close()
    //    {
    //        (Proxy as ICommunicationObject).Close();
    //    }

    //    public void Open()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void Abort()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public IAsyncResult BeginClose(AsyncCallback callback, object state)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public IAsyncResult BeginOpen(AsyncCallback callback, object state)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void EndClose(IAsyncResult result)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void EndOpen(IAsyncResult result)
    //    {
    //        throw new NotImplementedException();
    //    }
    //    //Rest of ICommunicationObject
    //}

    public interface ICommunicationObject
    {
        void Open();
        void Close();
        void Abort();
        event EventHandler Closed;
        event EventHandler Closing;
        event EventHandler Faulted;
        event EventHandler Opened;
        event EventHandler Opening;
        IAsyncResult BeginClose(AsyncCallback callback, object state);
        IAsyncResult BeginOpen(AsyncCallback callback, object state);
        void EndClose(IAsyncResult result);
        void EndOpen(IAsyncResult result);
        CommunicationState State
        { get; }
        //More members
    }
    public enum CommunicationState
    {
        Created,
        Opening,
        Opened,
        Closing,
        Closed,
        Faulted
    }


}
