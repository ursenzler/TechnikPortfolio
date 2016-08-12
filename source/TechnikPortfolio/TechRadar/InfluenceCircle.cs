using System.Collections.Generic;

namespace TechnikPortfolio.TechRadar
{
    public class InfluenceCircle
    {
        private readonly Circle circle;
        private readonly IEnumerable<BlipPosition> positions;

        public InfluenceCircle(Circle circle, List<BlipPosition> positions)
        {
            this.circle = circle;
            this.positions = positions;
        }

        public Circle Circle => this.circle;

        public IEnumerable<BlipPosition> Positions => this.positions;
    }
}