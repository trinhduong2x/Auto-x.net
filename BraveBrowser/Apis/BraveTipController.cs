//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.IO;
//using System.Linq;
//using System.Net.Http;
//using System.Threading.Tasks;
//using System.Web;
//using System.Web.Http;
//using BraveBrowser.ApiModels;
//using BraveBrowser.ApiModels.BraveTip;
//using BraveBrowser.Areas.Admin.ViewModels;
//using BraveBrowser.Helpers;
//using BraveBrowser.Logs;
//using BraveBrowser.Models;
//using Newtonsoft.Json;
//using static BraveBrowser.Helpers.Constants;
//using ActiveUp.Net.Mail;

//namespace BraveBrowser.Apis
//{
//	[RoutePrefix("api/BraveTip")]
//	public class BraveTipController : ApiController
//	{
//		#region SYNC DATA
//		[HttpGet]
//		[Route("SyncChannelLink")]
//		public ApiResponse SyncChannelLink(string channelType)
//		{
//			var result = new ApiResponse();
//			try
//			{
//				using (var db = new DataContext())
//				{
//					var twitchs = db.brave_tip.Where(x => channelType.Equals(x.channel_type)).ToList();
//					foreach (var item in twitchs)
//					{
//						string link = null;
//						if ("reddit".Equals(channelType))
//							link = $"https://www.reddit.com/user/{item.channel_username}";
//						else if ("twitch".Equals(channelType))
//							link = $"https://www.twitch.tv/{item.channel_username}";

//						if (link != null && !db.brave_tip_url.Any(x => link.Equals(x.youtube_video_url)))
//						{
//							var braveUrl = new brave_tip_url();
//							braveUrl.youtube_video_url = link;
//							braveUrl.brave_tip_id = item.brave_tip_id;
//							braveUrl.is_channel_link = true;
//							braveUrl.like_number = 0;
//							braveUrl.comment_number = 0;
//							braveUrl.create_date = DateTime.Now;
//							db.brave_tip_url.Add(braveUrl);
//							db.SaveChanges();

//							result.IsSuccess = true;
//							result.Message = $"Add success {link}";
//						}
//						else
//						{
//							result.IsSuccess = false;
//							result.Message = $"Error: exists link {link}";
//						}
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Exception("SyncChannelLink", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());

//				result.IsSuccess = false;
//				result.Message = ex.Message;
//			}

//			return result;
//		}

//		[HttpGet]
//		[Route("UpdatePasswordTemp")]
//		public ApiResponse UpdatePasswordTemp(string email, string password)
//		{
//			var result = new ApiResponse();
//			try
//			{
//				using (var db = new DataContext())
//				{
//					var wallet = db.brave_wallet.FirstOrDefault(x => x.email.Trim().ToLower().Equals(email.Trim().ToLower()));
//					if (wallet != null)
//					{
//						wallet.email_password = password;
//						db.Entry(wallet).State = EntityState.Modified;
//						db.SaveChanges();

//						result.IsSuccess = true;
//						result.Message = "Update wallet password sucessfully.";
//					}

//					var tip = db.brave_tip.FirstOrDefault(x => x.gmail_email.Trim().ToLower().Equals(email.Trim().ToLower()));
//					if (tip != null)
//					{
//						tip.gmail_password = password;
//						db.Entry(tip).State = EntityState.Modified;
//						db.SaveChanges();

//						result.IsSuccess = true;
//						result.Message = "Update pub password sucessfully.";
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Exception("UpdatePasswordTemp", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());

//				result.IsSuccess = false;
//				result.Message = ex.Message;
//			}

//			return result;
//		}
//		#endregion

//		#region brave_tip
//		[HttpGet]
//		[Route("GetRandomUrl")]
//		public BraveTipResponse GetRandomUrl()
//		{
//			try
//			{
//				using (var db = new DataContext())
//				{
//					var tips = db.GetRandomTipUrls();
//					return new BraveTipResponse
//					{
//						brave_tip_id = tips.brave_tip_id,
//						brave_tip_url_id = tips.brave_tip_url_id,
//						youtube_video_url = tips.youtube_video_url,
//						is_channel_link = tips.is_channel_link,
//						IsSuccess = true
//					};
//				}
//			}
//			catch (Exception ex)
//			{
//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Exception("GetTipUrl: GetRandomUrl", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				return null;
//			}
//		}

//		[HttpGet]
//		[Route("GetRandomUrlV2")]
//		public BraveTipResponse GetRandomUrlV2()
//		{
//			try
//			{
//				using (var db = new DataContext())
//				{
//					var tips = db.GetRandomTipUrlsV2();
//					return new BraveTipResponse
//					{
//						brave_tip_id = tips.brave_tip_id,
//						brave_tip_url_id = tips.brave_tip_url_id,
//						youtube_video_url = tips.youtube_video_url
//					};
//				}
//			}
//			catch (Exception ex)
//			{
//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Exception("GetTipUrl: GetRandomUrlV2", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				return null;
//			}
//		}

//		[HttpPost]
//		[Route("GetRandomUrlV3")]
//		public BraveTipResponse GetRandomUrlV3(BraveTipExclusionChannelListRequest request)
//		{
//			var result = new BraveTipResponse();
//			try
//			{
//				using (var db = new DataContext())
//				{
//					var tip = db.GetRandomTipUrlsV3(request.exclusion_channel_list);
//					if (tip == null)
//					{
//						result.IsSuccess = false;
//						result.StatusCode = Error.ERROR_TIP_01;
//						result.Message = "Lỗi: không còn Link để tip";
//					}
//					else
//					{
//						result.IsSuccess = true;
//						result.brave_tip_id = tip.brave_tip_id;
//						result.brave_tip_url_id = tip.brave_tip_url_id;
//						result.youtube_video_url = tip.youtube_video_url;
//						result.redirect_url = tip.redirect_url;
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Exception("GetRandomUrlV3: GetRandomUrlV3", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());

//				result.IsSuccess = false;
//				result.StatusCode = Error.ERROR_TIP_00;
//				result.Message = $"Lỗi: {ex.Message}";
//			}

//			return result;
//		}

//		[HttpPost]
//		[Route("GetRandomUrlV4")]
//		public BraveTipResponse GetRandomUrlV4(BraveTipExclusionChannelListRequest request)
//		{
//			var result = new BraveTipResponse();
//			try
//			{
//				using (var db = new DataContext())
//				{
//					var tip = db.GetRandomTipUrlsV4(request.exclusion_channel_list, request.tip_ip);
//					if (tip == null)
//					{
//						result.IsSuccess = false;
//						result.StatusCode = Error.ERROR_TIP_01;
//						result.Message = "Lỗi: không còn Link để tip";
//					}
//					else
//					{
//						result.IsSuccess = true;
//						result.brave_tip_id = tip.brave_tip_id;
//						result.brave_tip_url_id = tip.brave_tip_url_id;
//						result.youtube_video_url = tip.youtube_video_url;
//						result.redirect_url = tip.redirect_url;
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Exception("GetRandomUrlV3: GetRandomUrlV3", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());

//				result.IsSuccess = false;
//				result.StatusCode = Error.ERROR_TIP_00;
//				result.Message = $"Lỗi: {ex.Message}";
//			}

//			return result;
//		}

//		[HttpGet]
//		[Route("GetRedirectUrl")]
//		public BraveTipResponse GetRedirectUrl()
//		{
//			var result = new BraveTipResponse();
//			try
//			{
//				using (var db = new DataContext())
//				{
//					var tip = db.GetRedirectUrl();
//					if (tip == null)
//					{
//						result.IsSuccess = false;
//						result.StatusCode = Error.ERROR_TIP_01;
//						result.Message = "Lỗi: không còn redirect";
//					}
//					else
//					{
//						result.IsSuccess = true;
//						result.brave_tip_id = tip.brave_tip_id;
//						result.brave_tip_url_id = tip.brave_tip_url_id;
//						result.youtube_video_url = tip.youtube_video_url;
//						result.redirect_url = tip.redirect_url;
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Exception("GetRandomUrlV3: GetRedirectUrl", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());

//				result.IsSuccess = false;
//				result.StatusCode = Error.ERROR_TIP_00;
//				result.Message = $"Lỗi: {ex.Message}";
//			}

//			return result;
//		}

//		[HttpGet]
//		[Route("GetSequenceUrl")]
//		public BraveTipResponse GetSequenceUrl()
//		{
//			try
//			{
//				using (var db = new DataContext())
//				{
//					var tips = db.GetSequenceTipUrls();
//					return new BraveTipResponse
//					{
//						brave_tip_id = tips.brave_tip_id,
//						brave_tip_url_id = tips.brave_tip_url_id,
//						youtube_video_url = tips.youtube_video_url
//					};
//				}
//			}
//			catch (Exception ex)
//			{
//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Exception("GetTipUrl: GetTipUrl", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				return null;
//			}
//		}

//		[HttpPost]
//		[Route("SetTipUrl")]
//		public ApiResponse SetTipUrl(SetTipAdsRequest model)
//		{
//			var result = new ApiResponse() { IsSuccess = false, Message = string.Empty };
//			try
//			{
//				using (var db = new DataContext())
//				{
//					var item = db.brave_tip_url.FirstOrDefault(x => x.brave_tip_url_id == model.TipUrlId);
//					if (item != null)
//					{
//						// kiểm tra license đã được thêm chưa
//						var braveAds = db.brave_ads.FirstOrDefault(x => x.license_key.ToLower().Equals(model.LicenseKey.ToLower()));
//						if (braveAds != null)
//						{
//							// kiểm tra profile đã được thêm chưa
//							var profileAds = db.profile_ads.FirstOrDefault(x => x.brave_ads_id == braveAds.brave_ads_id
//								&& x.profile_name.ToLower().Equals(model.ProfileKey.ToLower()));
//							if (profileAds != null)
//							{
//								// add tip
//								var tipBat = new brave_tip_bat();
//								tipBat.brave_tip_url_id = item.brave_tip_url_id;
//								tipBat.profile_ads_id = profileAds.profile_ads_id;
//								tipBat.brave_tip_number = model.BatNumber;
//								tipBat.last_update_date = DateTime.Now;
//								db.brave_tip_bat.Add(tipBat);
//								db.SaveChanges();

//								// trừ BAT ở profile sau khi tip
//								var remain_bat_total = profileAds.profile_bat_total - model.BatNumber;
//								profileAds.profile_bat_total = remain_bat_total > 0 ? remain_bat_total : 0;
//								db.Entry(profileAds).State = EntityState.Modified;
//								db.SaveChanges();

