namespace TechnikPortfolio
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class DotFormatter
    {
        public string Format(IReadOnlyList<Issue> issues)
        {
            StringBuilder dot = new StringBuilder();
            dot.AppendLine("digraph Technik_Portfolio {");
            dot.AppendLine("node [shape=none fontname=Arial]");
            dot.AppendLine("rankdir=BT");
            dot.AppendLine("ranksep = 4");

            foreach (var issue in issues)
            {
                dot.AppendLine($"{issue.Id} [label=<<table border=\"0\" cellborder=\"1\"><tr><td bgcolor=\"gray\"><font point-size=\"8\">{issue.Id}</font>{issue.Name}</td></tr><tr><td><font point-size=\"8\">{issue.Classification}</font></td></tr><tr><td><font point-size=\"8\">{issue.Priority}</font></td></tr></table>>];");
                foreach (var linksTo in issue.LinksTo)
                {
                    dot.AppendLine($"{issue.Id} -> {linksTo};");
                }
            }

            string kompetenzen = issues.Where(i => i.Level == Level.Kompetenz).Aggregate("", (a, v) => $"{a} {v.Id}");
            dot.AppendLine($"{{rank=same {kompetenzen}}}");

            string keywords = issues.Where(i => i.Level == Level.Keyword).Aggregate("", (a, v) => $"{a} {v.Id}");
            dot.AppendLine($"{{rank=same {keywords}}}");

            dot.AppendLine("}");

            return dot.ToString();
        }
    }
}