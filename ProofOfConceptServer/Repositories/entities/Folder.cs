using System;
using System.Collections.Generic;

namespace ProofOfConceptServer.Repositories.entities
{
    public partial class Folder
    {
        public Folder()
        {
            FolderItems = new HashSet<FolderItems>();
        }

        public int FolderId { get; set; }
        public string FolderName { get; set; }
        public int ParentFolder { get; set; }
        public DateTime DateChanged { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<FolderItems> FolderItems { get; set; }
    }
}
