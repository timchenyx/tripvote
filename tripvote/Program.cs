using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace tripvote
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> lines = new List<string>(
                File.ReadAllLines(@"C:\scratch\Source\Repos\tripvote\vote.csv")
                .Select(i => i.Split(',').Last().ToUpper()) //assume last column is the vote
                .Where(i => i != "NO")  //ignore vote = "NO"
                );

            int round = 1;
            while (lines.Count > 0)
            {
                var total = lines.Count();
                var t = lines.Where(i => i.Length > 0)  //ignore empty votes
                    .Select(i => i[0])                  //get first option of each vote
                    .GroupBy(i => i, (i, j) => new { vote = i, count = j.Count(), percent = j.Count() * 100 / total })  //groupby first option
                    .OrderBy(i => i.count);             //order by count

                Console.WriteLine("Round " + round++);
                foreach (var i in t)
                    Console.WriteLine(i.vote + ":" + i.count + " " + i.percent + "%");

                lines = lines.Select(i => i.Replace(t.First().vote.ToString(), "")) //remove least popular option for this round
                    .Where(i => i.Length > 0)
                    .ToList();
                Console.ReadLine();
            }
        }
    }
}
