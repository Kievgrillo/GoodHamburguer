using GoodHamburger.Domain.Enums;

namespace GoodHamburger.Domain.Entities
{
    public class ItemMenu
    {
        //Representa os itens do menu 
        public int Id { get; init; }
        public string Nome { get; init; } = string.Empty;
        public decimal Preco { get; init; }
        public ItensDoMenu Tipo { get; init; }
    }
}
