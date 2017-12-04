using System;
using System.Collections.Generic;
using System.Threading;
using System.Web.Services;
using System.Linq;
using SQLChess.PlayerAndEngines;

namespace WebStuff2
{
    public partial class SelfPlay : System.Web.UI.Page
    {

        private static string now = DateTime.Now.Ticks.ToString();
        public string cssBody = string.Format("href='css/SelfPlay.css?{0}'", now);
        public string jsSrc = string.Format("src= 'js/SelfPlay.js?{0}'", now);
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private static void StartSelfPlay(object data)
        {
            int gameCount = (int)data;
            var engine = new SQLChess.PlayerAndEngines.SelfPlayEngine();
            engine.Start(gameCount);
        }



        [WebMethod]
        public static GetSelfGameStatsResult GetStats()
        {
            string connstr = Environment.GetEnvironmentVariable("DWKDBConnectionString");
            using (DWKDBDataContext DB = new DWKDBDataContext(connstr))
            {

                GetSelfGameStatsResult stats = DB.GetSelfGameStats().Single();

                return stats;
            }

        }

        [WebMethod]
        public static string RestartEngine()
        {
            var newThread = new Thread(StartSelfPlay);
            newThread.Start(10000);

            return string.Format("Engine restarted at: {0}", DateTime.Now);

        }
    }
}