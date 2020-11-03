using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProofOfConceptServer.database
{
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;

    public static class AzureConnection
    {
        public static CloudBlobContainer Container { get; private set; }

        public static void CreateContainerConnections(string connectionString)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            AzureConnection.Container = blobClient.GetContainerReference("blobfiles");
        }
    }
}
