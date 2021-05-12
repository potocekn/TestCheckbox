using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;

namespace AppBase.Models
{
    /// <summary>
    /// Class representing Database where HTML files are stored. This database is used for HTML versions of the resources.
    /// </summary>
    public class HtmlDatabase
    {
        readonly SQLiteAsyncConnection database;

        public HtmlDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<HtmlRecord>().Wait();
        }

        /// <summary>
        /// Method used for getting all of the records located in the database.
        /// </summary>
        /// <returns>Result of the Task contains all records from database.</returns>
        public Task<List<HtmlRecord>> GetPagesAsync()
        {           
            return database.Table<HtmlRecord>().ToListAsync();
        }

        /// <summary>
        /// Method used to get one specific record based on the value of the column name.
        /// </summary>
        /// <param name="name">name of the resource that should be retrieved</param>
        /// <returns>Result of the task contains the specific record with given name</returns>
        public Task<HtmlRecord> GetPageAsync(string name)
        {
            return database.Table<HtmlRecord>()
                            .Where(i => i.PageName == name)
                            .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Method used to save or update record. If the database contains this record (id or name) the record will be updated. 
        /// Otherwise new record will be created in the database.
        /// </summary>
        /// <param name="record"></param>
        /// <returns>Task Result contains int representing the success of the operation</returns>
        public Task<int> SavePageAsync(HtmlRecord record)
        {
            var alreadyThere = ContainsRecordWithSameName(record.PageName);
            if (record.ID != 0 || (alreadyThere != null))
            {               
                return database.UpdateAsync(record);
            }
            else
            {                
                return database.InsertAsync(record);
            }
        }

        /// <summary>
        /// Method that determines if the database contains record with given name.
        /// </summary>
        /// <param name="name">name of the record that should be found</param>
        /// <returns>Found record from the database or null if there is no such record.</returns>
        private HtmlRecord ContainsRecordWithSameName(string name)
        {
            var records = GetPagesAsync();
            foreach (var item in records.Result)
            {
                if (item.PageName == name)
                {
                    return item;
                }
            }
            return null;
        }

        /// <summary>
        /// Method used for deleting record from the database.
        /// </summary>
        /// <param name="record">Record that should be deleted.</param>
        /// <returns>Task Result contains int representing the success of the operation</returns>
        public Task<int> DeletePageAsync(HtmlRecord record)
        {            
            return database.DeleteAsync(record);
        }
    }
}
