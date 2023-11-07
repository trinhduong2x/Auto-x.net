using System;
using System.Configuration;

namespace BraveBrowser.Helpers
{
	public static class Constants
	{
		public static int OfflineCycleTime => Convert.ToInt32(ConfigurationManager.AppSettings["OfflineCycleTime"]);

		public const string COOKIE_FRONTEND_KEY_USERID = "mid";
		public const string COOKIE_FRONTEND_KEY_EMAIL = "e";
		public const string COOKIE_FRONTEND_IS_ADMIN = "adm";

		public const string RobotTipPath = "/robots/tips";
		public const string RobotTipPath_Proxy = "/robots/tips_proxy";
		public const string RobotAdsPath = "/robots/ads";
		public const string RobotAdsPath_Proxy = "/robots/ads_proxy";
		public const string ConfigsFolderPath = "/configs";
		public const int BRAVE_TOP_FIVE_TY = 50;
		public const int BRAVE_TOP_FIVE = 5;

		public const string AdminCookieName = "AdminLogin";

		public static class SettingTypes
		{
			public const string BraveAds = "brave_ads";
			public const string BraveTip = "brave_tip";
			public const string Mind = "mind_feed";
		}

		public static class TonActionTypes
		{
			public const string Follow = "follow";
			public const string Post = "post";
			public const string NewsFeed = "news_feed"; // lướt news feed
			public const string Message = "message";
			public const string SendTip = "send_tip";
		}

		public static class RedditActionType
		{
			public const string Comment = "comment";
			public const string Like = "like";
		}

		public static class Error
		{
			public const string ERROR_TIP_00 = "ERROR_TIP_00"; // Lỗi: Exception
			public const string ERROR_TIP_01 = "ERROR_TIP_01"; // Lỗi: không còn Link để tip
			public const string ERROR_TIP_02_NO_WALLET = "ERROR_TIP_02_NO_WALLET"; // Lỗi: không còn ví để add vào kênh
			public const string ERROR_TIP_03_NOT_FOUND_ACCOUNT_FOR_ADD_WALLET = "ERROR_TIP_03_NOT_FOUND_ACCOUNT_FOR_ADD_WALLET"; // Lỗi: Không còn tài khoản chưa gắn ví hoặc die ví
		}

		public enum XActionType { 
			Follow = 1,
			Comment = 2, 
			Like = 3,
			Reweet = 4,
			Views = 5,
			ClickAds = 6,
			Bookmark = 7,
			Tweet = 8,
			Message = 9
		}
	}
}