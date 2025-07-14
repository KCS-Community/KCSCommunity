namespace KCSCommunity.Abstractions.Interfaces;

public interface ISessionStore
{
    void Set<T>(string key, T value);
    T? Get<T>(string key);
    void Remove(string key);
}