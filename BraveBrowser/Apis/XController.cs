//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Data.Entity;
//using System.Linq;
//using System.Reflection;
//using System.Threading.Tasks;
//using System.Web.Http;
//using BraveBrowser.ApiModels;
//using BraveBrowser.ApiModels.X;
//using BraveBrowser.Logs;
//using BraveBrowser.Models;
//using static BraveBrowser.Helpers.Constants;

//namespace BraveBrowser.Apis
//{
//	[RoutePrefix("api/x")]
//	public class XController : ApiController
//	{
//		#region Group
//		[HttpGet]
//		[Route("group")]
//		public async Task<List<x_data_group>> GetGroups()
//		{
//			try
//			{
//				using (var db = new DataContext())
//				{
//					return await db.x_data_group.Where(x => x.is_delete == false).ToListAsync();
//				}
//			}
//			catch (Exception ex)
//			{
//				Loggers.BraveAPILog.InfoBegin(MethodBase.GetCurrentMethod().Name, MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Exception("Method error GetGroups", ex);
//				Loggers.BraveAPILog.InfoEnd(MethodBase.GetCurrentMethod().Name, MethodInfo.GetCurrentMethod());
//			}

//			return null;
//		}
//		#endregion

//		#region Action
//		[HttpGet]
//		[Route("action")]
//		public async Task<ApiResponse> GetAction(string fromUser)
//		{
//			var result = new ApiResponse();
//			try
//			{
//				using (var db = new DataContext())
//				{
//					// kiểm tra nếu user chưa thêm vào DB thì thêm vào
//					var user = db.x_user.FirstOrDefault(x => x.username.ToLower().Trim().Equals(fromUser.ToLower().Trim()));
//					if (user == null)
//					{
//						var newUser = new x_user
//						{
//							username = fromUser,
//							date_created = DateTime.Now,
//							updated_date = DateTime.Now
//						};
//						db.x_user.Add(newUser);
//						db.SaveChanges();
//					}
//					else
//					{
//						// nếu thời gian tương tác nhỏ hơn 10 phút thì ko cho tương tác
//						if ((DateTime.Now - user.updated_date.Value).TotalMinutes < 10)
//						{
//							return new ApiResponse();
//						}
//					}

//					#region trả về action tương tác và thông tin user cần tương tác
//					var xPercent = new List<XPercent>
//					{
//						new XPercent(XActionType.Follow, 10),
//						new XPercent(XActionType.Comment, 20),
//						new XPercent(XActionType.Like, 10),
//						new XPercent(XActionType.Reweet, 5),
//						new XPercent(XActionType.Views, 60),
//						new XPercent(XActionType.ClickAds, 3),
//						new XPercent(XActionType.Bookmark, 3),
//						new XPercent(XActionType.Tweet, 5),
//						new XPercent(XActionType.Message, 5)
//					};

//					// random ngẫu nhiên một số, lấy những action mà có % nhỏ hơn số ngẫu nhiên random ra
//					var randomNumber = new Random().Next(1, 100);
//					var randomArray = xPercent.Where(x => x.Percent > randomNumber).ToList();
//					XActionType action;
//					if (randomArray.Count > 0)
//						// tìm một action ngẫu nhiên trong các action lấy được trên
//						action = randomArray[new Random().Next(randomArray.Count - 1)].Name;
//					else
//						action = XActionType.Views;

