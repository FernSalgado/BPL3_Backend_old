using BPL3.Model;
using BPL3.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BPL3
{
    class Program
    {
        public const string filePathTheFormed = "C:\\Users\\FERNANDODASILVASALGA\\source\\repos\\BPL3\\BPL3\\JSONs\\TheFormed.json";
        public const string filePathTheFormedItems = "C:\\Users\\FERNANDODASILVASALGA\\source\\repos\\BPL3\\BPL3\\JSONs\\TheFormedItems.json";
        public const string filePathTheTwisted = "C:\\Users\\FERNANDODASILVASALGA\\source\\repos\\BPL3\\BPL3\\JSONs\\TheTwisted.json";
        public const string filePathTheTwistedItems = "C:\\Users\\FERNANDODASILVASALGA\\source\\repos\\BPL3\\BPL3\\JSONs\\TheTwistedItems.json";
        public const string filePathTheHidden = "C:\\Users\\FERNANDODASILVASALGA\\source\\repos\\BPL3\\BPL3\\JSONs\\TheHidden.json";
        public const string filePathTheHiddenItems = "C:\\Users\\FERNANDODASILVASALGA\\source\\repos\\BPL3\\BPL3\\JSONs\\TheHiddenItems.json";
        public const string filePathTheFeared = "C:\\Users\\FERNANDODASILVASALGA\\source\\repos\\BPL3\\BPL3\\JSONs\\TheFeared.json";
        public const string filePathTheFearedItems = "C:\\Users\\FERNANDODASILVASALGA\\source\\repos\\BPL3\\BPL3\\JSONs\\TheFearedItems.json";
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            ScrapingService _scrapper = new ScrapingService();
            Team theFormed = JsonSerializer.Deserialize<Team>(File.ReadAllText(filePathTheFormed)); 
            Team theTwisted = JsonSerializer.Deserialize<Team>(File.ReadAllText(filePathTheTwisted)); 
            Team TheFeared = JsonSerializer.Deserialize<Team>(File.ReadAllText(filePathTheFeared)); 
            Team TheHidden = JsonSerializer.Deserialize<Team>(File.ReadAllText(filePathTheHidden)); 
            List<Item> theFormedItems = JsonSerializer.Deserialize<List<Item>>(File.ReadAllText(filePathTheFormedItems));
            List<Item> theTwistedItems = JsonSerializer.Deserialize<List<Item>>(File.ReadAllText(filePathTheTwistedItems));
            List<Item> theFearedItems = JsonSerializer.Deserialize<List<Item>>(File.ReadAllText(filePathTheFearedItems));
            List<Item> theHiddenTeams = JsonSerializer.Deserialize<List<Item>>(File.ReadAllText(filePathTheHiddenItems));

            TeamItem updateFormed = await _scrapper.GetItems(theFormed.StashUrl, new TeamItem { Team = theFormed, Items = theFormedItems });
            SerializeJson(updateFormed.Items, filePathTheFormedItems);
            SerializeJson(updateFormed.Team, filePathTheFormed);
            //TeamItem updateTwistedItems = await _scrapper.GetItems(theTwisted.StashUrl, new TeamItem { Team = theTwisted, Items = theTwistedItems });
            //SerializeJson(updateTwistedItems.Items, filePathTheTwistedItems);
            //SerializeJson(updateTwistedItems.Team, filePathTheTwisted);
            //TeamItem updateFearedItems = await _scrapper.GetItems(TheFeared.StashUrl, new TeamItem { Team = TheFeared, Items = theFearedItems });
            //SerializeJson(updateFearedItems.Items, filePathTheFearedItems);
            //SerializeJson(updateFearedItems.Team, filePathTheFeared);
            //TeamItem updateHiddenItems = await _scrapper.GetItems(TheHidden.StashUrl, new TeamItem { Team = TheHidden, Items = theHiddenTeams });
            //SerializeJson(updateHiddenItems.Items, filePathTheHiddenItems);
            //SerializeJson(updateHiddenItems.Team, filePathTheHidden);
        }
        public static void SerializeJson(dynamic model, string filePath) 
        {
            var jsonUtf8Bytes= "";
            jsonUtf8Bytes = JsonSerializer.Serialize(model, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
            File.WriteAllText(filePath, jsonUtf8Bytes);
        }


    }
}
