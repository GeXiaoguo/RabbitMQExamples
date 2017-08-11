using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace PubSub
{
    class Publisher
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {       
                    channel.ExchangeDeclare(exchange:"logsExchange", type: "fanout");
                    
                    Console.WriteLine("Type a message and press [Enter] to publish message to all running subscribers");
                    string message = Console.ReadLine();
                    var body = Encoding.UTF8.GetBytes(message);

                    //message is sent to a fanout exchange
                    //fanout exchange broadcast to all queues it knows
                    //routing key in the message does not matter to routing
                    channel.BasicPublish(exchange: "logsExchange",
                             routingKey: "does not matter",
                             basicProperties: null,
                            body: body);

                    Console.WriteLine(" [x] Sent {0}", message);
                }
                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();                
            }
        }
    }
}
