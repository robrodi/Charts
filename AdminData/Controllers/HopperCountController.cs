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
    public class HopperCountController : ApiController
    {
        const string dataPath = "~/App_Data/data.txt";
        private readonly HopperCountReader reader;
        
        public HopperCountController() : this(new HopperCountReader(HostingEnvironment.MapPath(dataPath))) { }
        public HopperCountController(HopperCountReader reader) 
        {
            this.reader = reader;
        }

        // GET api/default1
        public IEnumerable<HopperCount> Get()
        {
            return reader.GetHopperCounts();
        }

        // GET api/default1/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/default1
        public void Post([FromBody]string value)
        {
        }

        // PUT api/default1/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/default1/5
        public void Delete(int id)
        {
        }
    }
}
