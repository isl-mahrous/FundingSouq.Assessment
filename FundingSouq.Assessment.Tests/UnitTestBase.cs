using System.Linq.Expressions;
using Bogus;
using FluentAssertions;
using FundingSouq.Assessment.Core.Dtos.Common;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace FundingSouq.Assessment.Tests;

public abstract class UnitTestBase
{
    protected readonly Faker Faker;

    protected UnitTestBase()
    {
        Faker = new Faker();
    }

    protected T CreateMock<T>() where T : class
    {
        return Substitute.For<T>();
    }

    /// <summary>
    /// Creates a mock IOptions instance for the given configuration type.
    /// </summary>
    /// <typeparam name="T">The configuration type.</typeparam>
    /// <param name="value">The configuration value to return.</param>
    /// <returns>A mocked IOptions object.</returns>
    protected IOptions<T> CreateOptions<T>(T value) where T : class, new()
    {
        var options = Substitute.For<IOptions<T>>();
        options.Value.Returns(value);
        return options;
    }

    protected void AssertSuccess<T>(Result<T> result)
    {
        result.IsSuccess.Should().BeTrue();
        result.Error.Should().BeNull();
    }

    protected void AssertFailure<T>(Result<T> result, Error expectedError)
    {
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(expectedError);
    }
}

public static class ExpressionComparer
{
    public static bool AreEqual<T>(Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
    {
        return ExpressionEqualityComparer.Instance.Equals(expr1, expr2);
    }
}

public class ExpressionEqualityComparer : IEqualityComparer<Expression>
{
    public static readonly ExpressionEqualityComparer Instance = new ExpressionEqualityComparer();

    public bool Equals(Expression x, Expression y)
    {
        return ExpressionEqual(x, y);
    }

    public int GetHashCode(Expression obj)
    {
        // You may need a more robust implementation depending on your requirements
        return obj.ToString().GetHashCode();
    }

    private bool ExpressionEqual(Expression x, Expression y)
    {
        // Implement logic to compare the structure of two expressions
        if (x == y) return true;
        if (x == null || y == null) return false;

        if (x.NodeType != y.NodeType || x.Type != y.Type)
            return false;

        switch (x.NodeType)
        {
            case ExpressionType.Lambda:
                var lambda1 = (LambdaExpression)x;
                var lambda2 = (LambdaExpression)y;
                return ExpressionEqual(lambda1.Body, lambda2.Body);

            case ExpressionType.Equal:
            case ExpressionType.NotEqual:
            case ExpressionType.AndAlso:
            case ExpressionType.OrElse:
                var binary1 = (BinaryExpression)x;
                var binary2 = (BinaryExpression)y;
                return ExpressionEqual(binary1.Left, binary2.Left) && ExpressionEqual(binary1.Right, binary2.Right);

            case ExpressionType.MemberAccess:
                var member1 = (MemberExpression)x;
                var member2 = (MemberExpression)y;
                return member1.Member == member2.Member && ExpressionEqual(member1.Expression, member2.Expression);

            // Handle other expression types as needed...
            default:
                return x.Equals(y); // Fallback to default equality
        }
    }
}