//								result.IsSuccess = true;
//								result.Message = $"Tip +{model.BatNumber} BAT từ {model.ProfileKey} thành công.";
//							}
//							else
//							{
//								result.IsSuccess = false;
//								result.Message = $"Không tồn tại Profile {model.ProfileKey}";
//							}
//						}
//						else
//						{
//							result.IsSuccess = false;
//							result.Message = $"Lỗi: Key {model.LicenseKey} không tồn tại, vui lòng đăng kí Key với server trước.";
//						}
//					}
//					else
//					{
//						result.IsSuccess = false;
//						result.Message = $"Không tồn tại TipUrlId {model.TipUrlId}";
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				result.IsSuccess = false;
//				result.Message = $"Lỗi {ex.Message}";

//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Info("      SetTipUrl TipUrlId: " + model.TipUrlId);
//				Loggers.BraveAPILog.Info("      SetTipUrl BatNumber: " + model.BatNumber);
//				Loggers.BraveAPILog.Info("      SetTipUrl LicenseKey: " + model.LicenseKey);
//				Loggers.BraveAPILog.Info("      SetTipUrl ProfileKey: " + model.ProfileKey);
//				Loggers.BraveAPILog.Exception("SetTipUrl: ", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//			}

//			return result;
//		}

//		[HttpPost]
//		[Route("SetTipUrlV1")]
//		public ApiResponse SetTipUrlV1(SetTipAdsRequestV1 model)
//		{
//			var result = new ApiResponse() { IsSuccess = false, Message = string.Empty };
//			try
//			{
//				using (var db = new DataContext())
//				{
//					var item = db.brave_tip_url.FirstOrDefault(x => x.brave_tip_url_id == model.TipUrlId);
//					if (item != null)
//					{
//						var validate = BraveAdsUtils.ValidateVmware(model.VmwareName);
//						if (validate.IsSuccess)
//						{
//							var braveAds = ((List<brave_ads>)validate.Data)[0];

//							// kiểm tra profile đã được thêm chưa
//							var profileAds = db.profile_ads.FirstOrDefault(x => x.brave_ads_id == braveAds.brave_ads_id
//								&& x.profile_name.ToLower().Equals(model.ProfileKey.ToLower()));
//							if (profileAds != null)
//							{
//								// add tip
//								var tipBat = new brave_tip_bat();
//								tipBat.brave_tip_url_id = item.brave_tip_url_id;
//								tipBat.profile_ads_id = profileAds.profile_ads_id;
//								tipBat.brave_tip_number = model.BatNumber;
//								tipBat.brave_tip_ip = model.TipIP;
//								tipBat.last_update_date = DateTime.Now;
//								db.brave_tip_bat.Add(tipBat);
//								db.SaveChanges();

//								// trừ BAT ở profile sau khi tip
//								var remain_bat_total = profileAds.profile_bat_total - model.BatNumber;
//								profileAds.profile_bat_total = remain_bat_total > 0 ? remain_bat_total : 0;
//								db.Entry(profileAds).State = EntityState.Modified;
//								db.SaveChanges();

//								result.IsSuccess = true;
//								result.Message = $"Tip +{model.BatNumber} BAT từ {model.ProfileKey} thành công.";
//							}
//							else
//							{
//								result.IsSuccess = false;
//								result.Message = $"Không tồn tại Profile {model.ProfileKey}";
//							}
//						}
//						else
//						{
//							result.IsSuccess = validate.IsSuccess;
//							result.Message = validate.Message;
//						}
//					}
//					else
//					{
//						result.IsSuccess = false;
//						result.Message = $"Không tồn tại TipUrlId {model.TipUrlId}";
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				result.IsSuccess = false;
//				result.Message = $"Lỗi {ex.Message}";

//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Info("      SetTipUrl TipUrlId: " + model.TipUrlId);
//				Loggers.BraveAPILog.Info("      SetTipUrl BatNumber: " + model.BatNumber);
//				Loggers.BraveAPILog.Info("      SetTipUrl VmwareName: " + model.VmwareName);
//				Loggers.BraveAPILog.Info("      SetTipUrl ProfileKey: " + model.ProfileKey);
//				Loggers.BraveAPILog.Exception("SetTipUrl: ", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//			}

//			return result;
//		}

//		[HttpPost]
//		[Route("YoutubeUpdateVideo")]
//		public ApiResponse YoutubeUpdateVideo(VideoRequest video)
//		{
//			var result = new ApiResponse();
//			try
//			{
//				using (var db = new DataContext())
//				{
//					var braveTip = db.brave_tip.FirstOrDefault(x => x.gmail_email.Trim().ToLower().Equals(video.GmailEmail.Trim().ToLower()));
//					if (braveTip != null)
//					{
//						var braveUrl = new brave_tip_url();
//						braveUrl.youtube_video_url = video.VideoURL;
//						braveUrl.redirect_url = video.RedirectUrl;
//						braveUrl.brave_tip_id = braveTip.brave_tip_id;
//						braveUrl.is_channel_link = video.IsChannelLink;
//						braveUrl.like_number = 0;
//						braveUrl.comment_number = 0;
//						braveUrl.create_date = DateTime.Now;
//						db.brave_tip_url.Add(braveUrl);
//						db.SaveChanges();

//						result.IsSuccess = true;
//						result.Message = "Create video sucessfully.";
//					}
//					else
//					{
//						result.IsSuccess = false;
//						result.Message = $"Không tìm thấy kênh Youtube của {video.GmailEmail}";
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Info("      AddNewVideo VideoURL: " + video.VideoURL);
//				Loggers.BraveAPILog.Info("      AddNewVideo BraveTipId: " + video.GmailEmail);
//				Loggers.BraveAPILog.Exception("Method error AddNewVideo: ", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//			}
//			return result;
//		}

