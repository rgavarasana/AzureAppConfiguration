using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Ravi.Learn.Azure.AppConfiguration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationsController : ControllerBase
    {
        private readonly ILogger<ConfigurationsController> _logger;
        private readonly IConfiguration _configuration;
        public ConfigurationsController(ILogger<ConfigurationsController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<string> GetAllValues()
        {
            var aiLogLevel = _configuration["AppInsightsSettings:LogLevel"];
            return await Task.FromResult(aiLogLevel);
        }

        [Route("CacheSettings")]
        [HttpGet]
        public async Task<string> GetCacheSetting()
        {
            var aiLogLevel = _configuration["CacheSettings:Servers"];
            return await Task.FromResult(aiLogLevel);
        }
    }
}