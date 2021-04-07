using System;

namespace CardCostApi.Infrastructure.Exceptions
{
    /// <summary>
    /// Exception for signalling card cost configuration errors.
    /// </summary>
    public class CardCostAlreadyExistsException : Exception
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="CardCostAlreadyExistsException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">(Optional) The inner exception.</param>
        public CardCostAlreadyExistsException(string message, Exception? innerException = null) : base(
            message,
            innerException)
        {
        }
    }
}