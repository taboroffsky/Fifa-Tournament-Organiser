using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Threading.Tasks;

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

        public override string ToString()
        {
            return PlayerID + " " + NickName + " " + WinCount + " " + TotalGoalScored + " " + TotalMatchesPlayed;
        }
    }
}
