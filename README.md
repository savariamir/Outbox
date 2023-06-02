
# Transactional Outbox pattern

Microservice architectures are becoming increasingly popular and show promise in solving problems like scalability, maintainability, and agility, especially in large applications. But this architectural pattern also introduces challenges when it comes to data handling. In distributed applications, each service independently maintains the data it needs to operate in a dedicated service-owned datastore. To support such a scenario, you typically use a messaging solution like RabbitMQ, Kafka, or Azure Service Bus that distributes data (events) from one service via a messaging bus to other services of the application. Internal or external consumers can then subscribe to those messages and get notified of changes as soon as data is manipulated.


A well-known example in that area is an `ordering` system: when a user wants to create an order, an Ordering service receives data from a client application via a REST endpoint. It maps the payload to an internal representation of an `Order` object to validate the data. After a successful commit to the database, it publishes an `OrderPlaced` event to a message bus. Any other service interested in new orders (for example an `Inventory` or `Invoicing` service), would subscribe to `OrderPlaced` messages, process them, and store them in its own database.


## First problem

![event-handling-before-pattern.png](https://s3-us-west-2.amazonaws.com/secure.notion-static.com/e9a211b7-3cc3-48d9-a80a-7ca011d956a3/event-handling-before-pattern.png)

```c#
 var order = new Order(options);
        
 await _repository.AddAsync(order);

 await _publisher.PublishAsync(new OrderPlaced(options.Id));
```

This approach works well until an error occurs between saving the order object and publishing the corresponding event. Sending an event might fail at this point for many reasons:

- Network errors
- Message service outage
- Host failure

## Solutions

There's a well-known pattern called `Transactional Outbox` that can help you avoid these situations. It ensures events are saved in a datastore (typically in an Outbox table in your database) before they're ultimately pushed to a message broker. If the business object and the corresponding events are saved within the same database transaction, it's guaranteed that no data will be lost. Everything will be committed, or everything will roll back if there's an error. To eventually publish the event, a different service or worker process queries the Outbox table for unhandled entries, publishes the events, and marks them as processed. This pattern ensures events won't be lost after a business object is created or modified.


![outbox.png](https://s3-us-west-2.amazonaws.com/secure.notion-static.com/6fb88e84-8fff-4b6b-9f8e-0c55766e630d/outbox.png)


## Implementation

![AzureComponents (1).png](https://s3-us-west-2.amazonaws.com/secure.notion-static.com/0716aa33-0b0f-403f-a321-08415861e6be/AzureComponents_(1).png)

## Second problem

![TransactionalOutbox (1)-WithoutOutbox.png](https://s3-us-west-2.amazonaws.com/secure.notion-static.com/9612bca2-8ff0-42bf-a41e-2a8e71258a99/TransactionalOutbox_(1)-WithoutOutbox.png)

Message Bus needs the Acknowledge

## Delivery Semantics

- At-most-once (Potentially data lost and best performace)
- At-least-once (Consumer might get duplicate messages - reasonable performance and easy to handle)
- Exactly-once (Most Expensive)

Main issue in At-least-once is Duplication.


# Message Processning

- Idempotency
    - f(f(x)) = f(x)
    - Absolute value: |x|
    - Delete in Sql
        
        ```sql
        Delete * From Customers Where Id = 1
        ```
        

### Idempotent Consumer

 1. Expilict De-duplication
 2. Semantic (better performace and schala)


## Expilict De-duplication With AOP (Aspect Oriented Programming) Solution

```csharp
public abstract class IdempotentMessageHandler<T> : IConsumer<T> where T : DomainEvent
{
    private readonly IDuplicateHandler _duplicateHandler;
    private readonly IUnitOfWork _unitOfWork;

    protected IdempotentMessageHandler(IDuplicateHandler duplicateHandler, IUnitOfWork unitOfWork)
    {
        _duplicateHandler = duplicateHandler;
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<T> context)
    {
        await _unitOfWork.BeginAsync();

        try
        {
            if (!await _duplicateHandler.HasMessageProcessedBeforeAsync(context.Message.EventId))
            {
                await ConsumeAsync(context);
                await _duplicateHandler.MarkMessageProcessed(context.Message.EventId);
            }

            await _unitOfWork.CommitAsync();
        }
        catch (Exception exception)
        {
            await _unitOfWork.RollbackAsync();
            throw new Exception(exception.Message, exception);
        }
    }

    protected abstract Task ConsumeAsync(ConsumeContext<T> context);
}
```
