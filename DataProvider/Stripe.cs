using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DataProvider
{
    public class Stripe
    {        
        [Key]
        public int StripeID { get; set; }
       
        public Session Session { get; set; }  
       
        public Player Player { get; set; }

        public int GamesPlayed { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Loses { get; set; }
        public int GoalsScored { get; set; }
        public int GoalsMissed { get; set; }

        [NotMapped]
        public int GoalDifference => GoalsScored - GoalsMissed;
        [NotMapped]
        public int Score => Wins * 3 + Draws;
    }
}