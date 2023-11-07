using BraveBrowser.ApiModels;
using System;
using System.Text;

namespace BraveBrowser.Helpers
{
	public static class ContextUtils
	{
		public static string Base64Encode(this string plainText)
		{
			var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
			return Convert.ToBase64String(plainTextBytes);
		}

		public static string Base64Decode(this string base64EncodedData)
		{
			var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
			return Encoding.UTF8.GetString(base64EncodedBytes);
		}

		public static string HmaIP { get; set; }

		public static string TimeAgo(this DateTime dateTime)
		{
			string result = string.Empty;
			var timeSpan = DateTime.Now.Subtract(dateTime);

			if (timeSpan <= TimeSpan.FromSeconds(60))
			{
				result = string.Format("{0} giây trước", timeSpan.Seconds);
			}
			else if (timeSpan <= TimeSpan.FromMinutes(60))
			{
				result = timeSpan.Minutes > 1 ? String.Format("{0} phút trước", timeSpan.Minutes) : "1 phút trước";
			}
			else if (timeSpan <= TimeSpan.FromHours(24))
			{
				result = timeSpan.Hours > 1 ? String.Format("{0} giờ trước", timeSpan.Hours) : "1 giờ trước";
			}
			else if (timeSpan <= TimeSpan.FromDays(30))
			{
				result = timeSpan.Days > 1 ? String.Format("{0} ngày trước", timeSpan.Days) : "hôm qua";
			}
			else if (timeSpan <= TimeSpan.FromDays(365))
			{
				result = timeSpan.Days > 30 ? String.Format("{0} tháng trước", timeSpan.Days / 30) : "1 tháng trước";
			}
			else
			{
				result = timeSpan.Days > 365 ? String.Format("{0} năm trước", timeSpan.Days / 365) : "1 năm trước";
			}

			return result;
		}
	}
}