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

namespace SharpFloat.FloatingPoint {

    /// <summary>
    ///     type definition for a <see langword="struct"/> representing an 80-bit floating point number
    /// </summary>
    [DebuggerDisplay("signExp = {signExp}, signif = {signif}")]
    public partial struct ExtF80 {

        /// <summary>
        ///     default NaN (not a number) value
        /// </summary>
        public static readonly ExtF80 DefaultNaN
            = new ExtF80(DefaultNaNExponent, DefaultNaNSignificant);

        /// <summary>
        ///     default NaN: exponent
        /// </summary>
        public const ushort DefaultNaNExponent
            = 0xFFFF;

        /// <summary>
        ///     default NaN: significant
        /// </summary>
        public const ulong DefaultNaNSignificant
            = 0xC000000000000000UL;

        /// <summary>
        ///     largest exponent value
        /// </summary>
        private const ushort MaxExponent
            = 0x7FFF;

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
        ///     exponent and sign
        /// </summary>
        public readonly ushort signExp;

        /// <summary>
        ///     value (significant)
        /// </summary>
        public readonly ulong signif;

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

    }
}
