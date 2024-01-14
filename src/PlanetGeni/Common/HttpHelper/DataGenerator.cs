using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class DataGenerator
    {
        public static string LoremIpsum(int minWords, int maxWords,
    int minSentences, int maxSentences,
    int numParagraphs)
        {

            var words = new[]{"lorem", "ipsum", "dolor", "sit", "amet", "consectetuer",
        "adipiscing", "elit", "sed", "diam", "nonummy", "nibh", "euismod",
        "tincidunt", "ut", "laoreet", "dolore", "magna", "aliquam", "erat"};

            var rand = new Random();
            int numSentences = rand.Next(maxSentences - minSentences)
                + minSentences + 1;
            int numWords = rand.Next(maxWords - minWords) + minWords + 1;

            string result = string.Empty;

            for (int p = 0; p < numParagraphs; p++)
            {

                for (int s = 0; s < numSentences; s++)
                {
                    for (int w = 0; w < numWords; w++)
                    {
                        if (w > 0) { result += " "; }
                        result += words[rand.Next(words.Length)];
                    }
                    result += ". ";
                }

            }

            return result;
        }

        public static int[] GetSlots(int slots, int max)
        {
            return new Random().Values(1, max)
                               .Take(slots - 1)
                               .Append(0, max)
                               .OrderBy(i => i)
                               .Pairwise((x, y) => y - x)
                               .ToArray();
        }

        public static IEnumerable<int> Values(this Random random, int minValue, int maxValue)
        {
            while (true)
                yield return random.Next(minValue, maxValue);
        }

        public static IEnumerable<TResult> Pairwise<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TSource, TResult> resultSelector)
        {
            TSource previous = default(TSource);

            using (var it = source.GetEnumerator())
            {
                if (it.MoveNext())
                    previous = it.Current;

                while (it.MoveNext())
                    yield return resultSelector(previous, previous = it.Current);
            }
        }

        public static IEnumerable<T> Append<T>(this IEnumerable<T> source, params T[] args)
        {
            return source.Concat(args);
        }
    }
}
