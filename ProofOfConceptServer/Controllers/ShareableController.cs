using Microsoft.AspNetCore.Authorization;
using System.Web.Http.Cors;
using Microsoft.AspNetCore.Mvc;
using ProofOfConceptServer.Repositories.interfaces;
using ProofOfConceptServer.Services.handlers;
using ProofOfConceptServer.Repositories.entities;
using ProofOfConceptServer.entities.interfaces;
using ProofOfConceptServer.Repositories.entities.interfaces;

namespace ProofOfConceptServer.View.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class ShareableController : Controller
    {


        private ShareableHandler handler;

        public ShareableController()
        {
            this.handler = new ShareableHandler();
        }


        [HttpPost]
        [Route("createShareable")]
        [Authorize]
        public ActionResult<ShareItem> Create([FromBody] ICreateSharable postData)
        {
            ShareItem s = this.handler.Create(postData);
            if(s == null)
                return Conflict("Something went wrong");
            return Created("",s);
        }

        [HttpGet]
        [Route("getShareItem/{id}")]
        public ActionResult<IShareItemInfo> ShareItemInfo(int id)
        {
            IShareItemInfo s = this.handler.ShareItemInfo(id);
            if (s == null)
                return Conflict("Something went wrong");
            return Ok(s);
        }

        [HttpGet]
        [Route("download/assistent/{id}")]
        public ActionResult<IFileInformation> DownloadFileAssistent(int id)
        {
            return handler.DownloadFileAssistent(id);
        }


        [HttpGet]
        [Route("download/{id}")]
        public ActionResult<BlobItem> DownloadFile(int id)
        {
            IDownloadFileResponse d = handler.DownloadFile(id);
            if (d == null)
                return Conflict("The wanted file couldn't be found or accessed!");

            return File(d.File, "application/octet-stream", d.FileName);
        }
    }
}
