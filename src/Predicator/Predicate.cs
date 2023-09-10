
using System;

namespace Predicator
{
    public class Predicate
    {
        public Predicate()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string Type { get; set; }
        public string Operator { get; set; }
        public PredicateOptions Options { get; set; } = new PredicateOptions();
    }
}
