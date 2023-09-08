using System;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Commands;

namespace RetryAttributes
{
    /// <summary>
    /// <para>
    ///     Applies retry functionality to the test assembly, fixture, or test method,
    ///     allowing it to be reattempted a specified number of times. The retry behavior
    ///     is hierarchical and can be overridden at lower levels. If the test assembly,
    ///     fixture, or test method has the <see cref="NoRetryAttribute" />, retry
    ///     functionality is ignored.
    /// </para>
    ///
    /// <remarks>
    ///     The logic behind the attribute is inspired by the RepeatAttributeTests
    ///     of Nunit repository:
    ///     https://github.com/nunit/nunit/blob/d22be76f6d7180142f197b7eeb8c8a36b97d8056/src/NUnitFramework/tests/Attributes/RepeatAttributeTests.cs#L98
    /// </remarks>
    ///
    /// <example>
    /// Add the following line of code to the AssemblyInfo.cs of your project:
    /// <code>
    ///     [assembly: RetryOnError(5)]
    /// </code><br/>
    /// </example>
    ///
    /// <example>
    /// Add retry functionality to a single <see cref="TestFixture"/>, or override the existing
    /// assembly retry setting on class level:
    /// <code>
    ///     [TestFixture, RetryOnError(3)]
    ///     public class MyTestFixture { ... }
    /// </code><br/>
    /// </example>
    ///
    /// <example>
    /// Add retry functionality to a single <see cref="TestMethod"/>, or override the existing
    /// assembly/fixture retry setting on method level:
    /// <code>
    ///     [Test, RetryOnError(3)]
    ///     public void MyTestMethod() { ... }
    /// </code><br/>
    /// </example>
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false,
        Inherited = true)]
    public class RetryOnErrorAttribute : NUnitAttribute, IRepeatTest, IApplyToTest
    {
        public int TryCount { get; }

        /// <summary>
        /// Construct a <see cref="RetryOnErrorAttribute" />
        /// </summary>
        /// <param name="tryCount">The maximum number of times the test should be run if it fails</param>
        public RetryOnErrorAttribute(int tryCount)
        {
            TryCount = tryCount;
        }

        /// <summary>
        /// Wrap a command and return the result.
        /// </summary>
        /// <param name="command">The command to be wrapped</param>
        /// <returns>The wrapped command</returns>
        public TestCommand Wrap(TestCommand command)
        {
            return new RetryOnErrorCommand(command, TryCount);
        }

        /// <summary>
        /// Apply retry attribute on existing test methods
        /// </summary>
        /// <param name="test"></param>
        public void ApplyToTest(Test test)
        {
            // Get all test methods of all fixtures recursively
            var testMethodEnumerable = test is TestMethod testMethod
                ? testMethod.GetRetryableTestCasesRecursively()
                : test.GetRetryableTestMethodsRecursively();
            
            // add retry attribute and replace the test method with the wrapped one
            foreach (var method in testMethodEnumerable)
                method.WrapWithAttributes(new RetryOnErrorAttribute(TryCount));
        }
    }
}