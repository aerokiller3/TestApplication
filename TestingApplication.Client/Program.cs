using System.Net.Sockets;
using System.Text;

internal class Program
{
    private static string _address;
    private static int _port;
    private static void Main(string[] args)
    {
        Console.WriteLine("Введите адрес сервера");
        _address = Console.ReadLine();

        Console.WriteLine("Введите порт сервера");
        while (!int.TryParse(Console.ReadLine(), out _port))
            Console.WriteLine("Был введён не порт сервера");

        TcpClient client = null;

        try
        {
            client = new TcpClient(_address, _port);
            var stream = client.GetStream();

            Console.WriteLine(ReadFromStream(stream));

            while (true)
            {
                Console.Write("Введите слово целиком или его начало - ");

                var message = Console.ReadLine();
                var data = Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length);
                message = ReadFromStream(stream);
                Console.WriteLine(message);
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            client.Close();
        }
    }

    private static string ReadFromStream(NetworkStream stream)
    {
        var data = new byte[64];
        var builder = new StringBuilder();
        do
        {
            builder.Append(Encoding.UTF8.GetString(data, 0, stream.Read(data, 0, data.Length)));
        }
        while (stream.DataAvailable);

        return builder.ToString();
    }
}
