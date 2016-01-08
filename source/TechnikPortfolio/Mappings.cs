namespace TechnikPortfolio
{
    using System;
    using System.Linq;
    using System.Reflection;

    public static class Mappings
    {
        public static string Of<T>()
        {
            var mappings = typeof(T)
                .GetMembers()
                .Select(GetMapping)
                .Where(a => a != null)
                .Select(a => $"{a.Shortcut} = {a.Value}");

            return string.Join(", ", mappings);
        }

        public static MappingAttribute GetMapping(this MemberInfo member)
        {
            return member.GetCustomAttribute<MappingAttribute>();
        }

        public static T ParseShortcutAs<T>(this string shortcut)
        {
            try
            {
                var memberInfo = typeof(T)
                    .GetMembers()
                    .Select(m => new { Member = m, Attribute = ((MappingAttribute)m.GetCustomAttribute(typeof(MappingAttribute))) })
                    .Where(m => m.Attribute != null)
                    .Single(l => l.Attribute.Shortcut == shortcut).Member;

                return (T)Enum.Parse(typeof(T), memberInfo.Name);
            }
            catch
            {
                throw new Exception($"could not parse shortcut {shortcut} for {typeof(T).Name}");
            }
        }

        public static T ParseValueAs<T>(this string value)
        {
            var memberInfo = typeof(T)
                .GetMembers()
                .Select(m => new { Member = m, Attribute = ((MappingAttribute)m.GetCustomAttribute(typeof(MappingAttribute))) })
                .Where(m => m.Attribute != null)
                .Single(l => l.Attribute.Value == value).Member;

            return (T)Enum.Parse(typeof(T), memberInfo.Name);
        }
    }
}