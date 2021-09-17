using System.Device.Gpio;

namespace BeagleBone
{
    public interface IPinReader
    {
        void OpenPin(int pin, PinMode pinMode);
        PinValue Read(int pin);
    }
}