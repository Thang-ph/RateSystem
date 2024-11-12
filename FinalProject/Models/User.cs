using System;
using System.Collections.Generic;

namespace FinalProject.Models
{
    public partial class User
    {
        public User()
        {
            Reviews = new HashSet<Review>();
        }

        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public int? RoleId { get; set; }

        public virtual Role? Role { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
