using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebStuff2.Models
{
    public class ChessMove
    {
        public string WebID;
        public string MyLastMove;
        public string ThisMove;
        public string Color;
        public bool IsLegalMove = true;
        public bool IsStalemate = false;
        public bool IsCheckmate = false;
    }
}