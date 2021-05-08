using Game_of_Life.model;
using Game_of_Life.enums;
using System;
using System.Collections.Generic;
using System.IO; // For FileStream
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using Microsoft.Win32;

/*
5 10
0010000000
0001000000
0111000000
0000000000
0000000000
 */

namespace Game_of_Life
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Map map;
        static DispatcherTimer timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            timer.Tick += new EventHandler(EventSpeed);
        }

        private void EventSpeed(Object myObject, EventArgs myEventArgs)
        {
            map.OneGeneration();
            grid.Items.Refresh();
        }

        private void ResizeButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void browseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*",
                FilterIndex = 2,
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                pathTextBox.Text = openFileDialog.FileName;
                List<string> inputGrid = new List<string>(File.ReadAllLines(pathTextBox.Text));
                string header = inputGrid.First();
                inputGrid.RemoveAt(0);

                Regex regEx = new Regex(@"([0-9]*) ([0-9]*)");
                Match matches = regEx.Match(header);
                string numberRowStr = matches.Groups[1].Value;
                string numberColumnStr = matches.Groups[2].Value;
                int numberRow = Int32.Parse(numberRowStr);
                int numberColumn = Int32.Parse(numberColumnStr);

                this.map = new Map(new List<List<Cell>>());

                for (int y = 0; y < numberRow; y++)
                {
                    this.map.grid.Insert(y, new List<Cell>());
                    for (int x = 0; x < numberColumn; x++)
                    {
                        bool isAlive = inputGrid[y][x] == '1';
                        Cell cell = new Cell(isAlive);
                        this.map.grid[y].Insert(x, cell);
                    }
                }
                
                grid.ItemsSource = this.map.grid;
                grid.Items.Refresh();
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            // Reload one generation
            map.MapNumberRefresh(1);
            grid.Items.Refresh();
        }

        private void SlowButton_Click(object sender, RoutedEventArgs e)
        {
            // Reload one generation every 1 second
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Start();
        }

        private void FastButton_Click(object sender, RoutedEventArgs e)
        {
            // Reload one generation every 0.1 second
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Start();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            // Stop the Thread
            timer.Stop();
        }

        private void RunGenerationButton_Click(object sender, RoutedEventArgs e)
        {
            // Reload multiple generation
            int generation = Int32.Parse(generationTextBox.Text);
            map.MapNumberRefresh(generation);
            grid.Items.Refresh();
        }


    }
}
