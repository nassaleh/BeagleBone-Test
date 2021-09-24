using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sqlite;

namespace BeagleBone
{
    public class PinManager
    {
        private IPinReader pinReader;
        private IDBHelper dbHelper;
        private int[] pins;

        public PinManager(int[] pins, IPinReader pinReader, IDBHelper dBContext)
        {
            this.pinReader = pinReader;
            this.dbHelper = dBContext;
            this.pins = pins;
        }

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
                //logger.Log(pin.ToString(), value);
                dbHelper.Log(new PinRecord()
                {
                    Gpio = pin.ToString(),
                    PinValue = value
                });
                Console.Write($"{pin}: {value} | ");
            }
            Console.WriteLine();

            pinReader.RegisterPinsForCallback(pins, PinChangeStateEventHandler); // TODO: Unregister?
        }

        public void PinChangeStateEventHandler(object sender, PinValueChangedEventArgs pinValueChangedEventArgs)
        {
            var record = new PinRecord()
            {
                Gpio = pinValueChangedEventArgs.PinNumber.ToString(),
                PinValue = pinValueChangedEventArgs.ChangeType.ToValue()
            };
            dbHelper.Log(record);
            Console.WriteLine($"{record.Gpio}: {record.PinValue}");
        }



        public void Run()
        {
            while (true)
            {
                Thread.Sleep(500);
            }
        }

    }

    public static class ExtensionMethods
    {
        public static string ToValue(this PinEventTypes pinType)
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
