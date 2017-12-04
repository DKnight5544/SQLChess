using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebStuff2.Models
{
    public class CastleCriteria
    {
        public bool WhiteKingHasMoved;
        public bool WhiteKingsRookHasMoved;
        public bool WhiteQueensRookHasMoved;
        public bool BlackKingHasMoved;
        public bool BlackKingsRookHasMoved;
        public bool BlackQueensRookHasMoved;
        public bool lastPlayerIsWhite;
    }
}