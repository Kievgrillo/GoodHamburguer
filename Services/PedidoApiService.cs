using GoodHamburger.Blazor.Models;
using System.Net.Http.Json;
using System.Net;

namespace GoodHamburger.Blazor.Services
{
    public class PedidoApiService
    {
        private readonly HttpClient _http;

        public PedidoApiService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<ItemMenuDto>> GetMenuAsync()
        {
            var result = await _http.GetFromJsonAsync<List<ItemMenuDto>>("api/menu");
            return result ?? new List<ItemMenuDto>();
        }

        public async Task<List<PedidoDto>> GetAllAsync()
        {
            var result = await _http.GetFromJsonAsync<List<PedidoDto>>("api/orders");
            return result ?? new List<PedidoDto>();
        }

        public async Task<(PedidoDto? pedido, string? erro)> GetByIdAsync(Guid id)
        {
            var response = await _http.GetAsync($"api/orders/{id}");
            if (response.StatusCode == HttpStatusCode.NotFound)
                return (null, "Pedido não encontrado.");
            var pedido = await response.Content.ReadFromJsonAsync<PedidoDto>();
            return (pedido, null);
        }

        public async Task<(PedidoDto? pedido, string? erro)> CreateAsync(string nomeCliente, List<int> itemIds)
        {
            var payload = new CriarPedidoRequest(nomeCliente, itemIds);
            var response = await _http.PostAsJsonAsync("api/orders", payload);

            if (response.IsSuccessStatusCode)
            {
                var pedido = await response.Content.ReadFromJsonAsync<PedidoDto>();
                return (pedido, null);
            }

            return (null, await LerErroAsync(response));
        }

        public async Task<(PedidoDto? pedido, string? erro)> UpdateAsync(Guid id, List<int> itemIds)
        {
            var response = await _http.PutAsJsonAsync($"api/orders/{id}", itemIds);

            if (response.IsSuccessStatusCode)
            {
                var pedido = await response.Content.ReadFromJsonAsync<PedidoDto>();
                return (pedido, null);
            }

            return (null, await LerErroAsync(response));
        }

        public async Task<(bool sucesso, string? erro)> DeleteAsync(Guid id)
        {
            var response = await _http.DeleteAsync($"api/orders/{id}");
            if (response.StatusCode == HttpStatusCode.NoContent) return (true, null);
            if (response.StatusCode == HttpStatusCode.NotFound) return (false, "Pedido não encontrado.");
            return (false, "Erro ao remover pedido.");
        }

        private static async Task<string> LerErroAsync(HttpResponseMessage response)
        {
            try
            {
                var body = await response.Content.ReadFromJsonAsync<ErroDto>();
                return body?.Error ?? $"Erro {(int)response.StatusCode}";
            }
            catch
            {
                return $"Erro {(int)response.StatusCode}";
            }
        }
    }

    internal record CriarPedidoRequest(string NomeCliente, List<int> ItemIds);
    internal record ErroDto(string Error);
}
