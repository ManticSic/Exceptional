using System;


/* Scenario: Method throws an exception but is neither caught nor documented
 * Expected Result:
 *   - 14:15, InvalidOperationException
 */

namespace ExceptionalAnalyzer.Test.Assets
{
    public class OneExceptionIsThrownButNotHandled
    {
        public void MethodThrowingException(bool value)
        {
            throw new InvalidOperationException();
        }
    }
}
