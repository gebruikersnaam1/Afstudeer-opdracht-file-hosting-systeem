using Microsoft.AspNetCore.Authorization;
using System.Web.Http.Cors;
using Microsoft.AspNetCore.Mvc;
using ProofOfConceptServer.Repositories.interfaces;

namespace ProofOfConceptServer.View.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class Shareable : Controller
    {
        [HttpPost]
        [Route("createShareable")]
        [Authorize]
        public ActionResult<bool> ValidateFolderID([FromBody] ICreateSharable postData)
        {
            
            return Ok(true);
        }
    }
}
