using System;
using System.Device.Gpio;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace BeagleBone
{
    /// <summary>
    /// Represents a class that listens to Pin change events and logs them to the <see cref="IDBHelper"/>
    /// </summary>
    public class PinManager
    {
        private IPinReader pinReader;
        private IDBHelper dbHelper;
        private int[] pins;
        private Display display;

        /// <summary>
        /// Constructor for the class
        /// </summary>
        /// <param name="pins">The pins to listen to for even changes</param>
        /// <param name="pinReader">An instance of <see cref="IPinReader"/> to read the events</param>
        /// <param name="dBContext">Used to log the events</param>
        public PinManager(int[] pins, IPinReader pinReader, IDBHelper dBContext)
        {
            this.pinReader = pinReader;
            this.dbHelper = dBContext;
            this.pins = pins;
        }

        /// <summary>
        /// Used to log the initial Pin Values and subscribe to the events for Pin changes
        /// </summary>
        public void Initialize()
        {
            try
            {
                foreach (var pin in pins)
                {
                    Console.WriteLine($"Opening pin {pin}");
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
                dbHelper.Log(new PinRecord()
                {
                    Gpio = pin.ToString(),
                    PinValue = value
                });
            }
            Console.WriteLine();

            pinReader.RegisterPinsForCallback(pins, PinChangeStateEventHandler); // TODO: Unregister?

            this.display = new Display(new System.Device.I2c.I2cConnectionSettings(2, 0x3c));




            var ipV4s = NetworkInterface.GetAllNetworkInterfaces()
            .Select(i => i.GetIPProperties().UnicastAddresses)
            .SelectMany(u => u)
            .Where(u => u.Address.AddressFamily == AddressFamily.InterNetwork)
            .Select(i => i.Address);

            var ipList = string.Join("\n", ipV4s.Select(x => x.ToString()));

            display.WriteLine(ipList);
        }

        /// <summary>
        /// Delegate that defines the structure for callbacks when a pin value changed event occurs.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="pinValueChangedEventArgs">The pin value changed arguments from the event.</param>
        public void PinChangeStateEventHandler(object sender, PinValueChangedEventArgs pinValueChangedEventArgs)
        {
            var record = new PinRecord()
            {
                Gpio = pinValueChangedEventArgs.PinNumber.ToString(),
                PinValue = pinValueChangedEventArgs.ChangeType.ToStringValue()
            };
            dbHelper.Log(record);
            Console.WriteLine($"{record.Gpio}: {record.PinValue}");
        }
    }

    public static class ExtensionMethods
    {
        public static string ToStringValue(this PinEventTypes pinType)
        {
            switch (pinType)
            {
                case PinEventTypes.Falling:
                    return PinValue.Low.ToString();
                case PinEventTypes.Rising:
                    return PinValue.High.ToString();
                default:
                    return PinEventTypes.None.ToString();
            }
        }
    }
}
