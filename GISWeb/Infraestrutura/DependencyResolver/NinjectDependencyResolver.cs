using GISCore.DI;
using GISWeb.Infraestrutura.Provider.Abstract;
using GISWeb.Infraestrutura.Provider.Concrete;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GISWeb.Infraestrutura.DependencyResolver
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        public IKernel Kernel { get; private set; }

        public NinjectDependencyResolver()
        {
            Kernel = new StandardKernel(new GISNinjectModule());
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return Kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return Kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            Kernel.Bind<ICustomAuthorizationProvider>().To<CustomAuthorizationProvider>();
        }
    }
}