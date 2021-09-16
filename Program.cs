using System;
using System.Threading;
using System.Device.I2c;
using Sqlite;
using Iot.Device.Bmxx80;
using Iot.Device.Bmxx80.PowerMode;
using ThreadUtils;

namespace BBB
{
    class LedWorker : Worker
    {
        public override void DoWork()
        {
            Console.WriteLine("Starting LED Thread");

            Led led = new Led(Led.UserLed3);
            led.TurnOff();

            while(!_shouldStop)
            {
                led.Blink(1000);
            }

            Console.WriteLine("Exiting LED thread");
        }
    }

    class Bme280Worker : Worker
    {
        public override void DoWork()
        {
            Console.WriteLine("Starting BME280 Thread");

            var i2cSettings = new I2cConnectionSettings(2, Bme280.SecondaryI2cAddress);
            using I2cDevice i2cDevice = I2cDevice.Create(i2cSettings);

            using (Bme280 bme280 = new Bme280(i2cDevice))
            {
                while(!_shouldStop)
                {
                    bme280.SetPowerMode(Bmx280PowerMode.Forced);
                    int measurementTime = bme280.GetMeasurementDuration();
                    Thread.Sleep(measurementTime);
                    bme280.TryReadTemperature(out var tempValue);
                    Console.WriteLine($"Temperature: {tempValue.DegreesCelsius:0.#}\u00B0C");
                    Thread.Sleep(1000);
                }
            }

            Console.WriteLine("Exiting BME280 thread");
        }
    }

    class ButtonWorker : Worker
    {
        public override void DoWork()
        {
            Console.WriteLine("Starting BUTTON Thread");

            Button button = new Button(2, 2);   // P8_07
            button.Open();
            while(!_shouldStop)
            {
                Thread.Sleep(250);
                Console.WriteLine("Button UP: " + button.Read());
            }

            Console.WriteLine("Exiting BUTTON thread");
        }
    }

    class Program
    {
        static int Main(String[] args)
        {
            Console.WriteLine("Entering Main");

            var exitEvent = new ManualResetEvent(false);

            // This is used to handle Ctrl+C
            Console.CancelKeyPress += (sender, eventArgs) => {
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

            return 0;
        }
    }
}   // ns BBB