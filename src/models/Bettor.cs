using pmu.connections;
using Npgsql;

namespace pmu.models;

public class Bettor
{
    /* FIELDS SECTION */
    public int Id { get; private set; }
    public String Name { get; private set; }
    public double Money { get; private set; }

    /* CONSTRUCTOR SECTION */
    public Bettor() { }
    public Bettor(int id, String name, double money)
    {
        this.Id = id;
        this.Name = name;
        this.Money = money;
    }

    /* SETTERS SECTION */
    public void setMoney(double money)
    {
        if (money >= 0)
        {
            this.Money = money;
        }
    }

    /* METHODS SECTION */
    public static List<Bettor> getBettors()
    {
        List<Bettor> list = new List<Bettor>();
        Connection connection = new Connection();
        NpgsqlConnection postgresConnection = connection.getNpgsqlConnection();

        postgresConnection.Open();

        NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM bettor ORDER BY id", postgresConnection);
        NpgsqlDataReader dr = command.ExecuteReader();

        while (dr.Read())
        {
            list.Add(new Bettor((int)dr[0], (String)dr[1], (double)dr[2]));
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
            NpgsqlCommand command = new NpgsqlCommand("UPDATE bettor SET money = " + this.Money + " WHERE id=" + this.Id, postgresConnection);
            command.ExecuteNonQuery();
            tran.Commit();
        }
        catch (Exception e)
        {
            tran.Rollback();
            throw e;
        }
        finally
        {
            postgresConnection.Close();
        }
    }

}
