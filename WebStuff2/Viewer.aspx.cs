using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebStuff2
{
    public partial class Viewer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string connstr = Environment.GetEnvironmentVariable("DWKDBConnectionString");
            using (DWKDBDataContext DB = new DWKDBDataContext(connstr))
            {
                string gameID = Request.QueryString["gid"];
                int iGameID = 0;

                if (gameID != null) iGameID = int.Parse(gameID);

                //var moves = DB.GetLastGame();
                var moves = DB.GetGameByGameID(iGameID);

                litMoves.Text = "<script> const MoveObj = new Object(); \r\n";

                int i = 0;
                foreach (var move in moves.OrderBy(m => m.MoveID))
                {
                    litMoves.Text += string.Format("MoveObj[{0}] = '{1}';", ++i, move.Board);
                }

                litMoves.Text += "</script>";

            }
        }
    }
}