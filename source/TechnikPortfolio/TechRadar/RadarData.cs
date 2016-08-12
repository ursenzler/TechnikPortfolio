using System.Collections.Generic;

namespace TechnikPortfolio.TechRadar
{
    public class RadarData
    {
        private readonly string name;
        private readonly IReadOnlyDictionary<Quadrant, QuadrantData> data;

        public RadarData(string radarName, IReadOnlyDictionary<Quadrant, QuadrantData> data)
        {
            this.name = radarName;
            this.data = data;
        }

        public string Name => this.name;

        public IReadOnlyDictionary<Quadrant, QuadrantData> Data => this.data;
    }
}