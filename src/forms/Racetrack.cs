using pmu.forms.details;
using pmu.horses;
using pmu.timer;

namespace pmu.forms;

public class Racetrack : Form
{
    /* FIELDS SECTION */
    public static readonly int FORM_WIDTH = 1360;
    public static readonly int FORM_HEIGHT = 680;
    public static readonly int MAIN_TRACK_HEIGHT = FORM_HEIGHT / 2;
    public static readonly int MAIN_TRACK_WIDTH = (FORM_WIDTH / 2) + 100;
    public static readonly Point MAIN_ELLIPSE_CENTER = new Point(FORM_WIDTH / 2, FORM_HEIGHT / 2);
    public pmu.models.Bettor Bettor1 { get; private set; }
    public pmu.models.Bettor Bettor2 { get; private set; }
    public List<pmu.models.Horse> Bettor1Horses { get; private set; }
    public List<pmu.models.Horse> Bettor2Horses { get; private set; }
    public int Laps { get; private set; }
    public double BetAmount { get; set; }
    public double TimeScale { get; set; }

    public bool IsPaused { get; set; }

    public Track[] Tracks { get; private set; }
    public List<int> ArrivedHorseId { get; private set; }

    /* CONSTRUCTOR SECTION */
    public Racetrack(List<pmu.models.Horse> bettor1Horses, pmu.models.Bettor bettor1, List<pmu.models.Horse> bettor2Horses, pmu.models.Bettor bettor2, int laps, double betAmount, double timeScale)
    {
        this.Bettor1 = bettor1;
        this.Bettor1Horses = bettor1Horses;

        this.Bettor2 = bettor2;
        this.Bettor2Horses = bettor2Horses;

        this.Laps = laps;
        this.BetAmount = betAmount;
        this.TimeScale = timeScale;

        this.Tracks = new Track[this.Bettor1Horses.Count() + this.Bettor2Horses.Count()];

        int index = 0;
        for (int i = 0; i < this.Bettor1Horses.Count(); i++)
        {
            this.Tracks[index] = new Track(index + 1, this.Bettor1Horses[i], "blue", Laps);
            index++;
        }
        for (int i = 0; i < this.Bettor2Horses.Count(); i++)
        {
            this.Tracks[index] = new Track(index + 1, this.Bettor2Horses[i], "red", Laps);
            index++;
        }

        this.ArrivedHorseId = new List<int>();
        this.IsPaused = true;
        this.InitializeComponent();
        new EventTimer(1, () => this.Animate()).Start();
    }

    /* METHODS SECTION */
    private void InitializeComponent()
    {
        this.SuspendLayout();
        // 
        // Racetrack
        // 
        this.DoubleBuffered = true;
        // this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.StartPosition = FormStartPosition.CenterScreen;
        // this.MaximizeBox = false;
        this.ClientSize = new Size(FORM_WIDTH, FORM_HEIGHT);
        this.Name = "Racetrack";
        this.Text = "Racetrack";

        this.ResumeLayout(true);
    }

    private void ShowResults()
    {
        Horse horseBettor1 = this.getFirstArrivedBettor1Horse();
        Horse horseBettor2 = this.getFirstArrivedBettor2Horse();
        TimeSpan time_scale = horseBettor1.ArrivalTime - horseBettor2.ArrivalTime;
        MessageBox.Show(horseBettor1.TrackNumber + " : " + horseBettor1.ArrivalTime);
        MessageBox.Show(horseBettor2.TrackNumber + " : " + horseBettor2.ArrivalTime);
        double time_scale_seconds = time_scale.TotalSeconds;
        MessageBox.Show("Time scale: " + time_scale_seconds);
        MessageBox.Show("Bet amount: " + this.BetAmount + " / " + this.TimeScale + "s");
        if (time_scale_seconds < 0)
        {
            MessageBox.Show("Bettor 1 won " + (this.BetAmount * ((time_scale_seconds < 0 ? time_scale_seconds * -1 : time_scale_seconds) / this.TimeScale)) + "!");
        }
        else if (time_scale_seconds > 0)
        {
            MessageBox.Show("Bettor 2 won " + (this.BetAmount * ((time_scale_seconds < 0 ? time_scale_seconds * -1 : time_scale_seconds) / this.TimeScale)) + "!");
        }
        else
        {
            MessageBox.Show("It's a tie!");
        }
    }

    private void Animate()
    {
        if (!this.IsPaused)
        {
            bool isGameFinished = true;
            foreach (Track track in this.Tracks)
            {
                if (track.Horse.hasArrived)
                {
                    if (!this.ArrivedHorseId.Contains(track.Horse.HorseId))
                    {
                        this.ArrivedHorseId.Add(track.Horse.HorseId);
                    }
                }
                else
                {
                    isGameFinished = false;
                    track.Horse.Move();
                }
            }
            if (isGameFinished)
            {
                this.IsPaused = true;
                this.ShowResults();
                Application.Exit();
            }
        }
        this.Invalidate();
    }

    private void DrawMainRacetrack(Graphics g)
    {
        g.DrawEllipse(Pens.Green, (FORM_WIDTH / 2) - (MAIN_TRACK_WIDTH / 2), (FORM_HEIGHT / 2) - (MAIN_TRACK_HEIGHT / 2), MAIN_TRACK_WIDTH, MAIN_TRACK_HEIGHT);
        g.FillEllipse(Brushes.Black, MAIN_ELLIPSE_CENTER.X - 5, MAIN_ELLIPSE_CENTER.Y - 5, 10, 10);
    }

    private void DrawTracks(Graphics g)
    {
        foreach (Track track in this.Tracks)
        {
            track.Draw(g);
            track.Horse.Draw(g);
        }
    }

    private void DrawFinishLine(Graphics g)
    {
        int x1 = MAIN_ELLIPSE_CENTER.X;
        int y1 = MAIN_ELLIPSE_CENTER.Y + (MAIN_TRACK_HEIGHT / 2);
        int x2 = MAIN_ELLIPSE_CENTER.X;
        int y2 = y1 + Track.TRACK_WIDTH * this.Tracks.Length;
        g.DrawLine(Pens.DarkOrange, x1, y1, x2, y2);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        Graphics g = e.Graphics;
        this.DrawMainRacetrack(g);
        this.DrawTracks(g);
        this.DrawFinishLine(g);
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        if (e.KeyCode == Keys.Space)
        {
            this.IsPaused = !this.IsPaused;
        }
    }

    private Horse getHorseById(int horseId)
    {
        foreach (Track track in this.Tracks)
        {
            if (track.Horse.HorseId == horseId)
            {
                return track.Horse;
            }
        }
        return null;
    }

    private Horse getFirstArrivedBettor1Horse()
    {
        foreach (int ID in this.ArrivedHorseId)
        {
            foreach (pmu.models.Horse horse in this.Bettor1Horses)
            {
                if (horse.Id == ID)
                {
                    return this.getHorseById(ID);
                }
            }
        }
        return null;
    }

    private Horse getFirstArrivedBettor2Horse()
    {
        foreach (int ID in this.ArrivedHorseId)
        {
            foreach (pmu.models.Horse horse in this.Bettor2Horses)
            {
                if (horse.Id == ID)
                {
                    return this.getHorseById(ID);
                }
            }
        }
        return null;
    }
}