using BraveBrowser.ApiModels;
using BraveBrowser.ApiModels.Reponse;
using BraveBrowser.Models;
using FluentValidation;
using MoreLinq;
using MoreLinq.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace BraveBrowser.Apis
{
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        [HttpPost]
        [Route("degister-action")]
        public async Task<ApiResponseV1<List<TimeLineModel>>> DegisterUserActionAsync([FromBody] DegisterUserActionRequestModel request)
        {
            var validation = new DegisterUserActionModelRequestValidation();
            var result = new ApiResponseV1<List<TimeLineModel>>();

            var validatorResult = await validation.ValidateAsync(request);
            if (!validatorResult.IsValid)
            {
                result.Message = string.Join($" AND ERROR ", validatorResult.Errors);
                return result;
            }

            try
            {
                #region Lấy ra time line
                var timelines = new List<TimeLineModel>();

                if (request.ActionSettings.Count > 0)
                {
                    // Lấy giá trị random và tổng số lần thực hiện
                    Dictionary<string, int> randomAction = new Dictionary<string, int>();
                    int sumAction = 0;
                    foreach (var action in request.ActionSettings)
                    {
                        int randomActionNumber = new Random().Next(action.AcctionDailyFrom, action.AcctionDailyTo);
                        randomAction.Add(action.ActionName, randomActionNumber);
                        sumAction += randomActionNumber;
                    }

                    // Tính số giây thực hiện
                    int periodTime = 86400 / sumAction;

                    // Random số lần thực hiện
                    var time = DateTime.Now;
                    int loop = 0;

                    while (randomAction.Count > 0)
                    {
                        // Lấy ngẫu nhiên một key và xóa value của nó đi 1
                        string key = randomAction.Keys.ElementAt(new Random().Next(loop, randomAction.Count));

                        randomAction[key] -= 1;

                        // Thêm action vào timeline và tăng time
                        timelines.Add(new TimeLineModel { Time = time.ToString("H:mm"), Action = key });

                        time = time.AddSeconds(periodTime);

                        // Xóa thằng đã hết action
                        if (randomAction[key] == 0)
                            randomAction.Remove(key);

                        loop++;
                        if (loop >= randomAction.Count())
                        {
                            loop = 0;
                        }
                        Thread.Sleep(5);
                    }
                }

                #endregion

                #region xử lý về dữ liệu vào database

                using (var db = new DataContext())
                {

                    using (var transaction = db.Database.BeginTransaction())
                    {
                        // Query and update database
                        var user = db.users.FirstOrDefault(x => x.username == request.UserName);

                        //Nếu tồn tại user name thì add vào bảng user và user_action
                        if (user is null)
                        {
                            user newUser = new user
                            {
                                username = request.UserName,
                                user_group_id = request.UserGroupId,
                                ip = request.Ip,
                                date_created = DateTime.Now,

                            };

                            db.users.Add(newUser);
                            db.SaveChanges();

                            var userNew = db.users.FirstOrDefault(x => x.username == request.UserName).user_id;
                            var userActions = new List<user_action>();

                            request.ActionSettings.ForEach(
                                (actionSetting) => userActions.Add(new user_action
                                {
                                    user_id = userNew,
                                    action_name = actionSetting.ActionName,
                                    action_code = actionSetting.ActionName.ToLower(),
                                    action_daily_setting = "a",
                                    action_daily_from = actionSetting.AcctionDailyFrom,
                                    action_daily_to = actionSetting.AcctionDailyTo,
                                    action_number = 0,
                                    created_date = DateTime.Now,
                                    is_active = true,
                                }));
                            db.user_action.AddRange(userActions);
                            db.SaveChanges();
                        }
                        else //Nếu tồn tại thì update vào bảng user
                        {
                            user.user_group_id = request.UserGroupId;
                            user.ip = request.Ip;
                            user.date_created = DateTime.Now;
                            db.Entry(user).State = EntityState.Modified;
                            db.SaveChanges();

                            var userNew = db.users.FirstOrDefault(x => x.username == request.UserName);
                        }
                        transaction.Commit();
                    }
                }

                #endregion

                result.IsSuccess = true;
                result.Data = timelines;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
            }

            return result;
        }

        [HttpGet]
        [Route("data-action")]
        public ApiResponseV1<DataActionModel> GetDataAction([Required] string userName, [Required] string action)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(action))
            {
                return new ApiResponseV1<DataActionModel>()
                {
                    Data = null,
                    IsSuccess = false,
                    Message = "username or action is null or empty"
                };
            }
            var result = new ApiResponseV1<DataActionModel>();

            try
            {
                using (var db = new DataContext())
                {

                    var user = db.users.FirstOrDefault(x => x.username == userName);

                    // Check exists của username input
                    if (user is null)
                    {
                        return new ApiResponseV1<DataActionModel>()
                        {
                            Data = null,
                            IsSuccess = false,
                            Message = "username not exist"
                        };
                    }

                    else //Nếu tồn tại thì update vào bảng user
                    {

                        var userNames = db.users.
                            Where(x => x.username != userName && x.is_delete == false)
                            .Join(db.user_action.
                            Where(x => x.action_code == action && x.is_active == true),
                            users => users.user_id,
                            userAction => userAction.user_id, (users, userAction) => new
                            {
                                userName = users.username
                            }).ToList();

                        if (userNames.Count > 0)
                        {
                            var random = new Random().Next(0, userNames.Count());
                            string s = userNames[random].userName;
                            var resultModel = new DataActionModel
                            {
                                UserName = userNames[new Random().Next(0, userNames.Count())].userName,
                                Data = "hard code"

                            };
                            result.Data = resultModel;
                            result.IsSuccess = true;
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
            }

            return result;
        }

        [HttpPut]
        [Route("action-interact")]
        public async Task<ApiResponseV1<bool>> UpdateActionInteractAsync(UpdateActionInteractRequestModel request)
        {
            var validation = new UpdateActionInteractRequestModelValidation();
            var result = new ApiResponseV1<bool>();

            var validatorResult = await validation.ValidateAsync(request);
            if (!validatorResult.IsValid)
            {
                result.Message = string.Join($" AND ERROR ", validatorResult.Errors);
                return result;
            }

            try
            {
                using (var db = new DataContext())
                {

                    var userNames = db.users.
                        Where(x => (x.username == request.UserNameFrom || x.username == request.UserNameTo) && x.is_delete == false)
                        .Join(db.user_action.Where(x => x.action_code == request.Action && x.is_active == true),
                              users => users.user_id,
                              userAction => userAction.user_id, (users, userAction) => new
                              {
                                  userName = users.username,
                                  userId = userAction.user_id,
                                  userActionId = userAction.user_action_id
                              }).ToList();

                    // Check exists của request
                    if (userNames.Count != 2)
                    {
                        return new ApiResponseV1<bool>()
                        {
                            Data = false,
                            IsSuccess = false,
                            Message = "username or action not exist"
                        };
                    }
                    else //Nếu tồn tại thì update vào bảng user
                    {

                        using (var transaction = db.Database.BeginTransaction())
                        {
                            // Update into table user_action
                            var userActionIdFrom = userNames.FirstOrDefault(x => x.userName == request.UserNameFrom);

                            var userActionFrom = db.user_action.FirstOrDefault(x => x.user_action_id == userActionIdFrom.userActionId);

                            userActionFrom.action_number += 1;
                            db.Entry(userActionFrom).State = EntityState.Modified;
                            db.SaveChanges();

                            var userActionIdTo = userNames.FirstOrDefault(x => x.userName == request.UserNameTo);
                            var userActionTo = db.user_action.FirstOrDefault(x => x.user_action_id == userActionIdTo.userActionId);

                            userActionTo.action_number -= 1;
                            db.Entry(userActionTo).State = EntityState.Modified;
                            db.SaveChanges();

                            // Insert into table user_action_log
                            var userActionLog = new user_action_log();
                            userActionLog.from_user_id = userActionIdFrom.userId;
                            userActionLog.to_user_id = userActionTo.user_id;
                            userActionLog.to_link = request.Link;
                            userActionLog.to_action = request.Action;
                            userActionLog.date_created = DateTime.Now;

                            db.user_action_log.Add(userActionLog);
                            db.SaveChanges();

                            transaction.Commit();
                        }

                    }
                    result.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
            }

            return result;
        }
    }
}