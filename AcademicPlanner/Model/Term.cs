using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcademicPlanner.Model
{
    public class Term
    {
        [PrimaryKey, AutoIncrement]
        public int TermID { get; set; }
        public string TermName { get; set; }
        public DateTime TermStart { get; set; }
        public DateTime TermEnd { get; set; }
    }
}
