using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mine.Models;

namespace Mine.Services
{
    public class MockDataStore : IDataStore<ItemModel>
    {
        readonly List<ItemModel> items;

        public MockDataStore()
        {
            items = new List<ItemModel>()
            {
                new ItemModel { Id = Guid.NewGuid().ToString(), Text = "Plain Pillow", Description="Soft but firm attacks", Value = 1},
                new ItemModel { Id = Guid.NewGuid().ToString(), Text = "Blue Toothpaste", Description="For a blue and brighter smile", Value = 3},
                new ItemModel { Id = Guid.NewGuid().ToString(), Text = "Tofu", Description="Cause greater harm than you could imagine", Value = 2},
                new ItemModel { Id = Guid.NewGuid().ToString(), Text = "Encyclopedia", Description="Knows more than you've ever known", Value = 5 },
                new ItemModel { Id = Guid.NewGuid().ToString(), Text = "Ugly Christmas Sweater", Description="Good for the weather but bad for the eyes", Value = 4 }     
            };
        }

        public async Task<bool> AddItemAsync(ItemModel item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(ItemModel item)
        {
            var oldItem = items.Where((ItemModel arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((ItemModel arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<ItemModel> ReadAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<ItemModel>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}