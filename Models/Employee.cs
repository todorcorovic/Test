using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.Models
{
    public class Employee
    {
        public string Name { get; set; }
        public List<Worklog> Worklogs { get; set; }
        public double TotalTimeWorked { get; set; }
    }

    public class Worklog
    {
        public string Id { get; set; }
        public string EmployeeName { get; set; }
        public DateTime StarTimeUtc { get; set; }
        public DateTime EndTimeUtc { get; set; }
        public string EntryNotes { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}