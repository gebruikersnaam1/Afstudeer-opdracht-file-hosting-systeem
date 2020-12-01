using ProofOfConceptServer.Repositories.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProofOfConceptServer.Repositories.entities.Factory
{
    public static class SynchronicFilesFactory
    {
        public static ISynchronicFiles Create(DateTime date, string name, long fileSize)
        {
            return new ISynchronicFiles
            {
                CreatedOn = date,
                FileName = name,
                FileSize = (int)fileSize / 1024
            };
        }
    }
}
