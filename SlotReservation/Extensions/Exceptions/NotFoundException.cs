using System.Net;

public class NotFoundException : BaseHttpException
{
    public NotFoundException(string message) : base(message, HttpStatusCode.NotFound)
    {
    }
}
