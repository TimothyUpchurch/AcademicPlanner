using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AcademicPlanner.Model;
using SQLite;
using Xamarin.Essentials;

namespace AcademicPlanner.Services
{
    public static class AssessmentService
    {
        static SQLiteAsyncConnection db;

        static async Task Init()
        {
            if (db != null) return;
            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "AcademicPlanner.db");
            db = new SQLiteAsyncConnection(databasePath);
            await db.CreateTableAsync<Assessment>();
        }

        public static async Task AddAssessment(Assessment assessment)
        {
            await Init();
            await db.InsertAsync(assessment);
        }

        public static async Task<IEnumerable<Assessment>> GetAssessment()
        {
            await Init();
            return await db.Table<Assessment>().ToListAsync();
        }

        public static async Task RemoveAssessment(Assessment assessment)
        {
            await Init();
            await db.DeleteAsync<Assessment>(assessment.AssessmentID);
        }
    }
}
