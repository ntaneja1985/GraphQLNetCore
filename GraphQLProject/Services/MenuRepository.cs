using GraphQLProject.Data;
using GraphQLProject.Interfaces;
using GraphQLProject.Models;
using Microsoft.EntityFrameworkCore;

namespace GraphQLProject.Services
{
    public class MenuRepository(GraphQLDBContext context) : IMenuRepository
    {
        
        public async Task<Menu> AddMenu(Menu menu)
        {
            await context.Menus.AddAsync(menu);
            await context.SaveChangesAsync();
            return menu;
        }

        public async Task DeleteMenu(int id)
        {
            var foundMenu = await context.Menus.FindAsync(id);
            context.Menus.Remove(foundMenu);
            await context.SaveChangesAsync();
        }

        public async Task<List<Menu>> GetAllMenus()
        {
            return await context.Menus.ToListAsync();
        }

        public async Task<Menu> GetMenuById(int id)
        {
            return await context.Menus.FindAsync(id);
        }

        public async Task<Menu> UpdateMenu(int id, Menu menu)
        {
            var foundMenu = await context.Menus.FindAsync(id);
            foundMenu.Name = menu.Name;
            foundMenu.Description = menu.Description;
            foundMenu.Price = menu.Price;
            await context.SaveChangesAsync();
            return foundMenu;
        }
    }
}
