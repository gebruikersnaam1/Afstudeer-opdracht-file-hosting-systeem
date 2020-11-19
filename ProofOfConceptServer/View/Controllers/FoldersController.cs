using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Web.Http.Cors;
using Microsoft.AspNetCore.Mvc;
using ProofOfConceptServer.entities.interfaces;
using ProofOfConceptServer.Repositories.entities;
using ProofOfConceptServer.Repositories.entities.interfaces;
using ProofOfConceptServer.Repositories.interfaces;
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
        [Authorize]
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
        [Authorize]
        public ActionResult<bool> ValidateFolderID(int folderid)
        {
            if (!this.handler.DoesFolderExist(folderid))
                return NotFound();
            return Ok(true);
        }

        [HttpGet]
        [Route("getFolderContent/{folderid}")]
        [Authorize]
        public ActionResult<List<IFolderContent>> GetFolderContent(int folderId)
        {
            if (!this.handler.DoesFolderExist(folderId))
                return NotFound();
            List<IFolderContent> c = this.handler.GetFolderContent(folderId);
            if (c.Count() == 0)
                return NoContent();
            return c;
        }

        [HttpPost]
        [Route("upload/")]
        [Authorize]
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
        [Authorize]
        public ActionResult<List<IFolderContent>> SearchFiles(string term)
        {
            List<IFolderContent> l = handler.SearchForFiles(term);

            if (l == null)
                return Conflict("No files found");

            return l;
        }

        [HttpGet]
        [Route("folderStructure")]
        [Authorize]
        public ActionResult<IFolderStructure> FolderStructure(){
            return this.handler.GetFolderStructure();
        }

        [HttpGet]
        [Route("getFolder/{folderId}")]
        [Authorize]
        public ActionResult<IFolderWithParent> GetFolder(int folderId)
        {
            if (!this.handler.DoesFolderExist(folderId))
                return Conflict("Wrong Id was given!");
            IFolderWithParent f = this.handler.GetFolderWithParent(folderId);

            if (f == null)
                NotFound();

            return Ok(f);
        }

        [HttpDelete]
        [Route("deleteFolder/{folderId}")]
        [Authorize]
        public ActionResult DeleteFolder(int folderId)
        {
            if (folderId == 1)
                return Conflict("This folder may not be deleted")!;
            if (!this.handler.DeleteFolder(folderId))
                return Conflict("Folders couldn't be deleted");
            return NoContent();
        }

        [HttpPut]
        [Route("changeFolder/")]
        [Authorize]
        public ActionResult<Folder> ChangeFolderName(IChangeFolder changeFolder)
        {
            if(changeFolder.folderId == 1)
                return Conflict("Folder not allowed to change");
            if (!this.handler.DoesFolderExist(changeFolder.folderId))
                return Conflict("Folder doesn't exist");

            Folder f =this.handler.ChangeFolderName(changeFolder);
            if(f == null)
                return Conflict("Something went wrong| Folder may not exist");

            return Ok(f);
        }
    }
}
