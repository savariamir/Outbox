using LiteBus.Commands.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Contracts;
using Ordering.Application.Contracts.Orders;

namespace Ordering.Api.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController : ControllerBase
{
    private readonly ICommandMediator _commandMediator;

    public OrderController(ICommandMediator commandMediator)
    {
        _commandMediator = commandMediator;
    }

    [HttpPost]
    public async Task<IActionResult> PlaceOrder(PlaceOrderCommand command)
    {
        await _commandMediator.SendAsync(command);
        return Ok();
    }
}