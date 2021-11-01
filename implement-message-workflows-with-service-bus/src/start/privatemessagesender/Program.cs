using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace privatemessagesender
{
    class Program
    {

        const string ServiceBusConnectionString = "Endpoint=sb://salesteamapp-sjc-nov1st.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=XDr0MV/cvznAqnhaAkEJlmYrH2+6sxEteEIDPIxBHM4=";
        const string QueueName = "salesmessages";

        static void Main(string[] args)
        {
            Console.WriteLine("Sending a message to the Sales Messages queue...");
            SendSalesMessageAsync().GetAwaiter().GetResult();
            Console.WriteLine("Message was sent successfully.");
        }

        static async Task SendSalesMessageAsync()
        {
            // Create a Queue Client here
            // By leveraging "await using", the DisposeAsync method will be called automatically once the client variable goes out of scope. 
            // In more realistic scenarios, you would want to store off a class reference to the client (rather than a local variable) so that it can be used throughout your program.
            await using var client = new ServiceBusClient(ServiceBusConnectionString);
            await using ServiceBusSender sender = client.CreateSender(QueueName);

             try
            {
                var messageBody = $"$10,000 order for bicycle parts from retailer Adventure Works.";
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
