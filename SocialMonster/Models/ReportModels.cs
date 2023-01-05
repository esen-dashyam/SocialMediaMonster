using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMonster.Models
{
    public class ReportModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string PersonID { get; set; }
        public string Date { get; set; }

    }
    public class ReportAccount
    {
        public string ID { get; set; }
        public string AccountID { get; set; }
        public string isReal { get; set; }
        public string AccountName { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Sex { get; set; }
        public string Register { get; set; }
        public string Aimag { get; set; }
        public string Sum { get; set; }
        public string FakeOrReal { get; set; }
    }
    public class ReportGraph
    {
        public int negativeCount { get; set; }
        public int positiveCount { get; set; }
        public int neutralCount { get; set; }
        public int negativeCount_no_fake { get; set; }
        public int positiveCount_no_fake { get; set; }
        public int neutralCount_no_fake { get; set; }
        public int realUserCount { get; set; }
        public int realUserCount_with_address { get; set; }
        public int trollUserCount { get; set; }
        public string listAimagAccountCount { get; set; }
        public string listAimagAccountCount_positive { get; set; }
        public string listAimagAccountCount_negative { get; set; }
    }
}