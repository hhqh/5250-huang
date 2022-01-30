using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using SQLite;

using Mine.Models;


namespace Mine.Services
{
    public class DatabaseService:IDataStore<ItemModel>
    {
        static readonly Lazy<SQLiteAsyncConnection> LazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        static SQLiteAsyncConnection Database => LazyInitializer.Value;
        static bool initialized = false;

        public DatabaseService()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(ItemModel).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(ItemModel)).ConfigureAwait(false);
                }
                initialized = true;
            }
        }

        /// <summary>
        /// Creates data if data is not null
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<bool> CreateAsync(ItemModel item)
        {
            // check if data is null
            if (item == null)
            {
                return false;
            }

            // insert data to database
            var result = await Database.InsertAsync(item);
            if(result == 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Update an item in the database
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(ItemModel item)
        {
            // Check if data is null
            if (item == null)
            {
                return false;
            }

            // Update data
            var result = await Database.UpdateAsync(item);
            if (result == 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Delete data from record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(string id)
        {
            // Check if data exist
            var data = await ReadAsync(id);
            if (data == null)
            {
                return false;
            }

            // Delete data
            var result = await Database.DeleteAsync(data);
            if (result == 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Read a record in the database that matches the ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ItemModel> ReadAsync(string id)
        {
            if (id == null)
            {
                return null;
            }

            // Call the database to read the id
            // Find the first record that has the ID that matches
            var result = Database.Table<ItemModel>().FirstOrDefaultAsync(m => m.Id.Equals(id));

            return result;
        }

        /// <summary>
        /// Resturn Database in Index
        /// </summary>
        /// <param name="forceRefresh"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ItemModel>> IndexAsync(bool forceRefresh = false)
        {
            var result = await Database.Table<ItemModel>().ToListAsync();
            return result;
        }
    }
}

