using System;
using System.ComponentModel;
using System.Device.Gpio;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;


namespace Sqlite
{

    class DBHelper
    {
        private String data_source;

        public DBHelper(String data_source)
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

        public record PinRecord
        {
            public int Id { get; set; }
            public string Gpio { get; set; }
            public string PinValue { get; set; }

            public DateTime Timestamp { get; set; } = DateTime.Now;
        }

        public void Log(string gpio, string pinValue)
        {
            using (var connection = new SqliteConnection("Data Source=" + data_source))
            {
                connection.Open();

                var cmdAddEntry = connection.CreateCommand();

                cmdAddEntry.CommandText = $"INSERT INTO log(gpio, pinValue) VALUES(\"{gpio}\", \"{pinValue}\")";
                cmdAddEntry.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void GetRecords()
        {
            using (var connection = new SqliteConnection("Data Source=" + data_source))
            {
                connection.Open();

                using (SqliteCommand fmd = connection.CreateCommand())
                {
                    //fmd.CommandText = @"SELECT DISTINCT FileName FROM Import";
                    //fmd.CommandType = CommandType.Text;
                    //SQLiteDataReader r = fmd.ExecuteReader();
                    //while (r.Read())
                    //{
                    //    ImportedFiles.Add(Convert.ToString(r["FileName"]));
                    //}
                }

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