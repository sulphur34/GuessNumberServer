namespace Entities
{
    [GenerateSerializer]
    public class PlayerState
    {
        public string Name { get; set; }
        public int Score { get; set; }
    }
}