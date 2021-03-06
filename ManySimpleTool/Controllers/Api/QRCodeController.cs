﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManySimpleTool.Models;
using ManySimpleTool.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManySimpleTool.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class QRCodeController : ControllerBase
    {
        private QRCodeService qrCodeService = new QRCodeService();

        /// <summary>
        /// test
        /// </summary>
        [HttpPost]
        public IActionResult QRCode(QRCodeModel model)
        {
            byte[] byteImg = qrCodeService.Create(model.InputText);
            return File(byteImg, "image/png");
            //return Ok(new { Value = true, model.InputText });
        }
    }
}
