namespace GoodHamburger.Application.DTOs
{
    public record PedidoDto
    (
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

    public record MenuItemDto(int Id, string Name, decimal Price, string Type);
}
