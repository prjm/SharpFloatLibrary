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


using System;
using SharpFloat.Globals;

namespace SharpFloat.FloatingPoint {

    public partial struct ExtF80 : IEquatable<ExtF80> {

        public static bool operator ==(ExtF80 l, ExtF80 r) {
            if (IsNaNExtF80UI(l.signExp, l.signif) || IsNaNExtF80UI(r.signExp, r.signif)) {

                if (IsSigNaNExtF80UI(l) || IsSigNaNExtF80UI(r)) {
                    Settings.Raise(ExceptionFlags.Invalid);
                }

                return false;
            }

            return //
                (l.signif == r.signif)
                && ((l.signExp == r.signExp) || (l.signif == 0 && 0 == ((l.signExp | r.signExp) & 0x7FFF)));
        }

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            if (obj is ExtF80 e)
                return e == this;
            return false;
        }

        public bool Equals(ExtF80 other)
            => this == other;

        public override int GetHashCode()
            => (signExp.GetHashCode() * 397) ^ signif.GetHashCode();
    }

}
