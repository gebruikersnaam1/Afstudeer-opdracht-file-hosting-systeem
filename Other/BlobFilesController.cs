using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http.Cors;

using Microsoft.AspNetCore.Authorization;
using ProofOfConceptServer.entities.Factory;

using ProofOfConceptServer.entities.helpers;

using ProofOfConceptServer.database;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using Azure.Storage.Blobs.Models;
using ProofOfConceptServer.entities;
using ProofOfConceptServer.entities.dummy_data;


namespace ProofOfConceptServer.Controllers
{
    

    [ApiController]
    [Route("api/[controller]")]
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class BlobFilesController : ControllerBase
    {

        private static List<BlobEntity> FilesStorage = DummyDataBlobfiles.GetDummyData();
        private string blobItemsPath;

        public BlobFilesController()
        {
            //create filepath
            blobItemsPath = Path.Combine(Startup.apiRoot, "Models/uploads");
        }

        [HttpGet]
        [Route("count")]
        [Authorize]
        public ActionResult<List<BlobItem>> GetCountOfRows()
        {
            return Ok(FilesStorage.Count());
        }

        [HttpGet]
        [Authorize]
        public ActionResult<List<BlobEntity>> GetPages([FromQuery]int itemsPerPage, [FromQuery]int currentPage)
        {
            if(itemsPerPage < 1 || currentPage <= 0)
                Conflict("Wrong values where given!");
            
            IEnumerable<BlobEntity> sorted = FilesStorage.Skip((itemsPerPage * currentPage)).Take(itemsPerPage);
            List<BlobEntity> newList = sorted.ToList();

            if(newList.Count() == 0)
                NotFound("No values where found");

            return Ok(newList);
        }

        
        [HttpGet]
        [Route("file/{term}")]
        [Authorize]
        public ActionResult<BlobEntity> GetSingleFile(string term)
        {
            var blobItem = FilesStorage.Find(item =>
                    item.fileId.Equals(term, StringComparison.InvariantCultureIgnoreCase));

            if (blobItem == null)
                return NotFound();
         
           return Ok(blobItem);
        }

        private int GenerateId()
        {
            //basic ID -- auto increment, but with a bit more safety
            int id = FilesStorage.Count();
            bool generatedId = false;
            while (!generatedId)
            {
                id += 1;
                if(FilesStorage.Find(i => i.fileId == id.ToString()) == null)
                    generatedId = true;
            }
            return id;
        }
   
        [HttpPost]
        [Route("upload")]
        [Authorize]
        public async Task<IActionResult> CreateBlobItem([FromForm] CreateBlob postData )
        {

            string id = this.GenerateId().ToString();
            BlobEntity blobItem = BlobItemFactory.Create(postData, id, blobItemsPath);

            if (blobItem == null)
                return Conflict("Error! Maybe you did not gave us all the needed info?");

            string fileName = Path.GetFileName(blobItem.pathFile);

            CloudBlockBlob blockBob = AzureConnection.Container.GetBlockBlobReference(fileName);
            using (Stream fileStream = postData.file.OpenReadStream())
            {
                await blockBob.UploadFromStreamAsync(fileStream);
                FilesStorage.Add(blobItem); //set file in the "db"
            }

            return Created("", blobItem);
        }

        [HttpGet]
        [Route("search/{term}")]
        [Authorize]
        public ActionResult<List<BlobEntity>> SearchFiles(string term)
        {
            if (term == "" || term == null)
                return Conflict("No value given");

            return Ok(FilesStorage.Where(file =>
                    file.fileName.ToLower().Contains(term.ToLower())
                ).ToList());
        }

        [HttpPut]
        [Route("update")]
        [Authorize]
        public ActionResult Update(BlobEntity newFile)
        {
            var oldFile = FilesStorage.Find(
                item => item.fileId.Equals
                            (newFile.fileId, StringComparison.InvariantCultureIgnoreCase));

            if (oldFile == null)
                return BadRequest("Cannot update as blob doesn't exist!");

            oldFile.fileName = newFile.fileName;
            oldFile.description = newFile.description;
            return Ok();
        }

        [HttpDelete]
        [Route("delete/{id}")]
        [Authorize]
        public async Task<ActionResult> Delete(string id)
        {
            var blobItem = FilesStorage.Find(item =>
                   item.fileId.Equals(id, StringComparison.InvariantCultureIgnoreCase));

            if (blobItem == null)
                return NotFound();

            try{
                    CloudBlockBlob blockBob = AzureConnection.Container.GetBlockBlobReference(blobItem.fileName);
                    await blockBob.DeleteIfExistsAsync();
                    FilesStorage.Remove(blobItem);
                    return NoContent();
                }
            catch (ArgumentException e)
                {
                    return Conflict("File delete went wrong. Error: " + e);
                }
            }

       

        [HttpGet]
        [Route("download/assistent/{id}")]
        [Authorize]
        public ActionResult<FileInformation> DownloadFileAssistent(string id)
        {
            var blobItem = FilesStorage.Find(item =>
                    item.fileId.Equals(id, StringComparison.InvariantCultureIgnoreCase));
            string extension = Path.GetExtension(blobItem.pathFile);

            return new FileInformation
            {
                fileName = Path.GetFileNameWithoutExtension(blobItem.fileName),
                extension = extension
            };
        }

        private async Task DownloadBlobFileToServer(BlobEntity blobItem)
        {
            try{
                string fileName = Path.GetFileName(blobItem.pathFile);
                CloudBlockBlob blockBob = AzureConnection.Container.GetBlockBlobReference(fileName);
                var rootDir = new FileInfo(blobItem.pathFile).Directory;
                if (!rootDir.Exists) //make sure the parent directory exists
                    rootDir.Create();

                await blockBob.DownloadToFileAsync(blobItem.pathFile, FileMode.Create);
            }
            catch{
                System.Diagnostics.Debug.WriteLine("File couldn't be downloaded from the Azure!");
            }         
        }

        [HttpGet]
        [Route("download/{id}")]
        [Authorize]
        public async Task<ActionResult<BlobEntity>> DownloadFile(string id)
        {
            BlobEntity blobItem = FilesStorage.Find(item =>
                    item.fileId.Equals(id, StringComparison.InvariantCultureIgnoreCase));

            if (blobItem == null)
                return NotFound();

            //download the file to the local server
            await DownloadBlobFileToServer(blobItem);

            var net = new System.Net.WebClient();
            try
            {
                var data = net.DownloadData(blobItem.pathFile);
                var content = new System.IO.MemoryStream(data);
                
                if (System.IO.File.Exists(blobItem.pathFile))
                    System.IO.File.Delete(blobItem.pathFile);
                
                return File(data, "application/octet-stream", blobItem.fileName);
            }
            catch
            {
                return Conflict("The wanted file couldn't be found or accessed!");
            }
        }
    }
}
