using System;


/* Scenario: Method throws an exception but is caught
 * Expected Result:
 *   - No diagnostic results
 */

namespace ExceptionalAnalyzer.Test.Assets
{
    public class OneExceptionIsThrownAndNestedRethrownButDocumented
    {
        /// <exception cref="InvalidOperationException"></exception>
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
