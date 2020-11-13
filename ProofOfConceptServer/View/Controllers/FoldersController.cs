﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using Microsoft.AspNetCore.Mvc;
using ProofOfConceptServer.Repositories.entities;
using ProofOfConceptServer.Repositories.entities.helpers;
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
        public ActionResult<List<IGetFolderResponse>> GetFolderContent(int folderId)
        {
            if (!this.handler.DoesFolderExist(folderId))
                return NotFound();
            List<IGetFolderResponse> c = this.handler.GetFolderContent(folderId);
            if(c.Count() == 0)
                return NoContent();
            return c;
        }
    }
}