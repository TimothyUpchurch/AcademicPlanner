using System;
using System.Collections.Generic;
using System.Text;

namespace AcademicPlanner.Services
{
    public static class Validations
    {
        public static bool EndDateAfterStart(DateTime start, DateTime end)
        {
            if (end >= start)
            {
                return true;
            }
            return false;
        }

        public static bool AllCoursePageFieldsOccupied(string courseName, DateTime startDate, DateTime endDate, string courseStatus, string instructorName, string instructorPhone, string instructorEmail)
        {
            if (courseName != null && startDate != null && endDate != null && courseStatus != null && instructorName != null && instructorPhone != null && instructorEmail != null)
            {
                return true;
            }
            //if (courseName != "" && courseStatus != "" && instructorName != "" && instructorPhone != "" && instructorEmail != "")
            //{
            //    return true;
            //}
            return false;
        }
    }
}
