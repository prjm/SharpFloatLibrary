/*  This library is a port of the standard softfloat library, Release 3e, from John Hauser to C#.
 *
 *    Copyright 2011, 2012, 2013, 2014, 2015, 2016, 2017, 2018 The Regents of the
 *    University of California.  All rights reserved.
 *
 *    Copyright 2018, 2019 Bastian Turcs. All rights reserved.
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

using System.Runtime.InteropServices;

namespace SharpFloat.Helpers {

    /// <summary>
    ///     helper methods for single precision floating point arithmethics
    /// </summary>
    public static partial class FloatHelpers {

        [StructLayout(LayoutKind.Explicit)]
        private struct FloatAndUIntUnion {

            [FieldOffset(0)]
            public uint UInt32Bits;

            [FieldOffset(0)]
            public float FloatValue;
        }


        /// <summary>
        ///     get bit value of a 32-bit floating point number
        /// </summary>
        /// <param name="value">floating point number</param>
        /// <returns>raw bit value</returns>
        public static uint SingleToInt32Bits(float value) {
            var u = new FloatAndUIntUnion {
                FloatValue = value
            };
            return u.UInt32Bits;
        }

        /// <summary>
        ///     get the floating-point value of a raw-32 bit value
        /// </summary>
        /// <param name="value">raw bit value</param>
        /// <returns>equivalent floating point number</returns>
        public static float Int32BitsToSingle(uint value) {
            var u = new FloatAndUIntUnion {
                UInt32Bits = value
            };
            return u.FloatValue;
        }

        /// <summary>
        ///     pack a 32-bit floating point number by the given parts
        /// </summary>
        /// <param name="sign">sign value</param>
        /// <param name="exp">binary exponent</param>
        /// <param name="sig">binary significand</param>
        /// <returns></returns>
        public static uint PackToF32UI(bool sign, short exp, uint sig)
            => (((sign ? 1U : 0U) << 31) + ((uint)(exp) << 23) + (sig));


    }
}
