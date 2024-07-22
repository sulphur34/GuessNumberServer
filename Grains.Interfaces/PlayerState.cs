namespace Grains.Interfaces
{
    [GenerateSerializer]
    public class PlayerState
    {
        public string Name { get; set; }
        public int Score { get; set; }
    }
}