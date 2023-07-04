using RabbitMQ.Client;
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

channel.QueueDeclare(queue: "hello",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

while (true)
{
    string message = Console.ReadLine() ?? string.Empty;

    if (message == string.Empty) break;

    var body = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(exchange: string.Empty,
                         routingKey: "hello",
                         basicProperties: null,
                         body: body);

    Console.WriteLine($" [x] Sent {message}");
}



Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();