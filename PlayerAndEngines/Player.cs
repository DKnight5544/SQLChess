
using System.Collections.Generic;
using System.Linq;
using SQLChess.PlayerAndEngines.CustomClasses;

namespace SQLChess.PlayerAndEngines
{
    public partial class Player
    {
        private string[,] ChessBoardArray { get; set; }
        private List<string> MaybeMoves { get; set; }
        private LastMove OpponentsLastMove { get; set; }

        private bool _playerisWhite;
        public bool PlayerIsWhite
        {
            get { return _playerisWhite; }
        }
        public bool PlayerIsBlack
        {
            get { return !_playerisWhite; }
        }

        public string OpponentsMove;
        public string MyLastMove;
        public bool KingHasMoved;
        public bool KingsRookHasMoved;
        public bool QueensRookHasMoved;

        public Player(PlayerColor color)
        {
            _playerisWhite = color == PlayerColor.White;
            this.MaybeMoves = new List<string>();
        }

        public string Move()
        {

            List<string> legalMoves = FindLegalMoves();

            //now choose the best of the legal moves
            if (legalMoves.Count() > 0)
            {
                string xml = legalMoves.ToXML();
                var bestMove = Global.DB.GetBestMove(xml).Single().BestMove;

                if (PlayerIsWhite)
                {
                    if (bestMove.Substring(0, 1) != "R") this.KingsRookHasMoved = true;
                    if (bestMove.Substring(4, 1) != "K") this.KingHasMoved = true;
                    if (bestMove.Substring(7, 1) != "R") this.QueensRookHasMoved = true;
                }
                else
                {
                    if (bestMove.Substring(56, 1) != "r") this.KingsRookHasMoved = true;
                    if (bestMove.Substring(59, 1) != "k") this.KingHasMoved = true;
                    if (bestMove.Substring(63, 1) != "r") this.QueensRookHasMoved = true;

                }


                return bestMove;
            }
            else
            {
                bool inCheck = AmIChecked(this.OpponentsMove);
                if (inCheck) return string.Format("Checkmate: {0} loses.", PlayerIsWhite ? "White" : "Black");
                else return string.Format("Stalemate: {0} out of legal moves.", PlayerIsWhite ? "White" : "Black");
            }

        }
        public List<string> FindLegalMoves()
        {
            //For approving en passant
            if (!string.IsNullOrWhiteSpace(MyLastMove)) OpponentsLastMove = GetLastMove();

            this.ChessBoardArray = GetNewBoardArray(this.OpponentsMove);

            this.MaybeMoves.Clear();

            var chessBoardList =
                Global.BoardMap  //maps board position to row and column
                    .Select(b => new
                    {
                        Piece = this.OpponentsMove.Substring(b.Pos - 1, 1),
                        b.Pos,
                        b.Row,
                        b.Col
                    })
                    .ToList()
                    ;

            chessBoardList.ForEach(i =>
            {

                if (
                        (i.Piece == "P" && this.PlayerIsWhite)
                        || (i.Piece == "p" && this.PlayerIsBlack)
                        )
                {
                    Pawn(i.Row, i.Col, i.Piece);
                }

                else if (
                        (i.Piece == "R" && this.PlayerIsWhite)
                        || (i.Piece == "r" && this.PlayerIsBlack)
                        )
                {
                    Rook(i.Row, i.Col, i.Piece);
                }

                else if (
                        (i.Piece == "K" && this.PlayerIsWhite)
                        || (i.Piece == "k" && this.PlayerIsBlack)
                        )
                {
                    King(i.Row, i.Col, i.Piece);
                }

                else if (
                        (i.Piece == "B" && this.PlayerIsWhite)
                        || (i.Piece == "b" && this.PlayerIsBlack)
                        )
                {
                    Bishop(i.Row, i.Col, i.Piece);
                }

                else if (
                        (i.Piece == "N" && this.PlayerIsWhite)
                        || (i.Piece == "n" && this.PlayerIsBlack)
                        )
                {
                    Knight(i.Row, i.Col, i.Piece);
                }

                else if (
                        (i.Piece == "Q" && this.PlayerIsWhite)
                        || (i.Piece == "q" && this.PlayerIsBlack)
                        )
                {
                    Queen(i.Row, i.Col, i.Piece);
                }

            });

            //toss out any maybe moves that leave you in check
            List<string> legalMoves = null;


            legalMoves =
                this.MaybeMoves
                    .Where(mm => AmIChecked(mm) == false)
                    .ToList()
                    ;

            return legalMoves;
            

        }

