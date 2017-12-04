using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;

namespace SQLChess.PlayerAndEngines
{
    public partial class Player
    {

        private void Bishop(int thisRow, int thisCol, string thisPiece)
        {

            //blank out current position
            this.ChessBoardArray[thisRow, thisCol] = ".";

            MultiCheck(thisRow, thisCol, 1, -1, thisPiece);// up left          
            MultiCheck(thisRow, thisCol, 1, 1, thisPiece);// up right          
            MultiCheck(thisRow, thisCol, -1, 1, thisPiece);// down right           
            MultiCheck(thisRow, thisCol, -1, -1, thisPiece);// down left 

            //reset chessboard array
            this.ChessBoardArray[thisRow, thisCol] = thisPiece;

        }
        

    }
}
