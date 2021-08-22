using Autofac;
using DynamicConfig.Absract;
using DynamicConfig.Models;
using DynamicConfig.Services;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DynamicConfig.DAL
{
    public class MongoConfigurationRepository : IConfigurationRepository
    {
        private readonly IConfigurationContext _context;

        public MongoConfigurationRepository()
        {
            _context = DependencyService.Instance.CurrentResolver.Resolve<IConfigurationContext>();
        }


        public async Task<IEnumerable<ConfigurationViewModel>> GetAll()
        {
            return await _context.Configurations
                .Find(_ => true)
                .ToListAsync();
        }

        public Task<ConfigurationViewModel> GetOne(string id)
        {
            FilterDefinition<ConfigurationViewModel> filter = Builders<ConfigurationViewModel>.Filter.Eq(m => m._id, id);
            return _context
                .Configurations
                .Find(filter)
                .FirstOrDefaultAsync();
        }

        public async Task Create(ConfigurationViewModel model)
        {
            await _context.Configurations.InsertOneAsync(model);
        }

        public async Task<bool> Update(ConfigurationViewModel model)
        {
            ReplaceOneResult updateResult =
                await _context
                    .Configurations
                    .ReplaceOneAsync(
                        filter: g => g._id == model._id,
                        replacement: model);
            return updateResult.IsAcknowledged
                   && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> Delete(string id)
        {
            FilterDefinition<ConfigurationViewModel> filter = Builders<ConfigurationViewModel>.Filter.Eq(m => m._id, id);
            DeleteResult deleteResult = await _context
                .Configurations
                .DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged
                   && deleteResult.DeletedCount > 0;
        }

        public Task<long> GetNextId()
        {
            throw new System.NotImplementedException();
        }
    }
}
