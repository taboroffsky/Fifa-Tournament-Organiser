using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataProvider
{
    [Table("Scedule")]
    public class Match
    {      
        [Key]
        public int MatchID { get; set; }

        public Session Session { get; set; }       

        public Player Player1 { get; set; }
        public Player Player2 { get; set; }

        public bool IsFinished { get; set; }

        public int? P1Scored { get; set; }
        public int? P2Scored { get; set; }


        public override string ToString()
        {
            return String.Format("{0} ) {1} - {2}  ({3} - {4}) is finished: {5}", MatchID, Player1.NickName, Player2.NickName, P1Scored, P2Scored, IsFinished);
        }
    }
}
