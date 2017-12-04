using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLChess.PlayerAndEngines.CustomClasses
{
    public class BoardMapRow
    {
        public int Pos { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }

        public BoardMapRow(int pos, int row, int col)
        {
            Pos = pos;
            Row = row;
            Col = col;
        }
    }
}
