using System;
using System.Net;

namespace CardCostApi.Core.Exceptions
{
    /// <summary>
    /// Exception for signalling remote service errors.
    /// </summary>
    public class ExternalServiceCommunicationException : Exception
    {
        /// <summary>
        /// Gets the status code returned if available.
        /// </summary>
        public HttpStatusCode? StatusCode { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="ExternalServiceCommunicationException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="statusCode">Response status code.</param>
        /// <param name="innerException">(Optional) The inner exception.</param>
        public ExternalServiceCommunicationException(string message, HttpStatusCode? statusCode = null,
            Exception? innerException = null)
            : base(message, innerException)
        {
            StatusCode = statusCode;
        }
    }
}