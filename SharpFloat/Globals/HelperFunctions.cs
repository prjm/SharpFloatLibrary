﻿/*  This library is a port of the standard softfloat library, Release 3e, from John Hauser to C#.
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

namespace SharpFloat.Globals {

    public static class HelperFunctions {

        public static byte CountLeadingZeroes(this ulong a) {

            byte count;
            uint a32;

            count = 0;
            a32 = (uint)(a >> 32);
            if (a32 == 0) {
                count = 32;
                a32 = (uint)a;
            }

            if (a32 < 0x10000) {
                count += 16;
                a32 <<= 16;
            }

            if (a32 < 0x1000000) {
                count += 8;
                a32 <<= 8;
            }

            count += ((byte)(a32 >> 24)).CountLeadingZeroes();
            return count;

        }

        public static byte CountLeadingZeroes(this uint value) {
            byte count = 0;
            if (value < 0x10000) {
                count = 16;
                value <<= 16;
            }
            if (value < 0x1000000) {
                count += 8;
                value <<= 8;
            }
            count += CountLeadingZeroes((byte)(value >> 24));
            return count;
        }

        private static byte[] lookup = new byte[256] {
            /* 000 = */ 8,
            /* 001 = */ 7,
            /* 002 = */ 6,
            /* 003 = */ 6,
            /* 004 = */ 5,
            /* 005 = */ 5,
            /* 006 = */ 5,
            /* 007 = */ 5,
            /* 008 = */ 4,
            /* 009 = */ 4,
            /* 010 = */ 4,
            /* 011 = */ 4,
            /* 012 = */ 4,
            /* 013 = */ 4,
            /* 014 = */ 4,
            /* 015 = */ 4,
            /* 016 = */ 3,
            /* 017 = */ 3,
            /* 018 = */ 3,
            /* 019 = */ 3,
            /* 020 = */ 3,
            /* 021 = */ 3,
            /* 022 = */ 3,
            /* 023 = */ 3,
            /* 024 = */ 3,
            /* 025 = */ 3,
            /* 026 = */ 3,
            /* 027 = */ 3,
            /* 028 = */ 3,
            /* 029 = */ 3,
            /* 030 = */ 3,
            /* 031 = */ 3,
            /* 032 = */ 2,
            /* 033 = */ 2,
            /* 034 = */ 2,
            /* 035 = */ 2,
            /* 036 = */ 2,
            /* 037 = */ 2,
            /* 038 = */ 2,
            /* 039 = */ 2,
            /* 040 = */ 2,
            /* 041 = */ 2,
            /* 042 = */ 2,
            /* 043 = */ 2,
            /* 044 = */ 2,
            /* 045 = */ 2,
            /* 046 = */ 2,
            /* 047 = */ 2,
            /* 048 = */ 2,
            /* 049 = */ 2,
            /* 050 = */ 2,
            /* 051 = */ 2,
            /* 052 = */ 2,
            /* 053 = */ 2,
            /* 054 = */ 2,
            /* 055 = */ 2,
            /* 056 = */ 2,
            /* 057 = */ 2,
            /* 058 = */ 2,
            /* 059 = */ 2,
            /* 060 = */ 2,
            /* 061 = */ 2,
            /* 062 = */ 2,
            /* 063 = */ 1,
            /* 064 = */ 1,
            /* 065 = */ 1,
            /* 066 = */ 1,
            /* 067 = */ 1,
            /* 068 = */ 1,
            /* 069 = */ 1,
            /* 070 = */ 1,
            /* 071 = */ 1,
            /* 072 = */ 1,
            /* 073 = */ 1,
            /* 074 = */ 1,
            /* 075 = */ 1,
            /* 076 = */ 1,
            /* 077 = */ 1,
            /* 078 = */ 1,
            /* 079 = */ 1,
            /* 080 = */ 1,
            /* 081 = */ 1,
            /* 082 = */ 1,
            /* 083 = */ 1,
            /* 084 = */ 1,
            /* 085 = */ 1,
            /* 086 = */ 1,
            /* 087 = */ 1,
            /* 088 = */ 1,
            /* 089 = */ 1,
            /* 090 = */ 1,
            /* 091 = */ 1,
            /* 092 = */ 1,
            /* 093 = */ 1,
            /* 094 = */ 1,
            /* 095 = */ 1,
            /* 096 = */ 1,
            /* 097 = */ 1,
            /* 098 = */ 1,
            /* 099 = */ 1,
            /* 100 = */ 1,
            /* 101 = */ 1,
            /* 102 = */ 1,
            /* 103 = */ 1,
            /* 104 = */ 1,
            /* 105 = */ 1,
            /* 106 = */ 1,
            /* 107 = */ 1,
            /* 108 = */ 1,
            /* 109 = */ 1,
            /* 110 = */ 1,
            /* 111 = */ 1,
            /* 112 = */ 1,
            /* 113 = */ 1,
            /* 114 = */ 1,
            /* 115 = */ 1,
            /* 116 = */ 1,
            /* 117 = */ 1,
            /* 118 = */ 1,
            /* 119 = */ 1,
            /* 120 = */ 1,
            /* 121 = */ 1,
            /* 122 = */ 1,
            /* 123 = */ 1,
            /* 124 = */ 1,
            /* 125 = */ 1,
            /* 126 = */ 1,
            /* 127 = */ 1,
            /* 128 = */ 0,
            /* 129 = */ 0,
            /* 130 = */ 0,
            /* 131 = */ 0,
            /* 132 = */ 0,
            /* 133 = */ 0,
            /* 134 = */ 0,
            /* 135 = */ 0,
            /* 136 = */ 0,
            /* 137 = */ 0,
            /* 138 = */ 0,
            /* 139 = */ 0,
            /* 140 = */ 0,
            /* 141 = */ 0,
            /* 142 = */ 0,
            /* 143 = */ 0,
            /* 144 = */ 0,
            /* 145 = */ 0,
            /* 146 = */ 0,
            /* 147 = */ 0,
            /* 148 = */ 0,
            /* 149 = */ 0,
            /* 150 = */ 0,
            /* 151 = */ 0,
            /* 152 = */ 0,
            /* 153 = */ 0,
            /* 154 = */ 0,
            /* 155 = */ 0,
            /* 156 = */ 0,
            /* 157 = */ 0,
            /* 158 = */ 0,
            /* 159 = */ 0,
            /* 160 = */ 0,
            /* 161 = */ 0,
            /* 162 = */ 0,
            /* 163 = */ 0,
            /* 164 = */ 0,
            /* 165 = */ 0,
            /* 166 = */ 0,
            /* 167 = */ 0,
            /* 168 = */ 0,
            /* 169 = */ 0,
            /* 170 = */ 0,
            /* 171 = */ 0,
            /* 172 = */ 0,
            /* 173 = */ 0,
            /* 174 = */ 0,
            /* 175 = */ 0,
            /* 176 = */ 0,
            /* 177 = */ 0,
            /* 178 = */ 0,
            /* 179 = */ 0,
            /* 180 = */ 0,
            /* 181 = */ 0,
            /* 182 = */ 0,
            /* 183 = */ 0,
            /* 184 = */ 0,
            /* 185 = */ 0,
            /* 186 = */ 0,
            /* 187 = */ 0,
            /* 188 = */ 0,
            /* 189 = */ 0,
            /* 190 = */ 0,
            /* 191 = */ 0,
            /* 192 = */ 0,
            /* 193 = */ 0,
            /* 194 = */ 0,
            /* 195 = */ 0,
            /* 196 = */ 0,
            /* 197 = */ 0,
            /* 198 = */ 0,
            /* 199 = */ 0,
            /* 200 = */ 0,
            /* 201 = */ 0,
            /* 202 = */ 0,
            /* 203 = */ 0,
            /* 204 = */ 0,
            /* 205 = */ 0,
            /* 206 = */ 0,
            /* 207 = */ 0,
            /* 208 = */ 0,
            /* 209 = */ 0,
            /* 210 = */ 0,
            /* 211 = */ 0,
            /* 212 = */ 0,
            /* 213 = */ 0,
            /* 214 = */ 0,
            /* 215 = */ 0,
            /* 216 = */ 0,
            /* 217 = */ 0,
            /* 218 = */ 0,
            /* 219 = */ 0,
            /* 220 = */ 0,
            /* 221 = */ 0,
            /* 222 = */ 0,
            /* 223 = */ 0,
            /* 224 = */ 0,
            /* 225 = */ 0,
            /* 226 = */ 0,
            /* 227 = */ 0,
            /* 228 = */ 0,
            /* 229 = */ 0,
            /* 230 = */ 0,
            /* 231 = */ 0,
            /* 232 = */ 0,
            /* 233 = */ 0,
            /* 234 = */ 0,
            /* 235 = */ 0,
            /* 236 = */ 0,
            /* 237 = */ 0,
            /* 238 = */ 0,
            /* 239 = */ 0,
            /* 240 = */ 0,
            /* 241 = */ 0,
            /* 242 = */ 0,
            /* 243 = */ 0,
            /* 244 = */ 0,
            /* 245 = */ 0,
            /* 246 = */ 0,
            /* 247 = */ 0,
            /* 248 = */ 0,
            /* 249 = */ 0,
            /* 250 = */ 0,
            /* 251 = */ 0,
            /* 252 = */ 0,
            /* 253 = */ 0,
            /* 254 = */ 0,
            /* 255 = */ 0
        };

        private static byte CountLeadingZeroes(this byte v)
            => lookup[v];


    }
}
