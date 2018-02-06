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

using SharpFloat.Globals;

namespace SharpFloat.Helpers {

    public partial struct UInt128 {

        public static UInt128 PropagateNaNExtF80UI(ushort uiA64, ulong uiA0, ushort uiB64, ulong uiB0) {
            var isSigNaNA = ExtF80.ExtF80.IsSigNaNExtF80UI(uiA64, uiA0);
            var isSigNaNB = ExtF80.ExtF80.IsSigNaNExtF80UI(uiB64, uiB0);
            var uiNonsigA0 = uiA0 | 0xC000000000000000UL;
            var uiNonsigB0 = uiB0 | 0xC000000000000000UL;

            if (isSigNaNA | isSigNaNB) {
                Settings.Raise(ExceptionFlags.Invalid);
                if (isSigNaNA) {
                    if (!isSigNaNB) {
                        if (ExtF80.ExtF80.IsNaNExtF80UI(uiB64, uiB0))
                            return new UInt128(uiB64, uiNonsigB0);
                        return new UInt128(uiA64, uiNonsigA0);
                    }
                }
                else {
                    if (ExtF80.ExtF80.IsNaNExtF80UI(uiA64, uiA0))
                        return new UInt128(uiA64, uiNonsigA0);
                    return new UInt128(uiB64, uiNonsigB0);
                }
            }

            var uiMagA64 = (ushort)(uiA64 & 0x7FFF);
            var uiMagB64 = (ushort)(uiB64 & 0x7FFF);

            if (uiMagA64 < uiMagB64)
                return new UInt128(uiB64, uiNonsigB0);
            if (uiMagB64 < uiMagA64)
                return new UInt128(uiA64, uiNonsigA0);
            if (uiA0 < uiB0)
                return new UInt128(uiB64, uiNonsigB0);
            if (uiB0 < uiA0)
                return new UInt128(uiA64, uiNonsigA0);
            if (uiA64 < uiB64)
                return new UInt128(uiA64, uiNonsigA0);
            return new UInt128(uiB64, uiNonsigB0);
        }
    }
}
