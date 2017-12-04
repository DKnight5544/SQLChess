using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebStuff2.Models;
using SQLChess.PlayerAndEngines;

namespace WebStuff2.Controllers
{
    public class NewGameWhiteController : ApiController
    {
        public ChessMove Get()
        {
            string connstr = Environment.GetEnvironmentVariable("DWKDBConnectionString");
            using (DWKDBDataContext DB = new DWKDBDataContext(connstr))
            {
                ChessMove move = new ChessMove();
                move.WebID = DB.GetNewGame(selfPlay: false).Single().WebID;
                move.ThisMove = "RNBKQBNRPPPPPPPP................................pppppppprnbkqbnr".Replace(".", "x");
                //DB.SaveMove(move.WebID, move.ThisMove);
                return move;
            }
        }
    }
    public class NewGameBlackController : ApiController
    {
        public ChessMove Get()
        {
            string connstr = Environment.GetEnvironmentVariable("DWKDBConnectionString");
            using (DWKDBDataContext DB = new DWKDBDataContext(connstr))
            {
                ChessMove move = new ChessMove();
                Player white = new Player(SQLChess.PlayerColor.White);

                white.OpponentsMove = "RNBKQBNRPPPPPPPP................................pppppppprnbkqbnr";
                white.MyLastMove = "";
                white.KingHasMoved = false;
                white.KingsRookHasMoved = false;
                white.QueensRookHasMoved = false;

                string board = white.Move();

                move.WebID = DB.GetNewGame(selfPlay: false).Single().WebID;
                DB.SaveMove(move.WebID, board);
                move.ThisMove = board.Replace(".", "x");

                return move;
            }
        }
    }
}
