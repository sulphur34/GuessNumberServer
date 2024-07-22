using Grains.Interfaces;

namespace Entities;

public class PlayerGrain : Grain<PlayerState>, IPlayerGrain
{
    public Task SetName(string name)
    {
        State.Name = name;
        return WriteStateAsync();
    }

    public Task<string> GetName()
    {
        return Task.FromResult(State.Name);
    }

    public Task<int> GetScore()
    {
        return Task.FromResult(State.Score);
    }

    public Task AddScore()
    {
        State.Score++;
        return WriteStateAsync();
    }
}