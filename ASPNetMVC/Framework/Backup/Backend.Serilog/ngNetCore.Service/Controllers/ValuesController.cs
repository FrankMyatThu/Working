﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ngNetCore.Model;

namespace ngNetCore.Service.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly ILogger _logger;
        public ValuesController(ILogger<ValuesController> logger)
        {
            _logger = logger;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            _logger.LogInformation(string.Format("[{0}.{1}]", MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name));

            Class1 class1 = new Class1(ILogger<Class1>);

            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
