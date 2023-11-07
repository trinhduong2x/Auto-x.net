using System.Data.Entity;
using System.Linq;

namespace BraveBrowser.Models
{
    public partial class DataContext : DbContext
    {
    //	public brave_tip_url GetRandomTipUrls()
    //{
    //	var urls = Database.SqlQuery<brave_tip_url>("exec stp_GetRandomTipUrls", new object[] { }).ToList();
    //	return (urls != null && urls.Count > 0) ? urls.First() : null;
    //}

    public user GetRandomTipUrlsV2()
    {
        var urls = Database.SqlQuery<user>("exec stp_AdminGetTipList", new object[] { }).ToList();
        return (urls != null && urls.Count > 0) ? urls.First() : null;
    }

    //public brave_tip_url GetRandomTipUrlsV3(string exclusionChannelList)
    //{
    //	var exclusionChannelListParam = new SqlParameter() { ParameterName = "@ExclusionChannelList", Direction = ParameterDirection.Input, SqlDbType = SqlDbType.NVarChar, Value = exclusionChannelList };
    //	var urls = Database.SqlQuery<brave_tip_url>("exec stp_GetRandomTipUrlsV3 @ExclusionChannelList", exclusionChannelListParam).ToList();
    //	return (urls != null && urls.Count > 0) ? urls.First() : null;
    //}

    //public brave_tip_url GetRandomTipUrlsV4(string exclusionChannelList, string tipIp)
    //{
    //	var parameters = new object[]
    //	{
    //		new SqlParameter()  {  ParameterName = "@ExclusionChannelList", Direction = ParameterDirection.Input, SqlDbType = SqlDbType.NVarChar, Value = exclusionChannelList  },
    //		new SqlParameter()  {  ParameterName = "@TipIP", Direction = ParameterDirection.Input, SqlDbType = SqlDbType.NVarChar, Value = tipIp  }
    //	};

    //	var urls = Database.SqlQuery<brave_tip_url>("exec stp_GetRandomTipUrlsV4 @ExclusionChannelList, @TipIP", parameters).ToList();
    //	return (urls != null && urls.Count > 0) ? urls.First() : null;
    //}

    //public brave_tip_url GetRedirectUrl()
    //{
    //	var urls = Database.SqlQuery<brave_tip_url>("exec stp_GetRedirectUrl", new object[] { }).ToList();
    //	return (urls != null && urls.Count > 0) ? urls.First() : null;
    //}

    //public brave_tip_url GetRandomFollowChannel(long redditAccountId)
    //{
    //	var parameters = new object[]
    //	{
    //		new SqlParameter() {ParameterName = "@RedditAccountId", Direction = ParameterDirection.Input, SqlDbType = SqlDbType.BigInt, Value = redditAccountId}
    //	};

    //	var urls = Database.SqlQuery<brave_tip_url>("exec stp_Reddit_GetFollowChannel @RedditAccountId", parameters).ToList();
    //	return (urls != null && urls.Count > 0) ? urls.First() : null;
    //}

    //public brave_tip GetInfoFixDieWallet(string braveTipType)
    //{
    //	var parameters = new object[]
    //	{
    //		new SqlParameter() {ParameterName = "@Type", Direction = ParameterDirection.Input, SqlDbType = SqlDbType.NVarChar, Value = braveTipType}
    //	};

    //	var tip = Database.SqlQuery<brave_tip>("exec stp_GetInfoFixDieWallet @Type", parameters).ToList();
    //	return (tip != null && tip.Count > 0) ? tip.First() : null;
    //}

    //public brave_tip_url GetCommentLikeUrl(long redditAccountId)
    //{
    //	var parameters = new object[]
    //	{
    //		new SqlParameter() {ParameterName = "@RedditAccountId", Direction = ParameterDirection.Input, SqlDbType = SqlDbType.BigInt, Value = redditAccountId}
    //	};

    //	var urls = Database.SqlQuery<brave_tip_url>("exec stp_Reddit_GetCommentLikeUrl @RedditAccountId", parameters).ToList();
    //	return (urls != null && urls.Count > 0) ? urls.First() : null;
    //}

    //public int GetNumberOfFollow(long redditAccountId)
    //{
    //	var redditAccountIdParam = new SqlParameter() { ParameterName = "@RedditAccountId", Direction = ParameterDirection.Input, SqlDbType = SqlDbType.BigInt, Value = redditAccountId };
    //	var numberOfFollowParam = new SqlParameter() { ParameterName = "@NumberOfFollow", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int };

