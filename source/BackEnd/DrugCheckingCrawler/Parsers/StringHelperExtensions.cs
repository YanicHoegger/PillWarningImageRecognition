using System;

namespace DrugCheckingCrawler.Parsers
{
    public static class StringHelperExtensions
    {
        public static string Replace(this string text, string oldValue, string newValue, int position)
        {
            var removed = text.Remove(position, oldValue.Length);
            return removed.Insert(position, newValue);
        }

        public static string RemoveAllBefore(this string text, char toRemove, int position)
        {
            int i;
            for(i = position - 1; i >= 0; i--)
            {
                if(text[i] != toRemove)
                {
                    break;
                }
            }

            return text.Remove(i + 1, position - i - 1);
        }
    }
}
