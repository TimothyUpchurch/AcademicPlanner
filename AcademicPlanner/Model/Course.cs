using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcademicPlanner.Model
{
    class Course
    {
        [PrimaryKey, AutoIncrement]
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        [Indexed]
        public int TermID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CourseStatus { get; set; }
        public string InstructorName { get; set; }
        public string InstructorPhone { get; set; }
        public string InstructorEmail { get; set; }
        public string CourseNotes { get; set; }

    }
}
