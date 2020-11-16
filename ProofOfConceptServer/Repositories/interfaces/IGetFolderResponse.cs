using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProofOfConceptServer.Repositories.entities.interfaces
{
    public class IFolderResponse
    {
        public string Name { get; set; }
        public DateTime LastChanged { get; set;  }
        public int Id { get; set; }
        public int Size { get; set; }
        public string Type { get; set; }

        public bool IsFolder { get; set;  }
    }
}
