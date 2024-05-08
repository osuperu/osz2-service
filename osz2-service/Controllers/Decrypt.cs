using Microsoft.AspNetCore.Mvc;
using OsuParsers.Decoders;
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

            var beatmaps = package.Files
                .Where(item => item.Key.EndsWith(".osu"))
                .ToDictionary(
                    item => item.Key,
                    item => BeatmapDecoder.Decode(System.Text.Encoding.UTF8.GetString(item.Value).Split("\n"))
                );

            // TODO: Validate files (audio, images, etc..)
            return this.Ok(new Dictionary<string, object> {
                { "metadata", package.Metadata },
                { "beatmaps", beatmaps },
                { "files", package.Files }
            });
        }
        catch (Exception e)
        {
            return this.BadRequest(e.Message);
        }
    }
}