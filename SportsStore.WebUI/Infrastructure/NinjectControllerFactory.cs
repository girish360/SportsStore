using Ninject;
using System.Web.Mvc;
using System;
using System.Web.Routing;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Concrete;
using System.Configuration;

namespace SportsStore.WebUI.Infrastructure {
    public class NinjectControllerFactory : DefaultControllerFactory {
        private IKernel ninjectKernel;

        public NinjectControllerFactory() {
            ninjectKernel = new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType) {
            return (controllerType == null) ? null : (IController)ninjectKernel.Get(controllerType);
        }

        private void AddBindings() {
            // put additional bindings here
            ninjectKernel.Bind<IProductRepository>().To<EFProductRepository>();
            ninjectKernel.Bind<IOrderProcessor>()
                .To<EmailOrderProcessor>()
                .WithConstructorArgument("settings", new EmailSettings {
                    WriteAsFile = bool.Parse(ConfigurationManager.AppSettings["Email.WriteAsFile"] ?? "false")
                });
            ninjectKernel.Bind<IAuthProvider>().To<FormsAuthProvider>();
        }
    }
}