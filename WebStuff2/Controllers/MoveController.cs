using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SQLChess.PlayerAndEngines;
using WebStuff2.Models;

namespace WebStuff2.Controllers
{
    public class MoveController : ApiController
    {
        public ChessMove Get(string id)
        {
            string connstr = Environment.GetEnvironmentVariable("DWKDBConnectionString");
            using (DWKDBDataContext DB = new DWKDBDataContext(connstr))
            {
                id = id.Replace("x", ".");
                var parms = id.Split('|');
                Player player = null;

                ChessMove move = new ChessMove();
                move.WebID = parms[0];
                move.ThisMove = parms[1];

                // here we will want to do some validation

                // first we will want to validate that the parameters look correct.
                // TODO: verify parameters

                var movesList = DB.GetGameByWebID(move.WebID).ToList();
                CastleCriteria cc = CheckForCastleCriteria(movesList);

                if (cc.lastPlayerIsWhite)
                {
                    //if last player was white then player is black
                    player = new Player(SQLChess.PlayerColor.Black);
                    player.KingHasMoved = cc.BlackKingHasMoved;
                    player.KingsRookHasMoved = cc.BlackKingsRookHasMoved;
                    player.QueensRookHasMoved = cc.BlackQueensRookHasMoved;
                    move.Color = "B";
                }
                else
                {
                    player = new Player(SQLChess.PlayerColor.White);
                    player.KingHasMoved = cc.WhiteKingHasMoved;
                    player.KingsRookHasMoved = cc.WhiteKingsRookHasMoved;
                    player.QueensRookHasMoved = cc.WhiteQueensRookHasMoved;
                    move.Color = "W";
                }

                if (movesList.Count == 0)
                {
                    player.OpponentsMove = "RNBKQBNRPPPPPPPP................................pppppppprnbkqbnr";
                    player.MyLastMove = "";
                }

                else if (movesList.Count == 1)
                {
                    player.OpponentsMove = movesList.Last().Board;
                    player.MyLastMove = "";
                }

                else
                {
                    player.OpponentsMove = movesList.Last().Board;
                    player.MyLastMove = movesList.Skip(movesList.Count - 2).First().Board;
                }
                

                List<string> legalMoves = player.FindLegalMoves();
                
                if (legalMoves.Count < 1)
                {
                    bool inCheck = player.AmIChecked(player.OpponentsMove);
                    if (inCheck)
                    {
                        move.ThisMove = string.Format("Checkmate: {0} loses.", player.PlayerIsWhite ? "White" : "Black");
                        move.IsCheckmate = true;
                        DB.SaveMove(move.WebID, move.ThisMove);
                        DB.SaveGameByWebID(move.WebID);
                        return move;
                    }
                    else
                    {
                        move.ThisMove = string.Format("Stalemate: {0} out of legal moves.", player.PlayerIsWhite ? "White" : "Black");
                        move.IsStalemate = true;
                        DB.SaveMove(move.WebID, move.ThisMove);
                        DB.SaveGameByWebID(move.WebID);
                        return move;
                    }
                }

                else if (legalMoves.Contains(move.ThisMove))
                {
                    DB.SaveMove(move.WebID, move.ThisMove);
                    move.MyLastMove = player.MyLastMove;
                    move.IsLegalMove = true;
                }
                else
                {
                    move.MyLastMove = player.MyLastMove;
                    move.ThisMove = player.OpponentsMove.Replace(".", "x");
                    move.IsLegalMove = false;
                    return move;
                }

                //*-------------------------------------------


                movesList = DB.GetGameByWebID(move.WebID).ToList();
                cc = CheckForCastleCriteria(movesList);

                if (cc.lastPlayerIsWhite)
                {
                    //if last player was white then player is black
                    player = new Player(SQLChess.PlayerColor.Black);
                    player.KingHasMoved = cc.BlackKingHasMoved;
                    player.KingsRookHasMoved = cc.BlackKingsRookHasMoved;
                    player.QueensRookHasMoved = cc.BlackQueensRookHasMoved;
                    move.Color = "B";
                }
                else
                {
                    player = new Player(SQLChess.PlayerColor.White);
                    player.KingHasMoved = cc.WhiteKingHasMoved;
                    player.KingsRookHasMoved = cc.WhiteKingsRookHasMoved;
                    player.QueensRookHasMoved = cc.WhiteQueensRookHasMoved;
                    move.Color = "W";
                }

                if (movesList.Count > 1)
                {
                    player.MyLastMove = movesList.Skip(movesList.Count - 2).First().Board;
                }
                else
                {
                    player.MyLastMove = "";
                }

                player.OpponentsMove = movesList.Skip(movesList.Count - 2).Last().Board;

                move.ThisMove = player.Move();

                if (move.ThisMove == string.Format("Checkmate: {0} loses.", player.PlayerIsWhite ? "White" : "Black"))
                {
                    move.IsCheckmate = true;
                    DB.SaveMove(move.WebID, move.ThisMove);
                    DB.SaveGameByWebID(move.WebID);
                    return move;
                }

                else if (move.ThisMove == string.Format("Stalemate: {0} out of legal moves.", player.PlayerIsWhite ? "White" : "Black"))
                {
                    move.IsStalemate = true;
                    DB.SaveMove(move.WebID, move.ThisMove);
                    DB.SaveGameByWebID(move.WebID);
                    return move;
                }

                else
                {
                    DB.SaveMove(move.WebID, move.ThisMove);
                    move.IsLegalMove = true;
                    move.ThisMove = move.ThisMove.Replace(".", "x");
                    return move;
                }
            }
        }
        private string CheckForStalemate(List<GetGameByWebIDResult> moveList)
        {

            if (moveList.Count > 50)
            {
                //50 Move Rule
                string result = Stalemate_50MoveRule(moveList);
                if (result != "GameOn") return result;
            }

            if (moveList.Count > 1)
            {
                //Solitary King vs too few pieces.
                var lastMove = moveList.OrderBy(m => m.MoveNum).Last();
                string result = Stalemate_TooFewPieces(lastMove);
                if (result != "GameOn") return result;
            }

            return "GameOn";

        }
        private string Stalemate_50MoveRule(List<GetGameByWebIDResult> moveList)
        {
            if (moveList.Count < 51) return "GameOn";

            //50 Moves with no capture and no Pawn Movement.
            var move1 = moveList
                    .OrderBy(l => l.MoveNum)
                    .Skip(moveList.Count() - 50)
                    .First()
                    .Board.ToCharArray()
            ;
            var move2 = moveList
                    .OrderBy(l => l.MoveNum)
                    .Last()
                    .Board.ToCharArray();
            ;

            var count1 = move1.Count(p => p == '.');
            var count2 = move1.Count(p => p == '.');

            //captures always result in more empty spaces.
            var captures = count1 != count2;

            if (captures) return "GameOn";

            //Check for pawn movement. Check first and last row.
            // finding the pawns, and seeing if they moved)
            bool pawnMovement =
                move1.Zip(move2, (p1, p2) => p1.ToString().ToUpper() == "P" && p1 != p2).Any(p => p == true);

            if (pawnMovement) return "GameOn";

            return "Stalemate: 50 move rule.";
        }
        private string Stalemate_TooFewPieces(GetGameByWebIDResult thisMove)
        {
            //In an endgame, the minimum of pieces necessary to force checkmate against a solitary king are:
            //•Queen alone (aided by the king).
            //•Rook alone (aided by the king).
            //•Two rooks.
            //•Two bishops (aided by the king).
            //•Knight and bishop(aided by the king) - rare.
            //•Three knights (aided by the king), one promoted -rare.



            bool solitaryWhiteKing = thisMove.Board
                                        .Where(piece => "QRNBP".Contains(piece))
                                        .Count() == 0
                                        ;

            bool solitaryBlackKing = thisMove.Board
                                        .Where(piece => "qrnbp".Contains(piece))
                                        .Count() == 0
                                        ;
            if (solitaryWhiteKing && solitaryBlackKing) return "Stalemates: Only two kings remain.";

            if (solitaryWhiteKing)
            {
                if (thisMove.Board.Any(piece => piece == 'q')) return "GameOn";
                if (thisMove.Board.Any(piece => piece == 'r')) return "GameOn";
                if (thisMove.Board.Any(piece => piece == 'p')) return "GameOn"; //pawns can be promoted
                if (thisMove.Board.Any(piece => piece == 'n') && thisMove.Board.Any(piece => piece == 'b')) return "GameOn";
                if (thisMove.Board.Where(piece => piece == 'b').Count() == 2) return "GameOn";
                if (thisMove.Board.Where(piece => piece == 'n').Count() == 3) return "GameOn";
                return "Stalemate: Solitary white king, too few black.";
            }

            if (solitaryBlackKing)
            {
                if (thisMove.Board.Any(piece => piece == 'Q')) return "GameOn";
                if (thisMove.Board.Any(piece => piece == 'R')) return "GameOn";
                if (thisMove.Board.Any(piece => piece == 'P')) return "GameOn"; //pawns can be promoted
                if (thisMove.Board.Any(piece => piece == 'N') && thisMove.Board.Any(piece => piece == 'B')) return "GameOn";
                if (thisMove.Board.Where(piece => piece == 'B').Count() == 2) return "GameOn";
                if (thisMove.Board.Where(piece => piece == 'N').Count() == 3) return "GameOn";
                return "Stalemate: Solitary black king, too few white.";
            }

            return "GameOn";

        }

