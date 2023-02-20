using System.Net;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace WireMockServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var server = WireMock.Server.WireMockServer.Start();

            Console.WriteLine($"Server is running in port {server.Urls.First()}");

            server
                .Given(Request
                    .Create()
                    .WithPath("/office/hu")
                    .UsingGet()
                    )
                .RespondWith(Response
                    .Create()
                    .WithBody("Hubsson it is.")
                    .WithStatusCode(HttpStatusCode.OK)
                );

            server
                .Given(Request
                    .Create()
                    .WithPath("/office/gr")
                    .UsingGet()
                )
                .RespondWith(Response
                    .Create()
                    .WithBody("There is no office in Greece.")
                    .WithStatusCode(HttpStatusCode.NotFound)
                );

            Console.ReadKey();

            server.Stop();
            server.Dispose();
        }
    }
}