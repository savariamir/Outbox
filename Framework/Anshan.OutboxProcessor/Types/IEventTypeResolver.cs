using System;

namespace Anshan.OutboxProcessor.Types
{
    public interface IEventTypeResolver
    {
        Type GetType(string typeName);
    }
}