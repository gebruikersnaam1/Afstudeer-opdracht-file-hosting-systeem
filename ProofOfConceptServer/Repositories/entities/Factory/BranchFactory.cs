using ProofOfConceptServer.Repositories.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProofOfConceptServer.Repositories.entities.Factory
{
    public static class BranchFactory
    {
        public static IFolderStructure Create(Folder f)
        {
            return new IFolderStructure
            {
                CurrentBranch = f,
                ChildBranches = new List<IFolderStructure>(),
            };
        }
    }
}
