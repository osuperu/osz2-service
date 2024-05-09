using Microsoft.AspNetCore.Mvc;
using OsuParsers.Decoders;

namespace osz2_service.Controllers;

public class Patch : ControllerBase
{
    [Route("/osz2/patch")]
    [HttpPost]
    [Consumes("multipart/form-data")]
    [Produces("application/json")]
    public ActionResult Post([FromForm(Name = "osz2")] IFormFile osz2, [FromForm(Name = "patch")] IFormFile patch)
    {
        using var newData = new MemoryStream();
        PatchDecryptor patcher = new PatchDecryptor();
        
        patcher.Patch(osz2.OpenReadStream(), newData, patch.OpenReadStream(), 0);
        return File(newData.ToArray(), "application/octet-stream", "osz2");
    }
}