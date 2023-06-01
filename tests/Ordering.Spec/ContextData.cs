namespace Ordering.Spec;

public class ContextData
{
    private readonly Dictionary<string, object> _data = new();

    public T Get<T>(string name)
    {
        return (T) _data[name];
    }
        
    public T Get<T>()
    {
        return (T) _data[typeof(T).Name];
    }

    public void Set<T>(string name, T data)
    {
        _data[name] = data;
    }
        
    public void Set<T>(T data)
    {
        _data[typeof(T).Name] = data;
    }
        
    public void SetStringId(string id)
    {
        _data["id"] = id;
    }
        
    public string GetStringId()
    {
        return (string)_data["id"];
    }
}