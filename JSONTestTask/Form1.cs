using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JSONTestTask
{
    public partial class Form1 : Form
    {
        int maxRequestsCount;
        int currentRequestsCount;
        int threadsCount;

        int hours;
        int minutes;
        int seconds;
        string ampm;

        public Form1()
        {
            InitializeComponent();
            maxRequestsCount = 2;
            threadsCount = 10;
            Thread[] threads = new Thread[threadsCount];
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(CheckTime);
                threads[i].Start();
            }
        }

        private bool CheckActiveThreads()
        {
            while (currentRequestsCount >= maxRequestsCount) { }
            return true;
        }

        private async void CheckTime()
        {
            await Task.Run(() => CheckActiveThreads());
            if (currentRequestsCount < maxRequestsCount)
            {
                currentRequestsCount++;
                LondonTimeGetter londonTimeGetter = new LondonTimeGetter();
                TimeInfo timeInfo = await Task.Run(() => londonTimeGetter.GetTime());
                int hours = int.Parse($"{timeInfo.datetime[11]}{timeInfo.datetime[12]}");
                int minutes = int.Parse($"{timeInfo.datetime[14]}{timeInfo.datetime[15]}");
                int seconds = int.Parse($"{timeInfo.datetime[17]}{timeInfo.datetime[18]}");
                string ampm = "AM";
                if (hours > 12)
                {
                    hours -= 12;
                    ampm = "PM";
                }

                if (hours != this.hours || minutes != this.minutes || seconds != this.seconds || ampm != this.ampm)
                {
                    ChangeTime(hours, minutes, seconds, ampm);
                }
            }
            CheckTime();
        }

        private void ChangeTime(int hours, int minutes, int seconds, string ampm)
        {
            this.hours = hours;
            this.minutes = minutes;
            this.seconds = seconds;
            this.ampm = ampm;
        }

        private void ChangeLabel() => label1.Text = $"{$"{hours:D2}"}:{$"{minutes:D2}"}:{$"{seconds:D2}"} {ampm}";

        private void timer1_Tick(object sender, EventArgs e)
        {
            label2.Text = $"Запросов в секунду: {currentRequestsCount}";
            label3.Text = $"Количество активных потоков: {threadsCount}";
            if (label1.Text != $"{$"{hours:D2}"}:{$"{minutes:D2}"}:{$"{seconds:D2}"} {ampm}")
                ChangeLabel();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            currentRequestsCount = 0;
        }
    }
}
