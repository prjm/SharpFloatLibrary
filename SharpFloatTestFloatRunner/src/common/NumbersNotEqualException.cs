using System;
using System.Runtime.Serialization;

namespace SharpFloatTestFloatRunner.Common {

    /// <summary>
    ///     exception thrown if two expected equal numbers aren't equal
    /// </summary>
    [Serializable]
    public class NumbersNotEqualException : Exception {

        /// <summary>
        ///     create a new exception
        /// </summary>
        public NumbersNotEqualException() : base() { }

        /// <summary>
        ///     create a new exception
        /// </summary>
        /// <param name="message">exception message</param>
        public NumbersNotEqualException(string message) : base(message) { }

        /// <summary>
        ///     create a new exception
        /// </summary>
        /// <param name="expected">expected value</param>
        /// <param name="actual">actual value</param>
        public NumbersNotEqualException(ulong expected, ulong actual)
            : base($"Expected value: {expected:X16}. Actual value {actual:X16}.") { }


        /// <summary>
        ///     create a new exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public NumbersNotEqualException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        ///     create a new exception
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected NumbersNotEqualException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    }
}