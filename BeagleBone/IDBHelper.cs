using System.Collections.Generic;

namespace Sqlite
{
    public interface IDBHelper
    {
        IEnumerable<PinRecord> GetRecords();
        void Log(PinRecord pinRecord);
    }
}