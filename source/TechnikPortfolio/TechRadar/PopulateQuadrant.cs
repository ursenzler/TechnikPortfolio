using System;
using System.Collections.Generic;

namespace TechnikPortfolio.TechRadar
{
    public class PopulateQuadrant
    {
        private readonly Quadrant quadrant;
        private readonly PopulateRadarData populateRadarData;
        private readonly QuadrantData quadrantData;
        private readonly RandomPositionProvider positionProvider;

        private readonly Dictionary<Classification, Circle> circleClassificationMapping =
            new Dictionary<Classification, Circle>
            {
                { Classification.NichtVerwenden, Circle.DoNotUse },
                { Classification.Experimentieren, Circle.Experiment },
                { Classification.Beobachten, Circle.Observe },
                { Classification.Beherrschen, Circle.Master }
            };

        public PopulateQuadrant(PopulateRadarData populateRadarData, QuadrantData quadrantData, RandomPositionProvider positionProvider)
        {
            this.populateRadarData = populateRadarData;
            this.quadrantData = quadrantData;
            this.quadrant = quadrantData.Quadrant;
            this.positionProvider = positionProvider;
        }

        public PopulateRadarData Add(Func<IEnumerable<Issue>> issuesSelector)
        {
            foreach (var issue in issuesSelector())
            {
                if(!this.circleClassificationMapping.ContainsKey(issue.Classification))
                {
                    continue;
                }

                var position = this.positionProvider.GetRandomPosition(this.quadrant,
                    circleClassificationMapping[issue.Classification]);

                this.quadrantData.Items.Add(new Blip(issue.Name, position.Radius, position.Angle));
            }
            return populateRadarData;
        }
    }
}