        private string[,] GetNewBoardArray(string board)
        {
            string[,] result = new string[9, 9];
            Global.BoardMap
                .ForEach(i =>
                {
                    result[i.Row, i.Col] = board.Substring(i.Pos - 1, 1);
                });

            return result;

        }

        public bool AmIChecked(string board)
        {
            bool inCheck = false;

            //first find my king
            int kingPos = PlayerIsWhite
                        ? board.IndexOf("K") + 1
                        : board.IndexOf("k") + 1
                        ;

            var king = Global.BoardMap.Where(i => i.Pos == kingPos).Single();

            var chessBoardArray = GetNewBoardArray(board);

            //Up
            inCheck = AmIChecked_RookBishopQueen(king.Row, king.Col, +1, 0, "RrQq", chessBoardArray);
            if (inCheck) return true;

            //Down
            inCheck = AmIChecked_RookBishopQueen(king.Row, king.Col, -1, 0, "RrQq", chessBoardArray);
            if (inCheck) return true;

            //Left
            inCheck = AmIChecked_RookBishopQueen(king.Row, king.Col, 0, -1, "RrQq", chessBoardArray);
            if (inCheck) return true;

            //Right
            inCheck = AmIChecked_RookBishopQueen(king.Row, king.Col, 0, 1, "RrQq", chessBoardArray);
            if (inCheck) return true;

            //Up & Left
            inCheck = AmIChecked_RookBishopQueen(king.Row, king.Col, 1, -1, "BbQq", chessBoardArray);
            if (inCheck) return true;

            //Up & Right
            inCheck = AmIChecked_RookBishopQueen(king.Row, king.Col, 1, 1, "BbQq", chessBoardArray);
            if (inCheck) return true;

            //Down & Left
            inCheck = AmIChecked_RookBishopQueen(king.Row, king.Col, -1, -1, "BbQq", chessBoardArray);
            if (inCheck) return true;

            //Down & Right
            inCheck = AmIChecked_RookBishopQueen(king.Row, king.Col, -1, 1, "BbQq", chessBoardArray);
            if (inCheck) return true;

            //Pawn
            if (PlayerIsWhite && king.Row < 7)
            {
                if (king.Col > 1 && chessBoardArray[king.Row + 1, king.Col - 1] == "p") return true;
                if (king.Col < 8 && chessBoardArray[king.Row + 1, king.Col + 1] == "p") return true;
            }

            if (PlayerIsBlack && king.Row > 2)
            {
                if (king.Col > 1 && chessBoardArray[king.Row - 1, king.Col - 1] == "P") return true;
                if (king.Col < 8 && chessBoardArray[king.Row - 1, king.Col + 1] == "P") return true;
            }

            //Knight
            string enemny = PlayerIsWhite ? "n" : "N";

            if (king.Row < 7)
            {
                if (king.Col > 1 && chessBoardArray[king.Row + 2, king.Col - 1] == enemny) return true;
                if (king.Col < 8 && chessBoardArray[king.Row + 2, king.Col + 1] == enemny) return true;
            }
            if (king.Row > 2)
            {
                if (king.Col > 1 && chessBoardArray[king.Row - 2, king.Col - 1] == enemny) return true;
                if (king.Col < 8 && chessBoardArray[king.Row - 2, king.Col + 1] == enemny) return true;
            }
            if (king.Col < 7)
            {
                if (king.Row > 1 && chessBoardArray[king.Row - 1, king.Col + 2] == enemny) return true;
                if (king.Row < 8 && chessBoardArray[king.Row + 1, king.Col + 2] == enemny) return true;
            }
            if (king.Col > 2)
            {
                if (king.Row > 1 && chessBoardArray[king.Row - 1, king.Col - 2] == enemny) return true;
                if (king.Row < 8 && chessBoardArray[king.Row + 1, king.Col - 2] == enemny) return true;
            }

            //King
            string enemy = PlayerIsWhite ? "k" : "K";

            if (AmIChecked_King(king.Row + 1, king.Col - 1, enemy, chessBoardArray)) return true;
            if (AmIChecked_King(king.Row + 1, king.Col, enemy, chessBoardArray)) return true;
            if (AmIChecked_King(king.Row + 1, king.Col + 1, enemy, chessBoardArray)) return true;
            if (AmIChecked_King(king.Row, king.Col - 1, enemy, chessBoardArray)) return true;
            if (AmIChecked_King(king.Row, king.Col + 1, enemy, chessBoardArray)) return true;
            if (AmIChecked_King(king.Row - 1, king.Col - 1, enemy, chessBoardArray)) return true;
            if (AmIChecked_King(king.Row - 1, king.Col, enemy, chessBoardArray)) return true;
            if (AmIChecked_King(king.Row - 1, king.Col + 1, enemy, chessBoardArray)) return true;

            return false;

        }

