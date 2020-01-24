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
using System.IO;

namespace ECE627_Project
{
    class Controller
    {
        private static Controller instance = null;
        TDES fileEncryptor, dataEncryptor;
        UserData userData;
        string UserName;
        private Controller()
        {
            fileEncryptor = new TDES();
            dataEncryptor = new TDES();
        }
        public static Controller GetInstance()
        {
            if (instance == null)
            {
                instance = new Controller();
            }
            return instance;
        }
        public void initEncryptors(string key1, string key2)
        {
            fileEncryptor.InitTDES(key1);
            dataEncryptor.InitTDES(key2);
        }
        public bool SignUp(string userName)
        {
            try
            {
                UserName = userName;
                string fileName = userName + ".pswds";
                if(File.Exists(fileName))
                {
                    return false;
                }
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    string userNameHash = HashingSHA.Hash(userName);
                    userData = new UserData();
                    userData.UserNameHashed = userNameHash;
                    writer.Write(fileEncryptor.Encrypt(userData.ToString()));
                    writer.Close();
                    return SignIn(userName);
                }
            }
            catch (Exception ioexc)
            {
                Console.Write("Error reading the file");
            }
            return false;
        }
        public bool SignIn(string userName)
        {
            try
            {
                UserName = userName;
                using (StreamReader reader = new StreamReader(userName + ".pswds"))
                {
                    string xml = reader.ReadToEnd();
                    userData = UserData.LoadFromXMLString(fileEncryptor.Decrypt(xml));
                    if (HashingSHA.Hash(userName) == userData.UserNameHashed)
                    {
                        return true;
                    }
                    reader.Close();
                }
            }
            catch (Exception ioexc)
            {
                Console.Write("Error reading the file");
            }
            return false;
        }
        public void AddEntry(string site, string user, string password)
        {
            userData.Passwords.Add(new WebsiteData
            {
                Name = site,
                UserName = dataEncryptor.Encrypt(user),
                Password = dataEncryptor.Encrypt(password)
            });
            using (StreamWriter writer = new StreamWriter(UserName + ".pswds"))
            {
                writer.Write(fileEncryptor.Encrypt(userData.ToString()));
                writer.Close();
            }
        }
        public WebsiteData GetEntry(int index,bool reveal = false)
        {
            if (index >= 0 && index < userData.Passwords.Count)
            {
                if (reveal)
                {
                    return new WebsiteData
                    {
                        Name = userData.Passwords[index].Name,
                        UserName = userData.Passwords[index].ViewUserName(dataEncryptor),
                        Password = userData.Passwords[index].ViewPassword(dataEncryptor)
                    };
                }
                else
                {
                    return userData.Passwords[index];
                }
            }
            return null;
        }
        public List<string> GetEntries()
        {
            List<string> list = new List<string>();
            for (int i = 0; i < userData.Passwords.Count; i++)
            {
                list.Add(userData.Passwords[i].Name);
            }
            return list;
        }
    }
}
