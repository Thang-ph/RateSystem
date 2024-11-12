using System;
using System.Collections.Generic;

namespace ProjectAPI.Models
{
    public partial class Review
    {
        public int ReviewId { get; set; }
        public int? ProductId { get; set; }
        public int? UserId { get; set; }
        public int? Star { get; set; }
        public DateTime? CreateAt { get; set; }

        public virtual Product? Product { get; set; }
        public virtual User? User { get; set; }
    }
}
