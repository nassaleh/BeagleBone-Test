using System;
using System.Collections.Generic;
using System.Linq;
using BeagleBone;


namespace Sqlite
{

    public class DBHelper : IDBHelper
    {
        public DBHelper()
        {
            using (PinContext db = new PinContext())
            {
                db.Database.EnsureCreated();
            }
        }

        public void Log(PinRecord pinRecord)
        {
            using (PinContext db = new PinContext())
            {
                db.PinRecords.Add(pinRecord);
                Console.Write($"{pinRecord.Gpio}: {pinRecord.PinValue} | ");
                db.SaveChanges();
            }
        }

        public IEnumerable<PinRecord> GetRecords()
        {
            using (PinContext db = new PinContext())
            {
                foreach (var record in db.PinRecords)
                {
                    Console.WriteLine($"{record.Id} | {record.Gpio} | {record.PinValue} | {record.Timestamp}");
                }

                return db.PinRecords.ToList();
            }
        }

    }

    public record PinRecord
    {
        public int Id { get; set; }

        public string Gpio { get; set; }

        public string PinValue { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.Now;
    }

}