    //	Database.ExecuteSqlCommand("exec stp_Reddit_GetNumberOfFollow @RedditAccountId, @NumberOfFollow OUTPUT", redditAccountIdParam, numberOfFollowParam);
    //	return Convert.ToInt32(numberOfFollowParam.Value);
    //}

    //public brave_tip_url GetSequenceTipUrls()
    //{
    //	var urls = Database.SqlQuery<brave_tip_url>("exec stp_GetSequenceTipUrls", new object[] { }).ToList();
    //	return (urls != null && urls.Count > 0) ? urls.First() : null;
    //}

    //#region BraveTip

    //public List<admin_get_tip_list> AdminGetTipList(int month, int year, int tipStatus, bool isNeedFeed)
    //{
    //	try
    //	{
    //		var feedLastDay = Convert.ToInt32(SettingUtils.GetValue("feed_last_day"));
    //		var parameters = new object[]
    //		{
    //		new SqlParameter() {ParameterName = "@FeedLastDay", Direction = ParameterDirection.Input, SqlDbType = SqlDbType.Int, Value = feedLastDay},
    //		new SqlParameter() {ParameterName = "@IsNeedFeed", Direction = ParameterDirection.Input, SqlDbType = SqlDbType.Bit, Value = isNeedFeed},
    //		new SqlParameter() {ParameterName = "@TipStatus", Direction = ParameterDirection.Input, SqlDbType = SqlDbType.Int, Value = tipStatus},
    //		new SqlParameter() {ParameterName = "@Month", Direction = ParameterDirection.Input, SqlDbType = SqlDbType.Int, Value = month},
    //		new SqlParameter() {ParameterName = "@Year", Direction = ParameterDirection.Input, SqlDbType = SqlDbType.Int, Value = year}
    //		};

    //		return Database.SqlQuery<admin_get_tip_list>("exec stp_AdminGetTipList @FeedLastDay,@IsNeedFeed,@TipStatus,@Month,@Year", parameters)?.ToList();
    //	}
    //	catch (Exception ex)
    //	{
    //		Loggers.BraveMVCLog.Exception("Method: AdminGetTipList", ex);
    //	}

    //	return null;
    //}

    //public List<admin_get_tip_url_list> AdminGetTipUrlList(long braveTipId, int month, int year)
    //{
    //	var parameters = new object[]
    //	{
    //		new SqlParameter() {ParameterName = "@BraveTipId", Direction = ParameterDirection.Input, SqlDbType = SqlDbType.BigInt, Value = braveTipId},
    //		new SqlParameter() {ParameterName = "@Month", Direction = ParameterDirection.Input, SqlDbType = SqlDbType.Int, Value = month},
    //		new SqlParameter() {ParameterName = "@Year", Direction = ParameterDirection.Input, SqlDbType = SqlDbType.Int, Value = year}
    //	};

    //	return Database.SqlQuery<admin_get_tip_url_list>("exec stp_AdminGetTipUrlList @BraveTipId,@Month,@Year", parameters).ToList();
    //}

    //#endregion BraveTip

    //#region Hive account

    //public List<admin_get_hive_list> HiveAccountList(bool isNeedFeed)
    //{
    //	var feedLastDay = Convert.ToInt32(SettingUtils.GetValue("hive_feed_last_day"));
    //	var parameters = new object[]
    //	{
    //		new SqlParameter() {ParameterName = "@FeedLastDay", Direction = ParameterDirection.Input, SqlDbType = SqlDbType.Int, Value = feedLastDay},
    //		new SqlParameter() {ParameterName = "@IsNeedFeed", Direction = ParameterDirection.Input, SqlDbType = SqlDbType.Bit, Value = isNeedFeed},
    //	};

    //	return Database.SqlQuery<admin_get_hive_list>("exec stp_HiveAccountList @FeedLastDay,@IsNeedFeed", parameters).ToList();
    //}

    //#endregion Hive account

    //#region brave_tip_follow

    //public brave_tip_url BraveTipFollow_GetFollowChannel(long accountId)
    //{
    //	var accountParameter = new SqlParameter() { ParameterName = "@AccountId", Direction = ParameterDirection.Input, SqlDbType = SqlDbType.BigInt, Value = accountId };
    //	var urls = Database.SqlQuery<brave_tip_url>("exec stp_BraveTipFollow_GetFollowChannel @AccountId", accountParameter).ToList();
    //	return (urls != null && urls.Count > 0) ? urls.First() : null;
    //}

