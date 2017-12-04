using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;

namespace SQLChess.PlayerAndEngines
{
    public partial class Player
    {
        private void Pawn(int thisRow, int thisCol, string thisPiece)
        {

            //blank out current position
            this.ChessBoardArray[thisRow, thisCol] = ".";

            var offset = this.PlayerIsWhite ? 1 : -1;

            string pieces;

            //In case of pawn promotions
            int nextRow = thisRow + offset;

            if (nextRow == 8) pieces = "R,N,B,Q";
            else if (nextRow == 1) pieces = "r,n,b,q";
            else pieces = thisPiece;

            pieces.Split(',')
                .ToList()
                .ForEach(piece =>
                {


                    //One Forward
                    TryThis(nextRow, thisCol, piece, CaptureCode.CantCapture);

                    //try capture left
                    TryThis(nextRow, thisCol-1, piece, CaptureCode.MustCapture);

                    //try capture right
                    TryThis(nextRow, thisCol+ 1, piece, CaptureCode.MustCapture);


                });


            //Two Forward
            bool firstMove = (PlayerIsWhite && thisRow == 2) || (PlayerIsBlack && thisRow == 7);
            if (firstMove)
            {
                bool nextRowIsBlank = this.ChessBoardArray[thisRow + offset, thisCol] == ".";
                if (nextRowIsBlank)
                {
                    offset = this.PlayerIsWhite ? 2 : -2;
                    nextRow = thisRow + offset;
                    TryThis(nextRow, thisCol, thisPiece, CaptureCode.CantCapture);
                }
            }

            //En Passant - if white pawn is on row 5 or Black Pawn on row 4 and there is
            // and enemy Pawn to either side then check to see if the enemy's pawn moved forward 2
            // on the very last move.  If so then we can choose to take that pawn as if it had only
            // moved 1. 
            
            if ((PlayerIsWhite && thisRow == 5) || (PlayerIsBlack && thisRow == 4))
            {
                int testRow = PlayerIsWhite ? 7 : 2;
                int rowOffset = PlayerIsWhite ? 1 : -1;
                string enemyPiece = PlayerIsWhite ? "p" : "P";

                int testCol = thisCol - 1;
                if (thisCol > 1 && ChessBoardArray[thisRow, testCol] == enemyPiece)
                {
                    
                    TryEnpassant(thisRow, testRow, testCol, offset, thisPiece);
                }

                testCol = thisCol + 1;
                if (thisCol < 8 && ChessBoardArray[thisRow, testCol] == enemyPiece)
                {
                    TryEnpassant(thisRow, testRow, testCol, offset, thisPiece);
                }
            }

            //reset chessboard array
            this.ChessBoardArray[thisRow, thisCol] = thisPiece;

        }

        private void TryEnpassant(int thisRow, int testRow, int testCol, int offset, string thisPiece)
        {
            if (this.OpponentsLastMove.Beg.Row == testRow
                && this.OpponentsLastMove.Beg.Col == testCol
                && this.OpponentsLastMove.End.Row == thisRow
                && this.OpponentsLastMove.End.Col == testCol
                )
            {
                var savePiece = this.ChessBoardArray[thisRow, testCol];
                ChessBoardArray[thisRow, testCol] = ".";
                ChessBoardArray[thisRow + offset, testCol] = thisPiece;
                var maybeMove = this.ChessBoardArray.ToBoardPositions();
                this.MaybeMoves.Add(maybeMove);
                this.ChessBoardArray[thisRow, testCol] = savePiece;
                this.ChessBoardArray[thisRow + offset, testCol] = ".";
            }

        }

    }
}
