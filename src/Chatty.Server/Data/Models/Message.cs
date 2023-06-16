using System.ComponentModel.DataAnnotations;

using Chatty.Server.Data.Models.Base;

using static Chatty.Server.Data.Validation;

namespace Chatty.Server.Data.Models;

public class Message : DeletableEntity
{
    public int Id { get; init; }

    [Required]
    [MaxLength(ChatMessageMaxLength)]
    public string Text { get; set; }

    [Required]
    public string UserId { get; set; }

    public User User { get; set; }

    [Required]
    public string ChatId { get; set; }

    public Chat Chat { get; set; }
}
