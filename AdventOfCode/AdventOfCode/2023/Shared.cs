using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AdventOfCode._2023
{
	internal class Shared
	{
		internal static string GetFileLocation(string relativePath)
		{
			var fileLocation = new Uri(new Uri(Assembly.GetExecutingAssembly().Location), relativePath);
			return fileLocation.AbsolutePath.ToString();
		}

		internal static string[] GetFileLines(string relativePath)
		{
			var fileLocation = Shared.GetFileLocation(relativePath);
			return File.ReadAllLines(fileLocation);
		}
	}
}
