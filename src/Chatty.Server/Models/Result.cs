namespace Chatty.Server.Models;

public class Result<T>
{
    public Result()
    {
        this.Errors = new List<string>();
    }

    public T Data { get; set; }

    public List<string> Errors { get; init; }

    public bool Failure => this.Errors.Any();
}
