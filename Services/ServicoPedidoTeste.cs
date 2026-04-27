using GoodHamburger.Application.Services;
using GoodHamburger.Infrastructure.Repository;

namespace GoodHamburger.Tests.Services
{
    public class ServicoPedidoTeste
    {
        //construtor sem parâmetro, cria o repositório internamente
        private ServicePedido CreateService() =>
            new(new InMemoryOrderRepository());

        [Fact]
        public async Task CreateOrder_SandwichFriesDrink_Applies20PercentDiscount()
        {
            var service = CreateService();
            var order = await service.CreateAsync("João", new List<int> { 1, 4, 5 });
            Assert.Equal(20m, order.DescontoPercentual);
            Assert.Equal(9.50m, order.Subtotal);
            Assert.Equal(1.90m, order.DiscountAmount);
            Assert.Equal(7.60m, order.Total);
        }

        [Fact]
        public async Task CreateOrder_SandwichDrink_Applies15PercentDiscount()
        {
            var service = CreateService();
            var order = await service.CreateAsync("Maria", new List<int> { 2, 5 });
            Assert.Equal(15m, order.DescontoPercentual);
        }

        [Fact]
        public async Task CreateOrder_SandwichFries_Applies10PercentDiscount()
        {
            var service = CreateService();
            var order = await service.CreateAsync("Carlos", new List<int> { 3, 4 });
            Assert.Equal(10m, order.DescontoPercentual);
        }

        [Fact]
        public async Task CreateOrder_DuplicateType_ThrowsInvalidOperationException()
        {
            var service = CreateService();
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                service.CreateAsync("Ana", new List<int> { 1, 2 }));
        }

        [Fact]
        public async Task CreateOrder_InvalidItemId_ThrowsKeyNotFoundException()
        {
            var service = CreateService();
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                service.CreateAsync("Pedro", new List<int> { 99 }));
        }

        [Fact]
        public async Task DeleteOrder_NonExistent_ReturnsFalse()
        {
            var service = CreateService();
            var result = await service.DeleteAsync(Guid.NewGuid());
            Assert.False(result);
        }

        //Validar o nome obrigatório
        [Fact]
        public async Task CreateOrder_EmptyName_ThrowsArgumentException()
        {
            var service = CreateService();
            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.CreateAsync("", new List<int> { 1 }));
        }
    }
}