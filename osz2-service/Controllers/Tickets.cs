using System.Collections.Concurrent;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace osz2_service.Controllers;

public class Tickets : ControllerBase {
    public static ConcurrentDictionary<string, KeyValuePair<string, byte[]>> FileStore = new();

    [Route("/osz2/receive")]
    [HttpGet]
    public ActionResult Receive([FromQuery(Name = "ticket")] string ticket) {
        bool found = FileStore.TryGetValue(ticket, out KeyValuePair<string, byte[]> ticketFile);

        if (!found) {
            return this.NotFound($"Could not find file for ticket: {ticket}");
        }

        bool removeSuccess = FileStore.TryRemove(ticket, out _);

        if (!removeSuccess) {
            Console.WriteLine($"Failed to remove ticket: {ticket}");
        } else {
            Console.WriteLine($"Receive ticket burned: {ticket}");
        }

        return this.File(ticketFile.Value, "waffle/osz2", ticketFile.Key);
    }

    [Route("/osz2/burn")]
    [HttpGet]
    public ActionResult BurnTicket([FromQuery(Name = "ticket")] string ticket) {
        if (!FileStore.ContainsKey(ticket)) {
            return this.Ok();
        }

        bool removeSuccess = FileStore.TryRemove(ticket, out _);

        if (!removeSuccess) {
            Console.WriteLine($"Failed to remove ticket: {ticket}");
        } else {
            Console.WriteLine($"Receive ticket burned: {ticket}");
        }

        return this.Ok();
    }
}
