using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;

namespace SQLChess.PlayerAndEngines
{
    public partial class Player
    {

        private void Knight(int thisRow, int thisCol, string thisPiece)
        {

            //blank out current position
            this.ChessBoardArray[thisRow, thisCol] = ".";

            TryThis(thisRow + 2, thisCol - 1, thisPiece, CaptureCode.CanCapture);
            TryThis(thisRow + 2, thisCol + 1, thisPiece, CaptureCode.CanCapture);
            TryThis(thisRow + 1, thisCol + 2, thisPiece, CaptureCode.CanCapture);
            TryThis(thisRow + 1, thisCol - 2, thisPiece, CaptureCode.CanCapture);
            TryThis(thisRow - 1, thisCol - 2, thisPiece, CaptureCode.CanCapture);
            TryThis(thisRow - 1, thisCol + 2, thisPiece, CaptureCode.CanCapture);
            TryThis(thisRow - 2, thisCol + 1, thisPiece, CaptureCode.CanCapture);
            TryThis(thisRow - 2, thisCol - 1, thisPiece, CaptureCode.CanCapture);

            //reset chessboard array
            this.ChessBoardArray[thisRow, thisCol] = thisPiece;

        }
    }
}
