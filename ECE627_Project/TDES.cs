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
using System.Linq;
using System.Text;

namespace ECE627_Project
{
    /// <summary>
    /// My Trippple DES implementation 
    /// </summary>
    public class TDES
    {
        private DES des1, des2, des3;
        public TDES()
        {
            des1 = new DES();
            des2 = new DES();
            des3 = new DES();
        }

        private string AddPadding(string msg)
        {
            int numberPaddingBytes = 8 - (msg.Length % 8);
            string new_message = msg + '1';
            for (int i = 1; i < numberPaddingBytes; i++)
            {
                new_message += '0';
            }
            return new_message;
        }

        private string RemovePadding(string msg)
        {
            bool found = false;
            int i = msg.Length - 1;
            for (int j = 0; i >= 0 && j < 8; j++, i--)
            {
                if (msg[i] == '0')
                {
                    // normal padding
                }
                else if (msg[i] == '1')
                {
                    // start of padding then break
                    found = true;
                    break;
                }
                else
                {
                    // error 
                    break;
                }
            }
            if (found)
            {
                return msg.Substring(0,i);
            }
            else
            {
                return "Error found in removing padding!";
            }
        }

        /// <summary>
        /// ciphertext = EK3(DK2(EK1(plaintext)))
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public string Encrypt(string text)
        {
            byte[] msg = UTF32Encoding.UTF32.GetBytes(AddPadding(text));
            return Base64.Encode(des3.DES_Encrypt(des2.DES_Decrypt(des1.DES_Encrypt(msg))));
            //return AddPadding(msg);
        }

        public string Decrypt(string cipher)
        {
            byte[] data = Base64.Decode(cipher);
            data = des3.DES_Decrypt(des2.DES_Encrypt(des1.DES_Decrypt(data)));
            string msgWithPadding = UTF32Encoding.UTF32.GetString(data);
            return RemovePadding(msgWithPadding);
            //return RemovePadding(cipher);
        }
        public void InitTDES(string key)
        {
            byte[] kb = UTF32Encoding.UTF32.GetBytes(key);
            byte []k1 = new byte[8];
            byte []k2 = new byte[8];
            byte []k3 = new byte[8];
            Array.Copy(kb, 0, k1, 0, 8);
            des1.generateSubKeys(k1);
            Array.Copy(kb, 8, k2, 0, 8);
            des1.generateSubKeys(k2);
            Array.Copy(kb, 16, k3, 0, 8);
            des1.generateSubKeys(k3);
        }
    }
}
