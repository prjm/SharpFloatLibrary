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

namespace SharpFloat.Helpers {

    public partial class BigInt {



        public static uint DivideWithRemainder_MaxQuotient9(BigInt dividend, BigInt divisor) {

            // Check that the divisor has been correctly shifted into range and that it is not
            // smaller than the dividend in length.
            if (divisor.Zero || divisor[divisor.Length - 1] < 8 || divisor[divisor.length] >= 0xFFFFFFFF || dividend.Length > divisor.Length)
                throw new System.ArgumentOutOfRangeException();

            // If the dividend is smaller than the divisor, the quotient is zero and the divisor is already
            // the remainder.
            var length = divisor.Length;
            if (dividend.Length < divisor.Length)
                return 0;

            var pFinalDivisorBlock = length - 1;
            var pFinalDividendBlock = length - 1;

            // Compute an estimated quotient based on the high block value. This will either match the actual quotient or
            // undershoot by one.
            var quotient = dividend[pFinalDividendBlock] / divisor[pFinalDivisorBlock + 1];
            if (quotient > 9)
                throw new System.InvalidOperationException();

            // Divide out the estimated quotient
            if (quotient != 0) {

                dividend.Length = length;

                // dividend = dividend - divisor*quotient
                var pDivisorCur = 0U;
                var pDividendCur = 0U;

                var borrow = 0UL;
                var carry = 0UL;

                do {
                    var product = (ulong)divisor[pDivisorCur] * quotient + carry;
                    carry = product >> 32;

                    var difference = (ulong)dividend[pDividendCur] - (product & 0xFFFFFFFF) - borrow;
                    borrow = (difference >> 32) & 1;

                    dividend[pDividendCur] = (uint)(difference & 0xFFFFFFFF);

                    ++pDivisorCur;
                    ++pDividendCur;
                } while (pDivisorCur <= pFinalDivisorBlock);

                // remove all leading zero blocks from dividend
                while (length > 0 && dividend[length - 1] == 0)
                    --length;

                dividend.Length = length;
            }

            // If the dividend is still larger than the divisor, we overshot our estimate quotient. To correct,
            // we increment the quotient and subtract one more divisor from the dividend.
            if (Compare(dividend, divisor) >= 0) {
                ++quotient;

                // dividend = dividend - divisor
                var pDivisorCur = 0U;
                var pDividendCur = 0U;

                var borrow = 0UL;
                do {
                    var difference = (ulong)dividend[pDividendCur] - divisor[pDivisorCur] - borrow;
                    borrow = (difference >> 32) & 1;


                    dividend[pDividendCur] = (uint)(difference & 0xFFFFFFFF);

                    ++pDivisorCur;
                    ++pDividendCur;
                } while (pDivisorCur <= pFinalDivisorBlock);

                // remove all leading zero blocks from dividend
                while (length > 0 && dividend[length - 1] == 0)
                    --length;

                dividend.Length = length;
            }

            return quotient;
        }

    }
}
