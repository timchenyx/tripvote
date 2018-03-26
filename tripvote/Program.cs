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
            string choice = "ABCDEFGH";
            while (lines.Count > 0)
            {
                Console.WriteLine("\nRound " + round++);
                var total = lines.Count();
                var t = lines.Where(i => i.Length > 0)  //ignore empty votes
                    .Select(i => i[0])                  //get first option of each vote
                    .GroupBy(i => i, (i, j) => new { option = i, count = j.Count(), percent = j.Count() * 100 / total })  //groupby first option
                    .OrderBy(i => i.count)              //order by count
                    .ToList();
                if (true)     //remove candidates with zero votes as first choic
                {
                    t = choice.Select(i => new
                    {
                        option = i,
                        count = t.Exists(j => j.option == i) ? t.First(j => j.option == i).count : 0,
                        percent = t.Exists(j => j.option == i) ? t.First(j => j.option == i).percent : 0
                    })
                        .OrderBy(i => i.count)
                        .ToList();
                }
                foreach (var i in t) Console.WriteLine(i.option + ":" + i.count + " " + i.percent + "%");
                Console.WriteLine("Remove " + t.First().option.ToString());
                choice = choice.Replace(t.First().option.ToString(), "");
                lines = lines.Select(i => i.Replace(t.First().option.ToString(), "")) //remove least popular option for this round
                    .Where(i => i.Length > 0)
                    .ToList();
                //Console.ReadLine();
            }
        }
    }
}
