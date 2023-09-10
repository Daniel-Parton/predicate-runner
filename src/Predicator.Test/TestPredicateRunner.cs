using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Predicator.Test
{
    public enum TestPredicateType
    {
        TextEquals,
        TextDoesNotEqual,
        GreaterThan1
    }

    public class TestPredicateContext
    {
        public string Text { get; set; }
        public decimal Number { get; set; }
    }

    public class TestPredicateRunner : PredicateRunner<TestPredicateContext>
    {
        public TestPredicateRunner(PredicateFilter<Predicate> filter) : base(filter) { }

        public TestPredicateRunner(
            IEnumerable<PredicateModel<Predicate>> models,
            PredicateJoiner joiner = PredicateJoiner.And) : base(models, joiner)
        {
        }

        public TestPredicateRunner(
            IEnumerable<Predicate> predicates,
            PredicateJoiner joiner = PredicateJoiner.And) : base(predicates, joiner)
        {
        }

        protected override Task<bool> EvaluateAsync(Predicate predicate, TestPredicateContext data)
        {
            if (!Enum.TryParse<TestPredicateType>(predicate.Type, out var type))
            {
                return Task.FromResult(false);
            }

            switch (type)
            {
                case TestPredicateType.TextEquals:
                    return Task.FromResult( data.Text.Equals(predicate.Options.StringValue1));
                case TestPredicateType.TextDoesNotEqual:
                    return Task.FromResult(!data.Text.Equals(predicate.Options.StringValue1));
                case TestPredicateType.GreaterThan1:
                    return Task.FromResult(data.Number > 1);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
