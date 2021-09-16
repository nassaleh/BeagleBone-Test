using System;
using System.Device.I2c;
using System.Threading;
using BBB;
using Iot.Device.Bmxx80;
using Iot.Device.Bmxx80.PowerMode;
using ThreadUtils;

namespace BeagleBone
{
    public class Bme280Worker : Worker
    {
        public override void DoWork()
        {
            Console.WriteLine("Starting BME280 Thread");

            var i2cSettings = new I2cConnectionSettings(2, Bme280.SecondaryI2cAddress);
            using I2cDevice i2cDevice = I2cDevice.Create(i2cSettings);

            using (Bme280 bme280 = new Bme280(i2cDevice))
            {
                while (!_shouldStop)
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

    public class ButtonWorker : Worker
    {
        public override void DoWork()
        {
            Console.WriteLine("Starting BUTTON Thread");

            Button button = new Button(2, 2);   // P8_07
            button.Open();
            while (!_shouldStop)
            {
                Thread.Sleep(250);
                Console.WriteLine("Button UP: " + button.Read());
            }

            Console.WriteLine("Exiting BUTTON thread");
        }
    }

    public class LedWorker : Worker
    {
        public override void DoWork()
        {
            Console.WriteLine("Starting LED Thread");

            Led led = new Led(Led.UserLed3);
            led.TurnOff();

            while (!_shouldStop)
            {
                led.Blink(1000);
            }

            Console.WriteLine("Exiting LED thread");
        }
    }
}
