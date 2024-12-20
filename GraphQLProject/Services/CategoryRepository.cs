using GraphQLProject.Data;
using GraphQLProject.Interfaces;
using GraphQLProject.Models;
using Microsoft.EntityFrameworkCore;

namespace GraphQLProject.Services
{
    public class CategoryRepository(GraphQLDBContext dbContext) : ICategoryRepository
    { 
        public async Task<Category> AddCategory(Category category)
        {
            dbContext.Categories.Add(category);
             await  dbContext.SaveChangesAsync();
            return category;
        }

        public async Task DeleteCategory(int id)
        {
            var category = dbContext.Categories.FirstOrDefault(c => c.Id == id);
            dbContext.Categories.Remove(category);
            await dbContext.SaveChangesAsync();
        }

        public async Task<List<Category>> GetAllCategories()
        {
            return await dbContext.Categories.ToListAsync();
        }

        public async Task<Category> UpdateCategory(int id, Category category)
        {
            var foundCategory = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
            foundCategory.Name = category.Name;
            foundCategory.ImageUrl = category.ImageUrl;
            //foundCategory.Menus = category.Menus;
            await dbContext.SaveChangesAsync();
            return foundCategory;
        }
    }
}
