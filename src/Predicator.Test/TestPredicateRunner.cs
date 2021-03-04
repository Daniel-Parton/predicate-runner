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

    public class TestPredicateRunnerData
    {
        public string Text { get; set; }
        public decimal Number { get; set; }
    }

    public class TestPredicateRunner : PredicateRunner<TestPredicateType, TestPredicateRunnerData>
    {
        public TestPredicateRunner(IEnumerable<PredicateGroup<TestPredicateType>> groups) : base(groups) { }

        public TestPredicateRunner(IEnumerable<PredicateModel<TestPredicateType>> models) : base(models)
        {
        }

        public TestPredicateRunner(IEnumerable<Predicate<TestPredicateType>> predicates) : base(predicates)
        {
        }

        protected override Task<bool> EvaluateAsync(Predicate<TestPredicateType> predicate, TestPredicateRunnerData data)
        {
            var passes = false;
            switch (predicate.Type)
            {
                case TestPredicateType.TextEquals:
                    passes = data.Text.Equals(predicate.Options.StringValue1);
                    break;
                case TestPredicateType.TextDoesNotEqual:
                    passes = !data.Text.Equals(predicate.Options.StringValue1);
                    break;
                case TestPredicateType.GreaterThan1:
                    passes = data.Number > 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return Task.FromResult(passes);
        }
    }
}
