using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace BPL3.Services
{
    public class SerializeJsonService
    {
        public static void SerializeJson(dynamic model, string filePath)
        {
            var jsonUtf8Bytes = "";
            jsonUtf8Bytes = JsonSerializer.Serialize(model, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
            File.WriteAllText(filePath, jsonUtf8Bytes);
        }
    }
}
