namespace TechnikPortfolio
{
    public enum Level
    {
        [Mapping("Keyword", "K")]
        Keyword,
        [Mapping("Dienstleistung", "D")]
        Dienstleistung,
        [Mapping("Kompetenz", "C")]
        Kompetenz,
        [Mapping("Technologie oder Methode", "M")]
        TechnologieMethode,
        [Mapping("Tool", "T")]
        Tool,
        [Mapping(YouTrack.NoLevel, "-")]
        NoLevel
    }
}