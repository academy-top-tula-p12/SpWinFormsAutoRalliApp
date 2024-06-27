namespace SpWinFormsAutoRalliApp
{
    public partial class Form1 : Form
    {
        List<Car> cars = new();
        List<string> carFiles = new()
        {
            "car-red.png",
            "car-blue.png",
            "car-green.png",
            "car-yellow.png",
            "car-magenta.png"
        };

        int start = 120;
        int finish = 500;
        public Form1()
        {
            InitializeComponent();

            foreach (var name in carFiles)
            {
                cars.Add(new Car(this, lstResults, name));
                this.Controls.Add(cars.Last().CarBox);
                Label label = new();
                label.Text = cars.Last().Id.ToString();
                label.Location = new Point(2, cars.Last().CarBox.Location.Y);
                this.Controls.Add(label);
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            lstResults.Items.Clear();
            //Task[] tasks = new Task[carFiles.Count];

            //for(int i = 0; i < cars.Count; i++)
            //{
            //    int y = cars[i].CarBox.Location.Y;
            //    cars[i].CarBox.Location = new Point(10, y);
            //    //cars[i].BackgroundWorker.RunWorkerAsync();
            //    Task task = new Task(cars[i].Move);
            //    tasks[i] = task;
            //}
            //for(int i = 0; i < tasks.Length; i++)
            //    tasks[i].Start();

            //Task.WaitAll(tasks);


            Task[] taskCars = new Task[carFiles.Count];
            for (int i = 0; i < cars.Count; i++)
            {
                int y = cars[i].CarBox.Location.Y;
                cars[i].CarBox.Location = new Point(10, y);
                taskCars[i] = cars[i].MoveAsync();
            }
            await Task.WhenAll(taskCars);

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            this.CreateGraphics().DrawLine(
                new Pen(Brushes.Coral, 20),
                start, 0, start, this.Height);
        }
    }
}
