using System;
using System.Collections.Generic;
using System.Linq;

namespace Predicator
{
    public class PredicateModel<T> where T : Predicate
    {
        public PredicateModel()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public List<T> Predicates { get; set; } = new List<T>();
        public PredicateJoiner Joiner { get; set; } = PredicateJoiner.And;
        public bool CanRun => Predicates != null && Predicates.Any();
        
    }
}
