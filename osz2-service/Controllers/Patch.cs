using Microsoft.AspNetCore.Mvc;
using OsuParsers.Decoders;

namespace osz2_service.Controllers;

public class Patch : ControllerBase
{
    [Route("/osz2/patch")]
    [HttpPost]
    [Consumes("multipart/form-data")]
    [Produces("application/json")]
    public ActionResult Post([FromForm(Name = "file")] IFormFile file)
    {
        // TODO: Implement patch files
        return this.Ok();
    }
}