using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    [System.Serializable]
    public class Condition
    {
        [SerializeField] Disjunction[] and;

        public bool check(IEnumerable<IPredicateEvaluator> evaluators)
        {
            foreach (Disjunction disjunction in and)
            {
                if (!disjunction.check(evaluators))
                {
                    return false;
                }
            }

            return true;
        }

        [System.Serializable]
        class Disjunction
        {
            [SerializeField] Predicate[] or;

            public bool check(IEnumerable<IPredicateEvaluator> evaluators)
            {
                foreach (Predicate predicate in or)
                {
                    if (predicate.check(evaluators))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        [System.Serializable]
        class Predicate
        {
            [SerializeField] string predicate;
            [SerializeField] string[] parameters;
            [SerializeField] bool negate = false;

            public bool check(IEnumerable<IPredicateEvaluator> evaluators)
            {
                foreach (var evaluator in evaluators)
                {
                    bool? result = evaluator.evalute(predicate, parameters);

                    if (result == null)
                    {
                        continue;
                    }
                    else if (result == negate)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        
    }
}