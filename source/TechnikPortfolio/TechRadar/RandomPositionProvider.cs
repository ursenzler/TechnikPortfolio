using System;
using System.Collections.Generic;
using System.Linq;

namespace TechnikPortfolio.TechRadar
{
    public class RandomPositionProvider
    {
        readonly Dictionary<Quadrant, List<InfluenceCircle>> postitionData = new Dictionary<Quadrant, List<InfluenceCircle>>();

        private readonly Random random;

        public RandomPositionProvider()
        {
            this.random = new Random();

            this.InitializeTopLeftPosition();
            this.InitializeTopRightPosition();
            this.InitializeBottomLeftPosition();
            this.InitializeBottomRightPosition();
        }

        public BlipPosition GetRandomPosition(Quadrant quadrant, Circle circle)
        {
            var availableBlipPoisitions =
                this.postitionData[quadrant]
                .Single(item => item.Circle == circle)
                .Positions
                .Where(position => !position.IsUsed).ToArray();

            var next = this.random.Next(0, availableBlipPoisitions.Length);

            var selectedPosition = availableBlipPoisitions[next];
            selectedPosition.IsUsed = true;
            return selectedPosition;
        }

        private void InitializeBottomRightPosition()
        {
            var influenceCircles = new List<InfluenceCircle>
                                       {
                                           new InfluenceCircle(
                                               Circle.Master,
                                               new List<BlipPosition>
                                                   {
                                                       new BlipPosition(20, 320),
                                                       new BlipPosition(40, 290),
                                                       new BlipPosition(40, 320),
                                                       new BlipPosition(45, 345),
                                                       new BlipPosition(60, 280),
                                                       new BlipPosition(60, 300),
                                                       new BlipPosition(60, 320),
                                                       new BlipPosition(65, 340),
                                                       new BlipPosition(85, 350),
                                                       new BlipPosition(80, 280),
                                                       new BlipPosition(80, 295),
                                                       new BlipPosition(80, 310),
                                                       new BlipPosition(80, 325)
                                                   }),
                                           new InfluenceCircle(
                                               Circle.Experiment,
                                               new List<BlipPosition>
                                                   {
                                                       new BlipPosition(130, 280),
                                                       new BlipPosition(130, 290),
                                                       new BlipPosition(130, 300),
                                                       new BlipPosition(130, 310),
                                                       new BlipPosition(130, 320),
                                                       new BlipPosition(130, 330),
                                                       new BlipPosition(130, 340),
                                                       new BlipPosition(130, 350),
                                                       new BlipPosition(165, 275),
                                                       new BlipPosition(165, 285),
                                                       new BlipPosition(165, 295),
                                                       new BlipPosition(165, 305),
                                                       new BlipPosition(165, 315),
                                                       new BlipPosition(165, 325),
                                                       new BlipPosition(165, 335),
                                                       new BlipPosition(165, 345),
                                                       new BlipPosition(165, 355),
                                                   }),
                                           new InfluenceCircle(
                                               Circle.Observe,
                                               new List<BlipPosition>
                                                   {
                                                       new BlipPosition(230, 280),
                                                       new BlipPosition(230, 290),
                                                       new BlipPosition(230, 300),
                                                       new BlipPosition(230, 310),
                                                       new BlipPosition(230, 320),
                                                       new BlipPosition(230, 330),
                                                       new BlipPosition(230, 340),
                                                       new BlipPosition(230, 350),
                                                       new BlipPosition(260, 275),
                                                       new BlipPosition(260, 285),
                                                       new BlipPosition(260, 295),
                                                       new BlipPosition(260, 305),
                                                       new BlipPosition(260, 315),
                                                       new BlipPosition(260, 325),
                                                       new BlipPosition(260, 335),
                                                       new BlipPosition(260, 345),
                                                       new BlipPosition(260, 355),
                                                   }),
                                           new InfluenceCircle(
                                               Circle.DoNotUse,
                                               new List<BlipPosition>
                                                   {
                                                       new BlipPosition(330, 280),
                                                       new BlipPosition(330, 290),
                                                       new BlipPosition(330, 300),
                                                       new BlipPosition(330, 310),
                                                       new BlipPosition(330, 320),
                                                       new BlipPosition(330, 330),
                                                       new BlipPosition(330, 340),
                                                       new BlipPosition(330, 350),
                                                       new BlipPosition(360, 275),
                                                       new BlipPosition(360, 285),
                                                       new BlipPosition(360, 295),
                                                       new BlipPosition(360, 305),
                                                       new BlipPosition(360, 315),
                                                       new BlipPosition(360, 325),
                                                       new BlipPosition(360, 335),
                                                       new BlipPosition(360, 345),
                                                       new BlipPosition(360, 355)
                                                   })
                                       };
            this.postitionData.Add(Quadrant.BottomRight, influenceCircles);
        }

