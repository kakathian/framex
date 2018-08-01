using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Framex.Platform.Processor;
using Framex.Platform.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Framex.Platform.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly ValuesProcessor _valuesProcessor;
        public ValuesController(ValuesProcessor processor)
        {
            this._valuesProcessor = processor;
        }
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<string> Get(int id, string name)
        {
            // Sample processor built using frame fr framework
            await _valuesProcessor.ProcessAsync();
            return _valuesProcessor.Response;
        }
    }
}
