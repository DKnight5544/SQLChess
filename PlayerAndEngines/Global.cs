using System.Collections.Generic;
using System.Linq;
using SQLChess.PlayerAndEngines.CustomClasses;

namespace SQLChess.PlayerAndEngines
{
    public static class Global
    {
        private static string connstr = System.Environment.GetEnvironmentVariable("DWKDBConnectionString");
        public static DWKDBDataContext DB = new DWKDBDataContext(connstr);

        private static List<BoardMapRow> _bml;
        public static List<BoardMapRow> BoardMap
        {

            get
            {
                if (_bml == null)
                {
                    _bml =
                         Enumerable.Range(1, 64)
                             .Select(p => new
                             {
                                 p,
                                 r = (int)((p + 7) / 8)
                             })
                             .Select(t => new
                             {
                                 t.p,
                                 t.r,
                                 c = 8 - ((t.r * 8) - t.p)
                             })
                            .Select(t => new BoardMapRow(t.p, t.r, t.c))
                            .OrderBy(t => t.Pos)
                            .ToList();
                }

                return _bml;
            }

        }

        public static string ToBoardPositions(this string[,] positionArray)
        {
            string result = "";
            Global.BoardMap
                .ForEach(i =>
                {
                    result += positionArray[i.Row, i.Col];
                });

            return result;
        }

        public static string ToXML(this List<string> list)
        {
            string XML = "<ds>";
            list.ForEach(row => { XML += "<r><v>" + row + "</v></r>"; });
            XML += "</ds>";
            return XML;
        }

    }

}
