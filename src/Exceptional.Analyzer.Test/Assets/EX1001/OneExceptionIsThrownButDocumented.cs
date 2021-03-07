using System;


/* Scenario: Method throws an exception but is documented
 * Expected Result:
 *   - No diagnostic results
 */

namespace ExceptionalAnalyzer.Test.Assets
{
    public class OneExceptionIsThrownButDocumented
    {
        /// <exception cref="InvalidOperationException"></exception>
        public void MethodThrowingException()
        {
            throw new InvalidOperationException();
        }
    }
}
