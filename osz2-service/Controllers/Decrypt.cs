using Microsoft.AspNetCore.Mvc;
using Osz2Decryptor;

namespace osz2_service.Controllers;

public class Decrypt : ControllerBase
{
    [Route("/osz2/decrypt")]
    [HttpPost]
    [Consumes("multipart/form-data")]
    [Produces("application/json")]
    public ActionResult Post([FromForm(Name = "file")] IFormFile file)
    {
        if (file.Length >= int.MaxValue)
            return this.BadRequest("Invalid file size");

        try
        {
            Osz2Package package = new Osz2Package(file.OpenReadStream());
            
            return this.Ok(new Dictionary<string, object> {
                { "metadata", package.Metadata },
                { "files", package.Files }
            });
        }
        catch (Exception e)
        {
            return this.BadRequest(e.Message);
        }
    }
}