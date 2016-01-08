namespace TechnikPortfolio
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
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
                        .DescribedBy("filter", "only root elements that match this regex will be shown (root elements are by default Keyword, unless you defined a different level range).")
                    .WithNamed("l", v => levelRange = ParseLevelRange(v))
                        .DescribedBy("Levels", $"specify which levels to report in the format from..to with from and to one of [{Mappings.Of<Level>()}]")
                    .WithNamed("k", v => classifications = ParseClassifications(v))
                        .DescribedBy("Klassifizierungen", $"comma seperated list of Klassifizierungen to include [{Mappings.Of<Classification>()}].")
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
            string keywordFilter, 
            LevelRange levelRange, 
            Collection<Classification> classifications)
        {
            Console.WriteLine("started");

            var youTrack = new YouTrack();
            var issues = youTrack.GetIssues(username, password);

            Console.WriteLine($"{issues.Count} issues parsed");

            var filter = new IssueFilter();
            issues = filter.FilterIssues(issues, levelRange, keywordFilter, classifications);

            Console.WriteLine($"{issues.Count} issues left after filtering");

            var formatter = new DotFormatter();
            var dot = formatter.Format(issues);

            WriteDotFile(dot);

            Console.WriteLine("finished");
        }

        private static void WriteDotFile(string dot)
        {
            using (StreamWriter w = new StreamWriter("issues.dot"))
            {
                w.Write(dot);
            }
        }
    }
}