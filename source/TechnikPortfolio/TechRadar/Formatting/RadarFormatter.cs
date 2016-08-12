namespace TechnikPortfolio.TechRadar.Formatting
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Newtonsoft.Json;

    public class RadarFormatter
    {
        public Dictionary<Quadrant, string> quadrantPlaceholder = new Dictionary<Quadrant, string>
        {
            { Quadrant.TopLeft, "{{top_left_data}}" },
            { Quadrant.TopRight, "{{top_right_data}}" },
            { Quadrant.BottomLeft, "{{bottom_left_data}}" },
            { Quadrant.BottomRight, "{{bottom_right_data}}" },
        }; 

        public string FormatDataToJsString(RadarData radarData)
        {
            var template = File.ReadAllText("TechRadar/template/template.js");

            template = template.Replace("{{radar_title}}", radarData.Name);

            foreach (var entry in radarData.Data)
            {
                Quadrant quadrant = entry.Key;
                QuadrantData quadrantData = entry.Value;
                var data = quadrantData.Items.Select(item => new JsBlip(item.Name, item.Radius, item.Angle));
                var jsonData = JsonConvert.SerializeObject(data);
                template = template.Replace(this.quadrantPlaceholder[quadrant], jsonData);
            }

            return template;
        }

        public string FormatHtmlFor(string dataJsFilename)
        {
            var template = File.ReadAllText("TechRadar/template/template.html");
            return template.Replace("{{js_filename}}", dataJsFilename);
        }
    }
}