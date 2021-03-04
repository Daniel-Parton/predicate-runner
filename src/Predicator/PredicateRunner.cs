using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Predicator
{
    public abstract class PredicateRunner<TPredicateType, TEvaluateData> where TPredicateType : struct, IConvertible
    {
        public IEnumerable<PredicateGroup<TPredicateType>> Groups { get; set; }
        public bool IsValid { get; set; }

        protected PredicateRunner(IEnumerable<PredicateGroup<TPredicateType>> groups)
        {
            Groups = groups ?? new List<PredicateGroup<TPredicateType>>();
            IsValid = true;
        }

        protected PredicateRunner(IEnumerable<PredicateModel<TPredicateType>> models)
        {
            Groups = new List<PredicateGroup<TPredicateType>>
            {
                new PredicateGroup<TPredicateType> {Models = new List<PredicateModel<TPredicateType>>(models)}
            };
            IsValid = true;
        }

        protected PredicateRunner(IEnumerable<Predicate<TPredicateType>> predicates)
        {
            Groups = new List<PredicateGroup<TPredicateType>>(new[]
            {
                new PredicateGroup<TPredicateType> {
                    After = PredicateJoiner.And,
                    Models = new List<PredicateModel<TPredicateType>>(new []
                    {
                        new PredicateModel<TPredicateType>
                        {
                            After = PredicateJoiner.And,
                            Predicates = new List<Predicate<TPredicateType>>(predicates)
                        }
                    })},
            });
            IsValid = true;
        }


        public async Task<bool> EvaluateAsync(TEvaluateData data)
        {
            if (!IsValid) return false;

            var evaluatedGroups = new List<(bool, PredicateJoiner)>();

            foreach (var group in Groups)
            {
                var evaluatedModels = new List<(bool, PredicateJoiner)>();
                foreach (var model in group.Models)
                {
                    var modelIsValid = true;
                    foreach (var predicate in model.Predicates)
                    {
                        modelIsValid = await EvaluateAsync(predicate, data);
                        if (!modelIsValid) break;
                    }
                    evaluatedModels.Add((modelIsValid, model.After));
                }

                var groupIsValid = CheckEvaluatedPredicates(evaluatedModels);
                evaluatedGroups.Add((groupIsValid, group.After));
            }

            var allGroupsValid = CheckEvaluatedPredicates(evaluatedGroups);
            return allGroupsValid;
        }

        private bool CheckEvaluatedPredicates(List<(bool, PredicateJoiner)> evaluatedPredicates)
        {
            var evaluation = true;
            var nextJoiner = PredicateJoiner.And;
            foreach (var evaluatedPredicate in evaluatedPredicates)
            {
                var isAnd = nextJoiner == PredicateJoiner.And;
                if (isAnd)
                {
                    evaluation = evaluation && evaluatedPredicate.Item1;
                }
                else
                {
                    evaluation = evaluation || evaluatedPredicate.Item1;
                }

                nextJoiner = evaluatedPredicate.Item2;
            }

            return evaluation;
        }

        protected abstract Task<bool> EvaluateAsync(Predicate<TPredicateType> predicate, TEvaluateData data);
    }
}
