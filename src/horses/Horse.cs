using pmu.forms.details;
using pmu.forms;

namespace pmu.horses;

public class Horse
{
    /* FIELDS SECTION */
    public static readonly int HORSE_SIZE = Track.TRACK_WIDTH / 2;
    public int HorseId { get; private set; }
    public double HorseSpeed { get; private set; } // this will be converted to an angular speed
    public double HorseEndurance { get; private set; }
    public double HorseEnduranceActivation { get; private set; }
    public static readonly int HORSE_STARTING_POSITION = 0;
    public int TrackNumber { get; private set; }
    public int Horse_Initial_X_Position { get; private set; }
    public int Horse_Initial_Y_Position { get; private set; }
    public int Horse_X_Position { get; private set; }
    public int Horse_Y_Position { get; private set; }
    public int TrackHeight { get; private set; }
    public int TrackWidth { get; private set; }
    public double TrackPerimeter = 632; // found by testing
    public double DistanceTraveled { get; private set; }
    public int CurrentLap { get; private set; }
    public int LapsToFinish { get; private set; }
    public double Angle { get; private set; }
    public String Color { get; private set; }
    public int isPassing = 0; // scale 
    public bool hasArrived = false;
    public DateTime ArrivalTime { get; private set; }


    /* CONSTRUCTOR SECTION */
    public Horse(int id, int trackNumber, Point initialPosition, double horse_speed = 1, double endurance = 0, double activation = 50, string color = "blue", int laps = 1)
    {
        this.HorseId = id;
        this.TrackNumber = trackNumber;
        this.Horse_X_Position = initialPosition.X;
        this.Horse_Y_Position = initialPosition.Y;
        this.Horse_Initial_X_Position = initialPosition.X;
        this.Horse_Initial_Y_Position = initialPosition.Y;
        this.TrackHeight = Racetrack.MAIN_TRACK_HEIGHT + (this.TrackNumber * Track.TRACK_WIDTH * 2) - Track.TRACK_WIDTH;
        this.TrackWidth = Racetrack.MAIN_TRACK_WIDTH + (this.TrackNumber * Track.TRACK_WIDTH * 2) - Track.TRACK_WIDTH;

        double a = this.TrackWidth / 2;
        double b = this.TrackHeight / 2;
        this.DistanceTraveled = 0;
        this.CurrentLap = 0;

        this.HorseSpeed = horse_speed;
        this.HorseEndurance = endurance;
        this.HorseEnduranceActivation = activation;
        this.Color = color;
        this.LapsToFinish = laps;

        double x = this.Horse_X_Position - Racetrack.MAIN_ELLIPSE_CENTER.X;
        double y = this.Horse_Y_Position - Racetrack.MAIN_ELLIPSE_CENTER.Y;
        this.Angle = Math.Atan2(y / (this.TrackHeight / 2), x / (this.TrackWidth / 2));
    }

    /* METHODS SECTION */
    public void Move()
    {
        double boost = this.DistanceTraveled >= ((this.HorseEnduranceActivation / 100.0) * (this.TrackPerimeter * this.LapsToFinish)) ? ((this.HorseEndurance * this.HorseSpeed) / 100.0) : 0;
        if (this.CurrentLap == LapsToFinish)
        {
            this.hasArrived = true;
            this.ArrivalTime = DateTime.Now;
            return;
        }

        double checkingTour = Math.Sqrt(Math.Pow(this.Horse_X_Position - this.Horse_Initial_X_Position, 2) + Math.Pow(this.Horse_Y_Position - this.Horse_Initial_Y_Position, 2));
        if (this.isPassing > 50 && checkingTour <= 10)
        {
            this.CurrentLap++;
            this.isPassing = 0;
        }

        double a = this.TrackWidth / 2;
        double b = this.TrackHeight / 2;
        double x = Racetrack.MAIN_ELLIPSE_CENTER.X - a * Math.Cos(this.Angle);
        double y = Racetrack.MAIN_ELLIPSE_CENTER.Y + b * Math.Sin(this.Angle);

        this.Horse_X_Position = (int)x;
        this.Horse_Y_Position = (int)y;
        this.Angle += (HorseSpeed + boost) / 100.0;

        this.DistanceTraveled += HorseSpeed + boost;
        this.isPassing++;
    }

    public void Draw(Graphics g)
    {
        g.FillEllipse(this.Color == "blue" ? Brushes.Blue : Brushes.Red, Horse_X_Position - (HORSE_SIZE / 2), Horse_Y_Position - (HORSE_SIZE / 2), HORSE_SIZE, HORSE_SIZE);
        g.DrawString(this.HorseId.ToString(), new Font("Arial", 10), Brushes.White, Horse_X_Position - (HORSE_SIZE / 2), Horse_Y_Position - (HORSE_SIZE / 2));
    }
}