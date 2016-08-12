namespace TechnikPortfolio.TechRadar
{
    public class Blip
    {
        private readonly string name;
        private readonly int radius;
        private readonly int angle;

        public Blip(string name, int radius, int angle)
        {
            this.name = name;
            this.radius = radius;
            this.angle = angle;
        }

        public string Name => this.name;

        public int Radius => this.radius;

        public int Angle => this.angle;
    }
}