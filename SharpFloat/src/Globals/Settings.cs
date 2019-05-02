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

using System.Threading;

[assembly: System.CLSCompliant(false)]

namespace SharpFloat.Globals {

    /// <summary>
    ///     global settings
    /// </summary>
    public static class Settings {

        private static readonly ThreadLocal<DetectTininess> detectTininess
            = new ThreadLocal<DetectTininess>(() => DetectTininess.AfterRounding);

        /// <summary>
        ///     detect tininess
        /// </summary>
        public static DetectTininess DetectTininess
            => detectTininess.Value;

        /// <summary>
        ///     rounding mode
        /// </summary>
        private static readonly ThreadLocal<RoundingMode> roundingMode
            = new ThreadLocal<RoundingMode>(() => RoundingMode.NearEven);

        /// <summary>
        ///     roding mode
        /// </summary>
        public static RoundingMode RoundingMode {
            get => roundingMode.Value;
            set => roundingMode.Value = value;
        }

        /// <summary>
        ///     exception flags
        /// </summary>
        private static readonly ThreadLocal<ExceptionFlags> flags
            = new ThreadLocal<ExceptionFlags>(() => ExceptionFlags.None);

        /// <summary>
        ///     flags
        /// </summary>
        public static ExceptionFlags Flags
            => flags.Value;

        /// <summary>
        ///     raise a flag
        /// </summary>
        /// <param name="newFlag">flag to set</param>
        public static void Raise(ExceptionFlags newFlag) {
            flags.Value |= newFlag;
        }

        /// <summary>
        ///     clear all flags
        /// </summary>
        public static void ClearFlags() {
            flags.Value = ExceptionFlags.None;
        }

        private static readonly ThreadLocal<byte> extF80_roundingPrecision
            = new ThreadLocal<byte>(() => 80);

        /// <summary>
        ///     used rounding precision
        /// </summary>
        public static byte ExtF80RoundingPrecision
            => extF80_roundingPrecision.Value;
    }

}
