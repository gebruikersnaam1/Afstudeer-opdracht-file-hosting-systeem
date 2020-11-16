using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProofOfConceptServer.Repositories.entities.interfaces
{
    public class ICreateFolder
    {
        public string folderName { get; set; }
        public int parentID { get; set; }

    }
}
