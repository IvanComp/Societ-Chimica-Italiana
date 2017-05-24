using System;
using System.Linq;

namespace sci.Models
{
    public delegate IQueryable<TItem> QueryMutator<TItem, TSearch>(IQueryable<TItem> items, TSearch search);
    public class SearchFieldMutator<TItem, TSearch>
    {
        public Predicate<TSearch> Condition { get; set; }
        public QueryMutator<TItem, TSearch> Mutator { get; set; }

        public SearchFieldMutator(Predicate<TSearch> condition, QueryMutator<TItem, TSearch> mutator)
        {
            Condition = condition;
            Mutator = mutator;
        }

        public IQueryable<TItem> Apply(TSearch search, IQueryable<TItem> query)
        {
            return Condition(search) ? Mutator(query, search) : query;
        }
    }
}