using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;

namespace SQLChess.PlayerAndEngines
{
    public partial class Player
    {
        private void King(int thisRow, int thisCol, string thisPiece)
        {
            //blank out current position
            this.ChessBoardArray[thisRow, thisCol] = ".";

            //Check up
            var nextRow = thisRow + 1;
            var nextCol = thisCol + 0;
            King_Check(nextRow, nextCol, thisPiece);

            //Check up right
            nextRow = thisRow + 1;
            nextCol = thisCol + 1;
            King_Check(nextRow, nextCol, thisPiece);

            //Check right
            nextRow = thisRow + 0;
            nextCol = thisCol + 1;
            King_Check(nextRow, nextCol, thisPiece);

            //Check down right
            nextRow = thisRow - 1;
            nextCol = thisCol + 1;
            King_Check(nextRow, nextCol, thisPiece);

            //Check down
            nextRow = thisRow - 1;
            nextCol = thisCol - 0;
            King_Check(nextRow, nextCol, thisPiece);

            //Check down left
            nextRow = thisRow - 1;
            nextCol = thisCol - 1;
            King_Check(nextRow, nextCol, thisPiece);

            //Check left
            nextRow = thisRow + 0;
            nextCol = thisCol - 1;
            King_Check(nextRow, nextCol, thisPiece);

            //Check up left
            nextRow = thisRow + 1;
            nextCol = thisCol - 1;
            King_Check(nextRow, nextCol, thisPiece);

            //reset chessboard array
            this.ChessBoardArray[thisRow, thisCol] = thisPiece;

            //Castle: If the path is clear between the king
            //and the Rook then the king can slide over to the rook and
            //the Rook can move to the other side of the King. 
            //IF: the King is NOT in check and does not pass through check.
            //IF: it is the King's first move.
            //IF: it is the Rook involved first move.

            if (this.KingHasMoved) return;

            string maybeMove;

            //Castle King Side White:
            if (this.PlayerIsWhite)
            {
                if (this.OpponentsMove.Substring(0, 4) == "R..K" && !this.KingsRookHasMoved)
                {
                    this.ChessBoardArray[1, 4] = ".";
                    this.ChessBoardArray[1, 3] = "K";
                    maybeMove = this.ChessBoardArray.ToBoardPositions();
                    if (!AmIChecked(maybeMove))
                    {
                        this.ChessBoardArray[1, 3] = "R";
                        this.ChessBoardArray[1, 2] = "K";
                        this.ChessBoardArray[1, 1] = ".";
                        maybeMove = this.ChessBoardArray.ToBoardPositions();
                        if (!AmIChecked(maybeMove)) this.MaybeMoves.Add(maybeMove);
                    }
                }
                this.ChessBoardArray = GetNewBoardArray(this.OpponentsMove); //reset;
            }

            //Castle Queen Side White:
            if (this.PlayerIsWhite)
            {
                if (this.OpponentsMove.Substring(4, 5) == "K...R" && !this.QueensRookHasMoved)
                {
                    this.ChessBoardArray[1, 4] = ".";
                    this.ChessBoardArray[1, 5] = "K";
                    maybeMove = this.ChessBoardArray.ToBoardPositions();
                    if (!AmIChecked(maybeMove))
                    {
                        this.ChessBoardArray[1, 5] = ".";
                        this.ChessBoardArray[1, 6] = "K";
                        maybeMove = this.ChessBoardArray.ToBoardPositions();
                        if (!AmIChecked(maybeMove))
                        {
                            this.ChessBoardArray[1, 6] = "R";
                            this.ChessBoardArray[1, 7] = "K";
                            this.ChessBoardArray[1, 8] = ".";
                            maybeMove = this.ChessBoardArray.ToBoardPositions();
                            if (!AmIChecked(maybeMove)) this.MaybeMoves.Add(maybeMove);
                        }
                    }
                }
                this.ChessBoardArray = GetNewBoardArray(this.OpponentsMove); //reset;
            }

            //Castle King Side Black:
            if (this.PlayerIsBlack)
            {
                if (this.OpponentsMove.Substring(56, 4) == "r..k" && !KingsRookHasMoved)
                {
                    this.ChessBoardArray[8, 4] = ".";
                    this.ChessBoardArray[8, 3] = "k";
                    maybeMove = this.ChessBoardArray.ToBoardPositions();
                    if (!AmIChecked(maybeMove))
                    {
                        this.ChessBoardArray[8, 3] = "r";
                        this.ChessBoardArray[8, 2] = "k";
                        this.ChessBoardArray[8, 1] = ".";
                        maybeMove = this.ChessBoardArray.ToBoardPositions();
                        if (!AmIChecked(maybeMove)) this.MaybeMoves.Add(maybeMove);
                    }
                }
                this.ChessBoardArray = GetNewBoardArray(this.OpponentsMove); //reset;
            }

            //Castle Queen Side Black:
            if (this.PlayerIsBlack)
            {
                if (this.OpponentsMove.Substring(59, 4) == "k...r" && !QueensRookHasMoved)
                {
                    this.ChessBoardArray[8, 4] = ".";
                    this.ChessBoardArray[8, 5] = "k";
                    maybeMove = this.ChessBoardArray.ToBoardPositions();
                    if (!AmIChecked(maybeMove))
                    {
                        this.ChessBoardArray[8, 5] = ".";
                        this.ChessBoardArray[8, 6] = "k";
                        maybeMove = this.ChessBoardArray.ToBoardPositions();
                        if (!AmIChecked(maybeMove))
                        {
                            this.ChessBoardArray[8, 6] = "r";
                            this.ChessBoardArray[8, 7] = "k";
                            this.ChessBoardArray[8, 8] = ".";
                            maybeMove = this.ChessBoardArray.ToBoardPositions();
                            if (!AmIChecked(maybeMove)) this.MaybeMoves.Add(maybeMove);
                        }
                    }
                }
                this.ChessBoardArray = GetNewBoardArray(this.OpponentsMove); //reset;           }
            }
        }

        private void King_Check(int nextRow, int nextCol, string thisPiece)
        {
            if (nextRow > 0 && nextRow < 9 && nextCol > 0 && nextCol < 9)
            {
                if (!IsMyPiece(this.ChessBoardArray[nextRow, nextCol]))
                {
                    var opponentPiece = this.ChessBoardArray[nextRow, nextCol];
                    this.ChessBoardArray[nextRow, nextCol] = thisPiece;
                    var maybeMove = this.ChessBoardArray.ToBoardPositions();
                    this.MaybeMoves.Add(maybeMove);
                    this.ChessBoardArray[nextRow, nextCol] = opponentPiece;
                }
            }
        }
    }
}
