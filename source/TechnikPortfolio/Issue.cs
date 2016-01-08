namespace TechnikPortfolio
{
    using System.Collections.Generic;

    public class Issue
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public Level Level { get; set; }

        public Classification Classification { get; set; }

        public string Priority { get; set; }

        public IEnumerable<string> LinksTo { get; set; }
    }
}