using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Linq;

namespace DataProvider
{
    public class Player
    {   
        [Key]
        public int PlayerID { get; set; }

        [Required]
        public string NickName { get; set; }

        [DefaultValue(0)]
        public int WinCount { get; set; }

        [DefaultValue(0), Column("Matches")]
        public int TotalMatchesPlayed { get; set; }

        [DefaultValue(0), Column("Goals")]
        public int TotalGoalScored { get; set; }


        internal static void Add(string nickname)
        {
            using (var db = new TournamentDB())
            {
                db.Players.Add(new Player() { NickName = nickname });
                db.SaveChanges();
            }
        }
        internal static void Add(Player p)
        {
            using (var db = new TournamentDB())
            {
                db.Players.Add(p);
                db.SaveChanges();
            }
        }
        internal async Task UpdateDataAsync()
        {
            await Task.Factory.StartNew(UpdateData);
        }
        public override string ToString()
        {
            return PlayerID + " " + NickName + " " + WinCount + " " + TotalGoalScored + " " + TotalMatchesPlayed;
        }
        private void UpdateData()
        {
            using (var db = new TournamentDB())
            {
                Player temp = db.Players.Find(this.PlayerID);

                var query = from stripe in db.Stripes
                            where stripe.Player.PlayerID == temp.PlayerID
                            select stripe;


                temp.TotalMatchesPlayed = query.Sum(s => s.GamesPlayed);
                temp.WinCount = query.Sum(s => s.Wins);
                temp.TotalGoalScored = query.Sum(s => s.GoalsScored);

                db.SaveChanges();
            }
        }
    }
}
