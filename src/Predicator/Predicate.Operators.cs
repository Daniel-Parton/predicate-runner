namespace Predicator
{
    public enum PredicateExistsOperators
    {
        IsSet = 1,
        IsNotSet = 2,
    }

    public enum PredicateStringOperators
    {
        Is = 1,
        Contains = 2,
        StartsWith = 3,
        EndsWith = 4,
        IsNot = 5,
        DoesNotContain = 6,
        RegexMatch = 7,
        IsOneOf = 8,
        ContainsOneOf = 9,
        IsNotOneOf = 10,
        DoesNotContainOneOf = 11,
    }

    public enum PredicateNumberOperators
    {
        Equals = 1,
        DoesNotEqual = 2,
        IsLessThan = 3,
        IsLessThanOrEquals = 4,
        IsGreaterThan = 5,
        IsGreaterThanOrEquals = 6
    }

    public enum PredicateDateOperators
    {
        IsBefore = 1,
        IsAfter,
        IsBetween,
        IsSameDayAsEntry,
    }
}


