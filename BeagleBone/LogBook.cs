using System;
using System.Device.Gpio;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace Sqlite
{

    class LogBook
    {
        private String data_source;

        //public static readonly (int, string)[] tupleList = new (int, string)[]
        //{
        //    (1, "boot"),
        //    (2, "push")
        //};

        public LogBook(String data_source)
        {
            this.data_source = data_source;
            CreateDB();
        }

        private void CreateDB()
        {
            using (var connection = new SqliteConnection("Data Source=" + data_source))
            {
                connection.Open();

                var cmdCreateLogbook = connection.CreateCommand();
                cmdCreateLogbook.CommandText =
                    @"CREATE TABLE IF NOT EXISTS log (
                            id INTEGER PRIMARY KEY,
                            gpio TEXT,
                            pinValue TEXT,
                            timestamp DATETIME DEFAULT CURRENT_TIMESTAMP
                    )";

                cmdCreateLogbook.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void Log(string gpio, PinValue pinValue)
        {
            using (var connection = new SqliteConnection("Data Source=" + data_source))
            {
                connection.Open();

                var cmdAddEntry = connection.CreateCommand();

                cmdAddEntry.CommandText = $"INSERT INTO log(gpio, pinValue) VALUES({gpio}, {pinValue})";
                cmdAddEntry.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void LogBoot()
        {
            using (var connection = new SqliteConnection("Data Source=" + data_source))
            {
                connection.Open();

                var cmdAddEntry = connection.CreateCommand();

                cmdAddEntry.CommandText = @"INSERT INTO log(event_id) VALUES(1)";
                cmdAddEntry.ExecuteNonQuery();

                connection.Close();
            }
        }
    }

}