        private CastleCriteria CheckForCastleCriteria(List<GetGameByWebIDResult> movesList)
        {
            CastleCriteria criteria = new CastleCriteria();

            if(movesList.Count == 0)
            {

                criteria.lastPlayerIsWhite = false;

                criteria.WhiteKingsRookHasMoved = false;
                criteria.WhiteKingHasMoved = false;
                criteria.WhiteQueensRookHasMoved = false;

                criteria.BlackKingsRookHasMoved = false;
                criteria.BlackKingHasMoved = false;
                criteria.BlackQueensRookHasMoved = false;


                return criteria;

            }



            criteria.lastPlayerIsWhite = movesList.Last().Color.Value == 'W';

            criteria.WhiteKingsRookHasMoved = movesList.Select(m => m.Board.Substring(0, 1)).Any(p => p != "R");
            criteria.WhiteKingHasMoved = movesList.Select(m => m.Board.Substring(3, 1)).Any(p => p != "K");
            criteria.WhiteQueensRookHasMoved = movesList.Select(m => m.Board.Substring(7, 1)).Any(p => p != "R");

            criteria.BlackKingsRookHasMoved = movesList.Select(m => m.Board.Substring(56, 1)).Any(p => p != "r");
            criteria.BlackKingHasMoved = movesList.Select(m => m.Board.Substring(59, 1)).Any(p => p != "k");
            criteria.BlackQueensRookHasMoved = movesList.Select(m => m.Board.Substring(63, 1)).Any(p => p != "r");


            return criteria;

        }

    }

}
