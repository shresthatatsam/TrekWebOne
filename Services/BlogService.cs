using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using UserRoles.Data;
using UserRoles.Models.Blog;
using UserRoles.Services.Interface;

namespace UserRoles.Services
{
    public class BlogService : IBlogService
    {
        private readonly AppDbContext _context;

        public BlogService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Blog>> GetAllAsync()
        {
            return await _context.Blogs.ToListAsync();
        }

        public async Task<Blog> GetByIdAsync(int id)
        {
            return await _context.Blogs.FindAsync(id);
        }

        public async Task AddAsync(Blog blog)
        {
            blog.CreatedAt = DateTime.UtcNow;
            blog.UpdatedAt = DateTime.UtcNow;
            blog.Content = blog.Content ?? string.Empty;
            blog.Title = blog.Title ?? string.Empty;
            _context.Blogs.Add(blog);
            _context.SaveChanges();
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Blog blog)
        {
            blog.UpdatedAt = DateTime.UtcNow;
            _context.Blogs.Update(blog);   
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var blog = await GetByIdAsync(id);
            if (blog != null)
            {
                _context.Blogs.Remove(blog);
                await _context.SaveChangesAsync();
            }
        }
    }
}
