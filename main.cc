#include <iostream>
#include <cstdlib>
#include <algorithm>
#include <bitset>
#include <vector>
#include <string>
#include <sstream>
#include <iomanip>
#include <map>
#include "DES.h"

using namespace std;

int main()
{
	cout << "Hello World" << endl;
	DES des;
	des.DES_Init_Key("asdafasfafsasf");
	string out = des.DES_Encrypt("Hello there");
	cout << out << endl;
	string out2 = des.DES_Decrypt(out);
	cout << out2 << endl;
	return 0;
}
