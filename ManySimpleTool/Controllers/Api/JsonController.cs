using ManySimpleTool.Models;
using ManySimpleTool.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManySimpleTool.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class JsonController : ControllerBase
    {
        private readonly ILogger logger;

        public JsonController(ILogger<JsonController> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// test
        /// </summary>
        [HttpPost]
        public IActionResult Test(JsonTestModel model)
        {
            
            return Ok(new { Value = true, model.Name });
        }


        /// <summary>
        /// test
        /// </summary>
        [HttpPost("keytest")]
        public IActionResult KeyTest()
        {
            PublicKeyService publicKeyService = new PublicKeyService(logger);
            publicKeyService.Test("我I");
            return Ok(new { Value = true, Res = "0" });
        }
    }
}
