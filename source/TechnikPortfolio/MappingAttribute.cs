namespace TechnikPortfolio
{
    using System;

    [AttributeUsage(AttributeTargets.All)]
    public class MappingAttribute : Attribute
    {
        public MappingAttribute(string value, string shortcut)
        {
            this.Value = value;
            this.Shortcut = shortcut;
        }

        public string Value { get; }

        public string Shortcut { get; }
    }
}