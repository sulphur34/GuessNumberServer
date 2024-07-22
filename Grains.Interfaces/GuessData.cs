using Grains.Interfaces;

namespace Entities
{
    [GenerateSerializer]
    public class GuessData
    {
        public GuessData(IPlayerGrain playerGrain, int guess)
        {
            PlayerGrain = playerGrain;
            Guess = guess;
        }

        public IPlayerGrain PlayerGrain { get; set; }
        public int Guess { get; set; }
    }
}
