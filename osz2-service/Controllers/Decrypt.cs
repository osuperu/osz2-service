using Microsoft.AspNetCore.Mvc;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.Formats;
using osu.Game.IO;
using Osz2Decryptor;

namespace osz2_service.Controllers;

public class Decrypt : ControllerBase
{
    [Route("/osz2/decrypt")]
    [HttpPost]
    [Consumes("multipart/form-data")]
    [Produces("application/json")]
    public ActionResult Post([FromForm(Name = "osz2")] IFormFile osz2)
    {
        if (osz2.Length >= int.MaxValue)
            return this.BadRequest("Invalid file size");

        try
        {
            Osz2Package package = new Osz2Package(osz2.OpenReadStream());

            var beatmaps = package.Files
                .Where(item => item.Key.EndsWith(".osu"))
                .ToDictionary(
                    item => item.Key,
                    item => ParseBeatmap(item.Value).BeatmapInfo
                );

            var files = RemoveUnusedFiles(package.Files);

            return this.Ok(new Dictionary<string, object> {
                { "metadata", package.Metadata },
                { "beatmaps", beatmaps },
                { "files", files }
            });
        }
        catch (Exception e)
        {
            return this.BadRequest(e.Message);
        }
    }

    private Beatmap ParseBeatmap(byte[] data)
    {
        LineBufferedReader reader = new LineBufferedReader(new MemoryStream(data));
        Beatmap beatmap = Decoder.GetDecoder<Beatmap>(reader).Decode(reader);
        beatmap.BeatmapInfo.BPM = beatmap.ControlPointInfo.BPMMinimum;
        beatmap.BeatmapInfo.Length = beatmap.CalculatePlayableLength();
        beatmap.BeatmapInfo.MaxCombo = beatmap.GetMaxCombo();
        return beatmap;
    }

    private Dictionary<string, byte[]> RemoveUnusedFiles(Dictionary<string, byte[]> files)
    {
        var validFileExtensions = new HashSet<string> {
            ".osu", ".osz", ".osb", ".osk", ".png", ".mp3", ".jpeg",
            ".wav", ".png", ".wav", ".ogg", ".jpg", ".wmv", ".flv",
            ".mp3", ".flac", ".mp4", ".avi", ".ini", ".jpg", ".m4v"
        };

        return files
            .Where(item => validFileExtensions.Contains(Path.GetExtension(item.Key.ToLower())))
            .ToDictionary(item => item.Key, item => item.Value);
    }
}