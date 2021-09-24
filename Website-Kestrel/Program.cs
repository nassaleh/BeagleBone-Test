using System;
using System.Device.Gpio;
using System.Device.Gpio.Drivers;
using System.Threading;
using System.Device.I2c;
using Sqlite;
using ThreadUtils;

using Web;
using BeagleBone;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.IO;

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
            PinManager pm = new PinManager(pins, new MockReader(), new DBHelper());

            pm.Initialize();

            //using var webWorker = new wsWorker();
            //Thread webThread = new Thread(webWorker.DoWork);
            Thread pinManagerThread = new Thread(pm.Run);
            pinManagerThread.Start();
            //webThread.Start();

            //exitEvent.WaitOne();

            //webWorker.RequestStop();

            //webThread.Join();

            var host = new WebHostBuilder()
               .UseKestrel()
               .UseUrls("http://192.168.1.7:8069", "http://localhost:8069")
               .UseContentRoot(Directory.GetCurrentDirectory())
               .UseIISIntegration()
               .UseStartup<Startup>()
               .Build();

            host.Run();
        }
    }
}   // ns BBB