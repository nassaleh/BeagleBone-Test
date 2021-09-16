using System;
using System.IO;
using System.Threading;


namespace BBB
{
    class Led
    {
        public const string UserLed0 = "/sys/devices/platform/leds/leds/beaglebone:green:usr0/brightness";
        public const string UserLed1 = "/sys/devices/platform/leds/leds/beaglebone:green:usr1/brightness";
        public const string UserLed2 = "/sys/devices/platform/leds/leds/beaglebone:green:usr2/brightness";
        public const string UserLed3 = "/sys/devices/platform/leds/leds/beaglebone:green:usr3/brightness";


        public Led(string path)
        {
            this.path = path;
            Console.WriteLine(path);
            Console.WriteLine(this.path);
        }

        private string path;

        public void TurnOn()
        {
            Set('1');
        }

        public void TurnOff()
        {
            Set('0');
        }

        public void Set(char value)
        {
            using (var led = new StreamWriter(path))
            {
                led.WriteAsync(value);
            }
        }

        public void Blink(int delayMs)
        {
            TurnOn();
            Thread.Sleep(delayMs);
            TurnOff();
            Thread.Sleep(delayMs);
        }

    }
}   // ns BBB