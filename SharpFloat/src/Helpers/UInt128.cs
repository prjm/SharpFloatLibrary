﻿/*  This library is a port of the standard softfloat library, Release 3e, from John Hauser to C#.
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

using System;
using System.Diagnostics;
using SharpFloat.FloatingPoint;

namespace SharpFloat.Helpers {

    /// <summary>
    ///     128-bit unsigned integer helper structure
    /// </summary>
    [DebuggerDisplay("v64 = {v64}, v0 = {v0}")]
    public readonly partial struct UInt128 : IEquatable<UInt128> {

        /// <summary>
        ///     lower half (bits 0 to 63)
        /// </summary>
        public readonly ulong v0;

        /// <summary>
        ///     upper half (bits 64 to 127)
        /// </summary>
        public readonly ulong v64;

        /// <summary>
        ///     create a new 128-bit value
        /// </summary>
        /// <param name="a64">upper half</param>
        /// <param name="a0">lower half</param>
        public UInt128(ulong a64, ulong a0) {
            v64 = a64;
            v0 = a0;
        }

        /// <summary>
        ///     create a new 128-bit value
        /// </summary>
        /// <param name="a"></param>
        public UInt128(ExtF80 a) : this(a.signExp, a.signif) { }

        /// <summary>
        ///     compare for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            return obj is UInt128 value && Equals(value);
        }

        /// <summary>
        ///     compare for equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(UInt128 other) {
            return (v64 == other.v64) && (v0 == other.v0);
        }

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            return unchecked((v64.GetHashCode() * 397) ^ v0.GetHashCode());
        }

        /// <summary>
        ///     compare for equality
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(UInt128 left, UInt128 right) {
            return left.Equals(right);
        }

        /// <summary>
        ///     compare for inequality
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(UInt128 left, UInt128 right) {
            return !(left == right);
        }
    }
}
