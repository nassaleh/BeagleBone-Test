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
            // PinReader pinReader = new PinReader();

            int pin = 3;
            var driver = new LibGpiodDriver(2);
            GpioController gpioController = new GpioController(PinNumberingScheme.Logical, driver);

            try
            {
                gpioController.OpenPin(pin, PinMode.Input);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            while (true)
            {
                var value = gpioController.Read(pin);
                Console.WriteLine($"Pin {pin}: {value}");
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