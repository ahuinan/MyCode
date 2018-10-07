using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.WebApi;
using Unity;
using System.Web.Mvc;
using System.Web.Http.Dispatcher;
using System.Web.Http;
using Microsoft.Practices.Unity;
using System.Web.Http.Controllers;
using System.Net.Http;

namespace MyCode.Project.Infrastructure.UnityExtensions
{




    public class UnityHttpControllerActivator:IHttpControllerActivator
    {
        public IUnityContainer UnityContainer { get; private set; }
      
         public UnityHttpControllerActivator(IUnityContainer unityContainer)
         {        
             this.UnityContainer = unityContainer;
         }


        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
       {
            return (IHttpController)this.UnityContainer.Resolve(controllerType);
       }




    }
}
