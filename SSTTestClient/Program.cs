using SSTTestClient.SSTMonitorService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSTTestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            SSTMonitorServiceClient client = new SSTMonitorServiceClient();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Status: {0}", client.GetStatus());
                Console.WriteLine("{0}: Status updated!", DateTime.Now.ToLongTimeString());
                Console.WriteLine("Press Enter to get the new status.");
                Console.ReadLine();
            }
        }
    }
}
