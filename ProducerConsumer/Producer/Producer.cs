using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Send
{
    class Producer
    {
        public static void Main()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "queue1",
                                      durable: false,
                                      exclusive: false,
                                      autoDelete: false,
                                      arguments: null);

                    Console.WriteLine("Type a message and press [Enter] to send");

                    string message = Console.ReadLine();

                    var body = Encoding.UTF8.GetBytes(message);

                    //the message is sent to the default exchange
                    //default exchange binds to all queues
                    //the binding routing key is set to the name of the queue
                    //default exchange is a direct exchange
                    //routing rule is the same as direct exchanges: the message routing key has to match the binding routing key exactly
                    channel.BasicPublish(
                        exchange: "",
                        routingKey: "queue1",
                        mandatory: true,
                        basicProperties: null,
                        body: body);

                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                }
            }
        }
    }
}
