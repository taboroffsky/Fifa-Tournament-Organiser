using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProvider
{
    public class TournamentDataModel
    {
        static void Main()
        {
           
        }

        public void AddPlayer(Player p)
        {
            Player.Add(p);
        }
        public void AddPlayer(string nickname)
        {
            Player.Add(nickname);
        }
        public int AddSession(Session s)
        {
            return Session.Add(s);
        }
        public int AddSession(DateTime date, string description)
        {
            return Session.Add(date, description);
        }
        public Session GetSession(int id)
        {
            Session temp = null;
            using (var db = new TournamentDB())
            {
                temp = db.Sessions.Find(id);
            }
            return temp;
        }
        public ICollection<Player> GetAllPlayers()
        {
            ICollection<Player> temp = null;
            using (var db = new TournamentDB())
            {
                temp = (from p in db.Players
                        select p).ToList();
            }
            return temp;
        }
        public void AssignPlayerToSession(Session s, Player p)
        {
            s.AddPlayer(p);
        }
        public void AssingPlayerRangeToSession(Session s, IEnumerable<Player> array)
        {
            s.AddRangePlayer(array);
        }
        public void GenerateMatchTable(Session s)
        {
            s.GenerateMatchTable();
        }
        public Match NextMatch(Session s)
        {
            int? m =  s.NextMatch();
            if (m == null) return null;

            Match match = null;
            using (var db = new TournamentDB())
            {
                match = (from ma in db.Matches.Include("Player1").Include("Player2")
                         where ma.MatchID == (int)m
                         select ma).First();
            }
            return match;
        }
        public void MatchFinished(Match match, int g1, int g2)
        {
            Session.MatchFinished(match.MatchID, g1, g2);
        }
        public IEnumerable<Stripe> GetResultTable(Session s)
        {
            return s.GetResultTable();
        }
    }
}
