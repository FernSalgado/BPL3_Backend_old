using BPL3.Model;
using BPL3.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BPL3
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();

        public static readonly string filePathBase = "C:\\Users\\FERNANDODASILVASALGA\\Documents\\repos\\BPL3_Backend\\BPL3\\JSONs\\";
        public static readonly string filePathTheFormed = "The Formed\\TheFormed.json";
        public static readonly string filePathTheFormedItems = "The Formed\\TheFormedItems.json";
        public static readonly string filePathTheTwisted = "The Twisted\\TheTwisted.json";
        public static readonly string filePathTheTwistedItems = "The Twisted\\TheTwistedItems.json";
        public static readonly string filePathTheHidden = "The Hidden\\TheHidden.json";
        public static readonly string filePathTheHiddenItems = "The Hidden\\TheHiddenItems.json";
        public static readonly string filePathTheFeared = "The Feared\\TheFeared.json";
        public static readonly string filePathTheFearedItems = "The Feared\\TheFearedItems.json";
        static async Task Main(string[] args)
        {
            //CsvReaderService _csvReader = new CsvReaderService();
            //_csvReader.ReadTeamFile();
            LadderService _ladder = new LadderService();
            List<Team> teams = new List<Team>();
            ScrapingService _scrapper = new ScrapingService();
            Team theFormed = JsonSerializer.Deserialize<Team>(File.ReadAllText(filePathBase + filePathTheFormed));
            Team theTwisted = JsonSerializer.Deserialize<Team>(File.ReadAllText(filePathBase + filePathTheTwisted));
            Team theFeared = JsonSerializer.Deserialize<Team>(File.ReadAllText(filePathBase + filePathTheFeared));
            Team theHidden = JsonSerializer.Deserialize<Team>(File.ReadAllText(filePathBase + filePathTheHidden));
            List<Item> theFormedItems = JsonSerializer.Deserialize<List<Item>>(File.ReadAllText(filePathBase + filePathTheFormedItems));
            List<Item> theTwistedItems = JsonSerializer.Deserialize<List<Item>>(File.ReadAllText(filePathBase + filePathTheTwistedItems));
            List<Item> theFearedItems = JsonSerializer.Deserialize<List<Item>>(File.ReadAllText(filePathBase + filePathTheFearedItems));
            List<Item> theHiddenTeams = JsonSerializer.Deserialize<List<Item>>(File.ReadAllText(filePathBase + filePathTheHiddenItems));
            List<Member> members = JsonSerializer.Deserialize<List<Member>>(File.ReadAllText(filePathBase + "\\BPL3Members.json"));
            
            
            List<Member> updateMember = await GetLadder(members, new List<Team> { theFormed, theTwisted , theFeared, theHidden});
            SerializeJsonService.SerializeJson(updateMember, filePathBase + "\\BPL3Members.json");
            TeamItem updateFormed = await _scrapper.GetItems(theFormed.StashUrl, new TeamItem { Team = theFormed, Items = theFormedItems });
            TeamItem updateFearedItems = await _scrapper.GetItems(theFeared.StashUrl, new TeamItem { Team = theFeared, Items = theFearedItems });
            TeamItem updateTwistedItems = await _scrapper.GetItems(theTwisted.StashUrl, new TeamItem { Team = theTwisted, Items = theTwistedItems });
            TeamItem updateHiddenItems = await _scrapper.GetItems(theHidden.StashUrl, new TeamItem { Team = theHidden, Items = theHiddenTeams });

            List<Member> theTwistedMembers = members.Where(m => m.TeamName == "The Twisted").ToList();
            List<Member> theFearedMembers = members.Where(m => m.TeamName == "The Feared").ToList();
            List<Member> theHiddenMembers = members.Where(m => m.TeamName == "The Hidden").ToList();
            List<Member> theFormedMembers = members.Where(m => m.TeamName == "The Formed").ToList();

            List<int> points = new List<int>();
            points.AddRange(CalcPoints(theTwistedMembers));
            points.AddRange(CalcPoints(theFearedMembers));
            points.AddRange(CalcPoints(theHiddenMembers));
            points.AddRange(CalcPoints(theFormedMembers));
            theTwisted.LevelPoints = points[0];
            theTwisted.DelvePoints = points[1];
            theTwisted.TotalPoints = theTwisted.LevelPoints + theTwisted.DelvePoints + theTwisted.SetPoints;
            theFeared.LevelPoints = points[2];
            theFeared.DelvePoints = points[3];
            theFeared.TotalPoints = theFeared.LevelPoints + theFeared.DelvePoints + theFeared.SetPoints;
            theHidden.LevelPoints = points[4];
            theHidden.DelvePoints = points[5];
            theHidden.TotalPoints = theHidden.LevelPoints + theHidden.DelvePoints + theHidden.SetPoints;
            theFormed.LevelPoints = points[6];
            theFormed.DelvePoints = points[7];
            theFormed.TotalPoints = theFormed.LevelPoints + theFormed.DelvePoints + theFormed.SetPoints;

            SerializeJsonService.SerializeJson(updateFormed.Team, filePathBase + filePathTheFormed);
            SerializeJsonService.SerializeJson(updateFormed.Items, filePathBase + filePathTheFormedItems);
            SerializeJsonService.SerializeJson(updateHiddenItems.Team, filePathBase + filePathTheHidden);
            SerializeJsonService.SerializeJson(updateHiddenItems.Items, filePathBase + filePathTheHiddenItems);
            SerializeJsonService.SerializeJson(updateTwistedItems.Team, filePathBase + filePathTheTwisted);
            SerializeJsonService.SerializeJson(updateTwistedItems.Items, filePathBase + filePathTheTwistedItems);
            SerializeJsonService.SerializeJson(updateFearedItems.Team, filePathBase + filePathTheFeared);
            SerializeJsonService.SerializeJson(updateFearedItems.Items, filePathBase + filePathTheFearedItems);
        }


        private static async Task<List<Member>> GetLadder(List<Member> members, List<Team> teams)
        {
            var total = 872;
            var count = 0;
            do
            {
                client.DefaultRequestHeaders.Accept.Clear();
                var url = ($"https://www.pathofexile.com/api/ladders?offset={count}&limit=200&id=Cops+VS+Robbers+(PL12168)&type=league&realm=pc&_=1612548934164");
                var streamTask = client.GetStringAsync(url);
                var aaa = streamTask.Result;
                aaa = aaa.Replace("\"class\"", "\"Class\"");
                dynamic json = Newtonsoft.Json.JsonConvert.DeserializeObject(aaa);
                foreach (var item in json.entries)
                {
                    string name = item.account.name;
                    Member m = members.Where(m => m.AccountName == name).FirstOrDefault();
                    if (m != null) 
                    {
                        var team = teams.Where(t => t.Name == m.TeamName).FirstOrDefault();
                        if (m.CharacterName == null)
                        {
                            m.CharacterName = item.character.name;
                            string c = item.character.Class;
                            m.Class = IsClassValid(team.AllowedClasses,c ) ? c : $"Invalid Class ({item.character.Class})";
                            m.Rank = item.rank;
                        }
                        else 
                        {
                            if (m.CharacterName != item.character.name.ToString()) 
                            {
                                m.CharacterName = item.character.level > m.Level ? item.character.name : m.CharacterName;
                                if (m.CharacterName != item.character.name.ToString()) continue;
                            }
                            m.Class = IsClassValid(team.AllowedClasses, item.character.Class.ToString()) ? item.character.Class : $"Invalid Class ({item.character.Class})";
                        }
                        m.Level = item.character.level;
                        m.Rank = item.rank;
                        m.Delve = item.character.depth != null ? item.character.depth.solo : 0;
                    }
                }
                count += 200;
            } while (count < total);
            return members;
        }

        public static bool IsClassValid(List<string> classes, string Class) 
        {
            try
            {
                return classes.Contains(Class);
            }
            catch (Exception e) {
                return false;
            }
        }
        private static List<int> CalcPoints(List<Member> members)
        {
            List<int> points = new List<int>();
            points.Add(0);
            points.Add(0);
            foreach (var item in members)
            {
                points[0] += CalcTeamLevelPoints(item.Level);
                points[1] += CalcTeamDelvePoints(item.Delve / 10);
            }
            return points;
        }
        private static int CalcTeamLevelPoints(int level)
        {
            return level;
        }
        private static int CalcTeamDelvePoints(int delve)
        {
            return delve / 10;
        }
    }
}
