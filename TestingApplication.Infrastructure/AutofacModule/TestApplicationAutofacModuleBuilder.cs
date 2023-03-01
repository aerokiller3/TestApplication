using Autofac;
using TestingApplication.Infrastructure.Base.Interface;
using TestingApplication.Infrastructure.Base.Repository;
using TestingApplication.Infrastructure.Data;
using TestingApplication.Infrastructure.Services;

namespace TestingApplication.Infrastructure.AutofacModule
{
    public class TestApplicationAutofacModuleBuilder : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApplicationDBContext>().AsSelf().InstancePerLifetimeScope().AsImplementedInterfaces();
            builder.RegisterType<SearchService>().AsSelf().InstancePerLifetimeScope().AsImplementedInterfaces();

            builder.RegisterGeneric(typeof(RepositoryService<>)).As(typeof(IRepository<>)).AsSelf();
        }
    }
}
