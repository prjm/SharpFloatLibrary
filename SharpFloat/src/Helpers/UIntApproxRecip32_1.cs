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

namespace SharpFloat.Helpers {

    /// <summary>
    ///     helper functions for unsigned integers
    /// </summary>
    public static partial class UIntHelper {

        private static readonly ushort[] approxRecip_1k0s = new ushort[16] {
            0xFFC4, 0xF0BE, 0xE363, 0xD76F, 0xCCAD, 0xC2F0, 0xBA16, 0xB201,
            0xAA97, 0xA3C6, 0x9D7A, 0x97A6, 0x923C, 0x8D32, 0x887E, 0x8417
        };

        private static readonly ushort[] approxRecip_1k1s = new ushort[16] {
            0xF0F1, 0xD62C, 0xBFA1, 0xAC77, 0x9C0A, 0x8DDB, 0x8185, 0x76BA,
            0x6D3B, 0x64D4, 0x5D5C, 0x56B1, 0x50B6, 0x4B55, 0x4679, 0x4211
        };


        /// <summary>
        /// Returns an approximation to the reciprocal of the number represented by `a',
        /// where `a' is interpreted as an unsigned fixed-point number with one integer
        /// bit and 31 fraction bits.  The `a' input must be "normalized", meaning that
        /// its most-significant bit (bit 31) must be 1.  Thus, if A is the value of
        /// the fixed-point interpretation of `a', then 1 &lt;= A &lt; 2.  The returned value
        /// is interpreted as a pure unsigned fraction, having no integer bits and 32
        /// fraction bits.  The approximation returned is never greater than the true
        /// reciprocal 1/A, and it differs from the true reciprocal by at most 2.006 ulp
        /// (units in the last place).
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static uint ApproxRecip32(this uint a) {
            uint index;
            ushort eps, r0;
            uint sigma0;
            uint r;
            uint sqrSigma0;

            index = a >> 27 & 0xF;
            eps = (ushort)(a >> 11);
            r0 = (ushort)(approxRecip_1k0s[index] - ((approxRecip_1k1s[index] * (uint)eps) >> 20));
            sigma0 = ~(uint)((r0 * (ulong)a) >> 7);
            r = (uint)(((uint)r0 << 16) + ((r0 * (ulong)sigma0) >> 24));
            sqrSigma0 = (uint)(((ulong)sigma0 * sigma0) >> 32);
            r += (uint)((r * (ulong)sqrSigma0) >> 48);
            return r;
        }

    }

}
