using GoodHamburger.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburguer.Api.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController(ServicePedido orderService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await orderService.GetAllAsync());

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var order = await orderService.GetByIdAsync(id);
            return order is null ? NotFound(new { error = "Pedido não encontrado." }) : Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] List<int> itemIds)
        {
            var created = await orderService.CreateAsync(itemIds);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] List<int> itemIds)
        {
            var updated = await orderService.UpdateAsync(id, itemIds);
            return updated is null ? NotFound(new { error = "Pedido não encontrado." }) : Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await orderService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound(new { error = "Pedido não encontrado." });
        }
    }
}
