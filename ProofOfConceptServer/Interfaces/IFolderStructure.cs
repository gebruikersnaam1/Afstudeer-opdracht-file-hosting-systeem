using ProofOfConceptServer.Repositories.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProofOfConceptServer.Repositories.interfaces
{
    public class IFolderStructure
    {
        public Folder CurrentBranch { get; set;  }
        public List<IFolderStructure> ChildBranches { get; set; }
    }
}
