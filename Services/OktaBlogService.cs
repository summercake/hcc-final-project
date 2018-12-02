using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Okta.Sdk;
using Vue2Spa.Models;

namespace Vue2Spa.Services
{
    public class OktaBlogService : IBlogService
    {
        private const string BlogProfileKey = "blog";

        private readonly IOktaClient _oktaClient;

        public OktaBlogService(IOktaClient oktaClient)
        {
            _oktaClient = oktaClient;
        }

        private IEnumerable<BlogModel> GetItemsFromProfile(IUser oktaUser)
        {
            if (oktaUser == null)
            {
                return Enumerable.Empty<BlogModel>();
            }

            var json = oktaUser.Profile.GetProperty<string>(BlogProfileKey);
            if (string.IsNullOrEmpty(json))
            {
                return Enumerable.Empty<BlogModel>();
            }

            return JsonConvert.DeserializeObject<BlogModel[]>(json);
        }

        private async Task SaveItemsToProfile(IUser user, IEnumerable<BlogModel> blogs)
        {
            var json = JsonConvert.SerializeObject(blogs.ToArray());

            user.Profile[BlogProfileKey] = json;
            await user.UpdateAsync();
        }

        public async Task AddItem(string userId, string title, string content)
        {
            var user = await _oktaClient.Users.GetUserAsync(userId);

            var existingItems = GetItemsFromProfile(user)
                .ToList();

            existingItems.Add(new BlogModel
            {
                Id = Guid.NewGuid(),
                Title = title,
                Content = content,
            });

            await SaveItemsToProfile(user, existingItems);
        }

        public async Task DeleteItem(string userId, Guid id)
        {
            var user = await _oktaClient.Users.GetUserAsync(userId);

            var updatedItems = GetItemsFromProfile(user)
                .Where(item => item.Id != id);

            await SaveItemsToProfile(user, updatedItems);
        }

        public async Task<IEnumerable<BlogModel>> GetItems(string userId)
        {
            var user = await _oktaClient.Users.GetUserAsync(userId);
            return GetItemsFromProfile(user);
        }

        public async Task UpdateItem(string userId, Guid id, BlogModel updatedData)
        {
            var user = await _oktaClient.Users.GetUserAsync(userId);

            var existingItems = GetItemsFromProfile(user)
                .ToList();

            var itemToUpdate = existingItems
                .FirstOrDefault(item => item.Id == id);
            if (itemToUpdate == null)
            {
                return;
            }
            
            if (!string.IsNullOrEmpty(updatedData.Title))
            {
                itemToUpdate.Title = updatedData.Title;
            }

            if (!string.IsNullOrEmpty(updatedData.Content))
            {
                itemToUpdate.Content = updatedData.Content;
            }

            await SaveItemsToProfile(user, existingItems);
        }
    }
}