        private void InitializeBottomLeftPosition()
        {
            var influenceCircles = new List<InfluenceCircle>
                                       {
                                           new InfluenceCircle(
                                               Circle.Master,
                                               new List<BlipPosition>
                                                   {
                                                       new BlipPosition(20, 230),
                                                       new BlipPosition(40, 200),
                                                       new BlipPosition(40, 230),
                                                       new BlipPosition(45, 255),
                                                       new BlipPosition(60, 190),
                                                       new BlipPosition(60, 210),
                                                       new BlipPosition(60, 230),
                                                       new BlipPosition(65, 250),
                                                       new BlipPosition(80, 260),
                                                       new BlipPosition(80, 190),
                                                       new BlipPosition(80, 205),
                                                       new BlipPosition(80, 220),
                                                       new BlipPosition(80, 235),
                                                   }),
                                           new InfluenceCircle(
                                               Circle.Experiment,
                                               new List<BlipPosition>
                                                   {
                                                       new BlipPosition(130, 190),
                                                       new BlipPosition(130, 200),
                                                       new BlipPosition(130, 210),
                                                       new BlipPosition(130, 220),
                                                       new BlipPosition(130, 230),
                                                       new BlipPosition(130, 240),
                                                       new BlipPosition(130, 250),
                                                       new BlipPosition(130, 260),
                                                       new BlipPosition(165, 185),
                                                       new BlipPosition(165, 195),
                                                       new BlipPosition(165, 205),
                                                       new BlipPosition(165, 215),
                                                       new BlipPosition(165, 225),
                                                       new BlipPosition(165, 235),
                                                       new BlipPosition(165, 245),
                                                       new BlipPosition(165, 255),
                                                       new BlipPosition(165, 265),
                                                   }),
                                           new InfluenceCircle(
                                               Circle.Observe,
                                               new List<BlipPosition>
                                                   {
                                                       new BlipPosition(230, 190),
                                                       new BlipPosition(230, 200),
                                                       new BlipPosition(230, 210),
                                                       new BlipPosition(230, 220),
                                                       new BlipPosition(230, 230),
                                                       new BlipPosition(230, 240),
                                                       new BlipPosition(230, 250),
                                                       new BlipPosition(230, 260),
                                                       new BlipPosition(260, 185),
                                                       new BlipPosition(260, 195),
                                                       new BlipPosition(260, 205),
                                                       new BlipPosition(260, 215),
                                                       new BlipPosition(260, 225),
                                                       new BlipPosition(260, 235),
                                                       new BlipPosition(260, 245),
                                                       new BlipPosition(260, 255),
                                                       new BlipPosition(260, 265),
                                                   }),
                                           new InfluenceCircle(
                                               Circle.DoNotUse,
                                               new List<BlipPosition>
                                                   {
                                                       new BlipPosition(330, 190),
                                                       new BlipPosition(330, 200),
                                                       new BlipPosition(330, 210),
                                                       new BlipPosition(330, 220),
                                                       new BlipPosition(330, 230),
                                                       new BlipPosition(330, 240),
                                                       new BlipPosition(330, 250),
                                                       new BlipPosition(330, 260),
                                                       new BlipPosition(360, 185),
                                                       new BlipPosition(360, 195),
                                                       new BlipPosition(360, 205),
                                                       new BlipPosition(360, 215),
                                                       new BlipPosition(360, 225),
                                                       new BlipPosition(360, 235),
                                                       new BlipPosition(360, 245),
                                                       new BlipPosition(360, 255),
                                                       new BlipPosition(360, 265)
                                                   })
                                       };
            this.postitionData.Add(Quadrant.BottomLeft, influenceCircles);
        }

