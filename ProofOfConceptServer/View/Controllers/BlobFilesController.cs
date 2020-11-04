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
using ProofOfConceptServer.Services.Handlers;
using ProofOfConceptServer.Repositories.entities.helpers;

namespace ProofOfConceptServer.Controllers
{
    

    [ApiController]
    [Route("api/[controller]")]
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class BlobFilesController : ControllerBase
    {

        private BlobFileHandler handler;

        public BlobFilesController()
        {
            this.handler = new BlobFileHandler();
        }

        [HttpGet]
        [Route("count")]
        [Authorize]
        public ActionResult<int> GetCountOfRows()
        {
            return this.handler.GetCountOfRows();
        }

        [HttpGet]
        [Authorize]
        public ActionResult<List<BlobEntity>> GetPages([FromQuery]int itemsPerPage, [FromQuery]int currentPage)
        {
            List<BlobEntity>  l = handler.GetPages(itemsPerPage,currentPage);

            if (l == null)
                Conflict("Wrong values where given!");
            return Ok(l);
        }

        
        [HttpGet]
        [Route("file/{term}")]
        [Authorize]
        public ActionResult<BlobEntity> GetSingleFile(string term)
        {
            BlobEntity b = handler.GetSingleFile(term);
            
            if (b == null)
                return NotFound();
         
           return Ok(b);
        }
   
        [HttpPost]
        [Route("upload")]
        [Authorize]
        public IActionResult CreateBlobItem([FromForm] CreateBlob postData )
        {
            BlobEntity b = handler.CreateBlobItem(postData);

            if (b == null)
                return Conflict("Error! Maybe you did not gave us all the needed info?");

            return Created("", b);
        }

        [HttpGet]
        [Route("search/{term}")]
        [Authorize]
        public ActionResult<List<BlobEntity>> SearchFiles(string term)
        {
            List<BlobEntity> l = handler.SearchFiles(term);

            if (l == null)
                return Conflict("No files found");

            return l;
        }

        [HttpPut]
        [Route("update")]
        [Authorize]
        public ActionResult Update(BlobEntity newFile)
        {
            bool r = handler.UpdateBlob(newFile);

            if (!r)
                return BadRequest("Couldn't update");
            return Ok();
        }

        [HttpDelete]
        [Route("delete/{id}")]
        [Authorize]
        public ActionResult Delete(string id)
        {
            bool b = handler.Delete(id);

            if (!b)
                Conflict("File couldn't be deleted");
            return NoContent();
        }

       

        [HttpGet]
        [Route("download/assistent/{id}")]
        [Authorize]
        public ActionResult<FileInformation> DownloadFileAssistent(string id)
        {
            return handler.DownloadFileAssistent(id);
        }


        [HttpGet]
        [Route("download/{id}")]
        [Authorize]
        public ActionResult<BlobEntity> DownloadFile(string id)
        {
            DownloadFileResponse d = handler.DownloadFile(id);
            if(d == null)
                return Conflict("The wanted file couldn't be found or accessed!");

            return File(d.File, "application/octet-stream", d.FileName);
            
        }
    }
}
