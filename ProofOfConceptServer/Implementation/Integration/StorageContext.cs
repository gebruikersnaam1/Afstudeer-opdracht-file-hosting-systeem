using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProofOfConceptServer.Repositories.Models
{
    public static class StorageContext 
    {
        private static string _Environment = "Azure";

        public static string[] Environments = { "Azure", "Amazon" };

        public static string Environment
        {
            get
            {
                return _Environment;
            }
            set
            {
                if (value == Environments[0] || value == Environments[1])
                {
                    _Environment = value;
                }
            }
        }
    }
}
