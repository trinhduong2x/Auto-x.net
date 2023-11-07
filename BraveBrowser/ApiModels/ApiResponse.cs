namespace BraveBrowser.ApiModels
{
	public class ApiResponse
	{
		public bool IsSuccess { get; set; }

		public string Message { get; set; }

		public string StatusCode { get; set; }

		public object Data { get; set; }
	}

	public class ApiResponseV1<T>
	{
		public bool IsSuccess { get; set; }

		public string Message { get; set; }

		public string StatusCode { get; set; }

		public T Data { get; set; }
	}
}