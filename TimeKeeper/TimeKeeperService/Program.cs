using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.InteropExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TimeKeeperService
{
    class Program
    {

        private static IQueueClient Client;
        static void Main(string[] args)
        {
            ActivateClient().GetAwaiter().GetResult();
        }
        static async Task ActivateClient()
        {
            //  Add Microsoft.Azure.ServiceBus NuGet Package
            var Connection = Azure.Configuration.ConnectionManager.GetServiceBusListenerConnectionString(); Azure.Configuration.ConnectionManager.GetServiceBusListenerConnectionString();
            Connection = Connection.Replace(";EntityPath=ChargeEntries", "");

            var QueueName = "ChargeEntries";
            Client = new QueueClient(Connection, QueueName);

            Console.WriteLine("======================================================");
            Console.WriteLine("Press ENTER key to exit.");
            Console.WriteLine("======================================================");

            // Register QueueClient's MessageHandler and receive messages in a loop
            RegisterOnMessageHandlerAndReceiveMessages();

            Console.ReadKey();

            await Client.CloseAsync();
        }

        private static void RegisterOnMessageHandlerAndReceiveMessages()
        {
            var MessageOpts = new MessageHandlerOptions(ExceptionReceivedHandler);
            MessageOpts.MaxConcurrentCalls = 1;
            MessageOpts.AutoComplete = false;
            Client.RegisterMessageHandler(ProcessMessageHandler, MessageOpts);
        }

        private static Task ProcessMessageHandler(Message msg, CancellationToken token)
        {
            string Body = Encoding.UTF8.GetString(msg.Body);
            Console.WriteLine(Body);
            return Client.CompleteAsync(msg.SystemProperties.LockToken);
        }

        private static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs ex)
        {
            Console.WriteLine(ex.Exception.Message);
            return Task.CompletedTask;
        }
    }
}
