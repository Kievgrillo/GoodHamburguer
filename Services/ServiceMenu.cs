using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;

namespace GoodHamburger.Application.Services
{
    public class ServiceMenu
    {
        public static readonly IReadOnlyList<ItemMenu> Items = new List<ItemMenu>
    {
        new() { Id = 1, Nome = "X Burger", Preco = 5.00m, Tipo = ItensDoMenu.Sanduiche },
        new() { Id = 2, Nome = "X Egg",    Preco = 4.50m, Tipo = ItensDoMenu.Sanduiche },
        new() { Id = 3, Nome = "X Bacon",  Preco = 7.00m, Tipo = ItensDoMenu.Sanduiche },
        new() { Id = 4, Nome = "Batata frita",  Preco = 2.00m, Tipo = ItensDoMenu.BatataFrita },
        new() { Id = 5, Nome = "Refrigerante",  Preco = 2.50m, Tipo = ItensDoMenu.Refrigerante },
    };

        public IEnumerable<ItemMenu> GetMenu() => Items;
    }
}
