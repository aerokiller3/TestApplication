using System.Net;
using System.Net.Sockets;
using TestingApplication.Infrastructure.Constants;
using TestingApplication.Infrastructure.Models;
using TestingApplication.Infrastructure.Services;

internal class Program
{
    private static TcpListener _listener;

    private static void Main(string[] args)
    {
        Console.WriteLine("Введите номер порта, на котором будет развёрнут сервер");
        int port;
        while (!int.TryParse(Console.ReadLine(), out port))
            Console.WriteLine("Некорректный номер порта");

        ServerConstants.Port = port;
        Task.Run(() => StartServer());

        Console.WriteLine($"Инициализация БД прошла успешно\n{ServerConstants.Menu}");

        RunMenu();
    }

    private static void RunMenu()
    {
        while (true)
        {
            var input = Console.ReadLine().Trim().ToLowerInvariant();

            try
            {
                switch (input)
                {
                    case "1":
                        CreateDictionary();
                        break;
                    case "2":
                        UpdateDictionary();
                        break;
                    case "3":
                        ClearDictionary();
                        break;
                    case "exit":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Введена неизвестная команда");
                        break;
                }

                Console.WriteLine(ServerConstants.Menu);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    private static void CreateDictionary()
    {
        Console.WriteLine("Укажите путь к файлу, из которого будет создан словарь");
        var path = Console.ReadLine().Trim();

        var size = DictionaryService.CreateDictionary(path);

        Console.WriteLine($"Словарь создан. Добавлено {size} слов");
    }

    private static void UpdateDictionary()
    {
        Console.WriteLine("Укажите путь к файлу, из которого будет обновлен словарь");
        var path = Console.ReadLine().Trim();

        var size = DictionaryService.UpdateDictionary(path);
        Console.WriteLine($"Словарь создан. Получено {size} слов");
    }

    private static void ClearDictionary()
    {
        Console.WriteLine("Начата операция очистки словаря");
        var size = DictionaryService.ClearDictionary();
        Console.WriteLine($"Словарь очищен. Было удалено {size} слов");
    }

    private static void StartServer()
    {
        try
        {
            _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), ServerConstants.Port);
            _listener.Start();
            var i = 0;

            while (true)
            {
                i++;
                var client = new Client(_listener.AcceptTcpClient(), i);
                Console.WriteLine($"Пользователь подключен. ClientId - {i}");

                var clientThread = new Thread(new ThreadStart(client.Process));
                clientThread.Start();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            _listener?.Stop();
        }
    }
}