    //#endregion brave_tip_follow

    //public List<admin_brave_tip_summary> GetTipItemSummary(string vmwareType)
    //{
    //	var parameters = new object[]
    //	{
    //		new SqlParameter() {ParameterName = "@VmwareType", Direction = ParameterDirection.Input, SqlDbType = SqlDbType.NVarChar, Value = vmwareType}
    //	};

    //	return Database.SqlQuery<admin_brave_tip_summary>("exec stp_GetTipItemSummary @VmwareType", parameters).ToList();
    //}

    //public void SetRandomBatTip(int batFrom, int batTo)
    //{
    //	var batFromParam = new SqlParameter() { ParameterName = "@BatFrom", Direction = ParameterDirection.Input, SqlDbType = SqlDbType.Int, Value = batFrom };
    //	var batToParam = new SqlParameter() { ParameterName = "@BatTo", Direction = ParameterDirection.Input, SqlDbType = SqlDbType.Int, Value = batTo };

    //	Database.ExecuteSqlCommand("exec stp_SetRandomBatTip @BatFrom, @BatTo", batFromParam, batToParam);
    //}

    //public void SetRandomBatTipSmaill(int batFrom, int batTo)
    //{
    //	var batFromParam = new SqlParameter() { ParameterName = "@BatFrom", Direction = ParameterDirection.Input, SqlDbType = SqlDbType.Int, Value = batFrom };
    //	var batToParam = new SqlParameter() { ParameterName = "@BatTo", Direction = ParameterDirection.Input, SqlDbType = SqlDbType.Int, Value = batTo };

    //	Database.ExecuteSqlCommand("exec stp_SetRandomBatTipSmall @BatFrom, @BatTo", batFromParam, batToParam);
    //}

    //#region Twitch_account

    //public brave_tip_url Twitch_GetFollowChannel(long twitchAccountId)
    //{
    //	var parameters = new object[]
    //	{
    //		new SqlParameter() {ParameterName = "@TwitchAccountId", Direction = ParameterDirection.Input, SqlDbType = SqlDbType.BigInt, Value = twitchAccountId}
    //	};

    //	var urls = Database.SqlQuery<brave_tip_url>("exec stp_Twitch_GetFollowChannel @TwitchAccountId", parameters).ToList();
    //	return (urls != null && urls.Count > 0) ? urls.First() : null;
    //}

    //#endregion Twitch_account

    //#region Live account

    //public brave_tip CheckLivePub()
    //{
    //	var tip = Database.SqlQuery<brave_tip>("exec stp_CheckLivePub", new object[] { }).ToList();
    //	return (tip != null && tip.Count > 0) ? tip.First() : null;
    //}

    //#endregion Live account

    //#region TOn

    //public ton_account Ton_GetFeedChannel(long tonAccountId)
    //{
    //	var parameters = new object[]
    //	{
    //		new SqlParameter() {ParameterName = "@TonAccountId", Direction = ParameterDirection.Input, SqlDbType = SqlDbType.BigInt, Value = tonAccountId},
    //		new SqlParameter() {ParameterName = "@ActionName", Direction = ParameterDirection.Input, SqlDbType = SqlDbType.NVarChar, Value = TonActionTypes.NewsFeed},
    //	};

    //	var urls = Database.SqlQuery<ton_account>("exec stp_Ton_GetFeedChannel @TonAccountId, @ActionName", parameters).ToList();
    //	return (urls != null && urls.Count > 0) ? urls.First() : null;
    //}

    //public ton_account Ton_GetFollowChannel(long tonAccountId)
    //{
    //	var parameters = new object[]
    //	{
    //		new SqlParameter() {ParameterName = "@TonAccountId", Direction = ParameterDirection.Input, SqlDbType = SqlDbType.BigInt, Value = tonAccountId},
    //		new SqlParameter() {ParameterName = "@ActionName", Direction = ParameterDirection.Input, SqlDbType = SqlDbType.NVarChar, Value = TonActionTypes.Post}, // follow những kênh đã có bài post
    //	};

    //	var urls = Database.SqlQuery<ton_account>("exec stp_Ton_GetFollowChannel @TonAccountId, @ActionName", parameters).ToList();
    //	return (urls != null && urls.Count > 0) ? urls.First() : null;
    //}

