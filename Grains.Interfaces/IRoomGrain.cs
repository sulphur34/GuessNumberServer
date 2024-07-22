namespace Grains.Interfaces
{
    public interface IRoomGrain : IGrainWithGuidKey
    {
        Task Configure(uint playersCount);

        Task<uint> GetGuessNumber();

        Task MakeGuess(IPlayerGrain playerGrain, int guess);

        Task<bool> IsAllPlayersGuessed();

        Task<List<IPlayerGrain>> DetermineWinners();
    }
}