

namespace SQLChess.PlayerAndEngines.CustomClasses
{
    struct LastMove
    {
        public BoardMapRow Beg { get; set; }
        public BoardMapRow End { get; set; }
        public char Piece { get; set; }
    }
}
