/*  This library is a port of the standard softfloat library, Release 3e, from John Hauser to C#.
 *
 *    Copyright 2011, 2012, 2013, 2014, 2015, 2016, 2017, 2018 The Regents of the
 *    University of California.  All rights reserved.
 *
 *    Copyright 2018 Bastian Turcs. All rights reserved.
 *
 *    Redistribution and use in source and binary forms, with or without
 *    modification, are permitted provided that the following conditions are met:
 *
 *     1. Redistributions of source code must retain the above copyright notice,
 *        this list of conditions, and the following disclaimer.
 *
 *     2. Redistributions in binary form must reproduce the above copyright
 *        notice, this list of conditions, and the following disclaimer in the
 *        documentation and/or other materials provided with the distribution.
 *
 *     3. Neither the name of the University nor the names of its contributors
 *        may be used to endorse or promote products derived from this software
 *        without specific prior written permission.
 *
 *    THIS SOFTWARE IS PROVIDED BY THE REGENTS AND CONTRIBUTORS "AS IS", AND ANY
 *    EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 *    WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE, ARE
 *    DISCLAIMED.  IN NO EVENT SHALL THE REGENTS OR CONTRIBUTORS BE LIABLE FOR ANY
 *    DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 *    (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 *    LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 *    ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 *    (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
 *    THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
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
        ///     default NaN: exponent value
        /// </summary>
        public const ushort DefaultNaNExponent
            = 0xFFFF;

        /// <summary>
        ///     default NaN: significant value
        /// </summary>
        public const ulong DefaultNaNSignificant
            = 0xC000000000000000UL;

        /// <summary>
        ///     default NaN (not a number) value
        /// </summary>
        public static readonly ExtF80 DefaultNaN
            = new ExtF80(DefaultNaNExponent, DefaultNaNSignificant);

        /// <summary>
        ///     special value: positive zero value
        /// </summary>
        public static readonly ExtF80 Zero
            = new ExtF80(0, 0);

        /// <summary>
        ///     special value: negative zero value
        /// </summary>
        public static readonly ExtF80 NegativeZero
            = new ExtF80(NegativeZeroExponent, 0);

        /// <summary>
        ///     special value: positive infinity
        /// </summary>
        public static readonly ExtF80 Infinity
            = new ExtF80(MaxExponent, MaskBit64);

        /// <summary>
        ///     special value: negative infinity
        /// </summary>
        public static readonly ExtF80 NegativeInfinity
            = new ExtF80(DefaultNaNExponent, MaskBit64);

        /// <summary>
        ///     smallest value
        /// </summary>
        public static readonly ExtF80 MinValue
            = new ExtF80(0x0001, 0x0000000000000001);

        /// <summary>
        ///     largest value
        /// </summary>
        public static readonly ExtF80 MaxValue
            = new ExtF80(0x7FFE, 0xFFFFFFFFFFFFFFFF);

        /// <summary>
        ///     smallest denormalized value
        /// </summary>
        public static readonly ExtF80 MinValueDenormal
            = new ExtF80(0x0000, 0x0000000000000001);

        /// <summary>
        ///     largest exponent value
        /// </summary>
        private const ushort MaxExponent
            = 0x7FFF;

        /// <summary>
        ///     negative zero exponent value
        /// </summary>
        private const ushort NegativeZeroExponent
            = 0x8000;

        /// <summary>
        ///     bit mask - 64 bits
        /// </summary>
        private const ulong MaskAll63Bits
            = 0x7FFFFFFFFFFFFFFFUL;

        /// <summary>
        ///     bit mask - 62 bits
        /// </summary>
        private const ulong MaskAll62Bits
            = 0x3FFFFFFFFFFFFFFFUL;

        /// <summary>
        ///     bit mask - bit 64
        /// </summary>
        private const ulong MaskBit64
            = 0x8000000000000000UL;

        /// <summary>
        ///     bis mask - bit 63
        /// </summary>
        private const ulong MaskBit63
            = 0x4000000000000000UL;

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