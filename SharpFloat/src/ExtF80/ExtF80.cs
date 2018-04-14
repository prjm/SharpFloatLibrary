/*  This library is part of the SharpFloat library and provided under the
 *  terms of the default license. See "LICENSE_Default.txt" in the project
 *  root directory.
 *
 *  Copyright 2018 Bastian Turcs. All rights reserved.
 */

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: InternalsVisibleTo("SharpFloatTests")]

namespace SharpFloat.FloatingPoint {

    /// <summary>
    ///     type definition for a <see langword="struct"/> representing an 80-bit floating point number
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 10)]
    public readonly partial struct ExtF80 {

        /// <summary>
        ///     value (significant)
        /// </summary>
        [FieldOffset(0)]
        public readonly ulong signif;

        /// <summary>
        ///     exponent and sign
        /// </summary>
        [FieldOffset(8)]
        public readonly ushort signExp;

        /// <summary>
        ///     create a new floating point value of extended precision
        /// </summary>
        /// <param name="signedExponent">exponent and sign (16 bit)</param>
        /// <param name="significant">significant (64 bit)</param>
        [DebuggerStepThrough]
        public ExtF80(ushort signedExponent, ulong significant) {
            signExp = signedExponent;
            signif = significant;
        }

        /// <summary>
        ///     <c>true</c> if this number is negative
        /// </summary>
        public bool IsNegative
            => (signExp >> 15) != 0;

        /// <summary>
        ///     unsigned exponent value
        /// </summary>
        public ushort UnsignedExponent
            => (ushort)(signExp & MaxExponent);

        /// <summary>
        ///     special operand values: infinity, indefinite
        /// </summary>
        public bool IsSpecialOperand
            => UnsignedExponent == MaxExponent;

        /// <summary>
        ///     denomeralized values have a zero exponent value
        /// </summary>
        public bool IsDenormal
            => UnsignedExponent == 0;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay
            => string.Format("0x{0:X4}{1:X16}", signExp, signif);

    }
}