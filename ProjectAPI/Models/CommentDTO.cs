namespace ProjectAPI.Models
{
    public class CommentDTO
    {
        public int CommentId { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public string? ContentComment { get; set; }
        public DateTime? CreateAt { get; set; }
        public int? FatherId { get; set; }
        public virtual UserDTO User { get; set; } = null!;
    }
}
