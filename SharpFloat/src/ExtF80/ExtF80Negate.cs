/*  This library is part of the SharpFloat library and provided under the
 *  terms of the default license. See "LICENSE_Default.txt" in the project
 *  root directory.
 *
 *  Copyright 2018, 2019 Bastian Turcs. All rights reserved.
 */

using SharpFloat.Globals;
using SharpFloat.Helpers;

namespace SharpFloat.FloatingPoint {

    public partial struct ExtF80 {

        /// <summary>
        ///     negate this value
        /// </summary>
        /// <returns></returns>
        public ExtF80 Negate()
            => -this;

        /// <summary>
        ///     negate an extended value
        /// </summary>
        /// <param name="value">value to negate</param>
        /// <returns></returns>
        public static ExtF80 operator -(in ExtF80 value) {

            if (value.IsSpecialOperand) {
                if (value.IsSignalingNaN) {
                    Settings.Raise(ExceptionFlags.Invalid);
                    return new ExtF80(value.signExp, value.signif | 0xC000000000000000);
                }
            }

            return new ExtF80(value.UnsignedExponent.PackToExtF80UI64(!value.IsNegative), value.signif);

        }
    }
}
