using System.Net.Sockets;
using System.Net;
using System.Text;

namespace HttpRequester
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            TcpListener listener = new TcpListener(IPAddress.Loopback, 12345);

            //openConnection
            listener.Start();

            //work untill client send request
            while (true)
            {
                //set using to close conntection
                //acceptClient
                var client = listener.AcceptTcpClient();
                using (var stream = client.GetStream())
                {
                    //readFromStream
                    byte[] buffer = new byte[10000];

                    var length = stream.Read(buffer, 0, buffer.Length);

                    Encoding.UTF8.GetString(buffer).ToString();
                    var requestString = Encoding.UTF8.GetString(buffer, 0, buffer.Length).ToString();


                    //response
                    string html = $"<h1>Hello From Anzhela {DateTime.UtcNow}";

                    string newLine = "\r\n";
                    string response =
                        "HTTP/1.1 200 OK" + newLine +
                        "Server: Appache/1.3.29 Win(64)" + newLine +
                        "Accept-Ranges: bytes" + newLine +
                        "Content-Type: text/html" + newLine +
                        "Content-Length: " + html.Length + newLine +
                        newLine +
                        html + newLine;

                    string responseRedirect =
                        "HTTP/1.1 307 Redirect" + newLine +
                        "Server: Appache/1.3.29 Win(64)" + newLine +
                        "Location: https://www.google.com" + newLine +
                        "Accept-Ranges: bytes" + newLine +
                        "Content-Type: text/html" + newLine +
                        "Content-Length: " + html.Length + newLine +
                        newLine +
                        html + newLine;

                    var responseBytes = Encoding.UTF8.GetBytes(response);


                    stream.Write(responseBytes);
                }
            }

            //Console.OutputEncoding = Encoding.UTF8;
            //await ReadHeaders();
            //await ReadHtml();
        }
        public static async Task ReadHtml()
        {
            string url = "https://softuni.bg/";
            HttpClient client = new HttpClient();

            var html = await client.GetStringAsync(url);

            Console.WriteLine(html);
        }

        public static async Task ReadHeaders()
        {
            string url = "https://softuni.bg/";
            HttpClient client = new HttpClient();

            var response = await client.GetAsync(url);

            response.Headers.Add("X-Test", "test/json");
            var headers = response.Headers.Select(x => x.Key + $"{x.Value.First()}");

            Console.WriteLine(string.Join(Environment.NewLine, headers));
        }
    }
}