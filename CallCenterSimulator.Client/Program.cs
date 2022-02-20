using System;
using System.IO;
using System.Threading.Channels;
using System.Threading.Tasks;
using CallCenterSimulator.Domain.GrpcClient.Core;
using CallCenterSimulator.Domain.GrpcClient.Core.Model.Events;
using Grpc.Core;
using Channel = Grpc.Core.Channel;

namespace CallCenterSimulator.Client
{
    class Program
    {
        const int PORT = 19021;

        public async static Task Main(string[] args)
        {
            Console.WriteLine("Chat Window Opened");

            var channelCredentials = new SslCredentials(File.ReadAllText(@"certificate.crt"));
            var channel = new Channel($"localhost:{PORT}",channelCredentials);
            
            Console.Write("Please write your User Name : ");
            var userName = Console.ReadLine();

            using (var client = new GrpcClient(channel))
            {
                client.OnCallStarted += ClientOnOnCallStarted;
                client.OnShuttingDown += ClientOnOnShuttingDown;
                
                await client.StartCall(userName);

                await client.ListenServerAsync();
                
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                
                client.ShutDownClient();
            }
        }

        private static void ClientOnOnShuttingDown(object? sender, ProcessEventArgs e)
        {
            Console.WriteLine("Shutting down...");
        }

        private static void ClientOnOnCallStarted(object? sender, ProcessEventArgs e)
        {
            var nl = Environment.NewLine;
            var orgTextColor = Console.ForegroundColor;
            
            Console.Write($"Connected to Chat.{nl}");
            Console.ForegroundColor = ConsoleColor.Cyan;
            
            Console.ForegroundColor = orgTextColor;
            Console.WriteLine($"{nl}Looking For Suitable Agent Please Wait{nl}");
        }
    }
}
