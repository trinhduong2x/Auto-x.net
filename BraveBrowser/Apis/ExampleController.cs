using BraveBrowser.ApiModels;
using BraveBrowser.Logs;
using BraveBrowser.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http;

namespace BraveBrowser.Apis
{
	[RoutePrefix("api/Example")]
	public class ExampleController : ApiController
	{
		[HttpGet]
		[Route("get-data")]
		public ApiResponse GetDatas()
		{
			var result = new ApiResponse();
			try
			{
				user user1;

                using (var db = new DataContext())
				{
                    user1 = db.users.FirstOrDefault();
				}	

				result.IsSuccess = true;
				result.Data = user1;
			}
			catch (Exception ex)
			{
				result.IsSuccess = false;
				result.Message = ex.Message;
			}

			return result;
		}

        [HttpGet]
        [Route("get-store")]
        public ApiResponse GetRandomUrlV2()
        {
            try
            {
                using (var db = new DataContext())
                {
                    var tips = db.GetRandomTipUrlsV2();
                    return new ApiResponse() { Data = tips };
                }
            }
            catch (Exception ex)
            {
                Loggers.BraveAPILog.InfoBegin(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
                Loggers.BraveAPILog.Exception("GetTipUrl: GetRandomUrlV2", ex);
                Loggers.BraveAPILog.InfoEnd(System.Reflection.MethodBase.GetCurrentMethod().Name, System.Reflection.MethodInfo.GetCurrentMethod());
                return null;
            }
        }

    }
}