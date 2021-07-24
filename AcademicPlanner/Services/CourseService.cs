using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicPlanner.Model;
using SQLite;
using Xamarin.Essentials;

namespace AcademicPlanner.Services
{
    public static class CourseService
    {
        static SQLiteAsyncConnection db;
        static async Task Init()
        {
            // create db and tables
            // Get an absolute path to the database file
            if (db != null) return;
            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "AcademicPlanner.db");
            db = new SQLiteAsyncConnection(databasePath);
            await db.CreateTableAsync<Course>();
        }

        public static async Task AddCourse(Course course)
        {
            await Init();
            await db.InsertAsync(course);
        }

        public static async Task<IEnumerable<Course>> GetCourse()
        {
            await Init();
            return await db.Table<Course>().ToListAsync();
        }

        public static async Task RemoveCourse(int courseID)
        {
            await Init();
            await db.DeleteAsync<Course>(courseID);
        }

        public static async Task UpdateCourse(Course course)
        {
            await Init();
            await db.UpdateAsync(course);
        }
    }
}
