using System.Collections.Generic;

namespace BeagleBone
{
    /// <summary>
    /// Interface used to interact with the database
    /// </summary>
    public interface IDBHelper
    {
        /// <summary>
        /// Gets all the records in the database
        /// </summary>
        /// <returns>An <see cref="IEnumerable{PinRecord}"/> of all the records</returns>
        IEnumerable<PinRecord> GetRecords();

        /// <summary>
        /// Logs the supplied <see cref="PinRecord"/> into the database
        /// </summary>
        /// <param name="pinRecord">A record to save</param>
        void Log(PinRecord pinRecord);

        /// <summary>
        /// Gets the amount of <see cref="PinRecord"/> in the database
        /// </summary>
        /// <returns>The number of records</returns>
        int GetRecordCount();
    }
}