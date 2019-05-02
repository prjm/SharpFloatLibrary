/*
 *    This library is a port of the Dragon4 algorithm of Ryan Juckett.
 *    This is not the original software library -- it is an adapted version for C#.
 *
 *    Copyright 2018, 2019 Bastian Turcs. All rights reserved.
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


namespace SharpFloat.Helpers {

    public static partial class ULongHelpers {

        /// <summary>
        ///     compute log2 of a given ulong value
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static uint LogBase2(this ulong val) {
            var temp = val >> 32;
            if (temp != 0)
                return 32 + ((uint)temp).LogBase2();

            return ((uint)val).LogBase2();
        }


    }
}
