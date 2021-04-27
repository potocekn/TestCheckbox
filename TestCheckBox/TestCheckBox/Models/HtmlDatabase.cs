using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;

namespace AppBase.Models
{
    public class HtmlDatabase
    {
        readonly SQLiteAsyncConnection database;

        public HtmlDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<HtmlRecord>().Wait();
        }

        public Task<List<HtmlRecord>> GetPagesAsync()
        {
            //Get all notes.
            return database.Table<HtmlRecord>().ToListAsync();
        }

        public Task<HtmlRecord> GetPageAsync(string name)
        {
            // Get a specific note.
            return database.Table<HtmlRecord>()
                            .Where(i => i.PageName == name)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SavePageAsync(HtmlRecord record)
        {
            if (record.ID != 0)
            {
                // Update an existing note.
                return database.UpdateAsync(record);
            }
            else
            {
                // Save a new note.
                return database.InsertAsync(record);
            }
        }

        public Task<int> DeletePageAsync(HtmlRecord record)
        {
            // Delete a note.
            return database.DeleteAsync(record);
        }
    }
}
