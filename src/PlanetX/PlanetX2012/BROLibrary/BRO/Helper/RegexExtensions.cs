using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PlanetX.BRO
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

        public static IEnumerable<string> GetHashTags(string value)
        {
            string regex = @"(?:(?<=\s)|^)#(\w*[A-Za-z_]+\w*)";

            return Regex.Matches(value, regex, RegexOptions.IgnoreCase)
                .OfType<Match>()
                .Select(m => m.Groups[0].Value);

        }

    }
}