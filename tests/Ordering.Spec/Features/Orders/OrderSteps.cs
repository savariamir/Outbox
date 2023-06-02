using System.Net;
using AutoFixture;
using FluentAssertions;
using Ordering.Application.Contracts.Orders;
using Ordering.Spec.Tasks;

namespace Ordering.Spec.Features.Orders;

public class OrderSteps
{
    private readonly OrderTask _task;
    private readonly ContextData _contextData;

    public OrderSteps(OrderTask task)
    {
        _task = task;
        _contextData = new ContextData();
    }

    public void UserWantsToPlaceTheOrder()
    {
        var fixture = new Fixture();
        var createCommand = fixture.Create<PlaceOrderCommand>();
        _contextData.Set(createCommand);
    }

    public async Task UserSubmitTheOrder()
    {
        var createCommand = _contextData.Get<PlaceOrderCommand>();

        var createHttpResponse = await _task.PlaceOrderAsync(createCommand);

        createHttpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    public void OrderShouldBeSuccessfulPlaced()
    {
    }
}