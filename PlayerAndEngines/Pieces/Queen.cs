using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;

namespace SQLChess.PlayerAndEngines
{
    public partial class Player
    {

        private void Queen(int thisRow, int thisCol, string thisPiece)
        {
            Rook(thisRow, thisCol, thisPiece);
            Bishop(thisRow, thisCol, thisPiece);
        }
    }
}
