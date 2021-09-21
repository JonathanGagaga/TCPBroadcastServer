using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JonathanServer
{
    public class Program
    {
        static void Main(string[] args)
        {
            ServerControl server = new ServerControl();
            server.Start();

            Console.ReadKey();
        }
       
    }

}
