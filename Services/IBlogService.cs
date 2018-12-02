using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vue2Spa.Models;

namespace Vue2Spa.Services
{
    public interface IBlogService
    {
        Task<IEnumerable<BlogModel>> GetItems(string userId);

        Task AddItem(string userId, string title, string content);

        Task UpdateItem(string userId, Guid id, BlogModel updatedData);

        Task DeleteItem(string userId, Guid id);
    }
}
