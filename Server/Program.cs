using Grains.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Server
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            string waitingQueueKey = "GlobalQueue";
            uint requiredPlayersCount = 2;

            using var host = Host.CreateDefaultBuilder(args)
                .UseOrleans(siloBuilder =>
                {
                    siloBuilder.UseLocalhostClustering();
                    siloBuilder.AddMemoryGrainStorage("GameStorage")
                        .AddMemoryGrainStorageAsDefault();
                })
                .Build();

            await host.StartAsync();

            var client = host.Services.GetRequiredService<IGrainFactory>();

            var waitingQueue = client.GetGrain<IWaitingQueueGrain>(waitingQueueKey);

            await waitingQueue.Configure(requiredPlayersCount);

            Console.WriteLine("Setup completed.");
            Console.WriteLine("Now you can launch the client.");
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
            await host.StopAsync();
        }
    }
}

