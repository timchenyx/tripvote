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
                .Select(i => i.Split(',').Last().ToUpper())
                .Where(i => i != "NO")
                );

            int round = 1;
            while (lines.Count > 0)
            {
                var total = lines.Count();
                var t = lines.Where(i => i.Length > 0)
                    .Select(i => i[0])
                    .GroupBy(i => i, (i, j) => new { vote = i, count = j.Count(), percent = j.Count() * 100 / total })
                    .OrderBy(i => i.count);

                Console.WriteLine("Round " + round++);
                foreach (var i in t)
                    Console.WriteLine(i.vote + ":" + i.count + " " + i.percent + "%");

                lines = lines.Select(i => i.Replace(t.First().vote.ToString(), ""))
                    .Where(i => i.Length > 0)
                    .ToList();
                Console.ReadLine();
            }
        }
    }
}
