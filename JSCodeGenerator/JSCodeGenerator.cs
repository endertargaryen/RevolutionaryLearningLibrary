using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JSCodeGenerator
{
    public static class JSCodeGenerator
    {
		public static void Main()
		{
			StringBuilder sb = new StringBuilder();

			sb.AppendLine("var StatusCode={");
			Enum.GetValues(typeof(HttpStatusCode)).Cast<HttpStatusCode>().ToList().ForEach(n =>
			{
				sb.AppendLine($"{n}: {(int)n},");
			});
			sb.AppendLine("}");

			File.WriteAllText("temp.txt", sb.ToString());

			Process.Start("temp.txt");
		}
    }
}
