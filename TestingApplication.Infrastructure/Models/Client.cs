using Autofac;
using System.Net.Sockets;
using System.Text;
using TestingApplication.Infrastructure.AutofacModule;
using TestingApplication.Infrastructure.Services.Interfaces;

namespace TestingApplication.Infrastructure.Models
{
    public class Client
    {
        private TcpClient _tcpClient;
        private int _clientId;

        public Client(TcpClient tcpClient, int clientId)
        {
            _tcpClient = tcpClient;
            _clientId = clientId;
        }

        public void Process()
        {
            NetworkStream stream = null;

            try
            {
                var builder = new ContainerBuilder();
                builder.RegisterModule<TestApplicationAutofacModuleBuilder>();

                var container = builder.Build();

                var searchService = container.Resolve<ISearchService>();

                stream = _tcpClient.GetStream();
                var data = new byte[64];
                var firstMessage = $"Вам присвоен id - {_clientId}";
                data = Encoding.UTF8.GetBytes(firstMessage);
                stream.Write(data, 0, data.Length);

                while(true)
                {
                    var str = new StringBuilder();
                    var bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        str.Append(Encoding.UTF8.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    var message = str.ToString();

                    Console.WriteLine($"Client №{_clientId}: {message}");

                    message = searchService.SearchWordsBySubstring(message);
                    data = Encoding.UTF8.GetBytes(message);
                    stream.Write(data, 0, data.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                stream?.Close();
                _tcpClient?.Close();
            }
        }
    }
}
