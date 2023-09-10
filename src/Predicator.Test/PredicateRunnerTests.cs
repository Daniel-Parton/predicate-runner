using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Predicator.Test
{
    public class PredicatorTests
    {
        [Fact]
        [Trait("Category", "Unit")]
        public async Task EvaluateAsync_1Model_WithAndJoiner_1PredicateCorrect_ReturnsTrue()
        {
            //Arrange
            var data = new TestPredicateContext { Text = "aaa" };
            var predicate = new Predicate
            {
                Type = TestPredicateType.TextEquals.ToString(),
                Options = new PredicateOptions { StringValue1 = data.Text }
            };

            //Act
            var result = await new TestPredicateRunner(new[] { predicate }).EvaluateAsync(data);

            //Assert
            Assert.True(result);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task EvaluateAsync_1Model_WithAndJoiner_2PredicatesCorrect_ReturnsTrue()
        {
            //Arrange
            var data = new TestPredicateContext { Text = "aaa", Number = 2 };
            var correct1 = new Predicate
            {
                Type = TestPredicateType.TextEquals.ToString(),
                Options = new PredicateOptions { StringValue1 = data.Text }
            };

            var correct2 = new Predicate
            {
                Type = TestPredicateType.GreaterThan1.ToString(),
            };

            //Act
            var result = await new TestPredicateRunner(new[] { correct1, correct2 }).EvaluateAsync(data);

            //Assert
            Assert.True(result);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task EvaluateAsync_1Model__WithAndJoiner_2Predicates1Wrong1Correct_ReturnsFalse()
        {
            //Arrange
            var data = new TestPredicateContext { Text = "aaa" };

            var correctPredicate = new Predicate
            {
                Type = TestPredicateType.TextEquals.ToString(),
                Options = new PredicateOptions { StringValue1 = data.Text }
            };

            var inCorrectPredicate = new Predicate
            {
                Type = TestPredicateType.TextEquals.ToString(),
                Options = new PredicateOptions { StringValue1 = "bbb" }
            };

            //Act
            var result = await new TestPredicateRunner(new[] { correctPredicate, inCorrectPredicate }).EvaluateAsync(data);

            //Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task EvaluateAsync_1Model_WithAndJoiner_2Predicates2Wrong_ReturnsFalse()
        {
            //Arrange
            var data = new TestPredicateContext { Text = "aaa" };

            var incorrect1 = new Predicate
            {
                Type = TestPredicateType.TextEquals.ToString(),
                Options = new PredicateOptions { StringValue1 = "ccc" }
            };

            var incorrect2 = new Predicate
            {
                Type = TestPredicateType.TextEquals.ToString(),
                Options = new PredicateOptions { StringValue1 = "bbb" }
            };

            //Act
            var result = await new TestPredicateRunner(new[] { incorrect1, incorrect2 })
                .EvaluateAsync(data);

            //Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task EvaluateAsync_1Model_WithOrJoiner_2Predicates2Wrong_ReturnsFalse()
        {
            //Arrange
            var data = new TestPredicateContext { Text = "aaa" };

            var incorrect1 = new Predicate
            {
                Type = TestPredicateType.TextEquals.ToString(),
                Options = new PredicateOptions { StringValue1 = "ccc" }
            };

            var incorrect2 = new Predicate
            {
                Type = TestPredicateType.TextEquals.ToString(),
                Options = new PredicateOptions { StringValue1 = "bbb" }
            };

            //Act
            var result = await new TestPredicateRunner(new[]
            {
                incorrect1, incorrect2
            }, PredicateJoiner.Or).EvaluateAsync(data);

            //Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task EvaluateAsync_1Model_WithOrJoiner_2Predicates1Wrong1Right_ReturnsTrue()
        {
            //Arrange
            var data = new TestPredicateContext { Text = "aaa" };

            var correct = new Predicate
            {
                Type = TestPredicateType.TextEquals.ToString(),
                Options = new PredicateOptions { StringValue1 = data.Text }
            };

            var incorrect = new Predicate
            {
                Type = TestPredicateType.TextEquals.ToString(),
                Options = new PredicateOptions { StringValue1 = "bbb" }
            };

            //Act
            var result = await new TestPredicateRunner(new[]
            {
                correct, incorrect
            }, PredicateJoiner.Or).EvaluateAsync(data);

            //Assert
            Assert.True(result);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task EvaluateAsync_WithFilterAndJoiner_2ModelsCorrect_ReturnsTrue()
        {
            //Arrange
            var data = new TestPredicateContext { Text = "aaa", Number = 2 };

            var correct = new Predicate
            {
                Type = TestPredicateType.TextEquals.ToString(),
                Options = new PredicateOptions { StringValue1 = data.Text }
            };

            var correct2 = new Predicate
            {
                Type = TestPredicateType.GreaterThan1.ToString()
            };

            //Act
            var result = await new TestPredicateRunner(new[]
            {
                ResolveModel(new[] { correct }),
                ResolveModel(new[] { correct2 }),
            }).EvaluateAsync(data);

            //Assert
            Assert.True(result);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task EvaluateAsync_WithFilterAndJoiner_2Models1Correct1InCorrect_ReturnsFalse()
        {
            //Arrange
            var data = new TestPredicateContext { Text = "aaa", Number = 0 };

            var correct = new Predicate
            {
                Type = TestPredicateType.TextEquals.ToString(),
                Options = new PredicateOptions { StringValue1 = data.Text }
            };

            var InCorrect = new Predicate
            {
                Type = TestPredicateType.GreaterThan1.ToString()
            };


            //Act
            var result = await new TestPredicateRunner(new[]
            {
                ResolveModel(new[] { correct}),
                ResolveModel(new[] { InCorrect}),
            }).EvaluateAsync(data);

            //Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task EvaluateAsync_WithFilterOrJoiner_2Models1Correct1InCorrect_ReturnsTrue()
        {
            //Arrange
            var data = new TestPredicateContext { Text = "aaa", Number = 0 };

            var correct = new Predicate
            {
                Type = TestPredicateType.TextEquals.ToString(),
                Options = new PredicateOptions { StringValue1 = data.Text }
            };

            var incorrect = new Predicate
            {
                Type = TestPredicateType.GreaterThan1.ToString()
            };

            //Act
            var result = await new TestPredicateRunner(new[]
            {
                ResolveModel(new[] { correct}),
                ResolveModel(new[] { incorrect}),
            }, PredicateJoiner.Or).EvaluateAsync(data);

            //Assert
            Assert.True(result);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task EvaluateAsync_WithFilterOrJoiner_3Models1Correct2InCorrect_ReturnsTrue()
        {
            //Arrange
            var data = new TestPredicateContext { Text = "aaa", Number = 0 };

            var incorrect = new Predicate
            {
                Type = TestPredicateType.GreaterThan1.ToString()
            };

            var incorrect2 = new Predicate
            {
                Type = TestPredicateType.TextEquals.ToString(),
                Options = new PredicateOptions { StringValue1 = "bbb" }
            };

            var correct = new Predicate
            {
                Type = TestPredicateType.TextEquals.ToString(),
                Options = new PredicateOptions { StringValue1 = data.Text }
            };

            //Act
            var result = await new TestPredicateRunner(new[]
            {
                ResolveModel(new[] { incorrect }),
                ResolveModel(new[] { incorrect2 }),
                ResolveModel(new[] { correct }),
            }, PredicateJoiner.Or).EvaluateAsync(data);

            //Assert
            Assert.True(result);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task EvaluateAsync_WithFilterOrJoiner_3ModelsAllInCorrect_ReturnsTrue()
        {
            //Arrange
            var data = new TestPredicateContext { Text = "aaa", Number = 0 };

            var incorrect1 = new Predicate
            {
                Type = TestPredicateType.GreaterThan1.ToString()
            };

            var incorrect2 = new Predicate
            {
                Type = TestPredicateType.TextEquals.ToString(),
                Options = new PredicateOptions { StringValue1 = "bbb" }
            };

            var incorrect3 = new Predicate
            {
                Type = TestPredicateType.TextEquals.ToString(),
                Options = new PredicateOptions { StringValue1 = "ccc" }
            };

            //Act
            var result = await new TestPredicateRunner(new[]
            {
                ResolveModel(new[] { incorrect1 }),
                ResolveModel(new[] { incorrect2 }),
                ResolveModel(new[] { incorrect3 }),
            }, PredicateJoiner.Or).EvaluateAsync(data);

            //Assert
            Assert.False(result);
        }

        PredicateFilter<Predicate> ResolveFilter(IEnumerable<PredicateModel<Predicate>> models, PredicateJoiner joiner = PredicateJoiner.And)
        {
            return new PredicateFilter<Predicate>
            {
                Joiner = joiner,
                Models = new List<PredicateModel<Predicate>>(models)
            };
        }

        private PredicateModel<Predicate> ResolveModel(IEnumerable<Predicate> predicates, PredicateJoiner joiner = PredicateJoiner.And)
        {
            return new PredicateModel<Predicate>
            {
                Joiner = joiner,
                Predicates = new List<Predicate>(predicates)
            };
        }
    }
}
