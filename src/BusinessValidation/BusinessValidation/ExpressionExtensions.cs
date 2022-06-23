using System;
using System.Linq.Expressions;
using System.Text;

namespace BusinessValidation
{
    public static class ExpressionExtensions
    {
        public static string ToPropertyPath<TObj, TRet>(this Expression<Func<TObj, TRet>> source)
        {
            var path = new StringBuilder();

            var memberExpression = GetMemberExpression(source);
            var insertDot = false;

            while (!ReferenceEquals(memberExpression, null))
            {
                if (insertDot)
                    path.Insert(0, ".");

                path.Insert(0, memberExpression.Member.Name);
                insertDot = true;

                memberExpression = GetMemberExpression(memberExpression.Expression);
            }

            return path.ToString();
        }

        public static MemberExpression GetMemberExpression(Expression expression)
        {
            if (expression is MemberExpression)
            {
                return (MemberExpression)expression;
            }
            else if (expression is LambdaExpression)
            {
                var lambdaExpression = (LambdaExpression)expression;

                if (lambdaExpression.Body is MemberExpression)
                {
                    return (MemberExpression)lambdaExpression.Body;
                }
                else if (lambdaExpression.Body is UnaryExpression)
                {
                    return ((MemberExpression)((UnaryExpression)lambdaExpression.Body).Operand);
                }
            }

            return null;
        }
    }
}
