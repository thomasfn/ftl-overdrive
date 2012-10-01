using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTLOverdrive.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Root.Singleton.Init();
            Root.Singleton.Run();
            Root.Singleton.Shutdown();
        }
    }
}
