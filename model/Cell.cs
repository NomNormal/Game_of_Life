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

        public void CellStatusReload(int nbrNeightbours)
        {
            if (nbrNeightbours == 3)
            {
                status = EStatus.ALIVE;
            }
            if (nbrNeightbours < 2 || nbrNeightbours > 3)
            {
                status = EStatus.DEAD;
            }
        }
    }
}
