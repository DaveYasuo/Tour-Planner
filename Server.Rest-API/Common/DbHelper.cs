using System.Data;
using Npgsql;

namespace Server.Rest_API.Common
{
    internal static class DbHelper
    {
        // Handling Null Data
        // See: https://stackoverflow.com/a/1772037
        // And: https://stackoverflow.com/a/52204396
        public static T SafeGet<T>(this NpgsqlDataReader reader, string columnName)
        {
            return reader.IsDBNull(columnName) ? default : reader.GetFieldValue<T>(columnName);
        }
    }
}
