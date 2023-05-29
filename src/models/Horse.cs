using Npgsql;
using pmu.connections;

namespace pmu.models;

public class Horse
{
    /* FIELDS SECTION */
    public int Id { get; private set; }
    public String Name { get; private set; }
    public double Speed { get; private set; }
    public double Endurance { get; private set; }
    public double EnduranceActivation { get; private set; }
    public int Wins { get; private set; }
    public int Losses { get; private set; }

    /* CONSTRUCTOR SECTION */
    public Horse() { }
    public Horse(int id, String name, double speed, double endurance, double enduranceActivation, int wins, int losses)
    {
        this.Id = id;
        this.Name = name;
        this.Speed = speed;
        this.Endurance = endurance;
        this.EnduranceActivation = enduranceActivation;
        this.Wins = wins;
        this.Losses = losses;
    }

    /* SETTERS SECTION */
    public void setWin(int win)
    {
        if (win >= 0)
        {
            this.Wins = win;
        }
    }
    public void setLosses(int loss)
    {
        if (loss >= 0)
        {
            this.Losses = loss;
        }
    }

    /* METHODS SECTION */
    public static List<Horse> getHorses()
    {
        List<Horse> list = new List<Horse>();
        Connection connection = new Connection();
        NpgsqlConnection postgresConnection = connection.getNpgsqlConnection();

        postgresConnection.Open();

        NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM horse ORDER BY id", postgresConnection);
        NpgsqlDataReader dr = command.ExecuteReader();

        while (dr.Read())
        {
            list.Add(new Horse((int)dr[0], (String)dr[1], (double)dr[2], (double)dr[3], (double)dr[4], (int)dr[5], (int)dr[6]));
        }

        postgresConnection.Close();

        return list;
    }

    public void updateBettor()
    {
        Connection connection = new Connection();
        NpgsqlConnection postgresConnection = connection.getNpgsqlConnection();

        postgresConnection.Open();

        NpgsqlTransaction tran = postgresConnection.BeginTransaction();
        try
        {
            NpgsqlCommand command = new NpgsqlCommand("UPDATE horse SET wins = @win, losses = @loss WHERE id = @id", postgresConnection);
            command.Parameters.AddWithValue("@win", this.Wins);
            command.Parameters.AddWithValue("@loss", this.Losses);
            command.Parameters.AddWithValue("@id", this.Id);
            command.ExecuteNonQuery();
            tran.Commit();
        }
        catch (Exception ex)
        {
            tran.Rollback();
            throw ex;
        }
        finally
        {
            postgresConnection.Close();
        }
    }
}