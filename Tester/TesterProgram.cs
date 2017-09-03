using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataProvider;
using System.Data.Entity;
using System.Threading;

namespace Tester
{
    class TesterProgram
    {
        static Func<int, int> matchesInTotal = a => a < 2 ? 0 : (a - 1) + matchesInTotal(a - 1);
        static Random r;

        static void Main(string[] args)
        {
            TournamentDataModel model = new TournamentDataModel();

            Session s = model.GetSession(1);
            r = new Random();

            while (true)
            {
                Match m = model.NextMatch(s);
                if (m == null) break;

                model.MatchFinished(m, r.Next(5), r.Next(5));
            }
          
           
            //Dalay.
            Console.WriteLine("Success");
            Thread.Sleep(1100);
        }
        static void Cross(List<int> collection)
        {
            int matches = 0;

            for (int i = 1; i < collection.Count; i++)
            {
                for (int j = 0; j < collection.Count; j++)
                {
                    int J = (j + i) % collection.Count;
                    if (J <= j) continue;

                    matches++;
                    Console.WriteLine(collection[j]+ " "+ collection[J]);
                }
                Console.WriteLine("------");
            }
            Console.WriteLine();
            Console.WriteLine("Result = "+ (matchesInTotal(collection.Count) == matches));
            Console.WriteLine("_____________________________________");
            Console.WriteLine();
        }
    }


}
