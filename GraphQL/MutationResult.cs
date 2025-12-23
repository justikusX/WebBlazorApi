namespace WebBlazorApi.GraphQL;

public sealed record MutationResult<T>(bool Ok, string? Error, T? Data)
{
    public static MutationResult<T> Success(T data) => new(true, null, data);
    public static MutationResult<T> Fail(string error) => new(false, error, default);
}
