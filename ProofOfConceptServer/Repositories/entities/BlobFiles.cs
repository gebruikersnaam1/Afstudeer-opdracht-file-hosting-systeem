using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProofOfConceptServer.entities
{

    //TODO: adjust the datatype
    public class BlobEntity
    {
        public string fileId { get; set; }
        public string fileName { get; set; }
        public string date { get; set; }
        public string pathFile { get; set; }
        public string fileSize { get; set; }
        public string userId { get; set; }
        public string description { get; set; }
    }

}
