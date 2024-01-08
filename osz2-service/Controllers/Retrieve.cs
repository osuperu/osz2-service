using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;

namespace osz2_service.Controllers;

public class Retrieve : ControllerBase {
    public static ConcurrentDictionary<string, KeyValuePair<string, byte[]>> FileStore = new();

    [Route("/osz2/receive")]
    [HttpGet]
    public ActionResult Get([FromQuery(Name = "ticket")] string ticket) {
        if (ticket.Length > 32) {
            return this.BadRequest("Invalid request.");
        }

        bool found = FileStore.TryGetValue(ticket, out KeyValuePair<string, byte[]> ticketFile);

        if (!found) {
            return this.NotFound($"Could not find file for ticket: {ticket}");
        }

        bool removeSuccess = FileStore.TryRemove(ticket, out _);

        if (!removeSuccess) {
            Console.WriteLine($"Failed to remove ticket: {ticket}");
        } else {
            Console.WriteLine($"Receive ticket burned: {ticket} - {ticketFile.Key}");
        }
        
        return this.File(ticketFile.Value, "waffle/osz2", ticketFile.Key);
    }
}
