using Microsoft.AspNetCore.Mvc;
using Osz2Decryptor;
using Osz2Decryptor.Utilities;

namespace osz2_service.Controllers;

public class Send : ControllerBase {
    private struct ReceiveTicketResult {
        public List<string> RetrieveTickets;
    }

    [Route("/osz2/send")]
    [HttpPost]
    public ActionResult Post([FromForm(Name = "file")] IFormFile file) {
        List<string> retrieveTickets = new();

        if (file.Length >= int.MaxValue) {
            return this.BadRequest("File size invalid.");
        }

        try {
            Osz2Package package = new Osz2Package(file.OpenReadStream());

            foreach (KeyValuePair<string,byte[]> oszFile in package.Files) {
                string retreiveTicket = CryptoUtilities.ComputeHash($"{DateTime.Now}-{oszFile.Key}");

                bool addSuccess = Retrieve.FileStore.TryAdd(retreiveTicket, oszFile);

                if (!addSuccess) {
                    return StatusCode(500, "Could not generate retrieval ticket.");
                }

                retrieveTickets.Add(retreiveTicket);

                Console.WriteLine($"New Ticket: {retreiveTicket} for {oszFile.Key}");
            }

            return this.Content(string.Join('-', retrieveTickets));
        }
        catch (Exception e) {
            return StatusCode(500, "Parsing osz2 failed.");
        }
    }
}
