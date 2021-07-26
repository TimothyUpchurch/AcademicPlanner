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
    }
}
