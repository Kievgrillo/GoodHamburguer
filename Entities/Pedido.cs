using GoodHamburger.Domain.Enums;

namespace GoodHamburger.Domain.Entities
{
    public class Pedido
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public List<ItemMenu> Items { get; private set; } = new();
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
        public string NomeCliente { get; private set; } = string.Empty;
        public int NumeroPedido { get; private set; }

        public decimal Subtotal => Items.Sum(i => i.Preco);

        public decimal DescontoPercentual
        {
            get
            {
                bool HSanduiche = Items.Any(i => i.Tipo == ItensDoMenu.Sanduiche);
                bool HBatata = Items.Any(i => i.Tipo == ItensDoMenu.BatataFrita);
                bool HRfrigerante = Items.Any(i => i.Tipo == ItensDoMenu.Refrigerante);

                if (HSanduiche && HBatata && HRfrigerante) return 0.20m;
                if (HSanduiche && HRfrigerante) return 0.15m;
                if (HSanduiche && HBatata) return 0.10m;
                return 0m;
            }
        }

        public decimal DiscountAmount => Subtotal * DescontoPercentual;
        public decimal Total => Subtotal - DiscountAmount;

        public void SetItems(List<ItemMenu> itens) => Items = itens;
        public void SetNomeCliente(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("O nome do cliente é obrigatório.");
            NomeCliente = nome;
        }

        public void SetNumeroPedido(int numero)
        {
            if (numero <= 0)
                throw new ArgumentException("O número do pedido deve ser maior que zero.");
            NumeroPedido = numero;
        }   
    }
}
