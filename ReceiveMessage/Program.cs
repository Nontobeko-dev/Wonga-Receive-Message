using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace ReceiveMessage
{
    class Program
    {
        static void Main(string[] args)
        {
            // Set up the factory
            var factory = new ConnectionFactory() { HostName = "localHost" };

            // Factory connection
            using (var connection = factory.CreateConnection())

            // New channel for pushing the message
            using (var channel = connection.CreateModel())
            {
                // Queue properties
                channel.QueueDeclare(queue: "messageSaver",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
                // This section is the consumer section that contains asynchronous method. This where the channel is consumed
                var consumer = new EventingBasicConsumer(channel);

                // The event collects the data/message
                consumer.Received += (model, ea) => // this is an event. ea is an argument
                {
                    // This event  Handler handles asynchronous delivery
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    Console.WriteLine("------------------------ MESSAGE RECEIVED -------------------------");
                    Console.WriteLine("{0}", message);

                    // Extract name from the string 
                    string sendersName = message.Substring(message.IndexOf(",") + 2);

                    // Output Message
                    Console.WriteLine("\n------------------- RESPONSE TO RECEIVED MESSAGE -------------------");
                    Console.WriteLine($"Hello {sendersName}, I am your father");
                    Console.WriteLine("\nPress [enter] to exit");

                    Console.WriteLine();
                };
                // Consume the data
                channel.BasicConsume("messageSaver",
                    autoAck: true,
                    consumer: consumer);

                Console.ReadLine();
            }
        }
    }
}
