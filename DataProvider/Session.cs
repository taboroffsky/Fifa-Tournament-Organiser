using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Data.Entity;

namespace DataProvider
{
    public class Session
    {
        public Session()
        {
            Stripes = new List<Stripe>();
            Matches = new List<Match>();
        }


        [Key]
        public int SessionID { get; set; }       
        public bool IsFinished { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "Date")]
        public DateTime StartDate { get; set; }

        public ICollection<Stripe> Stripes { get; set; }
        public ICollection<Match> Matches { get; set; }
        

        internal static int Add(DateTime date, string description)
        {
            int i = -1;

            using (var db = new TournamentDB())
            {
                db.Sessions.Add(new Session() { StartDate = date, Description = description });
                db.SaveChanges();
                i = db.Sessions.ToList().Last().SessionID;
            }

            return i;
        }
        internal static int Add(Session s)
        {
            using (var db = new TournamentDB())
            {
                db.Sessions.Add(s);
                db.SaveChanges();
            }
            return s.SessionID;
        }
        internal void AddPlayer(Player p)
        {
            using (var db = new TournamentDB())
            {
                Session temp = db.Sessions.Find(SessionID);
                temp.Stripes.Add(new Stripe() { Player = db.Players.Find(p.PlayerID) });
                db.SaveChanges();
            }
        }
        internal void AddRangePlayer(IEnumerable<Player> collection)
        {
            using (var db = new TournamentDB())
            {
                foreach (Player p in collection)
                {
                    Session temp = db.Sessions.Find(SessionID);
                    temp.Stripes.Add(new Stripe() { Player = db.Players.Find(p.PlayerID) });
                    
                }                 
                db.SaveChanges();

            }
        }
        internal void GenerateMatchTable()
        {
            using (var db = new TournamentDB())
            {  
                foreach (dynamic item in GenerateMatches())
                {
                    Session temp = db.Sessions.Find(this.SessionID);
                    temp.Matches.Add(new Match()
                    {
                        Player1 = db.Players.Find(item.p1),
                        Player2 = db.Players.Find(item.p2)
                    });
                }

                db.SaveChanges();
            }
        }
        internal int? NextMatch()
        {
            int? temp = null;

            using (var db = new TournamentDB())
            {
                Session s = db.Sessions.Find(this.SessionID);

                var query = db.Matches.Where(m => m.Session.SessionID == s.SessionID).Where(m => m.IsFinished == false);

                if(query.Count() != 0)
                    temp = query.First().MatchID;
                else
                {
                    s.IsFinished = true;
                    db.SaveChanges();
                }
            }

            return temp;
        }
        internal static void MatchFinished(int matchId, int g1, int g2)
        {
            using (var db = new TournamentDB())
            {

                var coll = db.Matches.ToList();
                Match m = db.Matches.Find(matchId);
                m.P1Scored = g1;
                m.P2Scored = g2;
                m.IsFinished = true;

                Session temp = db.Matches.Where(mat => mat.MatchID == m.MatchID)
                                         .Select(mat => mat.Session)
                                         .First();

                Player p1 = db.Matches.Where(mat => mat.MatchID== m.MatchID)
                                      .Select(mat => mat.Player1)
                                      .First();
                Player p2 = db.Matches.Where(mat => mat.MatchID == m.MatchID)
                                      .Select(mat => mat.Player2)
                                      .First();

                Stripe s1 = (from s in db.Stripes
                             where s.Session.SessionID == m.Session.SessionID
                             where s.Player.PlayerID == p1.PlayerID
                             select s).First();

                Stripe s2 = (from s in db.Stripes
                             where s.Session.SessionID == m.Session.SessionID
                             where s.Player.PlayerID == p2.PlayerID
                             select s).First();

                if (s1 == null | s2 == null) throw new Exception("stripe is null"); //TO DELETE

                if (g1 > g2)
                {
                    s1.Wins++;
                    s2.Loses++;
                }
                else if (g1 < g2)
                {
                    s1.Loses++;
                    s2.Wins++;
                }
                else
                {
                    s1.Draws++;
                    s2.Draws++;
                }

                s1.GamesPlayed++;
                s2.GamesPlayed++;

                s1.GoalsScored += g1;
                s1.GoalsMissed += g2;
                s2.GoalsScored += g2;
                s2.GoalsMissed += g1;

                db.SaveChanges();

                p1.UpdateDataAsync();
                p2.UpdateDataAsync();
            }
        }
        internal IEnumerable<Stripe> GetResultTable()
        {
            IEnumerable<Stripe> query;

            using (var db = new TournamentDB())
            {
                Session s = db.Sessions.Find(this.SessionID);

                query = (from str in db.Stripes.Include("Player").Include("Session")
                        where str.Session.SessionID == s.SessionID                      
                        select str).ToList();
            }

            return query.OrderByDescending(s => s.Score).ThenByDescending(s => s.GoalDifference).ThenByDescending(s => s.GoalsScored);
        }


        private static Func<int, int> matchesInTotal = a => a < 2 ? 0 : (a - 1) + matchesInTotal(a - 1);

        /// <summary>
        /// Creates a collection of all possible matched due to players in Stripes.
        /// </summary>
        /// <returns>Collection of ordered matches, that contains pleyers` ID</returns>
        private ICollection<dynamic> GenerateMatches() 
        {
            dynamic collection;

            using (var db = new TournamentDB())
            {
                Session s = db.Sessions.Find(this.SessionID);
               collection = (db.Stripes.Where(str => str.Session.SessionID == s.SessionID).Select(str => str.Player)).ToList();
            }

            List<dynamic> list = new List<dynamic>();
             
            for (int i = 1; i < collection.Count; i++)
            {
                for (int j = 0; j < collection.Count; j++)
                {
                    int J = (j + i) % collection.Count;
                    if (J <= j) continue;

                    list.Add(new { p1 = collection[j].PlayerID, p2 = collection[J].PlayerID });
                }
            }
            if (list.Count != matchesInTotal(collection.Count)) throw new Exception("Scedule creation error!");

            return list;
        }
    }
}
