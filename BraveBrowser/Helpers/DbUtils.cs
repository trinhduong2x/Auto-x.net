using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BraveBrowser.Helpers
{
	public class DbUtils
	{
		public static string GenerateCommandText(string storedProcedure, SqlParameter[] parameters)
		{
			var commandText = "EXEC {0} {1}";
			var parameterNames = new string[parameters.Length];
			for (int i = 0; i < parameters.Length; i++)
			{
				parameterNames[i] = parameters[i].ParameterName;
			}

			return string.Format(commandText, storedProcedure, string.Join(",", parameterNames));
		}
	}
}