    //public ton_account Ton_GetActionChannel(long tonAccountId, string actionName)
    //{
    //	var parameters = new object[]
    //	{
    //		new SqlParameter() {ParameterName = "@TonAccountId", Direction = ParameterDirection.Input, SqlDbType = SqlDbType.BigInt, Value = tonAccountId},
    //		new SqlParameter() {ParameterName = "@ActionName", Direction = ParameterDirection.Input, SqlDbType = SqlDbType.NVarChar, Value = actionName},
    //	};

    //	var urls = Database.SqlQuery<ton_account>("exec stp_Ton_GetActionChannel @TonAccountId, @ActionName", parameters).ToList();
    //	return (urls != null && urls.Count > 0) ? urls.First() : null;
    //}

    //public int Ton_GetActionCount(long tonAccountId, string actionName)
    //{
    //	var tonAccountIddParam = new SqlParameter() { ParameterName = "@TonAccountId", Direction = ParameterDirection.Input, SqlDbType = SqlDbType.Int, Value = tonAccountId };
    //	var actionParam = new SqlParameter() { ParameterName = "@ActionName", Direction = ParameterDirection.Input, SqlDbType = SqlDbType.NVarChar, Value = actionName };
    //	var numberOfActionParam = new SqlParameter() { ParameterName = "@NumberOfFollow", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int };

    //	Database.ExecuteSqlCommand("exec stp_Ton_GetActionCount @TonAccountId, @ActionName, @NumberOfFollow OUTPUT", tonAccountIddParam, actionParam, numberOfActionParam);
    //	return Convert.ToInt32(numberOfActionParam.Value);
    //}

    //public int Ton_GetTotalActionCount(long tonAccountId, string actionName)
    //{
    //	var tonAccountIddParam = new SqlParameter() { ParameterName = "@TonAccountId", Direction = ParameterDirection.Input, SqlDbType = SqlDbType.Int, Value = tonAccountId };
    //	var actionParam = new SqlParameter() { ParameterName = "@ActionName", Direction = ParameterDirection.Input, SqlDbType = SqlDbType.NVarChar, Value = actionName };
    //	var numberOfActionParam = new SqlParameter() { ParameterName = "@NumberOfFollow", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int };

    //	Database.ExecuteSqlCommand("exec stp_Ton_GetTotalActionCount @TonAccountId, @ActionName, @NumberOfFollow OUTPUT", tonAccountIddParam, actionParam, numberOfActionParam);
    //	return Convert.ToInt32(numberOfActionParam.Value);
    //}

    //public ton_account Ton_GetFollowingChannel(long tonAccountId)
    //{
    //	var parameters = new object[]
    //	{
    //		new SqlParameter() {ParameterName = "@TonAccountId", Direction = ParameterDirection.Input, SqlDbType = SqlDbType.BigInt, Value = tonAccountId}
    //	};

    //	var urls = Database.SqlQuery<ton_account>("exec stp_Ton_GetFollowingChannel @TonAccountId", parameters).ToList();
    //	return (urls != null && urls.Count > 0) ? urls.First() : null;
    //}

    //#endregion TOn

    //#region Shortlink

    //public shortlink_url GetShortlinkRandomUrl(long shortlinkViewerId, string site)
    //{
    //	var parameters = new object[]
    //	{
    //		new SqlParameter() {ParameterName = "@ShortlinkViewerId", Direction = ParameterDirection.Input, SqlDbType = SqlDbType.BigInt, Value = shortlinkViewerId },
    //		new SqlParameter() {ParameterName = "@Site", Direction = ParameterDirection.Input, SqlDbType = SqlDbType.NVarChar, Value = site}
    //	};

    //	var urls = Database.SqlQuery<shortlink_url>("exec stp_up4ever_GetShortlinkRandomUrl @ShortlinkViewerId, @Site", parameters).ToList();
    //	return (urls != null && urls.Count > 0) ? urls.First() : null;
    //}

    //public shortlink_url GetShortlinkUrl(string serviceCode, string viewIP)
    //{
    //	var parameters = new object[]
    //	{
    //		new SqlParameter() {ParameterName = "@ServiceCode", Direction = ParameterDirection.Input, SqlDbType = SqlDbType.NVarChar, Value = serviceCode},
    //		new SqlParameter() {ParameterName = "@ViewIP", Direction = ParameterDirection.Input, SqlDbType = SqlDbType.NVarChar, Value = viewIP},
    //	};

    //	var urls = Database.SqlQuery<shortlink_url>("exec stp_Shortlink_GetUrl @ServiceCode, @ViewIP", parameters).ToList();
    //	return (urls != null && urls.Count > 0) ? urls.First() : null;
    //}

    //#endregion Shortlink
}
}