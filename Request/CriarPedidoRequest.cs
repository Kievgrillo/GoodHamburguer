namespace GoodHamburger.Application.Requests
{
    public record CriarPedidoRequest(string NomeCliente, List<int> ItemIds);
}