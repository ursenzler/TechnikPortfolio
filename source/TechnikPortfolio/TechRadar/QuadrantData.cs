using System.Collections.Generic;

namespace TechnikPortfolio.TechRadar
{
    public class QuadrantData
    {
        private readonly Quadrant quadrant;
        private readonly List<Blip> items;

        public QuadrantData(Quadrant quadrant)
        {
            this.quadrant = quadrant;
            this.items = new List<Blip>();
        }

        public Quadrant Quadrant => this.quadrant;

        public ICollection<Blip> Items => this.items;
    }
}