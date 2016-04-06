namespace TechnikPortfolio
{
    public enum Level
    {
        [Mapping("Keyword", "K")]
        Keyword,
        [Mapping("Kompetenz", "C")]
        Kompetenz,
        [Mapping("Tool", "T")]
        Tool,
        [Mapping(YouTrack.NoLevel, "-")]
        NoLevel
    }
}