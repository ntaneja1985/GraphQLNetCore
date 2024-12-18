using GraphQLProject.Interfaces;
using GraphQLProject.Models;

namespace GraphQLProject.Services
{
    public class MenuRepository : IMenuRepository
    {
        private static List<Menu> MenuList = new List<Menu>()
        {
            new Menu() {Id = 1, Name="Classic Burger 0", Description="Veg Burger 0", Price=8.99},
            new Menu() {Id = 2, Name="Classic Burger 1", Description="Veg Burger 1", Price=7.99},
            new Menu() {Id = 3, Name="Classic Burger 2", Description="Veg Burger 2", Price=6.99},
            new Menu() {Id = 4, Name="Classic Burger 3", Description="Veg Burger 3", Price=5.99},
            new Menu() {Id = 5, Name="Classic Burger 4", Description="Veg Burger 4", Price=4.99},
            new Menu() {Id = 6, Name="Classic Burger 5", Description="Veg Burger 5", Price=3.99},
        };
        public Menu AddMenu(Menu menu)
        {
            MenuList.Add(menu);
            return menu;
        }

        public void DeleteMenu(int id)
        {
            var foundMenu = MenuList.Find(x => x.Id == id);
            MenuList.Remove(foundMenu);
        }

        public List<Menu> GetAllMenus()
        {
            return MenuList;
        }

        public Menu GetMenuById(int id)
        {
            return MenuList.Find(x=>x.Id == id);
        }

        public Menu UpdateMenu(int id, Menu menu)
        {
            var foundMenu = MenuList.Find(x => x.Id == id);
            foundMenu.Name = menu.Name;
            foundMenu.Description = menu.Description;
            foundMenu.Price = menu.Price;
            return menu;
        }
    }
}
