namespace Grains.Interfaces
{
    public interface IWaitingQueueGrain : IGrainWithStringKey
    {
        Task Configure(uint playersAmount);

        Task JoinQueue(IPlayerGrain player);

        Task<bool> IsQueueFull();

        Task<IRoomGrain> GetRoom();

        Task Clear();
    }
}