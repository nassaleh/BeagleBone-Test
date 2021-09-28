using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace BeagleBone
{
    public class DBHelper : IDBHelper
    {
        private String dataSource;

        /// <summary>
        /// Constructopr for the DBHelper class
        /// </summary>
        /// <param name="source">A datasource to use for the database</param>
        public DBHelper(string source = "logbook.db")
        {
            this.dataSource = source;
            CreateDB();
        }

        /// <inheritdoc/>
        public void Log(PinRecord pinRecord)
        {
            using (var connection = new SqliteConnection("Data Source=" + dataSource))
            {
                connection.Open();

                var cmdAddEntry = connection.CreateCommand();
                cmdAddEntry.CommandText = $"INSERT INTO PinRecords(Gpio, PinValue, Timestamp) VALUES(\"{pinRecord.Gpio}\", \"{pinRecord.PinValue}\", \"{pinRecord.Timestamp}\")";
                cmdAddEntry.ExecuteNonQuery();
                connection.Close();
            }
        }

        /// <inheritdoc/>
        public IEnumerable<PinRecord> GetRecords()
        {
            List<PinRecord> records = new List<PinRecord>();

            try
            {
                using var connection = new SqliteConnection("Data Source=" + dataSource);
                connection.Open();

                using var cmdGetTable = connection.CreateCommand();
                cmdGetTable.CommandText = "SELECT * from PinRecords";
                using var reader = cmdGetTable.ExecuteReader();

                while (reader.Read())
                {
                    if (!DateTime.TryParse(reader.GetString(3), out DateTime parsedTimestamp))
                    {
                        parsedTimestamp = DateTime.Now;
                    }

                    records.Add(new PinRecord()
                    {
                        Id = Convert.ToInt32(reader.GetValue(0)),
                        Gpio = reader.GetString(1),
                        PinValue = reader.GetString(2),
                        Timestamp = parsedTimestamp
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return records;
        }

        /// <summary>
        /// Creates the database if it doesnt already exist
        /// </summary>
        private void CreateDB()
        {
            using (var connection = new SqliteConnection("Data Source=" + dataSource))
            {
                connection.Open();

                var cmdCreateLogbook = connection.CreateCommand();
                cmdCreateLogbook.CommandText =
                @"CREATE TABLE IF NOT EXISTS PinRecords (
                    Id INTEGER NOT NULL CONSTRAINT PK_PinRecords PRIMARY KEY AUTOINCREMENT,
                    Gpio TEXT NULL,
                    PinValue TEXT NULL,
                    Timestamp TEXT NOT NULL
                )";

                cmdCreateLogbook.ExecuteNonQuery();

                connection.Close();
            }
        }
    }
}