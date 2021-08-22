using Autofac;
using DynamicConfig.Absract;
using DynamicConfig.DAL;
using DynamicConfig.Helpers;
using DynamicConfig.Models;

namespace DynamicConfig.Services
{
    public static class AutofacResolver
    {
        public static IContainer Bind(this ContainerBuilder builder)
        {

            var configParameters = new NamedParameter[]
            {
                new NamedParameter("applicationName", ConfigSettings.ApplicationName),
                new NamedParameter("connectionString", ConfigSettings.ConnectionString),
                new NamedParameter("refreshTimerIntervalInMs", ConfigSettings.RefreshTimerIntervalInMs),

            };

            builder.RegisterType<ConfigurationReader>().As<IConfigurationReader>()
                .WithParameters(configParameters).SingleInstance();

            builder.RegisterType<ConfigurationContext>().As<IConfigurationContext>().SingleInstance();

            builder.RegisterType<Config>();

            var config = new ServerConfig();
            builder.RegisterInstance(config);

            builder.RegisterType<MongoConfigurationRepository>().As<IConfigurationRepository>();

            return builder.Build();
        }
    }
}
