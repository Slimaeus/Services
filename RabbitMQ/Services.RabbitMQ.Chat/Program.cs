using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory
{
    HostName = "localhost",
    UserName = ConnectionFactory.DefaultUser,
    Password = ConnectionFactory.DefaultPass,
    Port = AmqpTcpEndpoint.UseDefaultPort
};

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "hello1",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

channel.QueueDeclare(queue: "hello2",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

Console.WriteLine(" [*] Waiting for messages.");

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" [x] Received {message}");
};

channel.BasicConsume(queue: "hello1",
                     autoAck: true,
                     consumer: consumer);

const string Name = "User 1";

while (true)
{
    string message = Console.ReadLine() ?? string.Empty;

    if (message == string.Empty) break;

    var body = Encoding.UTF8.GetBytes($"{Name}: {message}");

    channel.BasicPublish(exchange: string.Empty,
                         routingKey: "hello2",
                         basicProperties: null,
                         body: body);

    Console.WriteLine($" [x] Sent {message}");
}



Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();