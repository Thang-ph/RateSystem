using System;
using System.Collections.Generic;

namespace ProjectAPI.Models
{
    public partial class Comment
    {
        public Comment()
        {
            CommentRates = new HashSet<CommentRate>();
            InverseFather = new HashSet<Comment>();
        }

        public int CommentId { get; set; }
        public int? ProductId { get; set; }
        public int? UserId { get; set; }
        public string? ContentComment { get; set; }
        public DateTime? CreateAt { get; set; }
        public int? FatherId { get; set; }

        public virtual Comment? Father { get; set; }
        public virtual Product? Product { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<CommentRate> CommentRates { get; set; }
        public virtual ICollection<Comment> InverseFather { get; set; }
    }
}
