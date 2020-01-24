/*
 * Copyright (c) 2015, mostafaizz
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

* Redistributions of source code must retain the above copyright notice, this
  list of conditions and the following disclaimer.

* Redistributions in binary form must reproduce the above copyright notice,
  this list of conditions and the following disclaimer in the documentation
  and/or other materials provided with the distribution.

* Neither the name of passwordsafe nor the names of its
  contributors may be used to endorse or promote products derived from
  this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

 */

using System;
using System.Collections.Generic;
using System.Text;

namespace ECE627_Project
{
    /// <summary>
    /// My DES implementation
    /// </summary>
    public class DES
    {

        // initial permutation
        uint[] IP = new uint[]{ 58, 50, 42, 34, 26, 18, 10, 2,
				  60, 52, 44, 36, 28, 20, 12, 4,
				  62, 54, 46, 38, 30, 22, 14, 6,
				  64, 56, 48, 40, 32, 24, 16, 8,
	    		  57, 49, 41, 33, 25, 17, 9, 1,
				  59, 51, 43, 35, 27, 19, 11, 3,
				  61, 53, 45, 37, 29, 21, 13, 5,
				  63, 55, 47, 39, 31, 23, 15, 7};
        // inverse initial permutation
        uint[] IP_1 = new uint[]{ 40, 8, 48, 16, 56, 24, 64, 32,
				  39, 7, 47, 15, 55, 23, 63, 31,
		   		  38, 6, 46, 14, 54, 22, 62, 30,
				  37, 5, 45, 13, 53, 21, 61, 29,
				  36, 4, 44, 12, 52, 20, 60, 28,
				  35, 3, 43, 11, 51, 19, 59, 27,
				  34, 2, 42, 10, 50, 18, 58, 26,
				  33, 1, 41, 9, 49, 17, 57, 25};
        // expand permutation
        uint[] E = new uint[] { 32,  1,  2,  3,  4,  5,
				 4 ,  5,  6,  7,  8,  9,
				 8 ,  9, 10, 11, 12, 13,
				 12, 13, 14, 15, 16, 17,
				 16, 17, 18, 19, 20, 21,
				 20, 21, 22, 23, 24, 25,
				 24, 25, 26, 27, 28, 29,
				 28, 29, 30, 31, 32, 1};
        uint[] P = new uint[] {16,  7, 20, 21,
				29, 12, 28, 17,
				1 , 15, 23, 26,
				5 , 18, 31, 10,
				2 ,  8, 24, 14,
				32, 27,  3,  9,
				19, 13, 30,  6,
				22, 11,  4, 25};
        // sand boxes
        uint[] S1 = new uint[] {14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7,
				0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8,
				4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0,
				15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6, 13};
        //
        uint[] S2 = new uint[] {15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10,
				3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5,
				0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15,
				13, 8, 10, 1, 3, 15 ,4 ,2 ,11, 6, 7 ,12, 0, 5, 14, 9};
        //
        uint[] S3 = new uint[] {10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8,
				13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1,
				13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7,
				1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12};
        //
        uint[] S4 = new uint[] {7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15,
				13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9,
				10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4,
				3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14};
        //
        uint[] S5 = new uint[] {2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9,
				14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6,
				4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14,
				11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3};
        //
        uint[] S6 = new uint[] {12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11,
				10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8,
				9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6,
				4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13};
        //
        uint[] S7 = new uint[] {4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1,
				13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6,
				1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2,
				6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12};
        //
        uint[] S8 = new uint[] {13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7,
				1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2,
				7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3 ,5, 8,
				2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11};
        ulong[] Ki = new ulong[16];

        ulong mask_Shift(ulong src, uint pos, uint dst)
        {
            ulong temp = 0x8000000000000000;
            temp = (temp >> (int)pos);
            temp &= src;
            if (dst > pos)
            {
                temp >>= (int)(dst - pos);
            }
            else if (pos > dst)
            {
                temp <<= (int)(pos - dst);
            }
            return temp;
        }

        ulong permute(ulong src, uint[] p, uint n)
        {
            ulong temp = 0;
            for (uint i = 0; i < n; i++)
            {
                temp |= mask_Shift(src, (uint)(p[i] - 1), i);
            }
            return temp;
        }
        //

        //
        public void generateSubKeys(byte[] k)
        {
            uint[] PC_1 = new uint[]{57,49 ,41 ,33, 25, 17, 9,
						1, 58, 50, 42, 34, 26, 18,
						10, 2, 59, 51, 43, 35, 27,
						19, 11, 3, 60, 52, 44, 36,
						63, 55, 47, 39, 31, 23, 15,
						7, 62, 54, 46, 38, 30, 22,
						14, 6, 61, 53, 45, 37, 29,
						21, 13, 5, 28, 20, 12, 4};
            //
            int[] iter = new int[] { 1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1 };
            //
            uint[] PC_2 = new uint[]{14, 17, 11, 24, 1, 5,
								3, 28, 15, 6, 21, 10,
								23, 19, 12, 4, 26, 8,
								16, 7, 27, 20, 13, 2,
								41, 52, 31, 37, 47, 55,
								30, 40, 51, 45, 33, 48,
								44, 49, 39, 56, 34, 53,
								46, 42, 50, 36, 29, 32};

            ulong key = 0;
            for (int i = 0; i < 8; i++)
            {
                key = ((key << 8) | k[i]);
            }

            key = permute(key, PC_1, 56);
            //
            uint l = (((uint)(key >> 36)) << 4);
            uint r = (((uint)((key >> 8) & 0x000000000fffffff)) << 4);

            uint[] rotTemp = new uint[] { 0x80000000, 0xc0000000 };
            for (int i = 0; i < 16; i++)
            {
                int tt = iter[i];
                // rotation
                l = (((rotTemp[tt - 1] & l) >> (int)(28 - tt)) | (l << tt));
                r = (((rotTemp[tt - 1] & r) >> (int)(28 - tt)) | (r << tt));
                // pc_2
                ulong tmp = l;
                tmp = (((tmp << 24) | ((ulong)r >> 4)));
                tmp <<= 8;
                tmp = permute(tmp, PC_2, 48);
                // save K[i]
                Ki[i] = tmp;
            }
            // 
        }

