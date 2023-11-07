//using BraveBrowser.ApiModels;
//using BraveBrowser.Logs;
//using BraveBrowser.Models;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Web.Http;

//namespace BraveBrowser.Apis
//{
//	[RoutePrefix("api/Common")]
//	public class CommonController : ApiController
//	{
//		[HttpGet]
//		[Route("GetUpdateTools")]
//		public ApiResponse GetUpdateTools(string robotPathTool)
//		{
//			var result = new ApiResponse();
//			try
//			{
//				string path = System.Web.Hosting.HostingEnvironment.MapPath($"~/Robots/{robotPathTool}");
//				var files = Directory.EnumerateFiles(path);

//				var fileArray = new List<string>();
//				foreach (var item in files)
//				{
//					fileArray.Add(Path.GetFileName(item));
//				}

//				result.IsSuccess = true;
//				result.Data = fileArray;
//			}
//			catch (Exception ex)
//			{
//				result.IsSuccess = false;
//				result.Message = ex.Message;
//			}

//			return result;
//		}

//		#region Private internet access

//		[HttpGet]
//		[Route("SearchPiaVpn")]
//		public string SearchPiaVpn(string searchText)
//		{
//			try
//			{
//				string path = System.Web.Hosting.HostingEnvironment.MapPath("~/Robots/vpn/pia_all.txt");
//				var content = File.ReadAllText(path);
//				if (!string.IsNullOrEmpty(searchText))
//					return content.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
//						.Where(x => x.ToLower().Contains(searchText?.ToLower()))
//						.OrderBy(n => Guid.NewGuid()).FirstOrDefault();
//				else
//					return content.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
//						.OrderBy(n => Guid.NewGuid()).FirstOrDefault();
//			}
//			catch (Exception ex)
//			{
//				Loggers.BraveMVCLog.Exception("Method: SearchPiaVpn", ex);
//			}

//			return null;
//		}

//		[HttpGet]
//		[Route("SearchPiaVpn")]
//		public string SearchPiaVpn()
//		{
//			return SearchPiaVpn(string.Empty);
//		}

//		#endregion Private internet access

//		#region Nord VPN

//		public string SearchTipVpn(string searchText)
//		{
//			try
//			{
//				string path = System.Web.Hosting.HostingEnvironment.MapPath("~/Robots/nordvpn/server.json");
//				var content = System.IO.File.ReadAllText(path);
//				var ipList = JsonConvert.DeserializeObject<List<NordVpnModel>>(content);
//				using (var db = new DataContext())
//				{
//					if (!string.IsNullOrEmpty(searchText))
//					{
//						var rnd = new Random();
//						var searchVpn = ipList.OrderBy(x => rnd.Next())
//							.FirstOrDefault(x => x.name.ToLower().Contains(searchText.ToLower())
//								&& !x.name.Contains("Canada")
//								&& !x.name.Contains("#97")
//								&& !x.name.Contains("#96")
//								&& !db.brave_tip.Any(y => y.brave_tip_status_id == 2 && y.vpn_name.ToLower().Contains(x.name.ToLower())));

//						return searchVpn?.name;
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				Loggers.BraveMVCLog.Exception("Method: SearchVpnUpdate", ex);
//			}

//			return null;
//		}

//		[HttpGet]
//		[Route("SearchNordVpn")]
//		public string SearchNordVpn(string searchText)
//		{
//			try
//			{
//				string path = System.Web.Hosting.HostingEnvironment.MapPath("~/Robots/nordvpn/server.json");
//				var content = System.IO.File.ReadAllText(path);
//				var ipList = JsonConvert.DeserializeObject<List<NordVpnModel>>(content);
//				using (var db = new DataContext())
//				{
//					if (!string.IsNullOrEmpty(searchText))
//						return ipList.Where(x => x.name.ToLower().Contains(searchText?.ToLower()))
//							.OrderBy(n => Guid.NewGuid()).FirstOrDefault()?.name;
//					else
//						return ipList.OrderBy(n => Guid.NewGuid()).FirstOrDefault()?.name;
//				}
//			}
//			catch (Exception ex)
//			{
//				Loggers.BraveMVCLog.Exception("Method: SearchNordVpn", ex);
//			}

//			return null;
//		}

//		[HttpGet]
//		[Route("SearchNordVpn")]
//		public string SearchNordVpn()
//		{
//			return SearchNordVpn(string.Empty);
//		}

//		#endregion Nord VPN
//	}
//}