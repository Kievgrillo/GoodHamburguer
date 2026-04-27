namespace GoodHamburger.Blazor.Models
{
    public record PedidoDto(
        Guid Id,
        string NomeCliente,
        int NumeroPedido,
        List<MenuItemDto> Items,
        decimal Subtotal,
        decimal DescontoPercentual,
        decimal DiscountAmount,
        decimal Total,
        DateTime CreatedAt
    );

    public record ItemMenuDto(int Id, string Nome, decimal Preco, string Tipo);
}
