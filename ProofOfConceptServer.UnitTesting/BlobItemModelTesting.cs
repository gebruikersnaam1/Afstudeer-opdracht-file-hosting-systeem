using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProofOfConceptServer.Repositories.entities;
using ProofOfConceptServer.Repositories.entities.interfaces;
using ProofOfConceptServer.Repositories.interfaces;
using ProofOfConceptServer.Repositories.models;
using ProofOfConceptServer.Repositories.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProofOfConceptServer.UnitTesting
{
    [TestClass]
    public class BlobItemModelTesting
    {
        [TestMethod]
        public void GetSingleFile_FileExit_NotNull()
        {
            //arrange
            BlobItemModel model = new BlobItemModel();

            //act
            BlobItem result = model.GetSingleFile(7);

            //assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetSingleFile_FileNotExits_Null()
        {
            //arrange
            BlobItemModel model = new BlobItemModel();

            //act
            BlobItem result = model.GetSingleFile(99999999);

            //assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetSingleFile_FileExits_BlobItem()
        {
            //arrange
            BlobItemModel model = new BlobItemModel();

            //act
            BlobItem result = model.GetSingleFile(7);

            //assert
            Assert.IsInstanceOfType(result, typeof(BlobItem));
        }

    }
}
