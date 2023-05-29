using pmu.models;

namespace pmu.forms;

public class Game : Form
{
    /* FIELDS SECTION */
    private ComboBox bettorsOption1;
    private ComboBox bettorsOption2;
    private CheckedListBox horsesOption1;
    private CheckedListBox horsesOption2;
    private NumericUpDown lapsOption;
    private NumericUpDown betAmountOption;
    private NumericUpDown timeScaleOption;
    private List<Bettor> bettors;
    private List<Horse> horses;

    /* CONSTRUCTOR SECTION */
    public Game()
    {
        this.InitializeFields();
        this.InitializeComponents();
    }

    /* METHODS SECTION */
    private void InitializeFields()
    {
        this.bettorsOption1 = new ComboBox();
        this.bettorsOption2 = new ComboBox();
        this.horsesOption1 = new CheckedListBox();
        this.horsesOption2 = new CheckedListBox();
        this.lapsOption = new NumericUpDown();
        this.betAmountOption = new NumericUpDown();
        this.timeScaleOption = new NumericUpDown();
        this.bettors = Bettor.getBettors();
        this.horses = Horse.getHorses();
    }
    private void InitializeComponents()
    {
        this.SuspendLayout();
        //
        // Game
        //
        this.StartPosition = FormStartPosition.CenterScreen;
        this.ClientSize = new Size(500, 350);
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.Name = "Game";
        this.Text = "Game";

        //
        //  components
        //
        this.bettorsOption1.Name = "bettor 1";
        this.bettorsOption1.DataSource = new List<Bettor>(this.bettors);
        this.bettorsOption1.DisplayMember = "Name";
        this.bettorsOption1.ValueMember = "Id";
        this.bettorsOption1.Size = new Size(200, 20);
        this.bettorsOption1.DropDownStyle = ComboBoxStyle.DropDownList;
        this.bettorsOption1.FormattingEnabled = true;
        this.bettorsOption1.Location = new Point(20, 20);
        this.bettorsOption1.TabIndex = 0;

        this.bettorsOption2.Name = "bettor 2";
        this.bettorsOption2.DataSource = new List<Bettor>(this.bettors);
        this.bettorsOption2.DisplayMember = "Name";
        this.bettorsOption2.ValueMember = "Id";
        this.bettorsOption2.Size = new Size(200, 20);
        this.bettorsOption2.DropDownStyle = ComboBoxStyle.DropDownList;
        this.bettorsOption2.FormattingEnabled = true;
        this.bettorsOption2.Location = new Point(260, 20);
        this.bettorsOption2.TabIndex = 1;

        this.horsesOption1.Name = "horses 1";
        this.horsesOption1.DataSource = new List<Horse>(this.horses);
        this.horsesOption1.DisplayMember = "Name";
        this.horsesOption1.ValueMember = "Id";
        this.horsesOption1.Size = new Size(200, 100);
        this.horsesOption1.Location = new Point(20, 60);
        this.horsesOption1.TabIndex = 2;

        this.horsesOption2.Name = "horses 2";
        this.horsesOption2.DataSource = new List<Horse>(this.horses);
        this.horsesOption2.DisplayMember = "Name";
        this.horsesOption2.ValueMember = "Id";
        this.horsesOption2.Size = new Size(200, 100);
        this.horsesOption2.Location = new Point(260, 60);
        this.horsesOption2.TabIndex = 3;

        Label laps = new Label();
        laps.Name = "laps";
        laps.Text = "Laps";
        laps.Size = new Size(200, 20);
        laps.Location = new Point(150, 200);
        this.lapsOption.Name = "laps";
        this.lapsOption.Size = new Size(200, 20);
        this.lapsOption.Location = new Point(150, 220);
        this.lapsOption.Minimum = 1;
        this.lapsOption.TabIndex = 4;

        Label betAmountLabel = new Label();
        betAmountLabel.Name = "bet amount label";
        betAmountLabel.Text = "Bet Amount";
        betAmountLabel.Size = new Size(200, 20);
        betAmountLabel.Location = new Point(20, 250);
        this.betAmountOption.Name = "bet amount";
        this.betAmountOption.Size = new Size(200, 20);
        this.betAmountOption.Location = new Point(20, 270);
        this.betAmountOption.Minimum = 1;
        this.betAmountOption.TabIndex = 5;

        Label timeScaleLabel = new Label();
        timeScaleLabel.Name = "time scale label";
        timeScaleLabel.Text = "Time Scale (ms)";
        timeScaleLabel.Size = new Size(200, 20);
        timeScaleLabel.Location = new Point(260, 250);
        this.timeScaleOption.Name = "time scale";
        this.timeScaleOption.Size = new Size(200, 20);
        this.timeScaleOption.Location = new Point(260, 270);
        this.timeScaleOption.Minimum = 1;
        this.timeScaleOption.TabIndex = 6;

        Button startButton = new Button();
        startButton.Name = "start";
        startButton.Text = "Start";
        startButton.Size = new Size(200, 20);
        startButton.Location = new Point(150, 310);
        startButton.Click += new EventHandler(this.startGame);
        startButton.TabIndex = 7;

        this.Controls.Add(this.bettorsOption1);
        this.Controls.Add(this.bettorsOption2);
        this.Controls.Add(this.horsesOption1);
        this.Controls.Add(this.horsesOption2);
        this.Controls.Add(laps);
        this.Controls.Add(this.lapsOption);
        this.Controls.Add(this.betAmountOption);
        this.Controls.Add(betAmountLabel);
        this.Controls.Add(this.timeScaleOption);
        this.Controls.Add(timeScaleLabel);
        this.Controls.Add(startButton);
        this.ResumeLayout(true);
    }

    private void startGame(object sender, EventArgs e)
    {
        Bettor bettor1 = this.getBettorByID((int)this.bettorsOption1.SelectedValue);
        Bettor bettor2 = this.getBettorByID((int)this.bettorsOption2.SelectedValue);
        if (bettor1.Id == bettor2.Id)
        {
            MessageBox.Show("Bettors must be different", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        List<Horse> horses1 = new List<Horse>();
        foreach (Horse horse in this.horsesOption1.CheckedItems)
        {
            horses1.Add(horse);
        }
        if (horses1.Count == 0)
        {
            MessageBox.Show("Bettor 1 must select at least one horse", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        List<Horse> horses2 = new List<Horse>();
        foreach (Horse horse in this.horsesOption2.CheckedItems)
        {
            horses2.Add(horse);
        }
        if (horses2.Count == 0)
        {
            MessageBox.Show("Bettor 2 must select at least one horse", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (horses1.Count != horses2.Count)
        {
            MessageBox.Show("Bettors must select the same number of horses", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        int laps = (int)this.lapsOption.Value;

        double betAmount = (double)this.betAmountOption.Value;
        double timeScale = (double)this.timeScaleOption.Value;

        if (betAmount <= 0)
        {
            MessageBox.Show("Bet amount must be greater than 0", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (timeScale <= 0)
        {
            MessageBox.Show("Time scale must be greater than 0", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        Racetrack race = new Racetrack(horses1, bettor1, horses2, bettor2, laps, betAmount, timeScale);
        race.Show();
    }

    private Bettor getBettorByID(int id)
    {
        return this.bettors.Find(bettor => bettor.Id == id);
    }

    private Horse getHorseByID(int id)
    {
        return this.horses.Find(horse => horse.Id == id);
    }
}