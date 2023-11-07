//using BraveBrowser.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace BraveBrowser.Helpers
//{
//	public static class SettingUtils
//	{
//		public static string GetValue(string key)
//		{
//			using (var db = new DataContext())
//			{
//				return db.settings.FirstOrDefault(x => x.key.Equals(key)).value;
//			}
//		}

//		public static void SetValue(string key, string value)
//		{
//			using (var db = new DataContext())
//			{
//				var item = db.settings.FirstOrDefault(x => x.key.Equals(key));
//				if (item != null)
//				{
//					item.value = value;
//					db.SaveChanges();
//				}
//			}
//		}
//	}
//}