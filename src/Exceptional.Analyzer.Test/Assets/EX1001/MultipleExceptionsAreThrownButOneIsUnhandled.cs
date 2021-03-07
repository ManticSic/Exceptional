using System;


/* Scenario: Method throws an exception but is caught
 * Expected Result:
 *   - 19:17, InvalidOperationException
 *   - 36:13, ArgumentException
 */

namespace ExceptionalAnalyzer.Test.Assets
{
    public class MultipleExceptionsAreThrownButOneIsUnhandled
    {
        /// <exception cref="ArgumentException"></exception>
        public void MethodThrowingException(bool throwIt)
        {
            if(throwIt)
            {
                throw new InvalidOperationException();
            }

            throw new ArgumentException();
        }

        public void MethodThrowingException2()
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
