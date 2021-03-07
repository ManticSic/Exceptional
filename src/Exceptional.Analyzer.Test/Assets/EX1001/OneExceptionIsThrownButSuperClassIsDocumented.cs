using System;


/* Scenario: Method throws an exception but is documented
 * Expected Result:
 *   - No diagnostic results
 */

namespace ExceptionalAnalyzer.Test.Assets
{
    public class OneExceptionIsThrownButSuperClassIsDocumented
    {
        /// <exception cref="SystemException"></exception>
        public void MethodThrowingException()
        {
            throw new InvalidOperationException();
        }
    }
}
