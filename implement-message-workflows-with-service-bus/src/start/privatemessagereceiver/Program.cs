using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace privatemessagereceiver
{
    class Program
    {

        const string ServiceBusConnectionString = "Endpoint=sb://salesteamapp-sjc-nov1st.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=XDr0MV/cvznAqnhaAkEJlmYrH2+6sxEteEIDPIxBHM4=";
        const string QueueName = "salesmessages";

        static void Main(string[] args)
        {
            ReceiveSalesMessageAsync().GetAwaiter().GetResult();
        }

        static async Task ReceiveSalesMessageAsync()
        {
            Console.WriteLine("======================================================");
            Console.WriteLine("Press ENTER key to exit after receiving all the messages.");
            Console.WriteLine("======================================================");

            // Create a Queue Client here
            var client = new ServiceBusClient(ServiceBusConnectionString);

            var processorOptions = new ServiceBusProcessorOptions
            {
                MaxConcurrentCalls = 1,
                AutoCompleteMessages = false
            };

            ServiceBusProcessor processor = client.CreateProcessor(QueueName, processorOptions);

            processor.ProcessMessageAsync += ProcessMessagesAsync;
            processor.ProcessErrorAsync += ExceptionReceivedHandler;

            await processor.StartProcessingAsync();
            
            Console.Read();

            // Close the queue here
            await processor.DisposeAsync();
            await client.DisposeAsync();
        }

        static async Task ProcessMessagesAsync(ProcessMessageEventArgs args)
        {
            Console.WriteLine($"Received message: SequenceNumber:{args.Message.SequenceNumber} Body:{args.Message.Body}");
            await args.CompleteMessageAsync(args.Message);
        }

        static Task ExceptionReceivedHandler(ProcessErrorEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {exceptionReceivedEventArgs.FullyQualifiedNamespace}");
            Console.WriteLine($"- Entity Path: {exceptionReceivedEventArgs.EntityPath}");
            Console.WriteLine($"- Executing Action: {exceptionReceivedEventArgs.ErrorSource}");
            return Task.CompletedTask;
        }   
    }
}
