using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Gpio;
using System.Device.Gpio.Drivers;

namespace BeagleBone
{
    public class PinReader : IPinReader
    {
        public PinReader(int gpioChip)
        {
            var driver = new LibGpiodDriver(gpioChip);
            gpioController = new GpioController(PinNumberingScheme.Logical, driver);
        }

        public PinValue Read(int pin)
        {
            return Read(pin);
        }

        public void OpenPin(int pin, PinMode pinMode)
        {
            gpioController.OpenPin(pin, pinMode);
        }

        private GpioController gpioController;
    }

    public class MockReader : IPinReader
    {
        public void OpenPin(int pin, PinMode pinMode)
        {
            Console.WriteLine($"Set pin {pin} to {pinMode}");
        }

        public PinValue Read(int pin)
        {
            Random r = new Random();
            var value = r.Next(0, 2);
            return value == 1 ? PinValue.High : PinValue.Low;
        }
    }
}
