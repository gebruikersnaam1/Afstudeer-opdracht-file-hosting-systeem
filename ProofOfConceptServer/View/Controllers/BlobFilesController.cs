using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http.Cors;

using Microsoft.AspNetCore.Authorization;

using ProofOfConceptServer.entities.helpers;

using ProofOfConceptServer.entities;
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
        public ActionResult<List<BlobItem>> GetPages([FromQuery]int itemsPerPage, [FromQuery]int currentPage)
        {
            List<BlobItem>  l = handler.GetPages(itemsPerPage,currentPage);

            if (l == null)
                Conflict("Wrong values where given!");
            return Ok(l);
        }

        
        [HttpGet]
        [Route("file/")]
        [Authorize]
        public ActionResult<BlobItem> GetSingleFile([FromQuery]int id)
        {
            BlobItem b = handler.GetSingleFile(id);
            
            if (b == null)
                return NotFound();
         
           return Ok(b);
        }
   
        [HttpPost]
        [Route("upload")]
        [Authorize]
        public IActionResult CreateBlobItem([FromForm] CreateBlob postData )
        {
            BlobItem b = handler.CreateBlobItem(postData);

            if (b == null)
                return Conflict("Error! Maybe you did not gave us all the needed info?");

            return Created("", b);
        }

        [HttpGet]
        [Route("search/{term}")]
        [Authorize]
        public ActionResult<List<BlobItem>> SearchFiles(string term)
        {
            List<BlobItem> l = handler.SearchFiles(term);

            if (l == null)
                return Conflict("No files found");

            return l;
        }

        [HttpPut]
        [Route("update")]
        [Authorize]
        public ActionResult Update(BlobItem newFile)
        {
            bool r = handler.UpdateBlob(newFile);

            if (!r)
                return BadRequest("Couldn't update");
            return Ok();
        }

        [HttpDelete]
        [Route("delete/")]
        [Authorize]
        public ActionResult Delete([FromQuery]int id)
        {
            bool b = handler.Delete(id);

            if (!b)
                Conflict("File couldn't be deleted");
            return NoContent();
        }

       

        [HttpGet]
        [Route("download/assistent/")]
        [Authorize]
        public ActionResult<FileInformation> DownloadFileAssistent([FromQuery]int id)
        {
            return handler.DownloadFileAssistent(id);
        }


        [HttpGet]
        [Route("download/")]
        [Authorize]
        public ActionResult<BlobItem> DownloadFile([FromQuery]int id)
        {
            DownloadFileResponse d = handler.DownloadFile(id);
            if(d == null)
                return Conflict("The wanted file couldn't be found or accessed!");

            return File(d.File, "application/octet-stream", d.FileName);
            
        }
    }
}
