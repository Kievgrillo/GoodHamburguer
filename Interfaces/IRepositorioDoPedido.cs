using GoodHamburger.Domain.Entities;

namespace GoodHamburger.Domain.Interfaces
{
    public interface IRepositorioDoPedido
    {
        Task<IEnumerable<Pedido>> GetAllAsync();
        Task<Pedido?> GetByIdAsync(Guid id);
        Task<Pedido> CreateAsync(Pedido pedido);
        Task<Pedido> UpdateAsync(Pedido pedido);
        Task DeleteAsync(Guid id);
    }
}
