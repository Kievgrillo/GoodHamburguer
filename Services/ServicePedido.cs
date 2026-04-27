using GoodHamburger.Application.DTOs;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Interfaces;

namespace GoodHamburger.Application.Services
{
    public class ServicePedido
    {
        private readonly IRepositorioDoPedido _repository;

        public ServicePedido(IRepositorioDoPedido repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<PedidoDto>> GetAllAsync()
        {
            var orders = await _repository.GetAllAsync();
            return orders.Select(MapToDto);
        }

        public async Task<PedidoDto?> GetByIdAsync(Guid id)
        {
            var order = await _repository.GetByIdAsync(id);
            return order is null ? null : MapToDto(order);
        }

        public async Task<PedidoDto> CreateAsync(string nomeCliente, List<int> itemIds)
        {
            if (string.IsNullOrWhiteSpace(nomeCliente))
                throw new ArgumentException("O nome do cliente é obrigatório.");

            var items = ResolveAndValidate(itemIds);

            // gera número sequencial baseado na quantidade atual de pedidos
            var todos = await _repository.GetAllAsync();
            var numeroPedido = todos.Count() + 1;

            var order = new Pedido();
            order.SetNomeCliente(nomeCliente.Trim());
            order.SetNumeroPedido(numeroPedido);
            order.SetItems(items);

            var created = await _repository.CreateAsync(order);
            return MapToDto(created);
        }

        public async Task<PedidoDto?> UpdateAsync(Guid id, List<int> itemIds)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing is null) return null;

            var items = ResolveAndValidate(itemIds);
            existing.SetItems(items);
            var updated = await _repository.UpdateAsync(existing);
            return MapToDto(updated);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if (await _repository.GetByIdAsync(id) is null) return false;
            await _repository.DeleteAsync(id);
            return true;
        }

        private static List<ItemMenu> ResolveAndValidate(List<int> itemIds)
        {
            if (itemIds is null || itemIds.Count == 0)
                throw new ArgumentException("O pedido deve conter ao menos um item.");

            var resolved = itemIds.Select(id =>
                ServiceMenu.Items.FirstOrDefault(m => m.Id == id)
                ?? throw new KeyNotFoundException($"Item com id {id} não existe no cardápio.")
            ).ToList();

            var types = resolved.Select(i => i.Tipo).ToList();
            var duplicateType = types.GroupBy(t => t).FirstOrDefault(g => g.Count() > 1);
            if (duplicateType is not null)
                throw new InvalidOperationException(
                    $"Pedido inválido: mais de um item do tipo '{duplicateType.Key}' não é permitido.");

            return resolved;
        }

        private static PedidoDto MapToDto(Pedido o) => new(
            o.Id,
            o.NomeCliente,       
            o.NumeroPedido,      
            o.Items.Select(i => new MenuItemDto(i.Id, i.Nome, i.Preco, i.Tipo.ToString())).ToList(),
            o.Subtotal,
            o.DescontoPercentual * 100,
            o.DiscountAmount,
            o.Total,
            o.CreatedAt
        );
    }
}