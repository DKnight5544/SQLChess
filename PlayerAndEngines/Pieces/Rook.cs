using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;

namespace SQLChess.PlayerAndEngines
{
    public partial class Player
    {


        private void Rook(int thisRow, int thisCol, string thisPiece)
        {

            //blank out current position
            this.ChessBoardArray[thisRow, thisCol] = ".";
            
            MultiCheck(thisRow, thisCol, 1, 0, thisPiece);// up            
            MultiCheck(thisRow, thisCol, -1, 0, thisPiece);// down            
            MultiCheck(thisRow, thisCol, 0, -1, thisPiece);// left            
            MultiCheck(thisRow, thisCol, 0, 1, thisPiece);// right

            //reset chessboard array
            this.ChessBoardArray[thisRow, thisCol] = thisPiece;

        }


    }
}
