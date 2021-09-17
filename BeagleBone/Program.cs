using System;
using System.Threading;
using Sqlite;
using BeagleBone;
using System.Device.Gpio;
using System.Device.Gpio.Drivers;

namespace BBB
{
    class Program
    {
        static void Main(String[] args)
        {

            ReadPins();
        }

        public static void ReadPins()
        {
            LogBook logger = new LogBook("logbook.db");
            int[] pins = { 3, 4, 5, 6 };

            //IPinReader pinReader = new PinReader(2);
            IPinReader pinReader = new MockReader();

            try
            {
                foreach (var pin in pins)
                {
                    pinReader.OpenPin(pin, PinMode.Input);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            foreach (var pin in pins)
            {
                var value = $"{pinReader.Read(pin)}_ON_BOOT";
                logger.Log(pin.ToString(), value);
                Console.Write($"{pin}: {value} | ");
            }

            Console.WriteLine("Pin Values");

            int i = 0;
            while (i++ < 60)
            {
                foreach (var pin in pins)
                {
                    var value = pinReader.Read(pin).ToString();
                    logger.Log(pin.ToString(), value);
                    Console.Write($"{pin}: {value} | ");
                }
                Console.WriteLine();
                Thread.Sleep(1000);
            }
        }

        public void VinceMain()
        {
            Console.WriteLine("Entering Main");

            var exitEvent = new ManualResetEvent(false);

            // This is used to handle Ctrl+C
            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                eventArgs.Cancel = true;
                exitEvent.Set();
            };

            LogBook log = new LogBook("logbook.db");
            log.LogBoot();

            LedWorker ledWorker = new LedWorker();
            Thread ledThread = new Thread(ledWorker.DoWork);
            ledThread.Start();

            // Bme280Worker bmeWorker = new Bme280Worker();
            // Thread bmeThread = new Thread(bmeWorker.DoWork);
            // bmeThread.Start();

            ButtonWorker buttonWorker = new ButtonWorker();
            Thread buttonThread = new Thread(buttonWorker.DoWork);
            buttonThread.Start();

            // Wait for Ctrl+C
            exitEvent.WaitOne();

            ledWorker.RequestStop();
            // bmeWorker.RequestStop();
            buttonWorker.RequestStop();

            ledThread.Join();
            // bmeThread.Join();
            buttonThread.Join();

            Console.WriteLine("Exiting Main");
        }
    }
}   // ns BBB