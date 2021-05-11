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
            gridTextBox.Text = "5 10\n" + "0010000000\n" + "0001000000\n" + "0111000000\n" + "0000000000\n" + "0000000000";
            
        }

        private void EventSpeed(Object myObject, EventArgs myEventArgs)
        {
            map.OneGeneration();
            grid.Items.Refresh();
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

        private void ResizeMap ()
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
                MapInit();
                templateTextBox.Text = "";
                gridTextBox.Visibility = Visibility.Hidden;
                
            }
        }

        private void CheckGrid(int number)
        {
            // Check if the file exists before running anything

            Regex regexCheckFile = new Regex(@"(.*)(.txt)\n");
            Match matchCheckFile = regexCheckFile.Match(pathTextBox.Text);
            if (File.Exists(pathTextBox.Text) == true && matchCheckFile != null)
            {
                switch (number)
                {
                    case 1:
                        map.MapNumberRefresh(1);
                        grid.Items.Refresh();
                        break;
                    case 2:
                        timer.Interval = TimeSpan.FromMilliseconds(1000);
                        timer.Start();
                        break;
                    case 3:
                        timer.Interval = TimeSpan.FromMilliseconds(100);
                        timer.Start();
                        break;
                    case 4:
                        timer.Stop();
                        break;
                    case 5:
                        int generation = Int32.Parse(generationTextBox.Text);
                        map.MapNumberRefresh(generation);
                        grid.Items.Refresh();
                        break;
                    case 6:
                        MapInit();
                        break;
                    case 7:
                        ResizeMap();
                        grid.Items.Refresh();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                MessageBox.Show("Not a correct path or file.");
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            // Reload one generation
            CheckGrid(1);
        }

        private void SlowButton_Click(object sender, RoutedEventArgs e)
        {
            // Reload one generation every 1 second
            CheckGrid(2);
        }

        private void FastButton_Click(object sender, RoutedEventArgs e)
        {
            // Reload one generation every 0.1 second
            CheckGrid(3);
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            // Stop the Thread
            CheckGrid(4);
        }

        private void RunGenerationButton_Click(object sender, RoutedEventArgs e)
        {
            // Reload multiple generation
            CheckGrid(5);
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            // Reset the grid, based on the file
            CheckGrid(6);
        }

        private void ResizeButton_Click(object sender, RoutedEventArgs e)
        {
            // Resize the inputed file
            CheckGrid(7);
        }
    }
}
