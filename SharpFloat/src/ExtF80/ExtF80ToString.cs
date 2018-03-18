/*
 *    This library is a port of the Dragon4 algorithm of Ryan Juckett.
 *    This is not the original software library -- it is an adapted version for C#.
 *
 *    Copyright 2018 Bastian Turcs. All rights reserved.
 *
 */
/******************************************************************************
 Copyright (c) 2014 Ryan Juckett
 http://www.ryanjuckett.com/

 This software is provided 'as-is', without any express or implied
 warranty. In no event will the authors be held liable for any damages
 arising from the use of this software.

 Permission is granted to anyone to use this software for any purpose,
 including commercial applications, and to alter it and redistribute it
 freely, subject to the following restrictions:

 1. The origin of this software must not be misrepresented; you must not
    claim that you wrote the original software. If you use this software
    in a product, an acknowledgment in the product documentation would be
    appreciated but is not required.

 2. Altered source versions must be plainly marked as such, and must not be
    misrepresented as being the original software.

 3. This notice may not be removed or altered from any source
    distribution.
******************************************************************************/

using System;
using SharpFloat.Globals;
using SharpFloat.Helpers;

namespace SharpFloat.FloatingPoint {

    /// <summary>
    ///     cutoff-mode for formatting
    /// </summary>
    public enum FormatCutoffMode : byte {

        /// <summary>
        ///     all digits required for a unique number
        /// </summary>
        Unique = 0,

        /// <summary>
        ///     cutoff after a fixed number of digits
        /// </summary>
        TotalLength = 1,

        /// <summary>
        ///     cutoff after a fixed number of fractional digits
        /// </summary>
        FractionLength = 2

    }

    public partial struct ExtF80 {




    }
}
