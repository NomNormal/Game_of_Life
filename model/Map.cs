using Game_of_Life.enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Game_of_Life.model
{

    class Map
    {
        public List<List<Cell>> grid;

        public Map(List<List<Cell>> grid)
        {
            this.grid = grid;

            //Thread thread = new Thread(new ThreadStart(MapNumberRefresh));
        }

        public void MapNumberRefresh(int numberGeneration)
        {
            // Reload the generation one or multiple time
            for (int i = 0; i < numberGeneration; i++)
            {
                Task taskGeneration = Task.Run(() =>
                {
                    OneGeneration();
                }
                );
                Task.WaitAll(taskGeneration);
            }

        }

        public void OneGeneration()
        {
            // Refresh all the map only one time


            // Copy the original grid
            List<List<Cell>> gridCopy = new List<List<Cell>>();
            for (int y = 0; y < grid.Count(); y++)
            {
                gridCopy.Insert(y, new List<Cell>());
                for (int x = 0; x < grid[y].Count(); x++)
                {
                    Cell cell;
                    if (grid[y][x].status == EStatus.ALIVE)
                    {
                        cell = new Cell(true);
                    }
                    else
                    {
                        cell = new Cell(false);
                    }

                    gridCopy[y].Insert(x, cell);
                }
            }

            List<Task> cellCheck = new();

            
            // Reload cell status depending on the number of neightbours
            for (int y = 0; y < grid.Count(); y++)
            {
                for (int x = 0; x < grid[y].Count(); x++)
                {
                    int yy = y;
                    int xx = x;
                    Task taskCellCheck = Task.Run(() =>
                    {
                        int nbrNeighbours = GetAliveNeighboursCount(yy, xx);
                        gridCopy[yy][xx].CellStatusReload(nbrNeighbours);
                    });
                    cellCheck.Add(taskCellCheck);
                }
            }
            
            Task.WaitAll(cellCheck.ToArray());

            grid.Clear();
            grid.AddRange(gridCopy);

        }

        public int GetAliveNeighboursCount(int y, int x)
        {
            // Count the neighbours on a cell           

            int countNeighbours = 0;

            for (int targetedY = y - 1; targetedY <= y + 1; targetedY++)
            {
                for (int targetedX = x - 1; targetedX <= x + 1; targetedX++)
                {
                    if (targetedY == y && targetedX == x) continue;

                    if (targetedX >= 0 && targetedX < grid[y].Count() &&
                        targetedY >= 0 && targetedY < grid.Count() &&
                        grid[targetedY][targetedX].status == EStatus.ALIVE)
                    {
                        countNeighbours++;
                    }
                }
            }
            return countNeighbours;
        }

    }
}
