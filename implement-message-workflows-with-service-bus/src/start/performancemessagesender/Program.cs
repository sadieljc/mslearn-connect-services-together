using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace performancemessagesender
{
    class Program
    {
        const string ServiceBusConnectionString = "Endpoint=sb://salesteamapp-sjc-nov4th.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=OttwzqMcO85mohyGkBI++56o5d97qo9l+UUoMCPVN24=";
        const string TopicName = "salesperformancemessages";

        static void Main(string[] args)
        {
            Console.WriteLine("Sending a message to the Sales Performance topic...");
            SendPerformanceMessageAsync().GetAwaiter().GetResult();
            Console.WriteLine("Message was sent successfully.");
        }

        static async Task SendPerformanceMessageAsync()
        {
            // Create a Topic Client here
            await using var client = new ServiceBusClient(ServiceBusConnectionString);
            await using ServiceBusSender sender = client.CreateSender(TopicName);

            // Send messages.
            try
            {
                var messageBody = "Total sales for Brazil in August: $13m.";
                var message = new ServiceBusMessage(messageBody);
                Console.WriteLine($"Sending message: {messageBody}");
                await sender.SendMessageAsync(message);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }
        }
    }
}
