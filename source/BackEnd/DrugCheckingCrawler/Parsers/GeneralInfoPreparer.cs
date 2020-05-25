using System.Drawing.Printing;
using System.Linq;

namespace DrugCheckingCrawler.Parsers
{
    public static class GeneralInfoPreparer
    {
        public static string Prepare(string toPrepare)
        {
            return toPrepare
                .InsertAt("Name")
                .InsertAt(true, "Gewicht")
                .InsertAt(true, "Durchmesser")
                .InsertAt("Dicke")
                .InsertAt("Bruchrille")
                .InsertAt("Farbe")
                .InsertAt("Inhaltsstoffe", "Inhaltsstoff")
                .InsertAt("Getestet in");
        }

        private static string InsertAt(this string toInsert, params string[] positions)
        {
            return toInsert.InsertAt(false, positions);
        }

        private static string InsertAt(this string toInsert, bool isOptional, params string[] positions)
        {
            var indicies = positions.Select(x => (Index: toInsert.IndexOf(x), Position: x));

            if (indicies.All(x => x.Index < 0))
            {
                if (isOptional)
                    return toInsert;

                throw new GeneralInfoPreparerException(string.Join(" or ", positions));
            }

            var (Index, Position) = indicies.First(x => x.Index >= 0);

            return toInsert.Insert(Index + Position.Length, "\t");
        }
    }
}
