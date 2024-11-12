using System;
using System.Collections.Generic;

namespace ProjectAPI.Models
{
    public partial class CommentRate
    {
        public int? CommentId { get; set; }
        public int? UserId { get; set; }
        public int? Rate { get; set; }
        public int CommentRateId { get; set; }

        public virtual Comment? Comment { get; set; }
        public virtual User? User { get; set; }
    }
}
