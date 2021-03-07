using System;


/* Scenario: Method throws an exception but is neither caught nor documented
 * Expected Result:
 *   - 23:17, InvalidOperationException
 */

namespace ExceptionalAnalyzer.Test.Assets
{
    public class OneExceptionIsThrownAndRethrownButNotDocumented
    {
        public void MethodThrowingException()
        {
            try
            {
                throw new InvalidOperationException();
            }
            catch(InvalidOperationException invalidOperationException)
            {
                Console.WriteLine(invalidOperationException.Message);

                throw;
            }
        }
    }
}
