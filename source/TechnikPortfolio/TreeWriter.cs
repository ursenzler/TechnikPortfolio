namespace TechnikPortfolio
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;

    public class TreeWriter
    {
        public void Write(IReadOnlyList<Issue> issues)
        {
            var augmentedIssues = AddKompetenzenAsNodes(issues);

            var formatter = new DotFormatter();
            var dot = formatter.Format(augmentedIssues);

            Console.WriteLine("writing dot file");

            WriteDotFile(dot);

            Console.WriteLine("running dot.exe");

            RunDotExecutable();
        }

        private static IReadOnlyList<Issue> AddKompetenzenAsNodes(IReadOnlyList<Issue> issues)
        {
            var result = issues.ToList();

            var kompetenzen = issues
                .SelectMany(x => x.Kompetenzen)
                .Distinct()
                .Select(x => new Issue
                {
                    Id = x.Escape(),
                    Name = x
                });

            foreach (var issue in issues)
            {
                foreach (var kompetenz in issue.Kompetenzen)
                {
                    issue.LinksTo = issue.LinksTo.Union(new[] { kompetenz.Escape() });
                }
            }

            result.AddRange(kompetenzen);

            return result;
        }

        private static void WriteDotFile(string dot)
        {
            using (StreamWriter w = new StreamWriter("issues.dot"))
            {
                w.Write(dot);
            }
        }

        private static void RunDotExecutable()
        {
            using (var dot = new Process())
            {
                dot.StartInfo = new ProcessStartInfo("graphviz\\dot.exe", "-Tpng issues.dot -o issues.png");
                dot.Start();
            }
        }
    }
}