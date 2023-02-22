using System.IO;
using System.Text;

namespace Utility.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveOccurrences(this string text, string occurrence)
        {
            var sb = new StringBuilder();
            foreach (string line in new LineReader(() => new StringReader(text)))
            {
                if (!line.Contains(occurrence))
                    sb.AppendLine(line);
            }

            return sb.ToString();
        }
    }
}
