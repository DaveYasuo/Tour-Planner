using Npgsql;

namespace Server.Rest_API.Common
{
    public interface IDatabase
    {
        NpgsqlConnection Connection();
    }
}