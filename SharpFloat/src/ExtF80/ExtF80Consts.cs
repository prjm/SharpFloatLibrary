/*  This library is part of the SharpFloat library and provided under the
 *  terms of the default license. See "LICENSE_Default.txt" in the project
 *  root directory.
 *
 *  Copyright 2018 Bastian Turcs. All rights reserved.
 */

namespace SharpFloat.FloatingPoint {

    public partial struct ExtF80 {

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
            = new ExtF80(0x0001, 0x8000000000000000);

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
        ///     pi constant
        /// </summary>
        public static readonly ExtF80 Pi
            = new ExtF80(0x4000, 0xc90fdaa22168C235);

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
        ///     test if this value is zero
        /// </summary>
        public bool IsZero
            => signExp == 0 || signExp == NegativeZeroExponent;

    }

}
