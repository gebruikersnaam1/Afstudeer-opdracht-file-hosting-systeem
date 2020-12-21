using ProofOfConceptServer.Repositories.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace ProofOfConceptServer.Repositories.entities.Factory
{
    public static class ShareItemInfoFactory
    {
        public static IShareItemInfo Create(ShareItem s, BlobItem b)
        {
            return new IShareItemInfo
            {
                ShareId = s.Id,
                FileName = b.FileName,
                AvailableUntil = s.ActiveUntil
            };
        }
    }
}
