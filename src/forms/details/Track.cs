using pmu.forms;
using pmu.horses;

namespace pmu.forms.details;

public class Track
{
    /* FIELDS SECTION */
    public static readonly int TRACK_WIDTH = 30;
    public int TrackNumber { get; private set; }
    public Horse Horse { get; private set; }

    /* CONSTRUCTOR SECTION */
    public Track(int trackNumber, pmu.models.Horse horse, string color, int laps)
    {
        this.TrackNumber = trackNumber;

        int horse_initial_x_position = Racetrack.MAIN_ELLIPSE_CENTER.X;
        int horse_initial_y_position = Racetrack.MAIN_ELLIPSE_CENTER.Y + (Racetrack.MAIN_TRACK_HEIGHT / 2) + (this.TrackNumber * TRACK_WIDTH) - (TRACK_WIDTH / 2);
        this.Horse = new Horse(horse.Id, this.TrackNumber, new Point(horse_initial_x_position, horse_initial_y_position), horse.Speed, horse.Endurance, horse.EnduranceActivation, color, laps);
    }

    /* METHODS SECTION */
    public void Draw(Graphics g)
    {
        int width = Racetrack.MAIN_TRACK_WIDTH + (this.TrackNumber * TRACK_WIDTH * 2);
        int height = Racetrack.MAIN_TRACK_HEIGHT + (this.TrackNumber * TRACK_WIDTH * 2);
        int x = (Racetrack.FORM_WIDTH / 2) - (width / 2);
        int y = (Racetrack.FORM_HEIGHT / 2) - (height / 2);
        g.DrawEllipse(Pens.Black, x, y, width, height);
    }
}