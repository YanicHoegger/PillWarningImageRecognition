using System;
using System.Drawing;
using System.Linq;
using DatabaseInteraction.Interface;

namespace DatabaseInteraction.Repository
{
    public class DrugCheckingSourceQueries
    {
        public Func<IQueryable<DrugCheckingSource>, IQueryable<DrugCheckingSource>> GetColorQuery(Color color, int take)
        {
            IQueryable<DrugCheckingSource> Query(IQueryable<DrugCheckingSource> queryable)
            {
                return queryable.Where(x => x.Color == color)
                    .Take(take);
            }

            return Query;
        }

        public Func<IQueryable<DrugCheckingSource>, IQueryable<DrugCheckingSource>> GetTagNameQuery(string tagName)
        {
            IQueryable<DrugCheckingSource> Query(IQueryable<DrugCheckingSource> queryable)
            {
                return queryable.Where(x => x.Name.Equals(tagName));
            }

            return Query;
        }

        public Func<IQueryable<DrugCheckingSource>, IQueryable<DrugCheckingSource>> GetSameItemQuery(
            DrugCheckingSource toSearch)
        {
            IQueryable<DrugCheckingSource> Query(IQueryable<DrugCheckingSource> queryable)
            {
                return queryable.Where(x => x.PdfLocation.Equals(toSearch.PdfLocation));
            }

            return Query;
        }
    }
}
