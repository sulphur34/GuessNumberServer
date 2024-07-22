using Grains.Interfaces;

namespace Entities;

public class RoomGrain : Grain, IRoomGrain
{
    private uint _guessNumber;

    private readonly List<GuessData> _playerGuesses = new();

    private uint _playersCount;

    public Task Configure(uint playersCount)
    {
        _playersCount = playersCount;
        _guessNumber = ((uint)new Random().Next(0, 101));
        return Task.CompletedTask;
    }

    public Task MakeGuess(IPlayerGrain playerGrain, int guess)
    {
        GuessData guessData = new GuessData(playerGrain, guess);
        _playerGuesses.Add(guessData);
        return Task.CompletedTask;
    }

    public Task<bool> IsAllPlayersGuessed()
    {
        return Task.FromResult(_playerGuesses.Count == _playersCount);
    }

    public Task<List<IPlayerGrain>> DetermineWinners()
    {
        var winnerData = _playerGuesses
            .GroupBy(guessData => Math.Abs(_guessNumber - guessData.Guess))
            .OrderBy(group => group.Key)
            .First()
            .Select(guessData => guessData.PlayerGrain)
            .ToList();

        return Task.FromResult(winnerData);
    }

    public Task<uint> GetGuessNumber()
    {
        return Task.FromResult(_guessNumber);
    }
}