﻿/*
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
    public partial class Main_Window : Form
    {
        public Main_Window()
        {
            InitializeComponent();
            listBox1.Items.Clear();
            listBox1.Items.AddRange(Controller.GetInstance().GetEntries().ToArray());
        }

        private void add_button_Click(object sender, EventArgs e)
        {
            NewData newData = new NewData();
            newData.ShowDialog();
            listBox1.Items.Clear();
            listBox1.Items.AddRange(Controller.GetInstance().GetEntries().ToArray());
        }

        private void viewButton_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                ViewData viewData = 
                    new ViewData(Controller.GetInstance().GetEntry(listBox1.SelectedIndex, true));
                viewData.ShowDialog();
            }
        }

        private void exit_button_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
