namespace Chatty.Server.Models.Core;

public class ChatCoreModel
{
    public string Username { get; init; }

    public string ProfileImageUrl { get; init; }

    public override bool Equals(object obj)
    {
        if (obj == null || obj is not ChatCoreModel)
        {
            return false;
        }

        return this.Username == ((ChatCoreModel)obj).Username;
    }

    public override int GetHashCode()
        => this.Username.GetHashCode();
}
