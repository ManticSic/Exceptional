using System;


/* Scenario: Method throws an exception but is caught
 * Expected Result:
 *   - No diagnostic results
 */

namespace ExceptionalAnalyzer.Test.Assets
{
    public class MultipleExceptionsAreThrownButHandledDifferent
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
                // handle exception
            }

            throw new ArgumentException();
        }
    }
}
