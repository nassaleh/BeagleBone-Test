using System.Device.Gpio;

namespace BeagleBone
{
    /// <summary>
    /// An interface used to read the GPIO values of pins
    /// </summary>
    public interface IPinReader
    {
        /// <summary>
        /// Opens a pin in order for it to be ready to use. 
        /// </summary>
        /// <param name="pin">The pin number in the driver's logical numbering scheme.</param>
        /// <param name="pinMode">Which state we want the pin to be in</param>
        void OpenPin(int pin, PinMode pinMode);

        /// <summary>
        /// Reads the current value of a pin.
        /// </summary>
        /// <param name="pin">The pin number in the driver's logical numbering scheme.</param>
        /// <returns>The value of the pin.</returns>
        PinValue Read(int pin);

        /// <summary>
        /// Adds a handler for a pin value changed event.
        /// </summary>
        /// <param name="pins">The pin number in the driver's logical numbering scheme.</param>
        /// <param name="pinChangeEventHandler">Delegate that defines the structure for callbacks when a pin value changed event occurs.</param>
        void RegisterPinsForCallback(int[] pins, PinChangeEventHandler pinChangeEventHandler);
    }
}