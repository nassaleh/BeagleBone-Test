using System;
using System.Device.Gpio;
using System.Device.Gpio.Drivers;

namespace BeagleBone
{
    /// <summary>
    /// Class used to read GPIO pin values
    /// </summary>
    public class PinReader : IPinReader
    {
        /// <summary>
        /// Constructor used fogr this class
        /// </summary>
        /// <param name="gpioChip">The chip number to read from</param>
        public PinReader(int gpioChip)
        {
            var driver = new LibGpiodDriver(gpioChip);
            gpioController = new GpioController(PinNumberingScheme.Logical, driver);
        }

        /// <inheritdoc/>
        public PinValue Read(int pin)
        {
            return gpioController.Read(pin);
        }

        /// <inheritdoc/>
        public void OpenPin(int pin, PinMode pinMode)
        {
            gpioController.OpenPin(pin, pinMode);
        }

        /// <inheritdoc/>
        public void RegisterPinsForCallback(int[] pins, PinChangeEventHandler pinChangeEventHandler)
        {
            foreach (var pin in pins)
            {
                gpioController.RegisterCallbackForPinValueChangedEvent(pin, PinEventTypes.Rising, pinChangeEventHandler);
                gpioController.RegisterCallbackForPinValueChangedEvent(pin, PinEventTypes.Falling, pinChangeEventHandler);
            }
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

        public void RegisterPinsForCallback(int[] pins, PinChangeEventHandler pinChangeEventHandler)
        {
        }
    }
}
