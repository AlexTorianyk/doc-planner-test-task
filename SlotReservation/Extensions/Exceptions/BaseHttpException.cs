using System.Net;

public class BaseHttpException : Exception
{
  public HttpStatusCode StatusCode { get; set; }
  
  public BaseHttpException(string message, HttpStatusCode statusCode) : base(message)
  {
    StatusCode = statusCode;
  }
}
