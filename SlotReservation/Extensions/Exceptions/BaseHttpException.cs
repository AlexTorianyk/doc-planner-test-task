using System.Net;

// Decision is explained in the README.md file
public class BaseHttpException : Exception
{
    public HttpStatusCode StatusCode { get; set; }

    public BaseHttpException(string message, HttpStatusCode statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
}
