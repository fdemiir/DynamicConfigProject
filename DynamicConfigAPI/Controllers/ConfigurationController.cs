using Autofac;
using DynamicConfig.Absract;
using DynamicConfig.Models;
using DynamicConfig.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicConfigAPI.Controllers
{

    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class ConfigurationsController : Controller
    {
        private readonly IContainer _container;
        private readonly IConfigurationRepository _configurationRepository;
        private ResponseModel _responseModel;

        public ConfigurationsController()
        {
            _responseModel = new ResponseModel();
            _container = DependencyService.Instance.CurrentResolver;
            _configurationRepository = _container.Resolve<IConfigurationRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> List(string searchModel)
        {

            var list = await _configurationRepository.GetAll();
            if (!string.IsNullOrEmpty(searchModel))
            {
                list = list.Where(v => v.Name.ToLower().Contains(searchModel.ToLower()));
            }

            _responseModel.Data = list;

            return Ok(_responseModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ConfigurationViewModel config)
        {
            await _configurationRepository.Create(config);

            return Ok(_responseModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] ConfigurationViewModel config)
        {
            _responseModel.Status = await _configurationRepository.Update(config);
            return Ok(_responseModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string _id)
        {
            _responseModel.Status = await _configurationRepository.Delete(_id);
            return Ok(_responseModel);
        }
    }
}