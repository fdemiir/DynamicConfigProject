using Autofac;
using DynamicConfig.Absract;
using DynamicConfig.Services;
using System;
using Xunit;

namespace NUnitTestDynamicConfig
{
    public class UnitTestConfReader
    {
        private IContainer _container;

        [Fact]
        public void IsGetValueWork()
        {
            _container = DependencyService.Instance.CurrentResolver;

            IConfigurationReader _reader = _container.Resolve<IConfigurationReader>();

            string siteName = _reader.GetValue<string>("SiteName");

            Assert.Equal("boyner.com.tr", siteName);
        }     
    }
}