        private void InitializeTopRightPosition()
        {
            var influenceCircles = new List<InfluenceCircle>
                                       {
                                           new InfluenceCircle(
                                               Circle.Master,
                                               new List<BlipPosition>
                                                   {
                                                       new BlipPosition(20, 50),
                                                       new BlipPosition(40, 20),
                                                       new BlipPosition(40, 50),
                                                       new BlipPosition(45, 75),
                                                       new BlipPosition(60, 10),
                                                       new BlipPosition(60, 30),
                                                       new BlipPosition(60, 50),
                                                       new BlipPosition(65, 70),
                                                       new BlipPosition(80, 80),
                                                       new BlipPosition(80, 10),
                                                       new BlipPosition(80, 25),
                                                       new BlipPosition(80, 40),
                                                       new BlipPosition(80, 55),
                                                   }),
                                           new InfluenceCircle(
                                               Circle.Experiment,
                                               new List<BlipPosition>
                                                   {
                                                       new BlipPosition(130, 10),
                                                       new BlipPosition(130, 20),
                                                       new BlipPosition(130, 30),
                                                       new BlipPosition(130, 40),
                                                       new BlipPosition(130, 50),
                                                       new BlipPosition(130, 60),
                                                       new BlipPosition(130, 70),
                                                       new BlipPosition(130, 80),
                                                       new BlipPosition(165, 5),
                                                       new BlipPosition(165, 15),
                                                       new BlipPosition(165, 25),
                                                       new BlipPosition(165, 35),
                                                       new BlipPosition(165, 45),
                                                       new BlipPosition(165, 55),
                                                       new BlipPosition(165, 65),
                                                       new BlipPosition(165, 75),
                                                       new BlipPosition(165, 85),
                                                   }),
                                           new InfluenceCircle(
                                               Circle.Observe,
                                               new List<BlipPosition>
                                                   {
                                                       new BlipPosition(230, 10),
                                                       new BlipPosition(230, 20),
                                                       new BlipPosition(230, 30),
                                                       new BlipPosition(230, 40),
                                                       new BlipPosition(230, 50),
                                                       new BlipPosition(230, 60),
                                                       new BlipPosition(230, 70),
                                                       new BlipPosition(230, 80),
                                                       new BlipPosition(260, 5),
                                                       new BlipPosition(260, 15),
                                                       new BlipPosition(260, 25),
                                                       new BlipPosition(260, 35),
                                                       new BlipPosition(260, 45),
                                                       new BlipPosition(260, 55),
                                                       new BlipPosition(260, 65),
                                                       new BlipPosition(260, 75),
                                                       new BlipPosition(260, 85),
                                                   }),
                                           new InfluenceCircle(
                                               Circle.DoNotUse,
                                               new List<BlipPosition>
                                                   {
                                                       new BlipPosition(330, 10),
                                                       new BlipPosition(330, 20),
                                                       new BlipPosition(330, 30),
                                                       new BlipPosition(330, 40),
                                                       new BlipPosition(330, 50),
                                                       new BlipPosition(330, 60),
                                                       new BlipPosition(330, 70),
                                                       new BlipPosition(330, 80),
                                                       new BlipPosition(360, 5),
                                                       new BlipPosition(360, 15),
                                                       new BlipPosition(360, 25),
                                                       new BlipPosition(360, 35),
                                                       new BlipPosition(360, 45),
                                                       new BlipPosition(360, 55),
                                                       new BlipPosition(360, 65),
                                                       new BlipPosition(360, 75),
                                                       new BlipPosition(360, 85)
                                                   })
                                       };
            this.postitionData.Add(Quadrant.TopRight, influenceCircles);
        }

