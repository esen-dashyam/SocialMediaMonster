using System;
using System.Collections.Generic;
using System.Linq;
using SocialMonster.DAL;

namespace SocialMonster.Models
{
    public partial class Keyword
    {
        public string Word { get; set; }
        public int Count { get; set; }
        public string topWord { get; set; }
        public int topCount { get; set; }
    }
}