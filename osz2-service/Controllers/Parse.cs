using Microsoft.AspNetCore.Mvc;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.Formats;
using osu.Game.IO;

namespace osz2_service.Controllers;

public class Parse : ControllerBase
{
    [Route("/osu/parse")]
    [HttpPost]
    [Consumes("multipart/form-data")]
    [Produces("application/json")]
    public ActionResult Post([FromForm(Name = "osu")] IFormFile osu)
    {
        try
        {
            using Stream stream = osu.OpenReadStream();
            using MemoryStream memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            Beatmap beatmap = ParseBeatmap(memoryStream.ToArray());
            return this.Ok(beatmap.BeatmapInfo);
        }
        catch (Exception e)
        {
            return this.BadRequest(e.Message);
        }
    }

    private Beatmap ParseBeatmap(byte[] data)
    {
        using var reader = new LineBufferedReader(new MemoryStream(data));
        var beatmap = Decoder.GetDecoder<Beatmap>(reader).Decode(reader);
        beatmap.BeatmapInfo.BPM = beatmap.ControlPointInfo.BPMMinimum;
        beatmap.BeatmapInfo.Length = beatmap.CalculatePlayableLength();
        beatmap.BeatmapInfo.MaxCombo = beatmap.GetMaxCombo();
        return beatmap;
    }
}