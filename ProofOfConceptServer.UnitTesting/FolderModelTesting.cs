using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProofOfConceptServer.Repositories.entities;
using ProofOfConceptServer.Repositories.entities.interfaces;
using ProofOfConceptServer.Repositories.interfaces;
using ProofOfConceptServer.Repositories.Models;
using System.Collections.Generic;

namespace ProofOfConceptServer.UnitTesting
{
    [TestClass]
    public class FolderModelTesting
    {
        [TestMethod]
        public void GetFolder_FolderExist_Folder()
        {
            //arrange
            FolderModel model = new FolderModel();

            //act
            Folder result = model.GetFolder(1);

            //assert
            Assert.IsInstanceOfType(result, typeof(Folder));
        }

        [TestMethod]
        public void GetFolder_FolderNotExist_Null()
        {
            //arrange
            FolderModel model = new FolderModel();

            //act
            Folder result = model.GetFolder(999999999);

            //assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetFolderContent_FolderHasContent_IFolderContent()
        {
            //arrange
            FolderModel model = new FolderModel();

            //act
            List<IFolderContent> result = model.GetFolderContent(1);

            //assert
            Assert.IsInstanceOfType(result, typeof(List<IFolderContent>));
        }

        [TestMethod]
        public void GetFolderContent_FolderHasNoContent_Empty()
        {
            //arrange
            FolderModel model = new FolderModel();

            //act
            bool result = model.GetFolderContent(999999).Count > 0 ? true : false;

            //assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GetFolderStructure_GetFolderStructure_IFolderStructure()
        {
            //arrange
            FolderModel model = new FolderModel();

            //act
            IFolderStructure result = model.GetFolderStructure();

            //assert
            Assert.IsInstanceOfType(result, typeof(IFolderStructure));
        } 
    }
}
