using System;
using System.Collections.Generic;

namespace ProofOfConceptServer.entities
{
    public partial class BlobItem
    {
        public int FileId { get; set; }
        public string FileName { get; set; }
        public DateTime Date { get; set; }
        public string PathFile { get; set; }
        public int FileSize { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
    }
}
