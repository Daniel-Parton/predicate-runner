using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Predicator.Test
{
    public class PredicateRunnerTests
    {
        [Fact]
        public async Task EvaluateAsync_1Model1PredicateCorrect_ReturnsTrue()
        {
            //Arrange
            var data = new TestPredicateRunnerData {Text = "aaa"};
            var predicate = new Predicate<TestPredicateType>
            {
                Type = TestPredicateType.TextEquals, Options = new PredicateOptions {StringValue1 = data.Text}
            };

            //Act
            var success = await new TestPredicateRunner(new []{ predicate } ).EvaluateAsync(data);

            //Assert
            Assert.True(success);
        }

        [Fact]
        public async Task EvaluateAsync_1Model_2PredicatesCorrect_ReturnsTrue()
        {
            //Arrange
            var data = new TestPredicateRunnerData { Text = "aaa", Number = 2 };
            var correct1 = new Predicate<TestPredicateType>
            {
                Type = TestPredicateType.TextEquals,
                Options = new PredicateOptions { StringValue1 = data.Text }
            };

            var correct2 = new Predicate<TestPredicateType>
            {
                Type = TestPredicateType.GreaterThan1,
            };

            //Act
            var success = await new TestPredicateRunner(new []{ correct1, correct2 }).EvaluateAsync(data);

            //Assert
            Assert.True(success);
        }

        [Fact]
        public async Task EvaluateAsync_1Model_2Predicates1Wrong1Correct_ReturnsFalse()
        {
            //Arrange
            var data = new TestPredicateRunnerData {Text = "aaa"};

            var correctPredicate = new Predicate<TestPredicateType>
            {
                Type = TestPredicateType.TextEquals, Options = new PredicateOptions {StringValue1 = data.Text}
            };

            var inCorrectPredicate = new Predicate<TestPredicateType>
            {
                Type = TestPredicateType.TextEquals,
                Options = new PredicateOptions { StringValue1 = "bbb" }
            };


            //Act
            var success = await new TestPredicateRunner(new [] { correctPredicate, inCorrectPredicate }).EvaluateAsync(data);

            //Assert
            Assert.False(success);
        }

        [Fact]
        public async Task EvaluateAsync_2ModelsCorrect_ReturnsTrue()
        {
            //Arrange
            var data = new TestPredicateRunnerData { Text = "aaa", Number = 2 };

            var correct = new Predicate<TestPredicateType>
            {
                Type = TestPredicateType.TextEquals,
                Options = new PredicateOptions {  StringValue1 = data.Text }
            }; 

            var correct2 = new Predicate<TestPredicateType>
            {
                Type = TestPredicateType.GreaterThan1
            };


            //Act
            var success = await new TestPredicateRunner(new []
            {
                GetModel(new[] { correct}),
                GetModel(new[] { correct2}),
            }).EvaluateAsync(data);

            //Assert
            Assert.True(success);
        }

        [Fact]
        public async Task EvaluateAsync_2Models1Correct1InCorrect_AndJoiner_ReturnsFalse()
        {
            //Arrange
            var data = new TestPredicateRunnerData { Text = "aaa", Number = 0 };

            var correct = new Predicate<TestPredicateType>
            {
                Type = TestPredicateType.TextEquals,
                Options = new PredicateOptions { StringValue1 = data.Text }
            };

            var InCorrect = new Predicate<TestPredicateType>
            {
                Type = TestPredicateType.GreaterThan1
            };


            //Act
            var success = await new TestPredicateRunner(new[]
            {
                GetModel(new[] { correct}),
                GetModel(new[] { InCorrect}),
            }).EvaluateAsync(data);

            //Assert
            Assert.False(success);
        }

        [Fact]
        public async Task EvaluateAsync_2Models1Correct1InCorrect_OrJoiner_ReturnsTrue()
        {
            //Arrange
            var data = new TestPredicateRunnerData { Text = "aaa", Number = 0 };

            var correct = new Predicate<TestPredicateType>
            {
                Type = TestPredicateType.TextEquals,
                Options = new PredicateOptions { StringValue1 = data.Text }
            };

            var InCorrect = new Predicate<TestPredicateType>
            {
                Type = TestPredicateType.GreaterThan1
            };

            //Act
            var success = await new TestPredicateRunner(new[]
            {
                GetModel(new[] { correct}, PredicateJoiner.Or),
                GetModel(new[] { InCorrect}),
            }).EvaluateAsync(data);

            //Assert
            Assert.True(success);
        }

        [Fact]
        public async Task EvaluateAsync_2GroupsCorrect_ReturnsTrue()
        {
            //Arrange
            var data = new TestPredicateRunnerData { Text = "aaa", Number = 2 };

            var correct = new Predicate<TestPredicateType>
            {
                Type = TestPredicateType.TextEquals,
                Options = new PredicateOptions { StringValue1 = data.Text }
            };

            var correct2 = new Predicate<TestPredicateType>
            {
                Type = TestPredicateType.GreaterThan1
            };

            //Act
            var success = await new TestPredicateRunner(new[]
            {
                GetGroup(new [] { GetModel(new[] { correct }) }),
                GetGroup(new [] { GetModel(new[] { correct2 }) }),
            }).EvaluateAsync(data);

            //Assert
            Assert.True(success);
        }

        [Fact]
        public async Task EvaluateAsync_2Groups1Correct1InCorrect_AndJoiner_ReturnsFalse()
        {
            //Arrange
            var data = new TestPredicateRunnerData { Text = "aaa", Number = 0 };

            var correct = new Predicate<TestPredicateType>
            {
                Type = TestPredicateType.TextEquals,
                Options = new PredicateOptions { StringValue1 = data.Text }
            };

            var inCorrect = new Predicate<TestPredicateType>
            {
                Type = TestPredicateType.GreaterThan1
            };

            //Act
            var success = await new TestPredicateRunner(new[]
            {
                GetGroup(new [] { GetModel(new[] { correct }) }),
                GetGroup(new [] { GetModel(new[] { inCorrect }) }),
            }).EvaluateAsync(data);

            //Assert
            Assert.False(success);
        }

        [Fact]
        public async Task EvaluateAsync_2Groups1Correct1InCorrect_OrJoiner_ReturnsTrue()
        {
            //Arrange
            var data = new TestPredicateRunnerData { Text = "aaa", Number = 0 };

            var correct = new Predicate<TestPredicateType>
            {
                Type = TestPredicateType.TextEquals,
                Options = new PredicateOptions { StringValue1 = data.Text }
            };

            var inCorrect = new Predicate<TestPredicateType>
            {
                Type = TestPredicateType.GreaterThan1
            };

            //Act
            var success = await new TestPredicateRunner(new[]
            {
                GetGroup(new [] { GetModel(new[] { correct }) }, PredicateJoiner.Or),
                GetGroup(new [] { GetModel(new[] { inCorrect }) }),
            }).EvaluateAsync(data);

            //Assert
            Assert.True(success);
        }

        private PredicateGroup<TestPredicateType> GetGroup(IEnumerable<PredicateModel<TestPredicateType>> models, PredicateJoiner joiner = PredicateJoiner.And)
        {
            return new PredicateGroup<TestPredicateType>
            {
                After = joiner,
                Models = new List<PredicateModel<TestPredicateType>>(models)
            };
        }

        private PredicateModel<TestPredicateType> GetModel(IEnumerable<Predicate<TestPredicateType>>  predicates, PredicateJoiner joiner = PredicateJoiner.And)
        {
            return new PredicateModel<TestPredicateType>
            {
                After = joiner,
                Predicates = new List<Predicate<TestPredicateType>>(predicates)
            };
        }
    }
}
