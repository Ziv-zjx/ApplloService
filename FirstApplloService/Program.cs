using System;
using System.Threading;

namespace FirstApplloService
{
    class Program
    {
        static void Main(string[] args)
        {
            ApolloClient client = new ApolloClient();
            client.LinkTCP();
            client.Broker_Public();

            //10秒之后自动关闭
            Thread.Sleep(10000);
            client.Broker_Disconnect();
            Console.ReadKey();
        }
    }
}
