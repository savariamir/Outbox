namespace Anshan.Domain;

public interface IVersionable
{
    int Version { get; }

    void IncrementVersion();
}