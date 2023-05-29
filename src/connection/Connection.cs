using Npgsql;

namespace pmu.connections

{
    public class Connection
    {
        public NpgsqlConnection getNpgsqlConnection()
        {
            NpgsqlConnection connection = new NpgsqlConnection("Server=localhost;User Id=walker; " +
            "Password=w41k4z!;Database=pmu;");
            return connection;
        }

    }
}
