﻿using System;
using System.Collections.Generic;

namespace ProofOfConceptServer.Repositories.entities
{
    public partial class BlobItem
    {
        public BlobItem()
        {
            ShareItems = new HashSet<ShareItem>();
        }

        public int FileId { get; set; }
        public string FileName { get; set; }
        public DateTime Date { get; set; }
        public string Path { get; set; }
        public int FileSize { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }

        public virtual FolderItem FolderItems { get; set; }
        public virtual ICollection<ShareItem> ShareItems { get; set; }

        public object Clone()
        {
            return (BlobItem)this.MemberwiseClone();
        }
    }
}