//					// kiểm tra xem user nào mà có số action ngẫu nhiên nhỏ nhất thì cho user đó thực hiện nó
//					x_user toUser;
//					int min, max;
//					switch (action)
//					{
//						case XActionType.Follow:
//							min = db.x_user.Min(x => x.action_follow);
//							max = db.x_user.Max(x => x.action_follow);
//							if (max == 0)
//								toUser = await db.x_user.OrderBy(x => Guid.NewGuid()).FirstAsync();
//							else
//								toUser = await db.x_user
//									.Where(x => min <= x.action_follow && x.action_follow <= (max / 2))
//									.OrderBy(x => Guid.NewGuid()).FirstAsync();
//							break;
//						case XActionType.Comment:
//							min = db.x_user.Min(x => x.action_comment);
//							max = db.x_user.Max(x => x.action_comment);
//							if (max == 0)
//								toUser = await db.x_user.OrderBy(x => Guid.NewGuid()).FirstAsync();
//							else
//								toUser = await db.x_user
//									.Where(x => min <= x.action_comment && x.action_comment <= (max / 2))
//									.OrderBy(x => Guid.NewGuid()).FirstAsync();
//							break;
//						case XActionType.Like:
//							min = db.x_user.Min(x => x.action_like);
//							max = db.x_user.Max(x => x.action_like);
//							if (max == 0)
//								toUser = await db.x_user.OrderBy(x => Guid.NewGuid()).FirstAsync();
//							else
//								toUser = await db.x_user
//									.Where(x => min <= x.action_like && x.action_like <= (max / 2))
//									.OrderBy(x => Guid.NewGuid()).FirstAsync();
//							break;
//						case XActionType.Reweet:
//							min = db.x_user.Min(x => x.action_reweet);
//							max = db.x_user.Max(x => x.action_reweet);
//							if (max == 0)
//								toUser = await db.x_user.OrderBy(x => Guid.NewGuid()).FirstAsync();
//							else
//								toUser = await db.x_user
//									.Where(x => min <= x.action_reweet && x.action_reweet <= (max / 2))
//									.OrderBy(x => Guid.NewGuid()).FirstAsync();
//							break;
//						case XActionType.Views:
//							min = db.x_user.Min(x => x.action_views);
//							max = db.x_user.Max(x => x.action_views);
//							if (max == 0)
//								toUser = await db.x_user.OrderBy(x => Guid.NewGuid()).FirstAsync();
//							else
//								toUser = await db.x_user
//									.Where(x => min <= x.action_views && x.action_views <= (max / 2))
//									.OrderBy(x => Guid.NewGuid()).FirstAsync();
//							break;
//						case XActionType.ClickAds:
//							min = db.x_user.Min(x => x.action_click_ads);
//							max = db.x_user.Max(x => x.action_click_ads);
//							if (max == 0)
//								toUser = await db.x_user.OrderBy(x => Guid.NewGuid()).FirstAsync();
//							else
//								toUser = await db.x_user
//									.Where(x => min <= x.action_click_ads && x.action_click_ads <= (max / 2))
//									.OrderBy(x => Guid.NewGuid()).FirstAsync();
//							break;
//						case XActionType.Bookmark:
//							min = db.x_user.Min(x => x.action_bookmark);
//							max = db.x_user.Max(x => x.action_bookmark);
//							if (max == 0)
//								toUser = await db.x_user.OrderBy(x => Guid.NewGuid()).FirstAsync();
//							else
//								toUser = await db.x_user
//									.Where(x => min <= x.action_bookmark && x.action_bookmark <= (max / 2))
//									.OrderBy(x => Guid.NewGuid()).FirstAsync();
//							break;
//						case XActionType.Tweet:
//							min = db.x_user.Min(x => x.action_tweet);
//							max = db.x_user.Max(x => x.action_tweet);
//							if (max == 0)
//								toUser = await db.x_user.OrderBy(x => Guid.NewGuid()).FirstAsync();
//							else
//								toUser = await db.x_user
//									.Where(x => min <= x.action_tweet && x.action_tweet <= (max / 2))
//									.OrderBy(x => Guid.NewGuid()).FirstAsync();
//							break;
//						case XActionType.Message:
//							min = db.x_user.Min(x => x.action_message);
//							max = db.x_user.Max(x => x.action_message);
//							if (max == 0)
//								toUser = await db.x_user.OrderBy(x => Guid.NewGuid()).FirstAsync();
//							else
//								toUser = await db.x_user
//									.Where(x => min <= x.action_message && x.action_message <= (max / 2))
//									.OrderBy(x => Guid.NewGuid()).FirstAsync();
//							break;
//						default:
//							toUser = null;
//							break;
//					}

//					// trả về user và hành động ngẫu nhiên
//					return new ApiResponse
//					{
//						IsSuccess = true,
//						Message = "Success",
//						Data = new
//						{
//							randomNumber,
//							toUser = toUser.username,
//							action = action.ToString()
//						}
//					};
//					#endregion
//				}
//			}
//			catch (Exception ex)
//			{
//				result.IsSuccess = false;
//				result.Message = $"Lỗi: {ex.Message}";

//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Exception("Method error GetAction", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//			}

//			return result;
//		}

//		[HttpPost]
//		[Route("set-action")]
//		public ApiResponse SetAction(string fromUser, string toUser, XActionType action)
//		{
//			var result = new ApiResponse();
//			try
//			{
//				using (var db = new DataContext())
//				{
//					var fromUserObj = db.x_user.FirstOrDefault(x => x.username.ToLower().Trim().Equals(fromUser.ToLower().Trim()));
//					var toUserObj = db.x_user.FirstOrDefault(x => x.username.ToLower().Trim().Equals(toUser.ToLower().Trim()));
//					if (fromUser == null || toUser == null)
//						result.IsSuccess = false;
//					else
//					{
//						var log = new x_log
//						{
//							from_user_id = fromUserObj.x_user_id,
//							to_user_id = toUserObj.x_user_id,
//							action_name = action.ToString(),
//							created_date = DateTime.Now
//						};
//						db.x_log.Add(log);
//						db.SaveChanges();

//						result.IsSuccess = true;
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Exception("Method error SetAction: ", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());

//				result.IsSuccess = false;
//				result.Message = ex.Message;
//			}
//			return result;
//		}
//		#endregion

