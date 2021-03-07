using System;


/* Scenario: Method throws an exception but is caught
 * Expected Result:
 *   - 24:21, InvalidOperationException
 */

namespace ExceptionalAnalyzer.Test.Assets
{
    public class OneExceptionIsThrownAndNestedRethrownButCaughtOnHigherLevel
    {
        public void MethodThrowingException()
        {
            try
            {
                try
                {
                    throw new InvalidOperationException();
                }
                catch(InvalidOperationException invalidOperationException)
                {
                    object obj = new object();

                    throw;
                }
            }
            catch(InvalidOperationException invalidOperationException)
            {
                // handle
            }
        }
    }
}
