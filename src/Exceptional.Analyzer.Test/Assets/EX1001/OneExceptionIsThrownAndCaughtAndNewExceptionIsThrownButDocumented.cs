using System;


/* Scenario: Method throws an exception but is neither caught nor documented
 * Expected Result:
 *   - No diagnostic results
 */

namespace ExceptionalAnalyzer.Test.Assets
{
    public class OneExceptionIsThrownAndCaughtAndNewExceptionIsThrownButDocumented
    {
        /// <exception cref="ArgumentException"></exception>
        public void MethodThrowingException()
        {
            try
            {
                throw new InvalidOperationException();
            }
            catch(InvalidOperationException invalidOperationException)
            {
                Console.WriteLine(invalidOperationException.Message);

                throw new ArgumentException();
            }
        }
    }
}
