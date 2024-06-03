using System.Net;

public class BadRequestException : BaseHttpException
{
    public BadRequestException(string message) : base(message, HttpStatusCode.BadRequest)
    {
    }
}