//		[HttpGet]
//		[Route("GetChannelRandom")]
//		public BraveTipChannelResponse GetChannelRandom()
//		{
//			var result = new BraveTipChannelResponse();
//			try
//			{
//				using (var db = new DataContext())
//				{
//					var brave = db.brave_tip.Include("brave_tip_url").Where(p => p.brave_tip_url.Count < 3).FirstOrDefault();
//					if (brave != null)
//					{
//						result.youtube_email = brave.gmail_email;
//						result.youtube_password = brave.gmail_password;
//						result.brave_tip_id = brave.brave_tip_id;
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Exception("Method error GetChannelRandom: ", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//			}
//			finally
//			{
//			}
//			return result;
//		}

//		[HttpGet]
//		[Route("IsExistsEmail")]
//		public bool IsExistsEmail(string email)
//		{
//			try
//			{
//				using (var db = new DataContext())
//				{
//					return db.brave_tip.Any(x => x.gmail_email.Trim().ToLower().Equals(email.Trim().ToLower()));
//				}
//			}
//			catch (Exception ex)
//			{
//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Exception("IsExistsEmail()", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				return false;
//			}
//		}

//		[HttpPost]
//		[Route("AddChannelTip")]
//		public ApiResponse AddChannelTip(AddChannelTipRequest channel)
//		{
//			var result = new ApiResponse();
//			try
//			{
//				using (var db = new DataContext())
//				{
//					var braveTip = new brave_tip();
//					braveTip.gmail_email = channel.GmailEmail;
//					braveTip.gmail_password = channel.GmailPassword;
//					braveTip.gmail_2fa = channel.Gmail2Fa;
//					braveTip.uphold_email = channel.UpholdEmail;
//					braveTip.uphold_password = channel.UpholdPassword;
//					braveTip.uphold_2fa = channel.Uphold2Fa;
//					braveTip.vpn_name = channel.VpnName;
//					braveTip.publisher_2fa = channel.Publisher2Fa;
//					braveTip.max_follow_number = 1000;
//					braveTip.max_bat_number = new Random().Next(35, 50);
//					braveTip.brave_tip_status_id = 2;// Đang hoạt động
//					braveTip.last_update_date = DateTime.Now;
//					braveTip.create_date = DateTime.Now;
//					braveTip.feed_date = DateTime.Now;
//					braveTip.is_active = false;
//					braveTip.is_sync_feed = false;
//					db.brave_tip.Add(braveTip);
//					db.SaveChanges();

//					result.IsSuccess = true;
//					result.Message = "Create channel sucessfully.";
//				}
//			}
//			catch (Exception ex)
//			{
//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Exception("Method error AddNewChannel: ", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//			}
//			return result;
//		}

//		[HttpPost]
//		[Route("AddChannelTipV1")]
//		public ApiResponse AddChannelTipV1(AddChannelTipRequestV1 channel)
//		{
//			var result = new ApiResponse();
//			try
//			{
//				using (var db = new DataContext())
//				{
//					var braveTip = new brave_tip();
//					braveTip.vpn_name = channel.VpnName;
//					braveTip.type = channel.Type;
//					braveTip.gmail_email = channel.PubEmail;
//					braveTip.gmail_password = channel.PubPassword;
//					braveTip.gmail_recovery_email = channel.PubRecoveryEmail;
//					braveTip.publisher_2fa = channel.Pub2Fa;
//					braveTip.gmail_2fa = channel.PubEmail2Fa;
//					braveTip.uphold_email = channel.UpholdEmail;
//					braveTip.uphold_email_password = channel.UpholdEmailPassword;
//					braveTip.uphold_email_2fa = channel.UpholdEmail2Fa;
//					braveTip.uphold_recovery_email = channel.UpholdRecoveryEmail;
//					braveTip.uphold_password = channel.UpholdPassword;
//					braveTip.uphold_2fa = channel.Uphold2Fa;
//					braveTip.note = channel.Note;
//					braveTip.max_follow_number = 1000;
//					braveTip.max_bat_number = new Random().Next(35, 50);
//					braveTip.brave_tip_status_id = 2;// Đang hoạt động
//					braveTip.last_update_date = DateTime.Now;
//					braveTip.create_date = DateTime.Now;
//					braveTip.feed_date = DateTime.Now;
//					braveTip.is_sync_feed = false;
//					braveTip.is_active = false;
//					db.brave_tip.Add(braveTip);
//					db.SaveChanges();

//					result.IsSuccess = true;
//					result.Message = "Create channel sucessfully.";
//				}
//			}
//			catch (Exception ex)
//			{
//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Exception("Method error AddChannelTipV1: ", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//			}
//			return result;
//		}

//		[HttpPost]
//		[Route("AddChannelNoWallet")]
//		public ApiResponse AddChannelNoWallet(AddChannelNoWalletRequest channel)
//		{
//			var result = new ApiResponse();
//			try
//			{
//				using (var db = new DataContext())
//				{
//					var braveTip = new brave_tip();
//					braveTip.vpn_name = channel.VpnName;
//					braveTip.type = channel.Type;
//					braveTip.gmail_email = channel.PubEmail;
//					braveTip.gmail_password = channel.PubPassword;
//					braveTip.gmail_recovery_email = channel.PubRecoveryEmail;
//					braveTip.publisher_2fa = channel.Pub2Fa;
//					braveTip.gmail_2fa = channel.PubEmail2Fa;
//					braveTip.channel_username = channel.Username;
//					braveTip.channel_password = channel.UsernamePassword;
//					braveTip.channel_2fa = channel.Username2Fa;
//					braveTip.channel_vpn = channel.ChannelVpn;
//					braveTip.note = channel.Note;
//					braveTip.max_follow_number = 1000;
//					braveTip.max_bat_number = new Random().Next(35, 50);
//					braveTip.brave_tip_status_id = 10;// No Wallet
//					braveTip.last_update_date = DateTime.Now;
//					braveTip.create_date = DateTime.Now;
//					braveTip.feed_date = DateTime.Now;
//					braveTip.is_sync_feed = false;
//					braveTip.is_active = false;
//					braveTip.is_checking_live = false;
//					db.brave_tip.Add(braveTip);
//					db.SaveChanges();

//					// add channel link url
//					if (!string.IsNullOrEmpty(channel.ChannelLink))
//					{
//						var braveUrl = new brave_tip_url();
//						braveUrl.youtube_video_url = channel.ChannelLink;
//						braveUrl.brave_tip_id = braveTip.brave_tip_id;
//						braveUrl.is_channel_link = true;
//						braveUrl.like_number = 0;
//						braveUrl.comment_number = 0;
//						braveUrl.create_date = DateTime.Now;
//						db.brave_tip_url.Add(braveUrl);
//						db.SaveChanges();
//					}

//					result.IsSuccess = true;
//					result.Message = "Create channel sucessfully.";
//				}
//			}
//			catch (Exception ex)
//			{
//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Exception("Method error AddChannelNoWallet: ", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//			}
//			return result;
//		}

//		[HttpPost]
//		[Route("AddChannelWithSameWallet")]
//		public ApiResponse AddChannelWithSameWallet(AddChannelWithSameWalletRequest channel)
//		{
//			var result = new ApiResponse();
//			try
//			{
//				using (var db = new DataContext())
//				{
//					brave_wallet wallet = null;
//					// brave_wallet: add wallet
//					if (!string.IsNullOrEmpty(channel.WalletPassword))
//					{
//						// kiểm wallet wallet đã được thêm chưa
//						wallet = db.brave_wallet.FirstOrDefault(x => x.email.ToLower() == channel.PubEmail.ToLower());
//						if (wallet == null)
//						{
//							wallet = new brave_wallet
//							{
//								email = channel.PubEmail,
//								email_password = channel.PubPassword,
//								email_2fa = channel.PubEmail2Fa,
//								email_recovery = channel.PubRecoveryEmail,
//								wallet_2fa = channel.Wallet2Fa,
//								wallet_password = channel.WalletPassword,
//								wallet_phone = string.Empty,
//								wallet_type = channel.WalletType,
//								wallet_vpn = channel.ChannelVpn,
//								create_date = DateTime.Now,
//								feed_date = DateTime.Now,
//								is_forward = false,
//								is_die = false
//							};
//							db.brave_wallet.Add(wallet);
//							db.SaveChanges();
//						}
//					}

//					// brave_tip
//					var braveTip = new brave_tip();
//					braveTip.vpn_name = channel.VpnName;
//					braveTip.type = channel.Type;
//					braveTip.gmail_email = channel.PubEmail;
//					braveTip.gmail_password = channel.PubPassword;
//					braveTip.gmail_recovery_email = channel.PubRecoveryEmail;
//					braveTip.publisher_2fa = channel.Pub2Fa;
//					braveTip.gmail_2fa = channel.PubEmail2Fa;
//					braveTip.channel_username = channel.Username;
//					braveTip.note = channel.Note;
//					braveTip.brave_wallet_id = wallet.brave_wallet_id;
//					braveTip.channel_type = channel.ChannelType;
//					braveTip.channel_vpn = channel.ChannelVpn;
//					braveTip.max_follow_number = 1000;
//					braveTip.max_bat_number = new Random().Next(35, 50);
//					braveTip.brave_tip_status_id = 2;
//					braveTip.last_update_date = DateTime.Now;
//					braveTip.create_date = DateTime.Now;
//					braveTip.feed_date = DateTime.Now;
//					braveTip.is_sync_feed = false;
//					braveTip.is_active = false;
//					db.brave_tip.Add(braveTip);
//					db.SaveChanges();

//					// brave_tip_url: add channel link url
//					if (!string.IsNullOrEmpty(channel.ChannelLink))
//					{
//						var braveUrl = new brave_tip_url();
//						braveUrl.youtube_video_url = channel.ChannelLink;
//						braveUrl.brave_tip_id = braveTip.brave_tip_id;
//						braveUrl.is_channel_link = true;
//						braveUrl.like_number = 0;
//						braveUrl.comment_number = 0;
//						braveUrl.create_date = DateTime.Now;
//						db.brave_tip_url.Add(braveUrl);
//						db.SaveChanges();
//					}

//					result.IsSuccess = true;
//					result.Message = "Create channel sucessfully.";
//				}
//			}
//			catch (Exception ex)
//			{
//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Exception("Method error AddChannelWithSameWallet: ", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//			}
//			return result;
//		}

//		[HttpPost]
//		[Route("ReplaceUpholdTipV1")]
//		public ApiResponse ReplaceUpholdTipV1(AddChannelTipRequestV1 channel)
//		{
//			var result = new ApiResponse();
//			try
//			{
//				using (var db = new DataContext())
//				{
//					var braveTip = db.brave_tip.FirstOrDefault(x => x.gmail_email.Trim().ToLower().Equals(channel.PubEmail.Trim().ToLower()));
//					if (braveTip != null)
//					{
//						braveTip.last_update_date = DateTime.Now;
//						braveTip.brave_tip_status_id = 2; // Status: Đang hoạt động
//						braveTip.is_active = true; // kích hoạt tip

//						braveTip.uphold_email = channel.UpholdEmail;
//						braveTip.uphold_email_password = channel.UpholdEmailPassword;
//						braveTip.uphold_email_2fa = channel.UpholdEmail2Fa;
//						braveTip.uphold_recovery_email = channel.UpholdRecoveryEmail;
//						braveTip.uphold_password = channel.UpholdPassword;
//						braveTip.uphold_2fa = channel.Uphold2Fa;
//						braveTip.note = channel.Note;
//						db.Entry(braveTip).State = EntityState.Modified;
//						db.SaveChanges();

//						result.IsSuccess = true;
//						result.Message = "Replace uphold sucessfully.";
//					}
//					else
//					{
//						result.IsSuccess = false;
//						result.Message = $"Không tìm thấy kênh email= {channel.PubEmail}";
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Exception("Method error ReplaceUpholdTipV1: ", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());

//				result.IsSuccess = false;
//				result.Message = $"Error: {ex.Message}";
//			}
//			return result;
//		}

//		[HttpGet]
//		[Route("GetChannelByEmail")]
//		public brave_tip GetChannelByEmail(string email)
//		{
//			try
//			{
//				using (var db = new DataContext())
//				{
//					var tip = db.brave_tip.FirstOrDefault(x => x.gmail_email.Trim().ToLower().Equals(email.Trim().ToLower()));
//					if (tip?.brave_wallet_id != null)
//						tip = db.brave_tip.Include("brave_wallet").FirstOrDefault(x => x.gmail_email.Trim().ToLower().Equals(email.Trim().ToLower()));
//					return tip;
//				}
//			}
//			catch (Exception ex)
//			{
//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Exception("Method error GetChannelByEmail: ", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//			}
//			return null;
//		}

//		[HttpGet]
//		[Route("GetChannelByStatus")]
//		public ApiResponse GetChannelByStatus(int status)
//		{
//			return GetChannelByStatus(status, string.Empty);
//		}

//		[HttpGet]
//		[Route("GetChannelByStatus")]
//		public ApiResponse GetChannelByStatus(int status, string type)
//		{
//			var result = new ApiResponse();
//			try
//			{
//				var channels = new List<brave_tip>();
//				using (var db = new DataContext())
//				{
//					var tips = db.brave_tip.Where(x => (string.IsNullOrEmpty(type) || x.type.Equals(type)) && x.brave_tip_status_id == status).ToList();
//					foreach (var item in tips)
//					{
//						var uphold = db.brave_tip.FirstOrDefault(x => x.brave_tip_id == item.uphold_link_id);
//						if (uphold != null)
//						{
//							// nếu email uphold giống email pub thì lấy email pass và 2fa email của pub
//							if (uphold.gmail_email.Equals(uphold.uphold_email))
//							{
//								item.uphold_email_password = uphold.gmail_password;
//								item.uphold_email_2fa = uphold.gmail_2fa;
//								item.uphold_recovery_email = uphold.gmail_recovery_email;

//								item.uphold_email = uphold.uphold_email;
//								item.uphold_password = uphold.uphold_password;
//								item.uphold_2fa = uphold.uphold_2fa;
//								item.uphold_country = uphold.uphold_country;
//							}
//							else
//							{
//								item.uphold_email_password = uphold.uphold_email_password;
//								item.uphold_email_2fa = uphold.uphold_email_2fa;
//								item.uphold_recovery_email = uphold.uphold_recovery_email;

//								item.uphold_email = uphold.uphold_email;
//								item.uphold_password = uphold.uphold_password;
//								item.uphold_2fa = uphold.uphold_2fa;
//								item.uphold_country = uphold.uphold_country;
//							}
//						}

//						channels.Add(item);
//					}

//					result.IsSuccess = true;
//					result.Data = channels;
//				}
//			}
//			catch (Exception ex)
//			{
//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Exception("Method error GetChannelByType: ", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());

//				result.IsSuccess = false;
//				result.Message = ex.Message;
//			}
//			return result;
//		}

//		[HttpGet]
//		[Route("GetChannelTipFeed")]
//		public ApiResponse GetChannelTipFeed(string type)
//		{
//			var result = new ApiResponse();
//			try
//			{
//				using (var db = new DataContext())
//				{
//					var tipAction = SettingUtils.GetValue("brave_tip_current_tool");
//					var statusArray = new List<long> { 2 }; // Đang hoạt động
//					if (tipAction.Equals("TipFeed"))
//						statusArray = new List<long> { 2, 10, 16 }; // Đang hoạt động, No Wallet, Die Wallet

//					result.IsSuccess = true;
//					result.Data = db.brave_tip.Where(x => x.type.Equals(type) && statusArray.Contains(x.brave_tip_status_id) && x.channel_username != null).ToList();
//				}
//			}
//			catch (Exception ex)
//			{
//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Exception("Method error GetChannelTipFeed: ", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());

//				result.IsSuccess = false;
//				result.Message = ex.Message;
//			}
//			return result;
//		}

//		[HttpGet]
//		[Route("UpdateLastModifiedChannelByEmail")]
//		public ApiResponse UpdateLastModifiedChannelByEmail(string email)
//		{
//			var result = new ApiResponse();
//			try
//			{
//				using (var db = new DataContext())
//				{
//					var tip = db.brave_tip.FirstOrDefault(x => x.gmail_email.Trim().ToLower().Equals(email.Trim().ToLower()));
//					if (tip != null)
//					{
//						tip.feed_date = DateTime.Now;
//						tip.last_update_date = DateTime.Now;
//						db.Entry(tip).State = EntityState.Modified;
//						db.SaveChanges();

//						//if (tip.brave_wallet_id != null)
//						//{
//						//	var wallet = db.brave_wallet.Find(tip.brave_wallet_id);
//						//	if (wallet != null)
//						//	{
//						//		wallet.feed_date = DateTime.Now;
//						//		db.Entry(wallet).State = EntityState.Modified;
//						//		db.SaveChanges();
//						//	}
//						//}

//						result.IsSuccess = true;
//						result.Message = "Update sucessfully.";
//					}
//					else
//					{
//						result.IsSuccess = false;
//						result.Message = $"Không tìm thấy kênh email= {email}";
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				result.IsSuccess = false;
//				result.Message = $"Update fail: {ex.Message}";

//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Info("		email:	" + email);
//				Loggers.BraveAPILog.Exception("Method: UpdateLastModifiedChannelByEmail", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//			}

//			return result;
//		}

//		[HttpGet]
//		[Route("UpdateKarma")]
//		public ApiResponse UpdateKarma(string email, int karma)
//		{
//			var result = new ApiResponse();
//			try
//			{
//				using (var db = new DataContext())
//				{
//					var tip = db.brave_tip.FirstOrDefault(x => x.gmail_email.Trim().ToLower().Equals(email.Trim().ToLower()));
//					if (tip != null)
//					{
//						tip.reddit_karma = karma;
//						db.Entry(tip).State = EntityState.Modified;
//						db.SaveChanges();

//						result.IsSuccess = true;
//						result.Message = "Update sucessfully.";
//					}
//					else
//					{
//						result.IsSuccess = false;
//						result.Message = $"Không tìm thấy kênh email= {email}";
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				result.IsSuccess = false;
//				result.Message = $"Update fail: {ex.Message}";

//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Info($"		email:{email}, karma:{karma}");
//				Loggers.BraveAPILog.Exception("Method: UpdateKarma", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//			}

//			return result;
//		}

//		[HttpPost]
//		[Route("UpdateXrpWallet")]
//		public ApiResponse UpdateXrpWallet(BraveTipUpdateXrpRequest modal)
//		{
//			var result = new ApiResponse();
//			try
//			{
//				using (var db = new DataContext())
//				{
//					var tip = db.brave_tip.FirstOrDefault(x => x.gmail_email.Trim().ToLower().Equals(modal.email.Trim().ToLower()));
//					if (tip != null)
//					{
//						tip.last_update_date = DateTime.Now;
//						db.Entry(tip).State = EntityState.Modified;
//						db.SaveChanges();

//						result.IsSuccess = true;
//						result.Message = "Update xrp wallet sucessfully.";
//					}
//					else
//					{
//						result.IsSuccess = false;
//						result.Message = $"Không tìm thấy kênh email= {modal.email}";
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				result.IsSuccess = false;
//				result.Message = $"Update fail: {ex.Message}";

//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Info("		email:	" + modal.email);
//				Loggers.BraveAPILog.Exception("Method: UpdateXrpWallet", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//			}

//			return result;
//		}

//		[HttpGet]
//		[Route("GetConfiguration")]
//		public GetConfigurationResponse GetConfiguration(string email, string version)
//		{
//			var result = new GetConfigurationResponse();
//			try
//			{
//				using (var db = new DataContext())
//				{
//					var tip = db.brave_tip.FirstOrDefault(x => x.gmail_email.Trim().ToLower().Equals(email.Trim().ToLower()));
//					if (tip != null)
//					{
//						// update version
//						tip.last_update_date = DateTime.Now;
//						db.Entry(tip).State = EntityState.Modified;
//						db.SaveChanges();

//						// return configuration
//						var braveSetting = BraveTipUtils.GetBraveSetting(email);
//						var fileSetting = HttpContext.Current.Server.MapPath(Constants.ConfigsFolderPath) + "\\" + braveSetting.brave_setting_id + ".txt";
//						if (File.Exists(fileSetting))
//						{
//							result.ConfigurationFileName = $"{braveSetting.brave_setting_id}.txt";
//							result.CurrentTool = SettingUtils.GetValue("brave_tip_current_tool");
//							result.IsSuccess = true;
//							result.Message = "Sucessfully.";
//						}
//						else
//						{
//							result.IsSuccess = false;
//							result.Message = $"Không tồn tại file cấu hình {braveSetting.brave_setting_id}.txt của \"{braveSetting.name}\".";
//						}
//					}
//					else
//					{
//						result.IsSuccess = false;
//						result.Message = $"Không tồn tại kênh {email}.";
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				result.IsSuccess = false;
//				result.Message = $"Fail: {ex.Message}";

//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Info("		email:	" + email);
//				Loggers.BraveAPILog.Exception("Method: GetConfiguration", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//			}

//			return result;
//		}

//		[HttpGet]
//		[Route("GetCurrentTool")]
//		public string GetCurrentTool()
//		{
//			// return TipFeed/TipOpen
//			// TipFeed là kiểm tra xem pub bị under review ko, up hoặc gemini có die ko
//			return SettingUtils.GetValue("brave_tip_current_tool");
//		}

//		[HttpGet]
//		[Route("SetChecked")]
//		public ApiResponse SetChecked(string email)
//		{
//			var result = new ApiResponse();
//			try
//			{
//				using (var db = new DataContext())
//				{
//					var tip = db.brave_tip.FirstOrDefault(x => x.gmail_email.Trim().ToLower().Equals(email.Trim().ToLower()));
//					if (tip != null)
//					{
//						tip.is_checked = true;
//						tip.last_update_date = DateTime.Now;
//						db.Entry(tip).State = EntityState.Modified;
//						db.SaveChanges();

//						result.IsSuccess = true;
//						result.Message = "Update checked sucessfully.";
//					}
//					else
//					{
//						result.IsSuccess = false;
//						result.Message = $"Không tìm thấy kênh email= {email}";
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				result.IsSuccess = false;
//				result.Message = $"Update fail: {ex.Message}";

//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Info("		email:	" + email);
//				Loggers.BraveAPILog.Exception("Method: SetChecked", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//			}

//			return result;
//		}

//		[HttpPost]
//		[Route("SyncTipByStatus")]
//		public ApiResponse SyncTipByStatus(BraveTipSyncStatusRequest model)
//		{
//			var result = new ApiResponse();
//			try
//			{
//				if (!string.IsNullOrEmpty(model.channel_email_list))
//				{
//					using (var db = new DataContext())
//					{
//						var channels = model.channel_email_list.Split(',').ToList();
//						if (channels != null && channels.Count > 0)
//						{
//							var tips = db.brave_tip.Where(x => channels.Any(y => y.Trim().ToLower().Equals(x.gmail_email.Trim().ToLower())));
//							foreach (var item in tips)
//							{
//								item.brave_tip_status_id = model.status;
//								item.is_active = model.is_active;
//							}
//							db.SaveChanges();

//							result.IsSuccess = true;
//							result.Message = "Update sucessfully.";
//						}
//					}
//				}
//				else
//				{
//					result.IsSuccess = false;
//					result.Message = $"Lỗi dữ liệu channel list.";
//				}
//			}
//			catch (Exception ex)
//			{
//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Info("      EmailList: " + model.channel_email_list);
//				Loggers.BraveAPILog.Exception("Method error SyncTipByStatus: ", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//			}

//			return result;
//		}

//		[HttpPost]
//		[Route("SyncTipChecked")]
//		public ApiResponse SyncTipChecked(BraveTipUpdateUpholdStatusRequest model)
//		{
//			var result = new ApiResponse();
//			try
//			{
//				if (!string.IsNullOrEmpty(model.channel_email_list))
//				{
//					using (var db = new DataContext())
//					{
//						var channels = model.channel_email_list.Split(',').ToList();
//						var tips = db.brave_tip.Where(x => channels.Any(y => y.Trim().ToLower().Equals(x.gmail_email.Trim().ToLower())));
//						foreach (var item in tips)
//							item.is_checked = true;

//						db.SaveChanges();

//						result.IsSuccess = true;
//						result.Message = "Update sucessfully.";
//					}
//				}
//				else
//				{
//					result.IsSuccess = false;
//					result.Message = $"Lỗi dữ liệu channel list.";
//				}
//			}
//			catch (Exception ex)
//			{
//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Info("      EmailList: " + model.channel_email_list);
//				Loggers.BraveAPILog.Exception("Method error SyncTipChecked: ", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//			}
//			return result;
//		}

//		[HttpGet]
//		[Route("UpdateDiePublisher")]
//		public ApiResponse UpdateDiePublisher(string email)
//		{
//			var result = new ApiResponse();
//			try
//			{
//				using (var db = new DataContext())
//				{
//					var tip = db.brave_tip.FirstOrDefault(x => x.gmail_email.Trim().ToLower().Equals(email.Trim().ToLower()));
//					if (tip != null)
//					{
//						var note = $"- {DateTime.Now.ToString("dd/MM/yyyy")}: die publisher";

//						tip.is_active = false;
//						tip.brave_tip_status_id = 4;
//						tip.note = !string.IsNullOrEmpty(tip.note) ? $"{note}{Environment.NewLine}{tip.note}" : $"{note}";
//						tip.last_update_date = DateTime.Now;
//						db.Entry(tip).State = EntityState.Modified;
//						db.SaveChanges();

//						result.IsSuccess = true;
//						result.Message = $"Update die publisher {email} sucessfully.";
//						result.Data = $"- Ngày tạo:{tip.create_date.Value.ToString("dd/MM/yyyy")}" + Environment.NewLine + tip.note;
//					}
//					else
//					{
//						result.IsSuccess = false;
//						result.Message = $"Không tìm thấy kênh email= {email}";
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				result.IsSuccess = false;
//				result.Message = $"Update fail: {ex.Message}";

//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Info("		email:	" + email);
//				Loggers.BraveAPILog.Exception("Method: UpdateDiePublisher", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//			}

//			return result;
//		}

//		[HttpGet]
//		[Route("UpdateDiePublisher")]
//		public ApiResponse UpdateDiePublisher(string email, string note)
//		{
//			var result = new ApiResponse();
//			try
//			{
//				using (var db = new DataContext())
//				{
//					var tip = db.brave_tip.FirstOrDefault(x => x.gmail_email.Trim().ToLower().Equals(email.Trim().ToLower()));
//					if (tip != null)
//					{
//						note = $"- {DateTime.Now.ToString("dd/MM/yyyy")}: {note}";
//						tip.is_active = false;
//						tip.brave_tip_status_id = 4;
//						tip.note = !string.IsNullOrEmpty(tip.note) ? $"{note}{Environment.NewLine}{tip.note}" : $"{note}";
//						tip.last_update_date = DateTime.Now;
//						db.Entry(tip).State = EntityState.Modified;
//						db.SaveChanges();

//						result.IsSuccess = true;
//						result.Message = $"Update die publisher {email} sucessfully.";
//						result.Data = $"- Ngày tạo:{tip.create_date.Value.ToString("dd/MM/yyyy")}" + Environment.NewLine + tip.note;
//					}
//					else
//					{
//						result.IsSuccess = false;
//						result.Message = $"Không tìm thấy kênh email= {email}";
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				result.IsSuccess = false;
//				result.Message = $"Update fail: {ex.Message}";

//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Info("		email:	" + email);
//				Loggers.BraveAPILog.Exception("Method: UpdateDiePublisher", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//			}

//			return result;
//		}

//		[HttpGet]
//		[Route("UpdateDieWallet")]
//		public ApiResponse UpdateDieWallet(string email)
//		{
//			var result = new ApiResponse();
//			try
//			{
//				using (var db = new DataContext())
//				{
//					var tip = db.brave_tip.FirstOrDefault(x => x.gmail_email.Trim().ToLower().Equals(email.Trim().ToLower()));
//					if (tip != null)
//					{
//						var note = $"- {DateTime.Now.ToString("dd/MM/yyyy")}: die wallet(uphold/gemini)";
//						tip.is_active = true;
//						if (tip.brave_tip_status_id != 15) // nếu là die reddit thì ko update status
//							tip.brave_tip_status_id = 16;
//						tip.note = !string.IsNullOrEmpty(tip.note) ? $"{note}{Environment.NewLine}{tip.note}" : $"{note}";
//						tip.last_update_date = DateTime.Now;
//						db.Entry(tip).State = EntityState.Modified;
//						db.SaveChanges();

//						if (tip.brave_wallet_id != null)
//						{
//							var wallet = db.brave_wallet.Find(tip.brave_wallet_id);
//							if (wallet != null)
//							{
//								note = $"- {DateTime.Now.ToString("dd/MM/yyyy")}: die wallet(from pub {email})";
//								wallet.note = note;
//								wallet.is_die = true;
//								wallet.last_update_date = DateTime.Now;
//								db.Entry(wallet).State = EntityState.Modified;
//								db.SaveChanges();
//							}
//						}

//						result.IsSuccess = true;
//						result.Message = $"Update die wallet {email} sucessfully.";
//						result.Data = tip.note;
//					}
//					else
//					{
//						result.IsSuccess = false;
//						result.Message = $"Không tìm thấy kênh email= {email}";
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				result.IsSuccess = false;
//				result.Message = $"Update fail: {ex.Message}";

//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Info("		email:	" + email);
//				Loggers.BraveAPILog.Exception("Method: UpdateDieWallet", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//			}

//			return result;
//		}

//		[HttpGet]
//		[Route("UpdateDieReddit")]
//		public ApiResponse UpdateDieReddit(long braveTipId)
//		{
//			var result = new ApiResponse();
//			try
//			{
//				using (var db = new DataContext())
//				{
//					var tip = db.brave_tip.FirstOrDefault(x => x.brave_tip_id == braveTipId);
//					if (tip != null)
//					{
//						var note = $"- {DateTime.Now.ToString("dd/MM/yyyy")}: die reddit";
//						tip.is_active = false;
//						tip.brave_tip_status_id = 15;
//						tip.note = !string.IsNullOrEmpty(tip.note) ? $"{note}{Environment.NewLine}{tip.note}" : $"{note}";
//						tip.last_update_date = DateTime.Now;
//						db.Entry(tip).State = EntityState.Modified;
//						db.SaveChanges();

//						result.IsSuccess = true;
//						result.Message = $"Update die reddit {tip.gmail_email} sucessfully.";
//						result.Data = $"Update die reddit {tip.gmail_email} sucessfully.{Environment.NewLine}{tip.note}";
//					}
//					else
//					{
//						result.IsSuccess = false;
//						result.Message = $"Không tìm thấy kênh email= {braveTipId}";
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				result.IsSuccess = false;
//				result.Message = $"Update fail: {ex.Message}";

//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Info("		email:	" + braveTipId);
//				Loggers.BraveAPILog.Exception("Method: UpdateDieReddit", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//			}

//			return result;
//		}

//		[HttpGet]
//		[Route("UpdateNonWallet")]
//		public ApiResponse UpdateNonWallet(string email)
//		{
//			var result = new ApiResponse();
//			try
//			{
//				using (var db = new DataContext())
//				{
//					var note = "disconnect wallet";
//					var tip = db.brave_tip.FirstOrDefault(x => x.gmail_email.Trim().ToLower().Equals(email.Trim().ToLower()));
//					if (tip != null)
//					{
//						tip.note = !string.IsNullOrEmpty(tip.note) ? $"{note}{Environment.NewLine}{tip.note}" : $"{note}";
//						tip.brave_wallet_id = null;
//						tip.is_active = false;
//						tip.brave_tip_status_id = 19;
//						tip.last_update_date = DateTime.Now;
//						db.Entry(tip).State = EntityState.Modified;
//						db.SaveChanges();

//						result.IsSuccess = true;
//						result.Message = $"Disconnect wallet to {email} sucessfully.";
//					}
//					else
//					{
//						result.IsSuccess = false;
//						result.Message = $"Không tìm thấy kênh email= {email}";
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				result.IsSuccess = false;
//				result.Message = $"Update fail: {ex.Message}";

//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Info("		email:	" + email);
//				Loggers.BraveAPILog.Exception("Method: UpdateNonWallet", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//			}

//			return result;
//		}

//		[HttpGet]
//		[Route("UpdateNote")]
//		public ApiResponse UpdateNote(string email, string note)
//		{
//			var result = new ApiResponse();
//			try
//			{
//				using (var db = new DataContext())
//				{
//					var tip = db.brave_tip.FirstOrDefault(x => x.gmail_email.Trim().ToLower().Equals(email.Trim().ToLower()));
//					if (tip != null)
//					{
//						tip.note = !string.IsNullOrEmpty(tip.note) ? $"{note}{Environment.NewLine}{tip.note}" : $"{note}";
//						tip.last_update_date = DateTime.Now;
//						db.Entry(tip).State = EntityState.Modified;
//						db.SaveChanges();

//						result.IsSuccess = true;
//						result.Message = $"Update note {email} sucessfully.";
//					}
//					else
//					{
//						result.IsSuccess = false;
//						result.Message = $"Không tìm thấy kênh email= {email}";
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				result.IsSuccess = false;
//				result.Message = $"Update fail: {ex.Message}";

//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Info("		email:	" + email);
//				Loggers.BraveAPILog.Exception("Method: UpdateNote", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//			}

//			return result;
//		}

//		[HttpGet]
//		[Route("UpdatePayFeed")]
//		public ApiResponse UpdatePayFeed(string email, string payDate, string note)
//		{
//			var result = new ApiResponse();
//			try
//			{
//				using (var db = new DataContext())
//				{
//					var tip = db.brave_tip.FirstOrDefault(x => x.gmail_email.Trim().ToLower().Equals(email.Trim().ToLower()));
//					if (tip != null)
//					{
//						if (!"-".Equals(payDate))
//						{
//							try
//							{
//								tip.pay_date = Convert.ToDateTime(payDate);
//							}
//							catch { }
//						}
//						tip.note = !string.IsNullOrEmpty(tip.note) ? $"{note}{Environment.NewLine}{tip.note}" : $"{note}";
//						tip.last_update_date = DateTime.Now;

//						db.Entry(tip).State = EntityState.Modified;
//						db.SaveChanges();

//						result.IsSuccess = true;
//						result.Message = $"Update note {email} sucessfully.";
//					}
//					else
//					{
//						result.IsSuccess = false;
//						result.Message = $"Không tìm thấy kênh email= {email}";
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				result.IsSuccess = false;
//				result.Message = $"Update fail: {ex.Message}";

//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Info("		email:	" + email);
//				Loggers.BraveAPILog.Exception("Method: UpdateNote", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//			}

//			return result;
//		}

//		[HttpGet]
//		[Route("UpdateNoteByPubId")]
//		public ApiResponse UpdateNoteByPubId(long braveTipId, string note)
//		{
//			var result = new ApiResponse();
//			try
//			{
//				using (var db = new DataContext())
//				{
//					var tip = db.brave_tip.FirstOrDefault(x => x.brave_tip_id == braveTipId);
//					if (tip != null)
//					{
//						tip.note = !string.IsNullOrEmpty(tip.note) ? $"{note}{Environment.NewLine}{tip.note}" : $"{note}";
//						tip.last_update_date = DateTime.Now;
//						db.SaveChanges();

//						result.IsSuccess = true;
//						result.Message = $"Update note braveTipId:{braveTipId} sucessfully.";
//					}
//					else
//					{
//						result.IsSuccess = false;
//						result.Message = $"Không tìm thấy kênh braveTipId:{braveTipId}";
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				result.IsSuccess = false;
//				result.Message = $"Update fail: {ex.Message}";

//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Info("		braveTipId:	" + braveTipId);
//				Loggers.BraveAPILog.Exception("Method: UpdateNoteByPubId", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//			}

//			return result;
//		}

//		[HttpGet]
//		[Route("UpdateNoteByPubEmail")]
//		public ApiResponse UpdateNoteByPubEmail(string email, string note)
//		{
//			var result = new ApiResponse();
//			try
//			{
//				using (var db = new DataContext())
//				{
//					var tip = db.brave_tip.FirstOrDefault(x => x.gmail_email.Trim().ToLower() == email.Trim().ToLower());
//					if (tip != null)
//					{
//						tip.note = !string.IsNullOrEmpty(tip.note) ? $"{note}{Environment.NewLine}{tip.note}" : $"{note}";
//						tip.last_update_date = DateTime.Now;
//						db.SaveChanges();

//						result.IsSuccess = true;
//						result.Message = $"Update note email:{email} sucessfully.";
//					}
//					else
//					{
//						result.IsSuccess = false;
//						result.Message = $"Không tìm thấy kênh email:{email}";
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				result.IsSuccess = false;
//				result.Message = $"Update fail: {ex.Message}";

//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Info("		email:	" + email);
//				Loggers.BraveAPILog.Exception("Method: UpdateNoteByPubEmail", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//			}

//			return result;
//		}

//		[HttpGet]
//		[Route("UpdatePayNote")]
//		public ApiResponse UpdatePayNote(string email, string note)
//		{
//			var result = new ApiResponse();
//			try
//			{
//				using (var db = new DataContext())
//				{
//					var tip = db.brave_tip.FirstOrDefault(x => x.gmail_email.Trim().ToLower().Equals(email.Trim().ToLower()));
//					if (tip != null)
//					{
//						tip.note = !string.IsNullOrEmpty(tip.note) ? $"{note}{Environment.NewLine}{tip.note}" : $"{note}";
//						tip.pay_date = DateTime.Now;
//						tip.last_update_date = DateTime.Now;
//						db.Entry(tip).State = EntityState.Modified;
//						db.SaveChanges();

//						result.IsSuccess = true;
//						result.Message = $"Update note {email} sucessfully.";
//					}
//					else
//					{
//						result.IsSuccess = false;
//						result.Message = $"Không tìm thấy kênh email= {email}";
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				result.IsSuccess = false;
//				result.Message = $"Update fail: {ex.Message}";

//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Info("		email:	" + email);
//				Loggers.BraveAPILog.Info("		note:	" + note);
//				Loggers.BraveAPILog.Exception("Method: UpdatePayNote", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//			}

//			return result;
//		}

//		[HttpPost]
//		[Route("UpdateTipLocaiton")]
//		public ApiResponse UpdateTipLocaiton(BraveTipUpdateLocationRequest model)
//		{
//			var result = new ApiResponse();
//			try
//			{
//				if (!string.IsNullOrEmpty(model.channel_email_list) && !string.IsNullOrEmpty(model.location_name))
//				{
//					using (var db = new DataContext())
//					{
//						var tipLocation = db.brave_tip_location.FirstOrDefault(x => x.name.Trim().ToLower().Equals(model.location_name.Trim().ToLower()));
//						if (tipLocation != null)
//						{
//							var channels = model.channel_email_list.Split(',').ToList();
//							var tips = db.brave_tip.Where(x => channels.Any(y => y.Trim().ToLower().Equals(x.gmail_email.Trim().ToLower())));
//							foreach (var item in tips)
//							{
//								item.brave_tip_location_id = tipLocation.brave_tip_location_id;
//							}
//							db.SaveChanges();

//							result.IsSuccess = true;
//							result.Message = "Update sucessfully.";
//						}
//						else
//						{
//							result.IsSuccess = false;
//							result.Message = $"Không tìm thấy {model.location_name} trong database.";
//						}
//					}
//				}
//				else
//				{
//					result.IsSuccess = false;
//					result.Message = $"Lỗi dữ liệu channel list hoặc location name.";
//				}
//			}
//			catch (Exception ex)
//			{
//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Info("      AddNewVideo EmailList: " + model.channel_email_list);
//				Loggers.BraveAPILog.Info("      AddNewVideo LocationName: " + model.location_name);
//				Loggers.BraveAPILog.Exception("Method error UpdateTipLocaiton: ", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//			}
//			return result;
//		}

//		[HttpPost]
//		[Route("SyncDieUphold")]
//		public ApiResponse SyncDieUphold(BraveTipUpdateUpholdStatusRequest model)
//		{
//			var result = new ApiResponse();
//			try
//			{
//				if (!string.IsNullOrEmpty(model.channel_email_list))
//				{
//					using (var db = new DataContext())
//					{
//						var channels = model.channel_email_list.Split(',').ToList();
//						var tips = db.brave_tip.Where(x => channels.Any(y => y.Trim().ToLower().Equals(x.gmail_email.Trim().ToLower())));
//						foreach (var item in tips)
//						{
//							item.brave_tip_status_id = 16; // die uphold
//							item.is_active = false;
//						}
//						db.SaveChanges();

//						result.IsSuccess = true;
//						result.Message = "Update sucessfully.";
//					}
//				}
//				else
//				{
//					result.IsSuccess = false;
//					result.Message = $"Lỗi dữ liệu channel list.";
//				}
//			}
//			catch (Exception ex)
//			{
//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Info("      EmailList: " + model.channel_email_list);
//				Loggers.BraveAPILog.Exception("Method error UpdateUpholdDieStatus: ", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//			}
//			return result;
//		}

//		[HttpPost]
//		[Route("SyncTipActive")]
//		public ApiResponse SyncTipActive(BraveTipUpdateStatusRequest model)
//		{
//			var result = new ApiResponse();
//			try
//			{
//				if (!string.IsNullOrEmpty(model.channel_email_list))
//				{
//					using (var db = new DataContext())
//					{
//						var channels = model.channel_email_list.Split(',').ToList();
//						var tips = db.brave_tip.Where(x => channels.Any(y => y.Trim().ToLower().Equals(x.gmail_email.Trim().ToLower())));
//						foreach (var item in tips)
//						{
//							item.brave_tip_status_id = 2;
//							item.is_active = true;
//						}
//						db.SaveChanges();

//						result.IsSuccess = true;
//						result.Message = "Update sucessfully.";
//					}
//				}
//				else
//				{
//					result.IsSuccess = false;
//					result.Message = $"Lỗi dữ liệu channel list.";
//				}
//			}
//			catch (Exception ex)
//			{
//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Info("      EmailList: " + model.channel_email_list);
//				Loggers.BraveAPILog.Exception("Method error SyncTipActive: ", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//			}
//			return result;
//		}

//		[HttpGet]
//		[Route("IsTipFull")]
//		public bool IsTipFull()
//		{
//			try
//			{
//				return Convert.ToBoolean(SettingUtils.GetValue("brave_tip_is_tip_full"));
//			}
//			catch (Exception ex)
//			{
//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Exception("IsTipFull()", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				return false;
//			}
//		}

//		[HttpGet]
//		[Route("GetConfigTip")]
//		public GetConfigTipResponse GetConfigTip()
//		{
//			try
//			{
//				return new GetConfigTipResponse()
//				{
//					IsSuccess = true,
//					brave_tip_is_tip_full = Convert.ToBoolean(SettingUtils.GetValue("brave_tip_is_tip_full")),
//					brave_tip_max_tip = Convert.ToInt32(SettingUtils.GetValue("brave_tip_max_tip")),
//				};
//			}
//			catch (Exception ex)
//			{
//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Exception("GetConfigTip()", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());

//				return new GetConfigTipResponse()
//				{
//					IsSuccess = true,
//					Message = ex.Message
//				};
//			}
//		}

//		[HttpGet]
//		[Route("IsTipFollowRedirectUrl")]
//		public bool IsTipFollowRedirectUrl()
//		{
//			try
//			{
//				return Convert.ToBoolean(SettingUtils.GetValue("brave_tip_follow_redirect_url"));
//			}
//			catch (Exception ex)
//			{
//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Exception("IsTipFollowRedirectUrl()", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				return false;
//			}
//		}

//		#endregion brave_tip

//		#region brave_tip_url

//		[HttpGet]
//		[Route("GetRandomTipUrl")]
//		public RedditUrlResponse GetRandomTipUrl()
//		{
//			var result = new RedditUrlResponse();
//			try
//			{
//				using (var db = new DataContext())
//				{
//					var url = db.brave_tip_url
//						.Where(x => x.youtube_video_url.Contains("reddit.com/")
//							|| x.youtube_video_url.Contains("youtu.be/"))
//						.OrderBy(x => Guid.NewGuid()).Take(10).FirstOrDefault();

//					result.IsSuccess = true;
//					result.brave_tip_url_id = url.brave_tip_url_id;
//					result.url = url.youtube_video_url;
//				}
//			}
//			catch (Exception ex)
//			{
//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Exception("Method error RedditUrlResponse: ", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//			}
//			return result;
//		}

//		[HttpGet]
//		[Route("GetRandomRedditUrl")]
//		public RedditUrlResponse GetRandomRedditUrl()
//		{
//			var result = new RedditUrlResponse();
//			try
//			{
//				using (var db = new DataContext())
//				{
//					var url = db.brave_tip_url.Include("brave_tip")
//						.Where(x => x.youtube_video_url.Contains("reddit.com/")
//							&& x.brave_tip.brave_tip_status_id != 4
//							&& x.brave_tip.brave_tip_status_id != 15
//							&& x.brave_tip.brave_tip_status_id != 17)
//						.OrderBy(x => Guid.NewGuid()).Take(10).FirstOrDefault();

//					result.IsSuccess = true;
//					result.brave_tip_url_id = url.brave_tip_url_id;
//					result.url = url.youtube_video_url;
//				}
//			}
//			catch (Exception ex)
//			{
//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Exception("Method error GetRandomRedditUrl: ", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//			}
//			return result;
//		}

//		#endregion brave_tip_url

//		[HttpGet]
//		[Route("Get2Fa")]
//		public async Task<ApiResponse> Get2FaAsync(string _2fa)
//		{
//			var result = new ApiResponse();
//			try
//			{
//				using (var client = new HttpClient())
//				{
//					var res = await client.GetAsync("http://2fa.live/tok/" + _2fa);
//					if (res.IsSuccessStatusCode)
//					{
//						var rmpResponse = await res.Content.ReadAsStringAsync();
//						var obj = JsonConvert.DeserializeObject<Token2FaLive>(rmpResponse);
//						result.IsSuccess = true;
//						result.Data = obj.token;
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Exception("Method error Get2Fa: ", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//			}

//			result.IsSuccess = false;

//			return result;
//		}

//		#region Reddit account

//		[HttpGet]
//		[Route("GetFeedAccount")]
//		public brave_tip GetFeedAccount(string email)
//		{
//			using (var db = new DataContext())
//			{
//				var tip = this.GetChannelByEmail(email);
//				if (tip != null)
//				{
//					if (tip.is_sync_feed == false)
//					{
//						tip.is_sync_feed = true;
//						db.Entry(tip).State = EntityState.Modified;
//						db.SaveChanges();
//					}

//					return tip;
//				}
//			}

//			return null;
//		}

//		[HttpGet]
//		[Route("GetRedditRegisterAccount")]
//		public brave_tip GetRedditRegisterAccount()
//		{
//			try
//			{
//				using (var db = new DataContext())
//				{
//					return db.brave_tip.FirstOrDefault(x => x.brave_tip_status_id == 2 && x.is_sync_feed != true);
//				}
//			}
//			catch (Exception ex)
//			{
//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Exception("Method error GetRedditRegisterAccount: ", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//			}
//			return null;
//		}

//		#endregion Reddit account

//		#region Uphold
//		[HttpGet]
//		[Route("GetUpholdByEmail")]
//		public ApiResponse GetUpholdByEmail(string email)
//		{
//			var result = new ApiResponse();
//			try
//			{
//				using (var db = new DataContext())
//				{
//					var uphold = db.brave_tip.FirstOrDefault(x => x.uphold_email.Trim().ToLower().Equals(email.Trim().ToLower())
//						&& !string.IsNullOrEmpty(x.uphold_email_2fa)
//						&& !string.IsNullOrEmpty(x.uphold_recovery_email));
//					if (uphold == null)
//					{
//						uphold = db.brave_tip.FirstOrDefault(x => x.uphold_email.Trim().ToLower().Equals(email.Trim().ToLower())
//							&& !string.IsNullOrEmpty(x.uphold_email_2fa));
//						if (uphold == null)
//						{
//							uphold = db.brave_tip.FirstOrDefault(x => x.uphold_email.Trim().ToLower().Equals(email.Trim().ToLower())
//								&& !string.IsNullOrEmpty(x.uphold_recovery_email));
//							if (uphold == null)
//								uphold = db.brave_tip.FirstOrDefault(x => x.uphold_email.Trim().ToLower().Equals(email.Trim().ToLower()));
//						}
//					}

//					if (uphold != null)
//					{
//						if (string.IsNullOrEmpty(uphold.uphold_email_password))
//						{
//							uphold.uphold_email_password = uphold.gmail_password;
//							uphold.uphold_email_2fa = uphold.gmail_2fa;
//						}

//						result.IsSuccess = true;
//						result.Message = "Success";
//						result.Data = uphold;
//					}
//					else
//					{
//						result.IsSuccess = false;
//						result.Message = $"Not found {email}";
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				result.IsSuccess = false;
//				result.Message = $"Error {ex.Message}";

//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Exception("Method error GetUpholdByEmail: ", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//			}

//			return result;
//		}
//		#endregion

//		#region Link wallet to pub die wallet
//		[HttpGet]
//		[Route("SetInfoFixDieWallet")]
//		public ApiResponse SetInfoFixDieWallet(string pubEmail, long braveWalletId)
//		{
//			var result = new LinkDieWalletResponse();
//			try
//			{
//				using (var db = new DataContext())
//				{
//					var braveTip = db.brave_tip.FirstOrDefault(x => x.gmail_email.Trim().ToLower() == pubEmail.Trim().ToLower());
//					if (braveTip != null)
//					{
//						// thiết lập Die Wallet cho tài khoản
//						var note = $"- Active for tip {DateTime.Now.ToString("dd/MM/yyyy")}";
//						braveTip.note = !string.IsNullOrEmpty(braveTip.note) ? $"{note}{Environment.NewLine}{braveTip.note}" : $"{note}";
//						braveTip.brave_tip_status_id = 2; // Trạng thái tài khoản tip là 2 - "Đang hoạt động"
//						braveTip.is_active = true; // kích hoạt cho tip luôn
//						braveTip.last_update_date = DateTime.Now;
//						db.Entry(braveTip).State = EntityState.Modified;
//						db.SaveChanges();

//						result.IsSuccess = true;
//						result.Message = $"Update successfuly {pubEmail}!";
//					}
//					else
//					{
//						result.IsSuccess = false;
//						result.Message = $"Not found {pubEmail}!";
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				result.IsSuccess = false;
//				result.Message = $"Lỗi: {ex}";

//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Exception("SetInfoFixDieWallet", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//			}

//			return result;
//		}

//		[HttpGet]
//		[Route("GetInfoFixDieWallet")]
//		public LinkDieWalletResponse GetInfoFixDieWallet(string braveTipType)
//		{
//			var result = new LinkDieWalletResponse();
//			try
//			{
//				using (var db = new DataContext())
//				{
//					///
//					/// lấy 1 pub thỏa mãn:
//					/// - Có type = braveTipType truyền vào
//					/// - Và lấy tài khoản đang có trạng thái là die wallet (16)
//					/// - Và lấy tài khoản (chưa gắn ví hoặc gắn ví rùi nhưng ví die)
//					var braveTip = db.GetInfoFixDieWallet(braveTipType);
//					if (braveTip == null)
//					{
//						// tìm ví die, sau đó tìm tài khoản đã liên kết với ví die
//						var dieWallets = db.brave_wallet.Where(x => x.is_die == true).Select(x => x.brave_wallet_id).ToList();
//						if (dieWallets.Count > 0)
//							braveTip = db.brave_tip.FirstOrDefault(x => braveTipType.Equals(x.type) && dieWallets.Contains(x.brave_wallet_id.Value));
//					}

//					// tìm được tk die wallet hoặc chưa gắn ví
//					if (braveTip != null)
//					{
//						// thiết lập Die Wallet cho tài khoản
//						braveTip.brave_tip_status_id = 16; // Die Wallet
//						braveTip.last_update_date = DateTime.Now;
//						db.Entry(braveTip).State = EntityState.Modified;
//						db.SaveChanges();

//						// kiểm tra nếu ví hiện tại nếu die hoặc chưa gắn ví thì lấy 1 ví (chưa gắn vào pub nào) và gắn vào pub
//						if (braveTip.brave_wallet_id == null // chưa gắn ví
//							|| db.brave_wallet.FirstOrDefault(x => x.brave_wallet_id == braveTip.brave_wallet_id)?.is_die == true) // ví die
//						{
//							// lấy 1 wallet - (ko die và chưa gắn pub) sau đó gắn nó vào WalletId của pub vừa tìm được (ví mà gắn vào pub mà pub có status 4,5,10,17 vẫn được tính miễn chưa die)
//							var wallet = db.brave_wallet.FirstOrDefault(x => x.is_die == false
//								&& !x.brave_tip.Where(z => (z.brave_tip_status_id == 2 || z.brave_tip_status_id == 15)).Select(y => y.brave_wallet_id).Contains(x.brave_wallet_id));
//							if (wallet == null) // không có ví để gắn thì thông báo
//							{
//								result.IsSuccess = false;
//								result.StatusCode = Error.ERROR_TIP_02_NO_WALLET;
//								result.Message = $"Lỗi: không còn ví để add vào kênh {braveTip.gmail_email}.";
//								return result;
//							}

//							// cập nhật WalletID vào wallet id của bảng brave_tip
//							var note = $"- Add Wallet {DateTime.Now.ToString("dd/MM/yyyy")}";
//							braveTip.note = !string.IsNullOrEmpty(braveTip.note) ? $"{note}{Environment.NewLine}{braveTip.note}" : $"{note}";
//							braveTip.brave_wallet_id = wallet.brave_wallet_id; // cập nhật ví
//							braveTip.last_update_date = DateTime.Now;
//							db.Entry(braveTip).State = EntityState.Modified;
//							db.SaveChanges();
//						}

//						// trả về thông tin của bản ghi wallet id.
//						result.brave_tip = braveTip;

//						result.IsSuccess = true;
//						result.Message = $"Success add wallet {result.brave_tip.brave_wallet?.email} to tip account {result.brave_tip?.gmail_email}.";
//					}
//					else
//					{
//						result.IsSuccess = false;
//						result.StatusCode = Error.ERROR_TIP_03_NOT_FOUND_ACCOUNT_FOR_ADD_WALLET;
//						result.Message = $"Không còn tài khoản chưa gắn ví hoặc die ví.";
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				result.IsSuccess = false;
//				result.Message = $"Lỗi: {ex}";

//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Exception("LinkDieWalletAccount", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//			}

//			return result;
//		}

//		#endregion

//		#region AddWallet To OldPub or DieWalletPub
//		[HttpGet]
//		[Route("GetPubNoWallet")]
//		public brave_tip GetPubNoWallet(string braveTipType)
//		{
//			try
//			{
//				using (var db = new DataContext())
//				{
//					// brave_tip_status_id là 2 hoặc 10 vì 10 thì là No wallet, còn 2 trong trường hợp cho kênh đó tip trước
//					return db.brave_tip.FirstOrDefault(x => x.brave_wallet_id == null && x.type.Equals(braveTipType) && (x.brave_tip_status_id == 10 || x.brave_tip_status_id == 2));
//				}
//			}
//			catch (Exception ex)
//			{
//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Exception("GetPubNoWallet", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//			}

//			return null;
//		}

//		#endregion

//		#region Transfer old pub to gologin
//		[HttpGet]
//		[Route("GetChannelForGoLogin")]
//		public brave_tip GetChannelForGoLogin()
//		{
//			try
//			{
//				using (var db = new DataContext())
//				{
//					var tip = db.brave_tip.FirstOrDefault(x => "vmware".Equals(x.type)
//						&& x.brave_tip_status_id != 4
//						&& x.brave_tip_status_id != 17);
//					if (tip != null)
//					{
//						var uphold = db.brave_tip.FirstOrDefault(x => x.brave_tip_id == tip.uphold_link_id);
//						if (uphold != null)
//						{
//							// nếu email uphold giống email pub thì lấy email pass và 2fa email của pub
//							if (uphold.gmail_email.Equals(uphold.uphold_email))
//							{
//								tip.uphold_email_password = uphold.gmail_password;
//								tip.uphold_email_2fa = uphold.gmail_2fa;
//								tip.uphold_recovery_email = uphold.gmail_recovery_email;

//								tip.uphold_email = uphold.uphold_email;
//								tip.uphold_password = uphold.uphold_password;
//								tip.uphold_2fa = uphold.uphold_2fa;
//								tip.uphold_country = uphold.uphold_country;
//							}
//							else
//							{
//								tip.uphold_email_password = uphold.uphold_email_password;
//								tip.uphold_email_2fa = uphold.uphold_email_2fa;
//								tip.uphold_recovery_email = uphold.uphold_recovery_email;

//								tip.uphold_email = uphold.uphold_email;
//								tip.uphold_password = uphold.uphold_password;
//								tip.uphold_2fa = uphold.uphold_2fa;
//								tip.uphold_country = uphold.uphold_country;
//							}
//						}

//						return tip;
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Exception("Method error GetChannelForGoLogin: ", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//			}
//			return null;
//		}

//		[HttpGet]
//		[Route("UpdateChannelForGoLogin")]
//		public ApiResponse UpdateChannelForGoLogin(string email, string vpn, string type)
//		{
//			var result = new ApiResponse();
//			try
//			{
//				using (var db = new DataContext())
//				{
//					var tip = db.brave_tip.FirstOrDefault(x => x.gmail_email.Trim().ToLower().Equals(email.Trim().ToLower()));
//					if (tip != null)
//					{
//						var note = $"- {DateTime.Now.ToString("dd/MM/yyyy")}: transfer to GoLogin[{type}]";
//						tip.type = type;
//						tip.vpn_name = vpn;
//						tip.note = !string.IsNullOrEmpty(tip.note) ? $"{note}{Environment.NewLine}{tip.note}" : $"{note}";
//						tip.last_update_date = DateTime.Now;
//						db.Entry(tip).State = EntityState.Modified;
//						db.SaveChanges();

//						result.IsSuccess = true;
//						result.Message = $"Update {email} to GoLogin sucessfully.";
//						result.Data = tip.note;
//					}
//					else
//					{
//						result.IsSuccess = false;
//						result.Message = $"Không tìm thấy kênh email= {email}";
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				result.IsSuccess = false;
//				result.Message = $"Update fail: {ex.Message}";

//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Info("		email:	" + email);
//				Loggers.BraveAPILog.Exception("Method: UpdateChannelForGoLogin", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//			}

//			return result;
//		}

//		[HttpGet]
//		[Route("UpdateVpnName")]
//		public ApiResponse UpdateVpnName(string email, string vpn)
//		{
//			var result = new ApiResponse();
//			try
//			{
//				using (var db = new DataContext())
//				{
//					var tip = db.brave_tip.FirstOrDefault(x => x.gmail_email.Trim().ToLower().Equals(email.Trim().ToLower()));
//					if (tip != null)
//					{
//						var note = $"- {DateTime.Now.ToString("dd/MM/yyyy")}: update from {tip.vpn_name} to {vpn}";
//						tip.vpn_name = vpn;
//						tip.note = !string.IsNullOrEmpty(tip.note) ? $"{note}{Environment.NewLine}{tip.note}" : $"{note}";
//						tip.last_update_date = DateTime.Now;
//						db.Entry(tip).State = EntityState.Modified;
//						db.SaveChanges();

//						result.IsSuccess = true;
//						result.Message = $"Update vpn {email} sucessfully.";
//						result.Data = tip.note;
//					}
//					else
//					{
//						result.IsSuccess = false;
//						result.Message = $"Không tìm thấy kênh email= {email}";
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				result.IsSuccess = false;
//				result.Message = $"Update fail: {ex.Message}";

//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Info("		email:	" + email);
//				Loggers.BraveAPILog.Exception("Method: UpdateVpnName", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//			}

//			return result;
//		}

//		[HttpGet]
//		[Route("UpdateChannelVpn")]
//		public ApiResponse UpdateChannelVpn(string email, string vpn)
//		{
//			var result = new ApiResponse();
//			try
//			{
//				using (var db = new DataContext())
//				{
//					var tip = db.brave_tip.FirstOrDefault(x => x.gmail_email.Trim().ToLower().Equals(email.Trim().ToLower()));
//					if (tip != null)
//					{
//						var note = $"- {DateTime.Now.ToString("dd/MM/yyyy")}: update channel vpn from {tip.vpn_name} to {vpn}";
//						tip.channel_vpn = vpn;
//						tip.note = !string.IsNullOrEmpty(tip.note) ? $"{note}{Environment.NewLine}{tip.note}" : $"{note}";
//						tip.last_update_date = DateTime.Now;
//						db.Entry(tip).State = EntityState.Modified;
//						db.SaveChanges();

//						result.IsSuccess = true;
//						result.Message = $"Update channel vpn {email} sucessfully.";
//						result.Data = tip.note;
//					}
//					else
//					{
//						result.IsSuccess = false;
//						result.Message = $"Không tìm thấy kênh email= {email}";
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				result.IsSuccess = false;
//				result.Message = $"Update fail: {ex.Message}";

//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Info($"		email:	{email}, vpn: {vpn}");
//				Loggers.BraveAPILog.Exception("Method: UpdateChannelVpn", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//			}

//			return result;
//		}
//		#endregion

//		#region SearchVPN
//		[HttpGet]
//		[Route("SearchTipVpn")]
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

//		#endregion

//		[HttpGet]
//		[Route("EnablePayAccountForTip")]
//		public ApiResponse EnablePayAccountForTip()
//		{
//			// https://xuanthulab.net/asp-net-core-gui-mail-trong-ung-dung-web-asp-net.html
//			/* 
//			 * 1. Phải bật Enhanced Safe Browsing https://myaccount.google.com/u/2/account-enhanced-safe-browsing
//			 * 2. Mật khẩu App password: https://myaccount.google.com/u/2/apppasswords
//			 * 3. Enable IMAP(Enable IMA): https://mail.google.com/mail/u/2/?ogbl#settings/fwdandpop
//			 */

//			var result = new ApiResponse();
//			var imap = new Imap4Client();
//			try
//			{
//				// cập nhật is_checked của brave_wallet và brave_tip là 0
//				using (var db = new DataContext())
//				{
//					foreach (brave_tip tip in db.brave_tip.Where(x => x.brave_tip_status_id != 4 && x.brave_tip_status_id != 17))
//						tip.is_checked = false;

//					foreach (brave_wallet tip in db.brave_wallet.Where(x => x.is_die == false))
//						tip.is_checked = false;

//					db.SaveChanges();
//				}

//				imap.ConnectSsl("imap.gmail.com", 993);
//				imap.Login("game.nvh@gmail.com", "sqloyjifdxhfqjxb"); // => 2. Mật khẩu App password
//				var inbox = imap.SelectMailbox("inbox");
//				for (int n = 1; n < inbox.MessageCount + 1; n++)
//				{
//					var newMessage = inbox.Fetch.MessageObject(n);
//					if (newMessage.Subject.Contains("You received"))
//					{
//						var toEmail = newMessage.To[0].Email;
//						using (var db = new DataContext())
//						{
//							var wallet = db.brave_wallet.FirstOrDefault(x => x.email.Trim().ToLower().Equals(toEmail.Trim().ToLower()));
//							if (wallet != null)
//							{
//								// cập nhật is_check của brave_wallet
//								wallet.is_checked = true;

//								// cập nhật is_check của brave_tip
//								db.brave_tip.Where(x => x.brave_wallet_id == wallet.brave_wallet_id).ToList().ForEach(y => y.is_checked = true);
//								db.SaveChanges();
//							}
//						}
//					}
//				}

//				result.IsSuccess = true;
//				result.Message = $"Set is_checked=1 for pay tip sucessfully.";
//			}
//			catch (Imap4Exception iex)
//			{
//				result.IsSuccess = false;
//				result.Message = iex.Message;
//				Loggers.BraveMVCLog.Exception("Method: EnablePayAccountForTip", iex);
//			}
//			catch (Exception ex)
//			{
//				result.IsSuccess = false;
//				result.Message = ex.Message;
//				Loggers.BraveMVCLog.Exception("Method: EnablePayAccountForTip", ex);
//			}

//			finally
//			{
//				if (imap.IsConnected)
//				{
//					imap.Disconnect();
//				}
//			}

//			return result;
//		}

//		[HttpGet]
//		[Route("GetIP")]
//		public string GetIP()
//		{
//			string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
//			if (string.IsNullOrEmpty(ip))
//			{
//				ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
//			}
//			return ip;
//		}

//		#region Reset2Fa của pub die
//		[HttpGet]
//		[Route("GetPubDie")]
//		public brave_tip GetPubDie(string braveTipType)
//		{
//			try
//			{
//				using (var db = new DataContext())
//				{
//					return db.brave_tip.FirstOrDefault(x => x.brave_tip_status_id == 4 && x.type.ToLower().Equals(braveTipType.ToLower()));
//				}
//			}
//			catch (Exception ex)
//			{
//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Exception("GetPubDie", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//			}

//			return null;
//		}

//		[HttpGet]
//		[Route("UpdatePubDie")]
//		public ApiResponse UpdatePubDie(string pubEmail, bool isPubDie)
//		{
//			var result = new ApiResponse();
//			try
//			{
//				using (var db = new DataContext())
//				{
//					var tip = db.brave_tip.FirstOrDefault(x => x.gmail_email.Trim().ToLower().Equals(pubEmail.Trim().ToLower()));
//					if (tip != null)
//					{
//						// nếu pub chưa die thì sẽ đánh dấu là up đó được unlink
//						if (isPubDie == false)
//						{
//							var braveWallet = db.brave_wallet.FirstOrDefault(x => x.brave_wallet_id == tip.brave_wallet_id);
//							if (braveWallet != null)
//							{
//								var note = $"- {DateTime.Now.ToString("dd/MM/yyyy")}: unlink with pub {tip.gmail_email}";
//								braveWallet.note = !string.IsNullOrEmpty(braveWallet.note) ? $"{note}{Environment.NewLine}{braveWallet.note}" : $"{note}";
//								braveWallet.last_update_date = DateTime.Now;
//								db.SaveChanges();
//							}
//						}

//						var note1 = $"- {DateTime.Now.ToString("dd/MM/yyyy")}: reset 2fa";
//						tip.brave_tip_status_id = 17; // bỏ hoàn toàn
//						tip.is_active = false;
//						//  nếu pub chưa die thì mới unlink up ra khỏi pub
//						if (isPubDie == false)
//							tip.brave_wallet_id = null;
//						tip.note = !string.IsNullOrEmpty(tip.note) ? $"{note1}{Environment.NewLine}{tip.note}" : $"{note1}";
//						tip.last_update_date = DateTime.Now;
//						db.Entry(tip).State = EntityState.Modified;
//						db.SaveChanges();

//						result.IsSuccess = true;
//						result.Message = $"Reset 2fa {pubEmail} successfuly!";
//					}
//					else
//					{
//						result.IsSuccess = false;
//						result.Message = $"Not found pub  {pubEmail}";
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Exception("UpdatePubDie", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());

//				result.IsSuccess = false;
//				result.Message = ex.Message;
//			}

//			return result;
//		}

//		#endregion

//		#region Check Live pub
//		[HttpGet]
//		[Route("CheckLivePub")]
//		public brave_tip CheckLivePub()
//		{
//			try
//			{
//				using (var db = new DataContext())
//				{
//					return db.CheckLivePub();
//				}
//			}
//			catch (Exception ex)
//			{
//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Exception("CheckLivePub", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//			}

//			return null;
//		}

//		[HttpGet]
//		[Route("UpdateLivePub")]
//		public ApiResponse UpdateLivePub(string email, bool isLive)
//		{
//			var result = new ApiResponse();
//			try
//			{
//				using (var db = new DataContext())
//				{
//					var tip = db.brave_tip.FirstOrDefault(x => x.gmail_email.Trim().ToLower().Equals(email.Trim().ToLower()));
//					if (tip != null)
//					{
//						if(isLive)
//							tip.brave_tip_status_id = 20;
//						else
//							tip.brave_tip_status_id = 4;

//						tip.note = $"- Checked live {DateTime.Now.ToString()}";
//						tip.last_update_date = DateTime.Now;
//						db.Entry(tip).State = EntityState.Modified;
//						db.SaveChanges();

//						result.IsSuccess = true;
//						result.Message = $"Update note {email} sucessfully.";
//					}
//					else
//					{
//						result.IsSuccess = false;
//						result.Message = $"Không tìm thấy kênh email= {email}";
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				result.IsSuccess = false;
//				result.Message = $"Update fail: {ex.Message}";

//				Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//				Loggers.BraveAPILog.Info("		email:	" + email);
//				Loggers.BraveAPILog.Exception("Method: UpdateLivePub", ex);
//				Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
//			}

//			return result;
//		}
//		#endregion
//	}
//}