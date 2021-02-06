using BPL3.Model;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Json;

namespace BPL3.Services
{
    class CsvReaderService
    {
        public static readonly string filePathBase = "C:\\Users\\FERNANDODASILVASALGA\\Documents\\repos\\BPL3_Backend\\BPL3\\JSONs\\";
        public CsvReaderService()
        {

        }
        public void ReadTeamFile()
        {
            List<string> _feared = JsonSerializer.Deserialize<List<string>>(File.ReadAllText(filePathBase + "\\The Feared\\TheFearedTeamList.json"));
            List<string> _twisted = JsonSerializer.Deserialize<List<string>>(File.ReadAllText(filePathBase + "\\The Twisted\\TheTwistedTeamList.json"));
            List<string> _formed = JsonSerializer.Deserialize<List<string>>(File.ReadAllText(filePathBase + "\\The Formed\\TheFormedTeamList.json"));
            List<string> _hidden = JsonSerializer.Deserialize<List<string>>(File.ReadAllText(filePathBase + "\\The Hidden\\TheHiddenTeamList.json"));
            List<Member> _members = JsonSerializer.Deserialize<List<Member>>(File.ReadAllText(filePathBase + "\\BPL3Members.json"));

            FileStream fileStream = new FileStream("C:\\Users\\FERNANDODASILVASALGA\\Documents\\repos\\BPL3_Backend\\BPL3\\JSONs\\BPL3.txt", FileMode.Open);
            using (StreamReader reader = new StreamReader(fileStream))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    List<string> lLine = new List<string>();
                    lLine.AddRange(line.Split(","));
                    if (lLine[0] != string.Empty)
                    { 
                        Member m1 = new Member();
                        _hidden.Add(lLine[0]);
                        m1.AccountName = lLine[0];
                        m1.TeamName = "The Hidden";
                        _members.Add(m1);
                    }
                    if (lLine[1] != string.Empty)
                    {
                        Member m2 = new Member();
                        _formed.Add(lLine[1]);
                        m2.AccountName = lLine[1];
                        m2.TeamName = "The Formed";
                        _members.Add(m2);
                    }
                    if (lLine[2] != string.Empty)
                    {
                        Member m3 = new Member();
                        _twisted.Add(lLine[2]);
                        m3.AccountName = lLine[2];
                        m3.TeamName = "The Twisted";
                        _members.Add(m3);
                    }
                    if (lLine[3] != string.Empty)
                    {
                        Member m4 = new Member();
                        _feared.Add(lLine[3]);
                        m4.AccountName = lLine[3];
                        m4.TeamName = "The Feared";
                        _members.Add(m4);
                    }
                }
                SerializeJsonService.SerializeJson(_feared, filePathBase + "\\The Feared\\TheFearedTeamList.json");
                SerializeJsonService.SerializeJson(_twisted, filePathBase + "\\The Twisted\\TheTwistedTeamList.json");
                SerializeJsonService.SerializeJson(_formed, filePathBase + "\\The Formed\\TheFormedTeamList.json");
                SerializeJsonService.SerializeJson(_hidden, filePathBase + "\\The Hidden\\TheHiddenTeamList.json");
                SerializeJsonService.SerializeJson(_members, filePathBase + "\\BPL3Members.json");
            }
        }
    }
}
