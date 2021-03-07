using System;


/* Scenario: Method throws an exception but is neither caught nor documented
 * Expected Result:
 *   - No diagnostic results
 */

namespace ExceptionalAnalyzer.Test.Assets
{
    public class OneExceptionIsThrownAndRethrownButDocumented
    {
        /// <summary>
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
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
