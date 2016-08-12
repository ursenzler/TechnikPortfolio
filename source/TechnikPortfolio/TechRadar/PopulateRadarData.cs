using System.Collections.Generic;

namespace TechnikPortfolio.TechRadar
{
    public class PopulateRadarData
    {
        private readonly string radarName;
        private readonly Dictionary<Quadrant, QuadrantData> radarData;
        private readonly RandomPositionProvider positionProvider;

        private PopulateRadarData()
        {
        }

        private PopulateRadarData(string radarName)
        {
            this.radarName = radarName;
            this.radarData = new Dictionary<Quadrant, QuadrantData>();

            this.InitializeQuadrant(Quadrant.TopRight);
            this.InitializeQuadrant(Quadrant.TopLeft);
            this.InitializeQuadrant(Quadrant.BottomRight);
            this.InitializeQuadrant(Quadrant.BottomLeft);


            this.positionProvider = new RandomPositionProvider();
        }

        public static PopulateRadarData ForRadarWithName(string radarName)
        {
            return new PopulateRadarData(radarName);
        }

        public PopulateQuadrant InQuadrant(Quadrant quadrant)
        {
            return new PopulateQuadrant(this, this.radarData[quadrant], this.positionProvider);
        }

        public RadarData GetPopulatedRadar()
        {
            return new RadarData(this.radarName, this.radarData);
        }

        private void InitializeQuadrant(Quadrant quadrant)
        {
            this.radarData.Add(quadrant, new QuadrantData(quadrant));
        }
    }
}