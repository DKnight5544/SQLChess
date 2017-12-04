using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using SQLChess.PlayerAndEngines;
namespace SQLChess.PlayerAndEngines
{
    public class SelfPlayEngine
    {
        private BackgroundWorker Caller = null;
        private List<string> GameHistory { get; set; }
        private int Counter_50MoveRule { get; set; }
        public SelfPlayEngine()
        {
            this.GameHistory = new List<string>();
        }
        public void Start(object sender, DoWorkEventArgs e)
        {
            if (sender.GetType() != typeof(BackgroundWorker))
            {
                throw new Exception("Calling object should be a BackgroundWorker");
            }

            int? gamesToPlay = e.Argument as int?;
            Caller = sender as BackgroundWorker;
            Start(gamesToPlay.Value);
        }
        public void Start(int gamesToPlay)
        {
           

            while (gamesToPlay > 0)
            {
                string board = "RNBKQBNRPPPPPPPP................................pppppppprnbkqbnr";
                string lastMove_white = "";
                string lastMove_black = "";
                bool kingHasMoved_white = false;
                bool kingsRookHasMoved_white = false;
                bool queensRookHasMoved_white = false;
                bool kingHasMoved_black = false;
                bool kingsRookHasMoved_black = false;
                bool queensRookHasMoved_black = false;
                int? gameID = Global.DB.GetNewGame(selfPlay: true).Single().GameID;
                this.GameHistory.Clear();

                if (Caller != null && Caller.WorkerReportsProgress)
                {
                    Caller.ReportProgress(0, gameID);
                }                

                Player white = new Player(PlayerColor.White);
                Player black = new Player(PlayerColor.Black);

                while (1 == 1) //this.GameHistory.Count() < 500)
                {
                    //-----------White----------------------

                    white.OpponentsMove = board;
                    white.MyLastMove = lastMove_white;
                    white.KingHasMoved = kingHasMoved_white;
                    white.KingsRookHasMoved = kingsRookHasMoved_white;
                    white.QueensRookHasMoved = queensRookHasMoved_white;

                    board = white.Move();

                    lastMove_white = board;
                    kingHasMoved_white = white.KingHasMoved;
                    kingsRookHasMoved_white = white.KingsRookHasMoved;
                    queensRookHasMoved_white = white.QueensRookHasMoved;

                    this.GameHistory.Add(board);
                    if (board.Contains("Stalemate") || board.Contains("Checkmate") || board.Contains("Save")) break;

                    string status = CheckForStalemate(board);
                    if (status != "GameOn")
                    {
                        this.GameHistory.Add(status);
                        break;
                    }

                    //-----------Black----------------------

                    black.OpponentsMove = board;
                    black.MyLastMove = lastMove_black;
                    black.KingHasMoved = kingHasMoved_black;
                    black.KingsRookHasMoved = kingsRookHasMoved_black;
                    black.QueensRookHasMoved = queensRookHasMoved_black;

                    board = black.Move();

                    lastMove_black = board;
                    kingHasMoved_black = black.KingHasMoved;
                    kingsRookHasMoved_black = black.KingsRookHasMoved;
                    queensRookHasMoved_black = black.QueensRookHasMoved;


                    this.GameHistory.Add(board);
                    if (board.Contains("Stalemate") || board.Contains("Checkmate") || board.Contains("Save")) break;

                    status = CheckForStalemate(board);
                    if (status != "GameOn")
                    {
                        this.GameHistory.Add(status);
                        break;
                    }

                }

                string actualMoveXML = this.GameHistory.ToXML();
                Global.DB.SaveGame(actualMoveXML, gameID);

                gamesToPlay--;

            }

        }

