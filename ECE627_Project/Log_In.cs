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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ECE627_Project
{
    public partial class Log_In : Form
    {
        public Log_In()
        {
            InitializeComponent();
        }

        TDES tdes = new TDES();

        private void encrypt_button_Click(object sender, EventArgs e)
        {
            string hash_output = HashingSHA.Hash(pswd_textBox.Text);
            // divide into two keys
            string key1 = hash_output.Substring(0, hash_output.Length / 2);
            //MessageBox.Show(key1);
            string key2 = hash_output.Substring(hash_output.Length / 2, hash_output.Length / 2);
            //MessageBox.Show(key2);
            Controller.GetInstance().initEncryptors(key1, key2);
            if (Controller.GetInstance().SignUp(email_textBox.Text))
            {
                Main_Window mainWind = new Main_Window();
                mainWind.Show();
                this.Close(); 
            }
            else
            {
                MessageBox.Show("Please Check Your Email and Password");
            }
        }

        private void login_button_Click(object sender, EventArgs e)
        {
            
            //TDES ttt = new TDES();
            //ttt.InitTDES("12312134123testdfafasafasfasff");
            //string str = ttt.Encrypt(email_textBox.Text);
            //ttt.InitTDES("0989870987dfskd;lfkslk;;lk/.df");
            //string str1 = ttt.Decrypt(str);
            //MessageBox.Show(str);
            //MessageBox.Show(str1);
            
            string hash_output = HashingSHA.Hash(pswd_textBox.Text);
            // divide into two keys
            string key1 = hash_output.Substring(0, hash_output.Length / 2);
            //MessageBox.Show(key1);
            string key2 = hash_output.Substring(hash_output.Length / 2, hash_output.Length / 2);
            //MessageBox.Show(key2);
            Controller.GetInstance().initEncryptors(key1, key2);
            if (Controller.GetInstance().SignIn(email_textBox.Text))
            {
                Main_Window mainWind = new Main_Window();
                mainWind.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Please Check Your Email and Password");
            }
        }
    }
}
