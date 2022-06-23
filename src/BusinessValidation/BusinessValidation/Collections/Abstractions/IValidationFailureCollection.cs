using System.Collections.Generic;

namespace BusinessValidation.Collections.Abstractions
{
    internal interface IValidationFailureCollection : IEnumerable<KeyValuePair<string, IReadOnlyList<string>>>
    {
        IReadOnlyList<string> this[string key] { get; }
        void AddFailure(string failBundle, string errorMessage);
        bool ContainsFailed(string failBundle);
        int MessageCount { get; }
        int FailCount { get; }
        IReadOnlyList<string> ValidationMessages();
        IDictionary<string, IReadOnlyList<string>> ToDictionary();
    }
}

