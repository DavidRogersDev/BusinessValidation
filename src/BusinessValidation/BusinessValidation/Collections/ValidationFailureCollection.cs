using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BusinessValidation.Collections
{
    internal class ValidationFailureCollection : IEnumerable<KeyValuePair<string, IReadOnlyList<string>>>
    {
        private Dictionary<string, List<string>> _failures;

        internal ValidationFailureCollection()
        {
            _failures = new Dictionary<string, List<string>>();
        }

        internal int MessageCount => _failures.Sum(f => f.Value.Count);

        internal int FailCount => _failures.Count;

        internal void AddFailure(string failBundle, string failureMessage)
        {
            if (_failures.ContainsKey(failBundle))
            {
                _failures[failBundle].Add(failureMessage);
            }
            else
            {
                _failures.Add(failBundle, new List<string> { { failureMessage } });
            }
        }

        internal IDictionary<string, IReadOnlyList<string>> ToDictionary() => _failures.ToDictionary(k => k.Key, v => (IReadOnlyList<string>)v.Value);

        public IEnumerator<KeyValuePair<string, IReadOnlyList<string>>> GetEnumerator()
        {
            foreach (var failure in _failures)
            {
                yield return new KeyValuePair<string, IReadOnlyList<string>>(failure.Key, failure.Value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal bool ContainsFailed(string failBundle)
        {
            return _failures.ContainsKey(failBundle);
        }

        internal IReadOnlyList<string> ValidationMessages()
        {
            return _failures.SelectMany(f => f.Value).ToList();
        }

        internal IReadOnlyList<string> this[string index]
        {
            get
            {
                return _failures[index];
            }
        }
    }
}
