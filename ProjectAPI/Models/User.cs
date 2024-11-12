using System;
using System.Collections.Generic;

namespace ProjectAPI.Models
{
    public partial class User
    {
        public User()
        {
            CommentRates = new HashSet<CommentRate>();
            Comments = new HashSet<Comment>();
            Reviews = new HashSet<Review>();
        }

        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public int? RoleId { get; set; }

        public virtual Role? Role { get; set; }
        public virtual ICollection<CommentRate> CommentRates { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
