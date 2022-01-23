namespace RPG.Core
{
    public interface IPredicateEvaluator
    {
        bool? evalute(string predicate, string[] parameters);
    }
}
