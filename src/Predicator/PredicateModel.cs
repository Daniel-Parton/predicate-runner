using System;
using System.Collections.Generic;

namespace Predicator
{
    public class PredicateModel<T> where T : struct, IConvertible
    {
        public string Id { get; set; }
        public List<Predicate<T>> Predicates { get; set; } = new List<Predicate<T>>();
        public PredicateJoiner After { get; set; } = PredicateJoiner.And;
    }
}
