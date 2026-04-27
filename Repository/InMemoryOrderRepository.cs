using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Interfaces;

namespace GoodHamburger.Infrastructure.Repository
{
    public class InMemoryOrderRepository : IRepositorioDoPedido
    {
        private readonly Dictionary<Guid, Pedido> _store = new();

        public Task<IEnumerable<Pedido>> GetAllAsync() =>
            Task.FromResult<IEnumerable<Pedido>>(_store.Values.ToList());

        public Task<Pedido?> GetByIdAsync(Guid id) =>
            Task.FromResult(_store.TryGetValue(id, out var pedido) ? pedido : null);

        public Task<Pedido> CreateAsync(Pedido pedido)
        {
            _store[pedido.Id] = pedido;
            return Task.FromResult(pedido);
        }

        public Task<Pedido> UpdateAsync(Pedido pedido)
        {
            _store[pedido.Id] = pedido;
            return Task.FromResult(pedido);
        }

        public Task DeleteAsync(Guid id)
        {
            _store.Remove(id);
            return Task.CompletedTask;
        }
    }
}
