using System.Linq;
using System.Net;

namespace BraveBrowser.Helpers
{
	public static class StringUtils
	{
		public static string Clear(string text)
		{
			return !string.IsNullOrEmpty(text) ? text.ToLower().Trim() : string.Empty;
		}

		public static bool IsIPv4(this string ipString)
		{
			if (ipString.Count(c => c == '.') != 3)
				return false;

			return IPAddress.TryParse(ipString, out IPAddress address);
		}
	}
}