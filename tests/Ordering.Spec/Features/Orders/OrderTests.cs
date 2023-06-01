using Microsoft.AspNetCore.Mvc.Testing;
using Ordering.Api;
using Ordering.Spec.Tasks;
using TestStack.BDDfy;
using Xunit;

namespace Ordering.Spec.Features.Orders;

public class OrderTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly OrderSteps _steps;

    public OrderTests(WebApplicationFactory<Program> factory) 
    {
        _steps = new OrderSteps(new OrderTask(factory.CreateClient()));
    }
    
    [Fact]
    public void should_create_event()
    {
        //Scenario
        this.Given(_ => _steps.UserWantsToPlaceTheOrder())
            .When(_ => _steps.UserSubmitTheOrder())
            .Then(_ => _steps.OrderShouldBeSuccessfulPlaced())
            .BDDfy();
    }

}