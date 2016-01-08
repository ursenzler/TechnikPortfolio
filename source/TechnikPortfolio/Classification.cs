namespace TechnikPortfolio
{
    public enum Classification
    {
        [Mapping(YouTrack.NoClassification, "-")]
        None,
        [Mapping("Beobachten", "O")]
        Beobachten,
        [Mapping("Experimentieren", "E")]
        Experimentieren,
        [Mapping("Beherrschen", "H")]
        Beherrschen,
        [Mapping("nicht verwenden", "N")]
        NichtVerwenden
    }
}