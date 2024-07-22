using Grains.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var waitingQueueKey = "GlobalQueue";

            using var host = Host.CreateDefaultBuilder()
                .UseOrleansClient(clientBuilder =>
                    clientBuilder.UseLocalhostClustering())
                .Build();

            await host.StartAsync();
            var client = host.Services.GetRequiredService<IClusterClient>();

            Console.WriteLine();
            Console.WriteLine("What's your name?");
            var name = Console.ReadLine()!;

            var player = client.GetGrain<IPlayerGrain>(name);
            await player.SetName(name);
            var queue = client.GetGrain<IWaitingQueueGrain>(waitingQueueKey);

            char command = new char();

            while (command != 'x')
            {
                await Play(queue, player);

                Console.WriteLine("Press any key to continue or press \"x\" to exit game");
                command = Console.ReadKey().KeyChar;
            }

            Console.WriteLine("Exit game");
            Console.ReadKey();
        }

        private static async Task Play(IWaitingQueueGrain queue, IPlayerGrain player)
        {
            Console.WriteLine("You joined game. Waiting for second player");
            await queue.JoinQueue(player);

            while (await queue.IsQueueFull() == false)
                continue;

            var room = await queue.GetRoom();

            int guess = GetNumberFromUser();

            await room.MakeGuess(player, guess);

            while (await room.IsAllPlayersGuessed() == false)
                continue;

            await queue.Clear();

            List<IPlayerGrain> winnersData = await room.DetermineWinners();

            if (winnersData
                .Any(playerGrain =>
                    playerGrain.GetPrimaryKeyString() == player.GetPrimaryKeyString()))
                await player.AddScore();

            uint roomGuessNumber = await room.GetGuessNumber();

            Console.WriteLine("Room guess number was " + roomGuessNumber);
            Console.WriteLine("The winners are");

            foreach (var winner in winnersData)
            {
                var winnerName = await winner.GetName();
                var winnerScore = await winner.GetScore();

                Console.WriteLine("Player {0}. Total score is {1}", winnerName, winnerScore);
            }

            Console.WriteLine();
        }

        private static int GetNumberFromUser()
        {
            Console.WriteLine("Room found. Enter your guess:");
            Console.WriteLine("Number should be between 0 and 100");

            int minVAlue = 0;
            int maxValue = 100;
            bool isValidNumber = false;
            int number = 0;

            while (isValidNumber == false)
            {
                string input = Console.ReadLine();

                if (int.TryParse(input, out number))
                {
                    if (number >= minVAlue && number <= maxValue)
                        isValidNumber = true;
                    else
                        Console.WriteLine("Wrong! Number should be between 0 fnd 100");
                }
                else
                {
                    Console.WriteLine("Wrong! Input is not a number");
                }
            }

            return number;
        }
    }
}