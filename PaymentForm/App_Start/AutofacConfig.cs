using System.Configuration;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using PaymentForm.Domain;
using PaymentForm.Services;

namespace PaymentForm
{
    public class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(PaymentFormApplication).Assembly);
            builder.RegisterType<PaymentService>();
            builder.RegisterInstance(new PaymentSettings
            {
                PublicId = ConfigurationManager.AppSettings["PublicId"],
                ApiKey = ConfigurationManager.AppSettings["ApiKey"]
            });
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}