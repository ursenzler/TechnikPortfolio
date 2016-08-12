namespace TechnikPortfolio
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.IO;
    using System.Text.RegularExpressions;

    using Appccelerate.CommandLineParser;

    public class LevelRange
    {
        public Level From { get; set; }
        public Level To { get; set; }
    }

    public static class Program
    {
        private enum Outputs
        {
            [Mapping("Tree", "tree")]
            Tree,
            [Mapping("Radar", "radar")]
            Radar
        }

        static void Main(string[] args)
        {
            string username = null;
            string password = null;
            Outputs output = default(Outputs);
            IReadOnlyList<string> kompetenzen = null;
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
                    .WithNamed("o", v => output = ParseOutput(v))
                        .Required()
                        .HavingLongAlias("output")
                        .DescribedBy("output", "specifies what output to generate. Possible values: [tree, radar]")
                    .WithNamed("k", v => kompetenzen = ParseKompetenzen(v))
                        .HavingLongAlias("kompetenz")
                        .DescribedBy("kompetenzen", "comma separated list of Kompetenzen to include. All are included if not specified.")
                    .WithNamed("f", v => filter = v)
                        .DescribedBy("filter", "only elements related to elements matching the specified regex are included")
                    .WithNamed("l", v => levelRange = ParseLevelRange(v))
                        .DescribedBy("Levels", $"specify which levels to report in the format from..to with from and to one of [{Mappings.Of<Level>()}]. Default = K..T")
                    .WithNamed("c", v => classifications = ParseClassifications(v))
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
                Run(username, password, kompetenzen, filter, levelRange, classifications, output);
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

        private static IReadOnlyList<string> ParseKompetenzen(string v)
        {
            return v.Split(',').Select(x => x.Trim()).ToList();
        }

        private static LevelRange ParseLevelRange(string v)
        {
            var parts = v.Split(new[] { ".." }, StringSplitOptions.RemoveEmptyEntries);
            return new LevelRange
                       {
                           From = parts[0].ToUpperInvariant().ParseShortcutAs<Level>(),
                           To = parts[1].ToUpperInvariant().ParseShortcutAs<Level>()
                       };
        }

        private static Collection<Classification> ParseClassifications(string classifications)
        {
            return new Collection<Classification>(
                classifications
                    .Split(',')
                    .Select(c => c.ToUpperInvariant().ParseShortcutAs<Classification>())
                    .ToList());
        }

        private static Outputs ParseOutput(string v)
        {
            return v.ToLowerInvariant().ParseShortcutAs<Outputs>();
        }

        private static void Run(
            string username, 
            string password, 
            IReadOnlyList<string> kompetenzen,
            string filter, 
            LevelRange levelRange, 
            Collection<Classification> classifications,
            Outputs output)
        {
            Console.WriteLine("started using");
            Console.WriteLine($"    filter = {filter}");
            Console.WriteLine($"    levels included = {levelRange.From}..{levelRange.To}");
            Console.WriteLine($"    classifications included = {string.Join(",", classifications)}");
            Console.WriteLine($"    output as {output}");

            var youTrack = new YouTrack();
            var issues = youTrack.GetIssues(username, password);

            Console.WriteLine($"{issues.Count} issues parsed");

            var issueFilter = new IssueFilter();
            var filteredIssues = issueFilter.FilterIssues(issues, levelRange, kompetenzen, filter, classifications);

            Console.WriteLine($"{filteredIssues.Count} issues left after filtering. Filtered out:");
            foreach (var issue in issues.Except(filteredIssues))
            {
                Console.WriteLine($"    - {issue.Name}({issue.Level},{issue.Classification})");
            }

            switch (output)
            {
                case Outputs.Tree:
                    new TreeWriter().Write(filteredIssues);
                    break;

                case Outputs.Radar:
                    new RadarWriter().Write(filteredIssues);
                    break;
            }

            Console.WriteLine("finished");
        }
    }

    public static class ExtensionMethods
    {
        private static Regex Filter = new Regex(@"[^a-zA-Z0-9]");

        public static string Escape(this string s)
        {
            return Filter.Replace(s, string.Empty);
        }
    }
}