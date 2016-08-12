namespace TechnikPortfolio
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using RestSharp.Extensions.MonoHttp;
    using TechnikPortfolio.TechRadar;
    using TechnikPortfolio.TechRadar.Formatting;

    public class RadarWriter
    {
        public void Write(IReadOnlyList<Issue> issues)
        {
            string[] topLeft =
            {
                "library",
                "framework"
            };

            string[] topRight =
            {
                "concept",
                "method",
                "process"
            };

            string[] bottomLeft =
            {
                "service",
                "product",
                "tool-software"
            };

            string[] bottomRight =
            {
                "theme",
                "kompetenz",
                "thingy"
            };

            var kompetenzen = issues.SelectMany(x => x.Kompetenzen).Distinct();

            foreach (var kompetenz in kompetenzen)
            {
                var data = PopulateRadarData.ForRadarWithName($"Kompetenz: {kompetenz}")
                    .InQuadrant(Quadrant.TopLeft)
                    .Add(() => Filter(issues, kompetenz, topLeft))
                    .InQuadrant(Quadrant.TopRight)
                    .Add(() => Filter(issues, kompetenz, topRight))
                    .InQuadrant(Quadrant.BottomLeft)
                    .Add(() => Filter(issues, kompetenz, bottomLeft))
                    .InQuadrant(Quadrant.BottomRight)
                    .Add(() => Filter(issues, kompetenz, bottomRight))
                    .GetPopulatedRadar();

                var formatter = new RadarFormatter();

                var jsFileContent = formatter.FormatDataToJsString(data);
                File.WriteAllText($"TechRadar\\template\\radar-{HttpUtility.UrlEncode(kompetenz)}.js", jsFileContent);

                var htmlTemplate = formatter.FormatHtmlFor($"radar-{HttpUtility.UrlEncode(kompetenz)}.js");
                File.WriteAllText($"TechRadar\\template\\radar-{HttpUtility.UrlEncode(kompetenz)}.html", htmlTemplate);
            }
        }

        private static IEnumerable<Issue> Filter(IEnumerable<Issue> issues, string kompetenz, IEnumerable<string> types)
        {
            return issues
                .Where(x => x.Kompetenzen.Contains(kompetenz))
                .Where(x => x.Types.Any(t => types.Contains(t.ToLowerInvariant())));
        }
    }
}