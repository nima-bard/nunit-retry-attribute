using System;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Commands;

namespace RetryAttributes
{
    /// <summary>
    /// The test command for the <see cref="RetryAttribute"/>
    /// </summary>
    public class RetryOnErrorCommand : DelegatingTestCommand
    {
        private readonly int _tryCount;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="RetryOnErrorCommand"/> class.
        /// </summary>
        /// <param name="innerCommand">The inner command.</param>
        /// <param name="tryCount">The maximum number of repetitions</param>
        public RetryOnErrorCommand(TestCommand innerCommand, int tryCount)
            : base(innerCommand)
        {
            _tryCount = tryCount;
        }

        /// <summary>
        /// Runs the test, saving a TestResult in the supplied TestExecutionContext.
        /// </summary>
        /// <param name="context">The context in which the test should run.</param>
        /// <returns>A TestResult</returns>
        public override TestResult Execute(TestExecutionContext context)
        {
            var count = _tryCount;

            while (count-- > 0)
            {
                try
                {
                    context.CurrentResult = innerCommand.Execute(context);
                }
                // Commands are supposed to catch exceptions, but some don't
                // and we want to look at restructuring the API in the future.
                catch (Exception ex)
                {
                    if (context.CurrentResult == null) context.CurrentResult = context.CurrentTest.MakeTestResult();
                    context.CurrentResult.RecordException(ex);
                }

                if (context.CurrentResult.ResultState != ResultState.Failure &&
                    context.CurrentResult.ResultState != ResultState.Error)
                    break;

                // Clear result for retry
                if (count > 0)
                {
                    context.CurrentResult = context.CurrentTest.MakeTestResult();
                    context
                            .CurrentRepeatCount
                        ++; // increment Retry count for next iteration. will only happen if we are guaranteed another iteration
                }
            }

            return context.CurrentResult;
        }
    }
}