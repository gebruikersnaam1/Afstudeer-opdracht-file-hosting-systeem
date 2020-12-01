using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProofOfConceptServer.Repositories.interfaces
{
    public class ISynchronicFiles
    {
        public DateTime CreatedOn { get; set; }
        public string FileName { get; set;  }
        public int FileSize { get; set; }
    }
}
