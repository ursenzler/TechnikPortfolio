namespace TechnikPortfolio
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.IO;

    using Appccelerate.CommandLineParser;

    public class LevelRange
    {
        public Level From { get; set; }
        public Level To { get; set; }
    }

    public static class Program
    {
        static void Main(string[] args)
        {
            string username = null;
            string password = null;
            string filter = null;
            LevelRange levelRange = new LevelRange { From = Level.Keyword, To = Level.Tool };
            Collection<Classification> classifications = new Collection<Classification>()
                                                             {
                                                                 Classification.Beherrschen,
                                                                 Classification.Beobachten,
                                                                 Classification.Experimentieren
                                                             };

            var configuration = CommandLineParserConfigurator
                .Create()
                    .WithNamed("u", v => username = v)
                        .Required()
                        .HavingLongAlias("username")
                        .DescribedBy("username", "username to access YouTrack")
                    .WithNamed("p", v => password = v)
                        .Required()
                        .HavingLongAlias("password")
                        .DescribedBy("password", "password to access YouTrack")
                    .WithNamed("f", v => filter = v)
                        .DescribedBy("filter", "only elements related to elements matching the specified regex are included")
                    .WithNamed("l", v => levelRange = ParseLevelRange(v))
                        .DescribedBy("Levels", $"specify which levels to report in the format from..to with from and to one of [{Mappings.Of<Level>()}]. Default = K..T")
                    .WithNamed("k", v => classifications = ParseClassifications(v))
                        .DescribedBy("Klassifizierungen", $"comma seperated list of Klassifizierungen to include [{Mappings.Of<Classification>()}]. Default = {string.Join(",", classifications)}")
                    .BuildConfiguration();

            var parser = new CommandLineParser(configuration);
            ParseResult parseResult;
            try
            {
                parseResult = parser.Parse(args);
            }
            catch (Exception e)
            {
                parseResult = new ParseResult(false, e.Message);
            }

            // print usage if parsing failed
            if (!parseResult.Succeeded)
            {
                Usage usage = new UsageComposer(configuration).Compose();
                Console.WriteLine(parseResult.Message);
                Console.WriteLine("usage:" + usage.Arguments);
                Console.WriteLine("options");
                Console.WriteLine(usage.Options.IndentBy(4));
                Console.WriteLine();

                return;
            }

            try
            {
                Run(username, password, filter, levelRange, classifications);
            }
            catch (AggregateException e)
            {
                var message = e.InnerExceptions.Aggregate(string.Empty, (a, v) => $"{a}{Environment.NewLine}{v.Message}");
                Console.WriteLine(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static LevelRange ParseLevelRange(string v)
        {
            var parts = v.Split(new[] { ".." }, StringSplitOptions.RemoveEmptyEntries);
            return new LevelRange
                       {
                           From = parts[0].ParseShortcutAs<Level>(),
                           To = parts[1].ParseShortcutAs<Level>()
                       };
        }

        private static Collection<Classification> ParseClassifications(string classifications)
        {
            return new Collection<Classification>(
                classifications
                    .Split(',')
                    .Select(c => c.ParseShortcutAs<Classification>())
                    .ToList());
        }

        private static void Run(
            string username, 
            string password, 
            string filter, 
            LevelRange levelRange, 
            Collection<Classification> classifications)
        {
            Console.WriteLine("started using");
            Console.WriteLine($"    filter = {filter}");
            Console.WriteLine($"    levels included = {levelRange.From}..{levelRange.To}");
            Console.WriteLine($"    classifications included = {string.Join(",", classifications)}");

            var youTrack = new YouTrack();
            var issues = youTrack.GetIssues(username, password);

            Console.WriteLine($"{issues.Count} issues parsed");

            var issueFilter = new IssueFilter();
            var filteredIssues = issueFilter.FilterIssues(issues, levelRange, filter, classifications);

            Console.WriteLine($"{filteredIssues.Count} issues left after filtering. Filtered out:");
            foreach (var issue in issues.Except(filteredIssues))
            {
                Console.WriteLine($"    - {issue.Name}({issue.Level},{issue.Classification})");
            }

            var formatter = new DotFormatter();
            var dot = formatter.Format(filteredIssues);

            Console.WriteLine("writing dot file");

            WriteDotFile(dot);

            Console.WriteLine("running dot.exe");

            RunDotExecutable();

            Console.WriteLine("finished");
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