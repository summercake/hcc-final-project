using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vue2Spa.Services;
using Vue2Spa.Models;

namespace Vue2Spa.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        // GET /api/blog
        [HttpGet]
        public async Task<IActionResult> GetAllBlogs()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            if (string.IsNullOrEmpty(userId)) return BadRequest();

            var blogs = await _blogService.GetItems(userId);
            var blogInReverseOrder = blogs.Reverse();

            return Ok(blogInReverseOrder);
        }

        // POST /api/blog
        [HttpPost]
        public async Task<IActionResult> AddBlog([FromBody]BlogModel newBlog)
        {
            if (string.IsNullOrEmpty(newBlog?.Title)) return BadRequest();
            if (string.IsNullOrEmpty(newBlog?.Content)) return BadRequest();

            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            if (string.IsNullOrEmpty(userId)) return BadRequest();

            await _blogService.AddItem(userId, newBlog.Title, newBlog.Content);

            return Ok();
        }

        // POST /api/blog/{id}
        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateBlog(Guid id, [FromBody]BlogModel updatedData)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            if (string.IsNullOrEmpty(userId)) return BadRequest();

            await _blogService.UpdateItem(userId, id, updatedData);

            return Ok();
        }

        // DELETE /api/blog/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlog(Guid id)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            if (string.IsNullOrEmpty(userId)) return BadRequest();

            try
            {
                await _blogService.DeleteItem(userId, id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }
    }
}
