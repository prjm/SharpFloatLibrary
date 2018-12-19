using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace SharpFloatTestFloatRunner.Common {

    /// <summary>
    ///     exception thrown on an invalid computation result
    /// </summary>
    [Serializable]
    public class InvalidComputationException : Exception {

        /// <summary>
        ///     create a new invalid computation exception
        /// </summary>
        public InvalidComputationException() : base() { }

        /// <summary>
        ///     create a new invalid computation exception
        /// </summary>
        /// <param name="message">exception message</param>
        public InvalidComputationException(string message) : base(message) { }

        /// <summary>
        ///     create a new invalid computation exception
        /// </summary>
        /// <param name="message">exception message</param>
        /// <param name="innerException">inner exception</param>
        public InvalidComputationException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        ///     create a new invalid computation exception
        /// </summary>
        /// <param name="lineCount"></param>
        /// <param name="line"></param>
        /// <param name="innerException"></param>
        public InvalidComputationException(ulong lineCount, string line, Exception innerException) :
            base($"Invalid computation at line {lineCount.ToString(CultureInfo.InvariantCulture)}. Data: {line}.", innerException) { }

        /// <summary>
        ///     create a new invalid computation exception
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected InvalidComputationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}