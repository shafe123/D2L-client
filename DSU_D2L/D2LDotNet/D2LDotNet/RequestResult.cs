namespace D2LDotNet
{
    public enum RequestResult
    {
        BadRequest,
        NotFound,
        InternalServerError,
        InvalidSignature,
        InvalidTimestamp,
        PermissionDenied,
        Unknown
    }
}
