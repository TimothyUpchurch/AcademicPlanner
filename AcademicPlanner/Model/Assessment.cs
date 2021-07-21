using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace AcademicPlanner.Model
{
    public class Assessment
    {
        [PrimaryKey, AutoIncrement]
        public int AssessmentID { get; set; }
        [Indexed]
        public int CourseID { get; set; }
        public string AssessmentType { get; set; }
        public string AssessmentName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
