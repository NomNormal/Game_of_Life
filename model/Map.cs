using Game_of_Life.enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game_of_Life.model
{

    class Map
    {
        public List<List<Cell>> grid;
        public int countAliveCells = 0;

        public Map(List<List<Cell>> grid)
        {
            this.grid = grid;
        }

        public async Task MapNumberRefresh(int numberGeneration)
        {
            // Reload the generation one or multiple time
            for (int i = 0; i < numberGeneration; i++)
            {
                await OneGeneration();
            }
        }

        public async Task OneGeneration()
        {
            // Refresh all the map only one time


            // Copy the original grid
            countAliveCells = 0;
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
                        if (gridCopy[yy][xx].status == EStatus.ALIVE)
                        {
                            countAliveCells++;
                        }
                    });
                    cellCheck.Add(taskCellCheck);
                }
            }
            await Task.WhenAll(cellCheck.ToArray());

            grid.Clear();
            grid.AddRange(gridCopy);

        }

        private int GetAliveNeighboursCount(int y, int x)
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
