using Autofac;
using DynamicConfig.Absract;
using DynamicConfig.Models;
using DynamicConfig.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DynamicConfig.Helpers
{
    public class ConfigurationReader : IConfigurationReader
    {
        private ConcurrentBag<ConfigurationViewModel> _confList;

        private readonly IConfigurationRepository _configurationRepository;

        private readonly int _refreshTimerInterval;
        private readonly string _applicationName;
        private readonly string _connectionString;

        public CancellationToken CancellationToken { get; set; }

        public ConfigurationReader(string applicationName, string connectionString, int refreshTimerIntervalInMs)
        {
            _configurationRepository = DependencyService.Instance.CurrentResolver.Resolve<IConfigurationRepository>();
            _confList = new ConcurrentBag<ConfigurationViewModel>();
            _refreshTimerInterval = refreshTimerIntervalInMs;
            _applicationName = applicationName;
            _connectionString = connectionString;

            Task.Run(() => this.CheckDatas()).Wait(); 
            Task.Run(() => this.StartTimer(CancellationToken));
        }

        public async Task CheckDatas()
        {
            IEnumerable<ConfigurationViewModel> list = await _configurationRepository.GetAll();
            List<ConfigurationViewModel> conflist = list.Where(v => v.ApplicationName == _applicationName && v.IsActive == true).ToList();
            _confList.Clear();
            foreach (ConfigurationViewModel configuration in list)
            {
                _confList.Add(configuration);
            }
        }

        private async Task StartTimer(CancellationToken cancellationToken)
        {
            await Task.Run(async () =>
            {
                while (true)
                {
                    await CheckDatas();
                    await Task.Delay(_refreshTimerInterval, cancellationToken);
                    if (cancellationToken.IsCancellationRequested)
                        break;
                }
            }, cancellationToken);
        }

        public T GetValue<T>(string key)
        {
            ConfigurationViewModel configuration = _confList.FirstOrDefault(v => v.Name == key);
            if (configuration == null)
                throw new ArgumentNullException("There is no value for this key");

            Type confType = TypeMap(configuration.Type);

            object value = Convert.ChangeType(configuration.Value, confType);
            return (T)value;
        }

        public Type TypeMap(string type)
        {
            return DataTypeMapper.MapperWithKey()[type];
        }
    }
}
