using BusinessValidation.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace BusinessValidation
{
    [DebuggerDisplay("{ShowCounts,nq}")]
    /// <summary>
    /// A class for executing validation rules and collecting failures.
    /// </summary>
    public class Validator
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public Validator()
        {
            Results = new ValidationFailureCollection();
        }

        /// <summary>
        /// Constructor which takes a dictionary of failures as a parameter.
        /// </summary>
        /// <param name="failureCollection">Dictionary of existing failures.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="failureCollection"/> is <c>null</c>.</exception>
        public Validator(IDictionary<string, IReadOnlyList<string>> failureCollection)
        {
            if (failureCollection is null) throw new ArgumentNullException(nameof(failureCollection));

            Results = new ValidationFailureCollection();

            foreach (var failure in failureCollection)
            {
                foreach (var message in failure.Value)
                {
                    AddFailure(failure.Key, message);
                }
            }
        }

        /// <summary>
        /// Constructor which takes a readonly dictionary of failures as a parameter.
        /// </summary>
        /// <param name="failureCollection">Readonly Dictionary of existing failures.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="failureCollection"/> is <c>null</c>.</exception>
        public Validator(IReadOnlyDictionary<string, IReadOnlyList<string>> failureCollection)
        {
            if (failureCollection is null) throw new ArgumentNullException(nameof(failureCollection));

            Results = new ValidationFailureCollection();

            foreach (var failure in failureCollection)
            {
                foreach (var message in failure.Value)
                {
                    AddFailure(failure.Key, message);
                }
            }
        }

        internal ValidationFailureCollection Results { get; }

        /// <summary>
        /// Adds a validation failure to the collection.
        /// </summary>
        /// <param name="failBundle">The name of the fail bundle to which the message will be added.</param>
        /// <param name="failureMessage">The message for the validation failure.</param>
        /// <returns>The <see cref="Validator"/> object, to enable chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="failureMessage"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="failureMessage"/> is white space.</exception>
        public Validator AddFailure(string failBundle, string failureMessage)
        {
            if (failureMessage is null) throw new ArgumentNullException(nameof(failureMessage));
            if (string.IsNullOrWhiteSpace(failureMessage)) throw new ArgumentException($"'{nameof(failureMessage)}' cannot be whitespace.", nameof(failureMessage));

            Results.AddFailure(failBundle, failureMessage);

            return this;
        }

        /// <summary>
        /// Validates a condition.
        /// </summary>
        /// <param name="failBundle">The name of the fail bundle to which the message will be added in the event that validation fails.</param>
        /// <param name="failureMessage">The message for the validation failure.</param>
        /// <param name="condition">The condition, or business rule, to validate.</param>
        /// <returns>A <see cref="Boolean"/> representing the result of the validation check.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="failBundle"/> or <paramref name="failureMessage"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="failureMessage"/> is white space.</exception>
        public bool Validate(string failBundle, string failureMessage, bool condition)
        {
            if (!condition)
            {
                AddFailure(failBundle, failureMessage);
            }

            return condition;
        }

        /// <summary>
        /// Validates a general predicate returning a <see cref="Boolean"/>.
        /// </summary>        
        /// <param name="failBundle"></param>
        /// <param name="failureMessage"></param>        
        /// <param name="predicate"></param>
        /// <returns>A <see cref="Boolean"/> representing the result of the validation check.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="failBundle"/>, <paramref name="failureMessage"/> or <paramref name="predicate"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="failureMessage"/> is white space.</exception>
        public bool Validate(string failBundle, string failureMessage, Func<bool> predicate)
        {            
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));            

            var isValid = predicate();

            if (!isValid)
            {
                AddFailure(failBundle, failureMessage);
            }

            return isValid;
        }

        /// <summary>
        /// Validates a general predicate returning a <see cref="Boolean"/>.
        /// </summary>
        /// <typeparam name="T">Type of the object upon which validation is to be performed on.</typeparam>
        /// <param name="expression">An expression of type <see cref="Expression&lt;Func&gt;T&lt;TM&gt;&gt;" /> enabling the selection of a property on the object to validate, without using a string.</param>
        /// <param name="failureMessage">The message for the validation failure.</param>
        /// <param name="predicate">A func delegate of type <see cref="T:Func&lt;T, Boolean&gt;" /> providing a condition typed to an object of type <typeparamref name="T"/> for validation of that object.</param>
        /// <param name="propertyDepth">An enum of type <see cref="PropertyDepth" /> specifying whether the fullpath, or the terminating property, of the lambda in the first argument is to be added as the name of the fail-bundle. Fullpath is the default. E.g. if the lambda is p => p.Person.Address.Suburb, the name added will be "Person.Address.Suburb".</param>
        /// <returns>A <see cref="Boolean"/> representing the result of the validation check.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="expression"/>, <paramref name="failureMessage"/> or <paramref name="predicate"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="failureMessage"/> is white space.</exception>
        public bool Validate<T, TM>(Expression<Func<T, TM>> expression, string failureMessage, Func<bool> predicate, PropertyDepth propertyDepth = PropertyDepth.FullPath)
        {
            if (expression is null) throw new ArgumentNullException(nameof(expression));
            if (failureMessage is null) throw new ArgumentNullException(nameof(failureMessage));
            if (string.IsNullOrWhiteSpace(failureMessage)) throw new ArgumentException($"'{nameof(failureMessage)}' cannot be whitespace.", nameof(failureMessage));
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));

            var path = ExtractPath(expression, propertyDepth);

            var isValid = predicate();

            if (!isValid)
            {
                var failBundleToken = "{{fail-bundle}}";
                var failBundleTokenEscaped = "{fail-bundle}";

                if (failureMessage.Contains(failBundleToken, StringComparison.OrdinalIgnoreCase))
                {
                    AddFailure(path, failureMessage.Replace(failBundleToken, path));
                }
                else if (failureMessage.Contains(failBundleTokenEscaped, StringComparison.OrdinalIgnoreCase))
                {
                    AddFailure(path, failureMessage.Replace(failBundleTokenEscaped, path));
                }
                else
                {
                    AddFailure(path, failureMessage);
                }
            }

            return isValid;
        }

        /// <summary>
        /// Validates a predicate, strongly typed to an object which is passed in. The predicate is applied to the <paramref name="objectToValidate" /> parameter
        /// </summary>
        /// <typeparam name="T">Type of the object upon which validation is to be performed on.</typeparam>
        /// <param name="failBundle">The name of the fail bundle to which the message will be added in the event that validation fails.</param>
        /// <param name="failureMessage">The message for the validation failure.</param>
        /// <param name="objectToValidate">An object of the given type <typeparamref name="T"/> to validate.</param>
        /// <param name="predicate">A func delegate of type <see cref="T:Func&lt;T, Boolean&gt;" /> providing a condition typed to an object of type <typeparamref name="T"/> for validation of that object.</param>
        /// <returns>A <see cref="Boolean"/> representing the result of the validation check.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="failBundle"/>, <paramref name="failureMessage"/>, <paramref name="objectToValidate"/> or <paramref name="predicate"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="failureMessage"/> is white space.</exception>
        public bool Validate<T>(string failBundle, string failureMessage, T objectToValidate, Func<T, bool> predicate)
            where T : class
        {
            if (objectToValidate is null) throw new ArgumentNullException(nameof(objectToValidate));            
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));                        

            var isValid = predicate(objectToValidate);

            if (!isValid)
            {
                AddFailure(failBundle, failureMessage);
            }

            return isValid;
        }

        /// <summary>
        /// Validates a condition.
        /// </summary>
        /// <typeparam name="T">Type of the object upon which validation is to be performed on.</typeparam>
        /// <typeparam name="TM">Type of the property of the object upon which validation is to be performed on.</typeparam>
        /// <param name="expression">An expression of type <see cref="Expression&lt;Func&gt;T&lt;TM&gt;&gt;" /> enabling the selection of a property on the object to validate, without using a string.</param>
        /// <param name="failureMessage">The message for the validation failure.</param>
        /// <param name="condition">The condition, or business rule, to validate.</param>
        /// <param name="propertyDepth">An enum of type <see cref="PropertyDepth" /> specifying whether the fullpath, or the terminating property, of the lambda in the first argument is to be added as the name of the fail-bundle. Fullpath is the default. E.g. if the lambda is p => p.Person.Address.Suburb, the name added will be "Person.Address.Suburb".</param>
        /// <returns>A <see cref="Boolean"/> representing the result of the validation check.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="expression"/> or <paramref name="failureMessage"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="failureMessage"/> is white space.</exception>        
        public bool Validate<T, TM>(Expression<Func<T, TM>> expression, string failureMessage, bool condition, PropertyDepth propertyDepth = PropertyDepth.FullPath)
            where T : class
        {
            if (expression is null) throw new ArgumentNullException(nameof(expression));
            if (failureMessage is null) throw new ArgumentNullException(nameof(failureMessage));
            if (string.IsNullOrWhiteSpace(failureMessage)) throw new ArgumentException($"'{nameof(failureMessage)}' cannot be whitespace.", nameof(failureMessage));

            var path = ExtractPath(expression, propertyDepth);

            if (!condition)
            {
                var failBundleToken = "{{fail-bundle}}";
                var failBundleTokenEscaped = "{fail-bundle}";

                if (failureMessage.Contains(failBundleToken, StringComparison.OrdinalIgnoreCase))
                {
                    AddFailure(path, failureMessage.Replace(failBundleToken, path));
                }
                else if (failureMessage.Contains(failBundleTokenEscaped, StringComparison.OrdinalIgnoreCase))
                {
                    AddFailure(path, failureMessage.Replace(failBundleTokenEscaped, path));
                }
                else
                {
                    AddFailure(path, failureMessage);
                }
            }

            return condition;
        }

        /// <summary>
        /// Validates a condition.
        /// </summary>
        /// <typeparam name="T">Type of the object upon which validation is to be performed on.</typeparam>
        /// <typeparam name="TM">Type of the property of the object upon which validation is to be performed on.</typeparam>
        /// <param name="expression">An expression of type <see cref="Expression&lt;Func&gt;T&lt;TM&gt;&gt;" /> enabling the selection of a property on the object to validate, without using a string.</param>
        /// <param name="failureMessage">The message for the validation failure.</param>
        /// <param name="objectToValidate">An object of the given type <typeparamref name="T"/> to validate.</param>
        /// <param name="condition">The condition, or business rule, to validate.</param>
        /// <param name="propertyDepth">An enum of type <see cref="PropertyDepth" /> specifying whether the fullpath, or the terminating property, of the lambda in the first argument is to be added as the name of the fail-bundle. Fullpath is the default. E.g. if the lambda is p => p.Person.Address.Suburb, the name added will be "Person.Address.Suburb".</param>
        /// <returns>A <see cref="Boolean"/> representing the result of the validation check.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="objectToValidate"/> or <paramref name="failureMessage"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="failureMessage"/> is white space.</exception>
        [Obsolete("Deprecated. The objectToValidate parameter is redunandant. Please use the over load with the following parameters: expression, failureMessage, condition, propertyDepth")]
        public bool Validate<T, TM>(Expression<Func<T, TM>> expression, string failureMessage, T objectToValidate, bool condition, PropertyDepth propertyDepth = PropertyDepth.FullPath)
            where T : class
        {
            if (expression is null) throw new ArgumentNullException(nameof(expression));
            if (failureMessage is null) throw new ArgumentNullException(nameof(failureMessage));
            if (string.IsNullOrWhiteSpace(failureMessage)) throw new ArgumentException($"'{nameof(failureMessage)}' cannot be whitespace.", nameof(failureMessage));
            if (objectToValidate is null) throw new ArgumentNullException(nameof(objectToValidate));

            var path = ExtractPath(expression, propertyDepth);

            if (!condition)
            {
                var failBundleToken = "{{fail-bundle}}";
                var failBundleTokenEscaped = "{fail-bundle}";

                if (failureMessage.Contains(failBundleToken, StringComparison.OrdinalIgnoreCase))
                {
                    AddFailure(path, failureMessage.Replace(failBundleToken, path));
                }
                else if (failureMessage.Contains(failBundleTokenEscaped, StringComparison.OrdinalIgnoreCase))
                {
                    AddFailure(path, failureMessage.Replace(failBundleTokenEscaped, path));
                }
                else
                {
                    AddFailure(path, failureMessage);
                }
            }

            return condition;
        }

        /// <summary>
        /// Validates a predicate.
        /// </summary>
        /// <typeparam name="T">Type of the object upon which validation is to be performed on.</typeparam>
        /// <typeparam name="TM">Type of the property of the object upon which validation is to be performed on.</typeparam>
        /// <param name="expression">An expression of type <see cref="Expression&lt;Func&gt;T&lt;TM&gt;&gt;" /> enabling the selection of a property on the object to validate, without using a string.</param>
        /// <param name="failureMessage">The message for the validation failure.</param>
        /// <param name="objectToValidate">An object of the given type <typeparamref name="T"/> to validate.</param>
        /// <param name="predicate">A func delegate of type <see cref="T:Func&lt;T, Boolean&gt;" /> providing a condition typed to an object of type <typeparamref name="T"/> for validation of that object.</param>
        /// <param name="propertyDepth">An enum of type <see cref="PropertyDepth" /> specifying whether the fullpath, or the terminating property, of the lambda in the first argument is to be added as the name of the fail-bundle. Fullpath is the default. E.g. if the lambda is p => p.Person.Address.Suburb, the name added will be "Person.Address.Suburb".</param>
        /// <returns>A <see cref="Boolean"/> representing the result of the validation check.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="expression"/>, <paramref name="failureMessage"/>, <paramref name="objectToValidate"/> or <paramref name="predicate"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="failureMessage"/> is white space.</exception>
        public bool Validate<T, TM>(Expression<Func<T, TM>> expression, string failureMessage, T objectToValidate, Func<T, bool> predicate, PropertyDepth propertyDepth = PropertyDepth.FullPath)
            where T : class
        {
            if (expression is null) throw new ArgumentNullException(nameof(expression));
            if (failureMessage is null) throw new ArgumentNullException(nameof(failureMessage));
            if (string.IsNullOrWhiteSpace(failureMessage)) throw new ArgumentException($"'{nameof(failureMessage)}' cannot be whitespace.", nameof(failureMessage));
            if (objectToValidate is null) throw new ArgumentNullException(nameof(objectToValidate));
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));

            var path = ExtractPath(expression, propertyDepth);

            var isValid = predicate(objectToValidate);

            if (!isValid)
            {
                var failBundleToken = "{{fail-bundle}}";
                var failBundleTokenEscaped = "{fail-bundle}";

                if (failureMessage.Contains(failBundleToken, StringComparison.OrdinalIgnoreCase))
                {
                    AddFailure(path, failureMessage.Replace(failBundleToken, path));
                }
                else if (failureMessage.Contains(failBundleTokenEscaped, StringComparison.OrdinalIgnoreCase))
                {
                    AddFailure(path, failureMessage.Replace(failBundleTokenEscaped, path));
                }
                else
                {
                    AddFailure(path, failureMessage);
                }
            }

            return isValid;
        }

        /// <summary>
        /// The collection of validation failures, all collated by fail bundle.
        /// </summary>
        public IReadOnlyDictionary<string, IReadOnlyList<string>> ValidationFailures => new ReadOnlyDictionary<string, IReadOnlyList<string>>(Results.ToDictionary());

        /// <summary>
        /// The raw validation messages, not grouped by fail bundle.
        /// </summary>
        public IReadOnlyList<string> ValidationMessages => Results.ValidationMessages();

        /// <summary>
        /// Indicates whether validation passed.
        /// </summary>
        /// <returns>A <see cref="Boolean"/> representing whether the validator object is valid.</returns>
        public bool IsValid() => Results.FailCount < 1;

        /// <summary>
        /// Indicates whether validation failed.
        /// </summary>
        /// <returns>A <see cref="Boolean"/> representing whether the validator object is invalid.</returns>
        public bool NotValid() => Results.FailCount > 0;

        /// <summary>
        /// Merges one <see cref="Validator"/> object with another.
        /// </summary>
        /// <param name="other">Another <see cref="Validator"/> object to merge with the one invoking this method.</param>
        /// <returns>The <see cref="Validator"/> object to enable chaining.</returns>
        public Validator Merge(Validator other)
        {
            if (other is null || other.IsValid())
                return this;

            if (IsValid() && other.NotValid())
                return other;

            // if reach here, both this and other are invalid.
            var newValidator = new Validator();

            foreach (var failBundle in Results)
            {
                foreach (var failureMessage in failBundle.Value)
                    newValidator.AddFailure(failBundle.Key, failureMessage);
            }

            foreach (var errorKey in other.ValidationFailures)
            {
                foreach (var failureMessage in errorKey.Value)
                    newValidator.AddFailure(errorKey.Key, failureMessage);
            }

            return newValidator;
        }

        public override string ToString()
        {
            var sepKey = "_|_";
            var sepVal = "----";

            var representativeString = Results
                .Select(e => e.Key + sepKey + e.Value.Aggregate((fails, fail) => fails + sepVal + fail))
                .Aggregate("************ Validator Object ************", (acc, curr) => acc += Environment.NewLine + curr.Split(sepKey).First() + ":" + curr.Split(sepKey).Skip(1)
                .Aggregate(string.Empty, (innerAcc, innerCurr) => innerAcc + Environment.NewLine + "\t" + innerCurr.Split(sepVal)
                .Aggregate((failAcc, failCurr) => failAcc + Environment.NewLine + "\t" + failCurr)));

            return string.Concat(representativeString, Environment.NewLine, "************ **************** ************");
        }


        public static implicit operator bool(Validator validator)
        {
            if (validator is null)
            {
                throw new ArgumentNullException(nameof(validator));
            }

            return validator.IsValid();
        }

        public static bool operator true(Validator validator)
        {
            return validator;
        }

        public static bool operator false(Validator validator)
        {
            return !validator;
        }

        /// <summary>
        /// String indexer. Fail-bundle key.
        /// </summary>
        /// <param name="index">Index of item to retrieve.</param>
        /// <returns>List of failure messages for the fail-bundle.</returns>
        public IReadOnlyList<string> this[string index]
        {
            get
            {
                return ValidationFailures[index];
            }
        }

        /// <summary>
        /// Throws a custom exception to make the validation failures available to a higher layer in the application flow.
        /// </summary>
        /// <exception cref="ValidationFailureException"></exception>
        [Obsolete("Throw was poorly named and is deprecated. Please use ThrowIfInvalid, which does the same thing.")]
        public void Throw()
        {
            if (!this)
                throw new ValidationFailureException(ValidationFailures);
        }

        /// <summary>
        /// Throws a custom exception to make the validation failures available to a higher layer in the application flow.
        /// </summary>
        /// <exception cref="ValidationFailureException"></exception>
        public void ThrowIfInvalid()
        {
            if (!this)
                throw new ValidationFailureException(ValidationFailures);
        }

        private string ShowCounts => $"Failure Count ➡️ {Results.FailCount}, Message Count ➡️ {Results.MessageCount}";

        private static string ExtractPath<T, TM>(Expression<Func<T, TM>> expression, PropertyDepth propertyDepth) => propertyDepth switch
        {
            PropertyDepth.TerminatingProperty => expression.ToTerminatingProperty(),
            PropertyDepth.FullPath => expression.ToPropertyPath(),
            _ => throw new ArgumentOutOfRangeException(nameof(propertyDepth), $"Not a valid propertyDepth value: {propertyDepth}"),
        };
    }
}
