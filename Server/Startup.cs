using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using System.Linq;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Ajax.Utilities;

[assembly: OwinStartup(typeof(Server.Startup))]

namespace Server
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //GlobalHost.DependencyResolver = new CustomerIdependencyResolver();

            //GlobalHost.DependencyResolver.Register(typeof(IJavaScriptMinifier), () => new DefaultJavaScriptMinifier());

            GlobalHost.HubPipeline.AddModule(new DefaultHubPipelineModule());

            app.MapSignalR();
        }
    }

    #region 自定义JavaScriptMinifier---实现代码JSd代码压缩
    /// <summary>
    /// 自定义JavaScriptMinifier
    /// 实现代码JSd代码压缩
    /// </summary>
    public class DefaultJavaScriptMinifier : IJavaScriptMinifier
    {
        public string Minify(string source)
        {
            return new Minifier().MinifyJavaScript(source);
        }
    }

    #endregion

    #region HubPipelineModule & IHubPipelineModule

    /// <summary>
    /// Pipeline管道是架构在中间件之上，目的可用来拦截hub的一些操作。。。。
    ///比如说，方法执行之前的拦截，方法执行之后的拦截。
    /// </summary>
    public class DefaultHubPipelineModule : HubPipelineModule
    {
        /// <summary>
        /// 对传入参数进行监控
        /// </summary>
        /// <param name="invoke"></param>
        /// <returns></returns>
        public override Func<IHubIncomingInvokerContext, Task<object>> BuildIncoming(Func<IHubIncomingInvokerContext, Task<object>> invoke)
        {
            return (context) =>
            {

                //TODO 在此处可以记录日志。。逻辑判断

                //方法名称
                var method = context.MethodDescriptor.Name;

                //方法参数
                var args = context.MethodDescriptor.Parameters;

                return invoke(context);
            };
        }


        /// <summary>
        /// 对输出参数进行监控
        /// </summary>
        /// <param name="invoke"></param>
        public override Func<IHubOutgoingInvokerContext, Task> BuildOutgoing(Func<IHubOutgoingInvokerContext, Task> send)
        {
            return send;
        }
    }

    public class DefaultIHubPipelineModule : IHubPipelineModule
    {
        public Func<HubDescriptor, IRequest, bool> BuildAuthorizeConnect(Func<HubDescriptor, IRequest, bool> authorizeConnect)
        {
            throw new NotImplementedException();
        }

        public Func<IHub, Task> BuildConnect(Func<IHub, Task> connect)
        {
            throw new NotImplementedException();
        }

        public Func<IHub, bool, Task> BuildDisconnect(Func<IHub, bool, Task> disconnect)
        {
            throw new NotImplementedException();
        }

        public Func<IHubIncomingInvokerContext, Task<object>> BuildIncoming(Func<IHubIncomingInvokerContext, Task<object>> invoke)
        {
            throw new NotImplementedException();
        }

        public Func<IHubOutgoingInvokerContext, Task> BuildOutgoing(Func<IHubOutgoingInvokerContext, Task> send)
        {
            throw new NotImplementedException();
        }

        public Func<IHub, Task> BuildReconnect(Func<IHub, Task> reconnect)
        {
            throw new NotImplementedException();
        }

        public Func<HubDescriptor, IRequest, IList<string>, IList<string>> BuildRejoiningGroups(Func<HubDescriptor, IRequest, IList<string>, IList<string>> rejoiningGroups)
        {
            throw new NotImplementedException();
        }
    }

    #endregion

    #region 自定义DependencyResolver

    ///// <summary>
    ///// 自定义DependencyResolver
    ///// </summary>
    //public class CustomerIdependencyResolver : DefaultDependencyResolver
    //{
    //    public override object GetService(Type serviceType)
    //    {
    //        var service = base.GetService(serviceType);

    //        Debug.WriteLine(string.Format("{0} => {1}", serviceType.Name, service?.GetType()?.Name));

    //        return service;
    //    }

    //    public override IEnumerable<object> GetServices(Type serviceType)
    //    {
    //        var services = base.GetServices(serviceType);

    //        Debug.WriteLine(string.Format("{0} => {1}", serviceType.Name, services.Select(i => i.GetType().Name)));

    //        return services;
    //    }
    //}

    #endregion
}
