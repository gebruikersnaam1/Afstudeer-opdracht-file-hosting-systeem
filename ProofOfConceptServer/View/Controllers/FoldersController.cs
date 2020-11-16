using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using Microsoft.AspNetCore.Mvc;
using ProofOfConceptServer.entities.interfaces;
using ProofOfConceptServer.Repositories.entities;
using ProofOfConceptServer.Repositories.entities.interfaces;
using ProofOfConceptServer.Services.handlers;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProofOfConceptServer.View.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class FoldersController : Controller
    {
        private FolderHandler handler;

        public FoldersController()
        {
            this.handler = new FolderHandler();
        }
        // GET: /<controller>/
        [HttpPost]
        [Route("create")]
        //[Authorize]
        public IActionResult CreateFolder(ICreateFolder data)
        {
            Folder f = this.handler.CreateFolder(data);
            if (f == null)
            {
                return Conflict("Folder couldn't be created");
            }
            return Created("", f);
        }

        [HttpGet]
        [Route("validateid/{folderid}")]
        public ActionResult<bool> ValidateFolderID(int folderid)
        {
            if (!this.handler.DoesFolderExist(folderid))
                return NotFound();
            return Ok(true);
        }

        [HttpGet]
        [Route("getFolder/{folderid}")]
        public ActionResult<List<IFolderResponse>> GetFolderContent(int folderId)
        {
            if (!this.handler.DoesFolderExist(folderId))
                return NotFound();
            List<IFolderResponse> c = this.handler.GetFolderContent(folderId);
            if(c.Count() == 0)
                return NoContent();
            return c;
        }

        [HttpPost]
        [Route("upload/")]
        //[Authorize]
        public IActionResult CreateFolderBlobItem([FromForm] ICreateBlob postData, [FromForm] int folderId)
        {
            if (!this.handler.DoesFolderExist(folderId))
                return NotFound();

            BlobItem b = this.handler.CreateFolderBlobItem(postData, folderId);

            if (b == null)
                return Conflict("Error! Maybe you did not gave us all the needed info?");

            return Created("", b);
        }

        [HttpGet]
        [Route("search/{term}")]
        public ActionResult<List<IFolderResponse>> SearchFiles(string term)
        {
            List<IFolderResponse> l = handler.SearchForFiles(term);

            if (l == null)
                return Conflict("No files found");

            return l;
        }

        [HttpGet]
        [Route("parentFolder/{folderId}")]
        public ActionResult<Folder> GetParentFolder(int folderId)
        {
            if (!this.handler.DoesFolderExist(folderId))
                return NotFound();
            Folder f = this.handler.GetParentFolder(folderId);

            if (f == null)
                NotFound();

            return Ok(f);
        }
    }
}
