using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProofOfConceptServer.database
{
    using Azure.Storage.Blobs;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;

    public static class AzureConnection
    {
        public static CloudBlobContainer Container { get; private set; }
        public static BlobContainerClient containerClient { get; private set; }

        private static string ContainerName { get { return "blobfiles";  }  }

        public static void CreateConnections(string connectionString)
        {
            SetCloudStorage(connectionString);
            SetBlobServiceClient(connectionString);
        }

        private static void SetCloudStorage(string connectionString)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient BlobClient = storageAccount.CreateCloudBlobClient();
            AzureConnection.Container = BlobClient.GetContainerReference(ContainerName);
        }

        private static void SetBlobServiceClient(string connectionString)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            containerClient = blobServiceClient.GetBlobContainerClient(ContainerName);
        }
    }
}