        private bool AmIChecked_RookBishopQueen(
                int kingRow,
                int kingCol,
                int rowOffset,
                int colOffset,
                string enemyPieces,
                string[,] chessBoardArray
            )
        {
            int row = kingRow + rowOffset;
            int col = kingCol + colOffset;

            while (col > 0 && col < 9 && row > 0 && row < 9)
            {
                string piece = chessBoardArray[row, col];
                if (IsMyPiece(piece)) return false;
                else if (enemyPieces.Contains(piece)) return true;
                else if (piece != ".") return false;
                col += colOffset;
                row += rowOffset;
            }

            return false;

        }

        private bool AmIChecked_King(
                int row,
                int col,
                string enemyPieces,
                string[,] chessBoardArray
            )
        {
            if (row > 0 && row < 9 && col > 0 && col < 9)
            {
                string piece = chessBoardArray[row, col];
                return enemyPieces.Contains(piece);
            }

            return false;
        }
        private bool IsMyPiece(string piece)
        {
            return
                this.PlayerIsBlack && "rnbqpk".Contains(piece)
                || this.PlayerIsWhite && "RNBQPK".Contains(piece)
                ;
        }

        private void MultiCheck(int thisRow, int thisCol, int rowOffset, int colOffset, string thisPiece)
        {
            int row = thisRow + rowOffset;
            int col = thisCol + colOffset;

            while (row > 0 && row < 9 && col > 0 && col < 9)
            {
                var isOpenSpace = this.ChessBoardArray[row, col] == ".";
                var enemyPiece = !IsMyPiece(this.ChessBoardArray[row, col]) && !isOpenSpace;

                if (isOpenSpace || enemyPiece)
                {
                    var opponentPiece = this.ChessBoardArray[row, col];
                    this.ChessBoardArray[row, col] = thisPiece;
                    var maybeMove = this.ChessBoardArray.ToBoardPositions();
                    this.MaybeMoves.Add(maybeMove);
                    this.ChessBoardArray[row, col] = opponentPiece;
                    if (enemyPiece) return;
                    row += rowOffset;
                    col += colOffset;
                }

                else return; //my piece

            }

        }
        private void TryThis(int nextRow, int nextCol, string thisPiece, CaptureCode cc)
        {

            if (nextRow > 0 && nextRow < 9 && nextCol > 0 && nextCol < 9)
            {
                var isOpenSpace = this.ChessBoardArray[nextRow, nextCol] == ".";
                var isEnemyPiece = !IsMyPiece(this.ChessBoardArray[nextRow, nextCol]) && !isOpenSpace;

                if (isOpenSpace && cc != CaptureCode.MustCapture)
                {
                    var savePiece = this.ChessBoardArray[nextRow, nextCol];
                    this.ChessBoardArray[nextRow, nextCol] = thisPiece;
                    var maybeMove = this.ChessBoardArray.ToBoardPositions();
                    this.MaybeMoves.Add(maybeMove);
                    this.ChessBoardArray[nextRow, nextCol] = savePiece;
                }

                if (isEnemyPiece && cc != CaptureCode.CantCapture)
                {
                    var savePiece = this.ChessBoardArray[nextRow, nextCol];
                    this.ChessBoardArray[nextRow, nextCol] = thisPiece;
                    var maybeMove = this.ChessBoardArray.ToBoardPositions();
                    this.MaybeMoves.Add(maybeMove);
                    this.ChessBoardArray[nextRow, nextCol] = savePiece;
                }

            }

        }

        private LastMove GetLastMove()
        {
            //this method compares the previous board to the current board 
            // and returns the actual move. For use in determining if en passant pawn
            // capture is valid.
            int pos = 1;
            var pb = this.MyLastMove.ToCharArray();
            var tb = this.OpponentsMove.ToCharArray();
            var fromto = pb.Zip(tb, (BegPiece, EndPiece) => new
            {
                BegPiece,
                EndPiece,
                Position = pos++
            })
                            .Where(r => r.BegPiece != r.EndPiece)
                            //.Select(r=> r.Position)
                            .ToList();
            ;

            var begPos = fromto.First();
            var endPos = fromto.Last();
            var from = fromto.Where(i => i.BegPiece == '.' || i.EndPiece == '.').First();

            LastMove lm = new LastMove();
            lm.Beg = Global.BoardMap.Where(bm => bm.Pos == begPos.Position).Single();
            lm.End = Global.BoardMap.Where(bm => bm.Pos == endPos.Position).Single();
            lm.Piece = from.BegPiece == '.' ? from.EndPiece : from.BegPiece;
            return lm;
        }

    }
}
