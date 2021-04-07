using System;

namespace CardCostApi.Infrastructure.Exceptions
{
    /// <summary>
    /// Exception for signalling card cost configuration errors.
    /// </summary>
    public class CardCostNotConfiguredException : Exception
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="CardCostNotConfiguredException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">(Optional) The inner exception.</param>
        public CardCostNotConfiguredException(string message, Exception? innerException = null) : base(
            message,
            innerException)
        {
        }
    }
}