namespace TechnikPortfolio.TechRadar.Formatting
{
    public class JsBlip
    {
        public JsBlip(string name, int radius, int angle)
        {
            this.name = name;
            this.pc = new JsBlipCoordinate(radius, angle);
        }

        public string name { get; private set; }

        public JsBlipCoordinate pc { get; private set; }

        public string movement { get; private set; }
    }

    public class JsBlipCoordinate
    {
        public JsBlipCoordinate(int radius, int angle)
        {
            this.r = radius;
            this.t = angle;
        }

        public int r { get; private set; }

        public int t { get; private set; }
    }
}