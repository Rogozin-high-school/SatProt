using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace SatProt
{
    class Program
    {
        static void Main(string[] args)
        {
            Server s = new Server(8080);
            s.MessageHandler = (b) =>
            {
                InMsg msg = new InMsg(b);
                int x = msg.NextInt();
                ;
            };
            s.Start();
        }
    }
}
