using DynamicConfig.Models;
using MongoDB.Driver;

namespace DynamicConfig.Absract
{
    public interface IConfigurationContext
    {
        IMongoCollection<ConfigurationViewModel> Configurations { get; }
    }
}
