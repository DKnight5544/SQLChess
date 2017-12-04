using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLChess
{
        public enum PlayerColor { White, Black };

        public enum MoveResult { Checkmate, Stalemate, YourTurn};

        public enum CaptureCode { CantCapture, CanCapture, MustCapture}

}
