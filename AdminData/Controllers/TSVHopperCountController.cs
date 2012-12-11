using AdminData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Http;

namespace AdminData.Controllers
{
    public class TSVHopperCountController : ApiController
    {
        const string dataPath = "~/App_Data/data.txt";

        HopperCountReader model = new HopperCountReader(HostingEnvironment.MapPath(dataPath));

        // GET api/tsvhoppercount
        public string Get()
        {
            return model.RawTSV();
        }

        // GET api/tsvhoppercount/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/tsvhoppercount
        public void Post([FromBody]string value)
        {
        }

        // PUT api/tsvhoppercount/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/tsvhoppercount/5
        public void Delete(int id)
        {
        }
    }
}
