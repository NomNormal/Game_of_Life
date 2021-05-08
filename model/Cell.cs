using Game_of_Life.enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Game_of_Life.model
{
    class Cell
    {
        public EStatus status { get; set; }

        public Cell(bool isAlive)
        {
            if (isAlive)
            {
                status = EStatus.ALIVE;
            }
            else
            {
                status = EStatus.DEAD;
            }

        }

        public void CellStatusReload(int nbrNeighbours)
        {
            if (nbrNeighbours == 3)
            {
                status = EStatus.ALIVE;
            }
            if (nbrNeighbours < 2 || nbrNeighbours > 3)
            {
                status = EStatus.DEAD;
            }
        }
    }
}
