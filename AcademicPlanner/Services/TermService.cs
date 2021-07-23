using AcademicPlanner.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace AcademicPlanner.Services
{
    public static class TermService
    {
        static SQLiteAsyncConnection db;

        static async Task Init()
        {
            // create db and tables
            // Get an absolute path to the database file
            if (db != null) return;
            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "AcademicPlanner.db");
            db = new SQLiteAsyncConnection(databasePath);
            await db.CreateTableAsync<Term>();
        }
        public static async Task AddTerm(Term term)
        {
            await Init();
            await db.InsertAsync(term);
        }
        public static async Task RemoveTerm(int termID)
        {
            await Init();
            await db.DeleteAsync<Term>(termID);
        }
        public static async Task UpdateTerm(Term term)
        {
            await Init();
            await db.UpdateAsync(term);
        }
        public static async Task<IEnumerable<Term>> GetTerms()
        {
            await Init();
            return await db.Table<Term>().ToListAsync();
        }
    }
}
