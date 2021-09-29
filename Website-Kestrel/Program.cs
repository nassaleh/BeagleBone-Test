using System;
using System.Threading;
using BeagleBone;
using Web;

namespace BBB
{
    class Program
    {
        static void Main(String[] args)
        {
            Console.WriteLine("Entering Main");

            var exitEvent = new ManualResetEvent(false);

            // This is used to handle Ctrl+C
            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                eventArgs.Cancel = true;
                exitEvent.Set();
            };

            int[] pins = { 3, 4, 5, 6 };
            PinManager pm = new PinManager(pins, new PinReader(2), new DBHelper("logbook.db"));
            //PinManager pm = new PinManager(pins, new MockReader(), new DBHelper("logbook.db"));

            pm.Initialize();

            using var webWorker = new wsWorker();
            Thread webThread = new Thread(webWorker.DoWork);
            webThread.Start();
        }
    }
}