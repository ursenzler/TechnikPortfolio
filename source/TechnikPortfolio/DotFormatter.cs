﻿namespace TechnikPortfolio
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
            dot.AppendLine("rankdir=RL");
            dot.AppendLine("ranksep = 4");

            foreach (var issue in issues)
            {
                dot.AppendLine($"{issue.Id} [label=<<table border=\"0\" cellborder=\"1\"><tr><td bgcolor=\"gray\"><font point-size=\"8\">{issue.Id}</font>{issue.Name}</td></tr><tr><td><font point-size=\"8\">{issue.Classification}</font></td></tr></table>>];");
                foreach (var linksTo in issue?.LinksTo ?? Enumerable.Empty<string>())
                {
                    dot.AppendLine($"{issue.Id} -> {linksTo};");
                }
            }

            dot.AppendLine("}");

            return dot.ToString();
        }
    }
}