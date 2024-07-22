namespace Grains.Interfaces;

public interface IPlayerGrain : IGrainWithStringKey
{
    Task SetName(string name);
    Task<string> GetName();

    Task<int> GetScore();

    Task AddScore();
}