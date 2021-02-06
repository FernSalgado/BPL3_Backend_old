using System;
using System.Collections.Generic;
using System.Text;

namespace BPL3.Model
{
    public class Team
    {
        public string Name { get; set; }
        public string Leader { get; set; }
        public int TotalPoints { get; set; }
        public int LevelPoints { get; set; }
        public int DelvePoints { get; set; }
        public int SetPoints { get; set; }
        public string StashUrl { get; set; }
        public List<string> AllowedClasses { get; set; }
    }
}
