using System;

namespace Challenge
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
			var deciphered = decipher("Yvzs! I xzm'g yvorvev Lzmxv olhg srh qly zg gsv xlolmb!!");
			Console.WriteLine(deciphered);
			Console.ReadLine();
		}

		static string decipher(string cipher)
		{
			const int asciiOfa = 97;
			const int asciiOfz = 97 + 25;

			var deciphered = "";
			foreach (char ch in cipher)
			{
				var newChar = ch;
				var asciiOfch = (int)ch;
				if ( asciiOfch >= asciiOfa && asciiOfch <= asciiOfz) // is lowercase alphabet char
				{
					var newCharAscii = asciiOfz - asciiOfch + asciiOfa;
					newChar = (char)(newCharAscii);
				}

				deciphered += newChar;
				Console.WriteLine($"{ch} => {newChar}");
				//Console.WriteLine(ch);
			}
			return deciphered;
		}
	}
}
