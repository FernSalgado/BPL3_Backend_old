using AngleSharp;
using BPL3.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BPL3.Services
{
    class ScrapingService
    {
        private IBrowsingContext context { get; set; }

        public ScrapingService()
        {
            var config = Configuration.Default.WithDefaultLoader();
            context = BrowsingContext.New(config);
        }

        public async Task<TeamItem> GetItems(string urlLeader, TeamItem teamItem)
        {
            List<string> _allItems = JsonSerializer.Deserialize<List<string>>(File.ReadAllText("C:\\Users\\FERNANDODASILVASALGA\\source\\repos\\BPL3\\BPL3\\JSONs\\ItemList.json"));
            List<string> _items = new List<string>();
            for (int i = 1; i < 21; i++)
            {
                if (i != 5 && i != 19) 
                {
                    var url = $"{urlLeader}/{i}";
                    var document = await context.OpenAsync(url);
                    var items = document.QuerySelectorAll(".owned");
                    foreach (var item in items)
                    {
                         _items.Add(item.FirstElementChild.FirstElementChild.InnerHtml);
                    }
                }
            }
            var results = _items.Select(i => i).ToList().Intersect(_allItems.Select(i => i).ToList()).ToList();
            results.Add("The Pandemonius");
            foreach (var item in results)
            {
                Item i = teamItem.Items.Where(i => i.Name == item && i.Obtained == "False").FirstOrDefault();
                if (i != null) 
                {
                    i.Obtained = "True";
                    teamItem.Team.Points += 20;
                    var set = teamItem.Items.Where(it => it.SetName == i.SetName && it.Obtained == "False").ToList();
                    if (set.Count == 0)
                        teamItem.Team.Points += 200;
                }
            }
            return teamItem;
        }
    }
}
