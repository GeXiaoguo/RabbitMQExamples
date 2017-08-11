using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace TaskProducer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {     
                    channel.QueueDeclare(queue: "task_queue",
                                      durable: false,
                                      exclusive: false,
                                      autoDelete: false,
                                      arguments: null);

                    string line = Console.ReadLine();
                    var body = Encoding.UTF8.GetBytes(line);

                    //the message is sent to the default exchange
                    //default exchange binds to all queues
                    //the binding routing key is set to the name of the queue
                    //default exchange is a direct exchange
                    //routing rule is the same as direct exchanges: the message routing key has to match the binding routing key exactly
                    channel.BasicPublish(
                        exchange: "",
                        routingKey: "task_queue",
                        mandatory: true,
                        basicProperties: null,
                        body: body);

                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();                      
                }
            }
        }

        private static string GetMessage(string[] args)
        {
            return ((args.Length > 0) ? string.Join(" ", args) : "Hello World!");
        }        
    }
}
