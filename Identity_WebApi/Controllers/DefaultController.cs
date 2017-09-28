using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;

namespace Identity_WebApi.Controllers
{
    public class ResponseDTO {
        public string message { get; set; }
    }
    public class DefaultController : ApiController
    {
        [HttpGet]
        public IHttpActionResult One() {
            var message = new ResponseDTO { message="廢話" };
            return Json(message);
        }
    }
}
