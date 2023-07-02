using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Chatty.Server.Data.Models.Base;

using static Chatty.Server.Data.Validation;

namespace Chatty.Server.Data.Models;

public class Message : DeletableEntity
{
    public int Id { get; init; }

    [Required]
    [MaxLength(ChatMessageMaxLength)]
    public string Text { get; set; }

    public string SenderUserId { get; set; }

    [ForeignKey("SenderUserId")]
    public User SenderUser { get; set; }

    [Required]
    public string ReceiverUserId { get; set; }

    [ForeignKey("ReceiverUserId")]
    public User ReceiverUser { get; set; }
}
