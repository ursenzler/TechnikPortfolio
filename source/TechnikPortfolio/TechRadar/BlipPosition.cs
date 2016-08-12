namespace TechnikPortfolio.TechRadar
{
    public class BlipPosition
    {
        private readonly int radius;
        private readonly int angle;

        public BlipPosition(int radius, int angle)
        {
            this.radius = radius;
            this.angle = angle;
            this.IsUsed = false;
        }

        public bool IsUsed { get; set; }

        public int Radius => this.radius;

        public int Angle => this.angle;
    }
}