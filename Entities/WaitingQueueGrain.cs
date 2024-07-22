using Grains.Interfaces;

namespace Entities
{
    public class WaitingQueueGrain : Grain, IWaitingQueueGrain
    {
        private readonly Queue<IPlayerGrain> _joinedPlayers = new();

        private IRoomGrain? _roomGrain;
        private uint _playersAmount;

        public Task Clear()
        {
            _joinedPlayers.Clear();
            _roomGrain = null;
            return Task.CompletedTask;
        }

        public Task Configure(uint playersAmount)
        {
            _playersAmount = playersAmount;
            return Task.CompletedTask;
        }

        public async Task<IRoomGrain> GetRoom()
        {
            if (_roomGrain == null)
            {
                _roomGrain = GrainFactory.GetGrain<IRoomGrain>(Guid.NewGuid());
                await _roomGrain.Configure(_playersAmount);
            }

            return _roomGrain;
        }

        public Task<bool> IsQueueFull()
        {
            return Task.FromResult(_joinedPlayers.Count >= _playersAmount);
        }

        public Task JoinQueue(IPlayerGrain player)
        {
            _joinedPlayers.Enqueue(player);
            return Task.CompletedTask;
        }
    }
}
