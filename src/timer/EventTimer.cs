namespace pmu.timer;

public class EventTimer : System.Windows.Forms.Timer
{

    private Action onTick;

    public EventTimer(int interval, Action onTick) : base()
    {
        this.Interval = interval;
        this.onTick = onTick;
        this.Tick += new EventHandler(OnTick);
    }
    private void OnTick(object sender, EventArgs e)
    {
        if (onTick != null)
        {
            onTick();
        }
    }


}