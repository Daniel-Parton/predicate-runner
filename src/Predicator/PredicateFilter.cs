using System;
using System.Collections.Generic;

namespace Predicator
{
    public class PredicateFilter<T> where T : Predicate
    {
        public string Id { get; set; }

        public PredicateFilter()
        {
            Id = Guid.NewGuid().ToString();
        }

        public List<PredicateModel<T>> Models { get; set; } = new List<PredicateModel<T>>();

        public PredicateJoiner Joiner { get; set; } = PredicateJoiner.And;

        public static PredicateFilter<T> New(T predicate)
        {
            return new PredicateFilter<T>
            {
                Id = Guid.NewGuid().ToString(),
                Models =
                {
                    new PredicateModel<T>
                    {
                        Predicates = { predicate }
                    }
                }
            };
        }

        public static PredicateFilter<T> Empty()
        {
            return new PredicateFilter<T>
            {
                Models =
                {
                    new PredicateModel<T>
                    {
                        Predicates = new List<T>()
                    }
                }
            };
        }
    }
}
