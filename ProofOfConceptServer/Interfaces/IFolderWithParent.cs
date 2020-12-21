using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProofOfConceptServer.Repositories.interfaces
{
    public class IFolderWithParent
    {
        public int FolderId { get; set; }
        public string FolderName { get; set; }
        public IFolderWithParent ParentFolder { get; set; }
        public DateTime DateChanged { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
