using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ExponentialRandomSequenceWinForms
{
    public partial class MainForm : Form
    {
        private readonly List<double> exponentialSequence = new List<double>();
        private readonly int M = 100;
        private readonly double lambda = 0.7;
        private readonly int numberOfBins = 10;

        public MainForm()
        {
            InitializeComponent();
            GenerateExponentialSequence();
            BuildHistogram();
        }

        private void GenerateExponentialSequence()
        {
            Random random = new Random();
            for (int i = 0; i < M; i++)
            {
                double u = random.NextDouble();
                double x = -Math.Log(1 - u) / lambda;
                exponentialSequence.Add(x);
            }
        }

        private void BuildHistogram()
        {
            double min = exponentialSequence.Min();
            double max = exponentialSequence.Max();
            double binSize = (max - min) / numberOfBins;

            Dictionary<(double, double), int> histogram = new Dictionary<(double, double), int>();

            for (int i = 0; i < numberOfBins; i++)
            {
                double binStart = min + i * binSize;
                double binEnd = binStart + binSize;

                int count = exponentialSequence.Count(x => x >= binStart && x < binEnd);
                histogram[(binStart, binEnd)] = count;
            }

            // Далі можна вивести гістограму у вікні Windows Forms, наприклад, у TextBox або DataGridView
            DisplayHistogram(histogram);
        }

        private void DisplayHistogram(Dictionary<(double, double), int> histogram)
        {
            chart1.Series.Clear();
            chart1.ChartAreas.Clear();

            ChartArea chartArea = new ChartArea("Histogram");
            chart1.ChartAreas.Add(chartArea);

            Series series = new Series("Data");
            series.ChartType = SeriesChartType.Column;

            foreach (var bin in histogram)
            {
                double binStart = bin.Key.Item1;
                double binEnd = bin.Key.Item2;
                int count = bin.Value;

                // Додаємо дані до серії для кожного біна
                DataPoint dataPoint = new DataPoint();
                dataPoint.SetValueXY((binStart + binEnd) / 2, count); // Встановлюємо середину біна і кількість

                series.Points.Add(dataPoint);
            }

            chart1.Series.Add(series);
        }
    }
}