        private void InitializeTopLeftPosition()
        {
            var influenceCircles = new List<InfluenceCircle>
                                       {
                                           new InfluenceCircle(
                                               Circle.Master,
                                               new List<BlipPosition>
                                                   {
                                                       new BlipPosition(20, 130),
                                                       new BlipPosition(40, 110),
                                                       new BlipPosition(40, 140),
                                                       new BlipPosition(45, 165),
                                                       new BlipPosition(60, 105),
                                                       new BlipPosition(60, 130),
                                                       new BlipPosition(60, 150),
                                                       new BlipPosition(65, 170),
                                                       new BlipPosition(80, 100),
                                                       new BlipPosition(80, 115),
                                                       new BlipPosition(80, 130),
                                                       new BlipPosition(80, 145),
                                                       new BlipPosition(80, 160)
                                                   }),
                                           new InfluenceCircle(
                                               Circle.Experiment,
                                               new List<BlipPosition>
                                                   {
                                                       new BlipPosition(130, 100),
                                                       new BlipPosition(130, 110),
                                                       new BlipPosition(130, 120),
                                                       new BlipPosition(130, 130),
                                                       new BlipPosition(130, 140),
                                                       new BlipPosition(130, 150),
                                                       new BlipPosition(130, 160),
                                                       new BlipPosition(130, 170),
                                                       new BlipPosition(165, 95),
                                                       new BlipPosition(165, 105),
                                                       new BlipPosition(165, 115),
                                                       new BlipPosition(165, 125),
                                                       new BlipPosition(165, 135),
                                                       new BlipPosition(165, 145),
                                                       new BlipPosition(165, 155),
                                                       new BlipPosition(165, 165),
                                                       new BlipPosition(165, 175)
                                                   }),
                                           new InfluenceCircle(
                                               Circle.Observe,
                                               new List<BlipPosition>
                                                   {
                                                       new BlipPosition(230, 100),
                                                       new BlipPosition(230, 110),
                                                       new BlipPosition(230, 120),
                                                       new BlipPosition(230, 130),
                                                       new BlipPosition(230, 140),
                                                       new BlipPosition(230, 150),
                                                       new BlipPosition(230, 160),
                                                       new BlipPosition(230, 170),
                                                       new BlipPosition(260, 95),
                                                       new BlipPosition(260, 105),
                                                       new BlipPosition(260, 115),
                                                       new BlipPosition(260, 125),
                                                       new BlipPosition(260, 135),
                                                       new BlipPosition(260, 145),
                                                       new BlipPosition(260, 155),
                                                       new BlipPosition(260, 165),
                                                       new BlipPosition(260, 175),
                                                   }),
                                           new InfluenceCircle(
                                               Circle.DoNotUse,
                                               new List<BlipPosition>
                                                   {
                                                       new BlipPosition(330, 110),
                                                       new BlipPosition(330, 120),
                                                       new BlipPosition(330, 130),
                                                       new BlipPosition(330, 140),
                                                       new BlipPosition(330, 150),
                                                       new BlipPosition(330, 160),
                                                       new BlipPosition(330, 170),
                                                       new BlipPosition(360, 95),
                                                       new BlipPosition(360, 105),
                                                       new BlipPosition(360, 115),
                                                       new BlipPosition(360, 125),
                                                       new BlipPosition(360, 135),
                                                       new BlipPosition(360, 145),
                                                       new BlipPosition(360, 155),
                                                       new BlipPosition(360, 165),
                                                       new BlipPosition(360, 175)
                                                   })
                                       };

            this.postitionData.Add(Quadrant.TopLeft, influenceCircles);
        }
    }
}