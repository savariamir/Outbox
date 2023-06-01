using LiteBus.Commands.Abstractions;
using Ordering.Application.Contracts;
using Ordering.Domain;

namespace Ordering.Application;

public class PlaceOrderHandler: ICommandHandler<PlaceOrderCommand>
{
    private readonly IOrderRepository _repository;

    public PlaceOrderHandler(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task HandleAsync(PlaceOrderCommand command, CancellationToken cancellationToken = new())
    {
        var options = OrderFactory.CreateFrom(command);

        var order = Order.PlaceOrder(options);
        
        await _repository.AddAsync(order);
    }
}