//		#region Post
//		[HttpPost]
//		[Route("add-data-post")]
//		public async Task<bool> AddDataPost(x_data_post request)
//		{
//			try
//			{
//				using (var db = new DataContext())
//				{
//					if(!db.x_data_post.Any(x => x.crawl_post_id == request.crawl_post_id)) {
//						var post = new x_data_post
//						{
//							x_data_group_id = request.x_data_group_id,
//							text_ai = request.text_ai,
//							text_ai_en = request.text_ai_en,
//							image = request.image,
//							crawl_post_id = request.crawl_post_id,
//							crawl_text = request.crawl_text,
//							crawl_link = request.crawl_link,
//							crawl_date = DateTime.Now
//						};
//						db.x_data_post.Add(post);
//						await db.SaveChangesAsync();
//						return true;
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				Loggers.BraveAPILog.InfoBegin(MethodBase.GetCurrentMethod().Name, MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Exception("Method error AddDataPost", ex);
//				Loggers.BraveAPILog.InfoEnd(MethodBase.GetCurrentMethod().Name, MethodInfo.GetCurrentMethod());
//			}

//			return false;
//		}

//		[HttpGet]
//		[Route("get-max-post")]
//		public async Task<long> GetMaxPost(string groupName)
//		{
//			try
//			{
//				using (var db = new DataContext())
//				{
//					var group = db.x_data_group.FirstOrDefault(x => x.group_name.ToLower().Trim().Equals(groupName.ToLower().Trim()));
//					if (group != null)
//					{
//						return await db.x_data_post
//							.Where(p => p.x_data_group_id == group.x_data_group_id)
//							.MaxAsync(x => x.crawl_post_id.Value);
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				Loggers.BraveAPILog.InfoBegin(MethodBase.GetCurrentMethod().Name, MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Exception("Method error GetMaxPost", ex);
//				Loggers.BraveAPILog.InfoEnd(MethodBase.GetCurrentMethod().Name, MethodInfo.GetCurrentMethod());
//			}

//			return 0;
//		}

//		[HttpGet]
//		[Route("get-data-posts")]
//		public async Task<ApiResponse> GetPosts(string groupName, int numberOfLastPost)
//		{
//			var result = new ApiResponse();
//			try
//			{
//				using (var db = new DataContext())
//				{
//					var group = db.x_data_group
//						.FirstOrDefault(x => x.group_name.ToLower().Trim().Equals(groupName.ToLower().Trim()));
//					if (group != null)
//					{
//						var result1 = await db.x_data_post
//							.Where(p => p.x_data_group_id == group.x_data_group_id && p.is_delete == false)
//							.OrderByDescending(x => x.x_data_post_id)
//							.Select(i => new {
//								i.x_data_post_id,
//								i.x_data_group_id,
//								i.image,
//								i.crawl_text,
//								i.crawl_link,
//								i.text_ai,
//								i.text_ai_en,
//								i.crawl_post_id
//							})
//							.Take(numberOfLastPost)
//							.ToListAsync();

//						result.IsSuccess = true;
//						result.Data = result1;
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				Loggers.BraveAPILog.InfoBegin(MethodBase.GetCurrentMethod().Name, MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Exception("Method error GetPosts", ex);
//				Loggers.BraveAPILog.InfoEnd(MethodBase.GetCurrentMethod().Name, MethodInfo.GetCurrentMethod());

//				result.IsSuccess = false;
//			}

//			return result;
//		}
//		#endregion

//		#region Comment
//		[HttpPost]
//		[Route("add-data-comment")]
//		public async Task<bool> AddDataComment(x_data_comment request)
//		{
//			try
//			{
//				using (var db = new DataContext())
//				{
//					if (!db.x_data_comment.Any(x => x.crawl_comment_id == request.crawl_comment_id))
//					{
//						var comment = new x_data_comment
//						{
//							x_data_post_id = request.x_data_post_id,
//							crawl_text = request.crawl_text,
//							text_ai = request.text_ai,
//							text_ai_en = request.text_ai_en,
//							crawl_comment_id = request.crawl_comment_id,
//							crawl_link = request.crawl_link,
//							crawl_date = DateTime.Now
//						};
//						db.x_data_comment.Add(comment);
//						await db.SaveChangesAsync();
//						return true;
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				Loggers.BraveAPILog.InfoBegin(MethodBase.GetCurrentMethod().Name, MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Exception("Method error AddDataComment", ex);
//				Loggers.BraveAPILog.InfoEnd(MethodBase.GetCurrentMethod().Name, MethodInfo.GetCurrentMethod());
//			}

//			return false;
//		}

//		[HttpGet]
//		[Route("get-max-comment")]
//		public async Task<long> GetMaxComment(long postId)
//		{
//			try
//			{
//				using (var db = new DataContext())
//				{
//					return await db.x_data_comment
//						.Where(p => p.x_data_post_id == postId)
//						.MaxAsync(x => x.crawl_comment_id.Value);
//				}
//			}
//			catch (Exception ex)
//			{
//				Loggers.BraveAPILog.InfoBegin(MethodBase.GetCurrentMethod().Name, MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Exception("Method error GetMaxComment", ex);
//				Loggers.BraveAPILog.InfoEnd(MethodBase.GetCurrentMethod().Name, MethodInfo.GetCurrentMethod());
//			}

//			return 0;
//		}
//		#endregion
//	}
//}