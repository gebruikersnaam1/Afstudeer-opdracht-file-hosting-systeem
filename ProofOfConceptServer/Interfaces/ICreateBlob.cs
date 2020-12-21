using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProofOfConceptServer.entities.interfaces
{
    public class ICreateBlob
    {
        public IFormFile file { get; set; }

        public string description { get; set; }
        public string userId { get; set; }

    }
}