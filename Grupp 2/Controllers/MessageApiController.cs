using Data;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace Grupp_2.Controllers
{
    public class MessageApiController : ApiController
    {
        
        [Route("api/SendAPI")]
        [HttpGet]
        public IHttpActionResult SendMessage()
        {
            return Ok();
        }

       
    }
}