        private string CheckForStalemate(string board)
        {

            if (GameHistory.Count > 1)
            {
                List<char[]> lastTwoMoves = GameHistory
                    .Skip(GameHistory.Count - 2)
                    .Select(m => m.ToCharArray())
                    .ToList()
                    ;

                char[] lastMove = lastTwoMoves.First();
                char[] thisMove = lastTwoMoves.Last();

                //50 Move Rule
                string result = Stalemate_50MoveRule(lastMove, thisMove);
                if (result != "GameOn") return result;


                //Solitary King vs too few pieces.
                result = Stalemate_TooFewPieces(thisMove);
                if (result != "GameOn") return result;

            };

            return "GameOn";

        }

        private string Stalemate_50MoveRule(char[] lastMove, char[] thisMove)
        {
            //50 Moves with no capture and no Pawn Movement.
            // Check Last 2 moves, compare empty space count
            int lastMoveEmptySpaceCount = lastMove.Where(i => i == '.').Count();
            int thisMoveEmptySpaceCount = thisMove.Where(i => i == '.').Count();
            bool noCaptures = (lastMoveEmptySpaceCount == thisMoveEmptySpaceCount);
            if (noCaptures)
            {
                //Check for pawn movement (Compare lm & tm line by line,
                // finding the pawns, and seeing if they moved)
                bool noPawnMovement = !lastMove.Zip(thisMove, (m1, m2) => (
                                   "Pp".Contains(m1.ToString()) //if it's a pawn
                                   && (m1 != m2) //if moved or taken
                                   ))
                                    .Any(p => p == true);

                if (noPawnMovement)
                {
                    this.Counter_50MoveRule += 1;
                    if (this.Counter_50MoveRule == 50) return "Stalemate: 50 move rule. ";
                }
                else this.Counter_50MoveRule = 0;
            }
            else this.Counter_50MoveRule = 0;

            return "GameOn";
        }

        private string Stalemate_TooFewPieces(char[] thisMove)
        {
            //In an endgame, the minimum of pieces necessary to force checkmate against a solitary king are:
            //•Queen alone (aided by the king).
            //•Rook alone (aided by the king).
            //•Two rooks.
            //•Two bishops (aided by the king).
            //•Knight and bishop(aided by the king) - rare.
            //•Three knights (aided by the king), one promoted -rare.



            bool solitaryWhiteKing = thisMove
                                        .Where(piece => "QRNBP".Contains(piece))
                                        .Count() == 0
                                        ;

            bool solitaryBlackKing = thisMove
                                        .Where(piece => "qrnbp".Contains(piece))
                                        .Count() == 0
                                        ;
            if (solitaryWhiteKing && solitaryBlackKing) return "Stalemates: Only two kings remain.";

            if (solitaryWhiteKing)
            {
                if (thisMove.Any(piece => piece == 'q')) return "GameOn";
                if (thisMove.Any(piece => piece == 'r')) return "GameOn";
                if (thisMove.Any(piece => piece == 'p')) return "GameOn"; //pawns can be promoted
                if (thisMove.Any(piece => piece == 'n') && thisMove.Any(piece => piece == 'b')) return "GameOn";
                if (thisMove.Where(piece => piece == 'b').Count() == 2) return "GameOn";
                if (thisMove.Where(piece => piece == 'n').Count() == 3) return "GameOn";
                return "Stalemate: Solitary white king, too few black.";
            }

            if (solitaryBlackKing)
            {
                if (thisMove.Any(piece => piece == 'Q')) return "GameOn";
                if (thisMove.Any(piece => piece == 'R')) return "GameOn";
                if (thisMove.Any(piece => piece == 'P')) return "GameOn"; //pawns can be promoted
                if (thisMove.Any(piece => piece == 'N') && thisMove.Any(piece => piece == 'B')) return "GameOn";
                if (thisMove.Where(piece => piece == 'B').Count() == 2) return "GameOn";
                if (thisMove.Where(piece => piece == 'N').Count() == 3) return "GameOn";
                return "Stalemate: Solitary black king, too few white.";
            }

            return "GameOn";

        }
    }
}
