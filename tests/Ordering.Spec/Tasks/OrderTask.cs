using Ordering.Application.Contracts;
using Ordering.Application.Contracts.Orders;

namespace Ordering.Spec.Tasks;

public class OrderTask
{
    private readonly HttpClient _httpClient;

    public OrderTask(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    internal async Task<HttpResponseMessage> PlaceOrderAsync(PlaceOrderCommand command)
    {
        var response = await _httpClient.PostAsync("api/orders", command);
        return response;
    }
}