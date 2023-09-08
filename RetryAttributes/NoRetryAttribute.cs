using System;
using NUnit.Framework;

namespace RetryAttributes
{
    /// <summary>
    /// Specifies that the test assembly, fixture, or test method should not have
    /// retry functionality applied. Respects hierarchy like <see cref="RetryOnErrorAttribute" />.
    /// Ignores retry in fixture or method levels, if already applied at higher
    /// levels, such as the assembly.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class NoRetryAttribute : NUnitAttribute
    {
    }
}