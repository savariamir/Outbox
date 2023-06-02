using LiteBus.Commands.Abstractions;
using Ordering.Application.Contracts.Orders;
using Ordering.Domain.Models.Orders;

namespace Ordering.Application.Orders;

public class PlaceOrderHandler: ICommandHandler<PlaceOrderCommand>
{
    private readonly IOrderRepository _repository;

    public PlaceOrderHandler(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task HandleAsync(PlaceOrderCommand command, 
        CancellationToken cancellationToken = new())
    {
        var options = OrderFactory.CreateFrom(command);

        var order = Order.PlaceOrder(options);
        
        await _repository.AddAsync(order);
    }
}