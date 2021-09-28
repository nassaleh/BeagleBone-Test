using System;


namespace BeagleBone
{
    public record PinRecord
    {
        public int Id { get; set; }

        public string Gpio { get; set; }

        public string PinValue { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.Now;
    }

}