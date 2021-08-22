using Autofac;
using DynamicConfig.Absract;
using DynamicConfig.Models;
using DynamicConfig.Services;
using MongoDB.Driver;

namespace DynamicConfig.DAL
{
    public class ConfigurationContext : IConfigurationContext
    {
        private readonly IMongoDatabase _db;

        public ConfigurationContext()
        {
            Config config = DependencyService.Instance.CurrentResolver.Resolve<Config>();
            var client = new MongoClient(config.ConnectionString);
            _db = client.GetDatabase(config.Database);
        }

        public IMongoCollection<ConfigurationViewModel> Configurations => _db.GetCollection<ConfigurationViewModel>("Configurations");
    }
}
