using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PlanetX.Infrastructure
{
    public static class RegexExtensions
    {
        public static IEnumerable<string> FindMatches(this Regex regex, string value)
        {
            return regex.Match(value)
                .Groups
                .Cast<Group>()
                .Skip(1)
                .Select(g => g.Value);
        }
    }
}