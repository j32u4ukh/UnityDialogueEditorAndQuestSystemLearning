using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    [System.Serializable]
    public class Condition
    {
        [SerializeField] string predicate;
        [SerializeField] string[] parameters;

        public bool check(IEnumerable<IPredicateEvaluator> evaluators)
        {
            foreach(var evaluator in evaluators)
            {
                bool? result = evaluator.evalute(predicate, parameters);

                if(result == null)
                {
                    continue;
                }
                else if (result == false)
                {
                    return false;
                }
            }

            return true;
        }
    }
}