using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Anshan.Domain;

namespace Anshan.OutboxProcessor.Types
{
    public class EventTypeResolver : IEventTypeResolver
    {
        private readonly Dictionary<string, Type> _types = new();

        public void AddTypesFromAssembly(Assembly assembly)
        {
            var events = assembly.GetTypes()
                .Where(type => typeof(IDomainEvent).IsAssignableFrom(type) && !type.IsAbstract)
                .ToList();
            
            events.ForEach(a => { _types.Add(a.Name, a); });
        }

        public Type GetType(string typeName)
        {
            return _types.TryGetValue(typeName, out var type) ? type : null;
        }
    }
}