        char getBlock(char x1)
        {
            char temp;
            temp = (char)((x1 & 1) << 4);
            temp = (char)((x1 & 0x20) | temp);
            x1 = (char)(((x1 >> 1) & 0x0f) | temp);
            return x1;
        }

        uint f(uint x, ulong y)
        {
            ulong tmp = x;
            tmp <<= 32;
            tmp = permute(tmp, E, 48);
            tmp = tmp ^ y;
            // 
            uint res = 0;
            char x1;
            // S1
            x1 = (char)(tmp >> 58);
            tmp = (tmp << 6);
            x1 = getBlock(x1);
            res = ((res << 4) | S1[x1]);
            // S2
            x1 = (char)(tmp >> 58);
            tmp = (tmp << 6);
            x1 = getBlock(x1);
            res = ((res << 4) | S2[x1]);
            // S3
            x1 = (char)(tmp >> 58);
            tmp = (tmp << 6);
            x1 = getBlock(x1);
            res = ((res << 4) | S3[x1]);
            // S4
            x1 = (char)(tmp >> 58);
            tmp = (tmp << 6);
            x1 = getBlock(x1);
            res = ((res << 4) | S4[x1]);
            // S5
            x1 = (char)(tmp >> 58);
            tmp = (tmp << 6);
            x1 = getBlock(x1);
            res = ((res << 4) | S5[x1]);
            // S6
            x1 = (char)(tmp >> 58);
            tmp = (tmp << 6);
            x1 = getBlock(x1);
            res = ((res << 4) | S6[x1]);
            // S7
            x1 = (char)(tmp >> 58);
            tmp = (tmp << 6);
            x1 = getBlock(x1);
            res = ((res << 4) | S7[x1]);
            // S8
            x1 = (char)(tmp >> 58);
            tmp = (tmp << 6);
            x1 = getBlock(x1);
            res = ((res << 4) | S8[x1]);
            //
            ulong ll = res;
            ll <<= 32;
            ll = permute(ll, P, 32);
            res = (uint)(ll >> 32);
            return res;
        }

        ulong encBlock(ulong b)
        {
            b = permute(b, IP, 64);
            uint L = (uint)((b & 0xffffffff00000000) >> 32);
            uint R = (uint)(b & 0x00000000ffffffff);
            for (int i = 0; i < 16; i++)
            {
                uint Ltmp = L;
                L = R;

                R = Ltmp ^ f(R, Ki[i]);
            }
            b = 0; b = R; b = ((b << 32) | L);
            b = permute(b, IP_1, 64);
            return b;
        }

        ulong decBlock(ulong b)
        {

            b = permute(b, IP, 64);
            uint L = (uint)((b & 0xffffffff00000000) >> 32);
            uint R = (uint)(b & 0x00000000ffffffff);
            for (int i = 15; i >= 0; i--)
            {
                uint Ltmp = L;
                L = R;
                R = Ltmp ^ f(R, Ki[i]);
            }
            b = 0; b = R; b = ((b << 32) | L);
            b = permute(b, IP_1, 64);
            return b;
        }
        /// <summary>
        /// encrypt and decrypt
        /// </summary>
        /// <param name="txt">input data</param>
        /// <param name="flag">if true then encrypt else decrypt</param>
        /// <returns></returns>
        byte[] encode_decode(byte[] txt, bool flag)
        {
            ulong n = (ulong)txt.Length;
            List<byte> result = new List<byte>();
            for (ulong i = 0; i < n; i += 8)
            {
                ulong tmp = 0;
                for (ulong j = i; j < i + 8; j++)
                {
                    ulong ttt = (ulong)txt[(int)j];
                    tmp = ((tmp << 8) | (ttt & 0x00000000000000ff));
                }
                if (flag)
                {
                    tmp = encBlock(tmp);
                }
                else
                {
                    tmp = decBlock(tmp);
                }
                for (int j = 7; j >= 0; j--)
                {
                    byte x = (byte)((tmp & 0xff00000000000000) >> 56);
                    result.Add(x);
                    tmp <<= 8;
                }
            }
            return result.ToArray();
        }
        public byte[] DES_Encrypt(byte[] text)
        {
            return encode_decode(text, true);
        }
        public byte[] DES_Decrypt(byte[] text)
        {
            return encode_decode(text, false);
        }
    }

}