using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProofOfConceptServer.Repositories.entities.helpers
{
    public class DownloadFileResponse
    {
        public byte[] File { get; set;} 
        public string FileName { get; set;  }
    }
}
