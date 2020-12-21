using ProofOfConceptServer.Repositories.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProofOfConceptServer.Repositories.entities.Factory
{
    public static class FolderWithParentFactory
    {
        public static IFolderWithParent Create(Folder f, IFolderWithParent p)
        {
            return new IFolderWithParent
            {
                FolderId = f.FolderId,
                FolderName = f.FolderName,
                ParentFolder = p,
                DateChanged = f.DateChanged,
                CreatedDate = f.CreatedDate
            };

        }
    }
}
