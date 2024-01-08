using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;

namespace osz2_service.Controllers;

public class Retrieve : ControllerBase {
    public static ConcurrentDictionary<string, KeyValuePair<string, byte[]>> FileStore = new();
}
