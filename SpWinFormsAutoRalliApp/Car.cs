using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpWinFormsAutoRalliApp
{
    public class Car
    {
        static int idInit = 0;

        public readonly int Id;
        public string Image { get; } = null!;
        public PictureBox CarBox { get; }
        public BackgroundWorker BackgroundWorker { get; }

        ListBox lstResults { get; } = null!;

        Form parent;

        public Car(Form parent, ListBox lstResults, string image)
        {
            this.parent = parent;
            this.lstResults = lstResults;

            Id = ++idInit;
            Image = image;

            CarBox = new PictureBox();
            CarBox.Image = new Bitmap(image);

            CarBox.Location = new Point(10, (Id - 1) * 90 + 30);
            CarBox.Size = new Size(120, 65);
            CarBox.SizeMode = PictureBoxSizeMode.Zoom;
            

            BackgroundWorker = new BackgroundWorker();
            BackgroundWorker.WorkerReportsProgress = true;
            BackgroundWorker.WorkerSupportsCancellation = true;

            BackgroundWorker.DoWork += BackgroundWorker_DoWork;
            BackgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
            BackgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
        }

        private void BackgroundWorker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            lstResults.Items.Add(Id);
        }

        private void BackgroundWorker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            CarBox.Location = new Point((int)e.ProgressPercentage, CarBox.Location.Y);
            //CarBox.Refresh();
        }

        private void BackgroundWorker_DoWork(object? sender, DoWorkEventArgs e)
        {
            Random random = new Random();

            int x = CarBox.Location.X;

            while(x < parent.Width - CarBox.Width * 2.5)
            {
                if(BackgroundWorker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }

                Thread.Sleep(20 + random.Next(20));
                
                BackgroundWorker.ReportProgress(x);

                x += 10 + random.Next(10);
            }
        }
        public async Task MoveAsync()
        {
            Task task = new(Move);
            task.Start();
            await task.WaitAsync(new CancellationToken());
        }
        public void Move()
        {
            Random random = new Random();

            int x = CarBox.Location.X;
            int y = CarBox.Location.Y;

            while (x < parent.Width - CarBox.Width * 2.5)
            {
                Thread.Sleep(20 + random.Next(20));
                x += 10 + random.Next(10);
                CarBox.Location = new Point(x, y);
            }
            lstResults.Items.Add(Id);
        }
    }
}
