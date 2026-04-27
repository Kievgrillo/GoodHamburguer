namespace GoodHamburger.Blazor.Models
{
    public record OrderDto(
        Guid Id,
        List<MenuItemDto> Items,
        decimal Subtotal,
        decimal DescontoPercentual,
        decimal DiscountAmount,
        decimal Total,
        DateTime CreatedAt
    );

    public record MenuItemDto(int Id, string Name, decimal Price, string Type);
}
