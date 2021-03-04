using System;
using System.Collections.Generic;

namespace Predicator
{
    public class PredicateGroup<T> where T : struct, IConvertible
    {
        public string Id { get; set; }
        public List<PredicateModel<T>> Models { get; set; } = new List<PredicateModel<T>>();
        public PredicateJoiner After { get; set; } = PredicateJoiner.And;
    }
}
