using System;

namespace Predicator
{
    public class Predicate<T> where T : struct, IConvertible
    {
        public T Type { get; set; }
        public PredicateOptions Options { get; set; }
    }
}
