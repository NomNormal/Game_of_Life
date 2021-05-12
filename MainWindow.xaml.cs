using Game_of_Life.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Win32;


namespace Game_of_Life
{
    /// <summary>
    /// Hello :)
    /// </summary>
    public partial class MainWindow : Window
    {
        private Map map;
        static DispatcherTimer timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            timer.Tick += new EventHandler(EventSpeed);
            gridTextBox.Text = "5 10\n" + "0010000000\n" + "0001000000\n" + "0111000000\n" + "0000000000\n" + "0000000000";
        }

        private async void EventSpeed(Object myObject, EventArgs myEventArgs)
        {
            await map.OneGeneration();
            grid.Items.Refresh();
            countAliveTextBox.Text = map.countAliveCells.ToString();
        }

        private void MapInit()
        {
            List<string> inputGrid = new List<string>(File.ReadAllLines(pathTextBox.Text));
            string header = inputGrid.First();
            inputGrid.RemoveAt(0);

            Regex regexSize = new Regex(@"([0-9]*) ([0-9]*)");
            Match matches = regexSize.Match(header);
            string numberRowStr = matches.Groups[1].Value;
            string numberColumnStr = matches.Groups[2].Value;
            rowsTextBox.Text = numberRowStr;
            columnsTextBox.Text = numberColumnStr;
            int numberRow = Int32.Parse(numberRowStr);
            int numberColumn = Int32.Parse(numberColumnStr);

            this.map = new Map(new List<List<Cell>>());

            int countAlive = 0;

            for (int y = 0; y < numberRow; y++)
            {
                this.map.grid.Insert(y, new List<Cell>());
                for (int x = 0; x < numberColumn; x++)
                {
                    bool isAlive = inputGrid[y][x] == '1';
                    if (isAlive)
                    {
                        countAlive++;
                    }

                    Cell cell = new Cell(isAlive);
                    this.map.grid[y].Insert(x, cell);
                }
            }
            countAliveTextBox.Text = countAlive.ToString();
            grid.ItemsSource = this.map.grid;
            grid.Items.Refresh();
        }

        private void ResizeMap()
        {
            int resizeRow = Int32.Parse(rowsTextBox.Text);
            int resizeColumn = Int32.Parse(columnsTextBox.Text);
            int originalX = map.grid[0].Count();
            int originalY = map.grid.Count();

            for (int y = 0; y < resizeRow; y++)
            {
                if (originalY - 1 < y)
                {
                    this.map.grid.Insert(y, new List<Cell>());
                }

                for (int x = 0; x < resizeColumn; x++)
                {
                    if (originalX - 1 < x || originalY - 1 < y)
                    {
                        Cell cell = new Cell(false);
                        this.map.grid[y].Insert(x, cell);
                    }
                }
            }
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
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
                MapInit();
                templateTextBox.Text = "";
                gridTextBox.Visibility = Visibility.Hidden;

            }
        }

        private bool FilePathExists()
        {
            // Check if the file exists before running anything
            Regex regexCheckFile = new Regex(@"(.*)(.txt)\n");
            Match matchCheckFile = regexCheckFile.Match(pathTextBox.Text);
            return File.Exists(pathTextBox.Text) == true && matchCheckFile != null;
        }

        private void DisplayNonExistingFilePathError()
        {
            MessageBox.Show("Not a correct path or file.");
        }

        private async void NextButton_Click(object sender, RoutedEventArgs e)
        {
            // Reload one generation
            if (FilePathExists())
            {
                await map.MapNumberRefresh(1);
                grid.Items.Refresh();
                countAliveTextBox.Text = map.countAliveCells.ToString();
            }
            else
            {
                DisplayNonExistingFilePathError();
            }
        }

        private void SlowButton_Click(object sender, RoutedEventArgs e)
        {
            // Reload one generation every 1 second
            if (FilePathExists())
            {
                timer.Interval = TimeSpan.FromMilliseconds(1000);
                timer.Start();
            }
            else
            {
                DisplayNonExistingFilePathError();
            }
        }

        private void FastButton_Click(object sender, RoutedEventArgs e)
        {
            // Reload one generation every 0.1 second
            if (FilePathExists())
            {
                timer.Interval = TimeSpan.FromMilliseconds(100);
                timer.Start();
            }
            else
            {
                DisplayNonExistingFilePathError();
            }
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            // Pause the execution
            if (FilePathExists())
            {
                timer.Stop();
            }
            else
            {
                DisplayNonExistingFilePathError();
            }
        }

        private async void RunGenerationButton_Click(object sender, RoutedEventArgs e)
        {
            // Reload multiple generation
            if (FilePathExists())
            {
                int generation = Int32.Parse(generationTextBox.Text);
                await map.MapNumberRefresh(generation);
                grid.Items.Refresh();
                countAliveTextBox.Text = map.countAliveCells.ToString();
            }
            else
            {
                DisplayNonExistingFilePathError();
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            // Reset the grid, based on the file
            if (FilePathExists())
            {
                MapInit();
            }
            else
            {
                DisplayNonExistingFilePathError();
            }
        }

        private void ResizeButton_Click(object sender, RoutedEventArgs e)
        {
            // Resize the inputed file
            if (FilePathExists())
            {
                ResizeMap();
                grid.Items.Refresh();
                countAliveTextBox.Text = map.countAliveCells.ToString();
            }
            else
            {
                DisplayNonExistingFilePathError();
            }
        }
    }
}
