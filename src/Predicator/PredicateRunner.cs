using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Predicator
{
    public abstract class PredicateRunner<TPredicate, TContext> where TPredicate : Predicate
    {
        public PredicateFilter<TPredicate> Filter { get; }
        public bool IsValid { get; set; }

        protected PredicateRunner(PredicateFilter<TPredicate> filter)
        {
            Filter = filter;
            IsValid = true;
        }

        protected PredicateRunner(IEnumerable<PredicateModel<TPredicate>> models, PredicateJoiner joiner = PredicateJoiner.And)
        {
            Filter = new PredicateFilter<TPredicate>
            {
                Joiner = joiner,
                Models = new List<PredicateModel<TPredicate>>(models),
            };
            IsValid = true;
        }

        protected PredicateRunner(IEnumerable<TPredicate> predicates, PredicateJoiner joiner = PredicateJoiner.And)
        {
            Filter = new PredicateFilter<TPredicate>
            {
                Models = new List<PredicateModel<TPredicate>>
                {
                    new PredicateModel<TPredicate>
                    {
                        Joiner = joiner,
                        Predicates = new List<TPredicate>(predicates)
                    }
                }
            };
            IsValid = true;
        }


        public Task<bool> EvaluateAsync(TContext context)
        {
            if (!IsValid) return Task.FromResult(false);

            var runnableModels = (Filter.Models ?? new List<PredicateModel<TPredicate>>())
                .Where(x => x.CanRun)
                .ToList();

            //Bail if no filters provided
            if (!runnableModels.Any())
            {
                return Task.FromResult(true);
            }

            return EvaluateModels(context, runnableModels, Filter.Joiner);
        }

        Task<bool> EvaluateModels(TContext context, List<PredicateModel<TPredicate>> models, PredicateJoiner joiner)
        {
            return EvaluateCollectionAsync(models.Select<PredicateModel<TPredicate>, Func<Task<bool>>>(model =>
            {
                return () => EvaluatePredicates(context, model.Predicates, model.Joiner);
            }), joiner);
        }

        Task<bool> EvaluatePredicates(TContext context, List<TPredicate> predicates, PredicateJoiner joiner)
        {
            return EvaluateCollectionAsync(predicates.Select<TPredicate, Func<Task<bool>>>(predicate =>
            {
                return () => EvaluateAsync(predicate, context);
            }), joiner);
        }

        protected abstract Task<bool> EvaluateAsync(TPredicate predicate, TContext context);

        Task<bool> EvaluateCollectionAsync(
            IEnumerable<Func<Task<bool>>> evaluations,
            PredicateJoiner joiner)
        {
            //Skip if we have no predicates
            if (evaluations == null || !evaluations.Any())
            {
                return Task.FromResult(true);
            }

            if (joiner == PredicateJoiner.And)
            {
                return EvaluateCollectionForAndAsync(evaluations);
            }

            return EvaluateCollectionForOrAsync(evaluations);
        }

        static async Task<bool> EvaluateCollectionForOrAsync(
            IEnumerable<Func<Task<bool>>> evaluations)
        {
            foreach (var evaluation in evaluations)
            {
                var success = await evaluation();

                //If any predicate is valid we should short circuit true here
                if (success)
                {
                    return true;
                }
            }

            return false;
        }

        async Task<bool> EvaluateCollectionForAndAsync(IEnumerable<Func<Task<bool>>> evaluations)
        {
            foreach (var evaluation in evaluations)
            {
                var success = await evaluation();

                if (!success)
                {
                    return false;
                }
            }

            return true;
        }
    }

    public abstract class PredicateRunner<TContext> : PredicateRunner<Predicate, TContext>
    {
        protected PredicateRunner(PredicateFilter<Predicate> filter) : base(filter)
        {
        }

        protected PredicateRunner(
            IEnumerable<PredicateModel<Predicate>> models,
            PredicateJoiner joiner = PredicateJoiner.And) : base(models, joiner)
        {
        }

        protected PredicateRunner(
            IEnumerable<Predicate> predicates,
            PredicateJoiner joiner = PredicateJoiner.And) : base(predicates, joiner)
        {
        }
    }
}
