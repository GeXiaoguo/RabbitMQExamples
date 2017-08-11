using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

class Receive
{
    public static void Main()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        {
            using (var channel = connection.CreateModel())
            {
                var consumer = new EventingBasicConsumer(channel);

                //the same exchange is also declared in the publisher
                channel.ExchangeDeclare(exchange:"logsExchange", type: "fanout");
                    
                //QueueName is generated randomly
                string queueName = channel.QueueDeclare().QueueName;

                //bind the queue to the exchange to receive all the broadcast messages
                channel.QueueBind(
                    queue: queueName,
                    exchange : "logsExchange",
                    routingKey : "does not matter"
                );

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] {0}", message);
                };

                channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);
            
                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
