using BraveBrowser.ApiModels;
using BraveBrowser.ApiModels.Reponse;
using BraveBrowser.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web.Http;

namespace BraveBrowser.Apis
{
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        [HttpPost]
        [Route("degister-action")]
        public async Task<ApiResponseV1<List<TimeLineModel>>> DegisterUserActionAsync([FromBody] DegisterUserActionModel request)
        {
            var validation = new DegisterUserActionModelValidation();
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
                        timelines.Add(new TimeLineModel { Time = time.ToString("H:mm"), Action = key});

                        time = time.AddSeconds(periodTime);

                        // Xóa thằng đã hết action
                        if (randomAction[key] == 0)
                            randomAction.Remove(key);

                        loop++;
                        if(loop >= randomAction.Count())
                        {
                            loop = 0;
                        }
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
    }
}